Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Change80_5
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

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objSuche As Search
    Private objHaendler As ALD_1

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ucStyles As Styles
    Private versandart As String
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Private logApp As Base.Kernel.Logging.Trace

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If (Session("objHaendler") Is Nothing) OrElse (Not IsPostBack) Then
                objHaendler = New ALD_1(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
            Else
                objHaendler = CType(Session("objHaendler"), ALD_1)
            End If

            If Not IsPostBack Then

                logApp = New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 2

                'Kopfdaten1.Kontingente = objHaendler.Kontingente
                loadData()
                FillGrid(0, , True)
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub loadData()

        Dim status As String = ""
        Dim view As DataView

        objHaendler.getAutorizationData(status)
        objHaendler.Fahrzeuge.Columns.Add("MANDT", System.Type.GetType("System.String"))

        Dim tmpRow As DataRow
        For Each tmpRow In objHaendler.Fahrzeuge.Rows
            tmpRow("MANDT") = "99"
        Next

        view = objHaendler.Fahrzeuge.DefaultView

        DataGrid1.DataSource = view
        DataGrid1.DataBind()
        Session("objHaendler") = objHaendler    'Daten merken
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim tmpRow As DataRow
        Dim status As String = ""
        Dim statusAuth As String = ""
        Dim tblAuthorizationSets As New DataTable()
        Dim row As DataRow()

        Dim blnFound As Boolean = True

        CheckGrid()

        Dim intSelect As Int32 = 0
        For Each tmpRow In objHaendler.Fahrzeuge.Rows

            '§§§ JVE 30.06.2006 ###########################################################################
            'Zunächst prüfen, ob Datensatz noch in der Autorisierungstabelle...
            'Wenn nicht, wurde er parallel von einem anderen Benutzer freigegben / storniert.
            'Dann darf der Vorgang nicht erneut freigegeben oder storniert werden!!

            objHaendler.readAllAuthorizationSets(tblAuthorizationSets, statusAuth)

            If (statusAuth <> String.Empty) Then
                tmpRow("STATUS") = "Fehler beim Lesen der Authorisierungsinformationen."
            Else
                'Vorgang suchen
                row = tblAuthorizationSets.Select("equipment = '" & CStr(tmpRow("m_equ")) & "'")
                If (row.Length = 0) Then    'Wenn nicht gefunden, Fehler!
                    statusAuth = "Vorgang bereits freigegeben / storniert!"
                    tmpRow("STATUS") = statusAuth
                End If
            End If
            '##############################################################################################

            If (statusAuth = String.Empty) Then

                If CStr(tmpRow("MANDT")) = "99" Then
                    blnFound = True
                    intSelect += 1

                    With objHaendler
                        '.KUNNR = Right("0000000000" & m_User.KUNNR, 10)
                        .Haendlernummer = Right("0000000000" & m_User.Reference, 10)
                        .HalterNummer = CStr(tmpRow("m_strHalterNummer"))
                        .StandortNummer = CStr(tmpRow("m_strStandortNummer"))
                        .ZielFirma = CStr(tmpRow("m_strZielFirma"))
                        .ZielFirma2 = CStr(tmpRow("m_strZielFirma2"))
                        .ZielStrasse = CStr(tmpRow("m_strZielStrasse"))
                        .ZielHNr = CStr(tmpRow("m_strZielHNr"))
                        .ZielPLZ = CStr(tmpRow("m_strZielPLZ"))
                        .ZielOrt = CStr(tmpRow("m_strZielOrt"))
                        .ZielLand = CStr(tmpRow("m_strZielLand"))
                        .Auf = CStr(tmpRow("m_strAuf"))
                        .Betreff = CStr(tmpRow("m_strBetreff"))
                        .SucheFahrgestellNr = CStr(tmpRow("m_strSucheFahrgestellNr"))
                        .SucheKennzeichen = CStr(tmpRow("m_strSucheKennzeichen"))
                        .SucheLeasingvertragsNr = CStr(tmpRow("m_strSucheLeasingvertragsNr"))
                        .VersandAdresse_ZE = CStr(tmpRow("m_versandadr_ZE"))
                        .VersandAdresse_ZS = CStr(tmpRow("m_versandadr_ZS"))
                        .VersandAdresseText = CStr(tmpRow("m_versandadrtext"))
                        .Versicherung = CStr(tmpRow("m_versicherung"))
                        .Materialnummer = CStr(tmpRow("m_material"))
                        .ScheinSchildernummer = CStr(tmpRow("m_schein"))
                        .Versandart = CStr(tmpRow("m_abckz"))
                        .Equimpent = CStr(tmpRow("m_equ"))
                        .Kennzeichen = CStr(tmpRow("m_kennz"))
                        .TIDNr = CStr(tmpRow("m_tidnr"))
                        .LizenzNr = CStr(tmpRow("m_liznr"))
                        .VersandGrund = CStr(tmpRow("m_versgrund"))
                        .VersandGrundText = CStr(tmpRow("m_versgrundText"))
                    End With

                    objHaendler.AnfordernSAP()
                    tmpRow("MANDT") = ""

                    If (Not objHaendler.Versandart = "3") And ((objHaendler.Auftragsnummer Is Nothing) OrElse (objHaendler.Auftragsnummer.Length = 0)) Then
                        tmpRow("STATUS") = "Fehler: " & objHaendler.Message & "<br>Grund: " & objHaendler.Auftragsstatus
                    Else
                        tmpRow("STATUS") = objHaendler.Auftragsstatus
                        'Aufträge löschen (sql-server!)
                        objHaendler.clearDB(CInt(tmpRow("ID")), status)
                        If (status <> String.Empty) Then
                            tmpRow("STATUS") = status
                        End If
                    End If
                End If
            End If
        Next

        If Not blnFound Then
            lblError.Text = "Keine Fahrzeuge ausgewählt."
        End If

        FillGrid(0)
        If intSelect < objHaendler.Fahrzeuge.Rows.Count Then
            cmdSave.Enabled = True
        Else
            cmdSave.Enabled = False
        End If
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal fill As Boolean = False)
        Dim tmpDataView As New DataView()
        tmpDataView = objHaendler.Fahrzeuge.DefaultView
        tmpDataView.RowFilter = ""

        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
            ddlPageSize.Visible = False
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            lblNoData.Visible = True
            'ShowScript.Visible = False
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

            DataGrid1.CurrentPageIndex = intTempPageIndex

            DataGrid1.DataSource = tmpDataView
            DataGrid1.DataBind()

            lblNoData.Text = "Es wurde(n) " & tmpDataView.Count.ToString & " KFZ-Brief(e) gefunden."
            lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If

        End If
    End Sub


    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        CheckGrid()
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        CheckGrid()
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        CheckGrid()
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        Dim id As Integer
        Dim tmpDataview As DataView
        Dim status As String = ""
        Dim sqlConn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim row As DataRow()
        Dim tblAuthorizationSets As New DataTable()

        '§§§ JVE 30.06.2006 ###########################################################################
        'Zunächst prüfen, ob Datensatz noch in der Autorisierungstabelle...
        'Wenn nicht, wurde er parallel von einem anderen Benutzer freigegben / storniert.
        'Dann darf der Vorgang nicht erneut freigegeben oder storniert werden!!

        objHaendler.readAllAuthorizationSets(tblAuthorizationSets, status)

        If (status <> String.Empty) Then
            lblError.Text = "Authorisierungsinformationen konnten nicht ermittelt werden. Vorgang nicht abgeschlossen."
        Else
            'Vorgang suchen
            row = tblAuthorizationSets.Select("equipment = '" & e.Item.Cells(38).Text.ToString & "'")
            If (row.Length = 0) Then    'Wenn nicht gefunden, Fehler!
                status = "Dieser Vorgang wurde bereits freigegeben / storniert."
                lblError.Text = status
            End If
        End If

        If (status <> String.Empty) Then
            Exit Sub
        End If
        '##############################################################################################

        Try
            'FREIGABE----------------------------------------------
            If e.CommandName = "Freigeben" Then
                id = CType(e.Item.Cells(1).Text(), Int32) 'SQL-Datensatz ID
                With objHaendler
                    '.KUNNR = Right("0000000000" & m_User.KUNNR, 10)
                    .Haendlernummer = Right("0000000000" & m_User.Reference, 10)
                    .HalterNummer = e.Item.Cells(13).Text.ToString
                    .StandortNummer = e.Item.Cells(14).Text.ToString
                    .ZielFirma = e.Item.Cells(15).Text.ToString
                    .ZielFirma2 = e.Item.Cells(16).Text.ToString
                    .ZielStrasse = e.Item.Cells(17).Text.ToString
                    .ZielHNr = e.Item.Cells(18).Text.ToString
                    .ZielPLZ = e.Item.Cells(19).Text.ToString
                    .ZielOrt = e.Item.Cells(20).Text.ToString
                    .ZielLand = e.Item.Cells(21).Text.ToString
                    .Auf = e.Item.Cells(22).Text.ToString
                    .Betreff = e.Item.Cells(23).Text.ToString
                    .SucheFahrgestellNr = e.Item.Cells(25).Text.ToString
                    .SucheKennzeichen = e.Item.Cells(26).Text.ToString
                    .SucheLeasingvertragsNr = e.Item.Cells(27).Text.ToString
                    .VersandAdresse_ZE = e.Item.Cells(31).Text.ToString
                    .VersandAdresse_ZS = e.Item.Cells(32).Text.ToString
                    .VersandAdresseText = e.Item.Cells(33).Text.ToString
                    .Versicherung = e.Item.Cells(34).Text.ToString
                    .Materialnummer = e.Item.Cells(35).Text.ToString
                    .ScheinSchildernummer = e.Item.Cells(36).Text.ToString
                    .Versandart = e.Item.Cells(37).Text.ToString
                    .Equimpent = e.Item.Cells(38).Text.ToString
                    .Kennzeichen = e.Item.Cells(39).Text.ToString
                    .TIDNr = e.Item.Cells(40).Text.ToString
                    .LizenzNr = e.Item.Cells(41).Text.ToString
                    .VersandGrund = e.Item.Cells(42).Text.ToString
                    .VersandGrundText = e.Item.Cells(43).Text.ToString
                End With

                objHaendler.AnfordernSAP()

                tmpDataview = objHaendler.Result.DefaultView
                If (Not objHaendler.Versandart = "3") And ((objHaendler.Auftragsnummer Is Nothing) OrElse (objHaendler.Auftragsnummer.Length = 0)) Then
                    row = objHaendler.Result.Select("m_equ=" & e.Item.Cells(38).Text.ToString)
                    row(0)("STATUS") = "Fehler: " & objHaendler.Message & "<br>Grund: " & objHaendler.Auftragsstatus
                    row(0)("MANDT") = ""
                Else
                    row = objHaendler.Result.Select("m_equ=" & e.Item.Cells(38).Text.ToString)
                    row(0)("STATUS") = objHaendler.Auftragsstatus
                    'Aufträge löschen (sql-server!)
                    objHaendler.clearDB(id, status)
                    If (status <> String.Empty) Then
                        row(0)("STATUS") = status
                    End If
                    row(0)("MANDT") = ""
                End If
                FillGrid(0)
            End If

            'STORNO----------------------------------------------
            'Dim index As Integer
            Dim sqlDelete As String
            Dim sqlCommand As New SqlClient.SqlCommand()

            If e.CommandName = "delete" Then
                id = CType(e.Item.Cells(1).Text(), Int32) 'SQL-Datensatz ID

                sqlDelete = "DELETE FROM AuthorizationALD WHERE id = " & id
                sqlCommand.CommandText = sqlDelete
                sqlCommand.Connection = sqlConn
                sqlConn.Open()
                sqlCommand.ExecuteScalar()

                row = objHaendler.Result.Select("id=" & e.Item.Cells(1).Text.ToString)
                objHaendler.Result.Rows.Remove(row(0))
                FillGrid(0)
            End If
            'logApp.UpdateEntry("APP", Session("AppID").ToString, "Endgültiger Briefversand (Freigabe)")
        Catch ex As Exception
            Dim strMessage As String = "Ausführen der Aktion"
            If e.CommandName = "Freigeben" Then
                strMessage = "Freigeben"
            ElseIf e.CommandName = "delete" Then
                strMessage = "Löschen"
            End If
            lblError.Text = "Beim " & strMessage & " ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler im Report <Nicht durchführbare Zulassungen>. Fehler: " & ex.Message & ")")
        Finally
            sqlConn.Close()
            sqlConn.Dispose()
        End Try
    End Sub

    Private Function CheckGrid() As Int32
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim chbox As CheckBox
        Dim control As Control
        Dim blnScriptFound As Boolean = False
        Dim intReturn As Int32 = 0
        Dim tmpRows As DataRow()

        For Each item In DataGrid1.Items
            Dim strZZFAHRG As String = ""
            strZZFAHRG = "m_equ = '" & item.Cells(38).Text & "'"

            For Each cell In item.Cells
                For Each control In cell.Controls
                    If TypeOf control Is CheckBox Then
                        chbox = CType(control, CheckBox)

                        tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                        If (tmpRows.Length > 0) Then
                            tmpRows(0).BeginEdit()
                            Select Case chbox.ID
                                Case "chkSelect"
                                    If chbox.Checked Then           'anfordern
                                        tmpRows(0).Item("MANDT") = "99"
                                        intReturn += 1
                                    Else
                                        tmpRows(0).Item("MANDT") = ""
                                    End If
                            End Select
                            tmpRows(0).EndEdit()
                            objHaendler.Fahrzeuge.AcceptChanges()
                        End If
                    End If
                Next
            Next
        Next
        Session("objHaendler") = objHaendler
        Return intReturn
    End Function

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change80_5.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 29.04.09   Time: 14:58
' Updated in $/CKAG/Applications/appALD/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 17.04.08   Time: 15:15
' Created in $/CKAG/Applications/appald/Forms
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 6.12.07    Time: 13:15
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Forms
' ITA: 1440
' 
' *****************  Version 15  *****************
' User: Uha          Date: 20.06.07   Time: 15:25
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 14  *****************
' User: Uha          Date: 5.03.07    Time: 16:59
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Forms
' 
' ************************************************
