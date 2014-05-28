
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Performance2
    Inherits System.Web.UI.Page

    Private m_User As User
    Private m_App As App
    Private m_intPerformanceCounterID As Int32
    Private intValuesPerDateEntry As Int32

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

#Region " Declarations"
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblMin As System.Web.UI.WebControls.Label
    Protected WithEvents lblMax As System.Web.UI.WebControls.Label
    Protected WithEvents lblCategoryName As System.Web.UI.WebControls.Label
    Protected WithEvents lblCounterName As System.Web.UI.WebControls.Label
    Protected WithEvents lblInstanceName As System.Web.UI.WebControls.Label
    Protected WithEvents lblValue As System.Web.UI.WebControls.Label
    Protected WithEvents lblCounterUnit As System.Web.UI.WebControls.Label
    Protected WithEvents Repeater1 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Protected WithEvents Label16 As System.Web.UI.WebControls.Label
    Protected WithEvents Label17 As System.Web.UI.WebControls.Label
    Protected WithEvents Label18 As System.Web.UI.WebControls.Label
    Protected WithEvents Label19 As System.Web.UI.WebControls.Label
    Protected WithEvents Label20 As System.Web.UI.WebControls.Label
    Protected WithEvents Label21 As System.Web.UI.WebControls.Label
    Protected WithEvents Label22 As System.Web.UI.WebControls.Label
    Protected WithEvents Label23 As System.Web.UI.WebControls.Label
    Protected WithEvents Label24 As System.Web.UI.WebControls.Label
    Protected WithEvents Repeater2 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater3 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater4 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater5 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater6 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater7 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater8 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater9 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater10 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater11 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater12 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater13 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater14 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater15 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater16 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater17 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater18 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater19 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater20 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater21 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater22 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater23 As System.Web.UI.WebControls.Repeater
    Protected WithEvents Repeater24 As System.Web.UI.WebControls.Repeater
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
#End Region

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
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        ucStyles.TitleText = "Allgemeine Leistungsangaben - Details"
        AdminAuth(Me, m_User, AdminLevel.Organization)

        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""
            End If

            If Request.QueryString("Return") Is Nothing Then
                FillData()
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Performance2", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
            lblError.Visible = True
        End Try
    End Sub

    Private Sub FillData()
        If ((Session("PerformanceCounterID") Is Nothing) OrElse (Not IsNumeric(Session("PerformanceCounterID")))) And ((Request.QueryString("PerformanceCounterID") Is Nothing) OrElse (Not IsNumeric(Request.QueryString("PerformanceCounterID")))) Then
            Try
                Response.Redirect("Performance.aspx")
            Catch
            End Try
        Else
            If (Not Request.QueryString("PerformanceCounterID") Is Nothing) AndAlso (IsNumeric(Request.QueryString("PerformanceCounterID"))) Then
                m_intPerformanceCounterID = CInt(Request.QueryString("PerformanceCounterID"))
            Else
                m_intPerformanceCounterID = CInt(Session("PerformanceCounterID"))
            End If
        End If

        Dim objTrace1 As Base.Kernel.Logging.Trace
        objTrace1 = New Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP)
        If objTrace1.PerformanceData_All Then
            Dim rowSingle() As DataRow = objTrace1.StandardLog.Select("PerformanceCounterID = " & m_intPerformanceCounterID.ToString)
            Dim objTrace2 As Base.Kernel.Logging.Trace
            Dim decMax As Decimal
            Dim decMin As Decimal
            objTrace2 = New Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP)
            If objTrace2.PerformanceData_Detail(m_intPerformanceCounterID, 400, decMin, decMax) Then
                lblMin.Text = Format(decMin, CStr(rowSingle(0)("FormatString")))
                lblMax.Text = Format(decMax, CStr(rowSingle(0)("FormatString")))
                lblCategoryName.Text = CStr(rowSingle(0)("CategoryName"))
                lblCounterName.Text = CStr(rowSingle(0)("CounterName"))
                lblInstanceName.Text = CStr(rowSingle(0)("InstanceName"))
                lblCounterUnit.Text = CStr(rowSingle(0)("CounterUnit"))
                lblValue.Text = CStr(rowSingle(0)("PerformanceCounterValue"))

                If Not IsPostBack Then
                    Dim listitem As New listitem()
                    listitem.Text = "2 h"
                    listitem.Value = "20"
                    ddlPageSize.Items.Add(listitem)

                    If objTrace2.StandardLog.Rows.Count > 720 Then
                        listitem = New listitem()
                        listitem.Text = "3 h"
                        listitem.Value = "30"
                        ddlPageSize.Items.Add(listitem)
                    End If
                    If objTrace2.StandardLog.Rows.Count > 1440 Then
                        listitem = New listitem()
                        listitem.Text = "6 h"
                        listitem.Value = "60"
                        ddlPageSize.Items.Add(listitem)
                    End If
                    If objTrace2.StandardLog.Rows.Count > 2880 Then
                        listitem = New listitem()
                        listitem.Text = "12 h"
                        listitem.Value = "120"
                        ddlPageSize.Items.Add(listitem)
                    End If
                    If objTrace2.StandardLog.Rows.Count > 5760 Then
                        listitem = New listitem()
                        listitem.Text = "24 h"
                        listitem.Value = "240"
                        ddlPageSize.Items.Add(listitem)
                    End If
                    ddlPageSize.SelectedIndex = 0
                    'intValuesPerDateEntry = 240
                End If
                intValuesPerDateEntry = CInt(ddlPageSize.SelectedItem.Value)

                Dim i As Int32 = 0
                Dim j As Int32
                Dim k As Int32 = 1

                Dim intLastValue As Int32 = objTrace2.StandardLog.Rows.Count
                If intValuesPerDateEntry * 24 < intLastValue Then
                    intLastValue = intValuesPerDateEntry * 24
                End If

                'Dim rowTemp As DataRow
                Dim tbl2Temp As New DataTable()
                tbl2Temp.Columns.Add("IntValue", System.Type.GetType("System.Int32"))

                Dim control As control
                Dim control2 As control
                Dim label As label
                Dim repeater As repeater

                For j = 0 To intLastValue - 1
                    If i = 0 Then
                        For Each control In Page.Controls
                            If control.ID = "Form1" Then
                                For Each control2 In control.Controls
                                    If TypeOf control2 Is label Then
                                        If control2.ID = "Label" & k.ToString Then
                                            label = CType(control2, label)
                                            label.Text = CStr(objTrace2.StandardLog.Rows(j)("InsertDate"))
                                        End If
                                    End If
                                Next
                            End If
                        Next
                    End If
                    Dim rowNew As DataRow
                    rowNew = tbl2Temp.NewRow
                    rowNew("IntValue") = CInt(objTrace2.StandardLog.Rows(j)("IntValue"))
                    tbl2Temp.Rows.Add(rowNew)
                    i += 1
                    If (i = intValuesPerDateEntry) Or (j = intLastValue - 1) Then
                        For Each control In Page.Controls
                            If control.ID = "Form1" Then
                                For Each control2 In control.Controls
                                    If TypeOf control2 Is repeater Then
                                        If control2.ID = "Repeater" & k.ToString Then
                                            repeater = CType(control2, repeater)
                                            repeater.DataSource = tbl2Temp
                                            repeater.DataBind()
                                        End If
                                    End If
                                Next
                            End If
                        Next

                        tbl2Temp = New DataTable()
                        tbl2Temp.Columns.Add("IntValue", System.Type.GetType("System.Int32"))
                        i = 0
                        k += 1
                        If k > 24 Then Exit For
                    End If

                Next
            End If
        End If
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Try
            Response.Redirect("Performance.aspx?Return=True")
        Catch
        End Try
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        FillData()
    End Sub
End Class

' ************************************************
' $History: Performance2.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 14:47
' Created in $/CKAG/admin
' 
' *****************  Version 5  *****************
' User: Uha          Date: 13.03.07   Time: 10:53
' Updated in $/CKG/Admin/AdminWeb
' History-Eintrag vorbereitet
' 
' ************************************************
