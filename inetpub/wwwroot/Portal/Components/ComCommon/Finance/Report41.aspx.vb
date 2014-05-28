Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report41
    Inherits System.Web.UI.Page

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents GridView1 As System.Web.UI.WebControls.GridView
    Protected WithEvents lnkCreateExcel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents cmddel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblError2 As System.Web.UI.WebControls.Label

    Dim m_report As fin_02


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
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                m_report = New fin_02(m_User, m_App, strFileName)
                Session.Add("objReport", m_report)
                m_report.SessionID = Session.SessionID
                m_report.AppID = CStr(Session("AppID"))
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 1
                doSubmit()
            End If
        Catch ex As Exception

            lblError2.Text = ex.Message
            lblError2.Visible = True

        End Try

    End Sub

    Private Sub doSubmit()

        Try

            m_report.Fill(Session("AppID"), Session.SessionID.ToString)
            If m_report.Status < 0 Then
                lblError.Text = m_report.Message
            Else
                If m_report.ResultTable.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else

                    Session("ResultTable") = m_report.ResultTable
                    FillGrid(0)
                End If
            End If
        Catch ex As Exception

            lblError2.Text = ex.Message
            lblError2.Visible = True

        End Try

    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        Dim ResultTable As DataTable

        ResultTable = CType(Session("ResultTable"), DataTable)

        If Not ResultTable Is Nothing Then

            If ResultTable.Rows.Count = 0 Then
                lblError.Visible = True
                lblError.Text = "Keine Daten zur Anzeige gefunden."
                GridView1.Visible = False
            Else
                lblError.Visible = False
                GridView1.Visible = True
                lnkCreateExcel.Visible = True
                lblNoData.Visible = True

                Dim tmpDataView As New DataView(ResultTable)

                Dim intTempPageIndex As Int32 = intPageIndex
                Dim strTempSort As String = ""
                Dim strDirection As String = ""

                If strSort.Trim(" "c).Length > 0 Then
                    intTempPageIndex = 0
                    strTempSort = strSort.Trim(" "c)
                    If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = strTempSort) Then
                        If ViewState("Direction") Is Nothing Then
                            strDirection = "desc"
                        Else
                            strDirection = ViewState("Direction").ToString
                        End If
                    Else
                        strDirection = "desc"
                    End If

                    If strDirection = "asc" Then
                        strDirection = "desc"
                    Else
                        strDirection = "asc"
                    End If

                    ViewState("Sort") = strTempSort
                    ViewState("Direction") = strDirection
                Else
                    If Not ViewState("Sort") Is Nothing Then
                        strTempSort = ViewState("Sort").ToString
                        If ViewState("Direction") Is Nothing Then
                            strDirection = "asc"
                            ViewState("Direction") = strDirection
                        Else
                            strDirection = ViewState("Direction").ToString
                        End If
                    End If
                End If

                If Not strTempSort.Length = 0 Then
                    tmpDataView.Sort = strTempSort & " " & strDirection
                End If
                lblNoData.Text = "Anzahl: " & tmpDataView.Count


                GridView1.PageIndex = intTempPageIndex

                GridView1.DataSource = tmpDataView
                GridView1.DataBind()
            End If

        Else
            lblError.Text = "Keine Daten zur Anzeige gefunden."
        End If
    End Sub

    Private Sub lnkCreateExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCreateExcel.Click

        Dim tblTranslations As DataTable
        Dim tblTemp As DataTable = CType(Session("ResultTable"), DataTable).Copy
        Dim AppURL As String
        Dim col As DataControlField
        Dim col2 As DataColumn
        Dim bVisibility As Integer
        Dim i As Integer
        Dim sColName As String

        If tblTemp.Columns.Contains("EQUNR") Then
            tblTemp.Columns.Remove("EQUNR")
        End If


        AppURL = Replace(Request.Url.LocalPath, "/Portal", "..")
        tblTranslations = CType(Session(AppURL), DataTable)

        For Each col In GridView1.Columns
            For i = tblTemp.Columns.Count - 1 To 0 Step -1
                bVisibility = 0
                col2 = tblTemp.Columns(i)
                If col2.ColumnName.ToUpper = col.SortExpression.ToUpper Then
                    sColName = TranslateColLbtn(GridView1, tblTranslations, col.HeaderText, bVisibility)
                    If bVisibility = 0 Then
                        tblTemp.Columns.Remove(col2)
                    ElseIf sColName.Length > 0 Then
                        col2.ColumnName = sColName
                    End If
                End If
            Next
            tblTemp.AcceptChanges()
        Next

        Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
        excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Me)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        FillGrid(e.NewPageIndex)
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub
    Protected Sub ddlPageSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPageSize.SelectedIndexChanged
        GridView1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(GridView1.PageIndex)
    End Sub

    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex, e.SortExpression)
    End Sub

    Protected Sub cmddel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmddel.Click
        m_report = Session("objReport")
        m_report.ResultTable = Session("ResultTable")
        Checkgrid()
        If m_report.ResultTable.Select("Loeschflag='X'").Length > 0 Then
            m_report.Change(Session("AppID"), Session.SessionID.ToString)
            If m_report.Status <> 0 Then
                lblError.Text = m_report.Message
            Else
                Dim delRows() As DataRow = m_report.ResultTable.Select("Loeschflag='X'")
                For Each dRows As DataRow In delRows
                    m_report.ResultTable.Rows.Remove(dRows)
                Next
                m_report.ResultTable.AcceptChanges()
                Session("ResultTable") = m_report.ResultTable
                FillGrid(0)
            End If
        Else
            lblError.Text = "Bitte wählen Sie einen Datensatz aus!"
        End If
    End Sub

    Private Sub Checkgrid()
        Dim item As GridViewRow
        Dim chkBox As CheckBox

        Dim label As Label


        For Each item In GridView1.Rows
            chkBox = item.FindControl("chkLoeschbar")
            If chkBox.Checked = True Then
                label = item.FindControl("lblNummerZBII")
                m_report.ResultTable.Select("NummerZBII='" & label.Text & "'")(0)("Loeschflag") = "X"
                m_report.ResultTable.AcceptChanges()
            End If
            Session("ResultTable") = m_report.ResultTable

        Next


    End Sub
#End Region




End Class

' ************************************************
' $History: Report41.aspx.vb $
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 11.03.10   Time: 14:59
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 2.03.10    Time: 14:35
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA: 3509, 3515, 3522
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 10.04.08   Time: 12:52
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance
' 
' *****************  Version 2  *****************
' User: Uha          Date: 12.12.07   Time: 10:20
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Kosmetik im Bereich Finance
' 
' *****************  Version 1  *****************
' User: Uha          Date: 11.12.07   Time: 15:47
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1470 bzw. 1473/1497 ASPX-Seite und Lib hinzugefügt
' 
' ************************************************
