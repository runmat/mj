Option Explicit On
Option Strict On

Imports CKG
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements


Public Class Report03_1
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
    Private mObjSuche As Search
    Private mObjInanspruchnahme As Inanspruchnahme



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

            If mObjInanspruchnahme Is Nothing Then
                If Session("mObjInanspruchnahmeSession") Is Nothing Then
                    Throw New Exception("benötigtiges SessionObjekt nicht vorhanden")
                Else
                    mObjInanspruchnahme = CType(Session("mObjInanspruchnahmeSession"), Inanspruchnahme)
                End If
            End If


            If mObjSuche Is Nothing Then
                If Session("mObjSucheSession") Is Nothing Then
                    Throw New Exception("benötigtiges SessionObjekt nicht vorhanden")
                Else
                    mObjSuche = CType(Session("mObjSucheSession"), Search)
                End If
            End If




            'seitenspeziefische Aktionen
            If Not IsPostBack Then
                If Session("mObjSucheSession") Is Nothing Then
                    'wenn kein konkreter Händler angegeben worden ist, alle offenen Anforderungen anzeigen
                    'somit entfällt das füllen der Kopfdaten
                    Kopfdaten1.Visible = False
                Else
                    'wenn ein Konkreter Händler ausgwählt, dann kopfdaten und Kontingente füllen
                    Kopfdaten1.Visible = True


                    'Kopfdatenfüllen
                    Kopfdaten1.UserReferenz = m_User.Reference
                    Kopfdaten1.HaendlerNummer = mObjSuche.REFERENZ
                    Dim strTemp As String = mObjSuche.NAME
                    If mObjSuche.NAME_2.Length > 0 Then
                        strTemp &= "<br>" & mObjSuche.NAME_2
                    End If
                    Kopfdaten1.HaendlerName = strTemp
                    Kopfdaten1.Adresse = mObjSuche.COUNTRYISO & " - " & mObjSuche.POSTL_CODE & " " & mObjSuche.CITY & "<br>" & mObjSuche.STREET
                    Kopfdaten1.Kontingente = mObjSuche.Kontingente

                    'dropDownListe füllen
                    ddlPageSize.Items.Add("10")
                    ddlPageSize.Items.Add("20")
                    ddlPageSize.Items.Add("50")
                    ddlPageSize.Items.Add("100")
                    ddlPageSize.Items.Add("200")
                    ddlPageSize.Items.Add("500")
                    ddlPageSize.Items.Add("1000")
                    ddlPageSize.SelectedIndex = 2
                    If Request.QueryString("HDL") = "1" Then
                        Session("AppShowNot") = True
                    End If

                    FillGrid(0)
                End If
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
        If mObjInanspruchnahme.Status = 0 Then

            If IsNothing(mObjInanspruchnahme.Equipments) OrElse mObjInanspruchnahme.Equipments.Rows.Count = 0 Then

                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                DataGrid1.Visible = False
                lblNoData.Visible = True
            Else

                DataGrid1.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As New DataView(mObjInanspruchnahme.Equipments)


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
                    DataGrid1.PagerStyle.Visible = True
                Else
                    DataGrid1.PagerStyle.Visible = False
                End If

                For Each item As DataGridItem In DataGrid1.Items
                    If Not item.FindControl("lnkHistorie") Is Nothing Then
                        If Not m_User.Applications.Select("AppName = 'Report46'").Count = 0 Then
                            CType(item.FindControl("lnkHistorie"), HyperLink).NavigateUrl = "../../../Components/ComCommon/Finance/Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN=" & CType(item.FindControl("lnkHistorie"), HyperLink).Text
                        End If
                    End If
                Next

            End If
        Else
            lblError.Text = mObjInanspruchnahme.Message
            Exit Sub
        End If

    End Sub


    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid1.PreRender
        System.Diagnostics.Debug.WriteLine("")
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
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

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
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
            tblTemp = mObjInanspruchnahme.Equipments.Copy
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
                        Else
                            'alle spalten die nicht in der spaltenübersetzung sind, entfernen
                            tblTemp.Columns.Remove(col2)
                        End If
                    End If
                    If col2.ColumnName = "EQUNR" Then
                        tblTemp.Columns.Remove(col2)
                    End If
                Next

                tblTemp.AcceptChanges()
            Next
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Me.Page)
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub
End Class

' ************************************************
' $History: Report03_1.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 30.04.09   Time: 11:39
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2837
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 31.03.09   Time: 15:07
' Updated in $/CKAG/Applications/AppF1/forms
' Excedownload anpassungen
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 30.03.09   Time: 17:47
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2668 nachbesserungen
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 24.03.09   Time: 15:35
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2668 nachbesserungen
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 12.03.09   Time: 13:08
' Created in $/CKAG/Applications/AppF1/forms
' ITA 2668, 2688
' 
' ************************************************
