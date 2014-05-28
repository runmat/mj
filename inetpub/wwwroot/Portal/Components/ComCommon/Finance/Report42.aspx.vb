Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report42
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

    Dim m_report As fin_03


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
            m_report = New fin_03(m_User, m_App, strFileName)
            Session.Add("objReport", m_report)
            m_report.SessionID = Me.Session.SessionID
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
        ElseIf Not Session("objReport") Is Nothing Then
            m_report = CType(Session("objReport"), fin_03)
        End If


    End Sub

    Private Sub doSubmit()
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
        excelFactory.CreateDocumentAndSendAsResponse(strFileName, Table, Me.Page)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub GridView1_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridView1.RowCancelingEdit

    End Sub

    Private Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        FillGrid(e.NewPageIndex)
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridView1.SelectedIndexChanged

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

                m_report.EQUNR = lblEQUNR.Text
                m_report.Memo = txtMemo.Text
                m_report.SaveMemo(Session("AppID"), Session.SessionID.ToString)

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
                If m_report.E_MESSAGE <> "" Then
                    lblError.Text = m_report.E_MESSAGE & " Daten konnten nicht gespeichert werden!"
                Else
                    lblMemo.Text = m_report.Memo
                    lblMemo.ToolTip = m_User.UserName & " " & Now.ToShortDateString
                    m_report.ResultTable.Select("EQUNR = '" & m_report.EQUNR & "'")(0)("Memo") = m_report.Memo
                    m_report.ResultTable.Select("EQUNR = '" & m_report.EQUNR & "'")(0)("User") = m_User.UserName & " " & Now.ToShortDateString
                End If

            Case "Del"
                Dim index As Integer = CType(e.CommandArgument, Integer)

                Dim lblEQUNR As Label

                lblEQUNR = CType(GridView1.Rows(index).FindControl("lblEQUNR"), Label)

                Dim ibt As ImageButton
                Dim txtMemo As TextBox
                txtMemo = CType(GridView1.Rows(index).FindControl("txtMemo"), TextBox)
                m_report.EQUNR = lblEQUNR.Text
                m_report.Memo = txtMemo.Text
                m_report.SaveMemo(Session("AppID"), Session.SessionID.ToString, True)
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
                m_report.ResultTable.Select("EQUNR = '" & m_report.EQUNR & "'")(0)("Memo") = ""
                m_report.ResultTable.Select("EQUNR = '" & m_report.EQUNR & "'")(0)("User") = ""
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

    Protected Sub ddlPageSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPageSize.SelectedIndexChanged
        GridView1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(GridView1.PageIndex)
    End Sub

#End Region


End Class

' ************************************************
' $History: Report42.aspx.vb $
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 2.03.10    Time: 14:35
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA: 3509, 3515, 3522
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 23.02.10   Time: 9:56
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA: 3509
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 19.02.10   Time: 16:04
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 6  *****************
' User: Dittbernerc  Date: 18.06.09   Time: 17:18
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 - .net Connector umstellen
' 
' Bapis:
' Z_M_Brief_3mal_Gemahnt
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
' *****************  Version 3  *****************
' User: Jungj        Date: 18.02.08   Time: 13:47
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' akf änderungen
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
