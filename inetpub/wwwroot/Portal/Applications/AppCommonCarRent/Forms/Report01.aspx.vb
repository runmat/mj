Imports CKG
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Partial Public Class Report01
    Inherits System.Web.UI.Page
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private m_objTable As DataTable
    Private m_objExcel As DataTable
    Private mObjZulassungsFaehigeFahrzeuge As ZulassungsFaehigeFahrzeuge

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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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

            mObjZulassungsFaehigeFahrzeuge = New ZulassungsFaehigeFahrzeuge(m_User, m_App, "")
            mObjZulassungsFaehigeFahrzeuge.Fill(Session("AppID").ToString, Me.Session.SessionID, Me)
            If Not mObjZulassungsFaehigeFahrzeuge.Status = 0 Then
                lblError.Text = mObjZulassungsFaehigeFahrzeuge.Message
                Exit Sub
            End If
            Session("mObjZulassungsFaehigeFahrzeugeSession") = mObjZulassungsFaehigeFahrzeuge
            FillDDls()
            lblMessage.Text = "Anzahl gefundener Fahrzeuge: " & mObjZulassungsFaehigeFahrzeuge.Fahrzeuge.Rows.Count
        Else

            If mObjZulassungsFaehigeFahrzeuge Is Nothing Then
                If Not Session("mObjZulassungsFaehigeFahrzeugeSession") Is Nothing Then
                    mObjZulassungsFaehigeFahrzeuge = CType(Session("mObjZulassungsFaehigeFahrzeugeSession"), ZulassungsFaehigeFahrzeuge)
                End If
            End If

        End If

    End Sub


    Private Sub responseBack()
        If Refferer = "" Then
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            Response.Redirect(Refferer)
        End If
    End Sub

    Private Sub SetFilter()

        '----------------------------------------------------------------------
        'Methode:       SetFilter
        'Autor:         Julian Jung
        'Beschreibung:  Ruft pro ddl die Filtermethode auf und übergibt den selektierten wert
        'Erstellt am:   26.01.2009
        '----------------------------------------------------------------------

        mObjZulassungsFaehigeFahrzeuge.killFilter()

        If Not ddlStandort.SelectedIndex = 0 AndAlso Not ddlStandort.SelectedIndex = -1 Then
            If mObjZulassungsFaehigeFahrzeuge.wasUserSelected(ddlStandort.ID) Then
                mObjZulassungsFaehigeFahrzeuge.GenerateFilter("Standort", ddlStandort.SelectedValue)
            Else
                If ddlStandort.Enabled Then
                    'programatisch auf disabled gesetze ddls dürfen für den filter nicht verwendet werden!
                    mObjZulassungsFaehigeFahrzeuge.GenerateFilter("Standort", ddlStandort.SelectedValue)
                End If
            End If
        End If

        If Not ddlAbsender.SelectedIndex = 0 AndAlso Not ddlAbsender.SelectedIndex = -1 Then
            If mObjZulassungsFaehigeFahrzeuge.wasUserSelected(ddlAbsender.ID) Then
                mObjZulassungsFaehigeFahrzeuge.GenerateFilter("Absender", ddlAbsender.SelectedValue)
            Else
                If ddlAbsender.Enabled Then
                    'programatisch auf disabled gesetze ddls dürfen für den filter nicht verwendet werden!
                    mObjZulassungsFaehigeFahrzeuge.GenerateFilter("Absender", ddlAbsender.SelectedValue)
                End If
            End If
        End If



        If Not ddlLiefermonat.SelectedIndex = 0 AndAlso Not ddlLiefermonat.SelectedIndex = -1 Then
            If mObjZulassungsFaehigeFahrzeuge.wasUserSelected(ddlLiefermonat.ID) Then
                mObjZulassungsFaehigeFahrzeuge.GenerateFilter("Liefermonat", ddlLiefermonat.SelectedValue)
            Else
                If ddlLiefermonat.Enabled Then
                    'programatisch auf disabled gesetze ddls dürfen für den filter nicht verwendet werden!
                    mObjZulassungsFaehigeFahrzeuge.GenerateFilter("Liefermonat", ddlLiefermonat.SelectedValue)
                End If
            End If
        End If


        If Not ddlHersteller.SelectedIndex = 0 AndAlso Not ddlHersteller.SelectedIndex = -1 Then
            If mObjZulassungsFaehigeFahrzeuge.wasUserSelected(ddlHersteller.ID) Then
                mObjZulassungsFaehigeFahrzeuge.GenerateFilter("Hersteller", ddlHersteller.SelectedValue)
            Else
                If ddlHersteller.Enabled Then
                    'programatisch auf disabled gesetze ddls dürfen für den filter nicht verwendet werden!
                    mObjZulassungsFaehigeFahrzeuge.GenerateFilter("Hersteller", ddlHersteller.SelectedValue)
                End If
            End If
        End If


        If Not ddlTyp.SelectedIndex = 0 AndAlso Not ddlTyp.SelectedIndex = -1 Then
            If mObjZulassungsFaehigeFahrzeuge.wasUserSelected(ddlTyp.ID) Then
                mObjZulassungsFaehigeFahrzeuge.GenerateFilter("Typ", ddlTyp.SelectedValue)
            Else
                If ddlTyp.Enabled Then
                    'programatisch auf disabled gesetze ddls dürfen für den filter nicht verwendet werden!
                    mObjZulassungsFaehigeFahrzeuge.GenerateFilter("Typ", ddlTyp.SelectedValue)
                End If
            End If
        End If

        If Not ddlFarbe.SelectedIndex = 0 AndAlso Not ddlFarbe.SelectedIndex = -1 Then
            If mObjZulassungsFaehigeFahrzeuge.wasUserSelected(ddlFarbe.ID) Then
                mObjZulassungsFaehigeFahrzeuge.GenerateFilter("Farbe", ddlFarbe.SelectedValue)
            Else
                If ddlFarbe.Enabled Then
                    'programatisch auf disabled gesetze ddls dürfen für den filter nicht verwendet werden!
                    mObjZulassungsFaehigeFahrzeuge.GenerateFilter("Farbe", ddlFarbe.SelectedValue)
                End If
            End If


        End If

        If Not ddlBereifung.SelectedIndex = 0 AndAlso Not ddlBereifung.SelectedIndex = -1 Then
            If mObjZulassungsFaehigeFahrzeuge.wasUserSelected(ddlBereifung.ID) Then
                mObjZulassungsFaehigeFahrzeuge.GenerateFilter("Bereifung", ddlBereifung.SelectedValue)
            Else
                If ddlBereifung.Enabled Then
                    'programatisch auf disabled gesetze ddls dürfen für den filter nicht verwendet werden!
                    mObjZulassungsFaehigeFahrzeuge.GenerateFilter("Bereifung", ddlBereifung.SelectedValue)
                End If
            End If

        End If

        If Not ddlGetriebe.SelectedIndex = 0 AndAlso Not ddlGetriebe.SelectedIndex = -1 Then
            If mObjZulassungsFaehigeFahrzeuge.wasUserSelected(ddlGetriebe.ID) Then
                mObjZulassungsFaehigeFahrzeuge.GenerateFilter("Getriebe", ddlGetriebe.SelectedValue)
            Else
                If ddlGetriebe.Enabled Then
                    'programatisch auf disabled gesetze ddls dürfen für den filter nicht verwendet werden!
                    mObjZulassungsFaehigeFahrzeuge.GenerateFilter("Getriebe", ddlGetriebe.SelectedValue)
                End If
            End If
        End If

        If Not ddlKraftstoffart.SelectedIndex = 0 AndAlso Not ddlKraftstoffart.SelectedIndex = -1 Then
            If mObjZulassungsFaehigeFahrzeuge.wasUserSelected(ddlKraftstoffart.ID) Then
                mObjZulassungsFaehigeFahrzeuge.GenerateFilter("Kraftstoffart", ddlKraftstoffart.SelectedValue)
            Else
                If ddlKraftstoffart.Enabled Then
                    'programatisch auf disabled gesetze ddls dürfen für den filter nicht verwendet werden!
                    mObjZulassungsFaehigeFahrzeuge.GenerateFilter("Kraftstoffart", ddlKraftstoffart.SelectedValue)
                End If
            End If


        End If

        If Not ddlNavi.SelectedIndex = 0 AndAlso Not ddlNavi.SelectedIndex = -1 Then
            If mObjZulassungsFaehigeFahrzeuge.wasUserSelected(ddlNavi.ID) Then
                mObjZulassungsFaehigeFahrzeuge.GenerateFilter("Navi", ddlNavi.SelectedValue)
            Else
                If ddlNavi.Enabled Then
                    'programatisch auf disabled gesetze ddls dürfen für den filter nicht verwendet werden!
                    mObjZulassungsFaehigeFahrzeuge.GenerateFilter("Navi", ddlNavi.SelectedValue)
                End If
            End If

        End If


    End Sub


    

    Private Sub FillDDls()

        '----------------------------------------------------------------------
        'Methode:       SetFilter
        'Autor:         Julian Jung
        'Beschreibung:  ruft die fill methode der ddls auf und übergibt sich selbst+den spaltenname aus der sie gefüllt werden soll
        'Erstellt am:   26.01.2009
        '----------------------------------------------------------------------


        mObjZulassungsFaehigeFahrzeuge.FillDDls(ddlStandort, "Standort")
        mObjZulassungsFaehigeFahrzeuge.FillDDls(ddlHersteller, "Hersteller")
        mObjZulassungsFaehigeFahrzeuge.FillDDls(ddlAbsender, "Absender")
        mObjZulassungsFaehigeFahrzeuge.FillDDls(ddlLiefermonat, "Liefermonat")
        mObjZulassungsFaehigeFahrzeuge.FillDDls(ddlTyp, "Typ")
        mObjZulassungsFaehigeFahrzeuge.FillDDls(ddlFarbe, "Farbe")
        mObjZulassungsFaehigeFahrzeuge.FillDDls(ddlBereifung, "Bereifung")
        mObjZulassungsFaehigeFahrzeuge.FillDDls(ddlGetriebe, "Getriebe")
        mObjZulassungsFaehigeFahrzeuge.FillDDls(ddlKraftstoffart, "Kraftstoffart")
        mObjZulassungsFaehigeFahrzeuge.FillDDls(ddlNavi, "Navi")



    End Sub


    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

    Protected Sub cmdCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Create.Click
        Response.Redirect("Report01_2.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub ddlStandort_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlStandort.SelectedIndexChanged, _
                                                                                                              ddlAbsender.SelectedIndexChanged, _
                                                                                                              ddlBereifung.SelectedIndexChanged, _
                                                                                                              ddlFarbe.SelectedIndexChanged, _
                                                                                                              ddlGetriebe.SelectedIndexChanged, _
                                                                                                              ddlHersteller.SelectedIndexChanged, _
                                                                                                              ddlKraftstoffart.SelectedIndexChanged, _
                                                                                                              ddlLiefermonat.SelectedIndexChanged, _
                                                                                                              ddlNavi.SelectedIndexChanged, _
                                                                                                              ddlTyp.SelectedIndexChanged



        If CType(sender, DropDownList).SelectedIndex = 0 Then
            mObjZulassungsFaehigeFahrzeuge.UserHistorie.Remove(CType(sender, DropDownList).ClientID)
        Else
            mObjZulassungsFaehigeFahrzeuge.UserHistorie.Add(CType(sender, DropDownList).ClientID)

        End If



        SetFilter()
        FillDDls()

        Dim tmpDV As New DataView(mObjZulassungsFaehigeFahrzeuge.Fahrzeuge)
        tmpDV.RowFilter = mObjZulassungsFaehigeFahrzeuge.Filter

        lblMessage.Text = "Anzahl gefundener Fahrzeuge: " & tmpDV.Count
    End Sub
End Class
' ************************************************
' $History: Report01.aspx.vb $
' 
' *****************  Version 7  *****************
' User: Fassbenders  Date: 20.08.09   Time: 14:23
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA: 3078
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 26.01.09   Time: 12:48
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ita 2535 nachbesserungen
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 26.01.09   Time: 12:40
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ita 2535 nachbesserung
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 26.01.09   Time: 11:27
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ita 2535 fertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 26.01.09   Time: 11:21
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2535
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 23.01.09   Time: 17:12
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2535
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 22.01.09   Time: 17:11
' Created in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2535
' 
' ************************************************
