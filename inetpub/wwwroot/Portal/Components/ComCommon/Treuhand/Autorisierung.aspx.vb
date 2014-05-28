﻿Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Components.ComCommon.Treuhand
Imports System.Runtime.Serialization.Formatters.Binary

Namespace Treuhand
    Partial Public Class Autorisierung
        Inherits System.Web.UI.Page

#Region "Declarations"
        Private m_User As Base.Kernel.Security.User
        Private m_App As Base.Kernel.Security.App
        Private logApp As Base.Kernel.Logging.Trace
        Private m_intLineCount As Int32
#End Region


#Region "Events"
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            m_User = GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte
            FormAuth(Me, m_User)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            ucHeader.InitUser(m_User)
            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            GridView1.Visible = False
            GridView2.Visible = False
            cmdBack.Visible = False


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
        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub

        Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
            If e.CommandName = "Autorisieren" Or e.CommandName = "Loeschen" Then
                Dim index As Integer = CType(e.CommandArgument, Integer)
                Dim GridRow As GridViewRow = GridView1.Rows(index)

                Dim strAuthorizationID As String
                Dim strAppName As String
                Dim strAppID As String
                Dim strCustomer As String
                Dim strInitiator As String
                Dim strAngelegt As String
                Dim strMerkmal As String
                Dim strAppFriend As String
                Dim lbl As Label

                lbl = CType(GridRow.Cells(3).FindControl("lblAuthorizationID"), Label)
                strAuthorizationID = lbl.Text

                lbl = CType(GridRow.Cells(1).FindControl("lblAppName"), Label)
                strAppName = lbl.Text

                lbl = CType(GridRow.Cells(4).FindControl("lblAnwendung"), Label)
                strAppFriend = lbl.Text

                lbl = CType(GridRow.Cells(0).FindControl("lblAppID"), Label)
                strAppID = lbl.Text

                lbl = CType(GridRow.Cells(7).FindControl("lblReferenz"), Label)
                strCustomer = lbl.Text
                lbl = CType(GridRow.Cells(5).FindControl("lblInitiator"), Label)
                strInitiator = lbl.Text

                lbl = CType(GridRow.Cells(6).FindControl("lblAngelegt"), Label)
                strAngelegt = lbl.Text

                lbl = CType(GridRow.Cells(8).FindControl("lblMerkmal1"), Label)
                strMerkmal = lbl.Text

                Session("Authorization") = True
                Session("AuthorizationID") = strAuthorizationID

                Dim strLastRecordParam As String = ""
                If GridView1.Rows.Count < 2 Then
                    strLastRecordParam = "&LastRecord=True"
                End If
                Select Case e.CommandName
                    Case "Autorisieren"
                        Select Case strAppName
                            Case "Change01"
                                Session("objSuche") = Nothing
                                Session("SelectedDealer") = strCustomer
                                Response.Redirect("Change01Aut.aspx?AppID=" & strAppID & strLastRecordParam & "&Aut=" & strAuthorizationID)
                            Case "Change02"
                                Session("objSuche") = Nothing
                                Session("SelectedDealer") = strCustomer
                                Response.Redirect("Change02Aut.aspx?AppID=" & strAppID & strLastRecordParam & "&Aut=" & strAuthorizationID)
                            Case Else
                        End Select
                    Case "Loeschen"
                        Try
                            DeleteAuthorizationEntry(m_App.Connectionstring, CInt(strAuthorizationID))
                            WriteLog("Autorisierung - Anwendung " & strAppFriend & ", Initiator: " & strInitiator & ", Angelegt: " & strAngelegt & ", Referenz: " & strCustomer & ", Weiteres Merkmal: " & strMerkmal & " - erfolgreich gelöscht.", strCustomer)
                            lblNoData.Text = "<b>Daten wurden aus Autorisierung entfernt.</b><br>&nbsp;"
                            Session("Authorization") = Nothing
                            Session("AuthorizationID") = Nothing
                            If Not Request.QueryString("Aut") = Nothing Then
                                Request.QueryString("Aut").Replace("@!", "")
                            End If

                            FillGrid(0, m_User.Organization.OrganizationId.ToString)

                        Catch ex As Exception
                            WriteLog("Autorisierung - Anwendung " & strAppName & ", Initiator: " & strInitiator & ", Angelegt: " & strAngelegt & ", Referenz: " & strCustomer & ", Weiteres Merkmal: " & strMerkmal & " - Fehler beim Löschen: " & strCustomer, "ERR")
                            lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                        End Try
                    Case Else
                End Select
            End If

        End Sub

        Protected Sub cmdSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave.Click
            Dim dt As New DataTable()
            Dim strTemp As String = "0"
            Dim DistrictID As Integer = 0
            Dim sDitriktOrganisation As String
            If m_User.IsTestUser Then
                strTemp = "1"
            End If

            sDitriktOrganisation = m_User.Organization.OrganizationId.ToString
            Dim da As New SqlClient.SqlDataAdapter( _
              "SELECT * FROM vwAuthorization" & _
              " WHERE (NOT (InitializedBy='" & m_User.UserName & "'))" & _
              " AND (NOT (AuthorizationLevel<" & m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AuthorizationLevel").ToString & "))" & _
              " AND (OrganizationID=" & sDitriktOrganisation & ")" & _
              " AND (TestUser=" & strTemp & ")" & _
              " AND (BatchAuthorization=1)" & _
              " ORDER BY InitializedWhen", _
              m_App.Connectionstring)

            da.Fill(dt)
            dt.Columns.Add("Ergebnis", System.Type.GetType("System.String"))

            'Hier müssen die Vorgänge der verschiedenen Anwendungen autorisiert werden
            '1. Freigeben gesperrter Vorgänge
            Dim rowTemp As DataRow
            dt.AcceptChanges()
            For Each rowTemp In dt.Rows
                Select Case CStr(rowTemp("AppName"))
                    Case "Change01"
                        Dim OutPutStream As System.IO.MemoryStream
                        Dim formatter As New BinaryFormatter()
                        Dim m_report As New Treuhandsperre(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

                        OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(rowTemp("AuthorizationID")), "objReport")
                        m_report = DirectCast(formatter.Deserialize(OutPutStream), Treuhandsperre)

                        If OutPutStream Is Nothing Then
                            rowTemp("Ergebnis") = "Keine Daten für den Vorgang vorhanden."
                        Else
                            m_report.Change(Session("AppID").ToString, Session.SessionID.ToString)
                            If m_report.E_SUBRC > 0 Then
                                lblError.Text = "Beim Speichern Ihrer Daten sind Fehler aufgetreten."
                                rowTemp("Ergebnis") = m_report.E_MESSAGE
                            Else
                                Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                                For Each tmpRow As DataRow In m_report.Result.Rows

                                    If tmpRow("zurFreigabe").ToString = "X" Then
                                        logApp.CollectDetails("Treunehmer", CType(tmpRow("Treunehmer").ToString, Object), True)
                                        logApp.CollectDetails("Angefordert am", CType(Now.ToShortDateString, Object))
                                        logApp.CollectDetails("Fahrgestellnummer", CType(tmpRow("Fahrgestellnummer").ToString, Object))
                                        logApp.CollectDetails("Kennzeichen", CType(tmpRow("Kennzeichen").ToString, Object))
                                        If m_report.Aktion = "A" Then
                                            logApp.CollectDetails("Freigabe", "")
                                            logApp.CollectDetails("Abgelehnt", "X")
                                        Else
                                            logApp.CollectDetails("Freigabe", "X")
                                            logApp.CollectDetails("Abgelehnt", "")
                                        End If

                                        logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, tmpRow("Treunehmer").ToString, "Freigabe für Treunehmer " & tmpRow("Treunehmer").ToString & " erfolgreich autorisiert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                                    End If

                                Next
                                DeleteAuthorizationEntry(m_App.Connectionstring, CInt(rowTemp("AuthorizationID")))
                                rowTemp("Ergebnis") = "OK"
                            End If
                        End If
                    Case "Change02"
                        Dim OutPutStream As System.IO.MemoryStream
                        Dim formatter As New BinaryFormatter()
                        Dim CustomerObject As New SperreFreigabe(m_User, m_App, Session.SessionID.ToString, Session("AppID").ToString, "")
                        OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(rowTemp("AuthorizationID")), "CustomerObject")
                        CustomerObject = DirectCast(formatter.Deserialize(OutPutStream), SperreFreigabe)

                        If OutPutStream Is Nothing Then
                            rowTemp("Ergebnis") = "Keine Daten für den Vorgang vorhanden."
                        Else
                            CustomerObject.AutorizeCar(Session("AppID").ToString, Session.SessionID.ToString)
                            If CustomerObject.E_SUBRC > 0 Then
                                lblError.Text = "Beim Speichern Ihrer Daten sind Fehler aufgetreten."
                                rowTemp("Ergebnis") = CustomerObject.E_MESSAGE
                            Else
                                Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                                Dim dRows() As DataRow
                                dRows = CustomerObject.tblUpload.Select("EQUI_KEY = '" & CustomerObject.ReferenceforAut & "'")
                                If dRows.Length > 0 Then
                                    For Each ResultRow As DataRow In dRows
                                        logApp.CollectDetails("Vertrags. - / Fahrgestellnr.", CType(ResultRow("EQUI_KEY").ToString, Object), True)
                                        logApp.CollectDetails("Treunehmer", CType(CustomerObject.Treunehmer, Object))
                                        logApp.CollectDetails("Sachbearbeiter", CType(ResultRow("ERNAM").ToString, Object))
                                        logApp.CollectDetails("Sperrdatum", CType(ResultRow("SPERRDAT").ToString, Object))
                                        logApp.CollectDetails("Datum", CType(ResultRow("ERDAT").ToString, Object))
                                        If ResultRow("TREUH_VGA").ToString = "S" Then
                                            logApp.CollectDetails("Sperren", "X")
                                            logApp.CollectDetails("Entsperren", "")
                                        Else
                                            logApp.CollectDetails("Sperren", "")
                                            logApp.CollectDetails("Entsperren", "X")
                                        End If
                                        If ResultRow("TREUH_VGA").ToString = "S" Then
                                            logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CustomerObject.Treunehmer, "Sperrung für Treunehmer " & CustomerObject.Treunehmer & " erfolgreich autorisiert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                                        Else
                                            logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CustomerObject.Treunehmer, "Entsperrung für Treunehmer " & CustomerObject.Treunehmer & " erfolgreich autorisiert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                                        End If

                                    Next
                                End If
                                rowTemp("Ergebnis") = "OK"
                                DeleteAuthorizationEntry(m_App.Connectionstring, CInt(rowTemp("AuthorizationID")))
                            End If
                        End If

                    Case Else

                End Select
            Next
            dt.AcceptChanges()

            Session("SammelAutorisierungen") = dt
            GridView1.Visible = False
            FillGrid1(0)
            cmdSave.Visible = False
            cmdBack.Visible = True
        End Sub
#End Region

#Region "Methods"
        Private Sub FillGrid0(ByRef tblData As DataTable, ByRef dgShow As GridView, ByVal intPageIndex As Int32, ByVal strSort As String)
            m_intLineCount = 0
            If tblData.Rows.Count = 0 Then
                cmdBack.Visible = True
                cmdSave.Visible = True
                dgShow.Visible = False
                lblNoData.Visible = True
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                cmdSave.Enabled = False
            Else
                dgShow.Visible = True
                lblNoData.Visible = False
                cmdBack.Visible = True
                cmdSave.Visible = True
                Dim tmpDataView As New DataView()
                tmpDataView = tblData.DefaultView
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

                dgShow.PageIndex = intTempPageIndex

                dgShow.DataSource = tmpDataView
                dgShow.DataBind()

                lblNoData.Text = "Anzahl Vorgänge: " & tmpDataView.Count.ToString
                lblNoData.Visible = True

            End If
        End Sub

        Private Sub FillGrid1(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
            Dim dt As New DataTable()
            dt = CType(Session("SammelAutorisierungen"), DataTable)
            FillGrid0(dt, GridView2, intPageIndex, strSort)
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
            FillGrid0(dt, GridView1, intPageIndex, strSort)
        End Sub

        Private Sub WriteLog(ByVal strMessage As String, ByVal strHaendler As String, Optional ByVal strType As String = "APP")
            logApp.WriteEntry(strType, m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(strHaendler, 5), strMessage, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
        End Sub
#End Region

        Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
            Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString)
        End Sub
    End Class
End Namespace
