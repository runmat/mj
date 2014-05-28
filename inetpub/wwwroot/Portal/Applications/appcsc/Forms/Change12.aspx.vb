Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change12
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
    Private objFahrzeuge As CSC_Sperrliste

    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    'Protected WithEvents chk084 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chk083 As System.Web.UI.WebControls.RadioButton
    'Protected WithEvents chk082 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chk081 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chk080 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents lbl080 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl081 As System.Web.UI.WebControls.Label
    'Protected WithEvents lbl082 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl083 As System.Web.UI.WebControls.Label
    'Protected WithEvents lbl084 As System.Web.UI.WebControls.Label
    Protected WithEvents chk085 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents lbl085 As System.Web.UI.WebControls.Label
    Protected WithEvents chk999 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents lbl999 As System.Web.UI.WebControls.Label
    Protected WithEvents ShowUnknown As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ShowUnused As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblFilename As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    'Protected WithEvents ShowCalendar As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents ShowVon As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents ShowBis As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents calAbDatum As System.Web.UI.WebControls.Calendar
    'Protected WithEvents calBisDatum As System.Web.UI.WebControls.Calendar
    'Protected WithEvents txtAbDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnOpenSelectAb As System.Web.UI.WebControls.Button
    'Protected WithEvents txtBisDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnOpenSelectBis As System.Web.UI.WebControls.Button
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSearch.Visible = True
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        Try
            ShowUnused.Visible = False
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                DoSubmit0()
                'ShowBis.Visible = False
                'ShowVon.Visible = False
                'ShowCalendar.Visible = False
            Else
                objFahrzeuge = CType(Session("objFahrzeuge"), CSC_Sperrliste)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    'Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
    '    DoSubmit()
    'End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub

    Private Sub PreFill()
        lblFilename.Text = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        objFahrzeuge = New CSC_Sperrliste(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, lblFilename.Text)
        objFahrzeuge.IstNullliste = True
        objFahrzeuge.Customer = m_User.KUNNR
        objFahrzeuge.Show(Session("AppID").ToString, Session.SessionID, Me)
    End Sub

    Private Sub DoSubmit0()
        lblError.Text = ""
        lblError.Visible = False
        PreFill()
        If Not objFahrzeuge.Status = 0 Then
            lblError.Text = objFahrzeuge.Message
            lblError.Visible = True
            ShowUnknown.Visible = False
            chk080.Enabled = False
            chk081.Enabled = False
            'chk082.Enabled = False
            chk083.Enabled = False
            'chk084.Enabled = False
            chk085.Enabled = False
            chk999.Enabled = False
            lbl080.Text = "0"
            lbl081.Text = "0"
            'lbl082.Text = "0"
            lbl083.Text = "0"
            'lbl084.Text = "0"
            lbl085.Text = "0"
            lbl999.Text = "0"
        Else
            If objFahrzeuge.Fahrzeuge.Rows.Count = 0 Then
                lblError.Text = "Keine Einträge in der Liste."
            Else
                Dim vwTemp As DataView = objFahrzeuge.Fahrzeuge.DefaultView
                vwTemp.RowFilter = "ProblemCode = '080'"
                lbl080.Text = CStr(vwTemp.Count)
                If vwTemp.Count = 0 Then
                    chk080.Enabled = False
                End If
                vwTemp.RowFilter = "ProblemCode = '081'"
                lbl081.Text = CStr(vwTemp.Count)
                If vwTemp.Count = 0 Then
                    chk081.Enabled = False
                End If
                'vwTemp.RowFilter = "ProblemCode = '082'"
                'lbl082.Text = CStr(vwTemp.Count)
                'If vwTemp.Count = 0 Then
                'chk082.Enabled = False
                'End If
                vwTemp.RowFilter = "ProblemCode = '083'"
                lbl083.Text = CStr(vwTemp.Count)
                If vwTemp.Count = 0 Then
                    chk083.Enabled = False
                End If
                'vwTemp.RowFilter = "ProblemCode = '084'"
                'lbl084.Text = CStr(vwTemp.Count)
                'If vwTemp.Count = 0 Then
                '    chk084.Enabled = False
                'End If
                vwTemp.RowFilter = "ProblemCode = '085'"
                lbl085.Text = CStr(vwTemp.Count)
                If vwTemp.Count = 0 Then
                    chk085.Enabled = False
                End If
                vwTemp.RowFilter = "ProblemCode = '999'"
                lbl999.Text = CStr(vwTemp.Count)
                If vwTemp.Count = 0 Then
                    ShowUnknown.Visible = False
                    chk999.Enabled = False
                End If
                vwTemp.RowFilter = ""
            End If
        End If
        Session("objFahrzeuge") = objFahrzeuge
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Dim strTemp As String = ""
        If chk080.Checked Then strTemp = "080"
        If chk081.Checked Then strTemp = "081"
        'If chk082.Checked Then strTemp = "082"
        If chk083.Checked Then strTemp = "083"
        'If chk084.Checked Then strTemp = "084"
        If chk085.Checked Then strTemp = "085"
        If chk999.Checked Then strTemp = "999"

        'If strTemp = "082" Then
        '    If Not IsDate(txtAbDatum.Text) Then
        '        lblError.Text = "Kein gültiges Startdatum eingetragen."
        '        Exit Sub
        '    End If
        '    If Not IsDate(txtBisDatum.Text) Then
        '        lblError.Text = "Kein gültiges Enddatum eingetragen."
        '        Exit Sub
        '    End If
        '    If CDate(txtAbDatum.Text) > CDate(txtBisDatum.Text) Then
        '        lblError.Text = "Startdatum muss vor Enddatum liegen."
        '        Exit Sub
        '    End If
        'End If
        If strTemp.Length = 0 Then
            lblError.Text = "Kein Kriterium ausgewählt."
        Else
            If objFahrzeuge.Fahrzeuge.Rows.Count = 0 Then
                PreFill()
            End If
            Dim i As Int32
            For i = objFahrzeuge.Fahrzeuge.Rows.Count - 1 To 0 Step -1
                If Not CStr(objFahrzeuge.Fahrzeuge.Rows(i)("ProblemCode")) = strTemp Then
                    objFahrzeuge.Fahrzeuge.AcceptChanges()
                    objFahrzeuge.Fahrzeuge.Rows(i).Delete()
                    objFahrzeuge.Fahrzeuge.AcceptChanges()
                End If
            Next
            'If strTemp = "082" Then
            '    For i = objFahrzeuge.Fahrzeuge.Rows.Count - 1 To 0 Step -1
            '        If Not IsDate(objFahrzeuge.Fahrzeuge.Rows(i)("Datum_Nullliste")) Then
            '            objFahrzeuge.Fahrzeuge.AcceptChanges()
            '            objFahrzeuge.Fahrzeuge.Rows(i).Delete()
            '            objFahrzeuge.Fahrzeuge.AcceptChanges()
            '        Else
            '            If CDate(objFahrzeuge.Fahrzeuge.Rows(i)("Datum_Nullliste")) < CDate(txtAbDatum.Text) _
            '              Or CDate(objFahrzeuge.Fahrzeuge.Rows(i)("Datum_Nullliste")) > CDate(txtBisDatum.Text) Then
            '                objFahrzeuge.Fahrzeuge.AcceptChanges()
            '                objFahrzeuge.Fahrzeuge.Rows(i).Delete()
            '                objFahrzeuge.Fahrzeuge.AcceptChanges()
            '            End If
            '        End If
            '    Next
            'End If
            If objFahrzeuge.Fahrzeuge.Rows.Count = 0 Then
                lblError.Text = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
            Else
                Dim tblTemp As DataTable
                tblTemp = New DataTable()
                tblTemp.Columns.Add("Kontonummer", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Briefnummer", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Kennzeichen", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Label", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Modellbezeichnung", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Datum Nullliste", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Datum Briefeingang", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Datum Versand", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Problem", System.Type.GetType("System.String"))
                Dim rowNew As DataRow
                For i = 0 To objFahrzeuge.Fahrzeuge.Rows.Count - 1
                    rowNew = tblTemp.NewRow
                    rowNew("Kontonummer") = objFahrzeuge.Fahrzeuge.Rows(i)("Kontonummer")
                    rowNew("Fahrgestellnummer") = objFahrzeuge.Fahrzeuge.Rows(i)("Fahrgestellnummer")
                    rowNew("Briefnummer") = objFahrzeuge.Fahrzeuge.Rows(i)("Briefnummer")
                    rowNew("Kennzeichen") = objFahrzeuge.Fahrzeuge.Rows(i)("Kennzeichen")
                    rowNew("Label") = objFahrzeuge.Fahrzeuge.Rows(i)("Label")
                    rowNew("Modellbezeichnung") = objFahrzeuge.Fahrzeuge.Rows(i)("Modellbezeichnung")
                    If IsDate(objFahrzeuge.Fahrzeuge.Rows(i)("Datum_Nullliste")) Then
                        rowNew("Datum Nullliste") = CType(objFahrzeuge.Fahrzeuge.Rows(i)("Datum_Nullliste"), DateTime).ToShortDateString
                    End If
                    If IsDate(objFahrzeuge.Fahrzeuge.Rows(i)("Datum_Briefeingang")) Then
                        rowNew("Datum Briefeingang") = CType(objFahrzeuge.Fahrzeuge.Rows(i)("Datum_Briefeingang"), DateTime).ToShortDateString
                    End If
                    If IsDate(objFahrzeuge.Fahrzeuge.Rows(i)("Datum_Versand")) Then
                        rowNew("Datum Versand") = CType(objFahrzeuge.Fahrzeuge.Rows(i)("Datum_Versand"), DateTime).ToShortDateString
                    End If
                    rowNew("Problem") = objFahrzeuge.Fahrzeuge.Rows(i)("Problem")
                    tblTemp.Rows.Add(rowNew)
                Next

                Dim objExcelExport As New Excel.ExcelExport()
                Try
                    Excel.ExcelExport.WriteExcel(tblTemp, ConfigurationManager.AppSettings("ExcelPath") & lblFilename.Text)
                    Session("lnkExcel") = "/Portal/Temp/Excel/" & lblFilename.Text
                Catch
                End Try
                Session("objFahrzeuge") = objFahrzeuge
                Response.Redirect("Change12_2.aspx?AppID=" & Session("AppID").ToString)
            End If
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        DoSubmit()
    End Sub

    'Private Sub chk082_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk082.CheckedChanged
    '    If chk082.Checked Then
    '        ShowBis.Visible = True
    '        ShowVon.Visible = True
    '        ShowCalendar.Visible = True
    '    Else
    '        ShowBis.Visible = False
    '        ShowVon.Visible = False
    '        ShowCalendar.Visible = False
    '    End If
    'End Sub

    'Private Sub btnOpenSelectAb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectAb.Click
    '    calAbDatum.Visible = True
    '    calBisDatum.Visible = False
    'End Sub

    'Private Sub btnOpenSelectBis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectBis.Click
    '    calAbDatum.Visible = False
    '    calBisDatum.Visible = True
    'End Sub

    'Private Sub calAbDatum_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calAbDatum.SelectionChanged
    '    calAbDatum.Visible = False
    '    txtAbDatum.Text = calAbDatum.SelectedDate.ToShortDateString
    'End Sub

    'Private Sub calBisDatum_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calBisDatum.SelectionChanged
    '    calBisDatum.Visible = False
    '    txtBisDatum.Text = calBisDatum.SelectedDate.ToShortDateString
    'End Sub

    Private Sub chk080_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk080.CheckedChanged
        'ShowBis.Visible = False
        'ShowVon.Visible = False
        'ShowCalendar.Visible = False
    End Sub

    Private Sub chk081_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk081.CheckedChanged
        'ShowBis.Visible = False
        'ShowVon.Visible = False
        'ShowCalendar.Visible = False
    End Sub

    Private Sub chk083_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk083.CheckedChanged
        'ShowBis.Visible = False
        'ShowVon.Visible = False
        'ShowCalendar.Visible = False
    End Sub

    'Private Sub chk084_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk084.CheckedChanged
    '    ShowBis.Visible = False
    '    ShowVon.Visible = False
    '    ShowCalendar.Visible = False
    'End Sub

    Private Sub chk085_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk085.CheckedChanged
        'ShowBis.Visible = False
        'ShowVon.Visible = False
        'ShowCalendar.Visible = False
    End Sub

    Private Sub chk999_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk999.CheckedChanged
        'ShowBis.Visible = False
        'ShowVon.Visible = False
        'ShowCalendar.Visible = False
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change12.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Dittbernerc  Date: 5.08.10    Time: 9:32
' Updated in $/CKAG/Applications/appcsc/Forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.03.10   Time: 13:34
' Updated in $/CKAG/Applications/appcsc/Forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 2.05.08    Time: 10:37
' Updated in $/CKAG/Applications/appcsc/Forms
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 11:37
' Created in $/CKAG/Applications/appcsc/Forms
' 
' *****************  Version 10  *****************
' User: Uha          Date: 2.07.07    Time: 10:42
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 9  *****************
' User: Uha          Date: 7.03.07    Time: 9:28
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Forms
' 
' ************************************************
