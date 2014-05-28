Option Explicit On
Option Strict On

Imports CKG
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System.Drawing

Public Class Change02_1
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

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User

    Private mObjBriefanforderung As Briefanforderung
    Private mObjBez_Sperre As BzKennzeichen_Sperren

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lb_zurueck As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lb_Auswahl As System.Web.UI.WebControls.LinkButton
    Protected WithEvents imgbExcel As ImageButton
    Protected WithEvents ucStyles As Styles

    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label

    Protected WithEvents lbtnweiter As LinkButton
    Private AppParam As String



#Region "Properties"

    Private Property Refferer() As String
        Get
            Dim tmpStr As String = ""
            HelpProcedures.getAppParameters(Session("AppID").ToString, tmpStr, ConfigurationManager.AppSettings.Item("Connectionstring"))
            If Not Session.Item(Me.Request.Url.LocalPath & tmpStr & "Refferer") Is Nothing Then
                Return Session.Item(Me.Request.Url.LocalPath & tmpStr & "Refferer").ToString()
            Else : Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            Dim tmpStr As String = ""
            HelpProcedures.getAppParameters(Session("AppID").ToString, tmpStr, ConfigurationManager.AppSettings.Item("Connectionstring"))
            Session.Item(Me.Request.Url.LocalPath & tmpStr & "Refferer") = value
        End Set
    End Property



#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        Try
            m_User = GetUser(Me)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            lblError.Text = ""
            Select Case Me.Request.QueryString.Item("Art").ToUpper
                Case "ENDG"
                    AppParam = "ENDG"
                    lblHead.Text = "Endgültiger Dokumentenversand"
                Case "TEMP"
                    AppParam = "TEMP"
                    lblHead.Text = "Temporärer Dokumentenversand"
                Case "BZKENNZ"
                    AppParam = "BZKENNZ"
                    lbtnweiter.Text = "Absenden"
                    lblHead.Text = "Bezahltkennzeichen setzen und aufheben"
                Case "SPERR"
                    AppParam = "SPERR"
                    lbtnweiter.Text = "Absenden"
                    lblHead.Text = "Versandsperre setzen und aufheben"
                Case Else
                    Throw New Exception("Fehlender Querystringparameter 'Art' Vorgang wird abgebrochen.")
            End Select

            If Not IsPostBack Then
                If Refferer Is Nothing Then
                    If Not Me.Request.UrlReferrer Is Nothing Then
                        Refferer = Me.Request.UrlReferrer.ToString
                    Else
                        Refferer = ""
                    End If
                End If

                GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                ucStyles.TitleText = lblHead.Text
            End If

            If mObjBriefanforderung Is Nothing Then
                If Session("mObjBriefanforderungSession") Is Nothing Then
                    Throw New Exception("benötigtiges SessionObjekt nicht vorhanden")
                Else
                    mObjBriefanforderung = CType(Session("mObjBriefanforderungSession"), Briefanforderung)
                End If
            End If

            If Not IsPostBack Then

                'seitenspeziefische Aktionen

                'dropDownListe füllen
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 2
                FillGrid(0)
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub responseBack()
        Dim urlReffer As String
        If Refferer = "" Then
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            urlReffer = Refferer
            Refferer = Nothing
            Response.Redirect(urlReffer)
        End If
    End Sub

    Private Function XtoBool(ByVal strProof As String) As Boolean
        If strProof = "X" Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function BooltoX(ByVal bInput As Boolean) As String
        If bInput = True Then
            Return "X"
        Else
            Return ""
        End If
    End Function

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If mObjBriefanforderung.Status = 0 Then

            If IsNothing(mObjBriefanforderung.Fahrzeuge) OrElse mObjBriefanforderung.Fahrzeuge.Rows.Count = 0 Then

                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                DataGrid1.Visible = False
                lblNoData.Visible = True
                lblInfo.Text = "Anzahl: 0"
            Else

                DataGrid1.Visible = True
                lblNoData.Visible = False
                imgbExcel.Visible = True

                Dim tmpDataView As New DataView(mObjBriefanforderung.Fahrzeuge)


                Dim intTempPageIndex As Int32 = intPageIndex
                Dim strTempSort As String = ""
                Dim strDirection As String = ""

                If strSort.Trim(" "c).Length > 0 Then
                    intTempPageIndex = 0
                    strTempSort = strSort.Trim(" "c)
                    If (Session("App_Sort") Is Nothing) OrElse (Session("App_Sort").ToString = strTempSort) Then
                        If Session("App_Direction") Is Nothing Then
                            strDirection = "desc"
                        Else
                            strDirection = Session("App_Direction").ToString
                        End If
                    Else
                        strDirection = "desc"
                    End If

                    If strDirection = "asc" Then
                        strDirection = "desc"
                    Else
                        strDirection = "asc"
                    End If

                    Session("App_Sort") = strTempSort
                    Session("App_Direction") = strDirection
                Else
                    If Not Session("App_Sort") Is Nothing Then
                        strTempSort = Session("App_Sort").ToString
                        If Session("App_Direction") Is Nothing Then
                            strDirection = "asc"
                            Session("App_Direction") = strDirection
                        Else
                            strDirection = Session("App_Direction").ToString
                        End If
                    End If
                End If

                If Not strTempSort.Length = 0 Then
                    tmpDataView.Sort = strTempSort & " " & strDirection
                Else
                    tmpDataView.Sort = "Kennzeichen asc"
                End If
                If Not (Session("App_PageIndex") Is Nothing) Then
                    intTempPageIndex = CInt(Session("App_PageIndex"))
                End If
                lblInfo.Text = "Anzahl: " & tmpDataView.Count

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

                'farbe der Zeilen Ändern wenn temp oder endg JJU20081106
                DataGrid1.BackColor = Drawing.Color.FromArgb(244, 164, 96)

            End If
        Else
            lblError.Text = mObjBriefanforderung.Message
            Exit Sub
        End If

    End Sub

    Private Sub DataGrid1_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.EditCommand
        DataGrid1.EditItemIndex = e.Item.ItemIndex
        FillGrid(DataGrid1.CurrentPageIndex)
    End Sub

    Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        Dim CellBezahlt As TableCell
        Dim CellAngefordert As TableCell
        Dim CellSperre As TableCell

        Dim CellLeasingnehmer As TableCell
        Dim chkTempBezahlt As CheckBox
        Dim chkTempSperre As CheckBox
        Dim chkTempAngefordern As CheckBox
        Dim chkTempAbmeld As CheckBox
        Dim strCellEqui As String
        CellBezahlt = e.Item.Cells(14)
        CellSperre = e.Item.Cells(15)
        CellAngefordert = e.Item.Cells(16)
        CellLeasingnehmer = e.Item.Cells(11)

        chkTempBezahlt = CType(CellBezahlt.FindControl("chbBzKennz"), CheckBox)
        chkTempSperre = CType(CellSperre.FindControl("chbSperren"), CheckBox)
        chkTempAngefordern = CType(CellAngefordert.FindControl("chbAnfordern"), CheckBox)
        chkTempAbmeld = CType(CellAngefordert.FindControl("chbAbmeld"), CheckBox)
        strCellEqui = e.Item.Cells(0).Text


        If Not chkTempBezahlt Is Nothing AndAlso Not chkTempSperre Is Nothing AndAlso Not chkTempAngefordern Is Nothing Then
            With mObjBriefanforderung
                If Not .FahrzeugeChanged Is Nothing Then
                    If .FahrzeugeChanged.Rows.Count > 0 Then
                        Dim RowArray() As DataRow = .FahrzeugeChanged.Select("EQUNR= '" & strCellEqui & "'")
                        If RowArray.Length = 1 Then
                            chkTempBezahlt.Checked = XtoBool(RowArray(0)("Bezahltkennzeichen").ToString)
                            chkTempSperre.Checked = XtoBool(RowArray(0)("Versandsperre").ToString)
                            chkTempAngefordern.Checked = XtoBool(RowArray(0)("Anfordern").ToString)
                            chkTempAbmeld.Checked = XtoBool(RowArray(0)("Abmelden").ToString)
                        End If
                    End If
                End If
            End With

            If chkTempBezahlt.Checked AndAlso chkTempSperre.Checked = False Then
                chkTempAngefordern.Enabled = True
            End If
        End If
        If Not e.Item.FindControl("lnkFahrgestellnummer") Is Nothing Then
            CType(e.Item.FindControl("lnkFahrgestellnummer"), HyperLink).NavigateUrl = "../../../Components/ComCommon/Finance/Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN=" & CType(e.Item.FindControl("lnkFahrgestellnummer"), HyperLink).Text
        End If

    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        DataGrid1.EditItemIndex = -1
        Session("App_PageIndex") = e.NewPageIndex
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        DataGrid1.EditItemIndex = -1
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        DataGrid1.EditItemIndex = -1
        FillGrid(0)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub lb_zurueck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

    Protected Sub lb_weiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnweiter.Click
        Dim strError As String = "Für die rot markierten Dokumente fehlen die Versanddaten!<br />"
        Dim bAnfordern As Boolean
        Select Case AppParam.ToUpper
            Case "ENDG"
                Checkgrid()
                If mObjBriefanforderung.FahrzeugeChanged Is Nothing Then
                    lblError.Text = "Sie haben keine Auswahl vorgenommen"
                    Exit Sub
                Else
                    bAnfordern = mObjBriefanforderung.callBapiForAdressen(Session("AppID").ToString, Session.SessionID, Me)
                    If mObjBriefanforderung.Status = 0 Then
                        Refferer = Nothing
                        Dim Parameterlist As String = "&Art=Endg"
                        If bAnfordern = True Then
                            Response.Redirect("Change02_2.aspx?AppID=" & Session("AppID").ToString & Parameterlist)
                        Else
                            Response.Redirect("Change02_3.aspx?AppID=" & Session("AppID").ToString & Parameterlist)
                        End If


                    Else
                        lblError.Text = mObjBriefanforderung.Message
                    End If
                End If
        End Select

    End Sub

    Protected Sub chbAnfordern_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Protected Sub chbSperren_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim chk As CheckBox = CType(sender, CheckBox)
        Dim chkAnforden As CheckBox
        Dim BzKennz As CheckBox
        Dim CtrlIDArr() As String = Split(chk.UniqueID, "$")
        Dim CtrlID As String
        If Not chk Is Nothing Then
            CtrlID = CtrlIDArr(0).ToString & "$" & CtrlIDArr(1).ToString & "$chbAnfordern"
            chkAnforden = New CheckBox
            chkAnforden = FindCheckBox(CtrlID, 16)
            CtrlID = CtrlIDArr(0).ToString & "$" & CtrlIDArr(1).ToString & "$chbBzKennz"
            BzKennz = New CheckBox
            BzKennz = FindCheckBox(CtrlID, 14)
            If chk.Checked = True Then
                chkAnforden.Enabled = False
                chkAnforden.Checked = False
            Else
                If BzKennz.Checked Then
                    chkAnforden.Enabled = True
                End If

            End If
        End If

    End Sub

    Protected Sub chbBzKennz_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim chk As CheckBox = CType(sender, CheckBox)
        Dim chkAnforden As CheckBox
        Dim chkSperre As CheckBox
        Dim CtrlIDArr() As String = Split(chk.UniqueID, "$")
        Dim CtrlID As String
        If Not chk Is Nothing Then
            CtrlID = CtrlIDArr(0).ToString & "$" & CtrlIDArr(1).ToString & "$chbAnfordern"
            chkAnforden = New CheckBox
            chkAnforden = FindCheckBox(CtrlID, 16)
            CtrlID = CtrlIDArr(0).ToString & "$" & CtrlIDArr(1).ToString & "$chbSperren"
            chkSperre = New CheckBox
            chkSperre = FindCheckBox(CtrlID, 15)
            If chk.Checked = True Then
                If Not chkSperre.Checked Then
                    chkAnforden.Enabled = True
                End If

            Else
                chkAnforden.Enabled = False
                chkAnforden.Checked = False
            End If
        End If

    End Sub

    Private Function FindCheckBox(ByVal UniqueID As String, ByVal CellIndex As Integer) As CheckBox
        Dim intCounter As Integer
        Dim Cell As TableCell

        Dim chkTemp As CheckBox = Nothing
        For intCounter = 0 To DataGrid1.Items.Count - 1
            Cell = DataGrid1.Items(intCounter).Cells(CellIndex)
            Dim ctrlTempControl As Control
            For Each ctrlTempControl In Cell.Controls
                If ctrlTempControl.UniqueID = UniqueID Then
                    chkTemp = CType(ctrlTempControl, CheckBox)
                    Exit For
                End If
            Next
            If Not chkTemp Is Nothing Then
                Exit For
            End If
        Next
        Return chkTemp
    End Function

    Private Sub Checkgrid()
        Dim intCounter As Integer
        Dim intCounter2 As Integer
        Dim CellEqui As String
        Dim Cell As TableCell
        Dim boolChange As Boolean
        Dim chkSperre As CheckBox = Nothing
        Dim chkBezahlt As CheckBox = Nothing
        Dim chkAbmeld As CheckBox = Nothing
        Dim chkAnfordern As CheckBox = Nothing

        For intCounter = 0 To DataGrid1.Items.Count - 1
            boolChange = False
            CellEqui = DataGrid1.Items(intCounter).Cells(0).Text
            Cell = DataGrid1.Items(intCounter).Cells(14)
            chkSperre = CType(Cell.FindControl("chbSperren"), CheckBox)
            chkBezahlt = CType(Cell.FindControl("chbBzKennz"), CheckBox)
            chkAbmeld = CType(Cell.FindControl("chbAbmeld"), CheckBox)
            chkAnfordern = CType(Cell.FindControl("chbAnfordern"), CheckBox)
            Dim RowArray() As DataRow
            Dim RowArrayChange() As DataRow
            With mObjBriefanforderung
                If .Fahrzeuge.Select("EQUNR='" & CellEqui & "'")(0)("Versandsperre").ToString <> BooltoX(chkSperre.Checked) Then
                    boolChange = True
                End If
                If .Fahrzeuge.Select("EQUNR='" & CellEqui & "'")(0)("Anfordern").ToString <> BooltoX(chkAnfordern.Checked) Then
                    boolChange = True
                End If
                If .Fahrzeuge.Select("EQUNR='" & CellEqui & "'")(0)("Bezahltkennzeichen").ToString <> BooltoX(chkBezahlt.Checked) Then
                    boolChange = True
                End If
                If .Fahrzeuge.Select("EQUNR='" & CellEqui & "'")(0)("Abmelden").ToString <> BooltoX(Not chkAbmeld.Checked) Then
                    boolChange = True
                End If
                If boolChange = True Then
                    RowArray = .Fahrzeuge.Select("EQUNR='" & CellEqui & "'")
                    If RowArray.Length = 1 Then
                        If .FahrzeugeChanged Is Nothing Then
                            .FahrzeugeChanged = .Fahrzeuge.Clone
                        End If
                        RowArrayChange = .FahrzeugeChanged.Select("EQUNR='" & CellEqui & "'")
                        If RowArrayChange.Length = 0 Then
                            For Each DataRow1 As DataRow In RowArray
                                Dim NewRow As DataRow = .FahrzeugeChanged.NewRow
                                For intCounter2 = 0 To .FahrzeugeChanged.Columns.Count - 1
                                    NewRow(intCounter2) = DataRow1(intCounter2)
                                Next
                                NewRow("Versandsperre") = BooltoX(chkSperre.Checked)
                                NewRow("Abmelden") = BooltoX(chkAbmeld.Checked)
                                NewRow("Anfordern") = BooltoX(chkAnfordern.Checked)
                                NewRow("Bezahltkennzeichen") = BooltoX(chkBezahlt.Checked)
                                .FahrzeugeChanged.Rows.Add(NewRow)
                                .FahrzeugeChanged.AcceptChanges()
                            Next
                        ElseIf RowArrayChange.Length = 1 Then
                            For Each DataRow1 As DataRow In RowArrayChange
                                DataRow1("Versandsperre") = BooltoX(chkSperre.Checked)
                                DataRow1("Abmelden") = BooltoX(chkAbmeld.Checked)
                                DataRow1("Anfordern") = BooltoX(chkAnfordern.Checked)
                                DataRow1("Bezahltkennzeichen") = BooltoX(chkBezahlt.Checked)
                                .FahrzeugeChanged.AcceptChanges()
                            Next
                        End If
                    End If
                Else
                    If Not .FahrzeugeChanged Is Nothing Then
                        RowArray = .FahrzeugeChanged.Select("EQUNR='" & CellEqui & "'")
                        If RowArray.Length > 0 Then
                            .FahrzeugeChanged.Rows.Remove(RowArray(0))
                        End If

                    End If
                End If



            End With

        Next
        If Not mObjBriefanforderung.FahrzeugeChanged Is Nothing Then
            If mObjBriefanforderung.FahrzeugeChanged.Rows.Count = 0 Then
                mObjBriefanforderung.FahrzeugeChanged = Nothing
            End If
        End If

    End Sub

    Protected Sub chbAbmeld_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Protected Sub imgbExcel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbExcel.Click
        Try
            Dim control As New Control
            Dim tblTranslations As New DataTable()
            Dim tblTemp As New DataTable()
            Dim AppURL As String
            Dim col As DataGridColumn
            Dim col2 As DataColumn
            Dim bVisibility As Integer
            Dim i As Integer
            Dim sColName As String = ""

            AppURL = Replace(Me.Request.Url.LocalPath, "/Portal", "..")
            tblTranslations = CType(Me.Session(AppURL), DataTable)
            tblTemp = mObjBriefanforderung.Fahrzeuge.Copy

            For Each drow As DataRow In tblTemp.Rows
                If drow("Abmelden").ToString = "X" Then
                    drow("Abmelden") = " "
                Else
                    drow("Abmelden") = "X"
                End If
            Next


            For Each col In DataGrid1.Columns
                For i = tblTemp.Columns.Count - 1 To 0 Step -1
                    bVisibility = 0
                    col2 = tblTemp.Columns(i)
                    If col2.ColumnName.ToUpper = col.SortExpression.ToUpper OrElse col2.ColumnName.ToUpper = col.HeaderText.ToUpper.Replace("COL_", "") Then
                        sColName = TranslateColLbtn(DataGrid1, tblTranslations, col.HeaderText, bVisibility)
                        If bVisibility = 0 Then
                            tblTemp.Columns.Remove(col2)
                        ElseIf sColName.Length > 0 Then
                            col2.ColumnName = sColName
                        End If
                    End If
                    'EQUNR nicht mit in Excel ausgeben, wird meist nur als boundcolumn versteckt als schlüssel verwendet JJU2008.10.23
                    If col2.ColumnName.ToUpper = "EQUNR" Then
                        tblTemp.Columns.Remove(col2)
                    End If
                    If col2.ColumnName.ToUpper = "ERRORMESSAGE" Then
                        tblTemp.Columns.Remove(col2)
                    End If
                Next
                tblTemp.AcceptChanges()
            Next
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Me.Page)
        Catch ex As Exception
            lblError.Text = "Beim erstellen des Exceldatei ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

End Class

' ************************************************
' $History: Change02_1.aspx.vb $
' 
' *****************  Version 12  *****************
' User: Fassbenders  Date: 23.03.10   Time: 16:16
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 24.06.09   Time: 10:00
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA: 2939
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 23.06.09   Time: 15:05
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 15.06.09   Time: 13:41
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA:2804
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 2.06.09    Time: 14:19
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA: 2804
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 20.05.09   Time: 17:01
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 29.04.09   Time: 16:53
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' Warnungen
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 20.03.09   Time: 14:21
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 11.03.09   Time: 14:22
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA: 2671
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 9.03.09    Time: 17:25
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA: 2671
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 9.03.09    Time: 9:05
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 12.02.09   Time: 17:40
' Created in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2596 im test
' 
' ************************************************