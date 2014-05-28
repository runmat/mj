Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report20
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
    Private m_objTrace As Base.Kernel.Logging.Trace

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents calBisDatum As System.Web.UI.WebControls.Calendar
    Protected WithEvents txtAbDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents calAbDatum As System.Web.UI.WebControls.Calendar
    Protected WithEvents txtBisDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnOpenSelectAb As System.Web.UI.WebControls.Button
    Protected WithEvents btnOpenSelectBis As System.Web.UI.WebControls.Button
    Protected WithEvents optChange As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblFiliale As System.Web.UI.WebControls.Label
    Protected WithEvents ddlFiliale As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        Session("ShowOtherString") = ""
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        NoDealer(Me, m_User)
        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)


            If Not IsPostBack Then
                calAbDatum.SelectedDate = Today
                txtAbDatum.Text = calAbDatum.SelectedDate.ToShortDateString

                calBisDatum.SelectedDate = Today
                txtBisDatum.Text = calBisDatum.SelectedDate.ToShortDateString

                '28.9. Aenderung durch Seidel. Das Property Applications am Customer
                'war ueberfluessig, weil es die Funktion a) schon gab und b) diese 
                'Daten nicht bei jedem User mitgeladen werden muessen. (Werden 
                'bislang nur fuer diesen Report gebraucht.)
                Dim appAssigned As New Admin.ApplicationList(m_User.Customer.CustomerId, m_User.App.Connectionstring)
                appAssigned.GetAssigned()

                Dim dvLinks As DataView = appAssigned.DefaultView '28.9. herausgenommen durch Seidel (kann spaeter weg): m_User.Customer.Applications.DefaultView
                dvLinks.RowFilter = "AppType<>'Report' AND AppInMenu=1" '28.9. Aenderung durch Seidel. Die Application muss noch gefiltert werden.
                dvLinks.Sort = "AppRank"

                Dim tblApplikationen As DataTable
                tblApplikationen = New DataTable("")
                tblApplikationen.Columns.Add("AppID", System.Type.GetType("System.Int32"))
                tblApplikationen.Columns.Add("Applikation", System.Type.GetType("System.String"))

                Dim i As Int32
                For i = 0 To dvLinks.Count - 1
                    Dim rowTemp As DataRow = tblApplikationen.NewRow
                    rowTemp("AppID") = dvLinks.Item(i)("AppID")
                    Dim strTemp As String
                    If Not TypeOf dvLinks.Item(i)("AppComment") Is System.DBNull Then
                        strTemp = CStr(dvLinks.Item(i)("AppComment"))
                    Else
                        strTemp = ""
                    End If
                    If strTemp.Length = 0 Then
                        rowTemp("Applikation") = CStr(dvLinks.Item(i)("AppFriendlyName"))
                    Else
                        rowTemp("Applikation") = CStr(dvLinks.Item(i)("AppFriendlyName")) & " - " & strTemp
                    End If
                    tblApplikationen.Rows.Add(rowTemp)
                Next


                optChange.DataTextField = "Applikation"
                optChange.DataValueField = "AppID"
                optChange.DataSource = tblApplikationen
                optChange.DataBind()

                lblFiliale.Text = m_User.Organization.OrganizationName
                Dim dtOrganizations As New Admin.OrganizationList(m_User.Customer.CustomerId, m_User.App.Connectionstring, m_User.Customer.AccountingArea)
                dtOrganizations.AddAllNone(True, False)
                With ddlFiliale
                    .Items.Clear()
                    Dim dv As DataView = dtOrganizations.DefaultView
                    dv.Sort = "OrganizationName"
                    .DataSource = dv
                    .DataTextField = "OrganizationName"
                    .DataValueField = "OrganizationID"
                    .DataBind()
                    If IsNumeric(m_User.Organization.OrganizationId) Then
                        Dim _li As ListItem = .Items.FindByValue(m_User.Organization.OrganizationId.ToString)
                        If Not (_li Is Nothing) Then
                            _li.Selected = True
                        End If
                    End If
                End With
                ddlFiliale.Visible = m_User.Organization.AllOrganizations OrElse m_User.HighestAdminLevel > Security.AdminLevel.Customer
                lblFiliale.Visible = Not ddlFiliale.Visible
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Session("ShowOtherString") = ""
        Try
            lblError.Text = ""
            If Not IsDate(txtAbDatum.Text) Then
                If Not IsStandardDate(txtAbDatum.Text) Then
                    If Not IsSAPDate(txtAbDatum.Text) Then
                        lblError.Text = "Geben Sie bitte ein gültiges Datum ein!<br>"
                    End If
                End If
            End If
            If Not IsDate(txtBisDatum.Text) Then
                If Not IsStandardDate(txtBisDatum.Text) Then
                    If Not IsSAPDate(txtBisDatum.Text) Then
                        lblError.Text = "Geben Sie bitte ein gültiges Datum ein!<br>"
                    End If
                End If
            End If
            Dim datAb As Date = CDate(txtAbDatum.Text)
            Dim datBis As Date = CDate(txtBisDatum.Text)
            If datAb > datBis Then
                lblError.Text = "Das Enddatum darf nicht vor dem Anfangsdatum liegen!<br>"
            End If
            If lblError.Text.Length > 0 Then
                Exit Sub
            End If

            m_objTrace = New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP)

            Dim intSelectedOrganizationId As Integer = 0
            If Not ddlFiliale.SelectedItem Is Nothing AndAlso IsNumeric(ddlFiliale.SelectedItem.Value) Then
                intSelectedOrganizationId = CInt(ddlFiliale.SelectedItem.Value)
            End If
            Dim tblResult As DataTable
            tblResult = m_objTrace.GetLogOverViewPerSource(optChange.SelectedItem.Value, txtAbDatum.Text, CDate(txtBisDatum.Text).AddDays(1).ToShortDateString, m_User.Customer.CustomerId, intSelectedOrganizationId)

            Session("ResultTable") = tblResult

            If tblResult.Rows.Count = 0 Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            Else

                Dim objExcelExport As New Excel.ExcelExport()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                Try
                    Excel.ExcelExport.WriteExcel(tblResult, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                Catch
                End Try
                Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                Session("ShowOtherString") = optChange.SelectedItem.Text
                Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
            End If
        Catch ex As Exception
            If ex.Message = "NO_DATA" Or ex.Message = "NO_WEB" Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            Else
                lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End If
        End Try
    End Sub

    Private Sub btnOpenSelectAb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectAb.Click
        calAbDatum.Visible = True
    End Sub

    Private Sub btnOpenSelectBis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectBis.Click
        calBisDatum.Visible = True
    End Sub

    Private Sub calAbDatum_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calAbDatum.SelectionChanged
        calAbDatum.Visible = False
        txtAbDatum.Text = calAbDatum.SelectedDate.ToShortDateString
    End Sub

    Private Sub calBisDatum_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBisDatum.SelectionChanged
        calBisDatum.Visible = False
        txtBisDatum.Text = calBisDatum.SelectedDate.ToShortDateString
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report20.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Hartmannu    Date: 9.09.08    Time: 16:19
' Updated in $/CKAG/Applications/appcsc/Forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 2.05.08    Time: 10:37
' Updated in $/CKAG/Applications/appcsc/Forms
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 11:37
' Created in $/CKAG/Applications/appcsc/Forms
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 10:43
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 23.05.07   Time: 10:08
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 4  *****************
' User: Uha          Date: 7.03.07    Time: 9:28
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Forms
' 
' ************************************************
