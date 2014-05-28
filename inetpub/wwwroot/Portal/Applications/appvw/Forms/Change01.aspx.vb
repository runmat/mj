Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Change01
    Inherits System.Web.UI.Page
    '##### VW Report "Workflow Werkstattzuordnungsliste 1"
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

#Region "Declarations"
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objHandler As VW_04
    Private blnGridFilled As Boolean
    Private blnDP As Boolean

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtVorhabenNummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents rbAlle As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbUnbearbeitet As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbFreigegeben As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbStorniert As System.Web.UI.WebControls.RadioButton
    Protected WithEvents Datagrid2 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
    Protected WithEvents PanelDatagrids As System.Web.UI.WebControls.Panel
    Protected WithEvents PanelAdressAenderung As System.Web.UI.WebControls.Panel
    Protected WithEvents lblSchluessel As System.Web.UI.WebControls.Label
    Protected WithEvents lblReferenz As System.Web.UI.WebControls.Label
    Protected WithEvents litVorgang As System.Web.UI.WebControls.Literal
    Protected WithEvents txtName1Input As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtName2Input As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtStrasseInput As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPLZInput As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOrtInput As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtZielbahnhofInput As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBemerkungInput As System.Web.UI.WebControls.TextBox
    Protected WithEvents Literal3 As System.Web.UI.WebControls.Literal
    Protected WithEvents Literal5 As System.Web.UI.WebControls.Literal
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents txtHausnummerInput As System.Web.UI.WebControls.TextBox
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkExcelResult As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkAendern2 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdBack2 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents rbUnbearbeitetDP As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbFreigegebenDP As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbStorniertDP As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioVW As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucStyles As Styles
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        blnGridFilled = False
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Not Session("objHandler") Is Nothing Then
                objHandler = CType(Session("objHandler"), VW_04)
                Session("objHandler") = Nothing
            End If

            If Not Request.QueryString.Item("AG") Is Nothing Then
                If Request.QueryString.Item("AG").ToString = "VW" Then
                    blnDP = False
                ElseIf Request.QueryString.Item("AG").ToString = "DP" Then
                    blnDP = True
                Else
                    Throw New Exception("Keine Firmenzuordnung übergeben.")
                End If
            Else
                Throw New Exception("Keine Firmenzuordnung übergeben.")
            End If

            If Not IsPostBack Then
                HideInput()
                blnGridFilled = True
                Session("lnkExcel") = ""
                PanelAdressAenderung.Visible = False
                PanelDatagrids.Visible = True
                lnkExcelResult.Visible = False

                If blnDP Then
                    DataGrid1.Columns(DataGrid1.Columns.Count - 2).Visible = False
                    DataGrid1.Columns(DataGrid1.Columns.Count - 4).Visible = False
                    Datagrid2.Columns(Datagrid2.Columns.Count - 2).Visible = False
                    Datagrid2.Columns(Datagrid2.Columns.Count - 4).Visible = False
                    RadioVW.Visible = False
                    rbUnbearbeitetDP.Checked = True
                    rbFreigegebenDP.Checked = False
                    rbStorniertDP.Checked = False

                    rbAlle.Checked = False
                    rbUnbearbeitet.Checked = False
                    rbFreigegeben.Checked = False
                    rbStorniert.Checked = True
                Else
                    DataGrid1.Columns(DataGrid1.Columns.Count - 2).Visible = True
                    DataGrid1.Columns(DataGrid1.Columns.Count - 4).Visible = True
                    Datagrid2.Columns(Datagrid2.Columns.Count - 2).Visible = True
                    Datagrid2.Columns(Datagrid2.Columns.Count - 4).Visible = True
                    RadioVW.Visible = True

                    rbAlle.Checked = False
                    rbUnbearbeitet.Checked = True
                    rbFreigegeben.Checked = False
                    rbStorniert.Checked = False
                End If
            Else
                If Not Request.Form.Item("txtAktion") Is Nothing Then
                    If Request.Form.Item("txtAktion").ToString = "aendern" Then
                        AuswahlAdressAenderung()
                    ElseIf Not Request.Form.Item("txtAktion").ToString = "keine" Then
                        AusfuehrenSchreibenSAP(Request.Form.Item("txtAktion").ToString, False)
                    End If
                End If
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub AusfuehrenSchreibenSAP(ByVal strType As String, ByVal blnMitAdressAenderung As Boolean)
        Dim strReferenz1 As String = ""
        Dim strSchluessel As String = ""
        Dim strStat_sus_bearb As String

        'Suche der Schlüssel
        If blnMitAdressAenderung Then
            strReferenz1 = lblReferenz.Text
            strSchluessel = lblSchluessel.Text
        Else
            If Not Request.Form.Item("txtReferenz1") Is Nothing Then
                strReferenz1 = Request.Form.Item("txtReferenz1").ToString
            End If
            If Not Request.Form.Item("txtSchluessel") Is Nothing Then
                strSchluessel = Request.Form.Item("txtSchluessel").ToString
            End If
        End If

        If strReferenz1.Length + strSchluessel.Length = 0 Then
            Throw New Exception("Kein Bearbeitungsschlüssel übergeben.")
        End If

        'Auswahl der Datensätze
        Dim rows() As DataRow
        If Not strReferenz1.Length = 0 Then
            rows = objHandler.Result.Select("Referenz1 = '" & strReferenz1 & "'")
        End If

        If Not strSchluessel.Length = 0 Then
            Dim arraySplit() As String = strSchluessel.Split("/"c)
            rows = objHandler.Result.Select("Referenz2 = '" & arraySplit(0) & "' AND Nummer_sus = '" & arraySplit(1) & "' AND Status = '" & arraySplit(2) & "'")
        End If

        'Suche des Types
        Select Case strType
            Case "freigeben"
                strStat_sus_bearb = "1"
            Case "stornieren"
                strStat_sus_bearb = "9"
            Case "aendern"
                strStat_sus_bearb = "X"
                If rbUnbearbeitetDP.Checked Then
                    strStat_sus_bearb = " "
                End If
                If rbFreigegebenDP.Checked Then
                    strStat_sus_bearb = "1"
                End If
                If rbStorniertDP.Checked Then
                    strStat_sus_bearb = "9"
                End If
                If strStat_sus_bearb = "X" Then
                    Throw New Exception("Kein Bearbeitungsstatus übergeben.")
                End If
            Case Else
                Throw New Exception("Kein Bearbeitungsstatus übergeben.")
        End Select

        If rows.Length = 0 Then
            Throw New Exception("Keine Elemente zur Bearbeitung gefunden.")
        End If

        'Setzen der Werte
        '- Auf Zeilenebene
        objHandler.Result.AcceptChanges()
        Dim intLoop As Integer
        For intLoop = 0 To rows.Length - 1
            rows(intLoop)("StatusInput") = strStat_sus_bearb
            rows(intLoop)("Selected") = True
            If blnMitAdressAenderung Then
                rows(intLoop)("NewAddress") = True
                rows(intLoop)("Name1Input") = Left(txtName1Input.Text, 40)
                rows(intLoop)("Name2Input") = Left(txtName2Input.Text, 40)
                rows(intLoop)("StrasseInput") = Left(txtName2Input.Text, 60)
                rows(intLoop)("HausnummerInput") = Left(txtHausnummerInput.Text, 10)
                rows(intLoop)("PLZInput") = Left(txtPLZInput.Text, 10)
                rows(intLoop)("OrtInput") = Left(txtOrtInput.Text, 40)
                rows(intLoop)("ZielbahnhofInput") = Left(txtZielbahnhofInput.Text, 5)
                rows(intLoop)("BemerkungInput") = Left(txtBemerkungInput.Text, 100)
            End If
        Next
        objHandler.Result.AcceptChanges()

        '- Global
        If blnDP Then
            objHandler.KonzBearb = "DP"
        Else
            objHandler.KonzBearb = "VW"
        End If

        'Aufruf Change
        objHandler.Change()

        'Neuer Excellink
        If Not objHandler.Status = 0 Then
            lblError.Text = "Fehler: " & objHandler.Message
        Else
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & "_R.xls"
            Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()

            Try
                excelFactory.CreateDocumentAndWriteToFilesystem(ConfigurationManager.AppSettings("ExcelPath") & strFileName, objHandler.ReturnChanged, Me)
                lnkExcelResult.NavigateUrl = "/Portal/Temp/Excel/" & strFileName
                lnkExcelResult.Visible = True
            Catch
            End Try
        End If

        'Darstellung wiederherstellen
        HideAdressAenderung()

        'Neue Suche
        DoSubmit()
    End Sub

    Private Sub AuswahlAdressAenderung()
        ShowAdressAenderung()

        lblReferenz.Text = Request.Form.Item("txtReferenz1").ToString
        lblSchluessel.Text = Request.Form.Item("txtSchluessel").ToString

        Dim rows() As DataRow
        Dim strVorhabennummer As String = ""
        Dim strHaendlerName As String = ""
        Dim strIKZ As String = ""

        If Not lblReferenz.Text.Length = 0 Then
            rows = objHandler.Result.Select("Referenz1 = '" & lblReferenz.Text & "'")
            strIKZ = CStr(rows(0)("Referenz1"))
        End If

        If Not lblSchluessel.Text.Length = 0 Then
            Dim arraySplit() As String = lblSchluessel.Text.Split("/"c)
            rows = objHandler.Result.Select("Referenz2 = '" & arraySplit(0) & "' AND Nummer_sus = '" & arraySplit(1) & "' AND Status = '" & arraySplit(2) & "'")
            strIKZ = ""

            strVorhabennummer = CStr(rows(0)("Referenz2"))
            strHaendlerName = CStr(rows(0)("Name1_sus")) & " " & CStr(rows(0)("Name2_sus"))

            txtName1Input.Text = CStr(rows(0)("Name1_sus"))
            txtName2Input.Text = CStr(rows(0)("Name2_sus"))
            txtStrasseInput.Text = CStr(rows(0)("Strasse_sus"))
            txtHausnummerInput.Text = CStr(rows(0)("Hausnr_sus"))
            txtPLZInput.Text = CStr(rows(0)("Pstlz_sus"))
            txtOrtInput.Text = CStr(rows(0)("Ort_sus"))
            txtZielbahnhofInput.Text = CStr(rows(0)("Zielbahnhof"))
            txtBemerkungInput.Text = CStr(rows(0)("Bem_sus_bearb"))

            If Not blnDP Then
                Select Case CStr(rows(0)("Stat_sus_bearb")).ToString
                    Case "1"
                        rbFreigegebenDP.Checked = True
                    Case "9"
                        rbStorniertDP.Checked = True
                    Case Else
                        rbUnbearbeitetDP.Checked = True
                End Select
            End If
        End If
        litVorgang.Text = "<table border=""0"" cellspacing=""0"" cellpadding=""0"">" & vbCrLf

        litVorgang.Text &= "<tr>" & vbCrLf
        litVorgang.Text &= "<td><u>Vorhaben</u></td>" & vbCrLf
        litVorgang.Text &= "<td>&nbsp;&nbsp;&nbsp;</td>" & vbCrLf
        litVorgang.Text &= "<td>" & strVorhabennummer & "</td>" & vbCrLf
        litVorgang.Text &= "</tr>" & vbCrLf

        litVorgang.Text &= "<tr>" & vbCrLf
        litVorgang.Text &= "<td><u>Händler</u></td>" & vbCrLf
        litVorgang.Text &= "<td>&nbsp;&nbsp;&nbsp;</td>" & vbCrLf
        litVorgang.Text &= "<td>" & strHaendlerName & "</td>" & vbCrLf
        litVorgang.Text &= "</tr>" & vbCrLf

        If strIKZ.Length > 0 Then
            litVorgang.Text &= "<tr>" & vbCrLf
            litVorgang.Text &= "<td><u>Fahrzeugreferenz</u></td>" & vbCrLf
            litVorgang.Text &= "<td>&nbsp;&nbsp;&nbsp;</td>" & vbCrLf
            litVorgang.Text &= "<td>" & strIKZ & "</td>" & vbCrLf
            litVorgang.Text &= "</tr>" & vbCrLf
        End If
        litVorgang.Text &= "</table>" & vbCrLf

        blnGridFilled = True
    End Sub

    Private Sub ShowAdressAenderung()
        PanelAdressAenderung.Visible = True
        PanelDatagrids.Visible = False
        lnkExcel.Visible = False
        lblDownloadTip.Visible = False
        cmdSearch.Enabled = False
    End Sub

    Private Sub HideAdressAenderung()
        PanelAdressAenderung.Visible = False
        PanelDatagrids.Visible = True
        lnkExcel.Visible = True
        lblDownloadTip.Visible = True
        cmdSearch.Enabled = True
    End Sub

    Private Sub ShowInput()
        Datagrid2.Visible = True
    End Sub

    Private Sub HideInput()
        Datagrid2.Visible = False
    End Sub

    Private Sub FillDatagrid2(Optional ByVal strSetIndex As String = "")
        Dim vwKunde As DataView

        vwKunde = objHandler.Haendler.DefaultView

        If vwKunde.Count = 0 Then
            Datagrid2.Visible = False
        Else
            Datagrid2.Visible = True
            vwKunde.Sort = "Schluessel ASC"
            With Datagrid2
                .DataSource = vwKunde
                .DataBind()
            End With

            Dim item As DataGridItem
            Dim cell As TableCell
            Dim control As Control
            Dim linkbutton As LinkButton
            Dim label As Label

            If strSetIndex = String.Empty Then
                blnGridFilled = True
            Else
                For Each item In Datagrid2.Items
                    Dim strSchluessel As String = item.Cells(item.Cells.Count - 1).Text
                    If strSchluessel = strSetIndex Then
                        item.BackColor = System.Drawing.Color.DimGray
                    End If
                Next
            End If

            For Each item In Datagrid2.Items
                Dim strSchluessel As String = item.Cells(item.Cells.Count - 1).Text

                cell = item.Cells(item.Cells.Count - 5)
                Dim strStatus As String = ""
                For Each control In cell.Controls
                    If TypeOf control Is Label Then
                        label = CType(control, Label)
                        strStatus = label.Text
                    End If
                Next

                Dim rows() As DataRow = objHandler.Haendler.Select("Schluessel = '" & strSchluessel & "'")
                Dim strParameter As String = _
                        "'" & CStr(rows(0)("HaendlerName1")) & "'," & _
                        "'" & CStr(rows(0)("HaendlerName2")) & "'," & _
                        "'" & CStr(rows(0)("HaendlerStrasse")) & "'," & _
                        "'" & CStr(rows(0)("HaendlerHausnummer")) & "'," & _
                        "'" & CStr(rows(0)("HaendlerPLZ")) & "'," & _
                        "'" & CStr(rows(0)("HaendlerOrt")) & "'," & _
                        "'',''"

                For Each cell In item.Cells
                    For Each control In cell.Controls
                        If TypeOf control Is LinkButton Then
                            linkbutton = CType(control, LinkButton)
                            If linkbutton.CommandName = "Freigeben" Then
                                linkbutton.Attributes.Add("onClick", "if (!AktionZustimmen('freigeben','','" & strSchluessel & "'," & strParameter & ")) return false;")
                                'Falls kein Confirm gewünscht wird:
                                'linkbutton.Attributes.Add("onClick", "AktionAusfuehren('freigeben','','" & strSchluessel & "'," & strParameter & ");")
                            ElseIf linkbutton.CommandName = "Storno" Then
                                linkbutton.Attributes.Add("onClick", "if (!AktionZustimmen('stornieren','','" & strSchluessel & "'," & strParameter & ")) return false;")
                                'Falls kein Confirm gewünscht wird:
                                'linkbutton.Attributes.Add("onClick", "AktionAusfuehren('stornieren','','" & strSchluessel & "'," & strParameter & ");")
                            ElseIf linkbutton.CommandName = "Aendern" Then
                                linkbutton.Attributes.Add("onClick", "AktionAusfuehren('aendern','','" & strSchluessel & "'," & strParameter & ");")
                                If blnDP And (Not strStatus = "F") Then
                                    linkbutton.Enabled = True
                                End If
                            End If
                        End If
                    Next
                Next
            Next
        End If
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        blnGridFilled = True

        Dim tmpDataView As New DataView()
        tmpDataView = objHandler.Result.DefaultView
        Dim item As DataGridItem
        For Each item In Datagrid2.Items
            If item.BackColor.ToArgb = System.Drawing.Color.DimGray.ToArgb Then
                objHandler.Schluessel = item.Cells(item.Cells.Count - 1).Text
            End If
        Next

        If (Not objHandler.Schluessel Is Nothing) Then
            Dim arraySplit() As String = objHandler.Schluessel.Split("/"c)
            tmpDataView.RowFilter = "Referenz2 = '" & arraySplit(0) & "' AND Nummer_sus = '" & arraySplit(1) & "' AND Status = '" & arraySplit(2) & "'"
        End If

        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
        Else
            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = strTempSort) Then
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState("Sort") = strTempSort
                ViewState("Direction") = strDirection
            Else
                If Not ViewState("Sort") Is Nothing Then
                    strTempSort = ViewState("Sort").ToString
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState("Direction") = strDirection
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            DataGrid1.CurrentPageIndex = intTempPageIndex

            DataGrid1.DataSource = tmpDataView
            DataGrid1.DataBind()

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If

            Dim cell As TableCell
            Dim control As Control
            Dim linkbutton As LinkButton
            Dim label As Label
            For Each item In DataGrid1.Items
                Dim strSchluessel As String = item.Cells(item.Cells.Count - 1).Text

                cell = item.Cells(item.Cells.Count - 5)
                Dim strStatus As String = ""
                For Each control In cell.Controls
                    If TypeOf control Is Label Then
                        label = CType(control, Label)
                        strStatus = label.Text
                    End If
                Next

                Dim rows() As DataRow = objHandler.Result.Select("Referenz1 = '" & strSchluessel & "'")
                Dim strParameter As String = _
                        "'" & CStr(rows(0)("Name1_sus")) & "'," & _
                        "'" & CStr(rows(0)("Name2_sus")) & "'," & _
                        "'" & CStr(rows(0)("Strasse_sus")) & "'," & _
                        "'" & CStr(rows(0)("Hausnr_sus")) & "'," & _
                        "'" & CStr(rows(0)("Pstlz_sus")) & "'," & _
                        "'" & CStr(rows(0)("Ort_sus")) & "'," & _
                        "'',''"

                For Each cell In item.Cells
                    For Each control In cell.Controls
                        If TypeOf control Is LinkButton Then
                            linkbutton = CType(control, LinkButton)
                            If linkbutton.CommandName = "Freigeben" Then
                                linkbutton.Attributes.Add("onClick", "if (!AktionZustimmen('freigeben','" & strSchluessel & "',''," & strParameter & ")) return false;")
                                'Falls kein Confirm gewünscht wird:
                                'linkbutton.Attributes.Add("onClick", "AktionAusfuehren('freigeben','" & strSchluessel & "',''," & strParameter & ");")
                            ElseIf linkbutton.CommandName = "Storno" Then
                                linkbutton.Attributes.Add("onClick", "if (!AktionZustimmen('stornieren','" & strSchluessel & "',''," & strParameter & ")) return false;")
                                'Falls kein Confirm gewünscht wird:
                                'linkbutton.Attributes.Add("onClick", "AktionAusfuehren('stornieren','" & strSchluessel & "',''," & strParameter & ");")
                            ElseIf linkbutton.CommandName = "Aendern" Then
                                linkbutton.Attributes.Add("onClick", "AktionAusfuehren('aendern','" & strSchluessel & "',''," & strParameter & ");")
                                If blnDP And (Not strStatus = "F") Then
                                    linkbutton.Enabled = True
                                End If
                            End If
                        End If
                    Next
                Next

            Next
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Try
            If Not blnGridFilled Then
                FillGrid(0)
            End If
            If Not Session("lnkExcel") Is Nothing AndAlso CStr(Session("lnkExcel")).Length > 0 Then
                lnkExcel.NavigateUrl = CStr(Session("lnkExcel"))
                lnkExcel.Visible = True
                lblDownloadTip.Visible = True
            Else
                lnkExcel.Visible = False
                lblDownloadTip.Visible = False
            End If
            If Not objHandler Is Nothing Then
                Session.Add("objHandler", objHandler)
            End If

            Dim strLiteral As String = "<SCRIPT language=""Javascript"">" & vbCrLf & _
                                "<!-- //" & vbCrLf & _
                                "function AktionZustimmen(Aktion,Referenz1,Schluessel,Name1,Name2,Strasse,Hausnummer,PLZ,Ort,Zielbahnhof,Bemerkung)" & vbCrLf & _
                                "{" & vbCrLf & _
                                "	var Check = window.confirm(""Wollen Sie die Adresse für den Händler\n\t"" + Name1 + "" "" + Name2 + ""\n\t"" + Strasse + "" "" + Hausnummer + ""\n\t"" + PLZ + "" "" + Ort + ""\t\nwirklich "" + Aktion + ""?"");" & vbCrLf & _
                                "	if (Check)" & vbCrLf & _
                                "	{" & vbCrLf & _
                                "		window.document.Form1.txtAktion.value = Aktion;" & vbCrLf & _
                                "		window.document.Form1.txtReferenz1.value = Referenz1;" & vbCrLf & _
                                "		window.document.Form1.txtSchluessel.value = Schluessel;" & vbCrLf & _
                                "		window.document.Form1.txtName1.value = Name1;" & vbCrLf & _
                                "		window.document.Form1.txtName2.value = Name2;" & vbCrLf & _
                                "		window.document.Form1.txtStrasse.value = Strasse;" & vbCrLf & _
                                "		window.document.Form1.txtHausnummer.value = Hausnummer;" & vbCrLf & _
                                "		window.document.Form1.txtPLZ.value = PLZ;" & vbCrLf & _
                                "		window.document.Form1.txtOrt.value = Ort;" & vbCrLf & _
                                "		window.document.Form1.txtZielbahnhof.value = Zielbahnhof;" & vbCrLf & _
                                "		window.document.Form1.txtBemerkung.value = Bemerkung;" & vbCrLf & _
                                "	}" & vbCrLf & _
                                "	return (Check);" & vbCrLf & _
                                "}" & vbCrLf & vbCrLf & _
                                "function AktionAusfuehren(Aktion,Referenz1,Schluessel,Name1,Name2,Strasse,Hausnummer,PLZ,Ort,Zielbahnhof,Bemerkung)" & vbCrLf & _
                                "{" & vbCrLf & _
                                "	window.document.Form1.txtAktion.value = Aktion;" & vbCrLf & _
                                "	window.document.Form1.txtReferenz1.value = Referenz1;" & vbCrLf & _
                                "	window.document.Form1.txtSchluessel.value = Schluessel;" & vbCrLf & _
                                "	window.document.Form1.txtName1.value = Name1;" & vbCrLf & _
                                "	window.document.Form1.txtName2.value = Name2;" & vbCrLf & _
                                "	window.document.Form1.txtStrasse.value = Strasse;" & vbCrLf & _
                                "	window.document.Form1.txtHausnummer.value = Hausnummer;" & vbCrLf & _
                                "	window.document.Form1.txtPLZ.value = PLZ;" & vbCrLf & _
                                "	window.document.Form1.txtOrt.value = Ort;" & vbCrLf & _
                                "	window.document.Form1.txtZielbahnhof.value = Zielbahnhof;" & vbCrLf & _
                                "	window.document.Form1.txtBemerkung.value = Bemerkung;" & vbCrLf & _
                                "}" & vbCrLf & _
                                "//-->" & vbCrLf & _
                                "</SCRIPT>" & vbCrLf

            If PanelDatagrids.Visible Then
                Literal3.Text = strLiteral
                Literal5.Text = ""
            Else
                Literal3.Text = ""
                Literal5.Text = strLiteral
            End If
            SetEndASPXAccess(Me)
        Catch ex As Exception
            lblError.Text = "Beim Bearbeiten der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        Try
            FillGrid(0, e.SortExpression)
        Catch ex As Exception
            lblError.Text = "Beim Bearbeiten der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Try
            DoSubmit()
            lnkExcelResult.Visible = False
        Catch ex As Exception
            lblError.Text = "Beim Bearbeiten der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub DoSubmit()
        HideInput()
        Session("lnkExcel") = ""
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            objHandler = New VW_04(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)

            lblError.Text = ""

            If rbAlle.Checked Then
                objHandler.StatusBearbeitung = "A"
            ElseIf rbUnbearbeitet.Checked Then
                objHandler.StatusBearbeitung = "U"
            ElseIf rbFreigegeben.Checked Then
                objHandler.StatusBearbeitung = "F"
            ElseIf rbStorniert.Checked Then
                objHandler.StatusBearbeitung = "S"
            End If
            objHandler.Vorhaben = ""
            If Not txtVorhabenNummer.Text.Length = 0 Then
                objHandler.Vorhaben = txtVorhabenNummer.Text
            End If
            objHandler.Show()

            If Not objHandler.Status = 0 Then
                lblError.Text = "Fehler: " & objHandler.Message
                blnGridFilled = True
            Else
                ShowInput()
                If objHandler.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Daten vorhanden."
                Else

                    Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()

                    Try
                        excelFactory.CreateDocumentAndWriteToFilesystem(ConfigurationManager.AppSettings("ExcelPath") & strFileName, objHandler.ResultExcel, Me)
                        Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                    Catch
                    End Try
                    logApp.UpdateEntry("APP", Session("AppID").ToString, "Workflow Werkstattzuordnungsliste 1")

                    FillDatagrid2()
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Lesen der Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

        DataGrid1.Visible = False
    End Sub

    Private Sub Datagrid2_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles Datagrid2.ItemCommand
        Try
            Select Case e.CommandName
                Case "Details"
                    FillDatagrid2(e.Item.Cells(e.Item.Cells.Count - 1).Text)
                    DataGrid1.Visible = True
                    SetLiteral1ForDatagrid2(e.Item.Cells(e.Item.Cells.Count - 1).Text)
                Case Else
                    blnGridFilled = True
            End Select
        Catch ex As Exception
            lblError.Text = "Beim Bearbeiten der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub SetLiteral1ForDatagrid2(ByVal strTarget As String)
        Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
        Literal1.Text &= "						  <!-- //" & vbCrLf
        Literal1.Text &= "						    window.document.location.href = ""#" & Replace(Replace(Replace(strTarget, "/", ""), "-", ""), " ", "") & """;" & vbCrLf
        Literal1.Text &= "						  //-->" & vbCrLf
        Literal1.Text &= "						</script>" & vbCrLf
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        Try
            Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
            Literal1.Text &= "						  <!-- //" & vbCrLf
            Literal1.Text &= "						    window.document.location.href = ""#" & e.Item.Cells(e.Item.Cells.Count - 1).Text & """;" & vbCrLf
            Literal1.Text &= "						  //-->" & vbCrLf
            Literal1.Text &= "						</script>" & vbCrLf

        Catch ex As Exception
            lblError.Text = "Beim Bearbeiten der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        DoBack()
    End Sub

    Private Sub DoBack()
        Try
            HideAdressAenderung()
            blnGridFilled = True
        Catch ex As Exception
            lblError.Text = "Beim Bearbeiten der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    'Private Sub lnkFreigeben2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Try
    '        AusfuehrenSchreibenSAP("freigeben", True)
    '    Catch ex As Exception
    '        lblError.Text = "Beim Bearbeiten der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
    '    End Try
    'End Sub

    'Private Sub lnkStorno2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Try
    '        AusfuehrenSchreibenSAP("stornieren", True)
    '    Catch ex As Exception
    '        lblError.Text = "Beim Bearbeiten der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
    '    End Try
    'End Sub

    Private Sub cmdBack2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack2.Click
        DoBack()
    End Sub

    Private Sub lnkAendern2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkAendern2.Click
        Try
            AusfuehrenSchreibenSAP("aendern", True)
        Catch ex As Exception
            lblError.Text = "Beim Bearbeiten der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub
End Class

' ************************************************
' $History: Change01.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:14
' Updated in $/CKAG/Applications/appvw/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:48
' Created in $/CKAG/Applications/appvw/Forms
' 
' *****************  Version 7  *****************
' User: Uha          Date: 27.08.07   Time: 9:34
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' Änderungen in Workflow Werkstattzuordnungsliste: Darstellung in
' Detail-Datagrid und Vorauswahl Radio-Buttons
' 
' *****************  Version 6  *****************
' User: Uha          Date: 22.08.07   Time: 17:52
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' Bugfixing ITA 1120 und 1177
' 
' *****************  Version 5  *****************
' User: Uha          Date: 18.07.07   Time: 13:38
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' Report "Workflow Werkstattzuordnungsliste" für VW und DP zum Testen
' fertig
' 
' *****************  Version 4  *****************
' User: Uha          Date: 17.07.07   Time: 18:51
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 3. teilfunktionaler Zwischenstand von Change01
' 
' *****************  Version 3  *****************
' User: Uha          Date: 16.07.07   Time: 17:22
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 2. teilfunktionaler Zwischenstand von Change01
' 
' *****************  Version 2  *****************
' User: Uha          Date: 16.07.07   Time: 14:21
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' Teilfunktionaler Zwischenstand von Change01
' 
' *****************  Version 1  *****************
' User: Uha          Date: 12.07.07   Time: 16:58
' Created in $/CKG/Applications/AppVW/AppVWWeb/Forms
' Report "Workflow Werkstattzuordnungsliste 1" roh hinzugefügt = Keine
' Komplierfehler aber nicht lauffähig
' 
' ************************************************
