Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System.Collections.Generic


Public Class Change05
    Inherits System.Web.UI.Page
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ddlPDIs As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlHersteller As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlModellID As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlSIPP As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlReifenart As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlFarbe As System.Web.UI.WebControls.DropDownList
    Protected WithEvents btnConfirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cbxAlle As System.Web.UI.WebControls.CheckBox
    Protected WithEvents rbAktion As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents cbxPDI As System.Web.UI.WebControls.CheckBox
    Protected WithEvents ddlNavi As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlAHK As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtAuftragsnummerVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAuftragsnummerBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFahrgestellnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblAnzahl As System.Web.UI.WebControls.Label

    Private strPDI As String

    Private Const strTaskZulassen As String = "Zulassen"
    Private Const strTaskSperren As String = "Sperren"
    Private Const strTaskEntsperren As String = "Entsperren"
    Private Const strTaskVerschieben As String = "Verschieben"
    Private objSuche As change_01

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

#Region "Events"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                '§§§ JVE 18.07.2006: Zunächst PDI-Liste laden...
                Initialload()
                FillFilters()
            Else
                objSuche = CType(Session("objSuche"), change_01)
            End If
        Catch ex As Exception
            lblError.Text = String.Format("Beim Laden der Seite ist ein Fehler aufgetreten.<br>({0})", ex.Message)
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click

        objSuche = CType(Session("objSuche"), change_01)

        'Flag für PDI-Bereitmeldung
        objSuche.Task = rbAktion.SelectedItem.Value

        'Sperrkennzeichen setzen
        If objSuche.Task = strTaskEntsperren Then
            objSuche.PSperre = " "c
        End If
        If objSuche.Task = strTaskSperren Then
            objSuche.PSperre = "X"c
        End If

        Session("objSuche") = objSuche
        Response.Redirect("Change05_0.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Protected Sub ddlAHK_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAHK.SelectedIndexChanged
        FillFilters()
    End Sub

    Protected Sub ddlNavi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlNavi.SelectedIndexChanged
        FillFilters()
    End Sub

    Protected Sub ddlFarbe_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFarbe.SelectedIndexChanged
        FillFilters()
    End Sub

    Protected Sub ddlReifenart_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlReifenart.SelectedIndexChanged
        FillFilters()
    End Sub

    Protected Sub ddlSIPP_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSIPP.SelectedIndexChanged
        FillFilters()
    End Sub

    Protected Sub ddlModellID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlModellID.SelectedIndexChanged
        FillFilters()
    End Sub

    Protected Sub ddlHersteller_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlHersteller.SelectedIndexChanged
        FillFilters()
    End Sub

    Protected Sub ddlPDIs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPDIs.SelectedIndexChanged
        FillFilters()
    End Sub

    Protected Sub txtAuftragsnummerVon_TextChanged(sender As Object, e As EventArgs) Handles txtAuftragsnummerVon.TextChanged
        FillFilters()
    End Sub

    Protected Sub txtAuftragsnummerBis_TextChanged(sender As Object, e As EventArgs) Handles txtAuftragsnummerBis.TextChanged
        FillFilters()
    End Sub

    Protected Sub txtFahrgestellnummer_TextChanged(sender As Object, e As EventArgs) Handles txtFahrgestellnummer.TextChanged
        FillFilters()
    End Sub

    Protected Sub rbAktion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbAktion.SelectedIndexChanged
        FillFilters()
    End Sub

    Private Sub cbxPDI_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbxPDI.CheckedChanged
        If cbxPDI.Checked Then
            objSuche.PPhase = "B"
        Else
            objSuche.PPhase = " "c
        End If

        objSuche.GetCars(Session("AppID").ToString, Session.SessionID)

        If (objSuche.ErrorOccured) Then
            lblError.Text = objSuche.Message
        Else
            Session("objSuche") = objSuche
        End If

        ClearFilters()
        FillFilters()

    End Sub

#End Region


#Region "Methods"

    ''' <summary>
    ''' PDI-Liste initial befüllen
    ''' </summary>
    Private Sub Initialload()
        '§§§ JVE 18.07.2006: Hier DropDownlist mit PDIs füllen...

        objSuche = New change_01(m_User, m_App, Session("AppID").ToString, Session.SessionID, "")

        If cbxPDI.Checked Then
            objSuche.PPhase = "B"
        Else
            objSuche.PPhase = " "c
        End If

        objSuche.getCars(Session("AppID").ToString, Session.SessionID)

        If (objSuche.ErrorOccured) Then
            lblError.Text = objSuche.Message
        Else
            Session("objSuche") = objSuche
        End If

    End Sub

    Private Sub FillFilters()

        Dim pdi As String = ddlPDIs.SelectedValue
        Dim herst As String = ddlHersteller.SelectedValue
        Dim model As String = ddlModellID.SelectedValue
        Dim sipp As String = ddlSIPP.SelectedValue
        Dim reifen As String = ddlReifenart.SelectedValue
        Dim farbe As String = ddlFarbe.SelectedValue
        Dim navi As String = ddlNavi.SelectedValue
        Dim ahk As String = ddlAHK.SelectedValue
        Dim auftragvon As String = txtAuftragsnummerVon.Text
        Dim auftragbis As String = txtAuftragsnummerBis.Text
        Dim fahrgestellnummer As String = txtFahrgestellnummer.Text
        Dim gesperrte As Char = " "c

        If rbAktion.SelectedValue = strTaskEntsperren Then gesperrte = "X"c

        objSuche.FillFilterLists(pdi, herst, model, sipp, reifen, farbe, navi, ahk, auftragvon, auftragbis, fahrgestellnummer, gesperrte)

        Session("objSuche") = objSuche

        If String.IsNullOrEmpty(pdi) Then
            ddlPDIs.DataSource = objSuche.lstPDI
            ddlPDIs.DataBind()
        End If

        If String.IsNullOrEmpty(herst) Then
            ddlHersteller.DataSource = objSuche.lstHersteller
            ddlHersteller.DataBind()
        End If

        If String.IsNullOrEmpty(model) Then
            ddlModellID.DataSource = objSuche.lstModellID
            ddlModellID.DataBind()
        End If

        If String.IsNullOrEmpty(sipp) Then
            ddlSIPP.DataSource = objSuche.lstSIPPCode
            ddlSIPP.DataBind()
        End If

        If String.IsNullOrEmpty(reifen) Then
            ddlReifenart.DataSource = objSuche.lstReifenart
            ddlReifenart.DataBind()
        End If

        If String.IsNullOrEmpty(farbe) Then
            ddlFarbe.DataSource = objSuche.lstFarbe
            ddlFarbe.DataBind()
        End If

        If String.IsNullOrEmpty(navi) Then
            ddlNavi.DataSource = objSuche.lstNavi
            ddlNavi.DataBind()
        End If

        If String.IsNullOrEmpty(ahk) Then
            ddlAHK.DataSource = objSuche.lstAhk
            ddlAHK.DataBind()
        End If

        lblAnzahl.Text = objSuche.Result.Rows.Count

        SelectFilterValues()
    End Sub

    Private Sub ClearFilters()
        ddlPDIs.SelectedIndex = -1
        ddlHersteller.SelectedIndex = -1
        ddlModellID.SelectedIndex = -1
        ddlSIPP.SelectedIndex = -1
        ddlReifenart.SelectedIndex = -1
        ddlFarbe.SelectedIndex = -1
        ddlNavi.SelectedIndex = -1
        ddlAHK.SelectedIndex = -1
        txtAuftragsnummerVon.Text = ""
        txtAuftragsnummerBis.Text = ""
        txtFahrgestellnummer.Text = ""
    End Sub
    
    Private Sub SelectFilterValues()
        If ddlPDIs.Items.FindByValue(objSuche.FilterPdi) IsNot Nothing Then ddlPDIs.SelectedValue = objSuche.FilterPdi
        If ddlHersteller.Items.FindByValue(objSuche.FilterHersteller) IsNot Nothing Then ddlHersteller.SelectedValue = objSuche.FilterHersteller
        If ddlModellID.Items.FindByValue(objSuche.FilterModelId) IsNot Nothing Then ddlModellID.SelectedValue = objSuche.FilterModelId
        If ddlSIPP.Items.FindByValue(objSuche.FilterSipp) IsNot Nothing Then ddlSIPP.SelectedValue = objSuche.FilterSipp
        If ddlReifenart.Items.FindByValue(objSuche.FilterReifenart) IsNot Nothing Then ddlReifenart.SelectedValue = objSuche.FilterReifenart
        If ddlFarbe.Items.FindByValue(objSuche.FilterFarbe) IsNot Nothing Then ddlFarbe.SelectedValue = objSuche.FilterFarbe
        If ddlNavi.Items.FindByValue(objSuche.FilterNavi) IsNot Nothing Then ddlNavi.SelectedValue = objSuche.FilterNavi
        If ddlAHK.Items.FindByValue(objSuche.FilterAhk) IsNot Nothing Then ddlAHK.SelectedValue = objSuche.FilterAhk

        If objSuche.FilterSperre = "X"c Then
            rbAktion.SelectedValue = strTaskEntsperren
        End If
    End Sub


#End Region


End Class

' ************************************************
' $History: Change05.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 1.04.10    Time: 14:05
' Updated in $/CKAG/Applications/appec/Forms
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 1.04.10    Time: 13:24
' Updated in $/CKAG/Applications/appec/Forms
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 8.03.10    Time: 9:23
' Updated in $/CKAG/Applications/appec/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 17.06.09   Time: 8:47
' Updated in $/CKAG/Applications/appec/Forms
' Warnungen entfernt!
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:49
' Created in $/CKAG/Applications/appec/Forms
' 
' *****************  Version 11  *****************
' User: Uha          Date: 2.07.07    Time: 12:29
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 10  *****************
' User: Uha          Date: 22.05.07   Time: 13:31
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 9  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' ************************************************
