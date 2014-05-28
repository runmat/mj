Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Imports System
Imports System.IO
Imports System.Collections
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.Text.RegularExpressions

' Anfroderungsnummer 968
' Erstellt am: 17.04.2007 - Tim Bardenhagen
Public Class Change17Edit
    Inherits System.Web.UI.Page

    Private m_context As HttpContext = HttpContext.Current
    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private objSuche As Search
    Private objChange17_objFDDZahlungsfrist As FDD_Zahlungsfrist
    Private objFDDBank As BankBaseCredit
    Private m_strInitiator As String
    Dim m_tblKontingenteChanged As DataTable
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lnkHaendlerSuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ConfirmMessage As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents FocusScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdAuthorize As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblInformation As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblHaendlerNummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblHaendlerName As System.Web.UI.WebControls.Label
    Protected WithEvents lblAdresse As System.Web.UI.WebControls.Label
    Protected WithEvents txt_NF As System.Web.UI.WebControls.TextBox
    Protected WithEvents chk_temp As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chk_endgueltig As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chk_Retail As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chk_dp As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblPflege As System.Web.UI.WebControls.Label
    Protected WithEvents btn_Ok As System.Web.UI.WebControls.LinkButton
    Protected WithEvents trPflegelabel As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trPflege As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucStyles As Styles

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

#Region "Events"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSave.Enabled = False
        lblPageTitle.Text = "Werte ändern"

        lblInformation.Text = ""
        m_strInitiator = ""

        Try
            m_User = GetUser(Me)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)

            m_App = New Base.Kernel.Security.App(m_User)

            If Session("SelectedDealer") = Nothing OrElse (Session("objSuche") Is Nothing) Then
                If (Not Session("Authorization") Is Nothing) AndAlso CBool(Session("Authorization")) AndAlso _
                    (Not Session("AuthorizationID") Is Nothing) AndAlso IsNumeric(Session("AuthorizationID")) Then
                    Dim OutPutStream As System.IO.MemoryStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(Session("AuthorizationID")), "objSuche")
                    If OutPutStream Is Nothing Then
                        lblError.Text = "Keine Daten für den Vorgang vorhanden."
                    Else
                        Dim formatter As New BinaryFormatter()
                        objSuche = New Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                        objSuche = DirectCast(formatter.Deserialize(OutPutStream), Search)
                        objSuche.ReNewSAPDestination(Session.SessionID.ToString, Session("AppID").ToString)
                    End If
                    trPflege.Visible = False
                    trPflegelabel.Visible = False
                Else
                    Try
                    Catch
                        Response.Redirect("Change17.aspx?AppID=" & Session("AppID").ToString)
                    End Try
                    objSuche = CType(Session("objSuche"), Search)
                End If
            Else
                objSuche = CType(Session("objSuche"), Search)
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
            lnkHaendlerSuche.NavigateUrl = "Change17.aspx?AppID=" & Session("AppID").ToString & "&Back=1" & "&ID=" & Request.QueryString("ID")

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
                    Dim OutPutStream As System.IO.MemoryStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(Session("AuthorizationID")), "objFDDZahlungsfrist")
                    If OutPutStream Is Nothing Then
                        lblError.Text = "Keine Daten für den Vorgang vorhanden."
                    Else
                        'Dim formatter As New BinaryFormatter()
                        'objChange17_objFDDZahlungsfrist = New FDD_Zahlungsfrist(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                        'objChange17_objFDDZahlungsfrist = DirectCast(formatter.Deserialize(OutPutStream), FDD_Zahlungsfrist)
                        'FillGrid()

                        FocusScript.Visible = False

                        'cmdSave.Visible = False
                        Dim intAuthorizationID As Int32 = Session("AuthorizationID")
                        cmdAuthorize.Visible = True
                        cmdBack.Visible = True
                        LoadAuthorizatioData(intAuthorizationID)

                    End If
                End If
            Else
                Dim intAuthorizationID As Int32
                Dim iDistrictID As Int16
                If Not Session("SelectedDistrict") = Nothing Then
                    iDistrictID = Session("SelectedDistrict")
                Else
                    iDistrictID = m_User.Organization.OrganizationId
                End If
                m_App.CheckForPendingAuthorization(CInt(Session("AppID")), iDistrictID, Session("SelectedDealer").ToString, "", m_User.IsTestUser, m_strInitiator, intAuthorizationID)
                If Not m_strInitiator.Length = 0 Then
                    'Seite gesperrt aufgerufen, da Händlerdaten in Autorisierung

                    If Not IsPostBack Then
                    LoadAuthorizatioData(intAuthorizationID)

                    lblInformation.Text = "Die Angaben zum Händler " & Session("SelectedDealer").ToString & " wurden vom Benutzer """ & m_strInitiator & """ geändert.<br>&nbsp;&nbsp;Die Autorisierung steht noch aus!"
                    End If
                Else
                    'Seite im normalen Änderungsmodus aufgerufen

                    If Not IsPostBack Then
                        objChange17_objFDDZahlungsfrist = New FDD_Zahlungsfrist(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                        objChange17_objFDDZahlungsfrist.Customer = "60" & Session("SelectedDealer").ToString
                        'objChange17_objFDDZahlungsfrist.KUNNR = m_User.KUNNR
                        objChange17_objFDDZahlungsfrist.CreditControlArea = "ZDAD"
                        If (Not Request.QueryString("ID") Is Nothing) AndAlso (Request.QueryString("ID").Length > 0) Then
                            objChange17_objFDDZahlungsfrist.ZeigeGesperrt = True
                        Else
                            objChange17_objFDDZahlungsfrist.ZeigeGesperrt = False
                        End If
                    Else
                        objChange17_objFDDZahlungsfrist = CType(m_context.Cache("objChange17_objFDDZahlungsfrist"), FDD_Zahlungsfrist)
                        objFDDBank = Session("objFDDBank")
                    End If
                    objChange17_objFDDZahlungsfrist.Show(Session("AppID").ToString, Session.SessionID, Me)
                    If objChange17_objFDDZahlungsfrist.Status = 0 Then
                        If Not IsPostBack Then
                            StartLoadData()
                        End If
                        Session("objChange17_objFDDZahlungsfrist") = objChange17_objFDDZahlungsfrist
                        checkkontingente()
                        cmdSave.Enabled = True
                    Else
                        lblError.Text = objChange17_objFDDZahlungsfrist.Message
                        trPflegelabel.Visible = False
                        trPflege.Visible = False
                        FocusScript.Visible = False
                    End If
                End If
            End If

            If Not IsPostBack Then
                m_context.Cache.Insert("objChange17_objFDDZahlungsfrist", objChange17_objFDDZahlungsfrist, New System.Web.Caching.CacheDependency(Server.MapPath("Change17Edit.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Change17Edit", "Page_Load", ex.ToString)

            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Try
            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            Dim item As DataGridItem
            For Each item In DataGrid1.Items
                'Werte ermitteln

                Dim cell As TableCell
                cell = item.Cells(0)
                Dim strAuftragsnummer As String
                strAuftragsnummer = cell.Text

                For Each cell In item.Cells

                    Dim c As System.Web.UI.Control
                    For Each c In cell.Controls

                        If TypeOf c Is TextBox Then

                            Dim t As TextBox = CType(c, TextBox)

                            'Neue Zahlungsfrist auslesen und in DataRow schreiben
                            If t.ID = "txtZahlungsfristNeu" AndAlso t.Enabled = True Then
                                If Regex.IsMatch(t.Text.Trim, "^\d{0,2}$", RegexOptions.Singleline) AndAlso t.Text.Length <> 0 Then
                                    objChange17_objFDDZahlungsfrist = New FDD_Zahlungsfrist(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                                    objChange17_objFDDZahlungsfrist = Session("objChange17_objFDDZahlungsfrist")

                                    Dim sKontingentart As String
                                    Dim cell2 As TableCell
                                    cell2 = item.Cells(1)
                                    sKontingentart = cell2.Text

                                    Dim sKKber As String

                                    cell2 = item.Cells(0)
                                    sKKber = cell2.Text
                                    If sKKber = "0001" Then
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(0)(FDD_Zahlungsfrist.COLUMN_KONTINGENTART) = sKontingentart
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(0)(FDD_Zahlungsfrist.COLUMN_KONTINGENTID) = sKKber
                                    End If
                                    If sKKber = "0002" Then
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(1)(FDD_Zahlungsfrist.COLUMN_KONTINGENTART) = sKontingentart
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(1)(FDD_Zahlungsfrist.COLUMN_KONTINGENTID) = sKKber
                                    End If
                                    If sKKber = "0003" Then
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(2)(FDD_Zahlungsfrist.COLUMN_KONTINGENTART) = sKontingentart
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(2)(FDD_Zahlungsfrist.COLUMN_KONTINGENTID) = sKKber
                                    End If
                                    If sKKber = "0004" Then
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(3)(FDD_Zahlungsfrist.COLUMN_KONTINGENTART) = sKontingentart
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(3)(FDD_Zahlungsfrist.COLUMN_KONTINGENTID) = sKKber
                                    End If
                                    If (CInt(m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AuthorizationLevel")) > 0) Then
                                        'Anwendung erfordert Autorisierung (Level>0)
                                        Dim DetailArray(2, 2) As Object
                                        Dim ms As MemoryStream
                                        Dim formatter As BinaryFormatter
                                        Dim b() As Byte

                                        ms = New MemoryStream()
                                        formatter = New BinaryFormatter()
                                        formatter.Serialize(ms, objSuche)
                                        b = ms.ToArray
                                        ms = New MemoryStream(b)
                                        DetailArray(0, 0) = ms
                                        DetailArray(0, 1) = "objSuche"
                                        If sKKber = "0001" Then
                                            objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(0)(FDD_Zahlungsfrist.COLUMN_ZAHLUNGSFRIST_NEU) = t.Text.Trim
                                        End If
                                        If sKKber = "0002" Then
                                            objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(1)(FDD_Zahlungsfrist.COLUMN_ZAHLUNGSFRIST_NEU) = t.Text.Trim
                                        End If
                                        If sKKber = "0003" Then
                                            objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(2)(FDD_Zahlungsfrist.COLUMN_ZAHLUNGSFRIST_NEU) = t.Text.Trim
                                        End If
                                        If sKKber = "0004" Then
                                            objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(3)(FDD_Zahlungsfrist.COLUMN_ZAHLUNGSFRIST_NEU) = t.Text.Trim
                                        End If
                                        ms = New MemoryStream()
                                        formatter = New BinaryFormatter()
                                        formatter.Serialize(ms, objChange17_objFDDZahlungsfrist)
                                        b = ms.ToArray
                                        ms = New MemoryStream(b)
                                        DetailArray(1, 0) = ms
                                        DetailArray(1, 1) = "objFDDZahlungsfrist"

                                        'Pruefen, ob schon in der Autorisierung.
                                        Dim strInitiator As String = ""
                                        Dim intAuthorizationID As Int32
                                        Dim iDistrictID As Integer
                                        If Not Session("SelectedDistrict") = Nothing Then
                                            iDistrictID = Session("SelectedDistrict")
                                        Else
                                            iDistrictID = m_User.Organization.OrganizationId
                                        End If
                                        Dim sString As String = Session("AppID")
                                        If t.Enabled = True Then
                                            m_App.CheckForPendingAuthorization(CInt(Session("AppID")), iDistrictID, objSuche.REFERENZ, "", m_User.IsTestUser, strInitiator, intAuthorizationID)
                                            intAuthorizationID = WriteAuthorization(m_App.Connectionstring, CInt(Session("AppID")), m_User.UserName, iDistrictID, objSuche.REFERENZ, "", "", sKKber, m_User.IsTestUser, DetailArray)
                                            logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(Session("SelectedDealer").ToString, 5), "Zahlungsfrist für Händler " & objSuche.REFERENZ & " erfolgreich initiiert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                                            lblInformation.Text = "Die Änderung der Zahlungsfrist wurde initiiert und liegt zur Autorisierung vor!"
                                            FocusScript.Visible = False
                                            cmdSave.Visible = False
                                            t.Enabled = False
                                        End If
                                    Else
                                        'objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(0)(FDD_Zahlungsfrist.COLUMN_ZAHLUNGSFRIST_NEU) = t.Text.Trim
                                    End If

                                Else
                                    'Throw New Exception("Bitte geben Sie eine Zahl von 0 bis 99 ein oder lassen Sie das Feld leer.")
                                End If
                            End If
                        End If
                    Next

                Next

            Next

            'objChange17_objFDDZahlungsfrist.Change()
            'logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Session("SelectedDealer").ToString, "Die Zahlungsfrist für Händler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ") wurde erfolgreich geändert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

            'Me.FillGrid()

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Change17Edit", "Page_Load", ex.ToString)

            lblError.Text = ex.Message

        End Try
    End Sub

    Private Sub cmdAuthorize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAuthorize.Click
        Try
            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            Dim item As DataGridItem
            For Each item In DataGrid1.Items
                'Werte ermitteln

                Dim cell As TableCell
                cell = item.Cells(0)
                Dim strAuftragsnummer As String
                strAuftragsnummer = cell.Text

                For Each cell In item.Cells

                    Dim c As System.Web.UI.Control
                    For Each c In cell.Controls

                        If TypeOf c Is TextBox Then

                            Dim t As TextBox = CType(c, TextBox)

                            'Neue Zahlungsfrist auslesen und in DataRow schreiben
                            If t.ID = "txtZahlungsfristNeu" AndAlso t.Visible = True Then

                                If Regex.IsMatch(t.Text.Trim, "^\d{0,2}$", RegexOptions.Singleline) Then
                                    If strAuftragsnummer = "0001" Then
                                        objChange17_objFDDZahlungsfrist.RowFaelligkeit = 0
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(0)(FDD_Zahlungsfrist.COLUMN_ZAHLUNGSFRIST_NEU) = t.Text.Trim
                                    End If
                                    If strAuftragsnummer = "0002" Then
                                        objChange17_objFDDZahlungsfrist.RowFaelligkeit = 1
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(1)(FDD_Zahlungsfrist.COLUMN_ZAHLUNGSFRIST_NEU) = t.Text.Trim
                                    End If
                                    If strAuftragsnummer = "0003" Then
                                        objChange17_objFDDZahlungsfrist.RowFaelligkeit = 2
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(2)(FDD_Zahlungsfrist.COLUMN_ZAHLUNGSFRIST_NEU) = t.Text.Trim
                                    End If
                                    If strAuftragsnummer = "0004" Then
                                        objChange17_objFDDZahlungsfrist.RowFaelligkeit = 3
                                        objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows(3)(FDD_Zahlungsfrist.COLUMN_ZAHLUNGSFRIST_NEU) = t.Text.Trim
                                    End If
                                Else
                                    Throw New Exception("Bitte geben Sie eine Zahl von 0 bis 99 ein oder lassen Sie das Feld leer.")
                                End If
                            End If

                        End If
                    Next

                Next

            Next

            objChange17_objFDDZahlungsfrist.Change(Session("AppID").ToString, Session.SessionID, Me)
            logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Session("SelectedDealer").ToString, "Die Zahlungsfrist für Händler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ") wurde erfolgreich geändert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
            DeleteAuthorizationEntry(m_App.Connectionstring, Session("AuthorizationID"))
            lblError.Text = "Die Zahlungsfrist für Händler " & lblHaendlerNummer.Text & " wurde erfolgreich geändert."
            DataGrid1.Visible = False
            cmdAuthorize.Visible = False
            cmdBack.Visible = True
            'Me.FillGrid()

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Change17Edit", "Page_Load", ex.ToString)

            lblError.Text = ex.Message

        End Try
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Try
            Response.Redirect("Change14.aspx?AppID=" & CStr(m_User.Applications.Select("AppName='Change14'")(0).Item("AppID")) & "&Aut=@!", False)
        Catch
        End Try
    End Sub
    Private Sub btn_Ok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Ok.Click


        If IsNumeric(txt_NF.Text) = True Then
            Dim sNF As String
            Dim cell As TableCell
            Dim cell2 As TableCell
            Dim c As System.Web.UI.Control
            Dim item As DataGridItem
            sNF = txt_NF.Text
            If chk_temp.Checked = True Then

                For Each item In DataGrid1.Items
                    cell = item.Cells(0)
                    For Each cell In item.Cells
                        If cell.Text = "0001" Then
                            cell2 = item.Cells(3)
                            For Each c In cell2.Controls
                                If TypeOf c Is TextBox Then
                                    Dim t As TextBox = CType(c, TextBox)
                                    If t.ID = "txtZahlungsfristNeu" Then
                                        t.Text = sNF
                                        cmdSave.Enabled = True
                                    End If
                                End If
                            Next
                        End If
                    Next
                Next
            End If
            If chk_endgueltig.Checked = True Then
                For Each item In DataGrid1.Items
                    cell = item.Cells(0)
                    For Each cell In item.Cells
                        If cell.Text = "0002" Then
                            cell2 = item.Cells(3)
                            For Each c In cell2.Controls
                                If TypeOf c Is TextBox Then
                                    Dim t As TextBox = CType(c, TextBox)
                                    If t.ID = "txtZahlungsfristNeu" Then
                                        t.Text = sNF
                                        cmdSave.Enabled = True
                                    End If

                                End If
                            Next
                        End If
                    Next
                Next
            End If
            If chk_Retail.Checked = True Then
                For Each item In DataGrid1.Items
                    cell = item.Cells(0)
                    For Each cell In item.Cells
                        If cell.Text = "0003" Then
                            cell2 = item.Cells(3)
                            For Each c In cell2.Controls
                                If TypeOf c Is TextBox Then
                                    Dim t As TextBox = CType(c, TextBox)
                                    If t.ID = "txtZahlungsfristNeu" Then
                                        t.Text = sNF
                                        cmdSave.Enabled = True
                                    End If

                                End If
                            Next
                        End If
                    Next
                Next
            End If
            If chk_dp.Checked = True Then
                For Each item In DataGrid1.Items
                    cell = item.Cells(0)
                    For Each cell In item.Cells
                        If cell.Text = "0004" Then
                            cell2 = item.Cells(3)
                            For Each c In cell2.Controls
                                If TypeOf c Is TextBox Then
                                    Dim t As TextBox = CType(c, TextBox)
                                    If t.ID = "txtZahlungsfristNeu" Then
                                        t.Text = sNF
                                        cmdSave.Enabled = True
                                    End If

                                End If
                            Next
                        End If
                    Next
                Next
            End If
        Else : lblError.Text = "Bitte geben Sie einen numerischen Wert für die neue Fälligkeit ein!"
            cmdSave.Enabled = False
        End If
        txt_NF.Text = ""
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
#End Region

#Region "Methoden"

    Private Sub LoadAuthorizatioData(ByVal AuthorizationID As Int32)
        Dim sneu As String
        Dim sKkber As String
        Dim OutPutStream As System.IO.MemoryStream = GiveAuthorizationDetails(m_App.Connectionstring, AuthorizationID, "objFDDZahlungsfrist")
        Dim formatter As New BinaryFormatter()
        Dim Zahlungsfristen As DataTable
        objChange17_objFDDZahlungsfrist = New FDD_Zahlungsfrist(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        objChange17_objFDDZahlungsfrist = DirectCast(formatter.Deserialize(OutPutStream), FDD_Zahlungsfrist)
        Session("objChange17_objFDDZahlungsfrist") = objChange17_objFDDZahlungsfrist
        Zahlungsfristen = objChange17_objFDDZahlungsfrist.Zahlungsfristen
        Dim i As Integer

        FillGrid()
        For i = 0 To Zahlungsfristen.Rows.Count - 1
            sneu = Zahlungsfristen.Rows(i)(FDD_Zahlungsfrist.COLUMN_ZAHLUNGSFRIST_NEU).ToString()
            sKkber = Zahlungsfristen.Rows(i)(FDD_Zahlungsfrist.COLUMN_KONTINGENTID).ToString()
            If sneu.Length > 0 Then

                Dim item As DataGridItem
                Dim c As System.Web.UI.Control
                For Each item In DataGrid1.Items
                    'Werte ermitteln

                    Dim cell As TableCell
                    cell = item.Cells(3)
                    Dim cell2 As TableCell = item.Cells(0)
                    For Each c In cell.Controls

                        If TypeOf c Is TextBox Then

                            Dim t As TextBox = CType(c, TextBox)

                            'Neue Zahlungsfrist auslesen und in DataRow schreiben
                            If t.ID = "txtZahlungsfristNeu" And sKkber = cell2.Text Then
                                t.Text = sneu
                                t.Enabled = False
                                If sKkber = "0001" Then
                                    chk_temp.Enabled = False
                                    chk_temp.Checked = False
                                End If
                                If sKkber = "0002" Then
                                    chk_endgueltig.Enabled = False
                                    chk_endgueltig.Checked = False
                                End If
                                If sKkber = "0003" Then
                                    chk_Retail.Enabled = False
                                    chk_Retail.Checked = False
                                    chk_Retail.Visible = True
                                End If
                                If sKkber = "0004" Then
                                    chk_dp.Enabled = False
                                    chk_dp.Checked = False
                                End If
                                FocusScript.Visible = False
                                cmdSave.Visible = True
                                cmdSave.Enabled = True
                            End If
                        End If
                    Next
                Next
                If (Not Session("Authorization") Is Nothing) AndAlso (Not Session("ProcessReference") Is Nothing) Then
                    For Each item In DataGrid1.Items
                        'Werte ermitteln
                        Dim cell As TableCell
                        Dim cell2 As TableCell
                        Dim sreference As String = Session("ProcessReference")

                        cell = item.Cells(0)
                        If cell.Text = sreference Then
                            For Each cell2 In item.Cells
                                cell2.Visible = True
                            Next
                        Else
                            For Each cell2 In item.Cells
                                cell2.Visible = False
                            Next
                        End If
                    Next
                    cmdSave.Visible = False
                End If
            Else
                'Dim item As DataGridItem
                'Dim c As System.Web.UI.Control
                'For Each item In DataGrid1.Items
                '    'Werte ermitteln

                '    Dim cell As TableCell
                '    For Each cell In item.Cells
                '        Dim cell2 As TableCell
                '        cell2 = item.Cells(0)
                '        If cell2.text = sKkber Then
                '            cell.Visible = False
                '        End If
                '    Next
                'Next
            End If

        Next
        checkkontingente()
    End Sub

    Private Sub StartLoadData()
        FocusScript.Visible = True

        cmdSave.Visible = True

        If objChange17_objFDDZahlungsfrist.Zahlungsfristen Is Nothing OrElse objChange17_objFDDZahlungsfrist.Zahlungsfristen.Rows.Count = 0 Then
            lblError.Text = "Fehler: Es konnten keine Zahlungsfristendaten ermittelt werden."
            FocusScript.Visible = False
            lblError.CssClass = "TextError"
        Else
            lblError.CssClass = "LabelExtraLarge"
            FillGrid()
        End If
    End Sub

    Private Sub FillGrid()
        DataGrid1.DataSource = objChange17_objFDDZahlungsfrist.Zahlungsfristen

        DataGrid1.DataBind()
    End Sub

    Private Sub checkkontingente()
        Dim iRow As DataRow

        objFDDBank = New BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        objFDDBank.CreditControlArea = "ZDAD"
        objFDDBank.Customer = Session("SelectedDealer").ToString
        objFDDBank.Show()
        Session("objFDDBank") = objFDDBank


        For Each iRow In objFDDBank.Kontingente.Rows
            Dim sTemp As String = iRow("Kreditkontrollbereich")
            Dim bTemp As Boolean = iRow("ZeigeKontingentart")
            Dim m_tblKontingente As New DataTable()

            If sTemp = "0001" Then
                chk_temp.Visible = True
            End If
            If sTemp = "0002" Then
                chk_endgueltig.Visible = True
            End If
            If sTemp = "0003" Then
                chk_Retail.Visible = True

            End If
            If sTemp = "0004" Then
                chk_dp.Visible = True
            End If
        Next
    End Sub
#End Region
End Class

' ************************************************
' $History: Change17Edit.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 23.03.10   Time: 16:34
' Updated in $/CKAG/Applications/appffd/Forms
' Rückgängig: Dynproxy-Zugriff
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 11.03.10   Time: 12:36
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 10.03.10   Time: 15:22
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 15.06.09   Time: 17:08
' Updated in $/CKAG/Applications/appffd/Forms
' ITA 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 13.06.07   Time: 16:19
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 8.06.07    Time: 15:36
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Abgleich Beyond Compare
' 
' *****************  Version 1  *****************
' User: Uha          Date: 3.05.07    Time: 18:05
' Created in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************
