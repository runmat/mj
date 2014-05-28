Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change09_3
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
    'Private objSuche As Search
    'Private objAddressList As Search
    Private objHaendler As Straub_09c

    Protected WithEvents lnkFahrzeugsuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkFahrzeugAuswahl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ZeigeZULST As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmbZuslassungstellen As System.Web.UI.WebControls.DropDownList

    Protected WithEvents txtKennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkZulassungsstellen As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chkVersandStandard As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chk0900 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chk1000 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chk1200 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents idPreis1 As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents idPreis2 As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents txtName1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtName2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtStrasse As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPLZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOrt As System.Web.UI.WebControls.TextBox
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        lnkFahrzeugAuswahl.NavigateUrl = "Change09_2.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugsuche.NavigateUrl = "Change09.aspx?AppID=" & Session("AppID").ToString
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change09.aspx?AppID=" & Session("AppID").ToString)
            End If

            objHaendler = CType(Session("objHaendler"), Straub_09c)
            cmdSearch.Visible = False

            If Not IsPostBack Then
                txtName1.Text = objHaendler.PName
                txtName2.Text = objHaendler.PName1
                txtStrasse.Text = objHaendler.PStrasse
                txtNummer.Text = objHaendler.PHausnummer
                txtPLZ.Text = objHaendler.PPlz
                txtOrt.Text = objHaendler.POrt
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        DoSubmit()
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        DoSubmit()
    End Sub

    Private Function checkInput(ByRef strStatus As String) As Boolean
        Dim blnInput As Boolean

        strStatus = String.Empty
        blnInput = True

        If txtName1.Text.Trim = String.Empty Then
            strStatus = "Bitte einen Namen eingeben."
            blnInput = False
        End If

        If blnInput Then
            If txtStrasse.Text.Trim = String.Empty Then
                strStatus = "Bitte einen Straße eingeben."
                blnInput = False
            End If
        End If

        If blnInput Then
            If txtNummer.Text.Trim = String.Empty Then
                strStatus = "Bitte einen Hausnummer eingeben."
                blnInput = False
            End If
        End If

        If blnInput Then
            If txtPLZ.Text.Trim = String.Empty Then
                strStatus = "Bitte eine Postleitzahl eingeben."
                blnInput = False
            End If
            If blnInput Then
                If txtPLZ.Text.Trim.Length < 5 Then
                    strStatus = "Postleitzahl muß 5-Stellig sein."
                    blnInput = False
                End If
            End If
            If blnInput Then
                Try
                    If CInt(txtPLZ.Text) Then
                        blnInput = True
                    End If
                Catch ex As Exception
                    strStatus = "Postleitzahl darf nur Ziffern enthalten"
                    blnInput = False
                End Try
            End If
        End If

        If blnInput Then
            If txtOrt.Text.Trim = String.Empty Then
                strStatus = "Bitte einen Ort eingeben."
                blnInput = False
            End If
        End If
        Return blnInput
    End Function

    Private Sub DoSubmit()
        Dim strStatus As String

        objHaendler = CType(Session("objHaendler"), Straub_09c)

        If Not checkInput(strStatus) Then
            lblError.Text = strStatus
        Else
            If chkVersandStandard.Checked Then
                objHaendler.MaterialNummer = "1391"
            ElseIf chk0900.Checked Then
                objHaendler.MaterialNummer = "1385"
            ElseIf chk1000.Checked Then
                objHaendler.MaterialNummer = "1389"
            ElseIf chk1200.Checked Then
                objHaendler.MaterialNummer = "1390"
            End If

            With objHaendler
                .PName = txtName1.Text
                .PName1 = txtName2.Text
                .PStrasse = txtStrasse.Text
                .PHausnummer = txtNummer.Text
                .PPlz = txtPLZ.Text
                .POrt = txtOrt.Text
            End With

            Session("ObjHaendler") = objHaendler
            Response.Redirect("Change09_4.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change09_3.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:38
' Created in $/CKAG/Applications/AppSTRAUB/Forms
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 14:28
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 8.03.07    Time: 14:18
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Forms
' 
' ************************************************
