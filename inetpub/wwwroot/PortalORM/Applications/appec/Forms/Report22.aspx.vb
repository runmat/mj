Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report22
    Inherits System.Web.UI.Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucStyles As Styles

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
            Hyperlink1.Visible = False

            DoSubmit()

            If (Not Session("lnkExcel") Is Nothing) AndAlso (Not Session("lnkExcel").ToString.Length = 0) Then
                lnkExcel.Visible = True
                lnkExcel.NavigateUrl = Session("lnkExcel").ToString
            End If



        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub


    Private Sub DoSubmit()
        Dim checkInput As Boolean = True
        Session("lnkExcel") = ""
        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New Monatsreport(m_User, m_App, strFileName)

            lblError.Text = ""

            m_Report.FILL(Session("AppID").ToString, Session.SessionID.ToString)

            Session("ResultTable") = m_Report.Result

            If Not m_Report.Status = 0 Then
                lblError.Text = "Fehler: " & m_Report.Message
            Else
                If m_Report.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else
                    exportHTML()
                    Dim objExcelExport As New Excel.ExcelExport()
                    Try
                        Excel.ExcelExport.WriteExcel(m_Report.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                    Catch
                    End Try

                    Dim AbsoluterPfadZumVirituellenVerz As String = ConfigurationManager.AppSettings("ReplaceExcelPath")
                    Session("lnkExcel") = ConfigurationManager.AppSettings("ExcelPath").Replace(AbsoluterPfadZumVirituellenVerz, "") & strFileName & "".Replace("/", "\")
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub exportHTML()
        Dim strTemplate As String
        Dim objFileInfo As System.IO.FileInfo
        Dim objStreamWriter As System.IO.StreamWriter
        Dim tblOutput As DataTable
        Dim row As DataRow
        Dim strDate As String
        Dim strTemp As String
        Dim strBuild As New System.Text.StringBuilder(String.Empty)
        Dim strFA As String = "<font size=""1"" style=""Arial""><b>"
        Dim strFZ As String = "</font></b>"

        Dim strFAA As String = "<font size=""1"" style=""Arial"">"
        Dim strFZZ As String = "</font>"
        Dim strPDIOld As String = ""

        Dim strBoldAA As String
        Dim strBoldZZ As String

        Dim strHeader As String

        Dim bbegin As Boolean = True

        tblOutput = CType(Session("ResultTable"), DataTable)
        strBuild.Append("<table cellspacing=""0"" cellpadding=""3"" border=""0""><tr>")

        'Header
        strHeader = "<td valign=""bottom"">" & strFA & "Hersteller Name" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "Gruppe" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "erfolgte Zul" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "Zul im Prozess" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "Bestand" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "Summe" & strFZ & "</td></tr>"

        strBuild.Append(strHeader)

        For Each row In tblOutput.Rows

            If row("Gruppe") = "Gesamt" Then
                strBoldAA = "<b>"
                strBoldZZ = "</b>"

            Else
                strBoldAA = ""
                strBoldZZ = ""
            End If

            strTemp = "<tr><td>" & strBoldAA & strFAA & row("Hersteller Name") & strFZZ & strBoldZZ & "</td>"
            strBuild.Append(strTemp)
            strTemp = "<td>" & strBoldAA & strFAA & row("Gruppe") & strFZZ & strBoldZZ & "</td>"
            strBuild.Append(strTemp)
            strTemp = "<td>" & strBoldAA & strFAA & row("erfolgte Zul") & strFZZ & strBoldZZ & "</td>"
            strBuild.Append(strTemp)
            strTemp = "<td>" & strBoldAA & strFAA & row("Zul im Prozess") & strFZZ & strBoldZZ & "</td>"
            strBuild.Append(strTemp)
            strTemp = "<td>" & strBoldAA & strFAA & row("Bestand") & strFZZ & strBoldZZ & "</td>"
            strBuild.Append(strTemp)
            strTemp = "<td>" & strBoldAA & strFAA & row("Summe") & strFZZ & strBoldZZ & "</td></tr>"
            strBuild.Append(strTemp)

            If row("Gruppe") = "Gesamt" Then
                strBuild.Append("<tr><td colspan=""5"">&nbsp;</td></tr>")
            End If


        Next
        strBuild.Append("</table>")

        strTemplate = "<html><head><title>Statusbericht (Druckversion)</title></head><body bgcolor=""#FFFFFF"">" & strBuild.ToString & "</body></html>"

        strDate = Date.Now.Year.ToString & Date.Now.Month.ToString & Date.Now.Day.ToString & Date.Now.Hour.ToString & Date.Now.Minute.ToString & Date.Now.Second.ToString
        objFileInfo = New System.IO.FileInfo(ConfigurationManager.AppSettings("ExcelPath") & "StatusberichtEC" & strDate & ".htm")

        objStreamWriter = objFileInfo.CreateText()
        objStreamWriter.Write(strTemplate)
        objStreamWriter.Close()

        'änderung Excel Pfade JJ2007.12.14
        Dim AbsoluterPfadZumVirituellenVerz As String = ConfigurationManager.AppSettings("ReplaceExcelPath")
        Hyperlink1.NavigateUrl = ConfigurationManager.AppSettings("ExcelPath").Replace(AbsoluterPfadZumVirituellenVerz, "") & "StatusberichtEC" & strDate & ".htm".Replace("/", "\")
        Hyperlink1.Visible = True
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report22.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 17.06.09   Time: 8:47
' Updated in $/CKAG/Applications/appec/Forms
' Warnungen entfernt!
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 29.09.08   Time: 15:17
' Updated in $/CKAG/Applications/appec/Forms
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 20.08.08   Time: 14:18
' Created in $/CKAG/Applications/appec/Forms
' ITA 2097 hinzugefügt
' 
' ************************************************
