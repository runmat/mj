Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change01s_4
    Inherits System.Web.UI.Page
#Region "Declarations"
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    'Private objSuche As Search
    'Private objAddressList As Search
    Private objHaendler As Versand1
    Private preise As DataTable
    Private angefordert As Integer  'Fahrzeuge, die laut Preisstaffelung berechnet werden (nicht gesperrt angelegt)
    Private preis_stueck As Decimal 'Preis pro Fahrzeug
    Private blnSwitchOnly As Boolean
    Private versandart As String
    Private authentifizierung As String

    Protected WithEvents lnkFahrzeugsuche As HyperLink
    Protected WithEvents lnkFahrzeugAuswahl As HyperLink
    Protected WithEvents lnkAdressAuswahl As HyperLink
    Protected WithEvents lblXVersandartData As Label
    Protected WithEvents lblXVersand As Label
    Protected WithEvents lbl_Versandgrund As Label
    Protected WithEvents lblXVersandGrundData As Label
    Protected WithEvents GridView1 As GridView
    Protected WithEvents cmdSearch As LinkButton
    Protected WithEvents lblHead As Label
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)

        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        versandart = Request.QueryString.Item("art")
        authentifizierung = Request.QueryString.Item("art2").ToString

        lnkFahrzeugsuche.NavigateUrl = "Change01s.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart & "&art2=" & authentifizierung
        lnkFahrzeugAuswahl.NavigateUrl = "Change01s_2.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart & "&art2=" & authentifizierung
        lnkAdressAuswahl.NavigateUrl = "Change01s_3.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart & "&art2=" & authentifizierung

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        If m_User.Organization.OrganizationAdmin Then
            lblHead.Text = Replace(lblHead.Text, " (Beauftragung)", "")
        End If
        'ucStyles.TitleText = lblHead.Text

        m_App = New Base.Kernel.Security.App(m_User)

        If Session("objHaendler") Is Nothing Then
            Response.Redirect("Change01s.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart & "&art2=" & authentifizierung)
        End If

        objHaendler = CType(Session("objHaendler"), Versand1)

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
                Response.Redirect("Change01s.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart)
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

    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        cmdSearch.Enabled = False
        lnkAdressAuswahl.Enabled = False
        lnkFahrzeugAuswahl.Enabled = False
        DoSubmit()
    End Sub

    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(e.SortExpression)
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

#End Region

#Region "Methods"
    Private Sub InitialLoad()
        Dim str As String = String.Empty

        If blnSwitchOnly Then
            lblXVersandartData.Text = "Wird nicht geändert."
            objHaendler.Versandart = "Wird nicht geändert."
        Else
            lblXVersandartData.Text = objHaendler.Versandart         'Versandart (Uhrzeit)
        End If
        tr_Versandart.Visible = True

        lblXVersandartData.Text = objHaendler.MaterialBezeichnung

        If (versandart = "TEMP") Then
            lblXVersand.Text = objHaendler.VersandAdresseText  'Versandadresse

            lbl_Versandgrund.Visible = True
            lblXVersandGrundData.Visible = True
        Else
            If blnSwitchOnly Then
                objHaendler.VersandAdresseText = "Wird nicht geändert."
                lblXVersand.Text = "Wird nicht geändert."
            Else
                lblXVersand.Text = objHaendler.VersandAdresseText
            End If

            lbl_Versandgrund.Visible = False
            lblXVersandGrundData.Visible = False
        End If
        lblXVersandGrundData.Text = objHaendler.VersandGrundText
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
                tmpDataView.RowFilter = "MANDT = '99'"                                  'Für Leaseplan

                Dim intItemCounter As Int32
                Dim blnPerformedWithoutError As Boolean = True

                'angefordert = 0

                For intItemCounter = 0 To tmpDataView.Count - 1
                    'Daten sammeln
                    With objHaendler
                        .KUNNR = Right("0000000000" & m_User.KUNNR, 10)                 'Kunnr. Leaseplan

                        'Beim Leaseplan leer
                        .Haendlernummer = ""    'Addressnr. Haendler
                        '.Haendlernummer = Right("0000000000" & m_User.Reference, 10)    'Addressnr. Haendler

                        .HalterNummer = ""                                              'Addressnr. Halter (wird nicht benötigt...)
                        .ScheinSchildernummer = ""                                      'Addressnr. Schein und Schilder (wird nicht benötigt...)
                        .VersandAdresse_ZE = objHaendler.VersandAdresse_ZE                    'Addressnr. Breifversand
                        .VersandAdresse_ZS = objHaendler.VersandAdresse_ZS                    'Addressnr. Breifversand
                        If (versandart = "TEMP") Then
                            .Versandart = "1"                                               'Temporär!
                        Else
                            .Versandart = "2"                                               'Endgültig!
                        End If
                        .Equimpent = tmpDataView.Item(intItemCounter)("EQUNR").ToString
                        .SucheFahrgestellNr = tmpDataView.Item(intItemCounter)("CHASSIS_NUM").ToString
                        .Kennzeichen = tmpDataView.Item(intItemCounter)("LICENSE_NUM").ToString
                        .TIDNr = tmpDataView.Item(intItemCounter)("TIDNR").ToString
                        .LizenzNr = tmpDataView.Item(intItemCounter)("LIZNR").ToString
                        .Materialnummer = objHaendler.Materialnummer
                    End With
                    '#############################################################################
                    'UH: 02.03.2006
                    '
                    '             Authorisierung wird über DB freigeschaltet
                    '             In Tabelle ApplicationParamlist den Parameter &art2=O
                    '             aus URL-Liste entfernen
                    '
                    '             Standard/Default: Parameter ist "O" = Authorisierung AUS!!!!
                    '
                    '#############################################################################
                    Dim blnTemp As Boolean = True
                    If Not authentifizierung = "O" Then
                        blnTemp = m_User.Organization.OrganizationAdmin
                    End If

                    '§§§ JVE 30.10.2006: Briefe von BW-Fuhrpark immer autorisieren! (Laut IT-Anforderung)
                    'If (m_User.Reference <> String.Empty) Then
                    '    blnTemp = False
                    'End If
                    '####################################################################################


                    If m_User.Groups(0).IsCustomerGroup = True Then
                        blnTemp = False
                    End If


                    objHaendler.Anfordern(blnTemp)

                    tmpDataView.Item(intItemCounter)("STATUS") = objHaendler.Auftragsstatus
                    tmpDataView.Table.AcceptChanges()

                Next

                tmpDataView.RowFilter = "SWITCH = 1"

                'angefordert = 0

                For intItemCounter = 0 To tmpDataView.Count - 1
                    'Daten sammeln
                    With objHaendler
                        .KUNNR = Right("0000000000" & m_User.KUNNR, 10)                 'Kunnr. Leaseplan

                        'Beim Leaseplan leer
                        .Haendlernummer = ""    'Addressnr. Haendler
                        '.Haendlernummer = Right("0000000000" & m_User.Reference, 10)    'Addressnr. Haendler

                        .HalterNummer = ""                                              'Addressnr. Halter (wird nicht benötigt...)
                        .ScheinSchildernummer = ""                                      'Addressnr. Schein und Schilder (wird nicht benötigt...)
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
                    '#############################################################################
                    'UH: 02.03.2006
                    '
                    '             Authorisierung wird über DB freigeschaltet
                    '             In Tabelle ApplicationParamlist den Parameter &art2=O
                    '             aus URL-Liste entfernen
                    '
                    '             Standard/Default: Parameter ist "O" = Authorisierung AUS!!!!
                    '
                    '#############################################################################
                    Dim blnTemp As Boolean = True
                    If Not authentifizierung = "O" Then
                        blnTemp = m_User.Organization.OrganizationAdmin
                    End If
                    objHaendler.Anfordern(blnTemp)

                    'nur für testzwecke, da bei LP österreich manchmal kein Material mitgegeben wird, keine Ahnung unter welchen umständen JJ2008.02.20
                    'wobei man sagen muss die fehlerbehandlung ist hier auch sehr seltsam
                    '----------------------------------------------------------------------------
                    If objHaendler.Status = -9999 Then 'unbekannter Fehler
                        Throw New Exception(objHaendler.Auftragsstatus)
                    End If
                    '----------------------------------------------------------------------------


                    tmpDataView.Item(intItemCounter)("STATUS") = objHaendler.Auftragsstatus
                    tmpDataView.Table.AcceptChanges()

                Next

                tmpDataView.RowFilter = "MANDT = '99' OR SWITCH = 1"

                GridView1.DataSource = tmpDataView
                GridView1.DataBind()
            End If
            logApp.UpdateEntry("APP", Session("AppID").ToString, "Briefversand (" & versandart & ")")
        Catch ex As Exception
            lblerror.Text = "Fehler: " & ex.Message
            logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler bei der Briefanforderung zu Equipment. " & objHaendler.Equimpent & ", Fehler: " & ex.Message & ")")
            Throw ex
        End Try
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
        GridView1.DataSource = tmpDataView
        GridView1.DataBind()

        'GridView1.PagerStyle.Visible = False

        Dim intZaehl0099 As Int32 = 0
        'lblMessage.Text = ""
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
#End Region

End Class
' ************************************************
' $History: Change01s_4.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 24.09.09   Time: 17:29
' Updated in $/CKAG2/Services/Components/ComCommon
' ITA: 3112
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
