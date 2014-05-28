Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements


Public Class Report30_3
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

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lnkFahrzeugsuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Protected WithEvents Label16 As System.Web.UI.WebControls.Label
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents Label17 As System.Web.UI.WebControls.Label
    Protected WithEvents lnkPrint As System.Web.UI.WebControls.HyperLink
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        'GetAppIDFromQueryString(Me)

        Dim tblResultTableRaw As New DataTable()

        Try
            m_App = New Base.Kernel.Security.App(m_User)

            If (Session("ResultTableRaw") Is Nothing) Then
                lblError.Text = "Fehler: Die Seite wurde ohne Kontext aufgerufen."
            Else
                tblResultTableRaw = CType(Session("ResultTableRaw"), DataTable)
            End If

            If Not tblResultTableRaw Is Nothing Then
                lblHead.Text = "Detaildaten für Zulassungsdienst"
                ucStyles.TitleText = lblHead.Text

                If (Request.QueryString("ID") Is Nothing) OrElse (Request.QueryString("ID").ToString.Length = 0) Then
                    lblError.Text = "Feher: Die Seite wurde ohne Angaben zum Zulassungsdienst aufgerufen."
                Else
                    Dim rows() As DataRow = tblResultTableRaw.Select("ID = " & Request.QueryString("ID").ToString)
                    Label1.Text = rows(0)("NAME1").ToString
                    Label2.Text = rows(0)("ANSPRECHPARTNER").ToString
                    Label3.Text = rows(0)("NAME1").ToString
                    Label4.Text = rows(0)("NAME2").ToString
                    Label5.Text = rows(0)("STREET").ToString
                    Label6.Text = rows(0)("HOUSE_NUM1").ToString
                    Label7.Text = rows(0)("POST_CODE1").ToString
                    Label8.Text = rows(0)("CITY1").ToString
                    Label9.Text = rows(0)("TELE1").ToString
                    Label10.Text = rows(0)("TELE2").ToString
                    Label11.Text = rows(0)("TELE3").ToString
                    Label12.Text = rows(0)("FAX_NUMBER").ToString
                    Label13.Text = rows(0)("SMTP_ADDR").ToString
                    Label14.Text = rows(0)("ZTXT1").ToString
                    Label15.Text = rows(0)("ZTXT2").ToString
                    Label16.Text = rows(0)("ZTXT3").ToString
                    Label17.Text = rows(0)("BEMERKUNG").ToString
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
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
' $History: Report30_3.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 11:59
' Created in $/CKAG/Applications/apprdz/Forms
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 13:32
' Updated in $/CKG/Applications/AppRDZ/AppRDZWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 14.03.07   Time: 17:00
' Updated in $/CKG/Applications/AppRDZ/AppRDZWeb/Forms
' GetAppIDFromQueryString entfernt, da die AppID nicht übergeben und
' nicht benötigt wird
' 
' *****************  Version 4  *****************
' User: Uha          Date: 8.03.07    Time: 11:14
' Updated in $/CKG/Applications/AppRDZ/AppRDZWeb/Forms
' 
' ************************************************
