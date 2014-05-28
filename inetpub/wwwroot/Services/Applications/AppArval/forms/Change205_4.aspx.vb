Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Services.PageElements
Imports CKG
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change205_4
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    'Private objSuche As Search
    'Private objAddressList As Search
    Private objHaendler As Arval_1
    Private preise As DataTable
    Private angefordert As Integer  'Fahrzeuge, die laut Preisstaffelung berechnet werden (nicht gesperrt angelegt)
    Private preis_stueck As Decimal 'Preis pro Fahrzeug
    Private versandart As String
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)

        versandart = Request.QueryString.Item("art")
        lnkFahrzeugAuswahl.NavigateUrl = "Change205_2.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart
        lnkFahrzeugsuche.NavigateUrl = "Change205.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart
        lnkAdressAuswahl.NavigateUrl = "Change205_3.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart

        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change205.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart)
            End If

            objHaendler = CType(Session("objHaendler"), Arval_1)

            If Not IsPostBack Then
                InitialLoad()
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub InitialLoad()
        Dim str As String = String.Empty

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

            'UH 14.06.2005
            'lblVersandart.Text = objHaendler.Versandart         'Versandart (Uhrzeit)
            'trVersandArt.Visible = True

            lblVersandGrund.Visible = True
            lblVersandgrundText.Visible = True

            'UH 14.06.2005
            'Select Case objHaendler.Materialnummer
            '    Case "1391"
            '        lblVersandart.Text = "innerhalb von 24 bis 48h"
            '    Case "1385"
            '        lblVersandart.Text = "vor 9:00 Uhr"
            '    Case "1389"
            '        lblVersandart.Text = "vor 10:00 Uhr"
            '    Case "1390"
            '        lblVersandart.Text = "vor 12:00 Uhr"
            'End Select
        Else
            lblVersand.Text = objHaendler.VersandAdresseText

            'UH 14.06.2005
            'trVersandArt.Visible = False

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
            If objHaendler.VersandAdresseText.Length = 0 Then
                Response.Redirect(lnkFahrzeugAuswahl.NavigateUrl)
            Else
                Dim tmpDataView As New DataView()
                tmpDataView = objHaendler.Fahrzeuge.DefaultView
                tmpDataView.RowFilter = "MANDT = '99'"                                  'Für ARVAL

                Dim intItemCounter As Int32
                Dim blnPerformedWithoutError As Boolean = True

                'angefordert = 0

                For intItemCounter = 0 To tmpDataView.Count - 1
                    'Daten sammeln
                    With objHaendler
                        .KUNNR = Right("0000000000" & m_User.KUNNR, 10)                 'Kunnr. Arval
                        .Haendlernummer = Right("0000000000" & m_User.Reference, 10)    'Addressnr. Haendler
                        .HalterNummer = ""                                              'Addressnr. Halter (wird nicht benötigt...)
                        .ScheinSchildernummer = ""                                      'Addressnr. Schein und Schilder (wird nicht benötigt...)
                        .VersandAdresse = objHaendler.VersandAdresse                    'Addressnr. Breifversand
                        .Versandart = "1"                                               'Temporär!
                        .Equimpent = tmpDataView.Item(intItemCounter)("EQUNR").ToString
                        .SucheFgstNr = tmpDataView.Item(intItemCounter)("CHASSIS_NUM").ToString
                        .Kennzeichen = tmpDataView.Item(intItemCounter)("LICENSE_NUM").ToString
                        .TIDNr = tmpDataView.Item(intItemCounter)("TIDNR").ToString
                        .LizenzNr = tmpDataView.Item(intItemCounter)("LIZNR").ToString
                        .Materialnummer = objHaendler.Materialnummer
                    End With
                    'und anfordern...Bei Versandart = "ENDG" werden die Aufträge nicht sofort in SAP geschrieben, sondern auf SQL-Server zwischengespeichert.
                    objHaendler.Anfordern(versandart)

                    If (objHaendler.Auftragsnummer Is Nothing) OrElse (objHaendler.Auftragsnummer.Length = 0) Then
                        tmpDataView.Item(intItemCounter)("STATUS") = "Fehler: " & objHaendler.Message & "<br>Grund: " & objHaendler.Auftragsstatus
                    Else
                        tmpDataView.Item(intItemCounter)("STATUS") = objHaendler.Auftragsstatus
                    End If
                    tmpDataView.Table.AcceptChanges()

                Next
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

        tmpDataView.RowFilter = "MANDT = '99'"
        DataGrid1.DataSource = tmpDataView
        DataGrid1.DataBind()

        DataGrid1.PagerStyle.Visible = False

        Dim intZaehl0099 As Int32 = 0

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
' $History: Change205_4.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 17.04.09   Time: 15:45
' Created in $/CKAG2/Applications/AppArval/forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:18
' Created in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 6.03.07    Time: 15:30
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' GetAppIDFromQueryString(Me) hinzugefügt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 6.03.07    Time: 14:48
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' 
' ************************************************
