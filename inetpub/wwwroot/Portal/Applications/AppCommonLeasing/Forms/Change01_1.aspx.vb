Option Explicit On
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System.Drawing

Public Class Change01_1
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

    Private mObjUeberfaelligeRuecksendungen As UeberfaelligeRuecksendungen

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lb_zurueck As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucStyles As Styles

    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label

    Protected WithEvents imgbExcel As ImageButton




#Region "Properties"

    Private Property Refferer() As String
        Get
            If Not Session.Item(Me.Request.Url.LocalPath & "Refferer") Is Nothing Then
                Return Session.Item(Me.Request.Url.LocalPath & "Refferer").ToString()
            Else : Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            Session.Item(Me.Request.Url.LocalPath & "Refferer") = value
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

            If Not IsPostBack Then
                'in diesem fall im referrer neu setzen, da keine folgeseite und 
                'die gleiche applikation von 2 verschiedenen standpunkten aus startet
                If Not Me.Request.UrlReferrer Is Nothing Then
                    Refferer = Me.Request.UrlReferrer.ToString
                Else
                    Refferer = ""
                End If

                GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                ucStyles.TitleText = lblHead.Text
            End If

            If mObjUeberfaelligeRuecksendungen Is Nothing Then
                If Session("mObjUeberfaelligeRuecksendungenSession") Is Nothing Then
                    Throw New Exception("benötigtiges SessionObjekt nicht vorhanden")
                Else
                    mObjUeberfaelligeRuecksendungen = CType(Session("mObjUeberfaelligeRuecksendungenSession"), UeberfaelligeRuecksendungen)
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
        If Refferer = "" Then
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            Response.Redirect(Refferer)
        End If
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If mObjUeberfaelligeRuecksendungen.Status = 0 Then

            If IsNothing(mObjUeberfaelligeRuecksendungen.Vorgaenge) OrElse mObjUeberfaelligeRuecksendungen.Vorgaenge.Rows.Count = 0 Then

                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                DataGrid1.Visible = False
                lblNoData.Visible = True
                lblInfo.Text = "Anzahl: 0"
            Else

                DataGrid1.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As New DataView(mObjUeberfaelligeRuecksendungen.Vorgaenge)


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

                lblInfo.Text = "Anzahl: " & tmpDataView.Count

                DataGrid1.CurrentPageIndex = intTempPageIndex

                DataGrid1.DataSource = tmpDataView

                DataGrid1.DataBind()
            End If
        Else
            lblError.Text = mObjUeberfaelligeRuecksendungen.Message
            Exit Sub
        End If

    End Sub

    Private Sub DataGrid1_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.EditCommand
        DataGrid1.EditItemIndex = e.Item.ItemIndex
        FillGrid(DataGrid1.CurrentPageIndex)
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        Try
            Dim doAction As Boolean = False
      
            If e.CommandName = "Speichern" Then
                mObjUeberfaelligeRuecksendungen.Vorgaenge.Select("EQUNR='" & e.CommandArgument.ToString & "'")(0)("Memo") = CType(e.Item.FindControl("txtMemo"), TextBox).Text
                doAction = True
            End If

            If e.CommandName = "Fristverlaengerung" Then
                mObjUeberfaelligeRuecksendungen.Vorgaenge.Select("EQUNR='" & e.CommandArgument.ToString & "'")(0)("Faelligkeitsdatum") = "Fristverlängerung beauftragt"
                doAction = True
            End If



            If doAction Then
                'bei irgendeiner aktion soll immer sofort ins sap übermittelt werden!
                Dim tmpGridItem As DataGridItem = Nothing
                For Each item As DataGridItem In DataGrid1.Items
                    If item.Cells(0).Text = e.CommandArgument.ToString Then
                        tmpGridItem = item
                    End If
                Next

                Dim tmpVerlaengerung As String

                Dim tmpRow As DataRow = mObjUeberfaelligeRuecksendungen.Vorgaenge.Select("EQUNR='" & e.CommandArgument.ToString & "'")(0)

                'nur geänderte Rows sollen ans sap gesendet werden
                If Not tmpRow.RowState = DataRowState.Unchanged Then

                    If tmpRow("Memo").ToString.Length > 200 Then
                        lblError.Text = "Das Memo der Fahrgestellnummer: " & tmpRow("Fahrgestellnummer").ToString & " ist länger als 200 Zeichen"
                        Exit Sub
                    End If


                    tmpVerlaengerung = ""
                    If tmpRow("Faelligkeitsdatum").ToString = "Fristverlängerung beauftragt" Then
                        tmpVerlaengerung = "X"
                    End If

                    mObjUeberfaelligeRuecksendungen.Change(tmpRow("EQUNR").ToString, tmpRow("Memo").ToString, tmpVerlaengerung)

                    If Not mObjUeberfaelligeRuecksendungen.Status = 0 Then
                        lblError.Text = mObjUeberfaelligeRuecksendungen.Message
                    End If
                End If
            End If

            DataGrid1.EditItemIndex = -1
            FillGrid(DataGrid1.CurrentPageIndex)
        Catch ex As Exception
            lblError.Text = "Beim Updatevorgang ist ein Fehler aufgetreten: " & ex.Message
        End Try
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
            tblTemp = mObjUeberfaelligeRuecksendungen.Vorgaenge.Copy
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

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        DataGrid1.EditItemIndex = -1
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

End Class

' ************************************************
' $History: Change01_1.aspx.vb $
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 29.04.09   Time: 17:01
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' Warnungen
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 3.12.08    Time: 17:54
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2440 testfertig
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 6.11.08    Time: 13:34
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2365,2367,2362
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 29.10.08   Time: 16:27
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2284 unfertig
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 28.10.08   Time: 8:26
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' 2286 Änderungen
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 27.10.08   Time: 17:20
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2286 Änderungen
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 27.10.08   Time: 10:11
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' 2286, änderung stylsheet buttonTable, entfernen des Löschen buttons im
' grid
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 15.10.08   Time: 10:56
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2286 nachbesserungen
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 15.10.08   Time: 9:50
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2286 und hinzufügen von JBC,FW,Unicredit -styles
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 14.10.08   Time: 13:58
' Created in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2286 unfertig
' 
' ************************************************
