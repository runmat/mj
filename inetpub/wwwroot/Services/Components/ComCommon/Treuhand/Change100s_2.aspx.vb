Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Components.ComCommon.Treuhand
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO
Imports CKG.Base.Business

Namespace Treuhand

    Partial Public Class Change100s_2
        Inherits System.Web.UI.Page

#Region "Declarations"
        Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation

        Private m_User As Base.Kernel.Security.User
        Private m_App As Base.Kernel.Security.App
        Private CustomerObject As SperreFreigabe
#End Region

#Region "PageEvents"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            m_User = GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte
            FormAuth(Me, m_User)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            GetAppIDFromQueryString(Me) 'füllen page.Session("AppID")
            GridNavigation1.setGridElment(GridView1)
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            CustomerObject = CType(Session("CustomerObject"), SperreFreigabe)


            If Not IsPostBack Then
                FillGrid(0, , True)
                CheckUpload()
                GridNavigation1.PageSizeIndex = 0
            End If

        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            SetEndASPXAccess(Me)
            HelpProcedures.FixedGridViewCols(GridView1)
            FormatGridviewPage(True)

        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub

#End Region

#Region "Methods"

        ''' <summary>
        ''' Gridview prüfen
        ''' </summary>
        ''' <returns>Anzahl der Gridviewzeilen</returns>
        ''' <remarks></remarks>
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
                Result.Visible = False
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                lblNoData.Visible = True
            Else
                Dim intTempPageIndex As Int32 = intPageIndex
                Dim strTempSort As String = ""
                Dim strDirection As String = ""
                lblNoData.Text = "Bitte wählen Sie die Vorgänge ab, die Sie nicht absenden wollen."
                lblNoData.Visible = True
                Result.Visible = True
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

                If tmpDataView.Count = 0 Then
                    GridView1.Visible = False
                    lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                    lblNoData.Visible = True
                Else
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

                    GridView1.PageIndex = intTempPageIndex
                    GridView1.PagerStyle.CssClass = "PagerStyle"
                    GridView1.DataSource = tmpDataView
                    GridView1.DataBind()

                    GridView1.Columns(0).Visible = False
                    GridView1.Columns(5).Visible = False
                    GridView1.Columns(6).Visible = True
                End If
        End Sub

        Private Function CheckUpload() As Boolean

            Dim returnString As String = String.Empty

            Dim errCode As Integer = 0
            Dim chbox As CheckBox
            Dim intReturn As Int32 = 0
            Dim errSperrdat As String = String.Empty
            Dim errBapi As String = String.Empty
            Dim selRowCount As Integer = 0
            Dim dicError As Dictionary(Of String, String())
            Dim isWarning As Boolean = False
            Dim isError As Boolean = False
            Dim sperrPruef As SperrPruefung

            Try

                sperrPruef = New SperrPruefung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")


                If CustomerObject.ZSelect.Equals("TG") Then

                End If


                sperrPruef.Treuhandgeber = CustomerObject.TREU
                sperrPruef.Auftraggeber = CustomerObject.AG


                sperrPruef.IsSperren = CustomerObject.IsSperren

                'Zurücksetzen der Formatierung für neue prüfung
                GridView1.Columns(GridView1.Columns.Count - 1).Visible = False


                For Each row As DataRow In CustomerObject.tblUpload.Rows

                    If row("SELECT").Equals("99") Then
                        selRowCount += 1
                    End If

                Next

                'Setzen der standartwerte
                For Each Row As GridViewRow In GridView1.Rows
                    chbox = CType(Row.Cells(0).FindControl("chkAnfordern"), CheckBox)
                    Row.BackColor = Nothing
                    CType(Row.FindControl("lblFehlertext"), Label).Text = ""

                Next

                'verhalten wenn nichts selektiert
                If selRowCount <= 0 Then
                    InfoHead.InnerText = "Information"
                    GridView1.Columns(GridView1.Columns.Count - 1).Visible = False
                    InfoText.Text = "Bitte wählen Sie erst Fahrzeuge aus."
                    InfoText.ForeColor = Drawing.Color.Black
                    cmdSave.Visible = False
                    cmdCheck.Visible = True
                    Session.Add("dicError", Nothing)
                    Return False
                End If


                'GESAMMTE TABELLE gegen Bapi Prüfen
                dicError = sperrPruef.CheckAll(Me, CustomerObject.tblUpload)
                Session.Add("dicError", dicError)

                'Prüfen ob im gesamter Tabelle ein Fehler aufgetreten ist
                For Each elem As String() In dicError.Values
                    If elem(0).Equals("F") Then
                        isError = True
                        Exit For 'Abbruch sobald ein Fehler aufgetaucht ist
                    ElseIf elem(0).Equals("W") Then
                        isWarning = True
                    End If
                Next

                FormatGridviewPage(False)


                'Setzen der Information je nach Fehlertyp 

                If isError Then
                    GridView1.Columns(GridView1.Columns.Count - 1).Visible = True
                    InfoHead.InnerText = "Achtung!"
                    InfoText.ForeColor = Drawing.Color.Red
                    InfoText.Text = "Der Upload enthält Fehler"
                    cmdSave.Visible = False
                    Return False

                ElseIf isWarning Then
                    GridView1.Columns(GridView1.Columns.Count - 1).Visible = True
                    InfoHead.InnerText = "Information"
                    InfoText.ForeColor = Drawing.Color.Black
                    InfoText.Text = "Der Upload enthält Warnungen"
                    cmdSave.Visible = True
                    Return True

                Else
                    GridView1.Columns(GridView1.Columns.Count - 1).Visible = False
                    InfoHead.InnerText = "Information"
                    InfoText.ForeColor = Drawing.Color.DarkGreen
                    InfoText.Text = "Prüfung erfolgreich!"
                    cmdSave.Visible = True
                    'cmdCheck.Visible = False
                    Return True
                End If

            Catch ex As Exception
                lblError.Text = ex.Message
                Return False
            End Try

        End Function

        Private Function FormatGridviewPage(ByVal checkAllRows As Boolean) As Boolean

            Dim equikey As String
            Dim errors As String()
            Dim chbox As CheckBox
            Dim dicError As Dictionary(Of String, String())


            dicError = Session("DICERROR")

            If dicError Is Nothing Then

                Return False
            End If


            For Each Row As GridViewRow In GridView1.Rows

                chbox = CType(Row.FindControl("chkAnfordern"), CheckBox)

                'prüfen ob selektierte spalte (EQUI_KEY) im Fehler Dictionary ist ggf. Fehler ausgeben/formtieren 
                If chbox.Checked Then

                    equikey = CType(Row.FindControl("lblEQUI_KEY"), Label).Text

                    If Not dicError.ContainsKey(equikey) Then
                        Continue For
                    End If

                    errors = dicError(equikey)

                    If Not errors Is Nothing Then

                        CType(Row.FindControl("lblFehlertext"), Label).Text = errors(1)

                        If Not String.IsNullOrEmpty(errors(2)) Then

                            'Achtung beim Entsperren darf der Fehler 12 nicht berücksichtigt werden.
                            If CType(Row.FindControl("lblTREUH_VGA2"), Label).Visible And errors(2) = 12 Then
                                Continue For
                            End If

                        End If


                        CType(Row.FindControl("lblFehlertext"), Label).Text = errors(1)

                        Select Case errors(0)

                            Case "F"
                                'isError = True
                                Row.BackColor = Drawing.Color.LightCoral
                                Row.BorderStyle = BorderStyle.Solid
                                Row.BorderWidth = 1
                                Row.BorderColor = Drawing.Color.LightGray

                            Case "W"
                                'isWarning = True
                                Row.BackColor = Drawing.Color.LightYellow
                                Row.BorderStyle = BorderStyle.Solid
                                Row.BorderWidth = 1
                                Row.BorderColor = Drawing.Color.LightGray

                            Case Else

                        End Select


                    End If

                End If 'end if checked

            Next 'end for each



        End Function

#End Region

#Region "Events"

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

                    CustomerObject.GiveCars(Me.Page, Session("AppID").ToString, Session.SessionID.ToString)
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

                    cmdCheck.Visible = False
                    pnInfo.Visible = False


                End If

                GridView1.Columns(1).Visible = False
                GridView1.Columns(GridView1.Columns.Count - 1).Visible = False

            End If


        End Sub

        Private Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
            FillGrid(e.NewPageIndex)
            CheckUpload()
        End Sub

        Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
            GridView1.PageIndex = PageIndex
            FillGrid(PageIndex)
            CheckUpload()
        End Sub

        Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
            FillGrid(0)
            CheckUpload()
        End Sub

        Private Sub Gridview1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
            FillGrid(GridView1.PageIndex, e.SortExpression)
        End Sub


        Protected Sub chkAnfordern_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

            If cmdSave.Visible Then
                cmdSave.Visible = False
                cmdCheck.Visible = True
            End If

            Dim tmpGridRow As GridViewRow = CType(CType(sender, CheckBox).Parent.Parent, GridViewRow)

            If CType(sender, CheckBox).Checked Then
                CustomerObject.tblUpload.Select("ID='" & CType(tmpGridRow.FindControl("lblID"), Label).Text & "'")(0)("SELECT") = "99"
            Else
                CustomerObject.tblUpload.Select("ID='" & CType(tmpGridRow.FindControl("lblID"), Label).Text & "'")(0)("SELECT") = ""
            End If


        End Sub

        Protected Sub cmdCheck_Click(sender As Object, e As EventArgs) Handles cmdCheck.Click

            CheckUpload()

        End Sub

        Protected Sub GridView1_SelectedIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles GridView1.SelectedIndexChanging

        End Sub

        Protected Sub GridView1_PageIndexChanged(sender As Object, e As EventArgs) Handles GridView1.PageIndexChanged

        End Sub

        Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
            Response.Redirect("Change100s.aspx?AppID=" & Session("AppID").ToString)
        End Sub
#End Region


    End Class

End Namespace