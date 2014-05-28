Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change02_4
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
    Private mObjBriefanforderung As Briefanforderung
    'Private mstrHDL As String
    Private mstrREFEFERNZ1 As String
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Kopfdaten1 As AppBPLG.PageElements.Kopfdaten
    Protected WithEvents lnkFahrzeugsuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkFahrzeugAuswahl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkAdressAuswahl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblVersandart As System.Web.UI.WebControls.Label
    Protected WithEvents lblAddress As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblMaterialNummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents tblMessage As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblEnd As System.Web.UI.WebControls.Label
    Protected WithEvents lblTemp As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Versandhinweis As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        Kopfdaten1.Message = ""
        lblMessage.Text = ""
        lnkFahrzeugAuswahl.NavigateUrl = "Change02_2.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugsuche.NavigateUrl = "Change02.aspx?AppID=" & Session("AppID").ToString
        lnkAdressAuswahl.NavigateUrl = "Change02_3.aspx?AppID=" & Session("AppID").ToString

        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            m_App = New Base.Kernel.Security.App(m_User)
            'mstrHDL = ""
            'If Request.QueryString("HDL") = "1" Then
            '    mstrHDL = "&HDL=1"
            'End If

            If Session("mObjBriefanforderungSession") Is Nothing Then
                Response.Redirect("Change02.aspx?AppID=" & Session("AppID").ToString)
            End If

            If mObjBriefanforderung Is Nothing Then
                mObjBriefanforderung = CType(Session("mObjBriefanforderungSession"), Briefanforderung)
            End If

            Kopfdaten1.HaendlerNummer = mObjBriefanforderung.Endkundennummer
            Kopfdaten1.HaendlerName = mObjBriefanforderung.EndkundeFullName
            Kopfdaten1.Adresse = mObjBriefanforderung.EndkundenAdresse

            If Not IsPostBack Then
                InitialLoad()
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub InitialLoad()

        'Kopfdaten1.Kontingente = mObjBriefanforderung.Kontingente

        lnkAdressAuswahl.Visible = True
        lnkFahrzeugAuswahl.Visible = True
        FillGrid()
        lblAddress.Text = mObjBriefanforderung.VersandAdressText
        lblVersandart.Text = mObjBriefanforderung.MaterialText

    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        lblMessage.Text = String.Empty
        mObjBriefanforderung.StandardLogID = logApp.LogStandardIdentity
        Try
            Dim tmpDataView As New DataView()
            tmpDataView = mObjBriefanforderung.Fahrzeuge.DefaultView

            tmpDataView.RowFilter = "MANDT <> '0'"


            Dim intItemCounter As Int32
            Dim blnPerformedWithoutError As Boolean = True
            For intItemCounter = 0 To tmpDataView.Count - 1
                If CStr(tmpDataView.Item(intItemCounter)("COMMENT")).Length = 0 Then
                    mObjBriefanforderung.ZZFAHRG = tmpDataView.Item(intItemCounter)("ZZFAHRG").ToString
                    mObjBriefanforderung.KreditkontrollBereich = tmpDataView.Item(intItemCounter)("MANDT").ToString
                    'mObjBriefanforderung.KUNNR = m_User.KUNNR
                    mObjBriefanforderung.Anfordern()

                    If Not mObjBriefanforderung.Status = 0 Then
                        blnPerformedWithoutError = False
                        lblError.Text = mObjBriefanforderung.Message
                        Exit For
                    End If

                    Dim sngStart As Single = CSng(Microsoft.VisualBasic.Timer)
                    Dim intStart As Int32 = 0
                    Do While CSng(Microsoft.VisualBasic.Timer) < sngStart + 1
                        intStart += 1
                    Loop

                    tmpDataView.Item(intItemCounter)("VBELN") = mObjBriefanforderung.Auftragsnummer
                    If mObjBriefanforderung.Auftragsnummer.Length = 0 Then
                        tmpDataView.Item(intItemCounter)("COMMENT") = "Fehler: " & mObjBriefanforderung.Message
                        lblError.Text = "Vorgang mit Fehlern abgeschlossen."
                    ElseIf CType(Session("AppShowNot"), Boolean) = True Then
                        tmpDataView.Item(intItemCounter)("COMMENT") = mObjBriefanforderung.Auftragsstatus2
                    Else
                        tmpDataView.Item(intItemCounter)("COMMENT") = mObjBriefanforderung.Auftragsstatus
                    End If
                End If
            Next

            If blnPerformedWithoutError Then
                Dim tmpTable As DataTable
                Dim AppURL As String = Replace(Request.Url.LocalPath, "/Portal", "..")
                tmpTable = CType(Session(AppURL), DataTable)
                Dim RowColname As DataRow

                For Each RowColname In tmpTable.Rows
                    If RowColname("ControlID").ToString = "col_Kontonummer" Then
                        mstrREFEFERNZ1 = RowColname("Content").ToString
                    End If
                Next

                DataGrid1.DataSource = tmpDataView
                DataGrid1.DataBind()

                Dim tblTemp As New DataTable()
                Dim i As Int32

                tblTemp.Columns.Add("Kundennr.", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Händlernr.", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Fahrgestellnr.", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Vertragsnr.", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("ZBII Nummer", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Kennzeichen", System.Type.GetType("System.String"))
                tblTemp.Columns.Add(mstrREFEFERNZ1, System.Type.GetType("System.String"))
                ' tblTemp.Columns.Add("Equipmentnr.", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Bez.", System.Type.GetType("System.Boolean"))
                tblTemp.Columns.Add("COC Besch.vorh.", System.Type.GetType("System.Boolean"))
                tblTemp.Columns.Add("Auftragsnr.", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Kommentar", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Kontingentart", System.Type.GetType("System.String"))
                Dim tmpRow As DataRow
                For i = 0 To tmpDataView.Count - 1
                    tmpRow = tblTemp.NewRow
                    tmpRow("Kundennr.") = mObjBriefanforderung.KUNNR.TrimStart("0"c)
                    tmpRow("Händlernr.") = mObjBriefanforderung.Customer.TrimStart("0"c)
                    tmpRow("Fahrgestellnr.") = tmpDataView(i)("ZZFAHRG")
                    tmpRow("Vertragsnr.") = tmpDataView(i)("LIZNR")
                    tmpRow("ZBII Nummer") = tmpDataView(i)("TIDNR")
                    tmpRow("Kennzeichen") = tmpDataView(i)("LICENSE_NUM")
                    tmpRow(mstrREFEFERNZ1) = tmpDataView(i)("ZZREFERENZ1")
                    'If Not TypeOf tmpDataView(i)("EQUNR") Is System.DBNull Then
                    '    tmpRow("Equipmentnr.") = CStr(tmpDataView(i)("EQUNR")).TrimStart("0"c)
                    'End If
                    tmpRow("Bez.") = tmpDataView(i)("ZZBEZAHLT")
                    tmpRow("COC Besch.vorh.") = tmpDataView(i)("ZZCOCKZ")
                    tmpRow("Auftragsnr.") = tmpDataView(i)("VBELN")
                    tmpRow("Kommentar") = tmpDataView(i)("COMMENT")
                    Select Case tmpDataView(i)("MANDT").ToString
                        Case "1"
                            tmpRow("Kontingentart") = "Standard temporär"
                        Case "2"
                            tmpRow("Kontingentart") = "Standard endgültig"
                        Case "4"
                            tmpRow("Kontingentart") = "Erweitertes Zahlungsziel (Delayed Payment) endgültig"
                    End Select

                    tblTemp.Rows.Add(tmpRow)
                Next
                logApp.UpdateEntry("APP", Session("AppID").ToString, "Briefanforderung zu Adresse-Nr. " & mObjBriefanforderung.VersandAdressValue & " erfolgreich durchgeführt.", tblTemp)

                mObjBriefanforderung.Show()

                Kopfdaten1.Kontingente = mObjBriefanforderung.Kontingente

                Session("mObjBriefanforderungSession") = mObjBriefanforderung

                cmdSave.Visible = False
                lnkAdressAuswahl.Visible = False
                lnkFahrzeugAuswahl.Visible = False
            Else
                InitialLoad()
            End If
            DataGrid1.Columns(DataGrid1.Columns.Count - 2).Visible = True
            DataGrid1.Columns(DataGrid1.Columns.Count - 1).Visible = True
            ' End If
        Catch ex As Exception
            lblError.Text = "Fehler: " & ex.Message
            logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler bei der Briefanforderung zu Adresse-Nr. " & mObjBriefanforderung.VersandAdressValue & ", Fehler: " & ex.Message & ")")
        End Try
    End Sub

    'Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    DoSubmit()
    'End Sub

    Private Sub FillGrid(Optional ByVal strSort As String = "")
        Dim tmpDataView As DataView = mObjBriefanforderung.Fahrzeuge.DefaultView

        If Not strSort.Trim(" "c).Length = 0 Then
            Dim strDirection As String
            If ViewState("Direction") Is Nothing Then
                strDirection = "desc"
            Else
                strDirection = ViewState("Direction").ToString
            End If

            If strDirection = "asc" Then
                strDirection = "desc"
            Else
                strDirection = "asc"
            End If

            tmpDataView.Sort = strSort & " " & strDirection
            ViewState("Direction") = strDirection
        End If

        tmpDataView.RowFilter = "MANDT <> '0'"
        DataGrid1.DataSource = tmpDataView
        DataGrid1.DataBind()

        lblMessage.Text = "<br>Zur Aktivierung der ausgewählten Vorgänge klicken Sie auf [Absenden].<br>"
        DataGrid1.PagerStyle.Visible = False
    End Sub

    'Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
    '    FillGrid(e.SortExpression)
    'End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change02_4.aspx.vb $
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 29.07.08   Time: 16:08
' Updated in $/CKAG/Applications/AppBPLG/Forms
' Nachbesserung ita 2070
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 29.07.08   Time: 15:42
' Updated in $/CKAG/Applications/AppBPLG/Forms
' Nachbesserung ITA 2070
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 29.07.08   Time: 14:29
' Updated in $/CKAG/Applications/AppBPLG/Forms
' BPLG Test Nachbesserungen
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 29.07.08   Time: 10:58
' Updated in $/CKAG/Applications/AppBPLG/Forms
' nachbesserung
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 29.07.08   Time: 8:56
' Updated in $/CKAG/Applications/AppBPLG/Forms
' nachbesserung
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 25.07.08   Time: 14:32
' Updated in $/CKAG/Applications/AppBPLG/Forms
' ITA 2070 nachbesserungen
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 22.07.08   Time: 12:49
' Updated in $/CKAG/Applications/AppBPLG/Forms
' ITA 2070
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 18.07.08   Time: 14:19
' Updated in $/CKAG/Applications/AppBPLG/Forms
' ITA 2070 rohversion
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 18.07.08   Time: 12:51
' Created in $/CKAG/Applications/AppBPLG/Forms
' Klassen erstellt
' ************************************************

