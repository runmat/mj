Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report22
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
    Private objPDIs As SIXT_PDI

    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents txtPDINummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSearch.Visible = True
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            cmdSearch.Enabled = True
            ImageButton1.Enabled = True
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        DoSubmit()
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        Dim strFileNameDetail As String = "Detail" & Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        objPDIs = New SIXT_PDI(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName, m_User.KUNNR, "ZDAD", txtPDINummer.Text.Trim(" "c), "Zulassen")

        objPDIs.ShowPDIsAll(Session("AppID").ToString, Session.SessionID.ToString, Me)
        If objPDIs.Status = 0 Then
            If objPDIs.PDI_Data.Tables("PDIs").Rows.Count = 0 Then
                lblError.Text = "Keine Daten zur Anzeige gefunden."
            Else
                Dim k As Int32

                objPDIs.ShowAll()

                Dim intCount As Int32 = 0
                If Not objPDIs.Status = 0 Then
                    lblError.Text = "Fehler beim Lesen der Daten.<br>(" & objPDIs.Message & ")"
                Else
                    If 1 = 0 Then 'objPDIs.PDI_Data.Tables("Modelle").Rows.Count = 0 Then
                        lblError.Text = "Keine Daten zur Anzeige gefunden."
                    Else
                        Dim tblResult As New DataTable()
                        tblResult.Columns.Add("PDINummer", System.Type.GetType("System.String"))
                        tblResult.Columns.Add("PDIName", System.Type.GetType("System.String"))
                        tblResult.Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                        tblResult.Columns.Add("Hersteller", System.Type.GetType("System.String"))
                        tblResult.Columns.Add("Modell", System.Type.GetType("System.String"))
                        tblResult.Columns.Add("Schalt", System.Type.GetType("System.String"))
                        tblResult.Columns.Add("Ausf", System.Type.GetType("System.String"))
                        tblResult.Columns.Add("Antr", System.Type.GetType("System.String"))
                        tblResult.Columns.Add("Reifen", System.Type.GetType("System.String"))
                        tblResult.Columns.Add("Navi", System.Type.GetType("System.String"))
                        tblResult.Columns.Add("Bekleb", System.Type.GetType("System.String"))
                        'tblResult.Columns.Add("VM", System.Type.GetType("System.String"))
                        tblResult.Columns.Add("Limo", System.Type.GetType("System.String"))
                        tblResult.Columns.Add("Anzahl", System.Type.GetType("System.Int32"))
                        tblResult.Columns.Add("Bemerkungsdatum", System.Type.GetType("System.String"))

                        Dim rowNew As DataRow

                        For k = 0 To objPDIs.PDI_Data.Tables("PDIs").Rows.Count - 1
                            rowNew = tblResult.NewRow

                            rowNew("PDINummer") = CStr(objPDIs.PDI_Data.Tables("PDIs").Rows(k)("Kunden_PDI_Nummer"))
                            rowNew("PDIName") = CStr(objPDIs.PDI_Data.Tables("PDIs").Rows(k)("PDI_Name"))
                            rowNew("Fahrgestellnummer") = CStr(objPDIs.PDI_Data.Tables("PDIs").Rows(k)("Fahrgestellnr"))
                            rowNew("Hersteller") = CStr(objPDIs.PDI_Data.Tables("PDIs").Rows(k)("Hersteller"))
                            rowNew("Modell") = CStr(objPDIs.PDI_Data.Tables("PDIs").Rows(k)("Modellbezeichnung"))
                            rowNew("Schalt") = CStr(objPDIs.PDI_Data.Tables("PDIs").Rows(k)("Schaltung"))
                            rowNew("Ausf") = CStr(objPDIs.PDI_Data.Tables("PDIs").Rows(k)("Ausfuehrung"))
                            rowNew("Antr") = CStr(objPDIs.PDI_Data.Tables("PDIs").Rows(k)("Antrieb"))
                            rowNew("Reifen") = CStr(objPDIs.PDI_Data.Tables("PDIs").Rows(k)("Bereifung"))
                            rowNew("Navi") = CStr(objPDIs.PDI_Data.Tables("PDIs").Rows(k)("Navigation"))
                            rowNew("Bekleb") = CStr(objPDIs.PDI_Data.Tables("PDIs").Rows(k)("Beklebung"))

                            'If CType(objPDIs.PDI_Data.Tables("PDIs").Rows(k)("VM"), Boolean) Then
                            '    rowNew("VM") = "Y"
                            'Else
                            '    rowNew("VM") = "N"
                            'End If
                            If CType(objPDIs.PDI_Data.Tables("PDIs").Rows(k)("Limo"), Boolean) Then
                                rowNew("Limo") = "Y"
                            Else
                                rowNew("Limo") = "N"
                            End If
                            If Not objPDIs.PDI_Data.Tables("PDIs").Rows(k)("Bemerkungsdatum") Is DBNull.Value Then
                                rowNew("Bemerkungsdatum") = objPDIs.PDI_Data.Tables("PDIs").Rows(k)("Bemerkungsdatum").ToString
                            Else
                                rowNew("Bemerkungsdatum") = String.Empty
                            End If



                            tblResult.Rows.Add(rowNew)
                            tblResult.AcceptChanges()
                        Next

                        Dim tableResult As New DataTable()
                        Dim row As DataRow
                        Dim rows As DataRow()
                        Dim rowsNew As DataRow()
                        Dim countAll As Integer = 0
                        Dim tblResultDetail As New DataTable() 'Tabelle mit Bemerkungsdatum

                        tableResult = tblResult.Copy    'Tabelle kopieren 

                        tblResultDetail = tableResult.Copy

                        'Anzahl aus der Detailtabelle entfernen
                        tblResultDetail.Columns.Remove("Anzahl")

                        Session("ResultDetailTable") = tblResultDetail

                        'Bemerkungsdatum wird in der kumulierten Ansicht nicht benötigt.
                        tableResult.Columns.Remove("Bemerkungsdatum")
                        tableResult.Columns.Remove("Fahrgestellnummer")

                        tableResult.Clear()

                        For Each row In tblResult.Rows
                            'rows = tblResult.Select("PDINummer='" & row("PDINummer") & "' AND PDIName='" & row("PDIName") & "' AND Hersteller='" & row("Hersteller") & "' AND Modell='" & row("Modell") & "' AND Schalt='" & row("Schalt") & "' AND Ausf='" & row("Ausf") & "' AND Antr='" & row("Antr") & "' AND Reifen='" & row("Reifen") & "' AND Navi='" & row("Navi") & "' AND Bekleb='" & row("Bekleb") & "' AND VM='" & row("VM") & "' AND Limo='" & row("Limo") & "'")
                            'rowsNew = tableResult.Select("PDINummer='" & row("PDINummer") & "' AND PDIName='" & row("PDIName") & "' AND Hersteller='" & row("Hersteller") & "' AND Modell='" & row("Modell") & "' AND Schalt='" & row("Schalt") & "' AND Ausf='" & row("Ausf") & "' AND Antr='" & row("Antr") & "' AND Reifen='" & row("Reifen") & "' AND Navi='" & row("Navi") & "' AND Bekleb='" & row("Bekleb") & "' AND VM='" & row("VM") & "' AND Limo='" & row("Limo") & "'")

                            rows = tblResult.Select("PDINummer='" & row("PDINummer") & "' AND PDIName='" & row("PDIName") & "' AND Hersteller='" & row("Hersteller") & "' AND Modell='" & row("Modell") & "' AND Schalt='" & row("Schalt") & "' AND Ausf='" & row("Ausf") & "' AND Antr='" & row("Antr") & "' AND Reifen='" & row("Reifen") & "' AND Navi='" & row("Navi") & "' AND Bekleb='" & row("Bekleb") & "' AND Limo='" & row("Limo") & "'")
                            rowsNew = tableResult.Select("PDINummer='" & row("PDINummer") & "' AND PDIName='" & row("PDIName") & "' AND Hersteller='" & row("Hersteller") & "' AND Modell='" & row("Modell") & "' AND Schalt='" & row("Schalt") & "' AND Ausf='" & row("Ausf") & "' AND Antr='" & row("Antr") & "' AND Reifen='" & row("Reifen") & "' AND Navi='" & row("Navi") & "' AND Bekleb='" & row("Bekleb") & "' AND Limo='" & row("Limo") & "'")

                            If rowsNew.Length = 0 Then

                                If ((txtPDINummer.Text = String.Empty) Or (row("PDINUmmer").ToString = txtPDINummer.Text)) Then 'PDI Filtern
                                    rowNew = tableResult.NewRow()
                                    rowNew("PDINummer") = row("PDINummer")
                                    rowNew("PDIName") = row("PDIName")
                                    rowNew("Hersteller") = row("Hersteller")
                                    rowNew("Modell") = row("Modell")
                                    rowNew("Schalt") = row("Schalt")
                                    rowNew("Ausf") = row("Ausf")
                                    rowNew("Antr") = row("Antr")
                                    rowNew("Reifen") = row("Reifen")
                                    rowNew("Navi") = row("Navi")
                                    rowNew("Bekleb") = row("Bekleb")
                                    'rowNew("VM") = row("VM")
                                    rowNew("Limo") = row("Limo")
                                    rowNew("Anzahl") = rows.Length

                                    countAll += rows.Length

                                    tableResult.Rows.Add(rowNew)
                                    tableResult.AcceptChanges()
                                End If
                            End If
                        Next

                        Session("ResultTable") = tableResult
                        Session("ShowOtherString") = "Es wurden " & countAll & " ""Zulassungsfähige Fahrzeuge"" gefunden."

                        Try
                            Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                            Session("lnkExcelDetail") = "/Portal/Temp/Excel/" & strFileNameDetail
                            Excel.ExcelExport.WriteExcel(tableResult, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                            Excel.ExcelExport.WriteExcel(tblResultDetail, ConfigurationManager.AppSettings("ExcelPath") & strFileNameDetail)
                        Catch
                        End Try

                        Response.Redirect("Report22_2.aspx?AppID=" & Session("AppID").ToString)
                    End If
                End If
            End If
        Else
            lblError.Text = "Keine Daten zur Anzeige gefunden."
        End If
    End Sub

    Private Function MakeStandardDateString(ByVal strSAPDate As String) As String
        If strSAPDate = "00000000" Then
            Return ""
        Else
            Return Right(strSAPDate, 2) & "." & Mid(strSAPDate, 5, 2) & "." & Left(strSAPDate, 4)
        End If
    End Function

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
' User: Rudolpho     Date: 16.02.10   Time: 11:40
' Updated in $/CKAG/Applications/appsixt/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 10:16
' Updated in $/CKAG/Applications/appsixt/Forms
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:37
' Created in $/CKAG/Applications/appsixt/Forms
' 
' *****************  Version 14  *****************
' User: Uha          Date: 2.07.07    Time: 14:09
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 13  *****************
' User: Uha          Date: 22.05.07   Time: 11:27
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 12  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' 
' ************************************************
