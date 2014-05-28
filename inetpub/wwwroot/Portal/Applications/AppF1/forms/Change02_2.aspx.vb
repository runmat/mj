Imports CKG
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Imports System
Imports System.IO
Imports System.Collections
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization


Partial Public Class Change02_2
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private objSuche As Search
    Private objFDDBank As F1_BankBase
    Private objFDDBank2 As F1_Bank_2

    Protected WithEvents Kopfdaten1 As Kopfdaten
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        Try

            objSuche = CType(Session("objSuche"), Search)

            Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
            Dim strTemp3 As String = objSuche.NAME
            If objSuche.NAME_2.Length > 0 Then
                strTemp3 &= "<br>" & objSuche.NAME_2
            End If
            Kopfdaten1.HaendlerName = strTemp3
            Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET


            lnkVertragssuche.NavigateUrl = "Change02_1.aspx?AppID=" & Session("AppID").ToString & "&ShowAll=True"
            lnkKreditlimit.NavigateUrl = "Change02.aspx?AppID=" & Session("AppID").ToString

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            m_App = New Base.Kernel.Security.App(m_User)


            If Session("objFDDBank2") Is Nothing Then
                Session.Add("objFDDBank2", New F1_Bank_2(m_User, m_App, Session("AppID").ToString, Session.SessionID, ""))
                objFDDBank2 = CType(Session("objFDDBank2"), F1_Bank_2)
                objFDDBank2.Show(Session("AppID").ToString, Session.SessionID)
                objFDDBank2.Haendler = objSuche.REFERENZ

                If objFDDBank2.Auftraege Is Nothing Then
                    lblError.Text = "Keine Daten vorhanden"
                    lnkKreditlimit.Visible = False
                    lnkVertragssuche.Visible = False
                    Exit Sub
                End If

            Else
                objFDDBank2 = CType(Session("objFDDBank2"), F1_Bank_2)
            End If

            If objFDDBank2.Status = 0 Then
                If Not IsPostBack Then
                    ddlPageSize.Items.Add("10")
                    ddlPageSize.Items.Add("20")
                    ddlPageSize.Items.Add("50")
                    ddlPageSize.Items.Add("100")
                    ddlPageSize.Items.Add("200")
                    ddlPageSize.Items.Add("500")
                    ddlPageSize.Items.Add("1000")
                    ddlPageSize.SelectedIndex = 2

                    Dim objExcelExport As New Excel.ExcelExport()
                    Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

                    If objFDDBank2.Auftraege Is Nothing Then
                        lblError.Text = "Keine Daten vorhanden"
                        lnkKreditlimit.Visible = False
                        lnkVertragssuche.Visible = False
                        Exit Sub
                    End If
                    Dim tblTemp As DataTable = objFDDBank2.Auftraege.Copy
                    Dim tmpRows As DataRow()

                    tmpRows = objFDDBank2.Auftraege.Select("KontingentCode = '0003'")

                    If tmpRows.Length > 0 Then
                        Dim i2 As Integer

                        For i2 = 0 To tmpRows.Length - 1
                            Dim iLoeschFlag As Integer = 0
                            tmpRows(i2).BeginEdit()
                            If tmpRows(i2).Item("KontingentCode").ToString = "0003" Then
                                iLoeschFlag = 1
                            End If
                            If iLoeschFlag = 1 Then
                                tmpRows(i2).Delete()
                            End If
                            tmpRows(i2).EndEdit()
                            tblTemp.AcceptChanges()
                        Next i2
                    End If

                    tblTemp.Columns.Remove("InAutorisierung")
                    tblTemp.Columns.Remove("Initiator")
                    tblTemp.Columns.Remove("KontingentCode")
                    tblTemp.Columns.Remove("Fälligkeitsdatum")
                    tblTemp.Columns.Remove("HAENDLER_EX")
                    Session("lnkExcel") = tblTemp
                    lnkCreateExcel.Visible = True

                    FillGrid(objFDDBank2, 0)

                    If objFDDBank2.ZeigeFlottengeschaeft Then
                        DataGrid1.Columns(DataGrid1.Columns.Count - 2).Visible = True
                        DataGrid1.Columns(DataGrid1.Columns.Count - 3).Visible = True
                    Else
                        DataGrid1.Columns(DataGrid1.Columns.Count - 2).Visible = False
                        DataGrid1.Columns(DataGrid1.Columns.Count - 3).Visible = False
                    End If

                    If objFDDBank2.ZeigeHEZ Then 'Bei HEZ Spalte für Zulassungsart und Legende einblenden!
                        lblLegende.Visible = True
                        DataGrid1.Columns(DataGrid1.Columns.Count - 4).Visible = True
                    End If

                    Kopfdaten1.Kontingente = objSuche.Kontingente
                Else
                    If Request.Form("txtStorno").ToString = "X" Or Request.Form("txtFreigeben").ToString = "X" Then
                        Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
                        Literal1.Text &= "						  <!-- //" & vbCrLf
                        Literal1.Text &= "						    window.document.location.href = ""#" & CStr(Request.Form("txtAuftragsNummer")) & """;" & vbCrLf
                        Literal1.Text &= "						  //-->" & vbCrLf
                        Literal1.Text &= "						</script>" & vbCrLf

                        objFDDBank2 = CType(Session("objFDDBank2"), F1_Bank_2)
                        objFDDBank2.AuftragsNummer = Request.Form("txtAuftragsNummer").ToString
                        If Request.Form("txtStorno").ToString = "X" Then
                            objFDDBank2.Storno = True
                        Else
                            objFDDBank2.Storno = False
                        End If

                        Dim strTemp As String = ""
                        Dim strTemp2 As String = ""

                        Dim j As Int32
                        Dim strAuftragsnummer As String = Request.Form("txtAuftragsNummer").ToString
                        For j = 0 To Request.Form.Keys.Count - 1
                            If InStr(Request.Form.Keys(j).ToString, "txt" & Request.Form("txtAuftragsNummer").ToString) > 0 Then
                                strTemp = Request.Form(j).ToString
                                If (Not objFDDBank2.Storno) AndAlso (objFDDBank2.Auftraege.Select("VBELN=" & strAuftragsnummer)(0)("KontingentCode").ToString = "0004") AndAlso (strTemp.Length = 0) Then
                                    FillGrid(objFDDBank2, DataGrid1.CurrentPageIndex)
                                    Throw New System.Exception("Das Feld ""Kunde"" muss für diese Kontingentart gefüllt werden.")
                                End If
                                objFDDBank2.Kunde = strTemp
                                objFDDBank2.Auftraege.Select("VBELN=" & strAuftragsnummer)(0)("TEXT50") = strTemp ' Kunde
                            End If

                            If InStr(Request.Form.Keys(j).ToString, "dat" & Request.Form("txtAuftragsNummer").ToString) > 0 Then
                                strTemp2 = Request.Form(j).ToString
                            End If
                        Next

                        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                        logApp.CollectDetails("Händlernummer", CType(objSuche.REFERENZ, Object), True)
                        logApp.CollectDetails("Angefordert am", CType(Request.Form("txtAngefordert").ToString, Object))
                        logApp.CollectDetails("Fahrgestellnummer", CType(Request.Form("txtFahrgestellnummer").ToString, Object))
                        logApp.CollectDetails("Nummer ZBII", CType(Request.Form("txtBriefnummer").ToString, Object))
                        logApp.CollectDetails("Storno", CType(objFDDBank2.Storno, Object))
                        logApp.CollectDetails("Freigabe", CType((Not objFDDBank2.Storno), Object))


                        If (CInt(m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AuthorizationLevel")) > 0) Then
                            'Anwendung erfordert Autorisierung (Level>0)
                            Dim DetailArray(2, 2) As Object
                            Dim ms As MemoryStream
                            Dim formatter As BinaryFormatter
                            Dim b() As Byte

                            ms = New MemoryStream()
                            formatter = New BinaryFormatter()
                            formatter.Serialize(ms, objSuche)
                            b = ms.ToArray
                            ms = New MemoryStream(b)
                            DetailArray(0, 0) = ms
                            DetailArray(0, 1) = "objSuche"

                            ms = New MemoryStream()
                            formatter = New BinaryFormatter()
                            formatter.Serialize(ms, objFDDBank2)
                            b = ms.ToArray
                            ms = New MemoryStream(b)
                            DetailArray(1, 0) = ms
                            DetailArray(1, 1) = "objFDDBank2"

                            'Pruefen, ob schon in der Autorisierung.
                            Dim strInitiator As String = ""
                            Dim intAuthorizationID As Int32

                            m_App.CheckForPendingAuthorization(CInt(Session("AppID")), m_User.Organization.OrganizationId, objSuche.REFERENZ, Request.Form("txtFahrgestellnummer").ToString, m_User.IsTestUser, strInitiator, intAuthorizationID)
                            If Not strInitiator.Length = 0 Then
                                'Fahrzeug wurde schon mal freigegeben und liegt zur Autorisierung vor
                                lblError.Text = "Diese Anforderung wurde schon einmal freigegeben und liegt bereits zur Autorisierung vor!"
                                Exit Sub
                            End If

                            intAuthorizationID = WriteAuthorization(m_App.Connectionstring, CInt(Session("AppID")), m_User.UserName, m_User.Organization.OrganizationId, objSuche.REFERENZ, Request.Form("txtFahrgestellnummer").ToString, Request.Form("txtZulassungsart").ToString.Trim, "", m_User.IsTestUser, DetailArray)
                            objFDDBank2.Auftraege.Select("VBELN=" & strAuftragsnummer)(0)("InAutorisierung") = True
                            objFDDBank2.Auftraege.Select("VBELN=" & strAuftragsnummer)(0)("Initiator") = m_User.UserName

                            logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, objSuche.REFERENZ, "Freigabe für Händler " & objSuche.REFERENZ & " erfolgreich initiiert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                        Else
                            'Anwendung erfordert keine Autorisierung (Level=0)

                            objFDDBank2.Change(Session("AppId").ToString, Session.SessionID, Me)
                            If Not objFDDBank2.Status = 0 Then
                                lblError.Text = objFDDBank2.Message
                                logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, objSuche.REFERENZ, "Fehler bei der Freigabe für Händler " & objSuche.REFERENZ & " , (Fehler: " & objFDDBank2.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10, logApp.InputDetails)
                            Else
                                logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, objSuche.REFERENZ, "Freigabe für Händler " & objSuche.REFERENZ & " erfolgreich durchgeführt.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                            End If

                            logApp.WriteStandardDataAccessSAP(objFDDBank2.IDSAP)
                            objFDDBank2.Show(Session("AppId").ToString, Session.SessionID)
                        End If

                        Session("objFDDBank2") = objFDDBank2

                        FillGrid(objFDDBank2, DataGrid1.CurrentPageIndex)

                        objSuche.fillHaendlerData(Session("AppId").ToString, Session.SessionID, objSuche.REFERENZ)
                        If objSuche.Status = 0 Then
                            Kopfdaten1.Kontingente = objSuche.Kontingente

                            cmdSave.Enabled = True
                        Else
                            lblError.Text = objFDDBank.Message
                        End If

                    End If
                End If
                cmdSave.Enabled = True
            Else
                lblError.Text = objFDDBank2.Message
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Sub FillGrid(ByVal objBank As F1_Bank_2, ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If objBank.Status = 0 Then
            If objBank.Auftraege.Rows.Count = 0 Then
                DataGrid1.Visible = False
                lblNoData.Visible = True
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                ShowScript.Visible = False
            Else
                DataGrid1.Visible = True
                lblNoData.Visible = False
                ddlPageSize.Visible = True

                Dim tmpDataView As New DataView()
                tmpDataView = objBank.Auftraege.DefaultView

                Dim intTempPageIndex As Int32 = intPageIndex
                Dim strTempSort As String = ""
                Dim strDirection As String = ""

                If strSort.Trim(" "c).Length > 0 Then
                    intTempPageIndex = 0
                    strTempSort = strSort.Trim(" "c)
                    If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = strTempSort) Then
                        If ViewState("Direction") Is Nothing Then
                            strDirection = "desc"
                        Else
                            strDirection = ViewState("Direction").ToString
                        End If
                    Else
                        strDirection = "desc"
                    End If

                    If strDirection = "asc" Then
                        strDirection = "desc"
                    Else
                        strDirection = "asc"
                    End If

                    ViewState("Sort") = strTempSort
                    ViewState("Direction") = strDirection
                Else
                    If Not ViewState("Sort") Is Nothing Then
                        strTempSort = ViewState("Sort").ToString
                        If ViewState("Direction") Is Nothing Then
                            strDirection = "asc"
                            ViewState("Direction") = strDirection
                        Else
                            strDirection = ViewState("Direction").ToString
                        End If
                    End If
                End If

                If Not strTempSort.Length = 0 Then
                    tmpDataView.Sort = strTempSort & " " & strDirection
                End If

                DataGrid1.CurrentPageIndex = intTempPageIndex

                DataGrid1.DataSource = tmpDataView
                DataGrid1.DataBind()

                lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " gesperrte Aufträge gefunden."
                lblNoData.Visible = True

                If DataGrid1.PageCount > 1 Then
                    DataGrid1.PagerStyle.CssClass = "PagerStyle"
                    DataGrid1.DataBind()
                    DataGrid1.PagerStyle.Visible = True
                Else
                    DataGrid1.PagerStyle.Visible = False
                End If

                'Code zum Generieren des Button-Scriptes
                Dim item As DataGridItem
                Dim cell As TableCell
                Dim button As LinkButton
                Dim textbox As TextBox
                Dim control As Control
                Dim oLabel As Label
                Dim blnScriptFound As Boolean = False
                Dim intZaehl As Int32

                For Each item In DataGrid1.Items
                    cell = item.Cells(0)
                    Dim strAuftragsnummer As String = ""
                    For Each control In cell.Controls
                        If TypeOf control Is Label Then
                            oLabel = CType(control, Label)
                            If oLabel.ID = "Label1" Then
                                strAuftragsnummer = oLabel.Text.TrimStart("0"c)
                            End If
                        End If
                    Next
                    Dim strParameter As String = ""
                    strParameter = "'" & strAuftragsnummer & "',"

                    For intZaehl = 1 To 5
                        cell = item.Cells(intZaehl)
                        For Each control In cell.Controls
                            If TypeOf control Is Label Then
                                oLabel = CType(control, Label)
                                Dim LabelName As String = ""
                                LabelName = "Label" & intZaehl + 1
                                If oLabel.ID = LabelName Then
                                    If oLabel.Text = "&nbsp;" Then
                                        strParameter &= "'',"
                                    Else
                                        strParameter &= "'" & oLabel.Text & "',"
                                    End If
                                End If
                            End If
                        Next
                    Next

                    cell = item.Cells(8)
                    For Each control In cell.Controls
                        If TypeOf control Is TextBox Then
                            textbox = CType(control, TextBox)
                            textbox.ID = "dat" & strAuftragsnummer
                        End If
                    Next

                    cell = item.Cells(9)
                    For Each control In cell.Controls
                        If TypeOf control Is TextBox Then
                            textbox = CType(control, TextBox)
                            textbox.ID = "txt" & strAuftragsnummer
                        End If
                    Next
                    '§§§JVE 14.07.2005 <begin>: Zulassungsart als zusätztlicher Parameter für das Skript...
                    cell = item.Cells(7)
                    For Each control In cell.Controls
                        If TypeOf control Is Label Then
                            oLabel = CType(control, Label)
                            If oLabel.ID = "Label10" Then
                                If oLabel.Text = "N" Then
                                    strParameter &= "'3'"
                                End If
                                If oLabel.Text = "S" Then
                                    strParameter &= "'2'"
                                End If
                                If oLabel.Text = "V" Then
                                    strParameter &= "'4'"
                                End If
                            End If
                        End If
                    Next

                    '§§§JVE 14.07.2005 <begin>
                    cell = item.Cells(10)
                    For Each control In cell.Controls
                        If TypeOf control Is LinkButton Then
                            button = CType(control, LinkButton)
                            If button.CommandName = "Storno" Then
                                button.Attributes.Add("onClick", "DoStorno(" & strParameter.Trim(","c) & ");")
                                button.Attributes.Add("class", "StandardButtonTable")
                                blnScriptFound = True
                            End If
                            If button.CommandName = "Freigabe" Then
                                button.Attributes.Add("onClick", "DoFreigeben(" & strParameter.Trim(","c) & ");")
                                button.Attributes.Add("class", "StandardButtonTable")
                                blnScriptFound = True
                            End If
                        End If
                    Next
                Next

                If blnScriptFound Then
                    ShowScript.Visible = True
                End If
            End If
        Else
            lblError.Text = objBank.Message
            DataGrid1.Visible = False
            lblNoData.Visible = True
            ShowScript.Visible = False
        End If
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(objFDDBank2, e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(objFDDBank2, DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(objFDDBank2, 0)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
        Try
            Dim control As New Control
            Dim tblTranslations As New DataTable()
            Dim AppURL As String
            Dim m_datatable As DataTable = CType(Session("lnkExcel"), DataTable).Copy
            Dim col As DataGridColumn
            Dim col2 As DataColumn
            Dim bVisibility As Integer
            Dim i As Integer
            Dim sColName As String = ""

            With m_datatable.Columns
                .Remove("AG")
                .Remove("HDGRP")
                .Remove("HDGRP_EX")
                .Remove("HAENDLER")
                .Remove("EQUNR")
                .Remove("ZZKENN")
                .Remove("ZZKKBER")
                .Remove("ZZVSNR")
                .Remove("ANSWT")
                .Remove("ZZKKBERAnzeige")
                .Remove("ZZFAEDT")
                .Remove("TEXT50")
                .Remove("KVGR3")
            End With

            AppURL = Replace(Me.Request.Url.LocalPath, "/Portal", "..")
            tblTranslations = CType(Me.Session(AppURL), DataTable)
            For Each col In DataGrid1.Columns
                For i = m_datatable.Columns.Count - 1 To 0 Step -1
                    bVisibility = 0
                    col2 = m_datatable.Columns(i)
                    If col2.ColumnName.ToUpper = col.SortExpression.ToUpper OrElse col2.ColumnName.ToUpper = col.HeaderText.ToUpper.Replace("COL_", "") Then
                        sColName = TranslateColLbtn(DataGrid1, tblTranslations, col.HeaderText, bVisibility)
                        If bVisibility = 0 Then
                            m_datatable.Columns.Remove(col2)
                        ElseIf sColName.Length > 0 Then
                            col2.ColumnName = sColName
                        Else
                            'alle spalten die nicht in der spaltenübersetzung sind, entfernen
                            m_datatable.Columns.Remove(col2)

                        End If
                    End If
                Next
                m_datatable.AcceptChanges()
            Next
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, m_datatable, Me.Page)
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

End Class
' ************************************************
' $History: Change02_2.aspx.vb $
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 24.02.10   Time: 14:41
' Updated in $/CKAG/Applications/AppF1/forms
' ITA: 3223
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 3.04.09    Time: 9:05
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 1.04.09    Time: 8:32
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 26.03.09   Time: 11:53
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2675 anpassungen
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 25.03.09   Time: 10:21
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2741, 2670
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 23.03.09   Time: 16:33
' Updated in $/CKAG/Applications/AppF1/forms
' Autorisierung
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 17.03.09   Time: 11:38
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2675
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 16.03.09   Time: 10:26
' Updated in $/CKAG/Applications/AppF1/forms
' ita 2685
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 13.03.09   Time: 14:18
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2675 unfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 10.03.09   Time: 10:48
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2675 unfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 9.03.09    Time: 17:49
' Created in $/CKAG/Applications/AppF1/forms
' ITA 2675 unfertig
' 
' ************************************************
