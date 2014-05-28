Imports CKG.Base.Business
Imports CKG.Base.Kernel

Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change01s_5
    Inherits System.Web.UI.Page
#Region "Declarations"
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation
    'Private objSuche As Search
    Private objHaendler As Versand1
    Private versandart As String
    Private logApp As Base.Kernel.Logging.Trace
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)

        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        GridNavigation1.setGridElment(GridView1)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        m_App = New Base.Kernel.Security.App(m_User)

        If (Session("objHaendler") Is Nothing) OrElse (Not IsPostBack) Then
            objHaendler = New Versand1(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        Else
            objHaendler = CType(Session("objHaendler"), Versand1)
        End If

        If Not IsPostBack Then

            logApp = New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)


            loadData()
            FillGrid(0, , True)
        End If

    End Sub

    Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Dim id As Integer
        Dim tmpDataview As DataView
        Dim status As String = ""
        Dim sqlConn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim row As DataRow()
        Dim index As Integer

        Try
            'FREIGABE----------------------------------------------
            If e.CommandName = "Freigeben" Then

                index = CType(e.CommandArgument, Integer)

                Dim GridRow As GridViewRow = GridView1.Rows(index)

                id = CType(GridRow.Cells(1).Text(), Int32) 'SQL-Datensatz ID
                With objHaendler
                    .KUNNR = Right("0000000000" & m_User.KUNNR, 10)
                    .Haendlernummer = Right("0000000000" & m_User.Reference, 10)
                    .HalterNummer = GridRow.Cells(13).Text.ToString
                    .StandortNummer = GridRow.Cells(14).Text.ToString
                    .ZielFirma = GridRow.Cells(15).Text.ToString
                    .ZielFirma2 = GridRow.Cells(16).Text.ToString
                    .ZielStrasse = GridRow.Cells(17).Text.ToString
                    .ZielHNr = GridRow.Cells(18).Text.ToString
                    .ZielPLZ = GridRow.Cells(19).Text.ToString
                    .ZielOrt = GridRow.Cells(20).Text.ToString
                    .ZielLand = GridRow.Cells(21).Text.ToString
                    .Auf = GridRow.Cells(22).Text.ToString
                    .Betreff = GridRow.Cells(23).Text.ToString
                    .SucheFahrgestellNr = GridRow.Cells(25).Text.ToString
                    .SucheKennzeichen = GridRow.Cells(26).Text.ToString
                    .SucheLeasingvertragsNr = GridRow.Cells(27).Text.ToString
                    .VersandAdresse_ZE = GridRow.Cells(31).Text.ToString
                    .VersandAdresse_ZS = GridRow.Cells(32).Text.ToString
                    .VersandAdresseText = GridRow.Cells(33).Text.ToString
                    .Versicherung = GridRow.Cells(34).Text.ToString
                    .Materialnummer = GridRow.Cells(35).Text.ToString
                    .ScheinSchildernummer = GridRow.Cells(36).Text.ToString
                    .Versandart = GridRow.Cells(37).Text.ToString
                    .Equimpent = GridRow.Cells(38).Text.ToString
                    .Kennzeichen = GridRow.Cells(39).Text.ToString
                    .TIDNr = GridRow.Cells(40).Text.ToString
                    .LizenzNr = GridRow.Cells(41).Text.ToString
                    .VersandGrund = GridRow.Cells(42).Text.ToString
                    .VersandGrundText = GridRow.Cells(43).Text.ToString
                End With

                objHaendler.AnfordernSAP()

                tmpDataview = objHaendler.Result.DefaultView
                If (objHaendler.Auftragsnummer Is Nothing) OrElse (objHaendler.Auftragsnummer.Length = 0) Then
                    row = objHaendler.Result.Select("m_equ=" & GridRow.Cells(38).Text.ToString)
                    row(0)("STATUS") = "Fehler: " & objHaendler.Message & "<br>Grund: " & objHaendler.Auftragsstatus
                    row(0)("MANDT") = ""
                Else
                    row = objHaendler.Result.Select("m_equ=" & GridRow.Cells(38).Text.ToString)
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

                Dim GridRow As GridViewRow = GridView1.Rows(index)

                id = CType(GridRow.Cells(1).Text(), Int32) 'SQL-Datensatz ID

                sqlDelete = "DELETE FROM AuthorizationLeaseplan WHERE id = " & id
                sqlCommand.CommandText = sqlDelete
                sqlCommand.Connection = sqlConn
                sqlConn.Open()
                sqlCommand.ExecuteScalar()

                row = objHaendler.Result.Select("id=" & GridRow.Cells(1).Text.ToString)
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
            Throw ex
        Finally
            sqlConn.Close()
            sqlConn.Dispose()
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim tmpRow As DataRow
        Dim status As String = ""

        Dim blnFound As Boolean = True

        CheckGrid()

        Dim intSelect As Int32 = 0
        For Each tmpRow In objHaendler.Fahrzeuge.Rows
            If CStr(tmpRow("MANDT")) = "99" Then
                blnFound = True
                intSelect += 1

                With objHaendler
                    .KUNNR = Right("0000000000" & m_User.KUNNR, 10)
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

                If (objHaendler.Auftragsnummer Is Nothing) OrElse (objHaendler.Auftragsnummer.Length = 0) Then
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

    Private Function CheckGrid() As Int32
        Dim cell As TableCell
        Dim chbox As CheckBox
        Dim control As Control
        Dim blnScriptFound As Boolean = False
        Dim intReturn As Int32 = 0
        Dim tmpRows As DataRow()


        For Each Row As GridViewRow In GridView1.Rows
            Dim strZZFAHRG As String = ""
            strZZFAHRG = "m_equ = '" & Row.Cells(38).Text & "'"

            For Each cell In Row.Cells
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

#End Region

#Region "Methods"
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

        GridView1.DataSource = view
        GridView1.DataBind()
        Session("objHaendler") = objHaendler    'Daten merken
    End Sub


    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal fill As Boolean = False)
        Dim tmpDataView As New DataView()
        tmpDataView = objHaendler.Fahrzeuge.DefaultView
        tmpDataView.RowFilter = ""

        If tmpDataView.Count = 0 Then
            GridView1.Visible = False
            'ddlPageSize.Visible = False
            lblError.Text = "Keine Daten zur Anzeige gefunden."
            'lblNoData.Visible = True
            'ShowScript.Visible = False
        Else
            'ddlPageSize.Visible = True
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

            lblError.Text = "Es wurde(n) " & tmpDataView.Count.ToString & " KFZ-Brief(e) gefunden."
            'lblNoData.Visible = True

            If GridView1.PageCount > 1 Then
                'GridView1.PagerStyle.CssClass = "PagerStyle"
                GridView1.DataBind()
                'GridView1.PagerStyle.Visible = True
                'Else
                'GridView1.PagerStyle.Visible = False
            End If

        End If
    End Sub


#End Region

End Class

' ************************************************
' $History: Change01s_5.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 24.09.09   Time: 10:44
' Updated in $/CKAG2/Services/Components/ComCommon
' ITA: 3112
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 24.09.09   Time: 9:47
' Created in $/CKAG2/Services/Components/ComCommon
' 
