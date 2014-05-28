Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Change80_4
    Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist f�r den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist f�r den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objSuche As Search
    Private objAddressList As Search
    Private objHaendler As ALD_1
    Private preise As DataTable
    Private angefordert As Integer  'Fahrzeuge, die laut Preisstaffelung berechnet werden (nicht gesperrt angelegt)
    Private preis_stueck As Decimal 'Preis pro Fahrzeug
    Private blnSwitchOnly As Boolean

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
    Private versandart As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        versandart = Request.QueryString.Item("art")
        lnkFahrzeugsuche.NavigateUrl = "Change80.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart
        lnkFahrzeugAuswahl.NavigateUrl = "Change80_2.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart
        lnkAdressAuswahl.NavigateUrl = "Change80_3.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart

        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            If m_User.Organization.OrganizationAdmin Then
                lblHead.Text = Replace(lblHead.Text, " (Beauftragung)", "")
            End If
            ucStyles.TitleText = lblHead.Text

            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change80.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart)
            End If

            objHaendler = CType(Session("objHaendler"), ALD_1)

            Dim tmpDataView As New DataView()
            tmpDataView = objHaendler.Fahrzeuge.DefaultView

            blnSwitchOnly = False
            tmpDataView.RowFilter = "MANDT = '99'"
            Dim intFahrzeugBriefe As Int32 = tmpDataView.Count
            tmpDataView.RowFilter = ""

            If intFahrzeugBriefe = 0 Then
                tmpDataView.RowFilter = "SWITCH = 1"
                intFahrzeugBriefe = tmpDataView.Count
                tmpDataView.RowFilter = ""

                If intFahrzeugBriefe = 0 Then
                    'Schrott! Weg hier!
                    Response.Redirect("Change80.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart)
                Else
                    blnSwitchOnly = True
                End If
            End If

            If blnSwitchOnly Then
                lnkAdressAuswahl.Visible = False
            End If

            If Not IsPostBack Then
                InitialLoad()
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub InitialLoad()
        Dim str As String = String.Empty

        If blnSwitchOnly Then
            lblVersandart.Text = "Wird nicht ge�ndert."
            objHaendler.Versandart = "Wird nicht ge�ndert."
        Else
            lblVersandart.Text = objHaendler.Versandart         'Versandart (Uhrzeit)
        End If
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
            If blnSwitchOnly Then
                objHaendler.VersandAdresseText = "Wird nicht ge�ndert."
                lblVersand.Text = "Wird nicht ge�ndert."
            Else
                lblVersand.Text = objHaendler.VersandAdresseText
            End If

            lblVersandGrund.Visible = False
            lblVersandgrundText.Visible = False
        End If
        lblVersandGrund.Text = objHaendler.VersandGrundText
        FillGrid()
    End Sub

    Private Sub DoSubmit()
        Dim logApp As Base.Kernel.Logging.Trace
        logApp = CType(Session("logObj"), Base.Kernel.Logging.Trace)

        objHaendler.StandardLogID = logApp.LogStandardIdentity

        Try
            If (Not blnSwitchOnly) And (objHaendler.VersandAdresseText.Length = 0) Then
                Response.Redirect(lnkFahrzeugAuswahl.NavigateUrl)
            Else
                Dim tmpDataView As New DataView()
                tmpDataView = objHaendler.Fahrzeuge.DefaultView
                Dim intItemCounter As Int32
                Dim blnPerformedWithoutError As Boolean = True

                tmpDataView.RowFilter = "MANDT = '99'"                                  'F�r ALD

                'angefordert = 0

                For intItemCounter = 0 To tmpDataView.Count - 1
                    'Daten sammeln
                    With objHaendler
                        '.KUNNR = Right("0000000000" & m_User.KUNNR, 10)                 'Kunnr. ALD

                        'Beim ALD leer
                        .Haendlernummer = ""    'Addressnr. Haendler
                        '.Haendlernummer = Right("0000000000" & m_User.Reference, 10)    'Addressnr. Haendler

                        .HalterNummer = ""                                              'Addressnr. Halter (wird nicht ben�tigt...)
                        .ScheinSchildernummer = ""                                      'Addressnr. Schein und Schilder (wird nicht ben�tigt...)
                        .VersandAdresse_ZE = objHaendler.VersandAdresse_ZE                    'Addressnr. Breifversand
                        .VersandAdresse_ZS = objHaendler.VersandAdresse_ZS                    'Addressnr. Breifversand
                        If (versandart = "TEMP") Then
                            .Versandart = "1"                                               'Tempor�r!
                        Else
                            .Versandart = "2"                                               'Endg�ltig!
                        End If
                        .Equimpent = tmpDataView.Item(intItemCounter)("EQUNR").ToString
                        .SucheFahrgestellNr = tmpDataView.Item(intItemCounter)("CHASSIS_NUM").ToString
                        .Kennzeichen = tmpDataView.Item(intItemCounter)("LICENSE_NUM").ToString
                        .TIDNr = tmpDataView.Item(intItemCounter)("TIDNR").ToString
                        .LizenzNr = tmpDataView.Item(intItemCounter)("LIZNR").ToString
                        .Materialnummer = objHaendler.Materialnummer
                    End With
                    'und anfordern...Bei Versandart = "ENDG" werden die Auftr�ge nicht sofort in SAP geschrieben, sondern auf SQL-Server zwischengespeichert.
                    'objHaendler.Anfordern(versandart)
                    objHaendler.Anfordern(m_User.Organization.OrganizationAdmin)

                    tmpDataView.Item(intItemCounter)("STATUS") = objHaendler.Auftragsstatus
                    tmpDataView.Table.AcceptChanges()

                Next

                tmpDataView.RowFilter = "SWITCH = 1"

                For intItemCounter = 0 To tmpDataView.Count - 1
                    'Daten sammeln
                    With objHaendler
                        '.KUNNR = Right("0000000000" & m_User.KUNNR, 10)                 'Kunnr. ALD

                        'Beim ALD leer
                        .Haendlernummer = ""    'Addressnr. Haendler
                        '.Haendlernummer = Right("0000000000" & m_User.Reference, 10)    'Addressnr. Haendler

                        .HalterNummer = ""                                              'Addressnr. Halter (wird nicht ben�tigt...)
                        .ScheinSchildernummer = ""                                      'Addressnr. Schein und Schilder (wird nicht ben�tigt...)
                        .VersandAdresse_ZE = objHaendler.VersandAdresse_ZE                    'Addressnr. Breifversand
                        .VersandAdresse_ZS = objHaendler.VersandAdresse_ZS                    'Addressnr. Breifversand
                        .Versandart = "3"
                        .Equimpent = tmpDataView.Item(intItemCounter)("EQUNR").ToString
                        .SucheFahrgestellNr = tmpDataView.Item(intItemCounter)("CHASSIS_NUM").ToString
                        .Kennzeichen = tmpDataView.Item(intItemCounter)("LICENSE_NUM").ToString
                        .TIDNr = tmpDataView.Item(intItemCounter)("TIDNR").ToString
                        .LizenzNr = tmpDataView.Item(intItemCounter)("LIZNR").ToString
                        .Materialnummer = objHaendler.Materialnummer
                    End With
                    objHaendler.Anfordern(m_User.Organization.OrganizationAdmin)

                    tmpDataView.Item(intItemCounter)("STATUS") = objHaendler.Auftragsstatus
                    tmpDataView.Table.AcceptChanges()

                Next

                tmpDataView.RowFilter = "MANDT = '99' OR SWITCH = 1"

                DataGrid1.DataSource = tmpDataView
                DataGrid1.DataBind()
            End If
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

        tmpDataView.RowFilter = "MANDT = '99' OR SWITCH = 1"
        DataGrid1.DataSource = tmpDataView
        DataGrid1.DataBind()

        DataGrid1.PagerStyle.Visible = False

        Dim intZaehl0099 As Int32 = 0
        lblMessage.Text = ""
        intZaehl0099 = 0                            'Anforderungen z�hlen
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

        Try
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
        Catch ex As Exception
            lblError.Text = "Fehler bei der Ermittlung der Gesamtsumme."
        End Try
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
' $History: Change80_4.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 29.04.09   Time: 14:58
' Updated in $/CKAG/Applications/appALD/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 17.04.08   Time: 15:15
' Created in $/CKAG/Applications/appald/Forms
' 
' *****************  Version 12  *****************
' User: Uha          Date: 20.06.07   Time: 15:25
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingef�hrt
' 
' *****************  Version 11  *****************
' User: Uha          Date: 5.03.07    Time: 16:59
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Forms
' 
' ************************************************
