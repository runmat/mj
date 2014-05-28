Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements


Public Class Report01
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

    Dim mObjFristablauf As Fristablauf


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

        m_User = GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte
        FormAuth(Me, m_User)
        m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
        ucHeader.InitUser(m_User)
        GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        If Page.IsPostBack = False Then
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            mObjFristablauf = New Fristablauf(m_User, m_App, strFileName)
            Session.Add("objReport", mObjFristablauf)
            mObjFristablauf.SessionID = Session.SessionID
            mObjFristablauf.AppID = CStr(Session("AppID"))
            ddlPageSize.Items.Add("10")
            ddlPageSize.Items.Add("20")
            ddlPageSize.Items.Add("50")
            ddlPageSize.Items.Add("100")
            ddlPageSize.Items.Add("200")
            ddlPageSize.Items.Add("500")
            ddlPageSize.Items.Add("1000")
            ddlPageSize.SelectedIndex = 1
            doSubmit()
        ElseIf Not Session("objReport") Is Nothing Then
            mObjFristablauf = CType(Session("objReport"), Fristablauf)
        End If


    End Sub

    Private Sub doSubmit()
        mObjFristablauf.Fill(Session("AppID").ToString, Session.SessionID.ToString)
        If mObjFristablauf.Status < 0 Then
            lblError.Text = mObjFristablauf.Message
        Else
            If mObjFristablauf.ResultTable.Rows.Count = 0 Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            Else

                Session("ResultTable") = mObjFristablauf.ResultTable
                FillGrid(0)
            End If
        End If
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
                lblNoData.Visible = False

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

                GridView1.PageIndex = intTempPageIndex

                GridView1.DataSource = tmpDataView
                GridView1.DataBind()
            End If

        Else
            lblError.Text = "Keine Daten zur Anzeige gefunden."
        End If
    End Sub

    Private Sub lnkCreateExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCreateExcel.Click

        Dim Table As DataTable = CType(Session("ResultTable"), DataTable).Copy

        If Table.Columns.Contains("EQUNR") Then
            Table.Columns.Remove("EQUNR")
        End If

        Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
        excelFactory.CreateDocumentAndSendAsResponse(strFileName, Table, Me)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub GridView1_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridView1.RowCancelingEdit

    End Sub
    
    Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Select Case e.CommandName
            Case "Show"
                Dim index As Integer = CType(e.CommandArgument, Integer)
                Dim lblEQUNR As Label
                lblEQUNR = CType(GridView1.Rows(index).FindControl("lblEQUNR"), Label)
                Dim ibt As ImageButton
                Dim txtMemo As TextBox
                txtMemo = CType(GridView1.Rows(index).FindControl("txtMemo"), TextBox)
                txtMemo.Visible = True
                ibt = CType(GridView1.Rows(index).FindControl("ibtnSve"), ImageButton)
                ibt.Visible = True
                ibt = CType(GridView1.Rows(index).FindControl("ibtnEdit"), ImageButton)
                ibt.Visible = False
                ibt = CType(GridView1.Rows(index).FindControl("ibtnDel"), ImageButton)
                ibt.Visible = False
                ibt = CType(GridView1.Rows(index).FindControl("ibtnCancel"), ImageButton)
                ibt.Visible = True
                Dim lblMemo As Label
                lblMemo = CType(GridView1.Rows(index).FindControl("lblMemo"), Label)
                lblMemo.Visible = False

            Case "Save"
                Dim index As Integer = CType(e.CommandArgument, Integer)
                Dim lblEQUNR As Label
                lblEQUNR = CType(GridView1.Rows(index).FindControl("lblEQUNR"), Label)

                Dim ibt As ImageButton
                Dim txtMemo As TextBox

                ibt = CType(GridView1.Rows(index).FindControl("ibtnSve"), ImageButton)
                ibt.Visible = False
                ibt = CType(GridView1.Rows(index).FindControl("ibtnDel"), ImageButton)
                ibt.Visible = True

                txtMemo = CType(GridView1.Rows(index).FindControl("txtMemo"), TextBox)

                mObjFristablauf.EQUNR = lblEQUNR.Text
                mObjFristablauf.Memo = txtMemo.Text
                mObjFristablauf.SaveMemo(Session("AppID").ToString, Session.SessionID.ToString)

                If mObjFristablauf.E_MESSAGE <> "" Then
                    lblError.Text = mObjFristablauf.E_MESSAGE & " Daten nicht übernommen!"
                End If
                txtMemo.Visible = False
                ibt = CType(GridView1.Rows(index).FindControl("ibtnEdit"), ImageButton)
                ibt.Visible = True
                ibt = CType(GridView1.Rows(index).FindControl("ibtnDel"), ImageButton)
                ibt.Visible = True
                ibt = CType(GridView1.Rows(index).FindControl("ibtnCancel"), ImageButton)
                ibt.Visible = False
                Dim lblMemo As Label
                lblMemo = CType(GridView1.Rows(index).FindControl("lblMemo"), Label)
                lblMemo.Visible = True
                If mObjFristablauf.E_MESSAGE <> "" Then
                    lblError.Text = mObjFristablauf.E_MESSAGE & " Daten konnten nicht gelöscht werden!"
                Else
                    lblMemo.Text = mObjFristablauf.Memo
                    lblMemo.ToolTip = m_User.UserName & " " & Now.ToShortDateString
                    mObjFristablauf.ResultTable.Select("EQUNR = '" & mObjFristablauf.EQUNR & "'")(0)("Memo") = mObjFristablauf.Memo
                    mObjFristablauf.ResultTable.Select("EQUNR = '" & mObjFristablauf.EQUNR & "'")(0)("User") = m_User.UserName & " " & Now.ToShortDateString
                End If

            Case "Del"
                Dim index As Integer = CType(e.CommandArgument, Integer)

                Dim lblEQUNR As Label

                lblEQUNR = CType(GridView1.Rows(index).FindControl("lblEQUNR"), Label)

                Dim ibt As ImageButton
                Dim txtMemo As TextBox
                txtMemo = CType(GridView1.Rows(index).FindControl("txtMemo"), TextBox)
                mObjFristablauf.EQUNR = lblEQUNR.Text
                mObjFristablauf.Memo = txtMemo.Text
                mObjFristablauf.SaveMemo(Session("AppID").ToString, Session.SessionID.ToString, True)
                txtMemo.Visible = False
                ibt = CType(GridView1.Rows(index).FindControl("ibtnSve"), ImageButton)
                ibt.Visible = False
                ibt = CType(GridView1.Rows(index).FindControl("ibtnEdit"), ImageButton)
                ibt.Visible = True
                ibt = CType(GridView1.Rows(index).FindControl("ibtnDel"), ImageButton)
                ibt.Visible = True
                Dim lblMemo As Label
                lblMemo = CType(GridView1.Rows(index).FindControl("lblMemo"), Label)
                lblMemo.Visible = True
                lblMemo.Text = ""
                mObjFristablauf.ResultTable.Select("EQUNR = '" & mObjFristablauf.EQUNR & "'")(0)("Memo") = ""
                mObjFristablauf.ResultTable.Select("EQUNR = '" & mObjFristablauf.EQUNR & "'")(0)("User") = ""
            Case "Cancel"
                Dim index As Integer = CType(e.CommandArgument, Integer)

                Dim lblEQUNR As Label

                lblEQUNR = CType(GridView1.Rows(index).FindControl("lblEQUNR"), Label)

                Dim ibt As ImageButton
                Dim txtMemo As TextBox
                txtMemo = CType(GridView1.Rows(index).FindControl("txtMemo"), TextBox)
                txtMemo.Visible = False
                ibt = CType(GridView1.Rows(index).FindControl("ibtnSve"), ImageButton)
                ibt.Visible = False
                ibt = CType(GridView1.Rows(index).FindControl("ibtnEdit"), ImageButton)
                ibt.Visible = True
                ibt = CType(GridView1.Rows(index).FindControl("ibtnDel"), ImageButton)
                ibt.Visible = True
                ibt = CType(GridView1.Rows(index).FindControl("ibtnCancel"), ImageButton)
                ibt.Visible = False
                Dim lblMemo As Label
                lblMemo = CType(GridView1.Rows(index).FindControl("lblMemo"), Label)
                lblMemo.Visible = True

        End Select
    End Sub
    
    Private Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        FillGrid(e.NewPageIndex)
    End Sub

    Protected Sub ddlPageSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPageSize.SelectedIndexChanged
        GridView1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(GridView1.PageIndex)
    End Sub

    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex, e.SortExpression)
    End Sub

#End Region

End Class

' ************************************************
' $History: Report01.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 2.03.10    Time: 14:19
' Updated in $/CKAG/Applications/appakf/Forms
' ita. 3509
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 26.02.10   Time: 13:57
' Updated in $/CKAG/Applications/appakf/Forms
' 
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 28.04.09   Time: 16:36
' Updated in $/CKAG/Applications/appakf/Forms
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 13.11.08   Time: 15:47
' Updated in $/CKAG/Applications/appakf/Forms
' ITA 2384 nachbesserung
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 13.11.08   Time: 15:41
' Updated in $/CKAG/Applications/appakf/Forms
' Umstellung auf Framework 3.5
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 13.11.08   Time: 15:29
' Created in $/CKAG/Applications/appakf/Forms
' ITA 2384
' ************************************************
