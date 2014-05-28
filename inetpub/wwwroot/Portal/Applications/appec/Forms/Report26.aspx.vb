Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report26
    Inherits System.Web.UI.Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
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
                lblDownloadTip.Visible = True
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
            Dim m_Report As New ec_26(m_User, m_App, strFileName)

            lblError.Text = ""

            m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)

            Session("ResultTable") = m_Report.Result

            If Not m_Report.Status = 0 Then
                lblError.Text = "Fehler: " & m_Report.Message
            Else
                If m_Report.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else
                    exportHTML()
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
        strBuild.Append("<table cellspacing=""0"" cellpadding=""3"" border=""0"">")

        strBuild.Append("<tr><td colspan=""19"" align=""center""><img alt="""" src=""/Portal/Images/dadlogo.jpg"" />" & _
                        "</td></tr><tr><td colspan=""19"">&nbsp;</td></tr>")

        'Header
        strHeader = "<tr><td valign=""bottom"">" & strFA & "PDI" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "PDI Name" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "Fahrzeugart" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "Hersteller Name" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "Eing. ges." & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "aus<br>Vorjahr" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "Zul.<br> ges." & strFZ & "</td></tr>"
        strBuild.Append(strHeader)

        Dim LastRowIndex As Integer = tblOutput.Rows.Count
        Dim RowCount As Integer = 1
        For Each row In tblOutput.Rows


            If Not RowCount = LastRowIndex Then
                'Einträge  
                If (TypeOf row("PDI") Is System.DBNull) Then
                    ' strBuild.Append(strHeader)
                Else
                    If bbegin = True Then
                        strPDIOld = row("PDI")
                    End If

                    row("PDI") = row("PDI").ToString.Replace("ä", "ae").Replace("ö", "oe").Replace("ü", "ue").Replace("Ö", "OE").Replace("Ä", "AE").Replace("Ü", "UE")

                    


                    strTemp = "<tr><td>" & strBoldAA & strFAA & row("PDI") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("PDI Name") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("Fahrzeugart") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("Hersteller Name") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("Eing ges") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("aus Vorjahr") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("Zul ges") & strFZZ & strBoldZZ & "</td></tr>"
                    strBuild.Append(strTemp)
                    
                    bbegin = False
                    strPDIOld = row("PDI")
                End If
            Else
                strTemp = " <tr><td colspan=""18""><hr></td></tr>"
                strBuild.Append(strTemp)
                strTemp = " <tr><td style=""font-size:10; font-weight:bold"">" & row("PDI").ToString & ":</td><td colspan=""17"" style=""font-size:10; font-weight:bold"">" & row("PDI").ToString & "</td> </tr>"
                strBuild.Append(strTemp)
            End If
            RowCount += 1
        Next
        strBuild.Append("</table>")

        strTemplate = "<html><head><title>Statusbericht (Druckversion)</title></head><body bgcolor=""#FFFFFF"">" & strBuild.ToString & "</body></html>"

        strDate = Date.Now.Year.ToString & Date.Now.Month.ToString & Date.Now.Day.ToString & Date.Now.Hour.ToString & Date.Now.Minute.ToString & Date.Now.Second.ToString
        objFileInfo = New System.IO.FileInfo(ConfigurationManager.AppSettings("ExcelPath") & "StatusberichtEC" & strDate & ".htm")

        objStreamWriter = objFileInfo.CreateText()
        objStreamWriter.Write(strTemplate)
        objStreamWriter.Close()

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

