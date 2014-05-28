Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Change205_5
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
    Private objHaendler As SixtLease_02

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents calZul As System.Web.UI.WebControls.Calendar
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
                objHaendler = New SixtLease_02(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
            Else
                objHaendler = CType(Session("objHaendler"), SixtLease_02)
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

        view = objHaendler.Result.DefaultView

        DataGrid1.DataSource = view
        DataGrid1.DataBind()
        Session("objHaendler") = objHaendler    'Daten merken
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        '    'CheckGrid()

        '    Dim tmpDataView As New DataView()
        '    tmpDataView = objHaendler.Fahrzeuge.DefaultView

        '    tmpDataView.RowFilter = "MANDT = '99'"
        '    Dim intFahrzeugBriefe As Int32 = tmpDataView.Count
        '    tmpDataView.RowFilter = ""

        '    If intFahrzeugBriefe = 0 Then
        '        lblError.Text = "Bitte wählen Sie erst Fahrzeuge zur Anforderung aus."
        '        FillGrid(DataGrid1.CurrentPageIndex)
        '    Else
        '        Session("objHaendler") = objHaendler
        '        Response.Redirect("Change205_3.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart)
        '    End If
    End Sub

    'Private Sub DoSubmit()
    '    CheckGrid()

    '    Dim tmpDataView As New DataView()
    '    tmpDataView = objHaendler.Fahrzeuge.DefaultView

    '    tmpDataView.RowFilter = "MANDT <> '0'"
    '    Dim intFahrzeugBriefe As Int32 = tmpDataView.Count
    '    tmpDataView.RowFilter = ""

    '    If intFahrzeugBriefe = 0 Then
    '        'Kopfdaten1.Message = "Bitte wählen Sie erst Fahrzeuge zum Zulassen aus."
    '        FillGrid(DataGrid1.CurrentPageIndex)
    '    Else
    '        Session("objHaendler") = objHaendler
    '        'objHaendler.zuldatum = CType(txtZulDatum.Text, Date) 'für HEZ Zulassungsdatum setzen!
    '        Response.Redirect("Change205_3.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart)
    '    End If
    'End Sub

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
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub


    Private Sub Linkbutton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        calZul.Visible = True
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        Dim id As Integer
        Dim tmpDataview As DataView
        Dim status As String = ""
        Dim sqlConn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim row As DataRow()

        Try
            'FREIGABE----------------------------------------------
            If e.CommandName = "Freigeben" Then
                id = CType(e.Item.Cells(0).Text(), Int32) 'SQL-Datensatz ID
                With objHaendler
                    .KUNNR = Right("0000000000" & m_User.KUNNR, 10)
                    .Haendlernummer = Right("0000000000" & m_User.Reference, 10)    'Addressnr. Haendler
                    .HalterNummer = ""                                              'Addressnr. Halter (wird nicht benötigt...)
                    .ScheinSchildernummer = ""                                      'Addressnr. Schein und Schilder (wird nicht benötigt...)
                    .ZielFirma = e.Item.Cells(7).Text.ToString
                    .ZielFirma2 = e.Item.Cells(8).Text.ToString
                    .ZielStrasse = e.Item.Cells(9).Text.ToString
                    .ZielHNr = e.Item.Cells(10).Text.ToString
                    .ZielPLZ = e.Item.Cells(11).Text.ToString
                    .ZielOrt = e.Item.Cells(12).Text.ToString
                    .Versandart = "2"
                    .Equimpent = e.Item.Cells(3).Text.ToString
                    .SucheFgstNr = e.Item.Cells(4).Text.ToString
                    .Kennzeichen = e.Item.Cells(14).Text.ToString
                    .TIDNr = e.Item.Cells(15).Text.ToString
                    .LizenzNr = e.Item.Cells(16).Text.ToString
                    .Materialnummer = e.Item.Cells(17).Text.ToString
                    .VersandGrund = ""
                End With

                objHaendler.Anfordern("TEMP")


                tmpDataview = objHaendler.Result.DefaultView
                If (objHaendler.Auftragsnummer Is Nothing) OrElse (objHaendler.Auftragsnummer.Length = 0) Then
                    row = objHaendler.Result.Select("Equipment=" & e.Item.Cells(3).Text.ToString)
                    row(0)("STATUS") = "Fehler: " & objHaendler.Message & "<br>Grund: " & objHaendler.Auftragsstatus
                Else
                    row = objHaendler.Result.Select("Equipment=" & e.Item.Cells(3).Text.ToString)
                    row(0)("STATUS") = objHaendler.Auftragsstatus
                    'Aufträge löschen (sql-server!)
                    objHaendler.clearDB(id, status)
                    If (status <> String.Empty) Then
                        row(0)("STATUS") = status
                    End If
                End If
                FillGrid(0)
            End If
            'STORNO----------------------------------------------
            'Dim index As Integer
            Dim sqlDelete As String
            Dim sqlCommand As New SqlClient.SqlCommand()

            If e.CommandName = "delete" Then
                id = CType(e.Item.Cells(0).Text(), Int32) 'SQL-Datensatz ID

                sqlDelete = "DELETE FROM AuthorizationArval WHERE id = " & id
                sqlCommand.CommandText = sqlDelete
                sqlCommand.Connection = sqlConn
                sqlConn.Open()
                sqlCommand.ExecuteScalar()

                row = objHaendler.Result.Select("id=" & e.Item.Cells(0).Text.ToString)
                objHaendler.Result.Rows.Remove(row(0))
                FillGrid(0)
            End If
            'logApp.UpdateEntry("APP", Session("AppID").ToString, "Endgültiger Briefversand (Freigabe)")
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler im Report <Nicht durchführbare Zulassungen>. Fehler: " & ex.Message & ")")
        Finally
            sqlConn.Close()
            sqlConn.Dispose()
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change205_5.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 10:47
' Updated in $/CKAG/Applications/appsixtl/Forms
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:27
' Created in $/CKAG/Applications/appsixtl/Forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 6.12.07    Time: 14:46
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Forms
' ITA : 1440
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 14:17
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 8.03.07    Time: 13:42
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Forms
' 
' ************************************************
