Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO

Namespace Treuhand
    Partial Public Class Change02_2
        Inherits System.Web.UI.Page

        Private m_User As Base.Kernel.Security.User
        Private m_App As Base.Kernel.Security.App
        Private CustomerObject As SperreFreigabe

        Protected WithEvents ucStyles As CKG.Portal.PageElements.Styles
        Protected WithEvents ucHeader As CKG.Portal.PageElements.Header

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            m_User = GetUser(Me)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            GetAppIDFromQueryString(Me)


            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            CustomerObject = CType(Session("CustomerObject"), SperreFreigabe)
            If Not IsPostBack Then
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("400")
                ddlPageSize.Items.Add("600")
                ddlPageSize.Items.Add("800")
                ddlPageSize.SelectedIndex = 0
                FillGrid(0, , True)
            End If

        End Sub

        Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

            Dim iSelCount As Integer
            iSelCount = CheckGrid()

            If iSelCount = 0 Then
                lblError.Text = "Bitte wählen Sie erst Fahrzeuge aus."
                FillGrid(GridView1.PageIndex)
            Else



                If m_User.Groups.ItemByID(m_User.GroupID).Authorizationright > 0 Then
                    For Each tmpRow As DataRow In CustomerObject.tblUpload.Rows
                        If tmpRow("SELECT").ToString = "99" Then
                            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                            logApp.CollectDetails("Vertrags. - / Fahrgestellnr.", CType(tmpRow("EQUI_KEY").ToString, Object), True)
                            logApp.CollectDetails("Treunehmer", CType(CustomerObject.Treunehmer, Object))
                            logApp.CollectDetails("Sachbearbeiter", CType(tmpRow("ERNAM").ToString, Object))
                            logApp.CollectDetails("Sperrdatum", CType(tmpRow("SPERRDAT").ToString, Object))
                            logApp.CollectDetails("Datum", CType(tmpRow("ERDAT").ToString, Object))
                            CustomerObject.ReferenceforAut = tmpRow("EQUI_KEY").ToString
                            If tmpRow("TREUH_VGA").ToString = "S" Then
                                logApp.CollectDetails("Sperren", "X")
                                logApp.CollectDetails("Entsperren", "")
                            Else
                                logApp.CollectDetails("Sperren", "")
                                logApp.CollectDetails("Entsperren", "X")
                            End If

                            'CustomerObject.FinforAut = tmpRow("Fahrgestellnummer").ToString
                            'Anwendung erfordert Autorisierung (Level>0)
                            Dim DetailArray(1, 2) As Object
                            Dim ms As MemoryStream
                            Dim formatter As BinaryFormatter
                            Dim b() As Byte

                            ms = New MemoryStream()
                            formatter = New BinaryFormatter()
                            formatter.Serialize(ms, CustomerObject)
                            b = ms.ToArray
                            ms = New IO.MemoryStream(b)
                            DetailArray(0, 0) = ms
                            DetailArray(0, 1) = "CustomerObject"

                            'Pruefen, ob schon in der Autorisierung.
                            Dim strInitiator As String = ""
                            Dim intAuthorizationID As Int32


                            m_App.CheckForPendingAuthorization(CInt(Session("AppID")), m_User.Organization.OrganizationId, m_User.KUNNR, tmpRow("EQUI_KEY").ToString, m_User.IsTestUser, strInitiator, intAuthorizationID)
                            If Not strInitiator.Length = 0 Then
                                'Fahrzeug wurde schon mal freigegeben und liegt zur Autorisierung vor
                                tmpRow("MESSAGE") = "liegt zur Autorisierung vor"

                            Else
                                If tmpRow("TREUH_VGA").ToString = "S" Then
                                    intAuthorizationID = WriteAuthorization(m_App.Connectionstring, CInt(Session("AppID")), m_User.UserName, m_User.Organization.OrganizationId, CustomerObject.Treunehmer, tmpRow("EQUI_KEY").ToString, "Sperren", "", m_User.IsTestUser, DetailArray)
                                    logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CustomerObject.Treunehmer, "Sperrung für Treunehmer " & CustomerObject.Treunehmer & " erfolgreich initiiert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)

                                Else
                                    intAuthorizationID = WriteAuthorization(m_App.Connectionstring, CInt(Session("AppID")), m_User.UserName, m_User.Organization.OrganizationId, CustomerObject.Treunehmer, tmpRow("EQUI_KEY").ToString, "Entsperren", "", m_User.IsTestUser, DetailArray)
                                    logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CustomerObject.Treunehmer, "Entsperrung für Treunehmer " & CustomerObject.Treunehmer & " erfolgreich initiiert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                                End If

                                tmpRow("MESSAGE") = "Aut."
                            End If
                        End If
                    Next
                    cmdSave.Visible = False
                    FillGrid(0)
                    lblNoData.Visible = True
                Else
                    CustomerObject.GiveCars(Session("AppID").ToString, Session.SessionID.ToString)
                    cmdSave.Visible = False
                    FillGrid2(0)
                    lblNoData.Visible = True
                    Dim ErrCount As Integer = 0
                    Dim RefCount As Integer = 0
                    Dim rows As DataRow() = CustomerObject.Fahrzeuge.Select("SUBRC <> 0")
                    ErrCount = rows.Length
                    For Each row As DataRow In rows
                        If row("MESSAGE").ToString.StartsWith("W") Then
                            ErrCount -= 1
                        ElseIf row("MESSAGE").ToString.StartsWith("I") Then
                            ErrCount -= 1
                            RefCount += 1
                        End If
                    Next

                    lblNoData.Text = "Es wurden " & iSelCount - ErrCount & " Fahrzeuge erfolgreich " & CustomerObject.SperrEnsperr & "!"
                    If ErrCount > 0 Then
                        lblNoData.Text &= "<br /> " & ErrCount & " Fahrzeuge mit Fehler!"
                    End If
                    If RefCount > 0 Then
                        lblNoData.Text &= "<br /> " & RefCount & " Referenznummern aktualisiert!"
                    End If
                    Session("CustomerObject") = CustomerObject
                End If

            End If


        End Sub

        Private Function CheckGrid() As Int32

            Dim chbox As CheckBox
            Dim lblKey As Label
            Dim intReturn As Int32 = 0
            Dim tmpRows As DataRow()

            For Each Row As GridViewRow In GridView1.Rows
                Dim strZZFAHRG As String = ""
                lblKey = CType(Row.Cells(0).FindControl("lblEQUI_KEY"), Label)
                strZZFAHRG = "EQUI_KEY = '" & lblKey.Text & "'"



                tmpRows = CustomerObject.tblUpload.Select(strZZFAHRG)
                If (tmpRows.Length > 0) Then
                    tmpRows(0).BeginEdit()
                    chbox = CType(Row.Cells(0).FindControl("chkAnfordern"), CheckBox)
                    If chbox.Checked Then           'anfordern
                        tmpRows(0).Item("SELECT") = "99"
                        intReturn += 1
                    End If

                    tmpRows(0).EndEdit()
                    CustomerObject.tblUpload.AcceptChanges()
                End If
            Next
            Session("CustomerObject") = CustomerObject
            Return intReturn
        End Function

        Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal fill As Boolean = False)
            Dim tmpDataView As New DataView()
            tmpDataView = CustomerObject.tblUpload.DefaultView
            tmpDataView.RowFilter = ""

            If tmpDataView.Count = 0 Then
                GridView1.Visible = False
                ddlPageSize.Visible = False
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                lblNoData.Visible = True
            Else
                ddlPageSize.Visible = True
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
                GridView1.PagerStyle.CssClass = "PagerStyle"
                GridView1.DataSource = tmpDataView
                GridView1.DataBind()

                If GridView1.PageCount > 1 Then
                    GridView1.PagerSettings.Visible = True
                Else
                    GridView1.PagerSettings.Visible = False
                End If
                If CustomerObject.tblUpload.Select("Message <> ''").Length > 0 Then
                    GridView1.Columns(7).Visible = True
                Else
                    GridView1.Columns(7).Visible = False
                End If
            End If
        End Sub

        Private Sub FillGrid2(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal fill As Boolean = False)
            Dim tmpDataView As New DataView()
            tmpDataView = CustomerObject.Fahrzeuge.DefaultView

            Dim WidthErrors As Boolean = False

            If CustomerObject.Fahrzeuge.Select("SUBRC <> 0").Length > 0 Then
                WidthErrors = True
            End If

            If tmpDataView.Count = 0 Then
                GridView1.Visible = False
                ddlPageSize.Visible = False
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                lblNoData.Visible = True
            Else
                ddlPageSize.Visible = True
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

                'GridView1.PageIndex = intTempPageIndex
                If WidthErrors Then
                    'Dim Subrc As New BoundField()
                    'Subrc.DataField = "SUBRC"

                    'GridView1.Columns.Add(Subrc)

                    Dim Message As New BoundField()
                    Message.DataField = "MESSAGE"
                    Message.HeaderText = "col_Fehlertext"
                    GridView1.Columns.Add(Message)
                End If

                GridView1.PageIndex = intTempPageIndex
                GridView1.PagerStyle.CssClass = "PagerStyle"
                GridView1.DataSource = tmpDataView
                GridView1.DataBind()

                If GridView1.PageCount > 1 Then
                    GridView1.PagerSettings.Visible = True
                Else
                    GridView1.PagerSettings.Visible = False
                End If
                GridView1.Columns(0).Visible = False
                GridView1.Columns(5).Visible = False
                GridView1.Columns(6).Visible = True
            End If
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

        Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
            FillGrid(GridView1.PageIndex, e.SortExpression)
        End Sub

        Protected Sub chkAnfordern_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

            Dim tmpGridRow As GridViewRow = CType(CType(sender, CheckBox).Parent.Parent, GridViewRow)
            If CType(sender, CheckBox).Checked Then
                CustomerObject.tblUpload.Select("EQUI_KEY='" & CType(tmpGridRow.FindControl("lblEQUI_KEY"), Label).Text & "'")(0)("SELECT") = "99"
            Else
                CustomerObject.tblUpload.Select("EQUI_KEY='" & CType(tmpGridRow.FindControl("lblEQUI_KEY"), Label).Text & "'")(0)("SELECT") = ""
            End If
        End Sub

        Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
            Response.Redirect("Change02.aspx?AppID=" & Session("AppID").ToString)
        End Sub

        Protected Sub ddlPageSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPageSize.SelectedIndexChanged
            GridView1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
            FillGrid(GridView1.PageIndex)
        End Sub
    End Class
End Namespace
