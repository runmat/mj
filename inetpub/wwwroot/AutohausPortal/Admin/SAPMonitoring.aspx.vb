Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel

Public Class SAPMonitoring
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

    Private m_User As User
    Private m_App As App

    Private m_blnShowDetails() As Boolean

#Region "Events"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        AdminAuth(Me, m_User, AdminLevel.Organization)

        GetAppIDFromQueryString(Me)
        GridNavigation1.setGridElment(DataGrid1)

        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                FillForm()

                txtAbDatum_CalendarExtender.SelectedDate = Today

                txtBisDatum_CalendarExtender.SelectedDate = Today
            End If

            ReDim m_blnShowDetails(DataGrid1.PageSize)
            Dim i As Int32
            For i = 0 To DataGrid1.PageSize - 1
                m_blnShowDetails(i) = False
            Next
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "SAPMonitoring", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
            lblError.Visible = True
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbCreate.Click
        lblError.Text = ""
        trdata1.Visible = False
        trdata2.Visible = False
        Resultshow.Visible = True

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

        'Wenn der Datentyp nicht stimmt, sollte hier ausgestiegen werden
        If lblError.Text <> String.Empty Then Exit Sub


        Dim datAb As Date = CDate(txtAbDatum.Text)
        Dim datBis As Date = CDate(txtBisDatum.Text)
        If datAb > datBis Then
            lblError.Text = "Das Enddatum darf nicht vor dem Anfangsdatum liegen!<br>"
        End If
        If lblError.Text.Length > 0 Then
            Exit Sub
        End If

        If rbBAPI.Checked Then
            Me.ViewState("mySort") = ""
            FillDataGrid1(True, 0)
            trdata1.Visible = True
        Else
            Me.ViewState("mySort") = "[ASPX_ID]"
            FillDataGrid2(True, 0)
            trdata2.Visible = True
        End If

    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillDataGrid1(False, DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillDataGrid1(False, e.NewPageIndex)
    End Sub

    Private Sub rbBAPI_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbBAPI.CheckedChanged
        If rbBAPI.Checked Then
            FillForm()
        End If
    End Sub

    Private Sub rbASPX_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbASPX.CheckedChanged
        If rbASPX.Checked Then
            FillForm()
        End If
    End Sub

    Private Sub HGZ_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles HGZ.SortCommand
        FillDataGrid2(False, HGZ.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub HGZ_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles HGZ.PageIndexChanged
        FillDataGrid2(False, e.NewPageIndex)
    End Sub

    Private Sub HGZ_TemplateSelection(ByVal sender As Object, ByVal e As DBauer.Web.UI.WebControls.HierarGridTemplateSelectionEventArgs) Handles HGZ.TemplateSelection
        Select Case (e.Row.Table.TableName)
            Case "BAPI"
                e.TemplateFilename = "Templates\\BAPIData.ascx"
            Case Else
                Throw New NotImplementedException("Unexpected child row in TemplateSelection event")
        End Select
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If Me.trdata2.Visible Then
            Dim item As DataGridItem
            Dim cell As TableCell
            Dim control As Control
            Dim image As System.Web.UI.WebControls.Image
            For Each item In HGZ.Items
                For Each cell In item.Cells
                    For Each control In cell.Controls
                        If TypeOf control Is System.Web.UI.WebControls.Image Then
                            image = CType(control, System.Web.UI.WebControls.Image)
                            image.Width = 16
                            image.Height = 16
                            If InStr(image.ImageUrl, "plus.gif") > 0 Then
                                image.ImageUrl = "/Services/Images/plus.gif"
                            End If
                            If InStr(image.ImageUrl, "minus.gif") > 0 Then
                                image.ImageUrl = "/Services/Images/minus.gif"
                            End If
                        End If
                    Next
                Next
            Next
        End If
    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        Dim reportExcel As New DataTable()
        If TypeOf Session("SAPMonitorResult") Is DataTable Then
            reportExcel = CType(Session("SAPMonitorResult"), DataTable)
            If reportExcel.Rows.Count > 0 Then

                Try
                    Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
                    Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
                    excelFactory.CreateDocumentAndSendAsResponse(strFileName, reportExcel, Me)
                Catch ex As Exception
                    lblError.Text = ex.Message
                End Try

            End If
        End If
    End Sub

#End Region

#Region "Methods"

    Private Sub FillDataGrid2(ByVal blnForceNew As Boolean, ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        lblError.Text = "Keine Daten vorhanden. Dieses Logging ist veraltet."
    End Sub

    Private Sub FillDataGrid1(ByVal blnForceNew As Boolean, ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        lblError.Text = "Keine Daten vorhanden. Dieses Logging ist veraltet."
    End Sub

    Private Sub FillForm()
        lblError.Text = ""
        trdata1.Visible = False
        trdata2.Visible = False
        Resultshow.Visible = False

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        If rbBAPI.Checked Then
            FillBAPI(cn)
            ExcelDiv.Visible = True
        Else
            FillASPX(cn)
            ExcelDiv.Visible = False
        End If
    End Sub

    Private Sub FillASPX(ByVal cn As SqlClient.SqlConnection)

    End Sub

    Private Sub FillBAPI(ByVal cn As SqlClient.SqlConnection)

    End Sub

#End Region

End Class
