Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Change01_4
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
    Private objHaendler As LT_01c
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
    Protected WithEvents lblVersandart As System.Web.UI.WebControls.Label
    Protected WithEvents lblMaterialNummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblVersand As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersandAdresse As System.Web.UI.WebControls.Label
    Protected WithEvents trVersandTemp As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblVersandartTxt As System.Web.UI.WebControls.Label
    Protected WithEvents trVersandArt As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblVersandGrund As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersandgrundText As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblFormular As System.Web.UI.WebControls.Label
    Protected WithEvents lblBemerkung As Label
    Private versandart As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        versandart = Request.QueryString.Item("art")
        lnkFahrzeugAuswahl.NavigateUrl = "Change01_2.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart
        lnkFahrzeugsuche.NavigateUrl = "Change01.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart
        lnkAdressAuswahl.NavigateUrl = "Change01_3.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart)
            End If

            objHaendler = CType(Session("objHaendler"), LT_01c)

            If Not IsPostBack Then
                InitialLoad()
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub InitialLoad()
        'UH 14.06.2005
        lblVersandart.Text = objHaendler.Versandart         'Versandart (Uhrzeit)
        trVersandArt.Visible = True
        Select Case objHaendler.Materialnummer
            Case "1391"
                lblVersandart.Text = "innerhalb von 24 bis 48h"
            Case "1385"
                lblVersandart.Text = "vor 9:00 Uhr"
            Case "1389"
                lblVersandart.Text = "vor 10:00 Uhr"
            Case "1390"
                lblVersandart.Text = "vor 12:00 Uhr"
        End Select

        If (versandart = "TEMP") Then
            lblVersand.Text = objHaendler.VersandAdresseText  'Versandadresse

            lblVersandGrund.Visible = True
            lblVersandgrundText.Visible = True


        Else
            lblVersand.Text = objHaendler.VersandAdresseText


            lblVersandGrund.Visible = False
            lblVersandgrundText.Visible = False
        End If
        lblVersandGrund.Text = objHaendler.VersandGrundText
        lblFormular.Text = objHaendler.ListenartText
        lblBemerkung.Text = objHaendler.Bemerkung
        FillGrid()
    End Sub

    Private Sub DoSubmit()
        Dim logApp As Base.Kernel.Logging.Trace
        logApp = CType(Session("logObj"), Base.Kernel.Logging.Trace)

        objHaendler.StandardLogID = logApp.LogStandardIdentity

        Try
            Dim tmpDataView As DataView
            tmpDataView = objHaendler.Fahrzeuge.DefaultView
            tmpDataView.RowFilter = "MANDT = '99'"                                  'Für ARVAL

            Dim intItemCounter As Int32
            'Dim blnPerformedWithoutError As Boolean = True

            For intItemCounter = 0 To tmpDataView.Count - 1

                'Daten sammeln
                With objHaendler

                    .KUNNR = Right("0000000000" & m_User.KUNNR, 10)                 'Kunnr. Arval

                    .Haendlernummer = Right("0000000000" & m_User.Reference, 10)    'Addressnr. Haendler
                    .HalterNummer = ""                                              'Addressnr. Halter (wird nicht benötigt...)
                    .ScheinSchildernummer = ""                                      'Addressnr. Schein und Schilder (wird nicht benötigt...)
                    .VersandAdresse = objHaendler.VersandAdresse                    'Addressnr. Briefversand

                    If (versandart = "TEMP") Then
                        .Versandart = "1"                                               'Temporär
                    Else
                        .Versandart = "2"                                               'Endgültig
                    End If
                    .Equimpent = tmpDataView.Item(intItemCounter)("EQUNR").ToString
                    .SucheFgstNr = tmpDataView.Item(intItemCounter)("CHASSIS_NUM").ToString
                    .Kennzeichen = tmpDataView.Item(intItemCounter)("LICENSE_NUM").ToString
                    .TIDNr = tmpDataView.Item(intItemCounter)("TIDNR").ToString
                    .LizenzNr = tmpDataView.Item(intItemCounter)("LIZNR").ToString
                    .Materialnummer = objHaendler.Materialnummer
                End With
                objHaendler.Anfordern(Me)

                If (objHaendler.Auftragsnummer Is Nothing) OrElse (objHaendler.Auftragsnummer.Length = 0) Then
                    tmpDataView.Item(intItemCounter)("STATUS") = "Fehler: " & objHaendler.Message & "<br>Grund: " & objHaendler.Auftragsstatus
                Else
                    tmpDataView.Item(intItemCounter)("STATUS") = objHaendler.Auftragsstatus
                End If
                tmpDataView.Table.AcceptChanges()

            Next
            DataGrid1.DataSource = tmpDataView
            DataGrid1.DataBind()

            logApp.UpdateEntry("APP", Session("AppID").ToString, "Briefversand (" & versandart & ")")
        Catch ex As Exception
            lblError.Text = "Fehler: " & ex.Message
            logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler bei der Briefanforderung zu Equipment. " & objHaendler.Equimpent & ", Fehler: " & ex.Message & ")")
        End Try
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub

    Private Sub FillGrid(Optional ByVal strSort As String = "")
        Dim tmpDataView As DataView
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

    'Private Sub get_stueckpreis(ByVal fahrzeuge As Int32)
    '    Dim found As Boolean = False
    '    Dim row As Integer = 0

    '    Try
    '        While Not found And row <= preise.Rows.Count - 1
    '            If CType(preise.Rows(row)("KSTBM"), Integer) <= fahrzeuge Then
    '                row += 1
    '            Else
    '                found = True
    '            End If
    '        End While

    '        If found = True Then
    '            If (row = 0) Then
    '                preis_stueck = CType(preise.Rows(row)("KBETR"), Decimal)
    '            Else
    '                preis_stueck = CType(preise.Rows(row - 1)("KBETR"), Decimal)
    '            End If
    '        Else
    '            If (row = preise.Rows.Count) Then
    '                preis_stueck = CType(preise.Rows(row - 1)("KBETR"), Decimal)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        lblError.Text = "Fehler bei der Ermittlung der Gesamtsumme."
    '    End Try
    'End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        cmdSave.Enabled = False
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
' $History: Change01_4.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 15.10.10   Time: 14:43
' Updated in $/CKAG/Applications/AppLeaseTrend/Forms
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 30.06.09   Time: 14:58
' Updated in $/CKAG/Applications/AppLeaseTrend/Forms
' ITA 2918  Z_M_FEHLENDE_VERTRAGSNUMMERN, Z_M_BRIEFLEBENSLAUF_LT,
' Z_M_BRIEFERSTEINGANG_LEASETR, Z_M_BRIEFANFORDERUNG_ALLG,
' Z_M_UNANGEFORDERT_L, Z_M_KUNDENDATEN_LT, Z_M_VERSENDETE_ZB2_ENDG_LT,
' Z_M_VERSENDETE_ZB2_TEMP_LT, Z_M_BRIEF_TEMP_VERS_MAHN_002,
' Z_M_DATEN_OHNE_ZB2_LT
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 9:12
' Updated in $/CKAG/Applications/AppLeaseTrend/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 15:07
' Created in $/CKAG/Applications/AppLeaseTrend/Forms
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 13:19
' Updated in $/CKG/Applications/AppLeaseTrend/AppLeaseTrendWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 8.03.07    Time: 10:46
' Updated in $/CKG/Applications/AppLeaseTrend/AppLeaseTrendWeb/Forms
' 
' ************************************************
