Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Change81_4
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
    Private objAddressList As Search
    Private objHaendler As SonstDL
    Private preise As DataTable
    Private angefordert As Integer  'Fahrzeuge, die laut Preisstaffelung berechnet werden (nicht gesperrt angelegt)
    Private preis_stueck As Decimal 'Preis pro Fahrzeug

    Protected WithEvents lnkFahrzeugsuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkFahrzeugAuswahl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkAdressAuswahl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucHeader As Header
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblAddress As System.Web.UI.WebControls.Label
    Protected WithEvents lblMaterialNummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ucStyles As Styles
    Protected WithEvents trVersandTemp As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trVersandArt As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblBeauftragteDienstleistung As System.Web.UI.WebControls.Label
    Protected WithEvents lblBeauftragteDienstleistungAnzeige As System.Web.UI.WebControls.Label
    Protected WithEvents lblEingaben As System.Web.UI.WebControls.Label
    Protected WithEvents lblHinweis As System.Web.UI.WebControls.Label
    Protected WithEvents lblKreis As System.Web.UI.WebControls.Label
    Protected WithEvents trWunschkennzeichen As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trReserviertAuf As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trVersicherungstraeger As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trDurchfuehrungsdatum As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trHinweis As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trBemerkung As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblHalterName1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblHalterName2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblHalterStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents lblHalterPLZ As System.Web.UI.WebControls.Label
    Protected WithEvents lblHalterOrt As System.Web.UI.WebControls.Label
    Protected WithEvents lblStandortName1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblStandortName2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblStandortStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents lblStandortPLZ As System.Web.UI.WebControls.Label
    Protected WithEvents lblStandortOrt As System.Web.UI.WebControls.Label
    Protected WithEvents lblEmpfaengerName1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblEmpfaengerName2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblEmpfaengerStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents lblEmpfaengerPLZ As System.Web.UI.WebControls.Label
    Protected WithEvents lblEmpfaengerOrt As System.Web.UI.WebControls.Label
    Protected WithEvents lblDurchfuehrungsDatum As System.Web.UI.WebControls.Label
    Protected WithEvents lblReserviertAuf As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersicherungstraeger As System.Web.UI.WebControls.Label
    Protected WithEvents lblWunschkennzeichen As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrzeugangaben As System.Web.UI.WebControls.Label
    Protected WithEvents lblBemerkung As System.Web.UI.WebControls.Label
    Protected WithEvents trNeuerHalter01 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trNeuerHalter02 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trNeuerStandort01 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trNeuerStandort02 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trEmpfaenger01 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trEmpfaenger02 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblHalterHausnr As System.Web.UI.WebControls.Label
    Protected WithEvents lblEmpfaengerHausnr As System.Web.UI.WebControls.Label
    Protected WithEvents lblStandortHausnr As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label

    Protected WithEvents trevb As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblDatumVON As System.Web.UI.WebControls.Label
    Protected WithEvents lblDatumBis As System.Web.UI.WebControls.Label
    Protected WithEvents TrGueltigkeit As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblEVB As System.Web.UI.WebControls.Label

    Dim tmpbankBaseobj As BankBase


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        lnkFahrzeugsuche.NavigateUrl = "Change81.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugAuswahl.NavigateUrl = "Change81_2.aspx?AppID=" & Session("AppID").ToString
        lnkAdressAuswahl.NavigateUrl = "Change81_3.aspx?AppID=" & Session("AppID").ToString

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        If m_User.Organization.OrganizationAdmin Then
            lblHead.Text = Replace(lblHead.Text, " (Beauftragung)", "")
        End If
        ucStyles.TitleText = lblHead.Text

        m_App = New Base.Kernel.Security.App(m_User)

        If Session("objHaendler") Is Nothing Then
            Response.Redirect("Change81.aspx?AppID=" & Session("AppID").ToString)
        End If

        objHaendler = CType(Session("objHaendler"), SonstDL)

        Dim tmpDataView As New DataView()
        tmpDataView = objHaendler.Fahrzeuge.DefaultView

        tmpDataView.RowFilter = "MANDT = '99'"
        Dim intFahrzeugBriefe As Int32 = tmpDataView.Count
        tmpDataView.RowFilter = ""

        If intFahrzeugBriefe = 0 Then
            'Schrott! Weg hier!
            Response.Redirect("Change81.aspx?AppID=" & Session("AppID").ToString)
        End If

        'dient nur für die makeSAPDate-Funktion
        If tmpbankBaseobj Is Nothing Then
            tmpbankBaseobj = New BankBaseCredit(m_User, m_App, Session("AppID").ToString, Page.Session.SessionID, "")
        End If

        If Not IsPostBack Then
            InitialLoad()
        End If

    End Sub

    Private Sub InitialLoad()
        trVersandArt.Visible = True

        HideAll()

        Select Case objHaendler.Auftragsgrund
            Case "2052"
                'Ummeldung innerorts
                trNeuerHalter01.Visible = True
                trNeuerHalter02.Visible = True
                trNeuerStandort01.Visible = True
                trNeuerStandort02.Visible = True
                trWunschkennzeichen.Visible = True
                trReserviertAuf.Visible = True
                trVersicherungstraeger.Visible = True
                trevb.Visible = True
                trEmpfaenger01.Visible = True
                trEmpfaenger02.Visible = True
                trDurchfuehrungsdatum.Visible = True
                trBemerkung.Visible = True
            Case "572"
                'Ummeldung ausserorts
                trNeuerHalter01.Visible = True
                trNeuerStandort01.Visible = True
                trNeuerHalter02.Visible = True
                trNeuerStandort02.Visible = True
                trWunschkennzeichen.Visible = True
                trReserviertAuf.Visible = True
                trVersicherungstraeger.Visible = True
                trevb.Visible = True
                trEmpfaenger01.Visible = True
                trEmpfaenger02.Visible = True
                trDurchfuehrungsdatum.Visible = True
                trBemerkung.Visible = True
            Case "1294"
                'Umkennzeichnung
                trWunschkennzeichen.Visible = True
                trReserviertAuf.Visible = True
                trEmpfaenger01.Visible = True
                trEmpfaenger02.Visible = True
                trDurchfuehrungsdatum.Visible = True
                trBemerkung.Visible = True
            Case "2037"
                'Ersatzfahrzeugschein
                trEmpfaenger01.Visible = True
                trEmpfaenger02.Visible = True
                trDurchfuehrungsdatum.Visible = True
                trHinweis.Visible = True
                lblHinweis.Text = "Bitte Verlusterklärung / eidestattliche Versicherung im Original an DAD senden."
                trBemerkung.Visible = True
            Case "1380-1"
                trDurchfuehrungsdatum.Visible = True
                trHinweis.Visible = True
                lblHinweis.Text = "Bitte Gutachten im Original an DAD senden."
                trBemerkung.Visible = True
            Case "1380-2"
                trDurchfuehrungsdatum.Visible = True
                trBemerkung.Visible = True
            Case "1380-3"
                trDurchfuehrungsdatum.Visible = True
                trHinweis.Visible = True
                lblHinweis.Text = "Bitte ZB1 im Original und Kennzeichen an DAD senden."
                trBemerkung.Visible = True
            Case "1462"
                'Wiederzulassung
                trVersicherungstraeger.Visible = True
                trevb.Visible = True

                trEmpfaenger01.Visible = True
                trEmpfaenger02.Visible = True
                trDurchfuehrungsdatum.Visible = True
                trBemerkung.Visible = True
        End Select

        lblBeauftragteDienstleistungAnzeige.Text = objHaendler.BeauftragungKlartext

        lblHalterName1.Text = objHaendler.HalterName1
        lblHalterName2.Text = objHaendler.HalterName2
        lblHalterOrt.Text = objHaendler.HalterOrt
        lblHalterPLZ.Text = objHaendler.HalterPLZ
        lblHalterStrasse.Text = objHaendler.HalterStrasse
        lblHalterHausnr.Text = objHaendler.HalterHausnr

        lblStandortName1.Text = objHaendler.StandortName1
        lblStandortName2.Text = objHaendler.StandortName2
        lblStandortOrt.Text = objHaendler.StandortOrt
        lblStandortPLZ.Text = objHaendler.StandortPLZ
        lblStandortStrasse.Text = objHaendler.StandortStrasse
        lblStandortHausnr.Text = objHaendler.StandortHausnr

        If objHaendler.StandortName1.Length + objHaendler.StandortName2.Length + objHaendler.StandortOrt.Length + objHaendler.StandortPLZ.Length + objHaendler.StandortStrasse.Length = 0 Then
            trNeuerStandort01.Visible = False
            trNeuerStandort02.Visible = False
        End If

        lblKreis.Text = objHaendler.Kreis
        lblWunschkennzeichen.Text = objHaendler.Wunschkennzeichen
        lblReserviertAuf.Text = objHaendler.ReserviertAuf
        lblVersicherungstraeger.Text = objHaendler.Versicherungstraeger

        'evbfeld aufsplitten
        Dim split() As String
        If Not objHaendler.evbNummer Is String.Empty AndAlso Not objHaendler.evbNummer Is Nothing Then
            split = objHaendler.evbNummer.Split(" ")
            lblEVB.Text = split(0)
            If split.Length > 1 Then
                lblDatumVON.Text = HelpProcedures.MakeDateStandard(split(1))
                lblDatumBis.Text = HelpProcedures.MakeDateStandard(split(2))
            End If
        End If


        lblEmpfaengerName1.Text = objHaendler.EmpfaengerName1
        lblEmpfaengerName2.Text = objHaendler.EmpfaengerName2
        lblEmpfaengerOrt.Text = objHaendler.EmpfaengerOrt
        lblEmpfaengerPLZ.Text = objHaendler.EmpfaengerPLZ
        lblEmpfaengerStrasse.Text = objHaendler.EmpfaengerStrasse
        lblEmpfaengerHausnr.Text = objHaendler.EmpfaengerHausnr
        lblDurchfuehrungsDatum.Text = Format(objHaendler.DurchfuehrungsDatum, "dd.MM.yyyy")
        lblBemerkung.Text = objHaendler.Bemerkung

        FillGrid()
    End Sub

    Private Sub HideAll()
        trNeuerHalter01.Visible = False
        trNeuerStandort01.Visible = False
        trNeuerHalter02.Visible = False
        trNeuerStandort02.Visible = False
        trWunschkennzeichen.Visible = False
        trReserviertAuf.Visible = False
        trVersicherungstraeger.Visible = False
        trevb.Visible = False
        trEmpfaenger01.Visible = False
        trEmpfaenger02.Visible = False
        trDurchfuehrungsdatum.Visible = False
        trHinweis.Visible = False
        trBemerkung.Visible = False
    End Sub

    Private Sub DoSubmit()
        Dim logApp As Base.Kernel.Logging.Trace
        logApp = CType(Session("logObj"), Base.Kernel.Logging.Trace)

        objHaendler.StandardLogID = logApp.LogStandardIdentity

        Try
            Dim tmpDataView As DataView = objHaendler.Fahrzeuge.DefaultView
            tmpDataView.RowFilter = "MANDT = '99'"

            Dim intItemCounter As Int32
            'Dim blnPerformedWithoutError As Boolean = True

            objHaendler.KUNNR = m_User.KUNNR

            For intItemCounter = 0 To tmpDataView.Count - 1
                'Daten sammeln

                objHaendler.Equimpent = tmpDataView.Item(intItemCounter)("EQUNR").ToString
                objHaendler.SucheFahrgestellNr = tmpDataView.Item(intItemCounter)("CHASSIS_NUM").ToString
                objHaendler.Kennzeichen = tmpDataView.Item(intItemCounter)("LICENSE_NUM").ToString
                objHaendler.TIDNr = tmpDataView.Item(intItemCounter)("TIDNR").ToString
                objHaendler.LizenzNr = tmpDataView.Item(intItemCounter)("LIZNR").ToString

                objHaendler.Anfordern()

                tmpDataView.Item(intItemCounter)("STATUS") = objHaendler.Auftragsstatus
                tmpDataView.Table.AcceptChanges()

            Next

            DataGrid1.DataSource = tmpDataView
            DataGrid1.DataBind()
            logApp.UpdateEntry("APP", Session("AppID").ToString, "Beauftragung sonstiger Dienstleistungen")
        Catch ex As Exception
            lblError.Text = "Fehler: " & ex.Message
            logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler bei der Briefanforderung zu Equipment. " & objHaendler.Equimpent & ", Fehler: " & ex.Message & ")")
            Throw ex
        End Try
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub

    Private Sub FillGrid(Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = objHaendler.Fahrzeuge.DefaultView

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

        tmpDataView.RowFilter = "MANDT = '99'"
        DataGrid1.DataSource = tmpDataView
        DataGrid1.DataBind()

        DataGrid1.PagerStyle.Visible = False

        Dim intZaehl0099 As Int32 = 0
        lblMessage.Text = ""
        intZaehl0099 = 0                            'Anforderungen zählen
        Dim row As DataRow
        For Each row In tmpDataView.Table.Rows
            If (row("MANDT").ToString = "99") Then
                intZaehl0099 += 1
            End If
        Next
    End Sub

    Private Sub get_stueckpreis(ByVal fahrzeuge As Int32)
        Dim found As Boolean = False
        Dim row As Integer = 0

        While Not found And row <= preise.Rows.Count - 1
            If CType(preise.Rows(row)("KSTBM"), Integer) <= fahrzeuge Then
                row += 1
            Else
                found = True
            End If
        End While

        If found = True Then
            If (row = 0) Then
                preis_stueck = CType(preise.Rows(row)("KBETR"), Decimal)
            Else
                preis_stueck = CType(preise.Rows(row - 1)("KBETR"), Decimal)
            End If
        Else
            If (row = preise.Rows.Count) Then
                preis_stueck = CType(preise.Rows(row - 1)("KBETR"), Decimal)
            End If
        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        cmdSave.Enabled = False
        lnkAdressAuswahl.Enabled = False
        lnkFahrzeugAuswahl.Enabled = False
        DoSubmit()
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(e.SortExpression)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change81_4.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/SonstigeDL
' mögliche try catches entfernt
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:50
' Updated in $/CKAG/Components/ComCommon/SonstigeDL
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 9.10.08    Time: 15:06
' Created in $/CKAG/Components/ComCommon/SonstigeDL
' 
' *****************  Version 1  *****************

