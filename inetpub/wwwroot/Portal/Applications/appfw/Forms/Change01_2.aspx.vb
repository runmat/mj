Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.Text


Public Class Change01_2
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
    Private objHandler As Versandbeauftragung

    Protected WithEvents lnkFahrzeugsuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkFahrzeugAuswahl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucHeader As Header
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblAddress As System.Web.UI.WebControls.Label
    Protected WithEvents lblMaterialNummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lnkCreateExcel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkHTMLFormat As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        lnkFahrzeugsuche.NavigateUrl = "Change01.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugAuswahl.NavigateUrl = "Change01_1.aspx?AppID=" & Session("AppID").ToString
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHandler") Is Nothing Then
                Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString)
            End If

            objHandler = CType(Session("objHandler"), Versandbeauftragung)

            Dim tmpDataView As New DataView()
            tmpDataView = objHandler.Fahrzeuge.DefaultView

            tmpDataView.RowFilter = ""
            Dim intFahrzeugBriefe As Int32 = tmpDataView.Count

            If intFahrzeugBriefe = 0 Then
                'Schrott! Weg hier!
                Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString)
            End If

            If Not IsPostBack Then
                If objHandler.SucheHaendlernummer.Length + objHandler.EmpfaengerName.Length + objHandler.EmpfaengerOrt.Length + objHandler.EmpfaengerPLZ.Length + objHandler.EmpfaengerStrasse.Length = 0 Then
                    lnkFahrzeugsuche.Visible = False
                    lnkFahrzeugAuswahl.Visible = False
                Else
                    lnkFahrzeugsuche.Visible = True
                    lnkFahrzeugAuswahl.Visible = True
                End If

                FillGrid()
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub DoSubmit()
        cmdSave.Enabled = False
        lnkFahrzeugAuswahl.Visible = False
        lnkFahrzeugsuche.Visible = False

        Dim logApp As Base.Kernel.Logging.Trace
        logApp = CType(Session("logObj"), Base.Kernel.Logging.Trace)

        objHandler.StandardLogID = logApp.LogStandardIdentity

        Try
            objHandler.Change()
            If objHandler.Status = 0 Then
                logApp.UpdateEntry("APP", Session("AppID").ToString, lblHead.Text)
            Else
                lblError.Text = objHandler.Message
                logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler bei " & lblHead.Text & ", Fehler: " & objHandler.Message & ")")
            End If

            FillGrid()
            lblMessage.Text = "Beauftragte Positionen gesamt: " & objHandler.Fahrzeuge.Rows.Count


            Session("objHandler") = objHandler

            lnkCreateExcel.Visible = True
            lnkHTMLFormat.Visible = True
        Catch ex As Exception
            lblError.Text = "Fehler: " & ex.Message
            logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler bei " & lblHead.Text & ", Fehler: " & ex.Message & ")")
        End Try
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub

    Private Sub FillGrid(Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = objHandler.Fahrzeuge.DefaultView

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

        tmpDataView.RowFilter = ""
        DataGrid1.DataSource = tmpDataView
        DataGrid1.DataBind()

        DataGrid1.PagerStyle.Visible = False
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
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

    Private Sub lnkCreateExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCreateExcel.Click
        Try
            Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

            Dim tblTemp As DataTable = getDataTableForPrints()

            excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Me.Page)

        Catch ex As Exception
            lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
        End Try
    End Sub



    Public Function getVersandText(ByVal code As String) As String
        Dim returnText As String

        Select Case code
            Case "2035"
                returnText = "Versand Standardbrief Deutsche Post AG"
            Case "1391"
                returnText = "DHL Standardversand mit Sendungsverfolgung und Bestätigung vom Empfänger"
            Case "1390"
                returnText = "DHL Expressversand mit Zeitoption vor 12:00 Uhr"
            Case "1389"
                returnText = "DHL Expressversand mit Zeitoption vor 10:00 Uhr"
            Case "1385"
                returnText = "DHL Expressversand mit Zeitoption vor 09:00 Uhr"
            Case "9999"
                returnText = "Selbstabholer beim DAD Ahrensburg"
            Case Else
                returnText = "Versandcode " & code & " ist nicht bekannt!"
        End Select

        Return returnText

    End Function

    Public Function getBoolText(ByVal value As String) As String
        Dim returnText As String

        Select Case value
            Case "0"
                returnText = "Nein"
            Case "1"
                returnText = "Ja"
            Case Else
                returnText = "Der Boolische SAPWert " & value & " ist nicht bekannt!"
        End Select

        Return returnText

    End Function


    Private Function getDataTableForPrints() As DataTable

        '----------------------------------------------------------------------
        ' Methode: getDataTableForPrints
        ' Autor: JJU
        ' Beschreibung: erstellt eine Datentabelle die für die Druckausgabe in HTML und EXCEL benötigt wird.
        ' Erstellt am: 2008.09.09
        ' ITA: 1844
        '----------------------------------------------------------------------

        Try
            Dim tblTemp As New DataTable
            With tblTemp.Columns
                .Add("Händlernummer", GetType(System.String))
                .Add("Name", GetType(System.String))
                .Add("Straße", GetType(System.String))
                .Add("PLZ", GetType(System.String))
                .Add("Ort", GetType(System.String))
                .Add("Fahrgestellnummer", GetType(System.String))
                .Add("Versandart", GetType(System.String))
                .Add("Abmeldung erforderlich", GetType(System.String))
                .Add("Finanzierung Fordbank", GetType(System.String))
                .Add("Ersteller", GetType(System.String))
                .Add("Bemerkung", GetType(System.String))
                .Add("Datum", GetType(System.DateTime))
            End With

            'hinzufügen Gesamtanzahl der Beauftragungen sowie füllen der temporären tabelle 
            Dim tmpRow As DataRow
            Dim objTemp(tblTemp.Columns.Count - 1) As Object
            objTemp(0) = "Anzahl gesamt: " & objHandler.Fahrzeuge.Rows.Count.ToString
            For Each tmpOriginalRow As DataRow In objHandler.Fahrzeuge.Rows
                tmpRow = tblTemp.NewRow
                tmpRow("Händlernummer") = tmpOriginalRow("KUNNR_ZS").ToString
                tmpRow("Name") = tmpOriginalRow("NAME1").ToString
                tmpRow("Straße") = tmpOriginalRow("STREET").ToString
                tmpRow("PLZ") = tmpOriginalRow("POST_CODE1").ToString
                tmpRow("Ort") = tmpOriginalRow("CITY1").ToString
                tmpRow("Fahrgestellnummer") = tmpOriginalRow("CHASSIS_NUM").ToString
                tmpRow("Ersteller") = tmpOriginalRow("ERNAM").ToString
                tmpRow("Bemerkung") = tmpOriginalRow("ZFCODE").ToString
                tmpRow("Abmeldung erforderlich") = getBoolText(tmpOriginalRow("VERMARKT").ToString)
                tmpRow("Finanzierung Fordbank") = getBoolText(tmpOriginalRow("EIGENTUEMER").ToString)
                tmpRow("Versandart") = getVersandText(tmpOriginalRow("CODE_VERSANDART").ToString)
                If Not tmpOriginalRow("ERDAT") Is System.DBNull.Value Then
                    tmpRow("Datum") = CDate(tmpOriginalRow("ERDAT"))
                End If
                tblTemp.Rows.Add(tmpRow)
            Next
            tblTemp.Rows.Add(objTemp)
            tblTemp.AcceptChanges()
            Return tblTemp
        Catch ex As Exception
            lblError.Text = "Fehler bei der generierung der Druckausgabetabellen: " & ex.Message
        End Try
        Return Nothing

    End Function

    Private Function generateHTMLPrintVersion(ByVal dataSource As DataTable) As String
        Dim strBuilder As New StringBuilder()
        Dim row As DataRow
        Dim strTemp As String
        Dim i As Int32
        Dim strTrennLinie As String = "<tr><td colspan=""9""><P align=""Center"">------------------------------------------------------------------------------------------------------------------</p></td></tr>"
        Dim strTrennLinie2 As String = "<tr><td colspan=""9""><P align=""Left"">******************************************************************************</p></td></tr>"
        Dim strLeerZeile As String = "<tr><td colspan=""9""><td><tr>"

        For i = 0 To dataSource.Rows.Count - 1
            row = dataSource.Rows(i)

            If i = dataSource.Rows.Count - 1 Then 'Anzahl gesamt: zeile gesondert behandeln
                strBuilder.Append(strTrennLinie)
                strBuilder.Append(strTrennLinie)
            End If


            strBuilder.Append("<tr>")
            strBuilder.Append("<td><FONT size=""1""><b>" & row.Item("Händlernummer").ToString & "</b></FONT></td>")
            strBuilder.Append("<td><FONT size=""1""><b>" & row.Item("Name").ToString & "</b><br>" & row.Item("Straße").ToString & "<br>" & row.Item("PLZ").ToString & "&nbsp" & row.Item("Ort").ToString & "</FONT></td>")
            strBuilder.Append("<td><FONT size=""1"">" & row.Item("Abmeldung erforderlich").ToString & "</FONT></td>")
            strBuilder.Append("<td><FONT size=""1"">" & row.Item("Finanzierung Fordbank").ToString & "</FONT></td>")
            strBuilder.Append("<td><FONT size=""1""><b>" & row.Item("Fahrgestellnummer").ToString & "</b></FONT></td>")
            strBuilder.Append("<td><FONT size=""1"">" & row.Item("Ersteller").ToString & "</FONT></td>")
            strBuilder.Append("<td><FONT size=""1"">" & row.Item("Bemerkung").ToString & "</FONT></td>")
            If Not row.Item("Datum") Is DBNull.Value Then
                strBuilder.Append("<td><FONT size=""1"">" & CDate(row.Item("Datum")).ToShortDateString & "</FONT></td>")
            Else
                strBuilder.Append("<td><FONT size=""1"">" & "" & "</FONT></td>")
            End If


            strBuilder.Append("</tr>")
        Next
        Return strBuilder.ToString
    End Function

    Private Sub replaceGerUmlauteForHTML(ByRef htmlCode As String)
        htmlCode = htmlCode.Replace("ß", "&szlig;")
        htmlCode = htmlCode.Replace("ä", "&auml;")
        htmlCode = htmlCode.Replace("Ä", "&Auml;")
        htmlCode = htmlCode.Replace("ö", "&ouml;")
        htmlCode = htmlCode.Replace("Ö", "&Ouml;")
        htmlCode = htmlCode.Replace("ü", "&uuml;")
        htmlCode = htmlCode.Replace("Ü", "&Uuml;")
        htmlCode = htmlCode.Replace("€", "&euro;")
    End Sub

    Private Sub lnkHTMLFormat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkHTMLFormat.Click
        Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

        Dim tblTemp As DataTable = getDataTableForPrints()

        'htmlAusagabe generieren 
        Dim strHtmlCodeComplete As String
        Dim strHtmlCode As String
        Dim strTrennLinie As String = "<tr><td colspan=""9""><P align=""Center"">------------------------------------------------------------------------------------------------------------------</p></td></tr>"
        Dim strTrennLinie2 As String = "<tr><td colspan=""9""><P align=""Left"">******************************************************************************</p></td></tr>"
        Dim strLeerZeile As String = "<tr><td colspan=""9""><td><tr>"
        strHtmlCode = "<table cellpadding=""1"" cellSpacing=""1"" width=""630px"" >"

        strHtmlCode = strHtmlCode & strTrennLinie2
        strHtmlCode = strHtmlCode & strLeerZeile
        strHtmlCode = strHtmlCode & "<tr><td  width=""40px""><FONT size=""1""><b><u>Händler-Nr.</u></b></FONT></td>"
        strHtmlCode = strHtmlCode & "<td width=""110px""><FONT size=""1""><b><u>Händler</u></b></FONT></td>"
        strHtmlCode = strHtmlCode & "<td width=""35px""><FONT size=""1""><b><u>Abm. erf.</u>*</b></FONT></td>"
        strHtmlCode = strHtmlCode & "<td width=""35px""><FONT size=""1""><b><u>Fi. Fb.</u>**</b></FONT></td>"
        strHtmlCode = strHtmlCode & "<td width=""120px"" ><FONT size=""1""><b><u>Fahrgestellnummer</u></b></FONT></td>"
        strHtmlCode = strHtmlCode & "<td width=""40px""><FONT size=""1""><b><u>Ersteller</u></b></FONT></td>"
        strHtmlCode = strHtmlCode & "<td width=""110px"" ><FONT size=""1""><b><u>Bemerkung</u></b></FONT></td>"
        strHtmlCode = strHtmlCode & "<td width=""40px""><FONT size=""1""><b><u>Datum</u></b></FONT></td>"
        strHtmlCode = strHtmlCode & "</tr>"
        strHtmlCode = strHtmlCode & strTrennLinie

        strHtmlCode = strHtmlCode & generateHTMLPrintVersion(tblTemp)
        strHtmlCode = strHtmlCode & strTrennLinie
        strHtmlCode = strHtmlCode & "<tr><td colspan=""9""><b><FONT size=""1"">*</b> Abmeldung erforderlich</FONT><td><tr>"
        strHtmlCode = strHtmlCode & "<tr><td colspan=""9""><b><FONT size=""1"">**</b> Finanzierung Fordbank</FONT><td><tr>"
        strHtmlCode = strHtmlCode & strTrennLinie2
        strHtmlCode = strHtmlCode & "</table>"
        replaceGerUmlauteForHTML(strHtmlCode)

        strHtmlCodeComplete = "<html><head><title>(Druckversion Versandbeauftragung: " & Today.Now & ")</title></head><body bgcolor=""#FFFFFF""><font size=""2"" style=""Arial""><b><a href=""javascript:window.print()"">Auftrag drucken...</a></b></font>" & strHtmlCode & "</body></html>"

        Dim objFileInfo As System.IO.FileInfo
        Dim objStreamWriter As System.IO.StreamWriter

        objStreamWriter = New System.IO.StreamWriter("C:\Inetpub\wwwroot" & "\Portal\Temp\Excel\" & strFileName & ".htm", False)
        objStreamWriter.Write(strHtmlCodeComplete)
        objStreamWriter.Close()

        Dim openScript As String
        'spasseshalber mit JavaScript statt url linkbutton
        openScript = _
   "<" & "script language=""JavaScript"">" & _
      "var win = window.open('" & "/Portal/Temp/Excel/" & strFileName & ".htm" & "','unerheblich'," & _
         "'width=670,height=700,left=20,top=20,resizable=YES, scrollbars=YES,menubar=NO');" & _
   "</" & "script>"
        Response.Write(openScript)

    End Sub
End Class

' ************************************************
' $History: Change01_2.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 9.09.08    Time: 11:27
' Updated in $/CKAG/Applications/appfw/Forms
' ITA 1844
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 8.09.08    Time: 17:39
' Updated in $/CKAG/Applications/appfw/Forms
' ITA 1844 unfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 1.09.08    Time: 17:40
' Created in $/CKAG/Applications/appfw/Forms
' ITa 1844 Compilierfähig
' 
' ************************************************