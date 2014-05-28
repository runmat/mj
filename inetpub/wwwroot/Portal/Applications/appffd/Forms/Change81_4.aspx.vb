Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.Text


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
    Private objHandler As MDR_06

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

        lnkFahrzeugsuche.NavigateUrl = "Change81.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugAuswahl.NavigateUrl = "Change81_2.aspx?AppID=" & Session("AppID").ToString
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHandler") Is Nothing Then
                Response.Redirect("Change81.aspx?AppID=" & Session("AppID").ToString)
            End If

            objHandler = CType(Session("objHandler"), MDR_06)

            Dim tmpDataView As New DataView()
            tmpDataView = objHandler.Fahrzeuge.DefaultView

            tmpDataView.RowFilter = ""
            Dim intFahrzeugBriefe As Int32 = tmpDataView.Count

            If intFahrzeugBriefe = 0 Then
                'Schrott! Weg hier!
                Response.Redirect("Change81.aspx?AppID=" & Session("AppID").ToString)
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
            objHandler.Change(Session("AppID").ToString, Session.SessionID.ToString, Me)
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

            Dim tblTemp As DataTable = objHandler.Fahrzeuge.Copy
            tblTemp.Columns(0).ColumnName = "Händlernummer"
            tblTemp.Columns(1).ColumnName = "Name"
            tblTemp.Columns(2).ColumnName = "Straße"
            tblTemp.Columns(3).ColumnName = "PLZ"
            tblTemp.Columns(4).ColumnName = "Ort"
            tblTemp.Columns(5).ColumnName = "Fahrgestellnummer"
            tblTemp.Columns(8).ColumnName = "Ersteller"
            tblTemp.Columns(9).ColumnName = "Bemerkung"
            tblTemp.Columns(10).ColumnName = "Datum"

            'Maximale Zeilenlänge entfernen, damit Anzahltext Platz hat
            tblTemp.Columns(0).MaxLength = -1
            tblTemp.AcceptChanges()
            'hinzufügen Gesamtanzahl der Beauftragungen
            Dim objTemp(tblTemp.Columns.Count - 1) As Object
            Dim i As Int32
            objTemp(0) = "Anzahl gesamt: " & tblTemp.Rows.Count.ToString
            For i = 1 To objTemp.Length - 1
                If tblTemp.Columns(i).DataType.FullName = "System.DateTime" Then
                    objTemp(i) = Today
                Else
                    objTemp(i) = String.Empty
                End If
            Next
            tblTemp.Rows.Add(objTemp)

            excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Me.Page)

        Catch ex As Exception
            lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
        End Try
    End Sub


    Private Function generateHTMLPrintVersion(ByVal dataSource As DataTable) As String
        Dim strBuilder As New StringBuilder()
        Dim row As DataRow
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
            strBuilder.Append("<td><FONT size=""1""><b>" & row.Item("Händlernummer") & "</b></FONT></td>")
            strBuilder.Append("<td><FONT size=""1""><b>" & row.Item("Name") & "</b><br>" & row.Item("Straße") & "<br>" & row.Item("PLZ") & "&nbsp" & row.Item("Ort") & "</FONT></td>")
            strBuilder.Append("<td><FONT size=""1"">" & row.Item("Vermarktungscode") & "</FONT></td>")
            strBuilder.Append("<td><FONT size=""1"">" & row.Item("Versandart") & "</FONT></td>")
            strBuilder.Append("<td><FONT size=""1""><b>" & row.Item("Fahrgestellnummer") & "</b></FONT></td>")
            strBuilder.Append("<td><FONT size=""1"">" & row.Item("Ersteller") & "</FONT></td>")
            strBuilder.Append("<td><FONT size=""1"">" & row.Item("Bemerkung") & "</FONT></td>")
            strBuilder.Append("<td><FONT size=""1"">" & row.Item("Datum") & "</FONT></td>")
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
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
        Dim tblTemp As DataTable = objHandler.Fahrzeuge.Copy

        tblTemp.Columns(0).ColumnName = "Händlernummer"
        tblTemp.Columns(1).ColumnName = "Name"
        tblTemp.Columns(2).ColumnName = "Straße"
        tblTemp.Columns(3).ColumnName = "PLZ"
        tblTemp.Columns(4).ColumnName = "Ort"
        tblTemp.Columns(5).ColumnName = "Fahrgestellnummer"
        tblTemp.Columns(8).ColumnName = "Ersteller"
        tblTemp.Columns(9).ColumnName = "Bemerkung"
        tblTemp.Columns(10).ColumnName = "Datum"

        'Maximale Zeilenlänge entfernen, damit Anzahltext Platz hat
        tblTemp.Columns(0).MaxLength = -1
        tblTemp.AcceptChanges()
        'hinzufügen Gesamtanzahl der Beauftragungen zur ausgabetabelle
        Dim objTemp(tblTemp.Columns.Count - 1) As Object
        Dim i As Int32
        objTemp(0) = "Anzahl gesamt: " & tblTemp.Rows.Count.ToString
        'objTemp(1) = tblTemp.Rows.Count.ToString
        For i = 1 To objTemp.Length - 1
            If tblTemp.Columns(i).DataType.FullName = "System.DateTime" Then
                objTemp(i) = Today
            Else
                objTemp(i) = String.Empty
            End If
        Next
        tblTemp.Rows.Add(objTemp)


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
        strHtmlCode = strHtmlCode & "<td width=""35px""><FONT size=""1""><b><u>VMA</u>*</b></FONT></td>"
        strHtmlCode = strHtmlCode & "<td width=""35px""><FONT size=""1""><b><u>VA</u>**</b></FONT></td>"
        strHtmlCode = strHtmlCode & "<td width=""120px"" ><FONT size=""1""><b><u>Fahrgestellnummer</u></b></FONT></td>"
        strHtmlCode = strHtmlCode & "<td width=""40px""><FONT size=""1""><b><u>Ersteller</u></b></FONT></td>"
        strHtmlCode = strHtmlCode & "<td width=""110px"" ><FONT size=""1""><b><u>Bemerkung</u></b></FONT></td>"
        strHtmlCode = strHtmlCode & "<td width=""40px""><FONT size=""1""><b><u>Datum</u></b></FONT></td>"
        strHtmlCode = strHtmlCode & "</tr>"
        strHtmlCode = strHtmlCode & strTrennLinie

        strHtmlCode = strHtmlCode & generateHTMLPrintVersion(tblTemp)
        strHtmlCode = strHtmlCode & strTrennLinie
        strHtmlCode = strHtmlCode & "<tr><td colspan=""9""><b><FONT size=""1"">*</b> Vermarktung</FONT><td><tr>"
        strHtmlCode = strHtmlCode & "<tr><td colspan=""9""><b><FONT size=""1"">**</b> Versandart</FONT><td><tr>"
        strHtmlCode = strHtmlCode & strTrennLinie2
        strHtmlCode = strHtmlCode & "</table>"
        replaceGerUmlauteForHTML(strHtmlCode)

        strHtmlCodeComplete = "<html><head><title>(Druckversion Versandbeauftragung: " & Date.Now & ")</title></head><body bgcolor=""#FFFFFF""><font size=""2"" style=""Arial""><b><a href=""javascript:window.print()"">Auftrag drucken...</a></b></font>" & strHtmlCode & "</body></html>"

        Dim objStreamWriter As System.IO.StreamWriter

        objStreamWriter = New System.IO.StreamWriter(ConfigurationManager.AppSettings("ExcelPath") & strFileName & ".htm", False) 'C:\Inetpub\wwwroot" & "\Portal\Temp\Excel\
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
' $History: Change81_4.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 26.06.09   Time: 10:33
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 11.02.08   Time: 8:35
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ITA 1673 fertig/Druckversion Versandbeauftragung
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 7.02.08    Time: 9:58
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' unfertig ITA 1673
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 13.11.07   Time: 17:19
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 13.11.07   Time: 14:57
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 4  *****************
' User: Uha          Date: 22.08.07   Time: 14:14
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ITA 1208: Excel-Ergebnisausgabe hinzugefügt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 22.08.07   Time: 13:23
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ITA 1208: Bugfixing 1
' 
' *****************  Version 2  *****************
' User: Uha          Date: 22.08.07   Time: 12:30
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ITA 1208 Testversion
' 
' *****************  Version 1  *****************
' User: Uha          Date: 21.08.07   Time: 17:37
' Created in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ITA 1208: Kompilierfähige Vorversion mit Teilfunktionalität
' 
' ************************************************
