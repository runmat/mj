Imports KBS.KBS_BASE
Imports Telerik.Web.UI

Partial Public Class Change01
    Inherits Page

    Private mObjKasse As Kasse
    Private mObjBestellung As Bestellung

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        FormAuth(Me)
        lblError.Text = ""
        Title = lblHead.Text

        If mObjKasse Is Nothing Then
            If Not Session("mKasse") Is Nothing Then
                mObjKasse = CType(Session("mKasse"), Kasse)
            Else
                Throw New Exception("benötigtes Session Objekt nicht vorhanden")
            End If
        End If

        If mObjBestellung Is Nothing Then
            If Session("objBestellung") IsNot Nothing Then
                mObjBestellung = CType(Session("objBestellung"), Bestellung)
            Else
                mObjBestellung = mObjKasse.Bestellung(Me)
                Session("objBestellung") = mObjBestellung
            End If
        End If

        'Funktioniert nicht immer, focus wird nicht erkannt in diesem Feld, Verschluckt sich beim asynchr. postback
        'nicht analysierbar. JJU20090429
        Form.DefaultButton = defaultButtonInsert.UniqueID

        If Not IsPostBack Then
            'Beim ersten Aufruf der Seite prüfen, ob gespeicherte Bestellungen in der DB
            mObjBestellung.CheckForSavedBestellungen()
            SetFocus(txtEAN)
            FillGrid()
        End If

    End Sub

    Private Sub FillGrid()
        Dim tmpDataView As New DataView(mObjBestellung.Bestellungen)

        If tmpDataView.Count = 0 Then
            SearchMode()
        Else
            SearchMode(False)

            rgGrid1.Rebind()
            'Setzen der DataSource geschieht durch das NeedDataSource-Event
        End If
    End Sub

    Private Sub SearchMode(Optional ByVal search As Boolean = True)
        rgGrid1.Visible = Not search
        lblNoData.Visible = search
    End Sub

    Protected Sub rgGrid1_NeedDataSource(ByVal sender As Object, ByVal e As GridNeedDataSourceEventArgs) Handles rgGrid1.NeedDataSource
        If mObjBestellung.Bestellungen IsNot Nothing Then
            rgGrid1.DataSource = mObjBestellung.Bestellungen.DefaultView
        Else
            rgGrid1.DataSource = Nothing
        End If
    End Sub

    Private Sub FillGrid2()

        Dim tmpDataView As New DataView(mObjBestellung.Bestellungen)

        If tmpDataView.Count = 0 Then
            GridView2.Visible = False
            lblNoData.Visible = True
        Else
            GridView2.Visible = True
            lblNoData.Visible = False

            GridView2.DataSource = tmpDataView
            GridView2.DataBind()

            For Each row As GridViewRow In GridView2.Rows
                If CInt(mObjBestellung.Bestellungen.Select("EAN11='" & row.Cells(0).Text & "'")(0)("BSTMG")) > 99 Then
                    row.BackColor = Drawing.Color.Red
                End If
            Next
        End If
    End Sub

    Private Sub responseBack()
        Response.Redirect("../Selection.aspx")
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

    Private Sub doinsert()
        '----------------------------------------------------------------------
        'Methode:      doinsert
        'Autor:         Julian Jung
        'Beschreibung:  füg einen artikel in die aktuelle bestellung ein
        'Erstellt am:   05.05.2009
        '----------------------------------------------------------------------

        If txtMenge.Text.Trim(" "c).Length > 0 Then
            If Not txtMaterialnummer.Text = "" Then
                'wenn dies gefüllt, dann Artikel korrekt
                If mObjBestellung.Bestellungen.Select("EAN11='" & txtEAN.Text & "'").Count = 0 Then
                    If mObjBestellung.addArtikel(txtEAN.Text, CInt(txtMenge.Text)) Then
                        Session("objBestellung") = mObjBestellung
                        txtEAN.Text = ""
                        txtMaterialnummer.Text = ""
                        txtMenge.Text = ""
                        lblArtikelbezeichnung.Text = "(wird automatisch ausgefüllt)"
                        lbtnInsert.Visible = False
                        FillGrid()
                    Else
                        lblError.Text = "Hinzufügen nicht möglich: " & mObjBestellung.E_MESSAGE
                    End If
                Else
                    lblError.Text = "Artikel ist in der aktuellen Bestellung schon enthalten"
                End If
                SetFocus(txtEAN)
            End If
        Else
            lblError.Text = "Bitte geben sie eine Menge ein"
        End If
    End Sub

    Protected Sub rgGrid1_ItemCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs) Handles rgGrid1.ItemCommand
        If TypeOf e.Item Is GridDataItem Then
            Dim gridRow As GridDataItem = CType(e.Item, GridDataItem)
            Dim dRow As DataRow = mObjBestellung.Bestellungen.Select("EAN11='" & gridRow("EAN11").Text & "'")(0)

            If e.CommandName = "entfernen" Then
                dRow.Delete()
            ElseIf e.CommandName = "plusMenge" Then
                mObjBestellung.Bestellungen.Select("EAN11='" & dRow("EAN11") & "'")(0)("BSTMG") = CInt(dRow("BSTMG")) + 1
            ElseIf e.CommandName = "minusMenge" Then
                mObjBestellung.Bestellungen.Select("EAN11='" & dRow("EAN11") & "'")(0)("BSTMG") = Math.Max(0, CInt(dRow("BSTMG")) - 1)
            End If

            Session("objBestellung") = mObjBestellung
            FillGrid()

        End If
    End Sub

    Protected Sub rgGrid1_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles rgGrid1.ItemDataBound
        If TypeOf e.Item Is GridGroupHeaderItem Then
            Dim gruppenHeader As GridGroupHeaderItem = CType(e.Item, GridGroupHeaderItem)
            Dim dataRow As DataRowView = CType(e.Item.DataItem, DataRowView)
            gruppenHeader.DataCell.Text = "Lieferant: " & dataRow("NAME1")
        ElseIf TypeOf e.Item Is GridGroupFooterItem Then
            Dim gruppenFooter As GridGroupFooterItem = CType(e.Item, GridGroupFooterItem)
            Dim dataRow As DataRowView = CType(e.Item.DataItem, DataRowView)
            gruppenFooter("EAN11").Text = "Summe für Lieferant"
            gruppenFooter("MAKTX").Text = dataRow("NAME1") & ":"
        End If
    End Sub

    Protected Sub rgGrid1_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles rgGrid1.DataBound
        checkBestellung()
    End Sub

    Public Sub txtMenge_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        If IsNumeric(CType(sender, TextBox).Text) Then
            mObjBestellung.Bestellungen.Select("EAN11='" & CType(CType(sender, TextBox).Parent.Parent, GridDataItem)("EAN11").Text & "'")(0)("BSTMG") = CType(sender, TextBox).Text
            Session("objBestellung") = mObjBestellung
            FillGrid()
        End If
    End Sub

    Private Sub txtEAN_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtEAN.TextChanged
        If Not mObjBestellung.getArtikelInfo(txtEAN.Text, False, txtMaterialnummer.Text, lblArtikelbezeichnung.Text) Then
            If mObjBestellung.E_MESSAGE.Length > 0 Then
                lblError.Text = mObjBestellung.E_MESSAGE
            Else
                lblError.Text = "Artikel nicht vorhanden"
            End If

            lblArtikelbezeichnung.Text = "(wird automatisch ausgefüllt)"
            txtMaterialnummer.Text = ""
            lbtnInsert.Visible = False
        Else
            lbtnInsert.Visible = True
            SetFocus(txtMenge)
        End If
    End Sub

    Private Sub lbBestellungOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBestellungOk.Click
        mpeBestellungsCheck.Hide()
        doSubmit()
    End Sub

    Private Sub doSubmit()
        '----------------------------------------------------------------------
        'Methode:      doSubmit
        'Autor:         Julian Jung
        'Beschreibung:  je nach SAP status wird hier eine Meldung ausgegeben, im Fehlerfalle bleibt das OBJ bestehen, sonst kill
        'Erstellt am:   05.05.2009
        '----------------------------------------------------------------------

        mObjBestellung.sendOrderToSAPERP()
        Session("objBestellung") = mObjBestellung
        Select Case mObjBestellung.SAPStatus
            Case 0
                'ok
                lblBestellMeldung.ForeColor = Drawing.Color.Green
                lblBestellMeldung.Text = "Ihre Bestellung war erfolgreich."
                MPEBestellResultat.Show()
                mObjBestellung.endOrder()
                mObjBestellung = Nothing
            Case -1
                'teils ok
                lblBestellMeldung.ForeColor = Drawing.Color.Red
                lblBestellMeldung.Text = "Ihre Bestellung konnte nicht vollständig durchgeführt werden: <br><br> " & mObjBestellung.SAPStatusText & _
                "<br><br> Fehlerhafte Artikel bleiben bestehen."
                MPEBestellResultat.Show()
            Case Else
                lblBestellMeldung.ForeColor = Drawing.Color.Red
                lblBestellMeldung.Text = "Ihre Bestellung ist fehlgeschlagen: <br><br> " & mObjBestellung.SAPStatusText
                MPEBestellResultat.Show()
        End Select

    End Sub

    Private Sub lbSpeichern_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbSpeichern.Click
        mObjBestellung.SaveToSQLDB()
    End Sub

    Private Sub lbAbsenden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAbsenden.Click
        '----------------------------------------------------------------------
        'Methode:      lbAbsenden_Click
        'Autor:         Julian Jung
        'Beschreibung:  führt eine Bestellung aus oder zeigt einen Kontrollübersicht an
        'Erstellt am:   05.05.2009
        '----------------------------------------------------------------------

        If mObjBestellung.Bestellungen.Rows.Count = 0 Then
            lblError.Text = "Sie haben keine Artikel für eine Bestellung hinzugefügt"
        Else
            If (checkBestellung(True)) Then
                FillGrid2()
                mpeBestellungsCheck.Show()
            End If
        End If
    End Sub

    Protected Sub lbBestellFinalize_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBestellFinalize.Click
        Response.Redirect("../Selection.aspx")
    End Sub

    Private Sub defaultButtonInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles defaultButtonInsert.Click
        If lbtnInsert.Visible = True Then 'nur wenn der Anschauliche Insert Button auch visible ist.
            doinsert()
        End If
    End Sub

    Protected Sub lbtnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnInsert.Click
        doinsert()
    End Sub

    Private Function checkBestellung(Optional ByVal blnShowErrormessage As Boolean = False) As Boolean
        Dim blnOk As Boolean

        Try
            lblError.Text = ""

            'für alle Posten Bestelleinheiten und Mindestbestellmengen prüfen
            Dim blnMengenOk As Boolean = True
            For Each item As GridDataItem In rgGrid1.Items
                Dim blnItemOK As Boolean = True
                Dim strMessage As String = ""
                Dim imgFehler As Image = CType(item.FindControl("imgFehler"), Image)
                Dim dRow As DataRow = mObjBestellung.Bestellungen.Select("EAN11='" & item("EAN11").Text & "'")(0)
                If dRow("BSTMG") Mod dRow("UMRECH") > 0 Then
                    strMessage = "Bestelleinheit: " & item("BPRME").Text & " (" & dRow("UMRECH").ToString() & " ST)"
                    blnItemOK = False
                ElseIf dRow("BSTMG") < (dRow("MINBM") * dRow("UMRECH")) Then
                    strMessage = "Mindestbestellmenge: " & dRow("MINBM").ToString() & " " & item("BPRME").Text & " (" & (dRow("MINBM") * dRow("UMRECH")).ToString() & " ST)"
                    blnItemOK = False
                End If
                imgFehler.Visible = Not blnItemOK
                imgFehler.ToolTip = strMessage
                If Not blnItemOK Then blnMengenOk = False
            Next

            If blnShowErrormessage And Not blnMengenOk Then
                lblError.Text &= "Bitte prüfen Sie die Mengen."
            End If

            'Mindestbestellwerte prüfen (sofern alle Einzelposten ok sind)
            Dim blnMinBWOk As Boolean = True
            For Each fItem As GridGroupFooterItem In rgGrid1.MasterTableView.GetItems(GridItemType.GroupFooter)
                If CDbl(fItem("Gesamt").Text.Replace("€", "")) < CDbl(fItem("MINBW").Text.Replace("Max :", "")) Then
                    fItem("Menge").Text = "Bestellwert min. " & fItem("MINBW").Text.Replace("Max :", "") & " € !"
                    blnMinBWOk = False
                Else
                    fItem("Menge").Text = ""
                End If
            Next
            If blnShowErrormessage And Not blnMinBWOk Then
                lblError.Text &= " Bitte stellen Sie sicher, dass der Mindestbestellwert je Lieferant erreicht ist."
            End If

            blnOk = (blnMengenOk And blnMinBWOk)

        Catch ex As Exception
            lblError.Text &= " Bei der Prüfung der Bestellung ist ein Fehler aufgetreten: " & ex.Message
            blnOk = False
        End Try

        Return blnOk
    End Function

End Class

' ************************************************
' $History: Change01.aspx.vb $
' 
' *****************  Version 21  *****************
' User: Rudolpho     Date: 1.11.10    Time: 16:16
' Updated in $/CKAG2/KBS/Forms
' 
' *****************  Version 20  *****************
' User: Rudolpho     Date: 3.08.10    Time: 16:46
' Updated in $/CKAG2/KBS/Forms
' 
' *****************  Version 19  *****************
' User: Rudolpho     Date: 12.03.10   Time: 13:36
' Updated in $/CKAG2/KBS/Forms
' 
' *****************  Version 18  *****************
' User: Rudolpho     Date: 12.03.10   Time: 9:49
' Updated in $/CKAG2/KBS/Forms
' 
' *****************  Version 17  *****************
' User: Rudolpho     Date: 24.02.10   Time: 17:59
' Updated in $/CKAG2/KBS/Forms
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 10.02.10   Time: 17:53
' Updated in $/CKAG2/KBS/Forms
' 
' *****************  Version 15  *****************
' User: Jungj        Date: 26.05.09   Time: 10:13
' Updated in $/CKAG2/KBS/Forms
' Nachbesserungen
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 11.05.09   Time: 10:35
' Updated in $/CKAG2/KBS/Forms
' nderungen Nach test ITA 2808
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 7.05.09    Time: 17:07
' Updated in $/CKAG2/KBS/Forms
' Letzter Versuch beim Enterdrcken in Menge gescheitert
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 6.05.09    Time: 10:21
' Updated in $/CKAG2/KBS/Forms
' Nachbesserungen InputScanner
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 5.05.09    Time: 10:12
' Updated in $/CKAG2/KBS/Forms
' ITA 2838 kommentare 
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 29.04.09   Time: 17:38
' Updated in $/CKAG2/KBS/Forms
' ITA 2808
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 29.04.09   Time: 9:31
' Updated in $/CKAG2/KBS/Forms
' ITA 2808 testfertig
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 28.04.09   Time: 17:56
' Updated in $/CKAG2/KBS/Forms
' ITA 2808
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 27.04.09   Time: 17:24
' Updated in $/CKAG2/KBS/Forms
' ITA 2808
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 27.04.09   Time: 9:17
' Updated in $/CKAG2/KBS/Forms
' ITA 2808
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 24.04.09   Time: 15:48
' Updated in $/CKAG2/KBS/Forms
' ITA 2808
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 24.04.09   Time: 10:35
' Updated in $/CKAG2/KBS/Forms
' from server.transfer back wegen js
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 24.04.09   Time: 9:59
' Updated in $/CKAG2/KBS/Forms
' ITA 2808
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 23.04.09   Time: 17:50
' Updated in $/CKAG2/KBS/Forms
' ITA 2808
' 
'
' ************************************************