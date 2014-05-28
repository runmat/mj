Imports CKG.Components.ComCommon.Finance

Namespace PageElements
    Public MustInherit Class Suche
        Inherits System.Web.UI.UserControl

        Private objSuche As Search
        Private m_objUser As Base.Kernel.Security.User
        Private objApp As Base.Kernel.Security.App
        Private m_strHeadline As String
        Private AppName As String
        Private m_strRedirectUrl As String
        Private m_blnAllowEmptySearch As Boolean

        Protected WithEvents txtNummer As System.Web.UI.WebControls.TextBox
        Protected WithEvents txtName As System.Web.UI.WebControls.TextBox
        Protected WithEvents txtCity As System.Web.UI.WebControls.TextBox
        Protected WithEvents cmbHaendler As System.Web.UI.WebControls.DropDownList
        Protected WithEvents lblAuswahl As System.Web.UI.WebControls.Label
        Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
        Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
        Protected WithEvents cmdSelect As System.Web.UI.WebControls.LinkButton
        Protected WithEvents txtDatumAb As System.Web.UI.WebControls.TextBox
        Protected WithEvents cmdReset As System.Web.UI.WebControls.LinkButton
        Protected WithEvents trHaendlernummer As System.Web.UI.HtmlControls.HtmlTableRow
        Protected WithEvents trName As System.Web.UI.HtmlControls.HtmlTableRow
        Protected WithEvents trOrt As System.Web.UI.HtmlControls.HtmlTableRow
        Protected WithEvents lblHeadline As System.Web.UI.WebControls.Label
        Protected WithEvents lblTask As System.Web.UI.WebControls.Label
        Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
        Protected WithEvents trHdAuswahl As System.Web.UI.HtmlControls.HtmlTableRow
        Protected WithEvents lbl_HaendlerNummer As System.Web.UI.WebControls.Label
        Protected WithEvents Report As System.Web.UI.HtmlControls.HtmlTableRow

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

        Public Property AllowEmptySearch() As Boolean
            Get
                Return m_blnAllowEmptySearch
            End Get
            Set(ByVal Value As Boolean)
                m_blnAllowEmptySearch = Value
            End Set
        End Property

        Public Property Headline() As String
            Get
                Return m_strHeadline
            End Get
            Set(ByVal Value As String)
                m_strHeadline = Value
                lblHeadline.Text = m_strHeadline
            End Set
        End Property

        Public Property User() As Base.Kernel.Security.User
            Get
                Return m_objUser
            End Get
            Set(ByVal Value As Base.Kernel.Security.User)
                m_objUser = Value
            End Set
        End Property

        Public Property RedirectUrl() As String
            Get
                Return m_strRedirectUrl
            End Get
            Set(ByVal Value As String)
                m_strRedirectUrl = Value
            End Set
        End Property

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Session("ShowLink") = "True"

            AppName = Me.Page.Request.Url.LocalPath
            AppName = Left(AppName, InStrRev(AppName, ".") - 1)
            AppName = Right(AppName, Len(AppName) - InStrRev(AppName, "/"))
            If AppName = "Report12" Then
                Report.Visible = True
            Else
                Report.Visible = False
            End If

            lblTask.Text = "Händlersuche"

            objApp = New Base.Kernel.Security.App(m_objUser)

            If Not IsPostBack Then
                If Not Request.QueryString("Back") = Nothing Then
                    objSuche = CType(Session("objSuche"), Search)
                    Session("objSuche") = objSuche
                Else
                    objSuche = New Search(objApp, m_objUser, Session.SessionID.ToString, Session("AppID").ToString)
                    Session("objSuche") = objSuche

                End If

            Else
                objSuche = CType(Session("objSuche"), Search)
                Session("objSuche") = objSuche
            End If

            If Not IsPostBack Then
                If Not Request.QueryString("Back") = Nothing Then
                    objSuche = CType(Session("objSuche"), Search)
                Else
                    objSuche = New Search(objApp, m_objUser, Session.SessionID.ToString, Session("AppID").ToString)
                    Session("objSuche") = objSuche
                End If
                DoSubmit1()
            Else
                objSuche = CType(Session("objSuche"), Search)
                Session("objSuche") = objSuche
            End If

            If Not m_objUser.Reference.Length = 0 Then
                objSuche.HaendlerReferenzNummer = m_objUser.Reference
                Session("ShowLink") = "False"
                DoSubmit1()
            End If

            If Not m_objUser.Reference.Length = 0 And Not m_objUser.Organization.AllOrganizations Then
                objSuche.HaendlerReferenzNummer = m_objUser.Reference
                Session("ShowLink") = "False"
                DoSubmit1()
            End If
        End Sub

        Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
            DoSubmit1()
        End Sub

        Private Sub cmdSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelect.Click
            DoSubmit2()
        End Sub

        Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
            If cmdSearch.Text = "&#149;&nbsp;Neue Suche" Then
                DoSubmit2()
            Else
                DoSubmit1()
            End If
        End Sub

        Private Sub DoSubmit1()

            If m_objUser.Reference.Length = 0 Then

                objSuche.HaendlerReferenzNummer = txtNummer.Text
                objSuche.HaendlerName = txtName.Text
                objSuche.HaendlerOrt = txtCity.Text
            Else
                objSuche.HaendlerReferenzNummer = m_objUser.Reference
            End If

           
            'JJ 2008.01.30 schnelles Bugfix für AKF-Schulung
            '-----------------------------------------------------------------
            'dadurch das dass bapi ohne user-referenz angabe wert ausspuckt, ist die logik hier hinfällig! 
            'cmdSelect.Visible = False
            cmdSelect.Visible = True
            '-----------------------------------------------------------------


            lblMessage.Text = ""
            lblMessage.CssClass = ""
            cmdSearch.Text = "  Suchen  "

            Dim tmpIntValue As Int32 = objSuche.LeseHaendlerSAP(Session("AppID").ToString, Session.SessionID.ToString)

            If tmpIntValue < 0 Then
                lblMessage.CssClass = "TextError"
                lblMessage.Text = "Fehler: " & objSuche.ErrorMessage
            ElseIf tmpIntValue = 0 Then
                cmdReset.Visible = False
                lblMessage.Text = "Ihre Suche hat keine Treffer ergeben."
            Else
                If Not IsNothing(objSuche.Haendler) Then objSuche.Haendler.Sort = "NAME"
                cmbHaendler.DataSource = objSuche.Haendler
                cmbHaendler.DataTextField = "DISPLAY"
                cmbHaendler.DataValueField = "REFERENZ"
                cmbHaendler.DataBind()
                cmbHaendler.SelectedIndex = 0

                If tmpIntValue > 1 Then
                    If m_blnAllowEmptySearch And (objSuche.HaendlerReferenzNummer.Length + objSuche.HaendlerName.Length + objSuche.HaendlerOrt.Length = 0) Then
                        Session("SelectedDealer") = ""
                        Try
                            Response.Redirect(m_strRedirectUrl)
                        Catch
                        End Try

                    Else
                        cmdSelect.Visible = True

                        lblMessage.Text = "Ihre Suche ergab mehrere Treffer.<br>Bitte wählen Sie aus."
                        lblMessage.Font.Bold = True

                        cmdSearch.Visible = False
                        cmdReset.Visible = False
                    End If
                Else
                    DoSubmit2()
                End If
            End If

            Session("objSuche") = objSuche

        End Sub
       
        Private Sub DoSubmit2()
            If Not cmbHaendler.SelectedItem Is Nothing Then
                If cmbHaendler.SelectedItem.Value = "000000" Then
                    lblMessage.Text = "Bitte wählen Sie einen Händler aus."
                    lblMessage.Font.Bold = True
                Else
                    Session("SelectedDealer") = cmbHaendler.SelectedItem.Value
                    Session("objSuche") = objSuche
                    If CInt(m_objUser.Applications.Select("AppID=" & Session("AppID").ToString)(0)("AuthorizationLevel")) > 0 Then
                        Dim intAuthorizationID As Int32
                        Dim strInitiator As String = ""
                        objApp.CheckForPendingAuthorization(CInt(Session("AppID")), m_objUser.Organization.OrganizationId, Session("SelectedDealer").ToString, "", m_objUser.IsTestUser, strInitiator, intAuthorizationID)
                        If intAuthorizationID > -1 Then
                            Session("Authorization") = "True"
                            Session("AuthorizationID") = intAuthorizationID
                        End If
                    End If
                    Try
                        Response.Redirect(m_strRedirectUrl)
                    Catch
                    End Try
                End If
            End If
        End Sub

        Public Function IsDateWithoutPoint(ByRef strInput As String) As Boolean
            Dim strTemp As String
            If Not Len(strInput) = 8 Then
                Return False
            End If
            strTemp = Left(strInput, 2) & "." & Mid(strInput, 3, 2) & "." & Right(strInput, 4)
            If IsDate(strTemp) Then
                strInput = strTemp
                Return True
            Else
                Return False
            End If
        End Function

        Public Function IsStandardDate(ByRef strInput As String) As Boolean
            Dim strTemp As String
            If Not Len(strInput) = 8 Then
                Return False
            End If
            strTemp = Left(strInput, 2) & "." & Mid(strInput, 3, 2) & "." & Right(strInput, 4)
            If IsDate(strTemp) Then
                strInput = strTemp
                Return True
            Else
                Return False
            End If
        End Function

        Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
            Session("SelectedDealer") = Nothing
            objSuche = New Search(objApp, m_objUser, Session.SessionID.ToString, Session("AppID").ToString)

            lblMessage.Text = ""

            cmbHaendler.Items.Clear()
            cmdReset.Visible = False
            cmdSearch.Visible = True

            cmdSearch.Text = "  Suchen  "

            Session("objSuche") = objSuche
        End Sub

        'Private Function IsSpecialRedirectURL(ByVal aUrl As String) As Boolean
        '    If (aUrl.IndexOf("Report30_01.aspx") >= 0 OrElse aUrl.IndexOf("Report29_22.aspx") >= 0) Then
        '        Return True
        '    End If
        '    Return False
        'End Function
    End Class
End Namespace

' ************************************************
' $History: Suche.ascx.vb $
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 19.06.09   Time: 10:10
' Updated in $/CKAG/Components/ComCommon/Finance/PageElements
' ITa 2918 testfertig
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:50
' Updated in $/CKAG/Components/ComCommon/Finance/PageElements
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 24.04.08   Time: 8:39
' Updated in $/CKAG/Components/ComCommon/Finance/PageElements
' Migration AKF-Entwicklungen
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 23.04.08   Time: 9:44
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/PageElements
' ITA 1850
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 30.01.08   Time: 11:55
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/PageElements
' BUGFIX für AKF-Schulung s
' 
' *****************  Version 5  *****************
' User: Uha          Date: 7.01.08    Time: 18:47
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/PageElements
' 
' *****************  Version 4  *****************
' User: Uha          Date: 17.12.07   Time: 17:58
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/PageElements
' Versand ZB II / Briefe - komplierfähige Zwischenversion
' 
' *****************  Version 3  *****************
' User: Uha          Date: 13.12.07   Time: 13:58
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/PageElements
' ITA 1481/1509 (Änderung / Sperrung Händlerkontingent) Testversion
' 
' *****************  Version 2  *****************
' User: Uha          Date: 12.12.07   Time: 15:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/PageElements
' ITA 1473/1497 (Mahnstufe 3) als Testversion; ITA 1481/1509
' (Änderung/Sperrung Händlerkontingent) komplierfähig
' 
' *****************  Version 1  *****************
' User: Uha          Date: 12.12.07   Time: 13:23
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance/PageElements
' Übernahme der Suchfunktion aus FFD (ohne Anpassung)
' 
' ************************************************
