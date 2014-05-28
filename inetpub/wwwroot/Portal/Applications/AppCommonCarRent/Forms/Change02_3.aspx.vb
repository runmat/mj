Option Explicit On
Option Strict On

Imports CKG
Imports CKG.Base.Common
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System.Drawing

Public Class Change02_3
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

    Private m_context As HttpContext = HttpContext.Current
    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private mObjBriefanforderung As Briefanforderung

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lb_zurueck As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lb_Auswahl As System.Web.UI.WebControls.LinkButton

    Protected WithEvents ucStyles As Styles

    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label

    Protected WithEvents lb_weiter As LinkButton
    Protected WithEvents imgbExcel As ImageButton

    Protected WithEvents lblVersandAdressArtAnzeige As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersandadresseAnzeige As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersandartAnzeige As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersandgrundAnzeige As System.Web.UI.WebControls.Label

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

            If Not IsPostBack Then
                If Refferer Is Nothing Then
                    If Not Me.Request.UrlReferrer Is Nothing Then
                        Refferer = Me.Request.UrlReferrer.ToString
                    Else
                        Refferer = ""
                    End If
                End If
                GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
                Select Case Me.Request.QueryString.Item("Art").ToUpper
                    Case "ENDG"
                        lblHead.Text = "Endgültiger Dokumentenversand"
                    Case "TEMP"
                        lblHead.Text = "Temporärer Dokumentenversand"
                    Case Else
                        Throw New Exception("Fehlender Querystringparameter 'Art' Vorgang wird abgebrochen.")
                End Select
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
                FillGrid(0)
                FillOptionenUebersicht()
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

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If mObjBriefanforderung.Status = 0 Then

            If IsNothing(mObjBriefanforderung.FahrzeugeChanged) OrElse mObjBriefanforderung.FahrzeugeChanged.Rows.Count = 0 Then

                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                DataGrid1.Visible = False
                lblNoData.Visible = True
                lblInfo.Text = "Anzahl: 0"
            Else

                DataGrid1.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As New DataView(mObjBriefanforderung.FahrzeugeChanged)
                'tmpDataView.RowFilter = "Anfordern='X'"

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
                Else
                    tmpDataView.Sort = "Kennzeichen asc"
                End If

                DataGrid1.CurrentPageIndex = intTempPageIndex

                lblInfo.Text = "Anzahl: " & tmpDataView.Count

                DataGrid1.DataSource = tmpDataView
                DataGrid1.DataBind()

                If DataGrid1.PageCount > 1 Then
                    DataGrid1.PagerStyle.CssClass = "PagerStyle"
                    DataGrid1.DataBind()
                    DataGrid1.PagerStyle.Visible = True
                Else
                    DataGrid1.PagerStyle.Visible = False
                End If

                If tmpDataView.Count > 0 Then imgbExcel.Visible = True

                For Each item As DataGridItem In DataGrid1.Items
                    If Not item.FindControl("lnkFahrgestellnummer") Is Nothing Then
                        CType(item.FindControl("lnkFahrgestellnummer"), HyperLink).NavigateUrl = "../../../Components/ComCommon/Finance/Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN=" & CType(item.FindControl("lnkFahrgestellnummer"), HyperLink).Text
                    End If
                Next


            End If
        Else
            lblError.Text = mObjBriefanforderung.Message
            Exit Sub
        End If

    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        If e.CommandName = "entfernen" Then
            Dim RowArray() As DataRow
            RowArray = mObjBriefanforderung.FahrzeugeChanged.Select("EQUNR='" & e.CommandArgument.ToString & "'")
            If RowArray.Length = 1 Then
                mObjBriefanforderung.FahrzeugeChanged.Rows.Remove(RowArray(0))
            End If

            FillGrid(DataGrid1.CurrentPageIndex)
        End If
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
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


    Private Sub FillOptionenUebersicht()

        Dim tmpString As String = ""
        Select Case mObjBriefanforderung.VersandEmpfängerArt
            Case "Anschrift"
                tmpString = "manuelle Adresse"
            Case "Partner"
                tmpString = "Partneradresse"
            Case "Geschaeft"
                tmpString = "Geschäftsstelle"
        End Select
        lblVersandAdressArtAnzeige.Text = tmpString

        lblVersandadresseAnzeige.Text = mObjBriefanforderung.VersandAdressText
        lblVersandartAnzeige.Text = mObjBriefanforderung.MaterialText
    End Sub

    Protected Sub lb_weiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_weiter.Click
        Dim tmpVerlaengerung As String = ""
        Try
            'die schleife für alle vorgänge wird in der Briefanforderung ausgeführt. JJU20081029
            mObjBriefanforderung.change(Session("AppID").ToString, Session.SessionID, Me, Me.Request.QueryString.Item("Art"))
            If mObjBriefanforderung.Status = 0 Then
                imgbExcel.Visible = True
                For Each tmpItem As DataGridItem In DataGrid1.Items
                    Dim tmpRow As DataRow = mObjBriefanforderung.Fahrzeuge.Select("EQUNR='" & tmpItem.Cells(0).Text & "'")(0)

                    Dim tmpLabel As Label
                    tmpLabel = CType(tmpItem.FindControl("lblMessage"), Label)
                    tmpLabel.Visible = True
                    tmpItem.FindControl("lbEntfernen").Visible = False

                    If mObjBriefanforderung.Status = 0 Then
                        tmpLabel.Text = "<nobr><img src=../../../Images/erfolg.gif border=0> Vorgang OK</nobr>"
                        tmpLabel.ForeColor = Color.Green
                    Else
                        tmpLabel.Text = "<img src=../../../Images/fehler.gif border=0> " & mObjBriefanforderung.Message
                        tmpLabel.ForeColor = Color.Red
                    End If
                Next
            Else
                lblError.Text = mObjBriefanforderung.Message
                Exit Sub
            End If
        Catch ex As Exception
            lblError.Text = "Beim Updatevorgang ist ein Fehler aufgetreten: " & ex.Message
        Finally
            lb_weiter.Enabled = False
            lb_zurueck.Enabled = False
        End Try
    End Sub
    Protected Sub lb_Auswahl_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Auswahl.Click
        Refferer = Nothing
        Dim Parameterlist As String = Me.Request.QueryString.Item("Art").ToUpper
        Response.Redirect("Change02.aspx?AppID=" & Session("AppID").ToString & "&Art=" & Parameterlist)
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
            tblTemp = mObjBriefanforderung.FahrzeugeChanged.Copy
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
' $History: Change02_3.aspx.vb $
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 24.06.09   Time: 10:00
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA: 2939
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 18.06.09   Time: 12:34
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA: 2804
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 10.06.09   Time: 14:36
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA: 2804
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 2.06.09    Time: 14:19
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA: 2804
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 20.05.09   Time: 17:01
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 13.05.09   Time: 15:45
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 29.04.09   Time: 16:53
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' Warnungen
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 9.03.09    Time: 17:25
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA: 2671
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 12.02.09   Time: 17:40
' Created in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2596 im test
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 28.11.08   Time: 9:14
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' fehlerhafte rückwärtsnavigation fixed
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 29.10.08   Time: 16:27
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2284 unfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 22.10.08   Time: 11:15
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2284 Weiterentwicklung
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 21.10.08   Time: 17:11
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2284 weiterentwicklung
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 17.10.08   Time: 10:44
' Created in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2284 torso
' 
' ************************************************