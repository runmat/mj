Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Services.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports CKG

Partial Public Class Change205_3
    Inherits Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    'Private objSuche As Search
    'Private objAddressList As Search
    Private objHaendler As Arval_1
    Private versandart As String

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)

        versandart = Request.QueryString.Item("art")

        lnkFahrzeugAuswahl.NavigateUrl = "Change205_2.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart
        lnkFahrzeugsuche.NavigateUrl = "Change205.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart


        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change205.aspx?AppID=" & Session("AppID").ToString & "art=" & versandart)
            End If
            objHaendler = CType(Session("objHaendler"), Arval_1)

            If Not IsPostBack Then
                initialLoad()
                ddlGrund.Attributes.Add("onChange", "ShowHide();")
                If (versandart = "ENDG") Then
                    tdInput.Visible = False
                    tdInput1.Visible = False
                End If
            Else

            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub initialLoad()
        Dim status As String = ""
        Dim item As ListItem
        Dim view As DataView
        Dim row As DataRow
        Dim tblTemp As DataTable
        Dim newRow As DataRow
        Dim counter As Integer
        Dim sprache As String

        Dim zulDienste As DataTable = New DataTable()

        counter = 0
        If (versandart = "TEMP") Then
            'Zulassungsdientste holen
            objHaendler.getZulassungsdienste(zulDienste, status)

            tblTemp = zulDienste.Copy   'Kopie erstellen
            tblTemp.Clear()

            For Each row In zulDienste.Rows
                If row("ORT01").ToString <> String.Empty Then
                    newRow = tblTemp.NewRow
                    newRow("LIFNR") = row("LIFNR").ToString
                    newRow("ORT01") = row("ORT01").ToString
                    If row("STRAS").ToString <> String.Empty Then
                        newRow("ORT01") = newRow("ORT01").ToString & ", " & row("STRAS").ToString    '§§§ JVE 24.02.2006 Auch die Strasse mit anzeigen!
                    End If
                    newRow("STRAS") = row("STRAS").ToString
                    newRow("PSTLZ") = row("PSTLZ").ToString
                    newRow("PSTL2") = CType(counter, String)                        'Lfd. Nr. (für spätere Suche!)
                    tblTemp.Rows.Add(newRow)
                    tblTemp.AcceptChanges()
                    counter += 1
                End If
            Next

            zulDienste = tblTemp.Copy
            zulDienste.AcceptChanges()
            tblTemp = Nothing

            Session.Add("ZulDienste", zulDienste)

            view = zulDienste.DefaultView
            view.Sort = "ORT01"
            With ddlZulDienst
                .DataSource = view
                .DataTextField = "ORT01"
                .DataValueField = "PSTL2"
                .DataBind()
            End With

            trZeit.Visible = True
            trVersandAdrTemp.Visible = True
            trVersandAdrEnd1.Visible = False
            trVersandAdrEnd2.Visible = False
            trVersandAdrEnd3.Visible = False
            trVersandAdrEnd6.Visible = False
            trVersandAdrEnd5.Visible = False
            lblPageTitle.Text = " (Versandadresse, Versandgrund und -art wählen)"
        Else
            'UH 14.06.2005
            'Jetzt auch für endgültig:
            trZeit.Visible = True
            'trZeit.Visible = False

            trVersandAdrTemp.Visible = False
            trVersandAdrEnd1.Visible = True
            trVersandAdrEnd2.Visible = True
            trVersandAdrEnd3.Visible = True
            trVersandAdrEnd6.Visible = True
            trVersandAdrEnd5.Visible = True
            lblPageTitle.Text = " (Versandadresse eingeben)"

            'Länder DLL füllen
            ddlLand.DataSource = objHaendler.Laender
            'ddlLand.DataTextField = "Beschreibung"
            ddlLand.DataTextField = "FullDesc"
            ddlLand.DataValueField = "Land1"
            ddlLand.DataBind()
            'vorbelegung der Länderddl auf Grund der im Browser eingestellten erstsprache JJ2007.12.06
            Dim tmpstr() As String
            If Request("HTTP_ACCEPT_Language").IndexOf(",") = -1 Then
                'es ist nur eine sprache ausgewählt
                sprache = Request("HTTP_ACCEPT_Language")
            Else
                'es gibt eine erst und eine zweitsprache
                sprache = Request("HTTP_ACCEPT_Language").Substring(0, Request("HTTP_ACCEPT_Language").IndexOf(","))
            End If

            tmpstr = sprache.Split(CChar("-"))
            'Länderkennzeichen setzen sich aus Region und Sprache zusammen. de-ch, de-at usw. leider werden bei Regionen in denen die Sprache das selbe Kürzel hat nur einfache Kürzel geschrieben, z.b. bei "de"
            If tmpstr.Length > 1 Then
                sprache = tmpstr(1).ToUpper
            Else
                sprache = tmpstr(0).ToUpper
            End If
            ddlLand.Items.FindByValue(sprache).Selected = True

        End If
        'Gründe Füllen...
        For Each row In objHaendler.GrundTabelle.Rows
            item = New ListItem()
            item.Value = row("ZZVGRUND").ToString
            item.Text = row("TEXT50").ToString
            ddlGrund.Items.Add(item)
        Next
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If (versandart = "TEMP") Then
            DoSubmit()
        Else
            If txtName1.Text = "" OrElse txtStr.Text = "" OrElse txtPlz.Text = "" OrElse txtNr.Text = "" OrElse txtOrt.Text = "" Then
                lblError.Text = "Bitte alle Felder füllen."
                Exit Sub
            Else

                'PLZ prüfen
                If CInt(objHaendler.Laender.Select("Land1='" & ddlLand.SelectedItem.Value & "'")(0)("Lnplz")) > 0 Then

                    If Not CInt(objHaendler.Laender.Select("Land1='" & ddlLand.SelectedItem.Value & "'")(0)("Lnplz")) = txtPlz.Text.Trim(" "c).Length Then
                        lblError.Text = "Postleitzahl hat falsche Länge."
                        Exit Sub
                    End If
                End If

                DoSubmit()
            End If
        End If
    End Sub

    Private Sub DoSubmit()
        Dim status As String
        Dim key As String
        Dim row As DataRow()
        Dim zulDienste As DataTable

        status = String.Empty

        zulDienste = CType(Session("ZulDienste"), DataTable)

        'UH 14.06.2005
        'Jetzt auch für endgültig:
        If chkVersandStandard.Checked Then                                'Versandart
            objHaendler.Materialnummer = "1391"
        ElseIf chk0900.Checked Then
            objHaendler.Materialnummer = "1385"
        ElseIf chk1000.Checked Then
            objHaendler.Materialnummer = "1389"
        ElseIf chk1200.Checked Then
            objHaendler.Materialnummer = "1390"
        End If

        If (versandart = "TEMP") Then

            objHaendler = CType(Session("objHaendler"), Arval_1)
            key = ddlZulDienst.SelectedItem.Value
            row = zulDienste.Select("Pstl2='" & key & "'")
            objHaendler.Bemerkung = txtGrundBemerkung.Text

            If (row(0)("LIFNR").ToString <> String.Empty) Then
                objHaendler.VersandAdresse = row(0)("LIFNR").ToString      'Versandadresse Nr. (60...)
                objHaendler.VersandAdresseText = ddlZulDienst.SelectedItem.Text   'Versanddresse (Text...)
            Else

                objHaendler.ZielFirma = "Zulassungsstelle"                               'Adresse für temp. Versand (wenn keine Lifnr. vorhanden)
                objHaendler.ZielStrasse = row(0)("STRAS").ToString
                objHaendler.ZielHNr = ""
                objHaendler.ZielPLZ = row(0)("PSTLZ").ToString
                objHaendler.ZielOrt = row(0)("ORT01").ToString
                objHaendler.VersandAdresseText = objHaendler.ZielFirma & "<br>" & objHaendler.ZielFirma2 & "<br>" & objHaendler.ZielStrasse & "&nbsp; " & objHaendler.ZielHNr & "<br>" & objHaendler.ZielPLZ & "&nbsp; " & objHaendler.ZielOrt

            End If

            'UH 14.06.2005
            'Hier nicht mehr!
            'If chkVersandStandard.Checked Then                                'Versandart
            '    objHaendler.Materialnummer = "1391"
            'ElseIf chk0900.Checked Then
            '    objHaendler.Materialnummer = "1385"
            'ElseIf chk1000.Checked Then
            '    objHaendler.Materialnummer = "1389"
            'ElseIf chk1200.Checked Then
            '    objHaendler.Materialnummer = "1390"
            'End If
        Else
            objHaendler.ZielFirma = txtName1.Text                               'Adresse für Endgültigen Versand
            objHaendler.ZielFirma2 = txtName2.Text                               'Adresse für Endgültigen Versand
            objHaendler.ZielStrasse = txtStr.Text
            objHaendler.ZielHNr = txtNr.Text
            objHaendler.ZielPLZ = txtPlz.Text
            objHaendler.ZielOrt = txtOrt.Text
            'objHaendler.ZielLand = txtLand.Text
            objHaendler.ZielLand = ddlLand.SelectedItem.Value.ToString

            objHaendler.VersandAdresseText = objHaendler.ZielFirma & "<br>" & objHaendler.ZielFirma2 & "<br>" & objHaendler.ZielStrasse & "&nbsp; " & objHaendler.ZielHNr & "<br>" & objHaendler.ZielPLZ & "&nbsp; " & objHaendler.ZielOrt & "<br>" & objHaendler.ZielLand

        End If
        objHaendler.VersandGrund = ddlGrund.SelectedItem.Value.ToString
        objHaendler.VersandGrundText = ddlGrund.SelectedItem.Text
        Session("objHaendler") = objHaendler
        Response.Redirect("Change205_4.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change205_3.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 20.04.09   Time: 15:49
' Updated in $/CKAG2/Applications/AppArval/forms
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 17.04.09   Time: 14:40
' Created in $/CKAG2/Applications/AppArval/forms
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 11.03.09   Time: 15:38
' Updated in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 11.03.09   Time: 11:36
' Updated in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:18
' Created in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 8  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 7  *****************
' User: Uha          Date: 6.03.07    Time: 15:30
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' GetAppIDFromQueryString(Me) hinzugefügt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 6.03.07    Time: 14:48
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' 
' ************************************************
