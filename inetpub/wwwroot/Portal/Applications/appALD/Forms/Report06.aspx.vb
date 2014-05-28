Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report06
    Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "
    Protected WithEvents txtVonDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBisDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdReport As System.Web.UI.WebControls.Button
    Protected WithEvents lnkHTML As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents rdbKlaerfall As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rdbAlle As System.Web.UI.WebControls.RadioButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header

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

    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        Try
            m_App = m_User.App
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReport.Click
        lblError.Text = ""
        If Not IsDate(txtVonDatum.Text) Then
            If Not IsStandardDate(txtVonDatum.Text) Then
                If Not IsSAPDate(txtVonDatum.Text) Then
                    lblError.Text = "Geben Sie bitte ein gültiges Startdatum ein!<br>"
                End If
            End If
        End If
        If Not IsDate(txtBisDatum.Text) Then
            If Not IsStandardDate(txtBisDatum.Text) Then
                If Not IsSAPDate(txtBisDatum.Text) Then
                    lblError.Text = lblError.Text & "Geben Sie bitte ein gültiges Enddatum ein!<br>"
                End If
            End If
        End If
        If lblError.Text.Length = 0 Then
            If CDate(txtBisDatum.Text) < CDate(txtVonDatum.Text) Then
                lblError.Text = "Das Enddatum darf nicht kleiner als das Startdatum sein!<br>"
            End If
            If CDate(txtVonDatum.Text) < DateAdd(DateInterval.Month, -3, Now) Then
                If Not m_App.TestModus Then
                    lblError.Text = lblError.Text & "Das Startdatum darf nicht länger als drei Monate zurückliegen!<br>"
                End If
            End If
            If lblError.Text.Length = 0 Then
                Try
                    Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                    Dim m_Klaerfaelle As New Klaerfaelle(m_User, m_App, strFileName)

                    If rdbAlle.Checked = True Then
                        m_Klaerfaelle.Fill(CDate(txtBisDatum.Text), CDate(txtVonDatum.Text), Session("AppID").ToString, Session.SessionID.ToString, True)
                    Else
                        m_Klaerfaelle.Fill(CDate(txtBisDatum.Text), CDate(txtVonDatum.Text), Session("AppID").ToString, Session.SessionID.ToString, False)
                    End If

                    Session("ResultTable") = m_Klaerfaelle.Result

                    If Not m_Klaerfaelle.Status = 0 Then
                        lblError.Text = m_Klaerfaelle.Message
                    Else
                        If m_Klaerfaelle.Result.Rows.Count = 0 Then
                            lblError.Text = "Keine Ergebnisse für den gewählten Zeitraum."
                        Else
                            lnkHTML.Visible = True
                            lnkHTML.NavigateUrl = "Report06_2.aspx?Show=HTML&AppID=" & Session("AppID").ToString

                            Dim objExcelExport As New Excel.ExcelExport()

                            Excel.ExcelExport.WriteExcel(m_Klaerfaelle.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                            lnkExcel.NavigateUrl = "/Portal/Temp/Excel/" & strFileName
                            lnkExcel.Visible = True
                            lblDownloadTip.Visible = True
                        End If
                    End If
                Catch ex As Exception
                    lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Try
            End If
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report06.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 29.04.09   Time: 14:58
' Updated in $/CKAG/Applications/appALD/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 17.04.08   Time: 15:15
' Created in $/CKAG/Applications/appald/Forms
' 
' *****************  Version 6  *****************
' User: Uha          Date: 20.06.07   Time: 15:25
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 5.03.07    Time: 16:59
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Forms
' 
' ************************************************
