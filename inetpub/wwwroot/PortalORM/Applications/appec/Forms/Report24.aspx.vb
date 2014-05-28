Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Partial Public Class Report24
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private errorText As String
    Private m_Report As ec_04

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


            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

            If Session("Report") Is Nothing Then
                m_Report = New ec_04(m_User, m_App, strFileName)
            Else
                m_Report = CType(Session("Report"), ec_04)
            End If

            If ddl_Hersteller.Items.Count <= 0 Then
                fillHerstellerddl()
            End If


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub fillHerstellerddl()

        If m_Report Is Nothing Then
            m_Report = CType(Session("Report"), ec_04)
        End If

        m_Report.FillHersteller(Session("AppID").ToString, Session.SessionID.ToString, Me)

        Dim row As DataRow
        row = m_Report.Result.NewRow()
        Dim item As ListItem

        'erstes item keine Auwahl/kein Value JJ2007.11.21
        item = New ListItem("keine Auswahl", "")
        ddl_Hersteller.Items.Add(item)

        For Each row In m_Report.Result.Rows
            item = New ListItem(row(1), row(0))
            ddl_Hersteller.Items.Add(item)
        Next

    End Sub

    Private Sub DoSubmit()
        Dim checkInput As Boolean = True
        Session("lnkExcel") = ""
        Try

            If m_Report Is Nothing Then
                m_Report = CType(Session("Report"), ec_04)
            End If

            lblDownloadTip.Visible = False
            lnkExcel.Visible = False


            Dim strHerstellerID As String = ""

            If ddl_Hersteller.SelectedIndex > 0 Then
                strHerstellerID = ddl_Hersteller.SelectedItem.Value.ToString()
            End If

            errorText = ""
            lblError.Text = ""

            '---------------------------------------------
            checkInput = HelpProcedures.checkDate(txt_ErfassungsdatumVon, txt_ErfassungsdatumBis, errorText, False)

            'weil checkDate leider nur Jahresbedingte Zeitspannen erlaubt, und diese auch schlecht änderbar ist, weil Sie von anderen Reports bereits verwendet wird JJ2007.11.14
            If checkInput = True Then
                If DateDiff(DateInterval.DayOfYear, CDate(txt_ErfassungsdatumVon.Text), CDate(txt_ErfassungsdatumBis.Text)) > 180 Then
                    checkInput = False
                    errorText = "Die Zeitspanne darf maximal 180 Tage betragen"
                End If
            End If
            lblError.Text = errorText

            If checkInput Then
                m_Report.FillPDIZulauf(Session("AppID").ToString, Session.SessionID.ToString, txt_ErfassungsdatumVon.Text, txt_ErfassungsdatumBis.Text, strHerstellerID, Me)

                Session("ResultTable") = m_Report.Result

                If Not m_Report.Status = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else
                    If m_Report.Result.Rows.Count = 0 Then
                        lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                    Else


                        Try
                            Excel.ExcelExport.WriteExcel(m_Report.Result, ConfigurationManager.AppSettings("ExcelPath") & m_Report.FileName)
                        Catch
                        End Try

                        Dim AbsoluterPfadZumVirituellenVerz As String = ConfigurationManager.AppSettings("ReplaceExcelPath")
                        Session("lnkExcel") = ConfigurationManager.AppSettings("ExcelPath").Replace(AbsoluterPfadZumVirituellenVerz, "") & m_Report.FileName & "".Replace("/", "\")
                        lblDownloadTip.Visible = True
                        lnkExcel.Visible = True
                        lnkExcel.NavigateUrl = Session("lnkExcel").ToString

                        exportHTML()

                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub btnVon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVon.Click
        calVon.Visible = True
        calBis.Visible = False
    End Sub



    Private Sub calVon_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        txt_ErfassungsdatumVon.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    Private Sub calBis_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBis.SelectionChanged
        txt_ErfassungsdatumBis.Text = calBis.SelectedDate.ToShortDateString
        calBis.Visible = False
    End Sub



    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub lb_Bis_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lb_Bis.Click
        calBis.Visible = True
        calVon.Visible = False
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
        strHeader = "<td valign=""bottom"">" & strFA & "PDI Nr" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "PDI Name" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "FZG Art" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "SIPP Code" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "Hersteller" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "Model ID" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "Model Bez." & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "Anzahl" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "Zulauf von" & strFZ & "</td>" & _
                    "<td valign=""bottom"">" & strFA & "Zulauf bis" & strFZ & "</td></tr>"
                    
        strBuild.Append(strHeader)

        Dim LastRowIndex As Integer = tblOutput.Rows.Count
        Dim RowCount As Integer = 1
        For Each row In tblOutput.Rows


            If Not RowCount = LastRowIndex Then
                'Einträge  
                If (TypeOf row("PDI Nr") Is System.DBNull) Then
                    ' strBuild.Append(strHeader)
                Else
                    If bbegin = True Then
                        strPDIOld = row("PDI Nr")
                    End If

                    row("PDI Name") = row("PDI Name").ToString.Replace("ä", "ae").Replace("ö", "oe").Replace("ü", "ue").Replace("Ö", "OE").Replace("Ä", "AE").Replace("Ü", "UE")


                    If Not (strPDIOld.Equals(row("PDI Nr")) = True) Then
                        strTemp = " <tr><td colspan=""10""><hr></td></tr>"
                        strBuild.Append(strTemp)


                        If "GESAMT".Equals(row("PDI Nr")) Then
                            strTemp = " <tr><td colspan=""10""><hr></td></tr>"
                            strBuild.Append(strTemp)
                        End If

                        strBuild.Append(strHeader)

                    End If


                    If (row("PDI Nr") = "----------") Then
                        row("PDI Nr") = String.Empty
                        row("PDI Name") = String.Empty
                        row("Hersteller") = String.Empty
                        row("Model ID") = String.Empty
                        row("Model Bez") = String.Empty
                    End If


                    If String.Empty.Equals(row("FZG Art")) = True Or String.Empty.Equals(row("SIPP Code")) = True Then
                        strBoldAA = "<b>"
                        strBoldZZ = "</b>"
                    Else
                        strBoldAA = String.Empty
                        strBoldZZ = String.Empty
                    End If




                    strTemp = "<tr><td>" & strBoldAA & strFAA & row("PDI Nr") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("PDI Name") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("FZG Art") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("SIPP Code") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("Hersteller") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("Model ID") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("Model Bez") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("Anzahl") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("Zulauf von") & strFZZ & strBoldZZ & "</td>"
                    strBuild.Append(strTemp)
                    strTemp = "<td>" & strBoldAA & strFAA & row("Zulauf bis") & strFZZ & strBoldZZ & "</td></tr>"
                    strBuild.Append(strTemp)
                    
                    bbegin = False
                    strPDIOld = row("PDI Nr")
                End If
            Else

                strTemp = " <tr><td colspan=""10""><hr></td></tr>"
                strBuild.Append(strTemp)

                strBoldAA = "<b>"
                strBoldZZ = "</b>"
                strTemp = "<tr><td>" & strBoldAA & strFAA & row("PDI Nr") & strFZZ & strBoldZZ & "</td>"
                strBuild.Append(strTemp)
                strTemp = "<td>" & strBoldAA & strFAA & row("PDI Name") & strFZZ & strBoldZZ & "</td>"
                strBuild.Append(strTemp)
                strTemp = "<td>" & strBoldAA & strFAA & row("FZG Art") & strFZZ & strBoldZZ & "</td>"
                strBuild.Append(strTemp)
                strTemp = "<td>" & strBoldAA & strFAA & row("SIPP Code") & strFZZ & strBoldZZ & "</td>"
                strBuild.Append(strTemp)
                strTemp = "<td>" & strBoldAA & strFAA & row("Hersteller") & strFZZ & strBoldZZ & "</td>"
                strBuild.Append(strTemp)
                strTemp = "<td>" & strBoldAA & strFAA & row("Model ID") & strFZZ & strBoldZZ & "</td>"
                strBuild.Append(strTemp)
                strTemp = "<td>" & strBoldAA & strFAA & row("Model Bez") & strFZZ & strBoldZZ & "</td>"
                strBuild.Append(strTemp)
                strTemp = "<td>" & strBoldAA & strFAA & row("Anzahl") & strFZZ & strBoldZZ & "</td>"
                strBuild.Append(strTemp)
                strTemp = "<td>" & strBoldAA & strFAA & row("Zulauf von") & strFZZ & strBoldZZ & "</td>"
                strBuild.Append(strTemp)
                strTemp = "<td>" & strBoldAA & strFAA & row("Zulauf bis") & strFZZ & strBoldZZ & "</td></tr>"
                strBuild.Append(strTemp)



                'strTemp = " <tr><td colspan=""10""><hr></td></tr>"
                'strBuild.Append(strTemp)
                'strTemp = " <tr><td style=""font-size:10; font-weight:bold"">" & row("PDI Nr").ToString & ":</td><td colspan=""17"" style=""font-size:10; font-weight:bold"">" & row("PDI Name").ToString & "</td> </tr>"
                'strBuild.Append(strTemp)
            End If
            RowCount += 1
        Next
        strBuild.Append("</table>")

        strTemplate = "<html><head><title>PDI Zul&auml;ufe (Druckversion)</title></head><body bgcolor=""#FFFFFF"">" & strBuild.ToString & "</body></html>"

        strDate = Date.Now.Year.ToString & Date.Now.Month.ToString & Date.Now.Day.ToString & Date.Now.Hour.ToString & Date.Now.Minute.ToString & Date.Now.Second.ToString
        objFileInfo = New System.IO.FileInfo(ConfigurationManager.AppSettings("ExcelPath") & "PdiZulaufEC" & strDate & ".htm")

        objStreamWriter = objFileInfo.CreateText()
        objStreamWriter.Write(strTemplate)
        objStreamWriter.Close()

        'änderung Excel Pfade JJ2007.12.14
        Dim AbsoluterPfadZumVirituellenVerz As String = ConfigurationManager.AppSettings("ReplaceExcelPath")
        Hyperlink1.NavigateUrl = ConfigurationManager.AppSettings("ExcelPath").Replace(AbsoluterPfadZumVirituellenVerz, "") & "PdiZulaufEC" & strDate & ".htm".Replace("/", "\")
        Hyperlink1.Visible = True
    End Sub









End Class

' ************************************************
' $History: Report24.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 1.03.11    Time: 11:39
' Created in $/CKAG/Applications/appec/Forms
' 