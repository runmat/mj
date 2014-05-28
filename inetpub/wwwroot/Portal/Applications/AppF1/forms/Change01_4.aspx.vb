Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business
Imports CKG

Public Class Change01_4
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objSuche As Search
    Private objAddressList As Search
    Private objHaendler As F1_Haendler

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents Kopfdaten1 As Kopfdaten

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkFahrzeugsuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkFahrzeugAuswahl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkAdressAuswahl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersandart As System.Web.UI.WebControls.Label
    Protected WithEvents lblMaterialNummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersandhinweis As System.Web.UI.WebControls.Label
    Protected WithEvents lblAddress As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblError As System.Web.UI.WebControls.Label

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
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Kopfdaten1.Message = ""
        lnkFahrzeugAuswahl.NavigateUrl = "Change01_2.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugsuche.NavigateUrl = "Change01_1.aspx?AppID=" & Session("AppID").ToString
        lnkAdressAuswahl.NavigateUrl = "Change01_3.aspx?AppID=" & Session("AppID").ToString

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change01_1.aspx?AppID=" & Session("AppID").ToString)
            End If

            If (Session("objSuche") Is Nothing) Then
                Response.Redirect("Change01_1.aspx?AppID=" & Session("AppID").ToString)
            Else
                objSuche = CType(Session("objSuche"), Search)
            End If
            Kopfdaten1.SapInterneHaendlerNummer = objSuche.REFERENZ
            Kopfdaten1.UserReferenz = m_User.Reference
            Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
            Dim strTemp As String = objSuche.NAME
            If objSuche.NAME_2.Length > 0 Then
                strTemp &= "<br>" & objSuche.NAME_2
            End If
            Kopfdaten1.HaendlerName = strTemp
            Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET
            Kopfdaten1.Kontingente = objSuche.Kontingente
            Session("objSuche") = objSuche

            objHaendler = CType(Session("objHaendler"), F1_Haendler)

            If Not IsPostBack Then
                InitialLoad()
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub InitialLoad()

        lnkAdressAuswahl.Visible = True
        lnkFahrzeugAuswahl.Visible = True
        FillGrid()
        DataGrid1.Columns(8).Visible = False
        DataGrid1.Columns(9).Visible = False

        lblAddress.Text = Session("SelectedDeliveryText").ToString
        Dim strNullen As String = "00000000000000"

        Select Case CType(Session("Materialnummer"), String)
            Case "1391"
                lblMaterialNummer.Text = strNullen & "1391"
                lblVersandart.Text = Session("VersandArtText").ToString
            Case "1385"
                lblMaterialNummer.Text = strNullen & "1385"
                lblVersandart.Text = Session("VersandArtText").ToString
            Case "1389"
                lblMaterialNummer.Text = strNullen & "1389"
                lblVersandart.Text = Session("VersandArtText").ToString
            Case "1390"
                lblMaterialNummer.Text = strNullen & "1390"
                lblVersandart.Text = Session("VersandArtText").ToString
            Case "5530"
                lblMaterialNummer.Text = strNullen & "5530"
                lblVersandart.Text = Session("VersandArtText").ToString
        End Select
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim sLogmessage As String = ""
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        objHaendler.StandardLogID = logApp.LogStandardIdentity
        Try
            If Session("SelectedDeliveryValue").ToString.Length = 0 Then
                Response.Redirect(lnkFahrzeugAuswahl.NavigateUrl)
            Else
                Dim tmpDataView As New DataView()
                tmpDataView = objHaendler.Fahrzeuge.DefaultView

                tmpDataView.RowFilter = "MANDT <> '0'"
                objHaendler.Adresse = Session("SelectedDeliveryValue").ToString

                Dim intItemCounter As Int32
                Dim blnPerformedWithoutError As Boolean = True
                Dim blnErrorinTable As Boolean = False

                For intItemCounter = 0 To tmpDataView.Count - 1
                    If CStr(tmpDataView.Item(intItemCounter)("COMMENT")).Length = 0 Then
                        objHaendler.ZZFAHRG = tmpDataView.Item(intItemCounter)("ZZFAHRG").ToString
                        objHaendler.KreditkontrollBereich = tmpDataView.Item(intItemCounter)("MANDT").ToString
                        objHaendler.MaterialNummer = lblMaterialNummer.Text
                        objHaendler.KUNNR = m_User.KUNNR

                        objHaendler.Anfordern(Session("AppID").ToString, Session.SessionID)

                        If objHaendler.Status = -1111 Then
                            blnPerformedWithoutError = False
                            lblError.Text = objHaendler.Message
                            Exit For
                        End If

                        Dim sngStart As Single = CSng(Microsoft.VisualBasic.Timer)
                        Dim intStart As Int32 = 0
                        Do While CSng(Microsoft.VisualBasic.Timer) < sngStart + 1
                            intStart += 1
                        Loop

                        tmpDataView.Item(intItemCounter)("VBELN") = objHaendler.Auftragsnummer
                        If objHaendler.Auftragsnummer.Length = 0 Then
                            tmpDataView.Item(intItemCounter)("COMMENT") = "Fehler: " & objHaendler.Message
                            blnErrorinTable = True
                        Else
                            tmpDataView.Item(intItemCounter)("COMMENT") = objHaendler.Auftragsstatus

                        End If
                    End If
                Next

                If blnErrorinTable = True Then

                    If objHaendler.Customer.TrimStart("0"c).StartsWith("60") Then
                        sLogmessage = "Es konnten nicht alle Anforderungen zur Händler-Nr. " & objHaendler.Customer.TrimStart("0"c).Substring(2, objHaendler.Customer.TrimStart("0"c).Length - 2) & " verarbeitet werden."
                    Else
                        sLogmessage = "Es konnten nicht alle Anforderungen zur Händler-Nr. " & objHaendler.Customer.TrimStart("0"c) & " verarbeitet werden."
                    End If

                Else
                    If objHaendler.Customer.TrimStart("0"c).StartsWith("60") Then
                        sLogmessage = "Dokumentenanforderung zu Händler-Nr. " & objHaendler.Customer.TrimStart("0"c).Substring(2, objHaendler.Customer.TrimStart("0"c).Length - 2) & " erfolgreich durchgeführt."
                    Else
                        sLogmessage = "Dokumentenanforderung zu Händler-Nr. " & objHaendler.Customer.TrimStart("0"c) & " erfolgreich durchgeführt."
                    End If

                End If

                If blnPerformedWithoutError Then
                    DataGrid1.DataSource = tmpDataView
                    DataGrid1.DataBind()
                    DataGrid1.Columns(9).Visible = True
                    DataGrid1.Columns(10).Visible = True
                    Dim iCount As Integer
                    For iCount = 11 To 16
                        DataGrid1.Columns(iCount).Visible = False
                    Next iCount
                 
                    'spalte entfernen entfernen
                    DataGrid1.Columns(0).Visible = False

                    Dim item As DataGridItem
                    Dim cell As TableCell
                    Dim chkBox As CheckBox
                    Dim control As Control
                    Dim strKKB As String

                    For Each item In DataGrid1.Items
                        strKKB = item.Cells(1).Text

                        Select Case strKKB
                            Case "1"
                                item.Cells(16).Text = "Standard temporär"
                            Case "2"
                                item.Cells(16).Text = "Standard endgültig"
                            Case "3"
                                item.Cells(16).Text = "Retail"
                            Case "4"
                                item.Cells(16).Text = "Erweitertes Zahlungsziel (Delayed Payment) endgültig"
                            Case "6"
                                item.Cells(16).Text = "KF/KL"
                        End Select

                        For Each cell In item.Cells
                            For Each control In cell.Controls
                                If TypeOf control Is CheckBox Then
                                    chkBox = CType(control, CheckBox)
                                    Select Case chkBox.ID
                                        Case "chk0001"
                                            If strKKB = "1" Then
                                                chkBox.Checked = True
                                            Else
                                                chkBox.Checked = False
                                            End If
                                        Case "chk0002"
                                            If strKKB = "2" Then
                                                chkBox.Checked = True
                                            Else
                                                chkBox.Checked = False
                                            End If
                                        Case "chk0003"
                                            If strKKB = "3" Then
                                                chkBox.Checked = True
                                            Else
                                                chkBox.Checked = False
                                            End If
                                        Case "chk0004"
                                            If strKKB = "4" Then
                                                chkBox.Checked = True
                                            Else
                                                chkBox.Checked = False
                                            End If
                                        Case "chk0006"
                                            If strKKB = "6" Then
                                                chkBox.Checked = True
                                            Else
                                                chkBox.Checked = False
                                            End If
                                    End Select
                                End If
                            Next

                        Next
                    Next

                    Dim tblTemp As New DataTable()
                    Dim i As Int32


                    tblTemp.Columns.Add("Händlernr.", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Fahrgestellnr.", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Kundenreferenz", System.Type.GetType("System.String"))
                    For i = 0 To tmpDataView.Count - 1
                        If tmpDataView(i)("MANDT").ToString = "3" Then tblTemp.Columns.Add("Anfragenr.", System.Type.GetType("System.String"))
                    Next i
                    tblTemp.Columns.Add("Nummer ZBII", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Kennzeichen", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Kundenreferenz 2", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Equipmentnr.", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("COC", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Auftragsnr.", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Kommentar", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Kontingentart", System.Type.GetType("System.String"))
                    tblTemp.Columns.Add("Abrufgrund", System.Type.GetType("System.String"))


                    Dim tmpRow As DataRow
                    For i = 0 To tmpDataView.Count - 1
                        tmpRow = tblTemp.NewRow

                        'in dem LogFile soll die Händlernummer ohne vorstehende 60 sein. JJU2008.06.26
                        If objHaendler.Customer.TrimStart("0"c).StartsWith("60") Then
                            tmpRow("Händlernr.") = objHaendler.Customer.TrimStart("0"c).Substring(2, objHaendler.Customer.TrimStart("0"c).Length - 2)
                        Else
                            tmpRow("Händlernr.") = objHaendler.Customer.TrimStart("0"c)
                        End If
                        tmpRow("Fahrgestellnr.") = tmpDataView(i)("ZZFAHRG")
                        tmpRow("Kundenreferenz") = tmpDataView(i)("LIZNR")
                        tmpRow("Abrufgrund") = tmpDataView(i)("AUGRU_Klartext")
                        If tmpDataView(i)("MANDT").ToString = "3" Then tmpRow("Anfragenr.") = tmpDataView(i)("TEXT300")
                        tmpRow("Nummer ZBII") = tmpDataView(i)("TIDNR")
                        tmpRow("Kennzeichen") = tmpDataView(i)("LICENSE_NUM")
                        tmpRow("Kundenreferenz 2") = tmpDataView(i)("ZZREFERENZ1")
                        If Not TypeOf tmpDataView(i)("EQUNR") Is System.DBNull Then
                            tmpRow("Equipmentnr.") = CStr(tmpDataView(i)("EQUNR")).TrimStart("0"c)
                        End If
                        tmpRow("COC") = "Nein"
                        If tmpDataView(i)("ZZCOCKZ").ToString = "True" Then
                            tmpRow("COC") = "Ja"
                        End If


                        tmpRow("Auftragsnr.") = tmpDataView(i)("VBELN")
                            tmpRow("Kommentar") = tmpDataView(i)("COMMENT")
                            Select Case tmpDataView(i)("MANDT").ToString
                                Case "1"
                                    tmpRow("Kontingentart") = "Standard temporär"
                                Case "2"
                                    tmpRow("Kontingentart") = "Standard endgültig"
                                Case "3"
                                    tmpRow("Kontingentart") = "Retail"
                                Case "4"
                                    tmpRow("Kontingentart") = "Erweitertes Zahlungsziel (Delayed Payment) endgültig"
                                Case "6"
                                    tmpRow("Kontingentart") = "KF/KL"
                            End Select

                            tblTemp.Rows.Add(tmpRow)
                    Next

                    Dim tblLog As DataTable = tblTemp.Copy
                    tblLog.Columns.Remove("Equipmentnr.")
                    tblLog.Columns.Remove("Auftragsnr.")

                    logApp.UpdateEntry("APP", Session("AppID").ToString, sLogmessage, tblLog)

                    objSuche.fillHaendlerData(Session("AppID").ToString, Session.SessionID, objSuche.REFERENZ)

                    Kopfdaten1.Kontingente = objSuche.Kontingente

                    cmdSave.Visible = False
                    Session("SelectedDeliveryValue") = ""
                    lnkAdressAuswahl.Visible = False
                    lnkFahrzeugAuswahl.Visible = False
                    lblMessage.Text = "Sie haben folgenden Versandauftrag erstellt:"
                Else
                    InitialLoad()
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Fehler: " & ex.Message
            logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler bei der Dokumentenanforderung zu Adresse-Nr. " & objHaendler.Adresse &
                               ", Fehler: " & ex.Message & ")")
        End Try
    End Sub

    'Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    DoSubmit()
    'End Sub

    Private Sub FillGrid(Optional ByVal strSort As String = "")
        Dim tmpDataView As DataView = objHaendler.Fahrzeuge.DefaultView

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

        Dim item As DataGridItem
        Dim cell As TableCell
        Dim chkBox As CheckBox
        Dim control As Control
        Dim blnScriptFound As Boolean = False
        Dim intZaehl0001 As Int32 = 0
        Dim intZaehl0002 As Int32 = 0
        Dim intZaehl0003 As Int32 = 0
        Dim intZaehl0004 As Int32 = 0
        Dim intZaehl0006 As Int32 = 0
        Dim blnBezahlt As Boolean
        Dim strKKB As String
        Dim blnGesperrteAnforderungen As Boolean = False

        For Each item In DataGrid1.Items
            blnBezahlt = False
            strKKB = item.Cells(1).Text

            For Each cell In item.Cells

                For Each control In cell.Controls
                    If TypeOf control Is CheckBox Then
                        chkBox = CType(control, CheckBox)
                        If chkBox.ID = "Bezahlt" And chkBox.Checked Then
                            blnBezahlt = True
                        End If
                    End If
                Next

                For Each control In cell.Controls
                    If TypeOf control Is CheckBox Then
                        chkBox = CType(control, CheckBox)
                        If Not chkBox.ID = "Bezahlt" Then
                            Select Case chkBox.ID
                                Case "chk0001"
                                    If strKKB = "1" Then
                                        chkBox.Checked = True
                                        If Not blnBezahlt Then
                                            intZaehl0001 += 1
                                        End If
                                    Else
                                        chkBox.Checked = False
                                    End If
                                Case "chk0002"
                                    If strKKB = "2" Then
                                        chkBox.Checked = True
                                        If Not blnBezahlt Then
                                            intZaehl0002 += 1
                                        End If
                                    Else
                                        chkBox.Checked = False
                                    End If
                                Case "chk0003"
                                    If strKKB = "3" Then
                                        chkBox.Checked = True
                                        intZaehl0003 += 1
                                    Else
                                        chkBox.Checked = False
                                    End If
                                Case "chk0004"
                                    If strKKB = "4" Then
                                        chkBox.Checked = True
                                        intZaehl0004 += 1
                                    Else
                                        chkBox.Checked = False
                                    End If
                                Case "chk0006"
                                    If strKKB = "6" Then
                                        chkBox.Checked = True
                                        intZaehl0006 += 1
                                    Else
                                        chkBox.Checked = False
                                    End If
                            End Select
                        End If
                    End If
                Next
            Next
        Next

        For Each item2 As DataGridItem In DataGrid1.Items
            If Not item2.FindControl("lnkHistorie") Is Nothing Then
                If Not m_User.Applications.Select("AppName = 'Report46'").Count = 0 Then
                    CType(item2.FindControl("lnkHistorie"), HyperLink).NavigateUrl = "../../../Components/ComCommon/Finance/Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN=" & CType(item.FindControl("lnkHistorie"), HyperLink).Text
                End If
            End If
        Next

        lblMessage.Text = lblMessage.Text & "<br>Zur Aktivierung der ausgewählten Vorgänge klicken Sie auf [Absenden].<br>"

        If intZaehl0003 > 0 Then
           blnGesperrteAnforderungen = True
        End If

        If intZaehl0004 > 0 Then
            blnGesperrteAnforderungen = True
        End If

        If intZaehl0006 > 0 Then
            blnGesperrteAnforderungen = True
        End If

        If Not blnGesperrteAnforderungen Then
            lblVersandhinweis.Visible = False
        End If

        DataGrid1.PagerStyle.Visible = False
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        FillGrid(e.SortExpression)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        If e.CommandName = "entfernen" Then
            objHaendler.Fahrzeuge.Select("EQUNR='" & e.CommandArgument.ToString & "'")(0)("MANDT") = "0"
            FillGrid()
        End If
    
    End Sub

End Class
' ************************************************
' $History: Change01_4.aspx.vb $
' 
' *****************  Version 17  *****************
' User: Rudolpho     Date: 30.04.09   Time: 11:39
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2837
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 3.04.09    Time: 9:05
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 1.04.09    Time: 8:32
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 30.03.09   Time: 9:10
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2664 nachbesserungen
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 25.03.09   Time: 15:38
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 24.03.09   Time: 15:38
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 20.03.09   Time: 11:58
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2664 nachbesserungen
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 20.03.09   Time: 11:27
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2664 nachbesserungen
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 20.03.09   Time: 9:12
' Updated in $/CKAG/Applications/AppF1/forms
' 2674 nachbesserungen
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 17.03.09   Time: 10:37
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 16.03.09   Time: 10:26
' Updated in $/CKAG/Applications/AppF1/forms
' ita 2685
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 13.03.09   Time: 8:21
' Updated in $/CKAG/Applications/AppF1/forms
' ITA:2665
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 9.03.09    Time: 16:42
' Updated in $/CKAG/Applications/AppF1/forms
' nachbesserungen
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 9.03.09    Time: 16:12
' Updated in $/CKAG/Applications/AppF1/forms
' ITa 2664 nachbesserungen
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 9.03.09    Time: 14:51
' Updated in $/CKAG/Applications/AppF1/forms
' 2664 testfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 4.03.09    Time: 17:30
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 4.03.09    Time: 11:12
' Created in $/CKAG/Applications/AppF1/forms
'
' ************************************************