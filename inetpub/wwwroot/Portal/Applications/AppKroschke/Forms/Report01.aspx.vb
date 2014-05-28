Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report01
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private zulDaten As clsVorerf01
    Private vkorg As String
    Private vkbur As String
    Private kunnr As String

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header
    Protected WithEvents calVon As System.Web.UI.WebControls.Calendar
    Protected WithEvents calBis As System.Web.UI.WebControls.Calendar
    Protected WithEvents txtKundennummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlKunnr As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtReferenz As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblInputReq As System.Web.UI.WebControls.Label
    Protected WithEvents txtZulassungsdatumVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnVon As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txtZulassungsdatumBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnBis As System.Web.UI.WebControls.LinkButton
    Protected WithEvents RB_G As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RB_A As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RB_Leer As System.Web.UI.WebControls.RadioButton
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label
    Protected WithEvents txtDummy As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label

    Dim back As String

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
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)
            vkorg = Left(m_User.Reference, 4)
            vkbur = Right(m_User.Reference, 4)

            If Not IsPostBack Then

                If (back Is Nothing) Then
                    zulDaten = New clsVorerf01(m_User, True)
                    zulDaten.VKOrg = vkorg
                    zulDaten.VKBur = vkbur
                    Session.Add("ZulDaten", zulDaten)
                    loadForm()
                End If
            Else
                fillDropdownlist()
                zulDaten = CType(Session("ZulDaten"), clsVorerf01)
            End If
            'Skripte setzten
            ddlKunnr.Attributes.Add("onChange", "SetKnr();")
            txtKundennummer.Attributes.Add("onKeyup", "SetKunnr();")

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub loadForm()
        'Hier die bereits in SAP vorbelegten Daten laden: Dropdown-Listen Kunde und Dienstleistung
        Dim tblKundenBak As New DataTable()
        Dim message As String = ""
        Dim dv As DataView
        Dim row As DataRow
        Dim c() As Char = {"1"c, "2"c, "3"c, "4"c, "5"c, "6"c, "7"c, "8"c, "9"c}

        zulDaten = CType(Session("ZulDaten"), clsVorerf01)
        zulDaten.getSAPDatenBiz(vkorg, vkbur, message)

        If (message <> String.Empty) Then
            'Fehler aufgetreten
            lblError.Text = message
        Else
            'Kundenliste füllen
            For Each row In zulDaten.Kundentabelle.Rows
                row("NAME1") = row("NAME1").ToString & " ~ " & Right("000000" & row("KUNNR").ToString, 8)

                If (row("ALTKN").ToString <> String.Empty) Then
                    row("NAME1") = row("NAME1").ToString & " ~ " & Right("000000" & row("ALTKN").ToString, 8)
                End If
                '§§§JVE 27.09.2005 <begin>
                'Auf Wunsch die Tour-Nr. aus der Anzeige entfernt. (IT-Anforderung ZLD 56)

                'If row("KVGR4").ToString <> String.Empty Then
                '    row("NAME1") = row("NAME1").ToString & " / Tour " & row("KVGR4").ToString
                'End If
                '§§§JVE 27.09.2005 <end>
                If row("DATLT").ToString <> String.Empty Then
                    row("NAME1") = row("NAME1").ToString & " / " & row("DATLT").ToString
                End If
            Next

            'StVA füllen            
            For Each row In zulDaten.Stvatabelle.Rows
                row("KREISBEZ") = Left(row("KREISKZ").ToString & "....", 4) & row("KREISBEZ").ToString
            Next
            zulDaten.Stvatabelle.AcceptChanges()
        End If
        '§§§ JVE 06.02.2006 
        'txtSAPID.Text = CStr(GiveNewZulassungsID())

        dv = zulDaten.Kundentabelle.DefaultView
        dv.Sort = "NAME1"
        With ddlKunnr
            .DataSource = dv
            .DataTextField = "NAME1"
            .DataValueField = "KUNNR"
            .DataBind()
        End With
        Session("ZulDaten") = zulDaten
    End Sub
    Private Sub fillDropdownlist()

        Dim zulDaten As clsVorerf01
        Dim dv As DataView

        zulDaten = CType(Session("ZulDaten"), clsVorerf01)

        'Kundendaten füllen
        dv = zulDaten.Kundentabelle.DefaultView
        dv.Sort = "NAME1"

        With ddlKunnr
            .DataSource = dv
            .DataTextField = "NAME1"
            .DataValueField = "KUNNR"
            .DataBind()
        End With
        If (txtKundennummer.Text = String.Empty) Then
            ddlKunnr.SelectedIndex = 0
            'ddlKundeBak.SelectedIndex = 0
        Else
            'ddlKunnr.Items.FindByValue(Right("0000000000" & txtKundennummer.Text, 10)).Selected = True
            If txtDummy.Value.TrimStart("0"c) = String.Empty Then
                Dim ItemList As ListItem = ddlKunnr.Items.FindByValue(Right("0000000000" & txtKundennummer.Text, 10))
                If Not ItemList Is Nothing Then
                    ddlKunnr.Items.FindByValue(Right("0000000000" & txtKundennummer.Text, 10)).Selected = True
                Else
                    ddlKunnr.Items(0).Selected = True
                End If
            Else
                ddlKunnr.Items(txtDummy.Value).Selected = True
                txtKundennummer.Text = ddlKunnr.SelectedItem.Value.TrimStart("0"c)
            End If

            'ddlKundeBak.Items.FindByValue(Right("0000000000" & txtKundennummer.Text, 10)).Selected = True
        End If
    End Sub
    Private Sub btnVon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVon.Click
        calVon.Visible = True
        calBis.Visible = False
    End Sub

    Private Sub calVon_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        txtZulassungsdatumVon.Text = calVon.SelectedDate.ToShortDateString
        txtZulassungsdatumBis.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub



    Private Sub DoSubmit()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        Dim m_Report As New CK_02(m_User, m_App, strFileName)
        Dim datZulVon As DateTime
        Dim datZulBis As DateTime
        Dim datLoesch As String = ""
        Dim datReferenz As String = ""
        Dim sKunnr As String = ""
        Dim temp As String = ""
        Dim datKennzeichen As String = ""
        Dim checkInput As Boolean = True
        Try
            ' Referenz und Kennzeichen prüfen
            If txtKennzeichen.Text.Trim.Length > 0 Or txtReferenz.Text.Trim.Length > 0 Then
                datKennzeichen = Trim(txtKennzeichen.Text)
                datReferenz = Trim(txtReferenz.Text)

            ElseIf txtZulassungsdatumVon.Text <> String.Empty Or txtZulassungsdatumBis.Text <> String.Empty Then
                If checkInputData() Then
                    datZulVon = Trim(txtZulassungsdatumVon.Text)
                    datZulBis = Trim(txtZulassungsdatumBis.Text)
                Else : Exit Sub
                End If
            Else
                lblError.Text = "Bitte geben Sie Daten für die Selektion ein!<br>"
                Exit Sub
            End If

                temp = txtKundennummer.Text
                sKunnr = temp

                If RB_G.Checked = True Then
                    datLoesch = "G"
                End If
                If RB_A.Checked = True Then
                    datLoesch = "A"
                End If
                If RB_Leer.Checked = True Then
                    datLoesch = ""
                End If

                If checkInput Then
                m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, datReferenz, datKennzeichen, datZulVon, datZulBis, datLoesch, sKunnr)
                    Session("ResultTable") = m_Report.Result

                If Not m_Report.Status = 0 Then
                    lblError.Text = "Fehler: " & m_Report.Message
                Else
                    If m_Report.Result.Rows.Count = 0 Then
                        lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                        Session("ResultTable") = Nothing
                    Else

                        Try
                            Excel.ExcelExport.WriteExcel(m_Report.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                        Catch
                        End Try
                        Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                        Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Function checkInputData() As Boolean
        Dim datZulVon As DateTime
        Dim datZulBis As DateTime

        If Not IsDate(txtZulassungsdatumVon.Text) Then
            lblError.Text = "Bitte geben Sie ein Datum im Feld -Zulassungsdatum von:- ein."
            Return False
        Else
            datZulVon = CDate(txtZulassungsdatumVon.Text)
        End If

        If Not IsDate(txtZulassungsdatumBis.Text) Then
            lblError.Text = "Bitte geben Sie ein Datum im Feld -Zulassungsdatum bis:- ein."
            Return False
        Else
            datZulBis = CDate(txtZulassungsdatumBis.Text)
        End If

        If (datZulVon > datZulBis) Then
            lblError.Text = "Zulassungssdatum (von) muß kleiner oder gleich Zulassungsdatum (bis) sein!"
            Return False
        End If

        If (datZulBis.Subtract(datZulVon).Days > 60) Then
            lblError.Text = "Der angegebene Zeitraum umfasst mehr als 60 Tage!"
            Return False
        End If

        Return True
    End Function

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub btnBis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBis.Click
        calBis.Visible = True
        calVon.Visible = False
    End Sub

    Private Sub calBis_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBis.SelectionChanged
        txtZulassungsdatumBis.Text = calBis.SelectedDate.ToShortDateString
        calBis.Visible = False
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        If txtDummy.Value = "" Or txtDummy.Value = "0" Then
            If ddlKunnr.SelectedIndex < 1 Then
                lblError.Text = "Kein Kunde ausgewählt."
                Session("DummyValue") = Nothing
            Else
                txtDummy.Value = ddlKunnr.SelectedIndex.ToString
                Session("DummyValue") = txtDummy.Value
                DoSubmit()
                txtDummy.Value = String.Empty
            End If
        Else
            txtDummy.Value = ddlKunnr.SelectedIndex.ToString
            Session("DummyValue") = txtDummy.Value
            DoSubmit()
            txtDummy.Value = String.Empty
        End If
    End Sub
End Class

' ************************************************
' $History: Report01.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 29.04.10   Time: 9:06
' Updated in $/CKAG/Applications/AppKroschke/Forms
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 11.03.10   Time: 14:32
' Updated in $/CKAG/Applications/AppKroschke/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 14.10.08   Time: 13:07
' Updated in $/CKAG/Applications/AppKroschke/Forms
' ITA: 2301 & Warnungen bereinigt
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 14:30
' Created in $/CKAG/Applications/AppKroschke/Forms
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 11.09.07   Time: 14:25
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' ITA: 1218
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 11.09.07   Time: 10:41
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 6.07.07    Time: 15:03
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 6.07.07    Time: 14:29
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' ITA: 1130
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 13:07
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 23.05.07   Time: 9:31
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 4  *****************
' User: Uha          Date: 8.03.07    Time: 9:25
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' 
' ************************************************
