Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Public Class Report29_23
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
    Private objSuche As Search
    Private objHaendler As FDD_Haendler
    Private objFDDBank As BankBaseCredit
    'Private objFDDBank4 As FDD_Bank_4

    Protected WithEvents ucHeader As Header
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents HyperLink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents Kopfdaten1 As Kopfdaten

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        m_App = New Base.Kernel.Security.App(m_User)

        ucHeader.Visible = False
        Session("SelectedDealer") = Request.QueryString("Kunnr").ToString

        Try
            If (Session("objSuche") Is Nothing) Then
                Response.Redirect("Report29.aspx?AppID=" & Session("AppID").ToString)
            End If

            lblHead.Text = GetTextForLabelHead(Request.UrlReferrer.ToString())

            objSuche = CType(Session("objSuche"), Search)
            If objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, Session("SelectedDealer").ToString) Then
                Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
                Dim strTemp As String = objSuche.NAME
                If objSuche.NAME_2.Length > 0 Then
                    strTemp &= "<br>" & objSuche.NAME_2
                End If
                Kopfdaten1.HaendlerName = strTemp
                Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET
            Else
                lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
            End If

            If (Session("objHaendler") Is Nothing) OrElse (Not IsPostBack) Then
                objHaendler = New FDD_Haendler(Me.Page, m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", "60" & objSuche.REFERENZ, , True)
            Else
                objHaendler = CType(Session("objHaendler"), FDD_Haendler)
            End If

            If objHaendler.Status = 0 Then
                If Not IsPostBack Then
                    Kopfdaten1.Kontingente = objHaendler.Kontingente
                End If
            Else
                lblError.Text = "Fehler bei der Ermittlung der Kontingentdaten.<br>(" & objHaendler.Message & ")"
            End If

            Session("objHaendler") = objHaendler
            objFDDBank = CType(Session("objFDDBank"), BankBaseCredit)

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Function GetTextForLabelHead(ByVal urlReferrer As String) As String
        If urlReferrer.IndexOf("AppFFD/Forms/Report30_01.aspx") >= 0 Then
            Return "Händlerstatus"
        Else
            Return "Überzogene Kontingente (Details)"
        End If
    End Function

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report29_23.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 23.03.10   Time: 16:34
' Updated in $/CKAG/Applications/appffd/Forms
' Rückgängig: Dynproxy-Zugriff
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 16.03.10   Time: 10:16
' Updated in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 11.03.10   Time: 12:36
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 21.05.07   Time: 14:22
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Änderungen im Vergleich zur Startapplikation zum Stand 21.05.2007
' 
' *****************  Version 4  *****************
' User: Uha          Date: 3.05.07    Time: 18:05
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************
