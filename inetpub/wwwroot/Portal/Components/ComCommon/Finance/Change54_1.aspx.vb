Option Explicit On
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Components.ComCommon

Public Class Change54_1
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
    Private mObjSuche As CKG.Components.ComCommon.Finance.Search
    Private objBank As CKG.Components.ComCommon.BankBaseCredit
    Private mObjInanspruchnahme As Inanspruchnahme

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Kopfdaten1 As CKG.Components.ComCommon.PageElements.Kopfdaten
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lb_zurueck As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucStyles As Styles

    Protected WithEvents lb_weiter As LinkButton


#Region "Properties"

    Private Property Refferer(Optional ByVal realResponse As Boolean = False) As String
        Get
            If Not realResponse Then
                If Not Session.Item(Me.Request.Url.LocalPath & "Refferer") Is Nothing Then

                    If Not Me.Request.UrlReferrer.ToString.ToUpper.Contains("Components/ComCommon/Finance/Change54_2.aspx".ToUpper) Then
                        'wenn es nicht ein rücksprung von der nachfolge seite ist, dieses ersetzen, da die herkunft unterschiedlich sein kann.
                        Return Nothing
                    Else
                        Return Session.Item(Me.Request.Url.LocalPath & "Refferer").ToString()
                    End If
                Else : Return Nothing
                End If
            Else
                Return Session.Item(Me.Request.Url.LocalPath & "Refferer").ToString()
            End If
        End Get
        Set(ByVal value As String)
            Session.Item(Me.Request.Url.LocalPath & "Refferer") = value
        End Set
    End Property



#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen

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
                mObjSuche = CType(Session("mObjSucheSession"), CKG.Components.ComCommon.Finance.Search)
            End If
        End If

        If objBank Is Nothing Then
            If Not Session("objBank") Is Nothing Then
                objBank = CType(Session("objBank"), CKG.Components.ComCommon.BankBaseCredit)
            End If

        End If



        'seitenspeziefische Aktionen
        If Not IsPostBack Then
            If mObjInanspruchnahme.Haendler Is Nothing OrElse mObjInanspruchnahme.Haendler.Trim Is String.Empty Then
                'wenn kein konkreter Händler angegeben worden ist, alle offenen Anforderungen anzeigen
                'somit entfällt das füllen der Kopfdaten
                Kopfdaten1.Visible = False
            Else
                'wenn ein Konkreter Händler ausgwählt, dann kopfdaten und Kontingente füllen
                Kopfdaten1.Visible = True

                If Not mObjSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, mObjInanspruchnahme.Haendler) Then
                    lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & mObjSuche.ErrorMessage & ")"
                    Exit Sub
                End If

                'Kopfdatenfüllen
                Kopfdaten1.UserReferenz = m_User.Reference
                Kopfdaten1.HaendlerNummer = mObjInanspruchnahme.Haendler
                Dim strTemp As String = mObjSuche.NAME
                If mObjSuche.NAME_2.Length > 0 Then
                    strTemp &= "<br>" & mObjSuche.NAME_2
                End If
                Kopfdaten1.HaendlerName = strTemp
                Kopfdaten1.Adresse = mObjSuche.COUNTRYISO & " - " & mObjSuche.POSTL_CODE & " " & mObjSuche.CITY & "<br>" & mObjSuche.STREET

                'bankObjekt für Kontingente instanziieren 
                objBank = New BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                objBank.Customer = mObjSuche.REFERENZ
                objBank.KUNNR = m_User.KUNNR
                objBank.CreditControlArea = "ZDAD"
                objBank.Show(Session("AppID").ToString, Session.SessionID) 'kontingentetabelle füllen
                Session("objBank") = objBank
                Kopfdaten1.Kontingente = objBank.Kontingente 'kontingente anzeigen

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
        Else 'wenn postback
            saveActionSelection()
        End If
    End Sub

    Private Sub responseBack()
        If Refferer(True) = "" Then
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            Response.Redirect(Refferer(True))
        End If
    End Sub


    Private Sub saveActionSelection()
        For Each tmpitem As DataGridItem In DataGrid1.Items
            mObjInanspruchnahme.Equipments.Select("EQUNR='" & tmpitem.Cells(0).Text & "'")(0)("Aktion") = CType(tmpitem.FindControl("ddlAktion"), DropDownList).SelectedValue.ToString
        Next
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
            End If
        Else
            lblError.Text = mObjInanspruchnahme.Message
            Exit Sub
        End If

    End Sub

    Private Sub DataGrid1_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemCreated
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            If mObjInanspruchnahme Is Nothing Then
                If Session("mObjInanspruchnahmeSession") Is Nothing Then
                    Throw New Exception("benötigtiges SessionObjekt nicht vorhanden")
                Else
                    mObjInanspruchnahme = CType(Session("mObjInanspruchnahmeSession"), Inanspruchnahme)
                End If
            End If
            With CType(e.Item.FindControl("ddlAktion"), DropDownList)
                .DataSource = mObjInanspruchnahme.AktionCodes
                .DataTextField = "Text"
                .DataValueField = "Wert"
            End With
        End If
    End Sub

    Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            CType(e.Item.FindControl("ddlAktion"), DropDownList).SelectedValue = mObjInanspruchnahme.Equipments.Select("EQUNR='" & e.Item.Cells(0).Text & "'")(0)("Aktion").ToString
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

    Protected Sub lb_weiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_weiter.Click
        If mObjInanspruchnahme.Equipments.Select("Aktion<>''").Length = 0 Then
            lblError.Text = "keine Aktionen ausgewählt"
            Exit Sub
        Else
            Response.Redirect("Change54_2.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub
End Class

' ************************************************
' $History: Change54_1.aspx.vb $
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 22.06.09   Time: 13:18
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 Z_M_Creditlimit_Detail_001
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 19.06.09   Time: 10:10
' Updated in $/CKAG/Components/ComCommon/Finance
' ITa 2918 testfertig
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:50
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 25.11.08   Time: 10:51
' Updated in $/CKAG/Components/ComCommon/Finance
' Bugfix Inanspruchnahme
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 18.11.08   Time: 13:49
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2263 testfertig
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 10.10.08   Time: 11:48
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2246 unfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 9.10.08    Time: 14:51
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2246 unfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 8.10.08    Time: 10:18
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2246 unfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 7.10.08    Time: 16:44
' Created in $/CKAG/Components/ComCommon/Finance
' ITA 2246 Torso
' 
' ************************************************
