Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report03_1
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private m_objTable As DataTable

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents imgbExcel As ImageButton
    Private legende As String

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        If (Session("ResultTable") Is Nothing) Then
            Response.Redirect(Request.UrlReferrer.ToString)
        Else
            m_objTable = CType(Session("ResultTable"), DataTable)
        End If
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        Try
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
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

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If m_objTable.Rows.Count = 0 Then
            DataGrid1.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
        Else
            DataGrid1.Visible = True
            lblNoData.Visible = False

            Dim tmpDataView As New DataView()
            tmpDataView = m_objTable.DefaultView

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

            If (Not Session("ShowOtherString") Is Nothing) AndAlso CStr(Session("ShowOtherString")).Length > 0 Then
                lblNoData.Text = CStr(Session("ShowOtherString"))
            Else
                lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Einträge zu """ & m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString & """ gefunden."
            End If
            If (Not Session("BackLink") Is Nothing) AndAlso CStr(Session("BackLink")) = "HistoryBack" Then
                lnkKreditlimit.Text = "Zurück"
                lnkKreditlimit.NavigateUrl = "javascript:history.back()"
            End If
            lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If
        End If
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand

        Dim strTarget As String = ""
        Dim rows As DataRow()
        If e.CommandName = "Schilder" Then
            rows = m_objTable.Select("[KfzKennzeichen]='" & e.CommandArgument.ToString & "'")

            If rows.Length = 1 Then

                Dim mObjFehlendeAbmeldeunterlagen As FehlendeAbmeldeunterlagen

                mObjFehlendeAbmeldeunterlagen = CType(Session("mObjFehlendeAbmeldeunterlagenSession"), FehlendeAbmeldeunterlagen)

                With mObjFehlendeAbmeldeunterlagen

                    If Not rows(0)("KfzKennzeichen") Is Nothing AndAlso CStr(rows(0)("KfzKennzeichen")).Trim.Length > 0 Then

                        .Kennzeichen = rows(0)("KfzKennzeichen").ToString
                    End If

                    If Not rows(0)("Fahrzeugart") Is Nothing AndAlso CStr(rows(0)("Fahrzeugart")).Trim.Length > 0 Then

                        .FahrzeugUndAufbauart = rows(0)("Fahrzeugart").ToString
                    End If
                    If Not rows(0)("Hersteller") Is Nothing AndAlso CStr(rows(0)("Hersteller")).Trim.Length > 0 Then

                        .Hersteller = rows(0)("Fahrzeugart").ToString
                    End If
                    If Not rows(0)("TypSchluessel") Is Nothing AndAlso CStr(rows(0)("TypSchluessel")).Trim.Length > 0 Then

                        .TypUndAusfuehrung = rows(0)("TypSchluessel").ToString
                    End If
                    If Not rows(0)("Ausfuehrung") Is Nothing AndAlso CStr(rows(0)("Ausfuehrung")).Trim.Length > 0 Then

                        .TypUndAusfuehrung = rows(0)("Ausfuehrung").ToString
                    End If
                    If Not rows(0)("Fahrgestellnummer") Is Nothing AndAlso CStr(rows(0)("Fahrgestellnummer")).Trim.Length > 0 Then

                        .FIN = rows(0)("Fahrgestellnummer").ToString
                    End If
                    If Not rows(0)("Name") Is Nothing AndAlso CStr(rows(0)("Name")).Trim.Length > 0 Then

                        .Name = rows(0)("Name").ToString
                    End If
                    If Not rows(0)("Postleitzahl") Is Nothing AndAlso CStr(rows(0)("Postleitzahl")).Trim.Length > 0 Then

                        .Postleitzahl = CStr(rows(0)("Postleitzahl"))
                    End If
                    If Not rows(0)("Ort") Is Nothing AndAlso CStr(rows(0)("Ort")).Trim.Length > 0 Then

                        .Ort = CStr(rows(0)("Ort"))
                    End If
                    If Not rows(0)("Strasse") Is Nothing AndAlso CStr(rows(0)("Strasse")).Trim.Length > 0 Then

                        .Strasse = CStr(rows(0)("Strasse"))
                    End If
                    '§§§ JVE 26.01.2006 Checkboxen automatisch setzen.
                    If (rows(0)("AnzahlSchilder").ToString.Trim = "V") Then
                        .vorderes = "vorhanden"
                    End If
                    If (rows(0)("AnzahlSchilder").ToString.Trim = "H") Then
                        .hinteres = "vorhanden"
                    End If
                End With


                Dim imageHt As New Hashtable()
                imageHt.Add("Logo", m_User.Customer.LogoImage)
                Dim tblData As DataTable = Base.Kernel.Common.DataTableHelper.ObjectToDataTable(mObjFehlendeAbmeldeunterlagen)
                Dim docFactory As New Base.Kernel.DocumentGeneration.WordDocumentFactory(tblData, imageHt)
                docFactory.CreateDocument("Erklaerung_" & m_User.UserName, Me.Page, "\Applications\appCommonCarRent\docu\Kennzeichen.doc")
            End If


        ElseIf e.CommandName = "Schein" Then

            rows = m_objTable.Select("[KfzKennzeichen]='" & e.CommandArgument.ToString & "'")
            If rows.Length = 1 Then
                Dim mObjFehlendeAbmeldeunterlagen As FehlendeAbmeldeunterlagen

                mObjFehlendeAbmeldeunterlagen = CType(Session("mObjFehlendeAbmeldeunterlagenSession"), FehlendeAbmeldeunterlagen)

                With mObjFehlendeAbmeldeunterlagen

                    If Not rows(0)("KfzKennzeichen") Is Nothing AndAlso CStr(rows(0)("KfzKennzeichen")).Trim.Length > 0 Then
                        .Kennzeichen = CStr(rows(0)("KfzKennzeichen"))
                    End If

                    If Not rows(0)("Fahrgestellnummer") Is Nothing AndAlso CStr(rows(0)("Fahrgestellnummer")).Trim.Length > 0 Then

                        .FIN = rows(0)("Fahrgestellnummer").ToString
                    End If

                End With
                Dim imageHt As New Hashtable()
                imageHt.Add("Logo", m_User.Customer.LogoImage2)
                Dim tblData As DataTable = Base.Kernel.Common.DataTableHelper.ObjectToDataTable(mObjFehlendeAbmeldeunterlagen)
                Dim docFactory As New Base.Kernel.DocumentGeneration.WordDocumentFactory(tblData, imageHt)
                docFactory.CreateDocument("Erklaerung_" & m_User.UserName, Me.Page, "\Applications\appCommonCarRent\docu\Kennzeichen.doc")
            Else
                Me.lblError.Visible = True
            End If




        End If

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
            tblTemp = CType(Session("ExcelResult"), DataTable)
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

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

  
End Class

' ************************************************
' $History: Report03_1.aspx.vb $
' 
' *****************  Version 7  *****************
' User: Fassbenders  Date: 22.09.09   Time: 8:58
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA: 3157
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 29.04.09   Time: 16:53
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' Warnungen
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 23.02.09   Time: 11:54
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA2588
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 23.02.09   Time: 11:43
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' test
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 20.02.09   Time: 14:37
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2588
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 20.02.09   Time: 11:48
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ita 2588
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 16.02.09   Time: 14:19
' Created in $/CKAG/Applications/AppCommonCarRent/Forms
' ITa 2586/2588 unfertig
' 
'
' ************************************************
