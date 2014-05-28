Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports System.Drawing
Partial Public Class Report07
    Inherits System.Web.UI.Page


#Region "Declarations"
    Private m_App As App
    Private m_User As User
    Private objHaendler As TempZuEndg
    Private objSuche As AppF2.Search
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmdSearch.Visible = True
        m_User = GetUser(Me)
        FormAuth(Me, m_User, True)
        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        m_App = New App(m_User)

        If m_User.Reference.Trim(" "c).Length > 0 Then
            txtHaendlerNr.Text = m_User.Reference
        End If

        If (Not Request.QueryString("back") Is Nothing) AndAlso CStr(Request.QueryString("back")) = "1" Then
            'Session("AppReport07") = Nothing
            If Not Session("objSuche") Is Nothing Then
                txtHaendlerNr.Text = CType(Session("objSuche"), AppF2.Search).REFERENZ
            End If
        End If

        If (Not Request.QueryString("Linked") Is Nothing) AndAlso CStr(Request.QueryString("Linked")) = "1" Then
            If Not Session("objSuche") Is Nothing Then
                txtHaendlerNr.Text = Session("AppHaendlerNr")
            End If
        End If


        If IsPostBack = False Then
            If objHaendler Is Nothing Then
                If Session.Item("AppReport07") Is Nothing Then
                    objHaendler = New TempZuEndg(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                    Session.Add("AppReport07", objHaendler)
                Else
                    If TypeOf Session.Item("AppReport07") Is Haendler Then
                        objHaendler = CType(Session("AppReport07"), TempZuEndg)
                    Else
                        objHaendler = New TempZuEndg(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                        Session("AppReport07") = objHaendler
                    End If
                End If
            End If
            If objSuche Is Nothing Then
                searchHaendlerInitial()
            End If

        Else 'wenn Postback
            If objHaendler Is Nothing Then
                objHaendler = CType(Session("AppReport07"), TempZuEndg)
            End If
            If objSuche Is Nothing Then
                objSuche = CType(Session("objNewHaendlerSuche"), AppF2.Search)
            End If
        End If

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
#End Region

    Protected Sub lbSelektionZurueckSetzen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbSelektionZurueckSetzen.Click
        SelektionZuruecksetzen()
    End Sub


    Private Sub SelektionZuruecksetzen()
        'Zustand wie bei initial laden wiederherstellen
        txtName1.Text = ""
        txtName2.Text = ""
        txtPLZ.Text = ""
        txtNummer.Text = ""
        txtOrt.Text = ""
        lblSHistoryName1.Text = ""
        lblSHistoryName2.Text = ""
        lblSHistoryNR.Text = ""
        lblSHistoryOrt.Text = ""
        lblSHistoryPLZ.Text = ""

        'txtName2 Deaktivieren
        txtName2.Enabled = False
        txtName2.BackColor = System.Drawing.Color.FromKnownColor(KnownColor.InactiveCaptionText)

        txtDatumVon.Text = ""
        txtDatumBis.Text = ""

        lbHaendler.SelectedIndex = -1

        If objSuche Is Nothing Then
            objSuche = CType(Session("objNewHaendlerSuche"), AppF2.Search)
        End If

        With objSuche
            .sucheHaendlerNr = txtNummer.Text
            .sucheName1 = txtName1.Text
            .sucheName2 = txtName2.Text
            .sucheOrt = txtOrt.Text
            .suchePLZ = txtPLZ.Text
        End With

        Searchhaendler()
    End Sub

    Private Sub DoSubmit()

        lblError.Text = ""
        lblError.Visible = False

        objHaendler.Customer = lblHaendlerDetailsNR.Text


        objHaendler.KUNNR = m_User.KUNNR
        objHaendler.personenennummer= lblHaendlerDetailsNR.Text


        If checkDate() Then

            If txtDatumVon.Text.Length > 0 AndAlso txtDatumBis.Text.Length = 0 Then
                lblError.Text = "Bitte geben Sie einen Zeitraum 'von - bis' ein oder wählen Sie einen Händler aus."
                lblError.Visible = True
                Exit Sub
            End If

            If txtDatumBis.Text.Length > 0 AndAlso txtDatumVon.Text.Length = 0 Then
                lblError.Text = "Bitte geben Sie einen Zeitraum 'von - bis' ein oder wählen Sie einen Händler aus."
                lblError.Visible = True
                Exit Sub
            End If

            If txtDatumVon.Text.Length > 0 AndAlso txtDatumBis.Text.Length > 0 Then

                objHaendler.DatumVon = txtDatumVon.Text
                objHaendler.DatumBis = txtDatumBis.Text
            End If



        Else
            Exit Sub

        End If

        If lblHaendlerDetailsNR.Text.Length = 0 AndAlso txtDatumVon.Text.Length = 0 AndAlso txtDatumBis.Text.Length = 0 Then
            lblError.Text = "Es wurden keine Abfragekriterien ausgewählt."
            lblError.Visible = True
            Exit Sub
        End If


        objHaendler.Report(Session("AppID").ToString, Session.SessionID, Me)


        If Not objHaendler.Status = 0 Then
            lblError.Text = objHaendler.Message
            Exit Sub
        End If

        If objHaendler.Fahrzeuge.Rows.Count = 0 Then
            lblError.Text = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
            lbSelektionZurueckSetzen.Visible = True
            lblError.Visible = True
        Else
            Session("AppReport07") = objHaendler
            Session("objNewHaendlerSuche") = objSuche
            Response.Redirect("Report07_2.aspx?AppID=" & Session("AppID").ToString, False)
        End If

    End Sub

    Private Function checkDate() As Boolean
        Dim tmpbool As Boolean = True
        For Each ctrl As BaseValidator In Me.Validators
            If ctrl.IsValid = False Then
                tmpbool = False
            End If
        Next
        Return tmpbool
    End Function

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click
        DoSubmit()
    End Sub
    Protected Sub lbHaendler_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lbHaendler.SelectedIndexChanged
        objSuche = Session("objNewHaendlerSuche")

        objSuche.Haendler.RowFilter = "REFERENZ = '" & lbHaendler.SelectedValue & "'"
        lblHaendlerDetailsNR.Text = objSuche.Haendler.Item(0).Item("REFERENZ")
        lblHaendlerDetailsName1.Text = objSuche.Haendler.Item(0).Item("Name")
        lblHaendlerDetailsName2.Text = objSuche.Haendler.Item(0).Item("NAME_2")
        lblHaendlerDetailsStrasse.Text = objSuche.Haendler.Item(0).Item("STREET")
        lblHaendlerDetailsPLZ.Text = objSuche.Haendler.Item(0).Item("POSTL_CODE")
        lblHaendlerDetailsOrt.Text = objSuche.Haendler.Item(0).Item("CITY")
    End Sub

    Private Sub searchHaendlerInitial()


        objSuche = New AppF2.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)


        With objSuche
            .sucheHaendlerNr = txtNummer.Text
            .sucheName1 = txtName1.Text
            .sucheName2 = txtName2.Text
            .sucheOrt = txtOrt.Text
            .suchePLZ = txtPLZ.Text
        End With

        Searchhaendler()
        If lbl_error.Text = "" AndAlso objSuche.anzahlHaendlerTreffer > 0 AndAlso objSuche.anzahlHaendlerTreffer <= 200 Then


            'Name 2 Textbox wieder aktivieren da ja jetzt von name1 sapseitig auch in name2 gesucht wird, clientseitig nicht möglich! JJ2008.04.23
            txtName2.Enabled = True
            txtName2.BackColor = Nothing



            'werte in SearchHistoryLabels Schreiben
            lblSHistoryName1.Text = txtName1.Text
            lblSHistoryName2.Text = txtName1.Text
            '------------------------------------
            'nein weil jetzt sap seitig in name 1 und 2 gesucht wird bei eingabe in Name 1
            'lblSHistoryName2.Text = txtName2.Text
            '-----------------------------------
            lblSHistoryNR.Text = txtNummer.Text
            lblSHistoryOrt.Text = txtOrt.Text
            lblSHistoryPLZ.Text = txtPLZ.Text



            'wenn nicht * am ende dann in historyLabel Schreiben sonst * entfernen
            If Not txtNummer.Text.Trim(" "c).Length = 0 AndAlso txtNummer.Text.IndexOf("*") < txtNummer.Text.Length - 1 Then
                txtNummer.Text = ""
            Else
                txtNummer.Text = txtNummer.Text.Replace("*", "")
            End If

            If Not txtName1.Text.Trim(" "c).Length = 0 AndAlso txtName1.Text.IndexOf("*") < txtName1.Text.Length - 1 Then
                txtName1.Text = ""
            Else
                '---------------------------------------------------------------
                'der Bapi sucht bei einer übergabe von Name1 auch in Name 2, daher muss immer die Eingabe gelöscht werden, da sonst die clientseitige Selektion nicht greift, 
                'dort geht nur die suche eine Eingabe-> ein Sucharray, da ja alle nicht zutreffenden rausgeschmissen werden. JJ2008.4.23
                'txtName1.Text = txtName1.Text.Replace("*", "")
                '---------------------------------------------------------------
                txtName1.Text = ""
            End If

            If Not txtName2.Text.Trim(" "c).Length = 0 AndAlso txtName2.Text.IndexOf("*") < txtName2.Text.Length - 1 Then
                txtName2.Text = ""
            Else
                txtName2.Text = txtName2.Text.Replace("*", "")
            End If

            If Not txtPLZ.Text.Trim(" "c).Length = 0 AndAlso txtPLZ.Text.IndexOf("*") < txtPLZ.Text.Length - 1 Then
                txtPLZ.Text = ""
            Else
                txtPLZ.Text = txtPLZ.Text.Replace("*", "")
            End If


            If Not txtOrt.Text.Trim(" "c).Length = 0 AndAlso txtOrt.Text.IndexOf("*") < txtOrt.Text.Length - 1 Then
                txtOrt.Text = ""
            Else
                txtOrt.Text = txtOrt.Text.Replace("*", "")
            End If



        End If
        Session("objNewHaendlerSuche") = objSuche
    End Sub
    Private Sub Searchhaendler()
        'neue Klassse erstellen für BAPI Aufruf

        Try

            lbl_error.Text = ""
            lbl_Message.Text = ""
            Dim intStatus As Int32
            intStatus = objSuche.LeseHaendlerForSucheHaendlerControl(Session("AppID").ToString, Session.SessionID.ToString, Me.Page)

            If Not intStatus = 0 Then
                lbl_error.Text = objSuche.ErrorMessage
                Exit Sub
            End If

            lbHaendler.DataSource = objSuche.Haendler
            lbHaendler.DataTextField = "DISPLAY"
            lbHaendler.DataValueField = "REFERENZ"
            lbHaendler.DataBind()

            If objSuche.anzahlHaendlerTreffer = 0 Then
                lbl_Message.Text = "keine Ergebnisse gefunden"

                lblErgebnissAnzahl.Text = objSuche.anzahlHaendlerTreffer
                lbHaendler.Items.Clear()
                tr_HaendlerAuswahl.Visible = False
                lbl_Info.Visible = True
            ElseIf Not objSuche.anzahlHaendlerTreffer = 0 AndAlso objSuche.Haendler Is Nothing OrElse objSuche.Haendler.Count = 0 Then
                If anySearchEntrys() = True Then
                    lbl_Message.Text = "zu viele Ergebnisse gefunden (max 200 Treffer), weitere Einschränkungen benötigt "
                End If


                lbHaendler.Items.Clear()
                tr_HaendlerAuswahl.Visible = False
                lblErgebnissAnzahl.Text = objSuche.anzahlHaendlerTreffer
                lbl_Info.Visible = True
            Else
                lblErgebnissAnzahl.Text = objSuche.anzahlHaendlerTreffer
                tr_HaendlerAuswahl.Visible = True
                txtName2.Enabled = True
                txtName2.BackColor = Nothing
                lbl_Info.Visible = False
                lbl_Message.Text = ""
            End If

        Catch ex As Exception
            lbl_error.Text = "Es ist ein Fehler aufgetreten: " & ex.Message
        End Try
    End Sub

    Protected Sub txtName1_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtName1.TextChanged
        Dim strFilter As String
        strFilter = Replace(txtName1.Text, "*", "%")
        Search("NAME LIKE '%" & strFilter & "%'")
    End Sub

    Private Function anySearchEntrys() As Boolean

        If Not txtName1.Text.Trim(" "c) = "" OrElse Not txtName1.Text.Trim(" "c) = "" OrElse Not txtName2.Text.Trim(" "c) = "" OrElse Not txtOrt.Text.Trim(" "c) = "" OrElse Not txtPLZ.Text.Trim(" "c) = "" OrElse Not txtNummer.Text.Trim(" "c) = "" Then
            Return True
        Else
            Return False
        End If


    End Function

    Private Sub txtPLZ_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPLZ.TextChanged
        Dim strFilter As String
        strFilter = Replace(txtPLZ.Text, "*", "%")
        Search("POSTL_CODE LIKE '%" & strFilter & "%'")
    End Sub
    Private Sub Search(ByVal Filter As String)

        If Session("objNewHaendlerSuche") Is Nothing Then
            objSuche = New AppF2.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
            Session("objNewHaendlerSuche") = objSuche
            searchHaendlerInitial()
        Else
            objSuche = Session("objNewHaendlerSuche")
            If objSuche.Haendler Is Nothing Then
                searchHaendlerInitial()
            End If

        End If

        Try
            If objSuche.anzahlHaendlerTreffer = 0 Then
                lbl_Message.Text = "keine Ergebnisse gefunden"

                lblErgebnissAnzahl.Text = objSuche.anzahlHaendlerTreffer
                lbHaendler.Items.Clear()
                tr_HaendlerAuswahl.Visible = False

                lbl_Info.Visible = True
            ElseIf Not objSuche.anzahlHaendlerTreffer = 0 AndAlso objSuche.Haendler Is Nothing OrElse objSuche.Haendler.Count = 0 Then
                If anySearchEntrys() = True Then
                    lbl_Message.Text = "zu viele Ergebnisse gefunden (max 200 Treffer), weitere Einschränkungen benötigt "
                End If


                lbHaendler.Items.Clear()
                tr_HaendlerAuswahl.Visible = False
                lblErgebnissAnzahl.Text = objSuche.anzahlHaendlerTreffer
                lbl_Info.Visible = True
            Else
                lblErgebnissAnzahl.Text = objSuche.anzahlHaendlerTreffer
                tr_HaendlerAuswahl.Visible = True
                txtName2.Enabled = True
                txtName2.BackColor = Nothing
                lbl_Info.Visible = False
                lbl_Message.Text = ""
                objSuche.Haendler.RowFilter = Filter
                lbHaendler.DataSource = objSuche.Haendler
                lbHaendler.DataTextField = "DISPLAY"
                lbHaendler.DataValueField = "REFERENZ"
                lbHaendler.DataBind()
            End If

        Catch ex As Exception
            lbl_error.Text = "Es ist ein Fehler aufgetreten: " & ex.Message
        End Try
    End Sub

    Protected Sub txtNummer_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtNummer.TextChanged
        Dim strFilter As String
        strFilter = Replace(txtNummer.Text, "*", "%")
        Search("REFERENZ LIKE '%" & strFilter & "%'")
    End Sub

    Protected Sub txtName2_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtName2.TextChanged
        Dim strFilter As String
        strFilter = Replace(txtName2.Text, "*", "%")
        Search("NAME_2 LIKE '%" & strFilter & "%'")
    End Sub

    Protected Sub txtOrt_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtOrt.TextChanged
        Dim strFilter As String
        strFilter = Replace(txtOrt.Text, "*", "%")
        Search("CITY LIKE '" & strFilter & "'")
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx", False)
    End Sub
End Class



