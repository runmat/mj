Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Imports System
Imports System.IO
Imports System.Collections
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization


Public Class Change40Edit
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
    Private objSuche As CKG.Components.ComCommon.Finance.Search
    Private objChange40 As BankBaseCredit
    Private m_strInitiator As String

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblHaendlerName As System.Web.UI.WebControls.Label
    Protected WithEvents lblHaendlerNummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblAdresse As System.Web.UI.WebControls.Label
    Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblInformation As System.Web.UI.WebControls.Label
    Protected WithEvents ConfirmMessage As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblScript As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdConfirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdReset As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdAuthorisize As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdAuthorize As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdBack2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_HaendlerNummer As System.Web.UI.WebControls.Label
    Protected WithEvents chkInsolvent As System.Web.UI.WebControls.CheckBox

    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSave.Enabled = False
        cmdConfirm.Enabled = False
        cmdReset.Enabled = False
        lblPageTitle.Text = "Werte ändern"

        lblInformation.Text = ""
        m_strInitiator = ""

        Try
            m_User = GetUser(Me)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)

            m_App = New Base.Kernel.Security.App(m_User)

            If Session("SelectedDealer").ToString.Length = 0 OrElse (Session("objSuche") Is Nothing) Then
                If (Not Session("Authorization") Is Nothing) AndAlso CBool(Session("Authorization")) AndAlso _
                    (Not Session("AuthorizationID") Is Nothing) AndAlso IsNumeric(Session("AuthorizationID")) Then
                    Dim OutPutStream As System.IO.MemoryStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(Session("AuthorizationID")), "objSuche")
                    If OutPutStream Is Nothing Then
                        lblError.Text = "Keine Daten für den Vorgang vorhanden."
                    Else
                        Dim formatter As New BinaryFormatter()
                        objSuche = New Finance.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                        objSuche = DirectCast(formatter.Deserialize(OutPutStream), Finance.Search)
                        objSuche.ReNewSAPDestination(Session.SessionID.ToString, Session("AppID").ToString)
                    End If
                Else
                    Try
                    Catch
                        Response.Redirect("Change40.aspx?AppID=" & Session("AppID").ToString)
                    End Try
                End If
            Else
                objSuche = CType(Session("objSuche"), Finance.Search)
            End If

            If objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, Session("SelectedDealer").ToString) Then
                lblHaendlerNummer.Text = objSuche.REFERENZ
                lblHaendlerName.Text = objSuche.NAME
                If objSuche.NAME_2.Length > 0 Then
                    lblHaendlerName.Text &= "<br>" & objSuche.NAME_2
                End If
                lblAdresse.Text = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET

            Else
                lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
            End If
            lnkKreditlimit.NavigateUrl = "Change40.aspx?AppID=" & Session("AppID").ToString & "&Back=1"

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            If Not IsPostBack Then
                ConfirmMessage.Visible = False

                '#################### Debug only ####################
                'Session("Authorization") = True
                'Session("AuthorizationID") = 6
                '#################### Debug only ####################
            End If

            If (Not Session("Authorization") Is Nothing) AndAlso (CBool(Session("Authorization"))) Then
                'Seite wurde mit dem Merkmal "Autorisieren" aufgerufen

                If (Session("AuthorizationID") Is Nothing) OrElse Session("AuthorizationID").ToString.Length = 0 Then
                    'AuthorizationID leer
                    lblError.Text = "Kein Vorgang zum Autorisieren übergeben."
                    DataGrid1.Visible = False
                Else
                    'AuthorizationID gefüllt -> Vorgang wird aus DB geladen
                    Dim intAuthorizationID As Int32
                    m_App.CheckForPendingAuthorization(CInt(Session("AppID")), m_User.Organization.OrganizationId, Session("SelectedDealer").ToString, "", m_User.IsTestUser, m_strInitiator, intAuthorizationID)
                    If m_strInitiator = m_User.UserName Then
                        'Seite gesperrt aufgerufen, da Händlerdaten in Autorisierung und der Benutzer der gleiche ist

                        LoadAuthorizatioData(intAuthorizationID)
                        lblError.Text = "Die Angaben zum Händler " & Session("SelectedDealer").ToString & " wurden vom Benutzer """ & m_strInitiator & """ geändert.<br>&nbsp;&nbsp;Die Autorisierung steht noch aus!"
                    Else
                        Dim OutPutStream As System.IO.MemoryStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(Session("AuthorizationID")), "objChange40")
                        If OutPutStream Is Nothing Then
                            lblError.Text = "Keine Daten für den Vorgang vorhanden."
                        Else
                            Dim formatter As New BinaryFormatter()
                            objChange40 = New BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                            objChange40 = DirectCast(formatter.Deserialize(OutPutStream), BankBaseCredit)
                            FillGrid()
                            DoSubmit1()
                         
                            cmdSave.Visible = False
                            cmdConfirm.Visible = False
                            cmdReset.Visible = False
                            cmdBack.Visible = True
                            cmdAuthorize.Visible = True
                            cmdDelete.Visible = True
                        End If
                    End If
                End If
            Else
                Dim intAuthorizationID As Int32
                m_App.CheckForPendingAuthorization(CInt(Session("AppID")), m_User.Organization.OrganizationId, Session("SelectedDealer").ToString, "", m_User.IsTestUser, m_strInitiator, intAuthorizationID)
                If Not m_strInitiator.Length = 0 Then
                    'Seite gesperrt aufgerufen, da Händlerdaten in Autorisierung

                    LoadAuthorizatioData(intAuthorizationID)
                    lblError.Text = "Die Angaben zum Händler " & Session("SelectedDealer").ToString & " wurden vom Benutzer """ & m_strInitiator & """ geändert.<br>&nbsp;&nbsp;Die Autorisierung steht noch aus!"
                Else
                    'Seite im normalen Änderungsmodus aufgerufen

                    If Not IsPostBack Then
                        objChange40 = New BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                        objChange40.Customer = Session("SelectedDealer").ToString
                        objChange40.KUNNR = m_User.KUNNR
                        objChange40.CreditControlArea = "ZDAD"
                        'If (Not Request.QueryString("ID") Is Nothing) AndAlso (Request.QueryString("ID").Length > 0) Then
                        objChange40.ZeigeGesperrt = True
                        'Else
                        '    objChange40.ZeigeGesperrt = False
                        'End If
                    Else
                        objChange40 = CType(m_context.Cache("objChange40"), BankBaseCredit)
                    End If
                    objChange40.Show(Session("AppID").ToString, Session.SessionID)
                    If objChange40.Status = 0 Then
                        If Not IsPostBack Then
                            StartLoadData()
                        End If

                        cmdSave.Enabled = True
                        cmdConfirm.Enabled = True
                        cmdReset.Enabled = True
                        cmdAuthorize.Enabled = False
                        cmdBack.Enabled = False
                        cmdDelete.Enabled = False
                    Else
                        lblError.Text = objChange40.Message

                    End If
                End If
            End If

            If Not IsPostBack Then
                m_context.Cache.Insert("objChange40", objChange40, New System.Web.Caching.CacheDependency(Server.MapPath("Change40Edit.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Change40Edit", "Page_Load", ex.ToString)

            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            Throw ex
        End Try
    End Sub

    Private Sub FillGrid()
        DataGrid1.DataSource = objChange40.Kontingente
        DataGrid1.DataBind()

        Dim intKreditlimit As Int32
        Dim intAusschoepfung As Int32
        Dim blnGesperrt As Boolean

        Dim item As DataGridItem
        Dim cell As TableCell
        Dim chkBox As CheckBox
        Dim label As Label
        Dim textbox As TextBox
        Dim control As Control


        If objChange40.Kontingente.Rows.Count > 0 Then
            If CType(objChange40.Kontingente.Rows(0)("Insolvent_Alt"), Boolean) = True Then
                chkInsolvent.Checked = True
            End If
        End If

        For Each item In DataGrid1.Items
            Dim blnZeigeKontingentart As Boolean
            cell = item.Cells(item.Cells.Count - 1)
            For Each control In cell.Controls
                If TypeOf control Is CheckBox Then
                    chkBox = CType(control, CheckBox)
                    blnZeigeKontingentart = chkBox.Checked
                End If
            Next
            cell = item.Cells(2)
            For Each control In cell.Controls
                If TypeOf control Is Label Then
                    label = CType(control, Label)
                    If label.ID = "lblKontingent_Alt" And blnZeigeKontingentart Then
                        label.Visible = True
                        intKreditlimit = CInt(label.Text)
                    Else
                        label.Visible = False
                        If label.ID = "lblRichtwert_Alt" And (Not blnZeigeKontingentart) Then
                            label.Visible = True
                            intKreditlimit = CInt(label.Text)
                        Else
                            label.Visible = False
                        End If
                    End If
                End If
            Next

            intAusschoepfung = CInt(item.Cells(3).Text)

            cell = item.Cells(4)
            For Each control In cell.Controls
                If TypeOf control Is Label Then
                    label = CType(control, Label)
                    If Not blnZeigeKontingentart Then
                        label.Visible = False
                    End If
                End If
            Next

            cell = item.Cells(5)
            For Each control In cell.Controls
                If TypeOf control Is CheckBox Then
                    chkBox = CType(control, CheckBox)
                    blnGesperrt = chkBox.Checked
                    If Not blnZeigeKontingentart Then
                        chkBox.Visible = False
                    End If
                End If
            Next

            cell = item.Cells(6)
            For Each control In cell.Controls
                If TypeOf control Is TextBox Then
                    textbox = CType(control, TextBox)
                    If textbox.ID = "txtKontingent_Neu" And blnZeigeKontingentart Then
                        textbox.Visible = True
                        If chkInsolvent.Checked = True Then
                            textbox.Enabled = False
                        End If
                    Else
                        textbox.Visible = False
                        If textbox.ID = "txtRichtwert_Neu" And (Not blnZeigeKontingentart) Then
                            textbox.Visible = True
                            If chkInsolvent.Checked = True Then
                                textbox.Enabled = False
                            End If
                        Else
                            textbox.Visible = False
                        End If
                    End If
                End If
            Next

            cell = item.Cells(7)
            For Each control In cell.Controls
                If TypeOf control Is CheckBox Then
                    chkBox = CType(control, CheckBox)
                    If chkInsolvent.Checked = True Then
                        chkBox.Enabled = False
                    End If
                    If Not blnZeigeKontingentart Then
                        chkBox.Visible = False
                    End If
                End If
            Next

            If blnZeigeKontingentart Then
                If blnGesperrt Then
                    For Each cell In item.Cells
                    Next
                Else
                    'If Not objChange40.ZeigeGesperrt Then
                    If (intAusschoepfung > intKreditlimit) Then
                        cell.ForeColor = System.Drawing.Color.Red
                    End If
                    'End If
                End If
            End If
        Next


        'If objChange40.ZeigeGesperrt Then
        '    DataGrid1.Columns(DataGrid1.Columns.Count - 3).Visible = False
        '    DataGrid1.Columns(DataGrid1.Columns.Count - 5).Visible = False
        'Else
        '    DataGrid1.Columns(DataGrid1.Columns.Count - 2).Visible = False
        '    DataGrid1.Columns(DataGrid1.Columns.Count - 4).Visible = False
        'End If
    End Sub

    Private Sub StartLoadData()


        cmdSave.Visible = True
        cmdConfirm.Visible = False
        cmdReset.Visible = False
        cmdAuthorize.Visible = False
        cmdBack.Visible = False
        cmdDelete.Visible = False

        If (objChange40.Kontingente Is Nothing) OrElse (objChange40.Kontingente.Rows.Count = 0) Then
            lblError.Text = "Fehler: Es konnten keine Kontingentdaten ermittelt werden."

            lblError.CssClass = "TextError"
        Else
            lblError.CssClass = "LabelExtraLarge"
            FillGrid()
        End If
    End Sub

    Private Sub DoSubmit1()
        Dim intKreditlimit_Alt As Int32
        Dim intKreditlimit_Neu As Int32
        Dim intRichtwert_Alt As Int32
        Dim intRichtwert_Neu As Int32
        Dim intAusschoepfung As Int32
        Dim blnGesperrt_Alt As Boolean
        Dim blnGesperrt_Neu As Boolean
        Dim blnInsolvenz_Alt As Boolean
        Dim blnInsolvenz_Neu As Boolean
        Dim strChangeMessage As String = ""

        Dim item As DataGridItem
        Dim cell As TableCell
        Dim chkBox As CheckBox
        Dim textbox As TextBox
        Dim image As System.Web.UI.WebControls.Image
        Dim control As Control
        Dim blnChanged As Boolean = False

        Dim i As Int32 = 0

        For Each item In DataGrid1.Items
            'Werte ermitteln

            'Alt
            Dim blnZeigeKontingentart As Boolean = CBool(objChange40.Kontingente.Rows(i)("ZeigeKontingentart"))
            If blnZeigeKontingentart Then
                intKreditlimit_Alt = CInt(objChange40.Kontingente.Rows(i)("Kontingent_Alt"))
                intRichtwert_Alt = CInt(objChange40.Kontingente.Rows(i)("Richtwert_Alt"))
                intRichtwert_Neu = CInt(objChange40.Kontingente.Rows(i)("Richtwert_Neu"))
                blnGesperrt_Alt = CBool(objChange40.Kontingente.Rows(i)("Gesperrt_Alt"))
                blnInsolvenz_Alt = CBool(objChange40.Kontingente.Rows(i)("Insolvent_Alt"))
            Else
                intKreditlimit_Alt = CInt(objChange40.Kontingente.Rows(i)("Kontingent_Alt"))
                intKreditlimit_Neu = CInt(objChange40.Kontingente.Rows(i)("Kontingent_Neu"))
                intRichtwert_Alt = CInt(objChange40.Kontingente.Rows(i)("Richtwert_Alt"))
                blnGesperrt_Alt = CBool(objChange40.Kontingente.Rows(i)("Gesperrt_Alt"))
                blnGesperrt_Neu = CBool(objChange40.Kontingente.Rows(i)("Gesperrt_Neu"))
                blnInsolvenz_Alt = CBool(objChange40.Kontingente.Rows(i)("Insolvent_Alt"))
                blnInsolvenz_Neu = CBool(objChange40.Kontingente.Rows(i)("Insolvent_Neu"))
            End If
            intAusschoepfung = CInt(objChange40.Kontingente.Rows(i)("Ausschoepfung"))
            i += 1

            'Neu
            'If Not objChange40.ZeigeGesperrt Then
            cell = item.Cells(6)
            For Each control In cell.Controls
                If TypeOf control Is TextBox Then
                    textbox = CType(control, TextBox)
                    If IsNumeric(textbox.Text) AndAlso (textbox.Text.Length < 5) AndAlso (Not CInt(textbox.Text) < 0) Then

                        If textbox.ID = "txtKontingent_Neu" And blnZeigeKontingentart Then
                            intKreditlimit_Neu = CInt(textbox.Text)
                        Else
                            If textbox.ID = "txtRichtwert_Neu" And (Not blnZeigeKontingentart) Then
                                intRichtwert_Neu = CInt(textbox.Text)
                            End If
                        End If

                    Else
                        strChangeMessage &= "Bitte geben Sie numerische, positive und max. vierstellige Kontigentwerte ein.<br>"
                    End If
                End If
            Next
            'End If

            cell = item.Cells(7)
            blnGesperrt_Neu = False
            For Each control In cell.Controls
                If TypeOf control Is CheckBox Then
                    chkBox = CType(control, CheckBox)
                    blnGesperrt_Neu = chkBox.Checked
                End If
            Next

            cell = item.Cells(6)
            If (Not (intKreditlimit_Alt = intKreditlimit_Neu)) Or (Not (intRichtwert_Alt = intRichtwert_Neu)) Then
                For Each control In cell.Controls
                    If TypeOf control Is System.Web.UI.WebControls.Image Then
                        image = CType(control, System.Web.UI.WebControls.Image)
                        image.ImageUrl = "/Portal/Images/arrow.gif"
                    End If
                Next
                blnChanged = True
            Else
                For Each control In cell.Controls
                    If TypeOf control Is System.Web.UI.WebControls.Image Then
                        image = CType(control, System.Web.UI.WebControls.Image)
                        image.ImageUrl = "/Portal/Images/empty.gif"
                    End If
                Next
            End If

            cell = item.Cells(7)
            If blnGesperrt_Alt = blnGesperrt_Neu Then
                For Each control In cell.Controls
                    If TypeOf control Is System.Web.UI.WebControls.Image Then
                        image = CType(control, System.Web.UI.WebControls.Image)
                        image.ImageUrl = "/Portal/Images/empty.gif"
                    End If
                Next
            Else
                For Each control In cell.Controls
                    If TypeOf control Is System.Web.UI.WebControls.Image Then
                        image = CType(control, System.Web.UI.WebControls.Image)
                        image.ImageUrl = "/Portal/Images/arrow.gif"
                    End If
                Next
                blnChanged = True
            End If

            For Each cell In item.Cells
                cell.ForeColor = System.Drawing.Color.Black
            Next
            If blnZeigeKontingentart Then
                If blnGesperrt_Neu Then
                    For Each cell In item.Cells
                        cell.ForeColor = System.Drawing.Color.Red
                    Next
                Else
                    'If Not objChange40.ZeigeGesperrt Then
                    If (intAusschoepfung > intKreditlimit_Neu) Then
                        For Each cell In item.Cells
                            cell.ForeColor = System.Drawing.Color.Red
                        Next
                    End If
                    'End If
                End If
            End If
        Next

        blnInsolvenz_Neu = chkInsolvent.Checked

        If blnInsolvenz_Alt <> blnInsolvenz_Neu Then
            blnChanged = True
        End If


        If blnChanged Then
            If strChangeMessage.Length = 0 Then
                For Each item In DataGrid1.Items
                    cell = item.Cells(6)
                    For Each control In cell.Controls
                        If TypeOf control Is TextBox Then
                            textbox = CType(control, TextBox)
                            textbox.Enabled = False
                        End If
                    Next
                    cell = item.Cells(7)
                    For Each control In cell.Controls
                        If TypeOf control Is CheckBox Then
                            chkBox = CType(control, CheckBox)
                            chkBox.Enabled = False
                        End If
                    Next
                Next
            End If
            chkInsolvent.Enabled = False
            cmdSave.Visible = False
            cmdConfirm.Visible = True
            cmdReset.Visible = True
            cmdAuthorize.Visible = False
            cmdBack.Visible = False
            cmdDelete.Visible = False
        Else
            strChangeMessage &= "Die Werte wurden nicht geändert."
            'Knöppe ausblenden/einblenden?
            'Tut nicht not!
        End If
        lblInformation.Text = strChangeMessage
        lblError.Text = strChangeMessage

        m_context.Cache.Insert("objChange40", objChange40, New System.Web.Caching.CacheDependency(Server.MapPath("Change40Edit.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        DoSubmit1()
    End Sub

    Private Sub DoSubmit2()
        If m_strInitiator.Length = 0 Then
            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

            Try

                Dim strKontingentart As String = ""
                Dim intKreditlimit_Alt As Int32
                Dim intKreditlimit_Neu As Int32
                Dim intRichtwert_Alt As Int32
                Dim intRichtwert_Neu As Int32
                Dim intAusschoepfung As Int32
                Dim blnGesperrt_Alt As Boolean
                Dim blnGesperrt_Neu As Boolean
                Dim blnInsolvenz_Alt As Boolean
                Dim blnInsolvenz_Neu As Boolean

                Dim item As DataGridItem
                Dim cell As TableCell
                Dim chkBox As CheckBox
                Dim textbox As TextBox
                Dim control As Control
                Dim i As Int32 = 0

                For Each item In DataGrid1.Items
                    'Werte ermitteln

                    'Alt
                    Dim blnZeigeKontingentart As Boolean = CBool(objChange40.Kontingente.Rows(i)("ZeigeKontingentart"))
                    If blnZeigeKontingentart Then
                        intKreditlimit_Alt = CInt(objChange40.Kontingente.Rows(i)("Kontingent_Alt"))
                        intRichtwert_Alt = CInt(objChange40.Kontingente.Rows(i)("Richtwert_Alt"))
                        intRichtwert_Neu = CInt(objChange40.Kontingente.Rows(i)("Richtwert_Neu"))
                        blnGesperrt_Alt = CBool(objChange40.Kontingente.Rows(i)("Gesperrt_Alt"))
                        blnInsolvenz_Alt = CBool(objChange40.Kontingente.Rows(i)("Insolvent_Alt"))
                    Else
                        intKreditlimit_Alt = CInt(objChange40.Kontingente.Rows(i)("Kontingent_Alt"))
                        intKreditlimit_Neu = CInt(objChange40.Kontingente.Rows(i)("Kontingent_Neu"))
                        intRichtwert_Alt = CInt(objChange40.Kontingente.Rows(i)("Richtwert_Alt"))
                        blnGesperrt_Alt = CBool(objChange40.Kontingente.Rows(i)("Gesperrt_Alt"))
                        blnGesperrt_Neu = CBool(objChange40.Kontingente.Rows(i)("Gesperrt_Neu"))
                        blnInsolvenz_Alt = CBool(objChange40.Kontingente.Rows(i)("Insolvent_Alt"))
                        blnInsolvenz_Neu = CBool(objChange40.Kontingente.Rows(i)("Insolvent_Neu"))
                    End If
                    intAusschoepfung = CInt(objChange40.Kontingente.Rows(i)("Ausschoepfung"))
                    strKontingentart = CStr(objChange40.Kontingente.Rows(i)("Kontingentart"))
                    i += 1

                    'Neu
                    'If Not objChange40.ZeigeGesperrt Then
                    cell = item.Cells(6)
                    For Each control In cell.Controls
                        If TypeOf control Is textbox Then
                            textbox = CType(control, textbox)
                            If textbox.ID = "txtKontingent_Neu" And blnZeigeKontingentart Then
                                intKreditlimit_Neu = CInt(textbox.Text)
                            Else
                                If textbox.ID = "txtRichtwert_Neu" And (Not blnZeigeKontingentart) Then
                                    intRichtwert_Neu = CInt(textbox.Text)
                                End If
                            End If
                        End If
                    Next
                    'End If

                    cell = item.Cells(7)
                    blnGesperrt_Neu = False
                    For Each control In cell.Controls
                        If TypeOf control Is CheckBox Then
                            chkBox = CType(control, CheckBox)
                            blnGesperrt_Neu = chkBox.Checked
                        End If
                    Next

                    blnInsolvenz_Neu = chkInsolvent.Checked

                    If Not ((intKreditlimit_Alt = intKreditlimit_Neu) And _
                                        (intRichtwert_Alt = intRichtwert_Neu) And _
                                            (blnGesperrt_Alt = blnGesperrt_Neu) And _
                                                (blnInsolvenz_Alt = blnInsolvenz_Neu)) Then

                        objChange40.Kontingente.AcceptChanges()
                        Dim tmpRows As DataRow()
                        tmpRows = objChange40.Kontingente.Select("Kontingentart = '" & strKontingentart & "'")
                        tmpRows(0).BeginEdit()
                        'If objChange40.ZeigeGesperrt Then
                        '    tmpRows(0).Item("Kontingent_Neu") = tmpRows(0).Item("Kontingent_Alt")
                        '    tmpRows(0).Item("Richtwert_Neu") = tmpRows(0).Item("Richtwert_Alt")
                        tmpRows(0).Item("Gesperrt_Neu") = blnGesperrt_Neu
                        'Else
                        tmpRows(0).Item("Kontingent_Neu") = intKreditlimit_Neu
                        tmpRows(0).Item("Richtwert_Neu") = intRichtwert_Neu
                        tmpRows(0).Item("Insolvent_Neu") = blnInsolvenz_Neu
                        '    tmpRows(0).Item("Gesperrt_Neu") = tmpRows(0).Item("Gesperrt_Alt")
                        'End If
                        tmpRows(0).EndEdit()
                        objChange40.Kontingente.AcceptChanges()

                    End If
                Next



                Dim tblLogDetails As DataTable = GetChanges()

                If (CInt(m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AuthorizationLevel")) > 0) Then
                    'Anwendung erfordert Autorisierung (Level>0)

                    Dim DetailArray(2, 2) As Object
                    Dim ms As MemoryStream
                    Dim formatter As BinaryFormatter
                    Dim b() As Byte

                    ms = New MemoryStream()
                    formatter = New BinaryFormatter()
                    formatter.Serialize(ms, objChange40)
                    b = ms.ToArray
                    ms = New MemoryStream(b)
                    DetailArray(0, 0) = ms
                    DetailArray(0, 1) = "objChange40"


                    ms = New MemoryStream()
                    formatter = New BinaryFormatter()
                    formatter.Serialize(ms, objSuche)
                    b = ms.ToArray
                    ms = New MemoryStream(b)
                    DetailArray(1, 0) = ms
                    DetailArray(1, 1) = "objSuche"
                    If objChange40.Customer.TrimStart("0"c) = Session("SelectedDealer").ToString Then
                        Dim intAuthorizationID As Int32 = WriteAuthorization(m_App.Connectionstring, CInt(Session("AppID")), m_User.UserName, m_User.Organization.OrganizationId, objChange40.Customer.TrimStart("0"c), "", strKontingentart, "", m_User.IsTestUser, DetailArray)

                        logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Session("SelectedDealer").ToString, "Kontingentänderung für Händler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ") erfolgreich zur Autorisierung gespeichert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, tblLogDetails)

                        LoadAuthorizatioData(intAuthorizationID)
                        ConfirmMessage.Visible = True
                        lblInformation.Text = "<b>Ihre Daten wurden zur Autorisierung gespeichert.</b><br>&nbsp;"
                    Else
                        lblError.Text = "Die Daten konnten aufgrund technischer Probleme NICHT gespeichert werden.<br>Bitte starten Sie die Händlersuche erneut."
                        Session("SelectedDealer") = Nothing
                        HttpContext.Current.Cache.Remove("objChange40")
                        cmdAuthorize.Visible = False
                        cmdBack.Visible = False
                        cmdConfirm.Visible = False
                        cmdDelete.Visible = False
                        cmdReset.Visible = False
                        cmdSave.Visible = False
                        cmdBack2.Visible = True
                        cmdBack2.NavigateUrl = lnkKreditlimit.NavigateUrl
                        Exit Sub
                    End If

                Else
                    'Anwendung erfordert keine Autorisierung (Level=0)

                    objChange40.Change(Session("AppID"), Session.SessionID)
                    If objChange40.Status = 0 Then

                        logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Session("SelectedDealer").ToString, "Kontingent von Händler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ") erfolgreich geändert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, tblLogDetails)
                        lblInformation.Text = "<b>Ihre Daten wurden gespeichert.</b><br>&nbsp;"
                        ConfirmMessage.Visible = True
                        If blnInsolvenz_Alt <> blnInsolvenz_Neu Then
                            SendMailToDAD(blnInsolvenz_Neu)
                        End If
                    Else
                        logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Session("SelectedDealer").ToString, "Fehler bei der Kontingentänderung von Händler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ", Fehler: " & objChange40.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
                        lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & objChange40.Message & ")"
                        lblError.CssClass = "TextError"
                        ConfirmMessage.Visible = False

                    End If
                    logApp.WriteStandardDataAccessSAP(objChange40.IDSAP)
                    objChange40.Show(Session("AppID").ToString, Session.SessionID)
                    StartLoadData()
                End If

            Catch ex As Exception
                m_App.WriteErrorText(1, m_User.UserName, "Change40Edit", "DoSubmit2", ex.ToString)

                logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Session("SelectedDealer").ToString, "Fehler bei der Kontingentänderung von Händler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ", Fehler: " & ex.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
                lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                lblError.CssClass = "TextError"
                ConfirmMessage.Visible = False

                objChange40.Show(Session("AppID").ToString, Session.SessionID)
                Throw ex
            End Try
            m_context.Cache.Insert("objChange40", objChange40, New System.Web.Caching.CacheDependency(Server.MapPath("Change40Edit.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
        End If
    End Sub

    Private Sub DoSubmit3()
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        Dim blnError As Boolean = False
        Try

            Dim tblLogDetails As DataTable = GetChanges()

            objChange40.SessionID = Session.SessionID.ToString
            objChange40.Change(Session("AppID"), Session.SessionID)
            If objChange40.Status = 0 Then

                logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Session("SelectedDealer").ToString, "Kontingent von Händler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ") erfolgreich geändert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, tblLogDetails)
                logApp.WriteStandardDataAccessSAP(objChange40.IDSAP)
                lblInformation.Text = "<b>Ihre Daten wurden gespeichert.</b><br>&nbsp;"
                DeleteAuthorizationEntry(m_App.Connectionstring, CInt(Session("AuthorizationID")))
                Session("Authorization") = Nothing
                Session("AuthorizationID") = Nothing
                Session("objSuche") = objSuche
                ConfirmMessage.Visible = True

                blnError = False
                cmdSave.Enabled = True
                objChange40.Show(Session("AppID").ToString, Session.SessionID)
                StartLoadData()
                m_context.Cache.Insert("objChange40", objChange40, New System.Web.Caching.CacheDependency(Server.MapPath("Change40Edit.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
                If Not blnError Then
                    'zurueck zur Liste oder Hauptmenue
                    Dim strLastRecord As String = CStr(Request.QueryString("LastRecord"))
                    Try
                        If Not strLastRecord = "True" Then
                            Response.Redirect("Change48.aspx?AppID=" & CStr(m_User.Applications.Select("AppName='Change48'")(0).Item("AppID")), False)
                        Else
                            Response.Redirect("../../../Start/Selection.aspx", False)
                        End If
                    Catch
                    End Try
                End If
            Else
                m_context.Cache.Insert("objChange40", objChange40, New System.Web.Caching.CacheDependency(Server.MapPath("Change40Edit.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
                logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Session("SelectedDealer").ToString, "Fehler bei der Kontingentänderung von Händler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ", Fehler: " & objChange40.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
                logApp.WriteStandardDataAccessSAP(objChange40.IDSAP)
                lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & objChange40.Message & ")"
                lblError.CssClass = "TextError"
                ConfirmMessage.Visible = False

                blnError = True
            End If

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Change40Edit", "DoSubmit3", ex.ToString)

            m_context.Cache.Insert("objChange40", objChange40, New System.Web.Caching.CacheDependency(Server.MapPath("Change40Edit.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
            logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Session("SelectedDealer").ToString, "Fehler bei der Kontingentänderung von Händler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ", Fehler: " & ex.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
            lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            lblError.CssClass = "TextError"
            ConfirmMessage.Visible = False

            blnError = True
            Throw ex
        End Try
    End Sub

    Private Sub DoSubmit4()
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

        Try

            Dim tblLogDetails As DataTable = GetChanges()

            DeleteAuthorizationEntry(m_App.Connectionstring, CInt(Session("AuthorizationID")))

            logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Session("SelectedDealer").ToString, "Kontingentänderung für Händler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ") aus Autorisierung gelöscht.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, tblLogDetails)
            lblInformation.Text = "<b>Daten wurden aus Autorisierung entfernt.</b><br>&nbsp;"
            Session("Authorization") = Nothing
            Session("AuthorizationID") = Nothing
            Session("objSuche") = objSuche
            ConfirmMessage.Visible = True

            cmdSave.Enabled = True

            'zurueck zum Hauptmenue
            Try
                Response.Redirect("../../../Start/Selection.aspx", False)
            Catch
            End Try
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Change40Edit", "DoSubmit4", ex.ToString)

            logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Session("SelectedDealer").ToString, "Fehler bei der Kontingentänderung von Händler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ", Fehler: " & ex.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
            lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            ConfirmMessage.Visible = False

        End Try
        objChange40.Show(Session("AppID").ToString, Session.SessionID)
        StartLoadData()
        m_context.Cache.Insert("objChange40", objChange40, New System.Web.Caching.CacheDependency(Server.MapPath("Change40Edit.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
    End Sub

    Private Sub SendMailToDAD(ByVal Insolvenz As Boolean)
        Dim clsMail As New Base.Kernel.Common.GetMailTexte(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)

        If Insolvenz = True Then
            clsMail.LeseMailTexte("1")
        Else
            clsMail.LeseMailTexte("2")
        End If
        If clsMail.Status = 0 Then
            Try
                Dim Mail As Net.Mail.MailMessage
                Dim smtpMailSender As String
                Dim smtpMailServer As String

                smtpMailSender = ConfigurationManager.AppSettings("SmtpMailSender")

                clsMail.Betreff = clsMail.Betreff.Replace("#Haendlernr#", lblHaendlerNummer.Text)
                clsMail.MailBody = clsMail.MailBody.Replace("#Haendlernr#", lblHaendlerNummer.Text)
                clsMail.MailBody = clsMail.MailBody.Replace("#Haendlername#", lblHaendlerName.Text)
                clsMail.MailBody = clsMail.MailBody.Replace("#User#", m_User.UserName)
                Dim Adressen() As String
                If Split(clsMail.MailAdress.Trim, ";").Length > 1 Then

                    Mail = New Net.Mail.MailMessage()

                    Dim Mailsender As New Net.Mail.MailAddress(smtpMailSender)

                    Mail.Sender = Mailsender
                    Mail.From = Mailsender
                    Mail.Body = clsMail.MailBody
                    Mail.Subject = clsMail.Betreff

                    Adressen = Split(clsMail.MailAdress.Trim, ";")
                    For Each tmpStr As String In Adressen
                        Mail.To.Add(tmpStr)
                    Next
                Else
                    Mail = New Net.Mail.MailMessage(smtpMailSender, clsMail.MailAdress.Trim, clsMail.Betreff, clsMail.MailBody)
                End If

                If Split(clsMail.MailAdressCC.Trim, ";").Length > 1 Then
                    Adressen = Split(clsMail.MailAdressCC.Trim, ";")
                    For Each tmpStr As String In Adressen
                        Mail.CC.Add(tmpStr)
                    Next
                Else
                    If clsMail.MailAdressCC.Length > 0 Then
                        Mail.CC.Add(clsMail.MailAdressCC)
                    End If
                End If

                Mail.IsBodyHtml = True
                smtpMailServer = ConfigurationManager.AppSettings("SmtpMailServer")
                Dim client As New Net.Mail.SmtpClient(smtpMailServer)
                client.Send(Mail)
            Catch ex As Exception
                lblError.Text = "Fehler beim Versenden der E-Mail."
            End Try

        End If
    End Sub

    Private Sub cmdConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdConfirm.Click
        DoSubmit2()
    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        objChange40.Show(Session("AppID").ToString, Session.SessionID)
        StartLoadData()
        m_context.Cache.Insert("objChange40", objChange40, New System.Web.Caching.CacheDependency(Server.MapPath("Change40Edit.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
    End Sub

    'Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
    '    If cmdConfirm.Visible Then
    '        DoSubmit2()
    '    Else
    '        DoSubmit1()
    '    End If
    'End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        DoSubmit4()
    End Sub

    Private Sub cmdAuthorize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAuthorize.Click
        DoSubmit3()
    End Sub

    Private Function GetChanges() As DataTable
        Dim m_tblKontingenteChanged As DataTable
        m_tblKontingenteChanged = New DataTable()
        m_tblKontingenteChanged.Columns.Add("Status", System.Type.GetType("System.String"))
        m_tblKontingenteChanged.Columns.Add("Händler", System.Type.GetType("System.String"))
        m_tblKontingenteChanged.Columns.Add("Kontingentart", System.Type.GetType("System.String"))
        m_tblKontingenteChanged.Columns.Add("Kontingent", System.Type.GetType("System.Int32"))
        m_tblKontingenteChanged.Columns.Add("Richtwert", System.Type.GetType("System.Int32"))
        m_tblKontingenteChanged.Columns.Add("Ausschoepfung", System.Type.GetType("System.Int32"))
        m_tblKontingenteChanged.Columns.Add("Frei", System.Type.GetType("System.Int32"))
        m_tblKontingenteChanged.Columns.Add("Gesperrt", System.Type.GetType("System.Boolean"))
        m_tblKontingenteChanged.Columns.Add("Insolvenzsperre", System.Type.GetType("System.Boolean"))

        Dim rowTemp As DataRow
        For Each rowTemp In objChange40.Kontingente.Rows
            Dim tmpRow2 As DataRow
            tmpRow2 = m_tblKontingenteChanged.NewRow
            tmpRow2("Status") = "Alt"
            tmpRow2("Händler") = objSuche.REFERENZ
            tmpRow2("Kontingentart") = rowTemp("Kontingentart")
            tmpRow2("Kontingent") = rowTemp("Kontingent_Alt")
            tmpRow2("Richtwert") = rowTemp("Richtwert_Alt")
            tmpRow2("Ausschoepfung") = rowTemp("Ausschoepfung")
            tmpRow2("Frei") = CInt(rowTemp("Kontingent_Alt")) - CInt(rowTemp("Ausschoepfung"))
            tmpRow2("Gesperrt") = rowTemp("Gesperrt_Alt")
            tmpRow2("Insolvenzsperre") = rowTemp("Insolvent_Alt")
            m_tblKontingenteChanged.Rows.Add(tmpRow2)
            If (Not CInt(rowTemp("Kontingent_Alt")) = CInt(rowTemp("Kontingent_Neu"))) Or (Not CInt(rowTemp("Richtwert_Alt")) = CInt(rowTemp("Richtwert_Neu"))) Or (Not CBool(rowTemp("Gesperrt_Alt")) = CBool(rowTemp("Gesperrt_Neu"))) Or _
                 (Not (rowTemp("Insolvent_Alt")) = (rowTemp("Insolvent_Neu"))) Then
                tmpRow2 = m_tblKontingenteChanged.NewRow
                tmpRow2("Status") = "Neu"
                tmpRow2("Händler") = objSuche.REFERENZ
                tmpRow2("Kontingentart") = rowTemp("Kontingentart")
                tmpRow2("Kontingent") = rowTemp("Kontingent_Neu")
                tmpRow2("Richtwert") = rowTemp("Richtwert_Neu")
                tmpRow2("Ausschoepfung") = rowTemp("Ausschoepfung")
                tmpRow2("Frei") = CInt(rowTemp("Kontingent_Neu")) - CInt(rowTemp("Ausschoepfung"))
                tmpRow2("Gesperrt") = rowTemp("Gesperrt_Neu")
                tmpRow2("Insolvenzsperre") = rowTemp("Insolvent_Neu")
                m_tblKontingenteChanged.Rows.Add(tmpRow2)
            End If
        Next
        Return m_tblKontingenteChanged
    End Function

    Private Sub LoadAuthorizatioData(ByVal AuthorizationID As Int32)
        Dim OutPutStream As System.IO.MemoryStream = GiveAuthorizationDetails(m_App.Connectionstring, AuthorizationID, "objChange40")
        Dim formatter As New BinaryFormatter()
        objChange40 = New BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        objChange40 = DirectCast(formatter.Deserialize(OutPutStream), BankBaseCredit)
        FillGrid()
        DoSubmit1()


        cmdSave.Visible = False
        cmdConfirm.Visible = False
        cmdReset.Visible = False
        cmdAuthorize.Visible = False
        cmdBack.Visible = False
        cmdDelete.Visible = False
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click

        Response.Redirect("Change48.aspx?AppID=" & CStr(m_User.Applications.Select("AppName='Change48'")(0).Item("AppID")) & "&Aut=@!", False)
       
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub chkInsolvent_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkInsolvent.CheckedChanged

        DataGrid1.DataSource = objChange40.Kontingente
        DataGrid1.DataBind()

        Dim item As DataGridItem
        Dim cell As TableCell
        Dim chkBox As CheckBox
        Dim textbox As TextBox
        Dim control As Control

        Dim intKreditlimit As Int32
        Dim intAusschoepfung As Int32
        Dim blnGesperrt As Boolean
        Dim label As Label


        For Each item In DataGrid1.Items
            Dim blnZeigeKontingentart As Boolean
            cell = item.Cells(item.Cells.Count - 1)
            For Each control In cell.Controls
                Dim checkBox = TryCast(control, CheckBox)
                If (checkBox IsNot Nothing) Then
                    chkBox = checkBox
                    blnZeigeKontingentart = chkBox.Checked
                End If
            Next
            cell = item.Cells(2)
            For Each control In cell.Controls
                Dim control1 = TryCast(control, Label)
                If (control1 IsNot Nothing) Then
                    label = control1
                    If label.ID = "lblKontingent_Alt" And blnZeigeKontingentart Then
                        label.Visible = True
                        intKreditlimit = CInt(label.Text)
                    Else
                        label.Visible = False
                        If label.ID = "lblRichtwert_Alt" And (Not blnZeigeKontingentart) Then
                            label.Visible = True
                            intKreditlimit = CInt(label.Text)
                        Else
                            label.Visible = False
                        End If
                    End If
                End If
            Next

            intAusschoepfung = CInt(item.Cells(3).Text)

            cell = item.Cells(4)
            For Each control In cell.Controls
                If TypeOf control Is Label Then
                    label = CType(control, Label)
                    If Not blnZeigeKontingentart Then
                        label.Visible = False
                    End If
                End If
            Next

            cell = item.Cells(5)
            For Each control In cell.Controls
                If TypeOf control Is CheckBox Then
                    chkBox = CType(control, CheckBox)
                    blnGesperrt = chkBox.Checked
                    If Not blnZeigeKontingentart Then
                        chkBox.Visible = False
                    End If
                End If
            Next

            cell = item.Cells(6)
            For Each control In cell.Controls
                If TypeOf control Is TextBox Then
                    textbox = CType(control, TextBox)
                    If textbox.ID = "txtKontingent_Neu" And blnZeigeKontingentart Then
                        textbox.Visible = True
                        If chkInsolvent.Checked = True Then
                            textbox.Enabled = False
                        End If
                    Else
                        textbox.Visible = False
                        If textbox.ID = "txtRichtwert_Neu" And (Not blnZeigeKontingentart) Then
                            textbox.Visible = True
                            If chkInsolvent.Checked = True Then
                                textbox.Enabled = False
                            End If
                        Else
                            textbox.Visible = False
                        End If
                    End If
                End If
            Next

            cell = item.Cells(7)
            For Each control In cell.Controls
                Dim checkBox = TryCast(control, CheckBox)
                If (checkBox IsNot Nothing) Then
                    chkBox = checkBox
                    If chkInsolvent.Checked = True Then
                        chkBox.Enabled = False
                    End If
                    If Not blnZeigeKontingentart Then
                        chkBox.Visible = False
                    End If
                End If
            Next

            If blnZeigeKontingentart Then
                If Not blnGesperrt Then
                    If (intAusschoepfung > intKreditlimit) Then
                        cell.ForeColor = Color.Red
                    End If
                End If
            End If
        Next

    End Sub
End Class

' ************************************************
' $History: Change40Edit.aspx.vb $
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 3.03.10    Time: 13:52
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 2.03.10    Time: 17:18
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 2.03.10    Time: 14:35
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA: 3509, 3515, 3522
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 22.06.09   Time: 14:20
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 Z_M_Creditlimit_Change_001
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 22.06.09   Time: 13:18
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 Z_M_Creditlimit_Detail_001
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 19.06.09   Time: 10:10
' Updated in $/CKAG/Components/ComCommon/Finance
' ITa 2918 testfertig
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 5.06.09    Time: 13:51
' Updated in $/CKAG/Components/ComCommon/Finance
' Try Catch entfernt wenn möglich
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 16.03.09   Time: 17:06
' Updated in $/CKAG/Components/ComCommon/Finance
' RTFS Autorisierungskorrekturen
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 16.03.09   Time: 16:55
' Updated in $/CKAG/Components/ComCommon/Finance
' autorisierungsänderung/Berichtung RTFS
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:47
' Created in $/CKAG/portal/Components/ComCommon/Finance
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 25.01.08   Time: 13:51
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Rothe Verbesserungen RTFS TEIL 2
' 
' *****************  Version 6  *****************
' User: Uha          Date: 8.01.08    Time: 16:53
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 5  *****************
' User: Uha          Date: 17.12.07   Time: 17:58
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Versand ZB II / Briefe - komplierfähige Zwischenversion
' 
' *****************  Version 4  *****************
' User: Uha          Date: 13.12.07   Time: 13:58
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1481/1509 (Änderung / Sperrung Händlerkontingent) Testversion
' 
' *****************  Version 3  *****************
' User: Uha          Date: 13.12.07   Time: 9:49
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' fin_06.vb durch BankBaseCredit.vb ersetzt
' 
' *****************  Version 2  *****************
' User: Uha          Date: 12.12.07   Time: 15:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1473/1497 (Mahnstufe 3) als Testversion; ITA 1481/1509
' (Änderung/Sperrung Händlerkontingent) komplierfähig
' 
' *****************  Version 1  *****************
' User: Uha          Date: 12.12.07   Time: 13:23
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Übernahme der Suchfunktion aus FFD (ohne Anpassung)
' 
' ************************************************
