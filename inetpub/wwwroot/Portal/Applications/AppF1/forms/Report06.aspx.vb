Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business
Imports CKG

Public Class Report06
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private m_objTable As DataTable
    Private m_strAppID As String
    Private m_context As HttpContext = HttpContext.Current

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdPrint As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lnkCreateExcel As System.Web.UI.WebControls.LinkButton

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

#Region " Methods "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetAppIDFromQueryString(Me)

        Try
            m_strAppID = CStr(Request.QueryString("AppID"))
            cmdSave.Enabled = False
            m_User = GetUser(Me)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            m_App = New Base.Kernel.Security.App(m_User)

            Try
                Dim strFileName As String
                strFileName = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

                If Not IsPostBack Then
                    lblHead.Text = m_User.Applications.Select("AppID = " & m_strAppID)(0)("AppFriendlyName").ToString
                    ucStyles.TitleText = lblHead.Text

                    Dim m_Report As F1_Bank_TempMahn
                    If Not Session("Mahnungen") Is Nothing Then
                        m_objTable = CType(Session("Mahnungen"), DataTable)
                        ucHeader.LockLinks()
                        If IsNothing(Session("AusMahnSumme")) = False Then
                            lnkCreateExcel.Visible = True
                        End If
                    Else
                        m_Report = New F1_Bank_TempMahn(m_User, m_App, m_strAppID, Session.SessionID.ToString, strFileName)
                        m_Report.fill(Session("AppID").ToString, Session.SessionID)

                        If Not m_Report.Status = 0 Then
                            If m_Report.Status = -1 Then
                                lblError.Text = m_Report.Message
                                Exit Sub
                            Else
                                'wenn keine Mahnungen vorhanden sind, sofortige weiterleitung zur Startseite JJ2007.12.13
                                Response.Redirect("../../../Start/Selection.aspx")
                            End If

                        Else
                            If m_Report.NewResultTable.Rows.Count = 0 Then
                                'wenn keine Mahnungen vorhanden sind, sofortige weiterleitung zur Startseite JJ2007.12.13
                                Response.Redirect("../../../Start/Selection.aspx")
                            Else
                                m_objTable = m_Report.NewResultTable
                                Session.Add("Mahnungen", m_objTable)
                                ucHeader.LockLinks()
                                lnkCreateExcel.Visible = True
                            End If
                        End If
                    End If

                    ddlPageSize.Items.Add("10")
                    ddlPageSize.Items.Add("20")
                    ddlPageSize.Items.Add("50")
                    ddlPageSize.Items.Add("100")
                    ddlPageSize.Items.Add("200")
                    ddlPageSize.Items.Add("500")
                    ddlPageSize.Items.Add("1000")
                    ddlPageSize.SelectedIndex = 2

                    ViewState("Direction") = "asc"
                    FillGrid(0, "MAHNART")
                Else
                    If Not Session("Mahnungen") Is Nothing Then
                        m_objTable = CType(Session("Mahnungen"), DataTable)
                    End If
                End If

                If Not m_objTable Is Nothing AndAlso m_objTable.Rows.Count > 0 Then
                    Dim objExcelExport As New Excel.ExcelExport()
                    Try
                        Excel.ExcelExport.WriteExcel(m_objTable, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                    Catch
                    End Try
                End If

            Catch ex As Exception
                lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Try
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If IsNothing(Session("AusMahnSumme")) = True Then
            Log(m_User.Reference, "Mahnungen gesehen und mit ""OK"" bestätigt.")
            Session("Mahnungen") = Nothing
            Response.Redirect("../../../Start/Selection.aspx")
        Else
            Session("Mahnungen") = Nothing
            Session("AusMahnSumme") = Nothing
            Response.Redirect("Report08.aspx?AppID=" & Session("AppID").ToString)
        End If

    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If m_objTable.Rows.Count = 0 Then
            DataGrid1.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            ShowScript.Visible = False
        Else
            DataGrid1.Visible = True
            lblNoData.Visible = False

            Dim tmpDataView As New DataView()
            tmpDataView = m_objTable.DefaultView

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

            DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
            DataGrid1.CurrentPageIndex = intTempPageIndex

            DataGrid1.DataSource = tmpDataView
            DataGrid1.DataBind()

            If (Not Session("ShowOtherString") Is Nothing) AndAlso CStr(Session("ShowOtherString")).Length > 0 Then
                lblNoData.Text = CStr(Session("ShowOtherString"))
            Else
                lblNoData.Text = "" '"Es wurden " & tmpDataView.Count.ToString & " Einträge zu """ & m_User.Applications.Select("AppID = " & m_strAppID)(0)("AppFriendlyName").ToString & """ gefunden."
            End If
            If (Not Session("BackLink") Is Nothing) AndAlso CStr(Session("BackLink")) = "HistoryBack" Then
                lnkKreditlimit.Text = "Zurück"
                lnkKreditlimit.NavigateUrl = "javascript:history.back()"
            End If
            lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If

            For Each item As DataGridItem In DataGrid1.Items

                If Not item.FindControl("lnkHistorie") Is Nothing Then

                    If Not m_User.Applications.Select("AppName = 'Report46'").Count = 0 Then

                        CType(item.FindControl("lnkHistorie"), HyperLink).NavigateUrl = "../../../Components/ComCommon/Finance/Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN=" & CType(item.FindControl("lnkHistorie"), HyperLink).Text

                    End If

                End If

            Next



        End If
        With DataGrid1
            If .PageCount = .CurrentPageIndex + 1 Then
                cmdSave.Enabled = True
            End If
        End With
    End Sub


    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrint.Click
        '##

    End Sub

    Private Sub Log(ByVal strIdentification As String, ByVal strDescription As String, Optional ByVal strCategory As String = "APP")
        Dim logApp As New Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel)

        ' strCategory
        Dim strUserName As String = m_User.UserName ' strUserName
        Dim strSessionID As String = Session.SessionID ' strSessionID
        Dim intSource As Integer = CInt(Request.QueryString("AppID")) ' intSource 
        Dim strTask As String = Me.lblHead.Text ' strTask
        ' strIdentification
        ' strDescription
        Dim strCustomerName As String = m_User.CustomerName ' strCustomername
        Dim blnIsTestUser As Boolean = m_User.IsTestUser ' blnIsTestUser
        Dim intSeverity As Integer = 0 ' intSeverity 

        logApp.WriteEntry(strCategory, strUserName, strSessionID, intSource, strTask, strIdentification, strDescription, strCustomerName, m_User.Customer.CustomerId, blnIsTestUser, intSeverity)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub


    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
        Try
            Dim control As New Control
            Dim tblTranslations As New DataTable()
            Dim tblTemp As New DataTable()
            Dim AppURL As String
            Dim col As DataGridColumn
            Dim col2 As DataColumn
            Dim bVisibility As Integer
            Dim i As Integer
            Dim sColName As String = ""
            Dim isTranslated As Boolean = False

            AppURL = Replace(Me.Request.Url.LocalPath, "/Portal", "..")
            tblTranslations = CType(Me.Session(AppURL), DataTable)
            tblTemp = CType(Session("Mahnungen"), DataTable).Copy


            'hilfsspalten
            tblTemp.Columns.Remove("MAHNART")
            tblTemp.Columns.Remove("AG")
            tblTemp.Columns.Remove("HAENDLER")
            tblTemp.Columns.Remove("HDGRP")
            tblTemp.Columns.Remove("AUGRU")
            tblTemp.Columns.Remove("HDGRP_EX")




            For Each col In DataGrid1.Columns
                For i = tblTemp.Columns.Count - 1 To 0 Step -1
                    bVisibility = 0
                    col2 = tblTemp.Columns(i)
                    If col2.ColumnName.ToUpper = col.SortExpression.ToUpper OrElse col2.ColumnName.ToUpper = col.HeaderText.ToUpper.Replace("COL_", "") Then
                        sColName = TranslateColLbtn(DataGrid1, tblTranslations, col.HeaderText, bVisibility)
                        If bVisibility = 0 Then
                            tblTemp.Columns.Remove(col2)
                        ElseIf sColName.Length > 0 Then
                            col2.ColumnName = sColName
                        Else
                            'alle spalten die nicht in der spaltenübersetzung sind, entfernen
                            tblTemp.Columns.Remove(col2)
                        End If

                        isTranslated = True

                    End If
                Next
                tblTemp.AcceptChanges()
            Next
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Me.Page)
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub
#End Region

End Class
' ************************************************
' $History: Report06.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 29.09.09   Time: 13:54
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 25.03.09   Time: 17:12
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2670 nachbesserungen
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 25.03.09   Time: 17:06
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2670
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 25.03.09   Time: 16:50
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2670 testfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 25.03.09   Time: 14:15
' Updated in $/CKAG/Applications/AppF1/forms
' ITa 2670
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 25.03.09   Time: 8:35
' Created in $/CKAG/Applications/AppF1/forms
' ITA 2670
' 
' ************************************************


