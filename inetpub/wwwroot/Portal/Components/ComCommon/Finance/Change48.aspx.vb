Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System
Imports System.Runtime.Serialization.Formatters.Binary


Public Class Change48
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
    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    'Private m_intLineCount As Int32
    Private logApp As Base.Kernel.Logging.Trace

    Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucHeader As Header
    Protected WithEvents lnkVertragssuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Datagrid2 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblLegende As System.Web.UI.WebControls.Label
    Protected WithEvents trSaveButton As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trBackbutton As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        DataGrid1.Visible = False
        Datagrid2.Visible = False
        cmdBack.Visible = False


            GetAppIDFromQueryString(Me)

            Dim sAut As String = ""
            If Not Request.QueryString("Aut") = Nothing Then
                If Request.QueryString("Aut").ToString = "@!" Then
                    sAut = Request.QueryString("Aut").ToString()
                End If
            End If
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            logApp = New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

            If Not IsPostBack Then
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 2
                Session("Authorization") = Nothing
                Session("AuthorizationID") = Nothing
                Session("SelectedDealer") = Nothing
                Session("PageIndex") = Nothing

                trBackbutton.Visible = True
                trSaveButton.Visible = True
                If Session("SammelAutorisierungen") Is Nothing Then
                    FillGrid(0, m_User.Organization.OrganizationId.ToString)
                ElseIf sAut = "@!" Then ' zurück von der Autorisierung
                    FillGrid(0, m_User.Organization.OrganizationId.ToString)
                Else
                    FillGrid1(0)
                End If
            End If
      
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim dt As New DataTable()
        Dim strTemp As String = "0"
        Dim sOrganisation As String
        If m_User.IsTestUser Then
            strTemp = "1"
        End If
        sOrganisation = m_User.Organization.OrganizationId.ToString
        Dim da As New SqlClient.SqlDataAdapter( _
          "SELECT * FROM vwAuthorization" & _
          " WHERE (NOT (InitializedBy='" & m_User.UserName & "'))" & _
          " AND (NOT (AuthorizationLevel<" & m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AuthorizationLevel").ToString & "))" & _
          " AND (OrganizationID=" & sOrganisation & ")" & _
          " AND (TestUser=" & strTemp & ")" & _
          " AND (BatchAuthorization=1)" & _
          " ORDER BY InitializedWhen", _
          m_App.Connectionstring)

        da.Fill(dt)
        dt.Columns.Add("Ergebnis", System.Type.GetType("System.String"))

        'Hier müssen die Vorgänge der verschiedenen Anwendungen autorisiert werden

        Dim rowTemp As DataRow
        dt.AcceptChanges()
        For Each rowTemp In dt.Rows
            Select Case CStr(rowTemp("AppName"))
                Case "Change45"         'Freigeben gesperrter Vorgänge
                    Dim obj_fin_11 As fin_11
                    Dim OutPutStream As IO.MemoryStream
                    Dim formatter As BinaryFormatter

                    OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(rowTemp("AuthorizationID")), "m_change")
                    If OutPutStream Is Nothing Then
                        rowTemp("Ergebnis") = "Keine Daten für den Vorgang vorhanden."
                    Else
                        formatter = New BinaryFormatter()
                        'obj_fin_11 = New fin_11(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                        obj_fin_11 = DirectCast(formatter.Deserialize(OutPutStream), fin_11)
                        'obj_fin_11.SessionID = Session.SessionID.ToString


                        logApp = New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                        logApp.CollectDetails("Händlernummer", CType(obj_fin_11.Haendlernummer, Object), True)
                        logApp.CollectDetails("EQUINummer", CType(obj_fin_11.Equinr, Object))
                        logApp.CollectDetails("Storno", CType(obj_fin_11.Storno, Object))
                        logApp.CollectDetails("Fahrgestellnummer", CType(obj_fin_11.Fahrgestellnr, Object))

                        Try
                            obj_fin_11.AutorisierungUser = m_User.UserName
                            obj_fin_11.stornoorderfreigabe(Session("AppID").ToString, Session.SessionID, obj_fin_11.Storno, "X")
                            If obj_fin_11.Status = 0 Then
                                WriteLog("Brieffreigabe(Autorisierung) für Händler " & obj_fin_11.Haendlernummer & " erfolgreich durchgeführt.", obj_fin_11.Haendlernummer)
                                DeleteAuthorizationEntry(m_App.Connectionstring, CInt(rowTemp("AuthorizationID")))
                                rowTemp("Ergebnis") = "OK"
                            Else
                                WriteLog("Fehler bei der Brieffreigabe für Händler " & obj_fin_11.Haendlernummer & " , (Fehler: " & obj_fin_11.Message & ")", obj_fin_11.Haendlernummer, "ERR")
                                lblError.Text = "Beim Speichern Ihrer Daten sind Fehler aufgetreten."
                                rowTemp("Ergebnis") = obj_fin_11.Message
                            End If

                            logApp.WriteStandardDataAccessSAP(obj_fin_11.IDSAP)
                        Catch ex As Exception
                            WriteLog("Fehler bei der Brieffreigabe für Händler " & obj_fin_11.Haendlernummer & " , (Fehler: " & ex.Message & ")", obj_fin_11.Haendlernummer, "ERR")
                            lblError.Text = "Beim Speichern Ihrer Daten sind Fehler aufgetreten."
                            rowTemp("Ergebnis") = ex.Message
                            Throw ex
                        End Try


                    End If
                Case "Change47" 'Änderung Status
                    Dim OutPutStream As IO.MemoryStream
                    Dim formatter As New BinaryFormatter()
                    'Dim kkber As String = "0002"

                    Dim obj_fin_09 As fin_09

                    OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(rowTemp("AuthorizationID")), "objChange47_objRTFS")

                    'obj_fin_09 = New fin_09(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                    obj_fin_09 = DirectCast(formatter.Deserialize(OutPutStream), fin_09)

                    logApp = New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                    logApp.CollectDetails("Kundennummer", CType(obj_fin_09.KUNNR, Object), True)

                    Try
                        obj_fin_09.Change()
                        If obj_fin_09.Status = 0 Then
                            WriteLog("Änderung Status für Vertragsnummer " & CType(obj_fin_09.Vertragsnummer, String) & " erfolgreich durchgeführt.", obj_fin_09.KUNNR)
                            DeleteAuthorizationEntry(m_App.Connectionstring, CInt(rowTemp("AuthorizationID")))
                            rowTemp("Ergebnis") = "OK"
                        Else
                            WriteLog("Fehler bei Änderung Status für Vertragsnummer " & CType(obj_fin_09.Vertragsnummer, String) & " , (Fehler: " & obj_fin_09.Message & ")", obj_fin_09.KUNNR, "ERR")
                            lblError.Text = "Beim Speichern Ihrer Daten sind Fehler aufgetreten."
                            rowTemp("Ergebnis") = obj_fin_09.Message
                        End If

                        logApp.WriteStandardDataAccessSAP(obj_fin_09.IDSAP)
                    Catch ex As Exception
                        WriteLog("Fehler bei Änderung Status für Vertragsnummer " & CType(obj_fin_09.Vertragsnummer, String) & " , (Fehler: " & obj_fin_09.Message & ")", obj_fin_09.KUNNR, "ERR")
                        lblError.Text = "Beim Speichern Ihrer Daten sind Fehler aufgetreten."
                        rowTemp("Ergebnis") = ex.Message
                        Throw ex
                    End Try
            End Select
        Next
        dt.AcceptChanges()

        Session("SammelAutorisierungen") = dt
        DataGrid1.Visible = False
        FillGrid1(0)
        cmdSave.Visible = False
        cmdBack.Visible = True
    End Sub

    Private Sub FillGrid0(ByRef tblData As DataTable, ByRef dgShow As DataGrid, ByVal intPageIndex As Int32, ByVal strSort As String)
        'm_intLineCount = 0
        If tblData.Rows.Count = 0 Then
            trBackbutton.Visible = True
            trSaveButton.Visible = True
            dgShow.Visible = False
            ddlPageSize.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            ShowScript.Visible = False
            cmdSave.Enabled = False
            lblLegende.Visible = False
        Else
            dgShow.Visible = True
            lblNoData.Visible = False
            trBackbutton.Visible = True
            trSaveButton.Visible = True
            lblLegende.Visible = True
            Dim tmpDataView As DataView = tblData.DefaultView
            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            Dim rowsTmp() As DataRow = tblData.Select("BatchAuthorization=1")
            If rowsTmp.GetLength(0) > 0 Then
                cmdSave.Enabled = True
            Else
                cmdSave.Enabled = False
            End If

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

            dgShow.CurrentPageIndex = intTempPageIndex

            dgShow.DataSource = tmpDataView
            dgShow.DataBind()

            lblNoData.Text = "Anzahl Vorgänge: " & tmpDataView.Count.ToString
            lblNoData.Visible = True

            If dgShow.PageCount > 1 Then
                dgShow.PagerStyle.CssClass = "PagerStyle"
                dgShow.DataBind()
                dgShow.PagerStyle.Visible = True
            Else
                dgShow.PagerStyle.Visible = False
            End If
        End If
    End Sub

    Private Sub FillGrid1(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim dt As DataTable = CType(Session("SammelAutorisierungen"), DataTable)
        FillGrid0(dt, Datagrid2, intPageIndex, strSort)
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, ByVal sDitriktOrganisation As String, Optional ByVal strSort As String = "")
        Dim dt As New DataTable()
        Dim row As DataRow
        Dim strTemp As String = "0"
        If m_User.IsTestUser Then
            strTemp = "1"
        End If
        Dim da As New SqlClient.SqlDataAdapter( _
          "SELECT * FROM vwAuthorization" & _
          " WHERE (NOT (InitializedBy='" & m_User.UserName & "'))" & _
          " AND (NOT (AuthorizationLevel<" & m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AuthorizationLevel").ToString & "))" & _
          " AND (OrganizationID=" & sDitriktOrganisation & ")" & _
          " AND (TestUser=" & strTemp & ")" & _
          " ORDER BY InitializedWhen", _
          m_App.Connectionstring)


        da.Fill(dt)

        For Each row In dt.Rows
            If row("ProcessReference2").ToString = "2" Then
                row("ProcessReference2") = "S"
            End If
            If row("ProcessReference2").ToString = "3" Then
                row("ProcessReference2") = "N"
            End If
            If row("ProcessReference2").ToString = "4" Then
                row("ProcessReference2") = "V"
            End If
        Next
        dt.AcceptChanges()
        FillGrid0(dt, DataGrid1, intPageIndex, strSort)
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        If e.CommandName = "Autorisieren" Then
            Session("Authorization") = True
            Session("AuthorizationID") = e.Item.Cells(3).Text
            Dim strLastRecordParam As String = ""
            If DataGrid1.Items.Count < 2 Then
                strLastRecordParam = "&LastRecord=True"
            End If

            Select Case e.Item.Cells(1).Text
                Case "Change40" 'Kontingent-Aenderung
                    Session("objSuche") = Nothing
                    Session("SelectedDealer") = e.Item.Cells(7).Text
                    Response.Redirect(e.Item.Cells(1).Text & "Edit.aspx?AppID=" & e.Item.Cells(0).Text & strLastRecordParam)
                Case "Change45" 'Freigabe gesperrter Auftraege
                    Session("objSuche") = Nothing
                    Session("SelectedDealer") = e.Item.Cells(7).Text
                    Session("m_change") = deserialization("m_change")
                    Response.Redirect(e.Item.Cells(1).Text & "_1.aspx?AppID=" & e.Item.Cells(0).Text & strLastRecordParam)
                Case "Change47" 'Änderung Status
                    Session("objSuche") = Nothing
                    Session("SelectedDealer") = e.Item.Cells(7).Text
                    Response.Redirect(e.Item.Cells(1).Text & "_02.aspx?AppID=" & e.Item.Cells(0).Text & strLastRecordParam & "&FIN=" & e.Item.Cells(8).Text)
            End Select
        ElseIf e.CommandName = "Loeschen" Then
            Try
                DeleteAuthorizationEntry(m_App.Connectionstring, CInt(e.Item.Cells(3).Text))
                WriteLog("Autorisierung - Anwendung " & e.Item.Cells(4).Text & ", Initiator: " & e.Item.Cells(5).Text & ", Angelegt: " & e.Item.Cells(6).Text & ", Händler: " & e.Item.Cells(7).Text & ", Weiteres Merkmal: " & e.Item.Cells(8).Text & " - erfolgreich gelöscht.", e.Item.Cells(7).Text)
                lblNoData.Text = "<b>Daten wurden aus Autorisierung entfernt.</b><br>&nbsp;"
                Session("Authorization") = Nothing
                Session("AuthorizationID") = Nothing
                If Not Request.QueryString("Aut") = Nothing Then
                    Request.QueryString("Aut").Replace("@!", "")
                End If
                FillGrid(0, m_User.Organization.OrganizationId.ToString)

            Catch ex As Exception
                WriteLog("Autorisierung - Anwendung " & e.Item.Cells(4).Text & ", Initiator: " & e.Item.Cells(5).Text & ", Angelegt: " & e.Item.Cells(6).Text & ", Händler: " & e.Item.Cells(7).Text & ", Weiteres Merkmal: " & e.Item.Cells(8).Text & " - Fehler beim Löschen: " & ex.Message, e.Item.Cells(7).Text, "ERR")
                lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                Throw ex
            End Try
        End If
    End Sub


    Private Function deserialization(ByVal objName As String) As Object
        Dim x As Object
        Dim OutPutStream As IO.MemoryStream
        Dim formatter As BinaryFormatter
        OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(Session("AuthorizationID")), objName)
        If OutPutStream Is Nothing Then
            lblError.Text = "Keine Daten für den Vorgang vorhanden."
            Return Nothing
        Else
            formatter = New BinaryFormatter()
            x = formatter.Deserialize(OutPutStream)
            Return x
        End If
    End Function

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex, m_User.Organization.OrganizationId.ToString)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, m_User.Organization.OrganizationId, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0, m_User.Organization.OrganizationId.ToString)
    End Sub

    Private Sub WriteLog(ByVal strMessage As String, ByVal strHaendler As String, Optional ByVal strType As String = "APP")
        logApp.WriteEntry(strType, m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(strHaendler, 5), strMessage, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
    End Sub

    Private Sub Datagrid2_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles Datagrid2.PageIndexChanged
        FillGrid1(e.NewPageIndex)
    End Sub

    Private Sub Datagrid2_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles Datagrid2.SortCommand
        FillGrid1(Datagrid2.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        cmdBack.Visible = False
        cmdSave.Visible = True
        FillGrid(0, m_User.Organization.OrganizationId.ToString)
        Datagrid2.Visible = False
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change48.aspx.vb $
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 3.03.10    Time: 13:52
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 16.03.09   Time: 17:06
' Updated in $/CKAG/Components/ComCommon/Finance
' RTFS Autorisierungskorrekturen
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 16.03.09   Time: 16:55
' Updated in $/CKAG/Components/ComCommon/Finance
' autorisierungsänderung/Berichtung RTFS
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 26.05.08   Time: 10:20
' Updated in $/CKAG/Components/ComCommon/Finance
' RTFS BankPortal Autorisierung Bugfixing
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 9.01.08    Time: 9:24
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
'  bugfix Sammelautorisierung bei Freigabe gesperrter Aufträge
' 
' *****************  Version 7  *****************
' User: Uha          Date: 8.01.08    Time: 16:53
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 8.01.08    Time: 16:19
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Einzelautorisierung bei der Freigabe Briefanforderungen
' 
' *****************  Version 5  *****************
' User: Uha          Date: 8.01.08    Time: 15:33
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 8.01.08    Time: 14:09
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 3  *****************
' User: Uha          Date: 8.01.08    Time: 12:54
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 2  *****************
' User: Uha          Date: 8.01.08    Time: 12:45
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 1  *****************
' User: Uha          Date: 8.01.08    Time: 9:39
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Autorisierunganwendung (funktionslos) hinzugefügt
' 
' ************************************************
