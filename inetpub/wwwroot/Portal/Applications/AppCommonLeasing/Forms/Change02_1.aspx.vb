Option Explicit On
Option Strict On
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

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

    Protected WithEvents lb_weiter As LinkButton


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
            Else
                SaveGridSelection()
            End If


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub responseBack()
        If Refferer = "" Then
            'Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            'Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            Response.Redirect(Refferer)
        End If
    End Sub


    Private Sub SaveGridSelection()

        For Each item As DataGridItem In DataGrid1.Items
            If CType(item.FindControl("chbAnfordern"), CheckBox).Checked Then
                mObjBriefanforderung.Fahrzeuge.Select("EQUNR='" & item.Cells(0).Text & "'")(0)("Anfordern") = "X"
            Else
                mObjBriefanforderung.Fahrzeuge.Select("EQUNR='" & item.Cells(0).Text & "'")(0).RejectChanges()
            End If

        Next

    End Sub

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

                Dim tmpDataView As New DataView(mObjBriefanforderung.Fahrzeuge)


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


                For Each item As DataGridItem In DataGrid1.Items
                    'If Not item.FindControl("lnkFahrgestellnummer") Is Nothing Then
                    '    CType(item.FindControl("lnkFahrgestellnummer"), HyperLink).NavigateUrl = "../../../Components/ComCommon/Finance/Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN=" & CType(item.FindControl("lnkFahrgestellnummer"), HyperLink).Text
                    'End If

                    If item.FindControl("Literal1") Is Nothing Then
                        For Each Control As Control In item.Cells(2).Controls
                            Control.Visible = False
                        Next
                    Else
                        Dim lit As Literal = CType(item.FindControl("Literal1"), Literal)

                        If lit.Text.Length = 0 Then
                            'item.Cells(2).Visible = False
                            For Each Control As Control In item.Cells(2).Controls
                                Control.Visible = False
                            Next
                        End If

                    End If

                Next

                'farbe der Zeilen Ändern wenn temp oder endg JJU20081106
                Select Case Me.Request.QueryString.Item("Art").ToUpper
                    Case "ENDG"
                        DataGrid1.BackColor = Drawing.Color.FromArgb(244, 164, 96)
                    Case "TEMP"
                       DataGrid1.BackColor = Drawing.Color.FromArgb(135, 206, 250)
                    Case Else
                        Throw New Exception("Fehlender Querystringparameter 'Art' Vorgang wird abgebrochen.")
                End Select


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
        Response.Redirect("Change02.aspx?AppID=" & Session("AppID").ToString & "&Art=" & Me.Request.QueryString.Item("Art"))
    End Sub

    Protected Sub lb_weiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_weiter.Click
        If mObjBriefanforderung.Fahrzeuge.GetChanges() Is Nothing Then
            lblError.Text = "Sie haben keine Auswahl vorgenommen"
            Exit Sub
        Else
            mObjBriefanforderung.callBapiForAdressen()
            If mObjBriefanforderung.Status = 0 Then
                Dim Parameterlist As String = ""
                HelpProcedures.getAppParameters(Session("AppID").ToString, Parameterlist, ConfigurationManager.AppSettings.Get("Connectionstring"))
                Response.Redirect("Change02_2.aspx?AppID=" & Session("AppID").ToString & Parameterlist)
            Else
                lblError.Text = mObjBriefanforderung.Message

            End If
        End If
    End Sub
End Class

' ************************************************
' $History: Change02_1.aspx.vb $
' 
' *****************  Version 14  *****************
' User: Fassbenders  Date: 17.09.09   Time: 12:51
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' 
' *****************  Version 13  *****************
' User: Fassbenders  Date: 10.09.09   Time: 15:16
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA: 3050, 3056
' 
' *****************  Version 12  *****************
' User: Fassbenders  Date: 8.09.09    Time: 13:37
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA: 3056
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 29.04.09   Time: 17:01
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' Warnungen
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 3.12.08    Time: 16:26
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2446 testfertig
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 28.11.08   Time: 9:14
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' fehlerhafte rückwärtsnavigation fixed
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 6.11.08    Time: 13:34
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2365,2367,2362
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 30.10.08   Time: 13:41
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' testfertig
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 29.10.08   Time: 16:27
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2284 unfertig
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 27.10.08   Time: 17:20
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2286 Änderungen
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 22.10.08   Time: 11:15
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2284 Weiterentwicklung
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 21.10.08   Time: 17:11
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2284 weiterentwicklung
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 20.10.08   Time: 15:37
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' 2284
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 17.10.08   Time: 10:44
' Created in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2284 torso
' 
' ************************************************