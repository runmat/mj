Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security

Public Class Report01
    Inherits System.Web.UI.Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents calVon As System.Web.UI.WebControls.Calendar
    Protected WithEvents calBis As System.Web.UI.WebControls.Calendar
    Protected WithEvents btnCal1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents btnCal2 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents txtKunnr As System.Web.UI.WebControls.TextBox
    Protected WithEvents ucStyles As Styles
    Protected WithEvents txtMeldedatumVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtMeldedatumBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents ucHeader As Header

    Private m_User As User
    Protected WithEvents txtVKORG As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVKBUR As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddStatus As System.Web.UI.WebControls.DropDownList
    Private m_App As App



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
            m_App = New CKG.Base.Kernel.Security.App(m_User)
            If IsPostBack = False Then
                FillDropdowns()
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Function checkInput() As Boolean
        Dim datevon As Date
        Dim datebis As Date

        Try
            datevon = CType(txtMeldedatumVon.Text, Date)
            datebis = CType(txtMeldedatumBis.Text, Date)
        Catch ex As Exception
            Return False
        End Try

        If (datevon > datebis) Then
            Return False
        End If
        Return True
    End Function

    Private Sub DoSubmit()
        Session("lnkExcel") = ""

        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New QMBase(m_User, m_App, strFileName)
            Dim datMeldedatumVon As DateTime
            Dim datMeldedatumBis As DateTime
            Dim datKunnr As String
            Dim strVKORG As String
            Dim strVKBUR As String
            Dim strStatus As String = ""

            datKunnr = Me.txtKunnr.Text
            strVKORG = Me.txtVKORG.Text
            strVKBUR = Me.txtVKBUR.Text

            If ddStatus.SelectedItem.Value <> "00" Then
                strStatus = ddStatus.SelectedItem.Text
            End If


            If Not (txtMeldedatumVon.Text = String.Empty) Then
                If Not IsDate(txtMeldedatumVon.Text) Then
                    If Not IsStandardDate(txtMeldedatumVon.Text) Then
                        If Not IsSAPDate(txtMeldedatumVon.Text) Then
                            lblError.Text = "Geben Sie bitte ein gültiges ""Meldedatum von"" ein!<br>"
                        Else
                            datMeldedatumVon = CDate(txtMeldedatumVon.Text)
                        End If
                    Else
                        datMeldedatumVon = CDate(txtMeldedatumVon.Text)
                    End If
                Else
                    datMeldedatumVon = CDate(txtMeldedatumVon.Text)
                End If
            End If

            If Not (txtMeldedatumBis.Text = String.Empty) Then
                If Not IsDate(txtMeldedatumBis.Text) Then
                    If Not IsStandardDate(txtMeldedatumBis.Text) Then
                        If Not IsSAPDate(txtMeldedatumBis.Text) Then
                            lblError.Text &= "Geben Sie bitte ein gültiges ""Meldedatum bis"" ein!<br>"
                        Else
                            datMeldedatumBis = CDate(txtMeldedatumBis.Text)
                        End If
                    Else
                        datMeldedatumBis = CDate(txtMeldedatumBis.Text)
                    End If
                Else
                    datMeldedatumBis = CDate(txtMeldedatumBis.Text)
                End If
            End If

            If lblError.Text.Length = 0 Then
                Dim strVon As String
                Dim strBis As String

                If IsDate(datMeldedatumVon) = False OrElse datMeldedatumVon = "#12:00:00 AM#" Then
                    strVon = String.Empty
                Else
                    strVon = CStr(Format(datMeldedatumVon, "dd.MM.yyyy"))
                End If

                If IsDate(datMeldedatumBis) = False OrElse datMeldedatumBis = "#12:00:00 AM#" Then
                    strBis = String.Empty
                Else
                    strBis = CStr(Format(datMeldedatumBis, "dd.MM.yyyy"))
                End If



                m_Report.FILL_QM_Auswertung(Session("AppID").ToString, Session.SessionID.ToString, strVon, strBis, datKunnr, _
                                            strVKORG, strVKBUR, strStatus)

                Session("ResultTable") = m_Report.Result

                If Not m_Report.Status = 0 Then
                    lblError.Text = "Fehler: " & m_Report.Message
                Else
                    If m_Report.Result.Rows.Count = 0 Then
                        lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                    Else
                        Try
                            Excel.ExcelExport.WriteExcel(m_Report.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                        Catch
                        End Try
                        Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                        Response.Redirect("Report01_2.aspx?AppID=" & Session("AppID").ToString)
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub FillDropdowns()

        Dim ds As New DataSet()
        Dim QmGetData As New QMBase(m_User, m_App, String.Empty)
        Dim dr As DataRow

        'DataSet für die Dropdowns füllen
        ds = QmGetData.getTablesForDropdowns

        'Dropdown Status füllen

        dr = ds.Tables("dtStatus").NewRow()

        With ddStatus
            .Items.Add(New ListItem("Bitte auswählen", "00"))

            For Each dr In ds.Tables("dtStatus").Rows
                .Items.Add(New ListItem(dr.Item("Statustext"), dr.Item("Statusnr")))
            Next

        End With


    End Sub


    Private Sub btnCal1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCal1.Click
        calVon.Visible = True
        calBis.Visible = False
    End Sub

    Private Sub btnCal2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCal2.Click
        calVon.Visible = False
        calBis.Visible = True
    End Sub

    Private Sub calVon_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        txtMeldedatumVon.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    Private Sub calBis_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBis.SelectionChanged
        txtMeldedatumBis.Text = calBis.SelectedDate.ToShortDateString
        calBis.Visible = False
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub


    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        DoSubmit()
    End Sub
End Class
