Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report07
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblTitel As System.Web.UI.WebControls.Label
    Protected WithEvents lstHaendler As System.Web.UI.WebControls.ListBox
    Protected WithEvents cbxAlle As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents calVon As System.Web.UI.WebControls.Calendar
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txtStichtag As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnVon As System.Web.UI.WebControls.LinkButton
    Private objPorsche As Porsche_01

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
                Initialload()
            Else

            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub Refill()
        Dim vwHaendler As DataView
        objPorsche = CType(Session("objSuche"), Porsche_01)

        vwHaendler = objPorsche.PResult.DefaultView
        vwHaendler.Sort = "NAME1 asc"

        With lstHaendler
            .DataSource = vwHaendler
            .DataValueField = "KUNNR_ZF"
            .DataTextField = "Addresse"
            .DataBind()
        End With

        lstHaendler.Visible = True

        lstHaendler.Items.FindByValue(objPorsche.PSelection).Selected = True 'Eintrag auswählen
    End Sub

    Private Sub Initialload()
        Dim vwHaendler As DataView

        objPorsche = New Porsche_01(m_User, m_App, "")

        With objPorsche
            .PGueltigkeit = Now.ToShortDateString
            .PHierarchie = "A"
            .PKnotenlevel = "00"
            .PKundennr = Right("0000000000" & m_User.KUNNR, 10)
        End With

        objPorsche.getHaendler(m_User.Reference, Session("AppID").ToString, Session.SessionID, Me)

        If (objPorsche.Status <> 0) Then
            lblError.Text = objPorsche.Message
            lstHaendler.Visible = False
            lblTitel.Visible = False
            cmdCreate.Enabled = False
            Exit Sub
        End If

        If objPorsche.Result.Rows.Count = 0 Then
            lblError.Text = "Keine Händler gefunden."
            lstHaendler.Visible = False
            lblTitel.Visible = False
            cmdCreate.Enabled = False
            Exit Sub
        End If

        lstHaendler.Visible = True

        vwHaendler = objPorsche.Result.DefaultView
        vwHaendler.Sort = "NAME1 asc"

        With lstHaendler
            .DataSource = vwHaendler
            .DataValueField = "KUNNR_ZF"
            .DataTextField = "Addresse"
            .DataBind()
        End With

        lstHaendler.Items(0).Selected = True                        'Ersten Eintrag auswählen.

        objPorsche.PSelection = lstHaendler.SelectedItem.Value      '...und merken

        Session.Add("objSuche", objPorsche)

        txtStichtag.Text = Format(Now, "dd.MM.yyyy")
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        objPorsche = CType(Session("objSuche"), Porsche_01)
        objPorsche.PSelection = lstHaendler.SelectedItem.Value
        objPorsche.PKundennr = CType(lstHaendler.SelectedItem.Value, String)
        objPorsche.PHaendler = m_User.Reference     'Händlernummer
        objPorsche.Stichtag = txtStichtag.Text       'Stichtag

        If (cbxAlle.Checked = True) Then
            objPorsche.PKundennr = String.Empty
        End If

        objPorsche.getEquiStichtag(Session("AppID").ToString, Session.SessionID, Me)

        If (objPorsche.Status <> 0) Then
            lblError.Text = objPorsche.Message  'Fehler oder keine Daten vorhanden...
            Refill()
        Else
            If objPorsche.Result.Rows.Count = 0 Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            Else
                'Ok, daten vorhanden...
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                Dim objExcelExport As New Excel.ExcelExport()

                Session("ResultTable") = objPorsche.Result

                Try
                    Excel.ExcelExport.WriteExcel(objPorsche.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                Catch
                End Try

                Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                logApp.UpdateEntry("APP", Session("AppID").ToString, "Report: Händlerberstand")
                Response.Redirect("Report07_2.aspx?AppID=" & Session("AppID").ToString)
            End If
        End If
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        If IsDate(txtStichtag.Text) Then
            DoSubmit()
        Else
            lblError.Text = "Bitte geben Sie einen Stichtag ein!"
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub btnVon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVon.Click
        calVon.Visible = True
    End Sub

    Private Sub calVon_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        txtStichtag.Text = calVon.SelectedDate
    End Sub

    Private Sub lstHaendler_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstHaendler.SelectedIndexChanged

    End Sub
End Class
