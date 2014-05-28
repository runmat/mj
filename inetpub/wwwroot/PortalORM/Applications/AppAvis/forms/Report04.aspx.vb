Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System.IO

Public Class Report04
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
    Private m_blnUnvollstaendigeTuete As Boolean


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

            If (Not Request.QueryString("Ges") Is Nothing) AndAlso Request.QueryString("Ges").ToString = "1" Then
                m_blnUnvollstaendigeTuete = False
            Else
                m_blnUnvollstaendigeTuete = True
            End If


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
    End Sub


    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Response.Redirect("/PortalORM/(S(" & Session.SessionID & "))/Start/Selection.aspx")
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "False"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New Avis04(m_User, m_App, strFileName)

            lblError.Text = ""

            If lblError.Text.Length = 0 Then
                m_Report.UnvollstaendigeTuete = m_blnUnvollstaendigeTuete
                m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString)

                Session("ResultTable") = m_Report.Result
                Session("UnvollstaendigeTuete") = m_Report.UnvollstaendigeTuete

                If Not m_Report.Status = 0 Then
                    lblError.Text = "Fehler: " & m_Report.Message
                Else
                    If m_Report.Result.Rows.Count = 0 Then
                        lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                    Else

                        '§§§ JVE 06.09.2006
                        Dim tmpTbl As DataTable = m_Report.Result.Copy

                        tmpTbl.Columns.Remove("Equipmentnummer")
                        tmpTbl.AcceptChanges()
                        '-------------------

                        Dim destFile = Path.Combine(ConfigurationManager.AppSettings("ExcelPath"), strFileName)

                        Try
                            Excel.ExcelExport.WriteExcel(tmpTbl, destFile)
                        Catch ex As Exception
                        End Try

                        Dim root = Server.MapPath("~/")
                        Session("lnkExcel") = destFile.Replace(root, "~/").Replace("\", "/")
                        Response.Redirect("Report04_2.aspx?AppID=" & Session("AppID").ToString)
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten."
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report04.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 3.12.08    Time: 10:46
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA 2419 testfertig
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 25.11.08   Time: 15:20
' Updated in $/CKAG/Applications/AppAvis/forms
' ************************************************