Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report15
    Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtAmtlKennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFahrgestellnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBriefnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOrdernummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents gvSelectOne As GridView

    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                If Not String.IsNullOrEmpty(Page.Request.QueryString("VIN")) Then
                    DoSubmit(Page.Request.QueryString("VIN"))
                End If
            End If

            'DoSubmit()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit(Optional ByVal Fahrgestellnummer As String = "")

        Session("ResultTable") = Nothing
        Session("FahrzeugTable") = Nothing

        Dim m_Report As New ec_15(m_User, m_App, "")
        Try
            Dim strBriefnummer As String
            Dim strFahrgestellnummer As String
            Dim strAmtlKennzeichen As String
            Dim strOrdernummer As String


            If txtAmtlKennzeichen.Text.Length = 0 Then
                strAmtlKennzeichen = ""
            Else

                If txtAmtlKennzeichen.Text.Contains("*") = True Then
                    If txtAmtlKennzeichen.Text.Length < 8 Then
                        lblError.Text = "Kennzeichen-Eingabe unterschreitet Minimallänge."
                        Return
                    End If
                End If


                strAmtlKennzeichen = txtAmtlKennzeichen.Text


            End If

            If txtFahrgestellnummer.Text.Length = 0 Then
                strFahrgestellnummer = ""
            Else

                If txtFahrgestellnummer.Text.Contains("*") = True Then
                    If txtAmtlKennzeichen.Text.Length < 9 Then
                        lblError.Text = "Fahrgestellnummer-Eingabe unterschreitet Minimallänge."
                        Return
                    End If
                End If


                strFahrgestellnummer = txtFahrgestellnummer.Text
            End If

            If txtBriefnummer.Text.Length = 0 Then
                strBriefnummer = ""
            Else

                If txtBriefnummer.Text.Contains("*") = True Then
                    If txtBriefnummer.Text.Length < 5 Then
                        lblError.Text = "Briefnummer-Eingabe unterschreitet Minimallänge."
                        Return
                    End If
                End If

                strBriefnummer = txtBriefnummer.Text
            End If

            If txtOrdernummer.Text.Length = 0 Then
                strOrdernummer = ""
            Else

                If txtOrdernummer.Text.Contains("*") = True Then
                    If txtOrdernummer.Text.Length < 7 Then
                        lblError.Text = "Unitnummer-Eingabe unterschreitet Minimallänge."
                        Return
                    End If
                End If

                strOrdernummer = txtOrdernummer.Text
            End If
            
            
            

            If txtBriefnummer.Text.Length + txtFahrgestellnummer.Text.Length + txtAmtlKennzeichen.Text.Length + txtOrdernummer.Text.Length + Fahrgestellnummer.Length = 0 Then
                lblError.Text = "Bitte geben Sie ein Suchkriterium ein."
            Else

                If Fahrgestellnummer.Length > 0 Then
                    m_Report.FillHistory(Session("AppID").ToString, Session.SessionID.ToString, "", Fahrgestellnummer, "", "", Me)
                Else
                    m_Report.FillHistory(Session("AppID").ToString, Session.SessionID.ToString, strAmtlKennzeichen, strFahrgestellnummer, strBriefnummer, strOrdernummer, Me)
                End If


                Session("ResultTable") = m_Report.History
                Session("FahrzeugTable") = m_Report.ResultFahrzeuge
            End If



        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten."
        End Try

        If Not m_Report.Status = 0 Then
            lblError.Text = m_Report.Message
        Else

            If (m_Report.ResultFahrzeuge Is Nothing) OrElse (m_Report.ResultFahrzeuge.Rows.Count = 0) Then

                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            Else

                If m_Report.ResultFahrzeuge.Rows.Count = 1 Then
                    Response.Redirect("Report15_2.aspx?AppID=" & Session("AppID").ToString)
                Else
                    gvSelectOne.DataSource = m_Report.ResultFahrzeuge
                    gvSelectOne.DataBind()
                End If

                
            End If

            'If (m_Report.History Is Nothing) OrElse (m_Report.History.Rows.Count = 0) Then
            '    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            'Else
            '    Response.Redirect("Report15_2.aspx?AppID=" & Session("AppID").ToString)
            'End If



        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub gvSelectOne_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSelectOne.RowCommand
        If e.CommandName = "weiter" Then
            DoSubmit(e.CommandArgument.ToString)
        End If
    End Sub



End Class

' ************************************************
' $History: Report15.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 29.03.10   Time: 13:24
' Updated in $/CKAG/Applications/appec/Forms
' ITA: 3552
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 26.06.09   Time: 11:29
' Updated in $/CKAG/Applications/appec/Forms
' ITA 2918
' Z_M_EC_AVM_BRIEFLEBENSLAUF,Z_M_Ec_Avm_Fzg_M_Dfs_O_Zul,Z_M_EC_AVM_FZG_OH
' NE_BRIEF,Z_M_Ec_Avm_Fzg_Ohne_Unitnr,Z_M_Ec_Avm_Nur_Brief_Vorh,
' Z_M_EC_AVM_OFFENE_ZAHLUNGEN,  Z_M_EC_AVM_PDI_BESTAND,
' Z_M_EC_AVM_STATUS_EINSTEUERUNG,  Z_M_EC_AVM_STATUS_GREENWAY,
' Z_M_Ec_Avm_Status_Zul, Z_M_EC_AVM_ZULASSUNGEN, Z_M_Ec_Avm_Zulassungen_2
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 17.06.09   Time: 8:47
' Updated in $/CKAG/Applications/appec/Forms
' Warnungen entfernt!
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:49
' Created in $/CKAG/Applications/appec/Forms
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 12:29
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 22.05.07   Time: 13:31
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 5  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' ************************************************
