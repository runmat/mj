Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report07
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
            Dim m_Report As New ec_07(m_User, m_App, strFileName)

            lblError.Text = ""
            m_Report.GetCountBrief(Session("AppID").ToString, Session.SessionID.ToString, Me)

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
                    "<td valign=""bottom"">" & strFA & "SIPP <br>Cd." & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "Hersteller Name" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "Model ID" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "Modellbezeichnung" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "Eing.<br>ges." & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "aus<br>Vorjahr" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "Zul.<br>Vorm." & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "Lfd.<br> Monat" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "Zul.<br> ges." & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "Proz.<br>lfd. Monat" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "Proz.<br>folg. Monat" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "Bestand" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "ausge-<br>ruestet" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "mit<br>Brief" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "zul.-<br>bereit" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "ohne<br>Unitnr." & strFZ & "</td></tr>"
        strBuild.Append(strHeader)

        Dim LastRowIndex As Integer = tblOutput.Rows.Count
        Dim RowCount As Integer = 1
        For Each row In tblOutput.Rows

            'die beiden letzten Zeilen gesondert verarbeiten
            If RowCount < (LastRowIndex - 1) Then
                'Einträge  
                If (TypeOf row("PDI") Is System.DBNull) Then
                    ' strBuild.Append(strHeader)
                Else
                    If bbegin = True Then
                        strPDIOld = row("PDI")
                    End If

                    row("PDI Name") = row("PDI Name").ToString.Replace("ä", "ae").Replace("ö", "oe").Replace("ü", "ue").Replace("Ö", "OE").Replace("Ä", "AE").Replace("Ü", "UE")


                    If Not (strPDIOld.Equals(row("PDI")) = True) Then
                        strTemp = " <tr><td colspan=""19""><hr></td></tr>"
                        strBuild.Append(strTemp)


                        If "GESAMT".Equals(row("PDI")) Then
                            strTemp = " <tr><td colspan=""19""><hr></td></tr>"
                            strBuild.Append(strTemp)
                        End If

                        strBuild.Append(strHeader)

                    End If


                    If (row("PDI") = "----------") Then
                        row("PDI") = String.Empty
                        row("PDI Name") = String.Empty
                        row("Hersteller Name") = String.Empty
                        row("Model ID") = String.Empty
                        row("Modellbezeichnung") = String.Empty
                        'strTemp = "<tr><td>" & strFAA & row("PDI") & strFZZ & "</td>"
                        'strBoldAA = "<b>"
                        'strBoldZZ = "</b>"
                        'Else
                        '    strTemp = "<tr><td>" & strFAA & row("PDI") & strFZZ & "</td>"
                        '    strBoldAA = String.Empty
                        '    strBoldZZ = String.Empty
                    End If


                    If String.Empty.Equals(row("Fahrzeugart")) = True Or String.Empty.Equals(row("SIPP Code")) = True Then
                        strBoldAA = "<b>"
                        strBoldZZ = "</b>"
                    Else
                        strBoldAA = String.Empty
                        strBoldZZ = String.Empty
                    End If




                    strTemp = "<tr><td>" & strBoldAA & strFAA & row("PDI") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("PDI Name") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("Fahrzeugart") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("SIPP Code") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("Hersteller Name") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("Model ID") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("Modellbezeichnung") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("Eing ges") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("aus Vorjahr") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("Zul Vorm") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("Lfd Monat") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("Zul ges") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("Proz lfd Monat") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("Proz folg Monat") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("Bestand") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("ausgerüstet") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("mit Brief") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("zul-bereit") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("ohne Unitnr") & strFZZ & strBoldZZ & "</td></tr>"
                    strBuild.Append(strTemp)
                    ''Trennstrich einfügen...
                    'If (row("PDI") = String.Empty) Then
                    '    strBuild.Append("<tr><td colspan=""18""><hr></td></tr>")
                    'End If
                    bbegin = False
                    strPDIOld = row("PDI")
                End If
            Else
                'zwischen den einzelnen Summenzeilen keine Trennstriche, nur darüber einen
                If Not RowCount = LastRowIndex Then
                    strTemp = " <tr><td colspan=""19""><hr></td></tr>"
                    strBuild.Append(strTemp)
                End If
                strTemp = " <tr><td style=""font-size:10; font-weight:bold"">" & row("PDI").ToString & ":</td><td colspan=""17"" style=""font-size:10; font-weight:bold"">" & row("PDI Name").ToString & "</td> </tr>"
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
' $History: Report07.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 10.03.11   Time: 11:37
' Updated in $/CKAG/Applications/appec/Forms
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 9.02.11    Time: 14:49
' Updated in $/CKAG/Applications/appec/Forms
' 219853
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
' *****************  Version 13  *****************
' User: Jungj        Date: 14.12.07   Time: 13:45
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Anpassung Excel Links, wegen Webconfig Änderung, jetzt Variabel ab
' Virtuellem Verzeichnis
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 1.11.07    Time: 8:13
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 31.10.07   Time: 17:35
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 23.10.07   Time: 9:43
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 9  *****************
' User: Fassbenders  Date: 16.10.07   Time: 14:50
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 8  *****************
' User: Uha          Date: 2.07.07    Time: 12:29
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 7  *****************
' User: Uha          Date: 22.05.07   Time: 13:31
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 6  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' ************************************************
