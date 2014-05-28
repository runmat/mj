Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Namespace Treuhand
    Partial Public Class Change03s
        Inherits System.Web.UI.Page

        Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation

        Private m_User As Base.Kernel.Security.User
        Private m_App As Base.Kernel.Security.App

        Dim m_report As Treuhandsperre

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            m_User = GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte
            FormAuth(Me, m_User)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
            GridNavigation1.setGridElment(GridView1)

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            If Page.IsPostBack = False Then
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                m_report = New Treuhandsperre(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
                Session.Add("objReport", m_report)
                m_report.SessionID = Me.Session.SessionID
                m_report.AppID = CStr(Session("AppID"))
                rbGesperrt.Checked = True
                doSubmit()
            ElseIf Not Session("objReport") Is Nothing Then
                m_report = CType(Session("objReport"), Treuhandsperre)
            End If
        End Sub
        Private Sub doSubmit()
            If rbGesperrt.Checked Then
                m_report.Aktion = "G"
            Else
                m_report.Aktion = "A"
            End If
            m_report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)

            If m_report.Status < 0 Then
                lblError.Text = m_report.Message
            Else
                If m_report.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                    GridView1.Visible = False
                    Result.Visible = False
                Else
                    Session("ResultTable") = m_report.Result
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
                    Result.Visible = False
                Else
                    lblError.Visible = False
                    GridView1.Visible = True
                    Result.Visible = True
                    lblNoData.Visible = False


                    Select Case rbGesperrt.Checked
                        Case True
                            cmdAblehnen.Visible = True
                            cmdFreigabe.Visible = True
                        Case False
                            cmdAblehnen.Visible = False
                            cmdFreigabe.Visible = True
                    End Select

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
                    CheckgridForAutorisierung()
                End If

            Else
                lblError.Text = "Keine Daten zur Anzeige gefunden."
            End If
        End Sub

        Private Sub lnkCreateExcel1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCreateExcel1.Click

            Dim control As New Control
            Dim tblTranslations As New DataTable()
            Dim tblTemp As DataTable = CType(Session("ResultTable"), DataTable).Copy
            Dim AppURL As String
            Dim col As DataControlField
            Dim col2 As DataColumn
            Dim bVisibility As Integer
            Dim i As Integer
            Dim sColName As String = ""

            If tblTemp.Columns.Contains("BELNR") Then
                tblTemp.Columns.Remove("BELNR")
            End If
            Dim tmpRow As DataRow

            For Each tmpRow In tblTemp.Rows
                tmpRow("Versandadresse") = Replace(tmpRow("Versandadresse").ToString, "<br/>", ", ")
            Next

            AppURL = Replace(Me.Request.Url.LocalPath, "/Services", "..")
            tblTranslations = CType(Me.Session(AppURL), DataTable)
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
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Me.Page)

            m_report.Result = CType(Session("ResultTable"), DataTable)
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub

        Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
            GridView1.PageIndex = PageIndex
            FillGrid(PageIndex)
        End Sub

        Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
            FillGrid(0)
        End Sub
        Private Sub Gridview1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
            FillGrid(GridView1.PageIndex, e.SortExpression)
        End Sub

        Private Function Checkgrid(ByVal FreigabeFlag As String) As Boolean
            Dim item As GridViewRow
            Dim chkBox As CheckBox
            Dim bchecked As Boolean
            Dim label As Label
            Dim txtBox As TextBox
            bchecked = False
            lblError.Text = ""
            For Each item In GridView1.Rows
                chkBox = CType(item.FindControl("chkSperre"), CheckBox)
                If chkBox.Checked = True Then
                    label = CType(item.FindControl("lblBELNR"), WebControls.Label)
                    Dim dRow As DataRow = m_report.Result.Select("BELNR='" & label.Text & "'")(0)
                    dRow("zurFreigabe") = "X"
                    If FreigabeFlag = "A" Then
                        txtBox = CType(item.FindControl("txtAblehnungsgrund"), TextBox)
                        If txtBox.Text.Trim.Length > 0 Then
                            dRow("Ablehnungsgrund") = txtBox.Text
                            bchecked = True
                        Else
                            item.Cells(7).BackColor = System.Drawing.Color.FromArgb(196, 0, 0)
                            lblError.Text = "Bitte geben Sie in den markierten Zellen einen Ablehnungsgrund an!"
                            lblError.Visible = True
                            bchecked = False
                        End If
                    Else
                        bchecked = True
                    End If

                    m_report.Result.AcceptChanges()

                End If
                Session("ResultTable") = m_report.Result
            Next
            If lblError.Text <> "" Then
                Return False
            Else
                Return bchecked
            End If

        End Function
        Private Sub CheckgridForAutorisierung()
            Dim item As GridViewRow
            Dim label As Label
            Dim strInitiator As String = ""
            Dim strFin As String
            Dim strTreunehmer As String
            Dim chkBox As CheckBox
            Dim txtBox As TextBox

            Dim intAutID As Int32 = 0
            lblError.Text = ""
            For Each item In GridView1.Rows

                label = CType(item.FindControl("lblFahrgestellnummer"), WebControls.Label)
                strFin = label.Text
                label = CType(item.FindControl("lblTreunehmer"), WebControls.Label)
                strTreunehmer = label.Text
                m_App.CheckForPendingAuthorization(CInt(Session("AppID")), m_User.Organization.OrganizationId, strTreunehmer, strFin, m_User.IsTestUser, strInitiator, intAutID)
                If Not strInitiator.Length = 0 Then
                    label = CType(item.FindControl("lblInAut"), WebControls.Label)
                    label.Visible = True
                    chkBox = CType(item.FindControl("chkSperre"), CheckBox)
                    chkBox.Checked = False
                    chkBox.Visible = False
                    txtBox = CType(item.FindControl("txtAblehnungsgrund"), TextBox)
                    txtBox.Enabled = False
                End If
            Next
        End Sub
        Protected Sub cmdFreigabe_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdFreigabe.Click
            If Checkgrid("F") Then

                m_report.Aktion = "F"
                Dim iGroupLevel As Integer = m_User.Groups(0).Authorizationright
                Dim iAppLevel As Integer = CInt(m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AuthorizationLevel"))

                If m_User.Groups.ItemByID(m_User.GroupID).Authorizationright > 0 Then
                    For Each tmpRow As DataRow In m_report.Result.Rows
                        If tmpRow("zurFreigabe").ToString = "X" Then
                            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                            logApp.CollectDetails("Treunehmer", CType(tmpRow("Treunehmer").ToString, Object), True)
                            logApp.CollectDetails("Angefordert am", CType(Now.ToShortDateString, Object))
                            logApp.CollectDetails("Fahrgestellnummer", CType(tmpRow("Fahrgestellnummer").ToString, Object))
                            logApp.CollectDetails("Kennzeichen", CType(tmpRow("Kennzeichen").ToString, Object))
                            logApp.CollectDetails("Freigabe", "X")
                            logApp.CollectDetails("Abgelehnt", "")
                            m_report.FinforAut = tmpRow("Fahrgestellnummer").ToString
                            'Anwendung erfordert Autorisierung (Level>0)
                            Dim DetailArray(1, 2) As Object
                            Dim ms As MemoryStream
                            Dim formatter As BinaryFormatter
                            Dim b() As Byte

                            ms = New MemoryStream()
                            formatter = New BinaryFormatter()
                            formatter.Serialize(ms, m_report)
                            b = ms.ToArray
                            ms = New MemoryStream(b)
                            DetailArray(0, 0) = ms
                            DetailArray(0, 1) = "objReport"

                            'Pruefen, ob schon in der Autorisierung.
                            Dim strInitiator As String = ""
                            Dim intAuthorizationID As Int32

                            m_App.CheckForPendingAuthorization(CInt(Session("AppID")), m_User.Organization.OrganizationId, m_User.KUNNR, tmpRow("Fahrgestellnummer").ToString, m_User.IsTestUser, strInitiator, intAuthorizationID)
                            If Not strInitiator.Length = 0 Then
                                'Fahrzeug wurde schon mal freigegeben und liegt zur Autorisierung vor
                                lblError.Text = "Diese Anforderung wurde schon einmal freigegeben und liegt bereits zur Autorisierung vor!"
                                Exit Sub
                            End If

                            intAuthorizationID = WriteAuthorization(m_App.Connectionstring, CInt(Session("AppID")), m_User.UserName, m_User.Organization.OrganizationId, tmpRow("Treunehmer").ToString, tmpRow("Fahrgestellnummer").ToString, "Freigabe", "", m_User.IsTestUser, DetailArray)
                            tmpRow("Status") = "Aut."
                            logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, tmpRow("Treunehmer").ToString, "Freigabe für Treunehmer " & tmpRow("Treunehmer").ToString & " erfolgreich initiiert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                            CheckgridForAutorisierung()
                        End If
                    Next
                Else
                    'Anwendung erfordert keine Autorisierung (Level=0)
                    m_report.Change(Session("AppID").ToString, Session.SessionID.ToString, Me)
                    If m_report.Status < 0 Then
                        lblError.Text = m_report.Message
                        GridView1.Visible = False
                        Result.Visible = False
                    Else
                        doSubmit()
                    End If
                End If
            End If
        End Sub

        Protected Sub cmdAblehnen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAblehnen.Click
            If Checkgrid("A") Then
                m_report.Aktion = "A"
                If m_User.Groups.ItemByID(m_User.GroupID).Authorizationright > 0 Then
                    For Each tmpRow As DataRow In m_report.Result.Rows
                        If tmpRow("zurFreigabe").ToString = "X" Then
                            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                            logApp.CollectDetails("Treunehmer", CType(tmpRow("Treunehmer").ToString, Object), True)
                            logApp.CollectDetails("Angefordert am", CType(Now.ToShortDateString, Object))
                            logApp.CollectDetails("Fahrgestellnummer", CType(tmpRow("Fahrgestellnummer").ToString, Object))
                            logApp.CollectDetails("Kennzeichen", CType(tmpRow("Kennzeichen").ToString, Object))
                            logApp.CollectDetails("Freigabe", "")
                            logApp.CollectDetails("Abgelehnt", "X")
                            m_report.FinforAut = tmpRow("Fahrgestellnummer").ToString
                            'Anwendung erfordert Autorisierung (Level>0)
                            Dim DetailArray(1, 2) As Object
                            Dim ms As MemoryStream
                            Dim formatter As BinaryFormatter
                            Dim b() As Byte

                            ms = New MemoryStream()
                            formatter = New BinaryFormatter()
                            formatter.Serialize(ms, m_report)
                            b = ms.ToArray
                            ms = New MemoryStream(b)
                            DetailArray(0, 0) = ms
                            DetailArray(0, 1) = "objReport"

                            'Pruefen, ob schon in der Autorisierung.
                            Dim strInitiator As String = ""
                            Dim intAuthorizationID As Int32

                            m_App.CheckForPendingAuthorization(CInt(Session("AppID")), m_User.Organization.OrganizationId, m_User.KUNNR, tmpRow("Fahrgestellnummer").ToString, m_User.IsTestUser, strInitiator, intAuthorizationID)
                            If Not strInitiator.Length = 0 Then
                                'Fahrzeug wurde schon mal freigegeben und liegt zur Autorisierung vor
                                lblError.Text = "Diese Anforderung wurde schon einmal freigegeben und liegt bereits zur Autorisierung vor!"
                                Exit Sub
                            End If

                            intAuthorizationID = WriteAuthorization(m_App.Connectionstring, CInt(Session("AppID")), m_User.UserName, m_User.Organization.OrganizationId, tmpRow("Treunehmer").ToString, tmpRow("Fahrgestellnummer").ToString, "Ablehnung", "", m_User.IsTestUser, DetailArray)
                            tmpRow("Status") = "Aut."
                            logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, tmpRow("Treunehmer").ToString, "Ablehnung für Treunehmer " & tmpRow("Treunehmer").ToString & " erfolgreich initiiert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                            CheckgridForAutorisierung()
                        End If
                    Next
                Else
                    'm_report.Treunehmer = m_User.Reference
                    m_report.Aktion = "A"
                    m_report.Change(Session("AppID").ToString, Session.SessionID.ToString, Me)
                    If m_report.Status < 0 Then
                        lblError.Text = m_report.Message
                        GridView1.Visible = False
                        Result.Visible = False
                    Else
                        doSubmit()
                    End If
                End If

            End If

        End Sub

        Protected Sub rbGesperrt_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbGesperrt.CheckedChanged
            If rbGesperrt.Checked = True Then doSubmit()
        End Sub

        Protected Sub rbAbgelehnt_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbAbgelehnt.CheckedChanged
            If rbAbgelehnt.Checked = True Then doSubmit()
        End Sub

        Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
            Session("objReport") = Nothing
            Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
        End Sub
    End Class


End Namespace
