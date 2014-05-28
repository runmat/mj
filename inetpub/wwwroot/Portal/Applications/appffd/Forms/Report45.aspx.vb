Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business


Public Class Report45

    Inherits System.Web.UI.Page
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Fahrgestellnummer As System.Web.UI.WebControls.Label
    Protected WithEvents txtFahrgestellnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_Kennzeichen As System.Web.UI.WebControls.Label
    Protected WithEvents txtKennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_Ordernummer As System.Web.UI.WebControls.Label
    Protected WithEvents txtOrdernummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents tableMain As System.Web.UI.HtmlControls.HtmlTable

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Dim m_report As MDR_07
   

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



#Region " Construktor "
    Public Sub New()

    End Sub
#End Region

#Region "Methoden"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            m_User = GetUser(Me)
            FormAuth(Me, m_User)
            m_App = New Base.Kernel.Security.App(m_User)
            ucHeader.InitUser(m_User)
            GetAppIDFromQueryString(Me)

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            m_report = New MDR_07(m_User, m_App, "")
            m_report.SessionID = Me.Session.SessionID
            m_report.AppID = CStr(Session("AppID"))
        Catch ex As Exception
            lblError.Text = "Fehler beim laden der Seite: " & ex.Message.ToString
        End Try
    End Sub
#End Region


    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click

        If txtFahrgestellnummer.Text = "WF0_XX" Then
            txtFahrgestellnummer.Text = ""
        Else

            txtFahrgestellnummer.Text = generateFullChassisNum(txtFahrgestellnummer.Text)
        End If
        m_report.fahrgestellnummer = txtFahrgestellnummer.Text
        m_report.Ordernummer = txtOrdernummer.Text
        m_report.kennzeichen = txtKennzeichen.Text

        txtFahrgestellnummer.Text = "WF0_XX"

        m_report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)


        If Not m_report.Result Is Nothing Then
            DataGrid1.DataSource = m_report.Result
            DataGrid1.DataBind()
        End If


        lblError.Text = m_report.Message

        If Not lblError.Text.Equals("") = True And Not lblError.Text.Equals(" ") = True Then
            DataGrid1.DataSource = Nothing
            DataGrid1.DataBind()
        End If


    End Sub

    Private Function generateFullChassisNum(ByVal FGSNR As String) As String
        'Durch der Platzhalter _ soll durch die 10. Stelle einer kompletten Fahrgestelltnumemr ersetzt werden. JJ2007.11.28

        If FGSNR.Trim(" "c).Length = 17 AndAlso Not FGSNR.IndexOf("_") = -1 AndAlso FGSNR.IndexOf("_") = 3 Then
            FGSNR = FGSNR.Replace("_", FGSNR.Chars(9))
        End If

        Return FGSNR

    End Function


    Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        Dim m_objTable As New DataTable()
        m_objTable = m_report.Result

        Dim intItem As Int32

        For intItem = 0 To m_objTable.Columns.Count - 1
            If m_objTable.Columns(intItem).DataType Is System.Type.GetType("System.DateTime") Then
                If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.Cells(intItem).Text = DataBinder.Eval(e.Item.DataItem, m_objTable.Columns(intItem).ColumnName, "{0:dd.MM.yyyy}")
                End If
            End If
        Next
    End Sub

End Class
