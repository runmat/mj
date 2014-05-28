Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Business

Partial Public Class Report01s_02
    Inherits Page

#Region "Declarations"
    Private m_App As App
    Private m_User As User

#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        m_App = New App(m_User)


        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            m_App = New App(m_User)

            lblError.Text = ""

            If IsPostBack = False Then
                FillFields()

            End If

        Catch ex As Exception
            lblError.Text = String.Format("Beim Laden der Seite ist ein Fehler aufgetreten.<br>({0})", ex.Message)
        End Try

    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub lbInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbInsert.Click
        DoSubmit()
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Dim parameterlist As String = ""
        HelpProcedures.getAppParameters(Session("AppID").ToString, parameterlist, ConfigurationManager.AppSettings.Get("Connectionstring"))
        Response.Redirect("Report01s.aspx?AppID=" & Session("AppID").ToString & parameterlist)
    End Sub

#End Region

#Region "Methods"

    Private Function GetAufnr() As String
        Dim sAufnr = Page.Request.Params("AUFNR").Trim()

        If Regex.IsMatch(sAufnr, "^\d+$") Then Return sAufnr

        Return String.Empty
    End Function

    Private Sub FillFields()
        Dim aufnr = GetAufnr()
        Dim mObjBestand = CType(Session("mObjBestand"), Bestand)
        Dim tmpRow = mObjBestand.Rueckmeldung.Select(String.Format("Auftragsnummer='{0}'", aufnr)).First

        txtEmail.Text = tmpRow("Email").ToString
        txtHalter.Text = tmpRow("Halter").ToString
        txtFahrgestellnummer.Text = tmpRow("Fahrgestellnummer").ToString
        txtOrt.Text = tmpRow("Ort").ToString
        txtStrasse.Text = tmpRow("Strasse").ToString
        txtPostleitzahl.Text = tmpRow("Postleitzahl").ToString
        txtTelefon.Text = tmpRow("Telefon").ToString
        txtArbeitsende.Text = Now.Date.ToShortDateString
        If tmpRow("Teilrückmeldung").ToString = "X" Then
            txtProgEnd.Enabled = True
            hiddenTeilRück.Value = "X"
        End If
        FillDropdowns(aufnr)

    End Sub

    Private Sub FillDropdowns(aufnr As Integer)

        Dim mObjBestand = CType(Session("mObjBestand"), Bestand)

        ddlLeistungsart.Items.Add(New ListItem("Wählen Sie eine Leistungsart", "0"))

        'Leistungsarten ermitteln
        If mObjBestand.Leistungsart.Select(String.Format("ARBPL='{0}'", mObjBestand.ArbeitsplatzUser)).Length > 0 Then

            For Each dRow As DataRow In mObjBestand.Leistungsart.Select(String.Format("ARBPL='{0}'", mObjBestand.ArbeitsplatzUser))

                'Gibt es einen Eintrag in der Gesamttabelle
                Dim leistungsart As String = dRow("LTXA1").ToString
                Dim meldungswert As String = mObjBestand.Gesamt.Select(String.Format("Auftragsnummer='{0}'", aufnr))(0)(leistungsart).ToString

                If meldungswert = "" OrElse meldungswert.Contains("*TR*") OrElse meldungswert.Contains("*PD*") Then
                    'Gibt es bereits einen Eintrag in der Rückmeldetabelle?
                    If mObjBestand.ArbeitsplanDatumswerte.Select(String.Format("AUFNR='{0}' AND LEARR ='{1}'", aufnr, dRow("LARNT").ToString)).Length > 0 Then

                        Dim tempTable = mObjBestand.ArbeitsplanDatumswerte.Select(String.Format("AUFNR='{0}' AND LEARR ='{1}'", aufnr, dRow("LARNT"))).CopyToDataTable
                        tempTable.DefaultView.Sort = "IEDD DESC, IEDZ DESC"

                        If tempTable.DefaultView(0)("AUERU").ToString.ToUpper <> "X" Then
                            'Handelt es sich um eine Teilrückmeldung?
                            ddlLeistungsart.Items.Add(New ListItem(dRow("LTXA1").ToString, dRow("LARNT").ToString))
                        End If
                    Else
                        ddlLeistungsart.Items.Add(New ListItem(dRow("LTXA1").ToString, dRow("LARNT").ToString))
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub DoSubmit()
        If ddlLeistungsart.SelectedItem.Value = "0" Then
            lblError.Text = "Bitte wählen Sie eine Leistungsart aus."
            Exit Sub
        End If

        Dim mObjBestand = CType(Session("mObjBestand"), Bestand)

        Dim plZaehler As String

        Dim dRow = mObjBestand.Leistungsart.Select(String.Format("LARNT='{0}' AND ARBPL='{1}'", ddlLeistungsart.SelectedItem.Value, mObjBestand.ArbeitsplatzUser))

        If dRow.Length > 0 Then
            plZaehler = dRow(0)("PLNAL").ToString
        Else
            lblError.Text = "Planzähler konnte nicht ermittelt werden."
            Exit Sub
        End If

        'If hiddenTeilRück.Value = "X" AndAlso IsDate(txtProgEnd.Text) Then
        '    If CDate(txtProgEnd.Text) <= Now Then
        '        lblError.Text = "Prognose Ende muss nur in der Zukunft liegen!"
        '        Exit Sub
        '    End If
        If chkEndrückmeldung.Checked And txtProgEnd.Text.Trim.Length > 0 Then
            lblError.Text = "Nachdem setzen des neuen Prognosedatums kann die Endrückmeldung nicht gesetzt werden!"
            Exit Sub
        End If
        'End If

        Dim strMessage As String = mObjBestand.SaveData(m_User, m_App, _
                                                        Me, _
                                                        plZaehler, _
                                                        txtFahrgestellnummer.Text, _
                                                        ddlLeistungsart.SelectedItem.Value, _
                                                        IIf(chkEndrückmeldung.Checked = True, "X", "").ToString, _
                                                        txtRueckmeldetext.Text, _
                                                        ddlLeistungsart.SelectedItem.Text, _
                                                        txtProgEnd.Text)

        If strMessage.Length > 0 Then
            lblError.Text = strMessage
        Else
            lblMessage.Text = "Ihre Daten wurden gespeichert."
            ddlLeistungsart.Enabled = False
            txtRueckmeldetext.Enabled = False
            chkEndrückmeldung.Enabled = False
            lbInsert.Visible = False
            mObjBestand.GetAuswertung(m_User, m_App, Me)
            Session("mObjBestand") = mObjBestand
        End If
    End Sub

#End Region

    Protected Sub chkEndrückmeldung_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkEndrückmeldung.CheckedChanged
        If chkEndrückmeldung.Checked And txtProgEnd.Text.Trim.Length > 0 Then
            txtProgEnd.Enabled = False
            txtProgEnd.Text = String.Empty
            lblError.Text = "Nachdem setzen des neuen Prognosedatums kann die Endrückmeldung nicht gesetzt werden!"
        Else
            txtProgEnd.Enabled = True
        End If
    End Sub
End Class

' ************************************************
' $History: Report01s_02.aspx.vb $
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 26.07.10   Time: 17:20
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 23.07.10   Time: 16:20
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 23.07.10   Time: 11:16
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 23.07.10   Time: 10:53
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 7  *****************
' User: Fassbenders  Date: 1.06.10    Time: 14:35
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 31.05.10   Time: 17:34
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 18.05.10   Time: 20:37
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' ITA 3754 unfertig
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 17.05.10   Time: 21:08
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 14.05.10   Time: 16:59
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 14.05.10   Time: 12:01
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 14.05.10   Time: 11:33
' Created in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 11.05.10   Time: 13:51
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.05.10    Time: 16:56
' Created in $/CKAG2/Services/Components/ComCommon/Leasing
' ITA: 3738
' 