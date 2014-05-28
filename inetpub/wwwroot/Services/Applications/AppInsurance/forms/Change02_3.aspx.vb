Option Explicit On
Option Strict On


Imports CKG
Imports BusyBoxDotNet
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
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

    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lbBack As System.Web.UI.WebControls.LinkButton




    Protected WithEvents lb_weiter As LinkButton
    Protected WithEvents imgbExcel As ImageButton
    Protected WithEvents GridNavigation1 As Services.GridNavigation

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
            FormAuth(Me, m_User)
            GridNavigation1.setGridElment(DataGrid1)

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

            End If

            If mObjBriefanforderung Is Nothing Then
                If Session("mObjBriefanforderungSession") Is Nothing Then
                    Throw New Exception("benötigtiges SessionObjekt nicht vorhanden")
                Else
                    mObjBriefanforderung = CType(Session("mObjBriefanforderungSession"), Briefanforderung)
                End If
            End If

            If Not IsPostBack Then

                FillGrid(0)
                FillOptionenUebersicht()
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
        If mObjBriefanforderung.Status = 0 Then

            If IsNothing(mObjBriefanforderung.Fahrzeuge) OrElse mObjBriefanforderung.Fahrzeuge.Rows.Count = 0 Then

                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                DataGrid1.Visible = False
                lblNoData.Visible = True
            Else

                DataGrid1.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As New DataView(mObjBriefanforderung.Fahrzeuge)
                tmpDataView.RowFilter = "Anfordern='X'"

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

                For Each item As DataGridItem In DataGrid1.Items
                    If Not item.FindControl("lnkFahrgestellnummer") Is Nothing Then
                        If Not m_User.Applications.Select("AppName = 'Report46'").Length = 0 Then
                            CType(item.FindControl("lnkFahrgestellnummer"), HyperLink).NavigateUrl = "../../../Components/ComCommon/Finance/Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN=" & CType(item.FindControl("lnkFahrgestellnummer"), HyperLink).Text
                        End If
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
            mObjBriefanforderung.Fahrzeuge.Select("EQUNR='" & e.CommandArgument.ToString & "'")(0)("Anfordern") = ""
            FillGrid(DataGrid1.CurrentPageIndex)
        End If
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
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
        lblVersandgrundAnzeige.Text = mObjBriefanforderung.Abrufgruende.Select("SapWert='" & mObjBriefanforderung.Versandgrund & "'")(0)("WebBezeichnung").ToString & " " & mObjBriefanforderung.VersandgrundZusatztext
    End Sub

    Protected Sub lb_weiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_weiter.Click
        Dim tmpVerlaengerung As String = ""
        Try
            'die schleife für alle vorgänge wird in der Briefanforderung ausgeführt. JJU20081029
            mObjBriefanforderung.change(Me.Request.QueryString.Item("Art"))
            If mObjBriefanforderung.Status = 0 Then
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
            lbBack.Enabled = False
        End Try
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub
End Class

' ************************************************
' $History: Change02_3.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 28.04.09   Time: 13:58
' Updated in $/CKAG2/Applications/AppGenerali/forms
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 14.04.09   Time: 17:05
' Updated in $/CKAG2/Applications/AppGenerali/forms
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 27.02.09   Time: 14:31
' Created in $/CKAG2/Applications/AppGenerali/forms
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 28.11.08   Time: 9:14
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' fehlerhafte rückwärtsnavigation fixed
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 29.10.08   Time: 16:27
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2284 unfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 22.10.08   Time: 11:15
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2284 Weiterentwicklung
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 21.10.08   Time: 17:11
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2284 weiterentwicklung
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 17.10.08   Time: 10:44
' Created in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2284 torso
' 
' ************************************************