Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business


Public Class Report21
    Inherits System.Web.UI.Page
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents ddl_Hersteller As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label


    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Dim m_report As ec_21

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



#Region " Konstruktor "
    Public Sub New()

    End Sub
#End Region

#Region " methoden "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            m_User = GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte
            FormAuth(Me, m_User)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            ucHeader.InitUser(m_User)
            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            If Page.IsPostBack = False Then
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & "Fahrzeugbriefbestand.xls"
                m_report = New ec_21(m_User, m_App, strFileName)
                Session.Add("objReport", m_report)
                m_report.SessionID = Me.Session.SessionID
                m_report.AppID = CStr(Session("AppID"))
                'doSubmit()
                fuelleDDLHersteller()

            End If

        Catch ex As Exception
            lblError.Text = "Fehler beim laden der Seite: " & ex.Message.ToString
        End Try
    End Sub


    Private Sub fuelleDDLHersteller()
        Dim tmpRow As DataRow
        Dim item As ListItem
        m_report.fillHersteller(Session("AppID").ToString, Session.SessionID, Me)
        m_report.GetAllData(Session("AppID").ToString, Session.SessionID, Me)


        Dim dt As DataTable = m_report.Gesamtdaten
        Dim BooFound As Boolean = False

        For Each hRow As DataRow In m_report.SAPHerstellertabelle.Rows

            BooFound = False

            For Each dr As DataRow In m_report.Gesamtdaten.Rows


                If hRow("HERST_T") = dr("HERST_T") Then
                    BooFound = True
                    Exit For
                End If

            Next

            If BooFound = False Then hRow.Delete()


        Next
        m_report.SAPHerstellertabelle.AcceptChanges()

        dt.Clear()
        dt = Nothing




        tmpRow = m_report.SAPHerstellertabelle.NewRow


        item = New ListItem("", "")
        ddl_Hersteller.Items.Add(item)

        For Each tmpRow In m_report.SAPHerstellertabelle.Rows
            item = New ListItem(tmpRow(1), tmpRow(1))
            ddl_Hersteller.Items.Add(item)
        Next

    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        If ddl_Hersteller.SelectedItem.Value = "" Then
            lblError.Text = "Wählen Sie einen Hersteller aus"
        Else
            doSubmit()
        End If
    End Sub

    Private Sub doSubmit()
        If m_report Is Nothing Then
            m_report = CType(Session("objReport"), ec_21)
        End If

        m_report.HerstellerBezeichnung = ddl_Hersteller.SelectedItem.Value
        m_report.Fill(Session("AppID").ToString, Session.SessionID, Me)

        Session("ResultTable") = m_report.Result

        If Not m_report.Status = 0 Then
            lblError.Text = "Fehler: " & m_report.Message
        Else
            If m_report.Result.Rows.Count = 0 Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            Else

                Try
                    Excel.ExcelExport.WriteExcel(m_report.Result, ConfigurationManager.AppSettings("ExcelPath") & m_report.FileName)
                Catch
                End Try

                Dim AbsoluterPfadZumVirituellenVerz As String = ConfigurationManager.AppSettings("ReplaceExcelPath")
                Session("lnkExcel") = ConfigurationManager.AppSettings("ExcelPath").Replace(AbsoluterPfadZumVirituellenVerz, "") & m_report.FileName & "".Replace("/", "\")
                Response.Redirect("/PortalORM/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
            End If
        End If

    End Sub
#End Region




End Class
