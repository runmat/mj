Option Explicit On 
Option Strict On


Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Public Class Change02
    Inherits System.Web.UI.Page
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lb_Suche As System.Web.UI.WebControls.LinkButton
    Protected WithEvents calVon As System.Web.UI.WebControls.Calendar
    Protected WithEvents calBis As System.Web.UI.WebControls.Calendar
    Protected WithEvents txt_Fahrgestellnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_Ordernummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents trCreate As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trSelectDropdown As System.Web.UI.HtmlControls.HtmlTableRow
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Dim m_change As kruell_02
    Protected WithEvents btnAnlageCal1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents btnAnlageCal2 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents btnStornierungCal1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents btnStornierungCal2 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txt_DatumVonAnlage As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DatumBisAnlage As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DatumVonStornierung As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_DatumBisStornierung As System.Web.UI.WebControls.TextBox
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lb_Senden As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbl_Fehlgeschlagen As Label
    Protected WithEvents lbl_Auswahl As Label
    Protected WithEvents lbl_Wiederhergestellt As Label
    Protected WithEvents Tr1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Tr2 As System.Web.UI.HtmlControls.HtmlTableRow


    Dim strErrorText As String



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
        Try
            m_User = GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte 
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 

            FormAuth(Me, m_User)
            ucHeader.InitUser(m_User)
            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            If Not IsPostBack Then
                m_change = New kruell_02(m_User, m_App, "")
                m_change.SessionID = Me.Session.SessionID
                m_change.AppID = CStr(Session("AppID"))
                Session.Add("objChange", m_change)
            End If


        Catch ex As Exception
            lblError.Text = "Fehler beim laden der Seite: " & ex.Message.ToString
        End Try

    End Sub



    Private Sub lb_Suche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Suche.Click

        lbl_Auswahl.Text = ""
        lbl_Wiederhergestellt.Text = ""
        lbl_Fehlgeschlagen.Text = ""

        Dim valid As Boolean


        If HelpProcedures.checkDate(txt_DatumVonStornierung, txt_DatumBisStornierung, strErrorText, True) = True AndAlso HelpProcedures.checkDate(txt_DatumVonAnlage, txt_DatumBisAnlage, strErrorText, True) = True Then
            valid = True
        Else
            valid = False
        End If

        If m_change Is Nothing Then
            m_change = CType(Session("objChange"), kruell_02)
        End If


        If valid = True Then


            m_change.fahrgestellnummer = txt_Fahrgestellnummer.Text.ToUpper
            m_change.Ordernummer = txt_Ordernummer.Text

            'Testweise, da wenn ich SAP leere Strings übergebe, ich keine Daten bekomme 
            If Not txt_DatumBisAnlage.Text = "" Then
                m_change.AnlageDatumBis = txt_DatumBisAnlage.Text
                m_change.AnlageDatumVon = txt_DatumVonAnlage.Text
                m_change.StornierDatumBis = txt_DatumBisStornierung.Text
                m_change.StornierDatumVon = txt_DatumVonStornierung.Text
            End If


            'füllen der Datentabelle des Auftrags
            m_change.Fill(m_change.AppID, m_change.SessionID)
            Session.Add("Source", m_change.Result)
            DataGrid1.DataSource = m_change.Result
            DataGrid1.DataBind()

            If Not DataGrid1.Items.Count <= 0 Then
                lb_Senden.Enabled = True
            Else
                lb_Senden.Enabled = False
            End If


            lblError.Text = m_change.Message 'fehlermeldung ausgeben
        Else
            lblError.Text = strErrorText
        End If



    End Sub


    Private Sub calVon_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged

        If CStr(Viewstate.Item("CalState")) = "Stornierung" Then
            txt_DatumVonStornierung.Text = calVon.SelectedDate.ToShortDateString
        ElseIf CStr(Viewstate.Item("CalState")) = "Anlage" Then
            txt_DatumVonAnlage.Text = calVon.SelectedDate.ToShortDateString
        End If
        calVon.Visible = False
    End Sub

    Private Sub calBis_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBis.SelectionChanged

        If CStr(Viewstate.Item("CalState")) = "Stornierung" Then
            txt_DatumBisStornierung.Text = calBis.SelectedDate.ToShortDateString
        ElseIf CStr(Viewstate.Item("CalState")) = "Anlage" Then
            txt_DatumBisAnlage.Text = calBis.SelectedDate.ToShortDateString
        End If
        calBis.Visible = False
    End Sub

   

    'es werden 2 Kalendersteuerelemente für 4 Datumseingaben verwendet, es muss jeweils das ausgwählte datum zum richtigen Datumsfeld zugeordnet werden, jeder Kalender muss mit dem Aktuellen datum neu aufgerufen werden. JJ2007.12.3
    Private Sub btnAnlageCal1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnlageCal1.Click
        calVon.Visible = True
        ViewState.Clear()
        Viewstate.Add("CalState", "Anlage")
        calBis.Visible = False
        'calVon.SelectedDate = Today
        calVon.SelectedDate = Nothing
    End Sub

    Private Sub btnStornierungCal1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStornierungCal1.Click
        calVon.Visible = True
        ViewState.Clear()
        Viewstate.Add("CalState", "Stornierung")
        calBis.Visible = False
        'calVon.SelectedDate = Today
        calVon.SelectedDate = Nothing
    End Sub

    Private Sub btnAnlageCal2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnlageCal2.Click
        calBis.Visible = True
        ViewState.Clear()
        Viewstate.Add("CalState", "Anlage")
        calVon.Visible = False
        'calBis.SelectedDate = Today

        calBis.SelectedDate = Nothing
    End Sub

    Private Sub btnStornierungCal2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStornierungCal2.Click
        calBis.Visible = True
        ViewState.Clear()
        Viewstate.Add("CalState", "Stornierung")
        calVon.Visible = False
        'calBis.SelectedDate = Today
        calBis.SelectedDate = Nothing
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        ' Create a DataView from the DataTable.
        Dim dv As DataView = New DataView(CType(Session("Source"), DataTable))

        If Viewstate.Item("exSortExpression") Is Nothing Then
            Viewstate.Add("exSortExpression", e.SortExpression)
            Viewstate.Add("Sort", "asc")
        Else
            If CStr(Viewstate.Item("exSortExpression")) = e.SortExpression Then
                If CStr(Viewstate.Item("Sort")) = "asc" Then
                    Viewstate.Item("Sort") = "desc"
                ElseIf CStr(Viewstate.Item("Sort")) = "desc" Then
                    Viewstate.Item("Sort") = "asc"
                End If
            Else
                Viewstate("exSortExpression") = e.SortExpression
                Viewstate("Sort") = "asc"
            End If
        End If


        dv.Sort = e.SortExpression & " " & CStr(ViewState.Item("Sort"))
        'System.Diagnostics.Debug.WriteLine(dv.Sort)

        ' Rebind the data source and specify that it should be sorted
        ' by the field specified in the SortExpression property.
        DataGrid1.DataSource = dv
        DataGrid1.DataBind()

    End Sub

    'Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
    '    Dim m_objTable As New DataTable()
    '    m_objTable = CType(Session("Source"), DataTable)

    '    Dim intItem As Int32

    '    For intItem = 0 To m_objTable.Columns.Count - 1
    '        If m_objTable.Columns(intItem).DataType Is System.Type.GetType("System.DateTime") Then
    '            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '                e.Item.Cells(intItem).Text = DataBinder.Eval(e.Item.DataItem, m_objTable.Columns(intItem).ColumnName, "{0:dd.MM.yyyy}")
    '            End If
    '        End If
    '    Next
    'End Sub

    Private Sub lb_Senden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Senden.Click
        If m_change Is Nothing Then
            m_change = CType(Session("objChange"), kruell_02)
        End If

        Dim alOrdernrs As New ArrayList()
        Dim item As DataGridItem

        For Each item In DataGrid1.Items
            If CType(item.FindControl("chbx_Wiederherstellen"), CheckBox).Checked = True Then
                alOrdernrs.Add(item.Cells(1).Text)
            End If
        Next

        If Not alOrdernrs.Count <= 0 Then
            lbl_Auswahl.Text = alOrdernrs.Count.ToString
            m_change.sendToSAP(alOrdernrs)
            lbl_Wiederhergestellt.Text = m_change.Wiederherstellungen.ToString
            lbl_Fehlgeschlagen.Text = m_change.FehlgeschlageneOrdernummern
            DataGrid1.DataSource = Nothing
            DataGrid1.DataBind()
            lb_Senden.Enabled = False
        Else
            lblError.Text = "Keine ausgewählten Aufträge"
        End If

    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class
' ************************************************
' $History: Change02.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 14:37
' Created in $/CKAG/Applications/AppKruell/Forms
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 9.01.08    Time: 13:57
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Forms
' ITA 1580 Report01 hinzugefügt, SS History Bodys hinzugefügt
' ************************************************

