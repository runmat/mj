Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System.Threading
Imports System.Globalization

Public Class Report01_2
    Inherits System.Web.UI.Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblVermittlerAnzahl As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennzeichenGesamtbestand As System.Web.UI.WebControls.Label
    Protected WithEvents lblVerkaufteKennzeichen As System.Web.UI.WebControls.Label
    Protected WithEvents lblUnverkaufteKennzeichen As System.Web.UI.WebControls.Label
    Protected WithEvents lblRuecklaeufer As System.Web.UI.WebControls.Label
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles


    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents Table4 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblStand As System.Web.UI.WebControls.Label
    Protected WithEvents lblVerlustKennzeichen As System.Web.UI.WebControls.Label
    Protected WithEvents lblBestandDAD As System.Web.UI.WebControls.Label
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkCreateExcel As System.Web.UI.WebControls.LinkButton
    Private objHandler As VFS01



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

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            'lblHead.Text &= " - Kennzeichenbestand"
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHandler") Is Nothing Then
                Response.Redirect("Report01.aspx?AppID=" & Session("AppID").ToString())
            End If

            If Not IsPostBack Then

                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 2

                FillGrid(0)
                Dim tmpListeVermittlernummer As New System.Collections.ArrayList
                If TryGetListOfVermittlernummernFromSession(tmpListeVermittlernummer) Then
                    SetVermittlernummerInDataGridCheckedOrUnchecked()
                End If

            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub GetObjHaendler()
        If Session("objHandler") Is Nothing = False Then
            objHandler = Session("objHandler")
        End If

        'Daten holen
        Dim tmpTable As DataTable

        tmpTable = objHandler.DetailTable
        If tmpTable Is Nothing Or tmpTable.Rows.Count = 0 Then
            objHandler.GiveData(Session("AppID").ToString, Session.SessionID)
        End If
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        UpdateSessionListOfCheckedVermittlernummern()

        GetObjHaendler()

        Dim tmpDataView As New DataView()
        tmpDataView = objHandler.DetailTable.DefaultView
        tmpDataView.RowFilter = ""


        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
            ddlPageSize.Visible = False
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            lblNoData.Visible = True
            ShowScript.Visible = False

            Me.Table4.Visible = False


        Else
            lnkCreateExcel.Visible = True

            ddlPageSize.Visible = True
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

            With objHandler
                Me.lblKennzeichenGesamtbestand.Text = .KennzeichenGesamtbestand
                Me.lblRuecklaeufer.Text = .RuecklaeuferAnzahl
                Me.lblUnverkaufteKennzeichen.Text = .UnverkaufteAnzahl
                Me.lblVerkaufteKennzeichen.Text = .VerkaufteAnzahl
                Me.lblVermittlerAnzahl.Text = .VermittlerAnzahl
                Me.lblStand.Text = Right(.Erdat, 2) & "." & Mid(.Erdat, 5, 2) & "." & Left(.Erdat, 4)
                Me.lblVerlustKennzeichen.Text = .VerloreneAnzahl
                Me.lblBestandDAD.Text = .LagerAnzahl
            End With

            objHandler = Nothing

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If
        End If
        SetVermittlernummerInDataGridCheckedOrUnchecked()
    End Sub

    Private Function ClearBrForExcelOutput(ByVal strIN As String) As String
        Dim strOut As String = Replace(strIN, "-<br>", "")
        strOut = Replace(strOut, " -<br>", "")
        strOut = Replace(strOut, "- <br>", "")
        strOut = Replace(strOut, " - <br>", "")
        strOut = Replace(strOut, "<br>", " ")
        Return strOut
    End Function

    Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Session.Remove("App_ADR_VFS01")
        Response.Redirect("Report01.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand

        If e.CommandName = "AdressenAnzeigen" Then
            UpdateSessionListOfCheckedVermittlernummern()
            ShowReportSelectedAddresses()
        ElseIf e.CommandName = "AlleAdressenAnzeigen" Then
            UpdateSessionListOfCheckedVermittlernummern(True)
            ShowReportSelectedAddresses()
        End If

    End Sub

#Region "Private methods"
    'Erzeugen des Excel-Sheets für die selektierten Adressen
    Private Sub CreateExcelReportForSelectedAddresses()
        Dim objExcelExport As New Excel.ExcelExport()
        Dim strfilename As String = ConfigurationManager.AppSettings("ExcelPath") & Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

        objHandler = Session("objHandler")
        Dim reportExcel As DataTable = objHandler.AddressTable.Copy()
        Try
            reportExcel.Columns(0).ColumnName = "VD-Bezirk"
            reportExcel.AcceptChanges()
            Excel.ExcelExport.WriteExcel(reportExcel, strfilename)
            Session("lnkExcel") = strfilename
        Catch
        End Try
    End Sub

    'Anzeige der selektierten Adressen
    Private Sub ShowReportSelectedAddresses()
        Dim listOfVermittlernummer As New System.Collections.ArrayList
        Dim outputString As String = ""
        Dim countAddressesNotFound As Int32

        If Not TryGetListOfVermittlernummernFromSession(listOfVermittlernummer) Then Exit Sub

        If listOfVermittlernummer.Count > 0 Then
            objHandler = Session("objHandler")
            objHandler.GiveAddressData(listOfVermittlernummer, Session("AppID").ToString, Session.SessionID)
            Session("ResultTable") = objHandler.AddressTable
            countAddressesNotFound = GetCountOfAddressesNotFound(objHandler.AddressTable)
            Session("ShowOtherString") = BuildOutputStringForReportAddresses(objHandler.AddressTable.Rows.Count, countAddressesNotFound)
            CreateExcelReportForSelectedAddresses()
            Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString() & "&legende=AppVFS-ADR")
            'Response.Redirect("/Portal/(" & Session.SessionID & ")/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString() & "&legende=AppVFS-ADR&schmal=Ja")
        End If
    End Sub

    'Pflegt die Liste der Vermittlernummern, deren Adressen angezeigt werden sollen.
    'Count ist 0 wenn keine Checkbox selektiert wurde. 
    Private Sub UpdateSessionListOfCheckedVermittlernummern(Optional ByVal showAllAddr As Boolean = False)
        Dim listOfVermittlernummern As New System.Collections.ArrayList
        Dim isNewList As Boolean = False

        If Not TryGetListOfVermittlernummernFromSession(listOfVermittlernummern) Then
            listOfVermittlernummern = New System.Collections.ArrayList()
            isNewList = True
        End If

        Dim item As DataGridItem
        Dim idxColVermittlernummer As Integer = GetDataGridColumnIndexOfVermittlernummer(DataGrid1)
        For Each item In DataGrid1.Items
            'Werte ermitteln

            Dim cell As TableCell
            Dim tmpVermittlernummer As String
            For Each cell In item.Cells

                Dim c As System.Web.UI.Control
                For Each c In cell.Controls

                    If TypeOf c Is CheckBox Then
                        Dim chk As CheckBox = CType(c, CheckBox)

                        If chk.ID = "chkAdresseAnzeigen" Then
                            tmpVermittlernummer = item.Cells(idxColVermittlernummer).Text

                            If chk.Checked OrElse showAllAddr Then
                                If Not listOfVermittlernummern.Contains(tmpVermittlernummer) Then
                                    listOfVermittlernummern.Add(tmpVermittlernummer)
                                End If
                            ElseIf listOfVermittlernummern.Contains(tmpVermittlernummer) Then
                                listOfVermittlernummern.Remove(tmpVermittlernummer)
                            End If
                        End If
                    End If

                Next

            Next

        Next

        If isNewList Then
            Session("App_ADR_VFS01") = listOfVermittlernummern
        End If

    End Sub

    'Liefert den Spaltenindex der Vermittlernummer
    'Achtung: Diese Funktion löst eine Exception aus, wenn die Spalte mit dem
    'Headertext 'VD-Bezirk' und der Sortexpression 'KUN_EXT_VM' nicht gefunden wird.
    Private Function GetDataGridColumnIndexOfVermittlernummer(ByVal pDataGrid As DataGrid) As Integer
        Dim tmpColumn As DataGridColumn
        Dim i As Int32 = 0

        For Each tmpColumn In pDataGrid.Columns
            If tmpColumn.HeaderText = "VD-Bezirk" And tmpColumn.SortExpression = "KUN_EXT_VM" Then
                Return i
            End If
            i = 1 + 1
        Next

        Throw New Exception("Die Spalte mit der Überschrift 'VD-Bezirk' und der Sortexpression 'KUN_EXT_VM' wurde im DataGrid1 nicht gefunden.")

    End Function

    'Setzt den Wert der Checkboxen für die Adressauswahl im DataGrid
    Private Sub SetVermittlernummerInDataGridCheckedOrUnchecked()
        Dim listOfVermittlernummern As New System.Collections.ArrayList

        If Not TryGetListOfVermittlernummernFromSession(listOfVermittlernummern) Then Exit Sub
        If listOfVermittlernummern.Count = 0 Then Exit Sub

        Dim item As DataGridItem
        For Each item In DataGrid1.Items
            'Werte ermitteln

            Dim cell As TableCell
            Dim idxColVermittlernummer As Integer = GetDataGridColumnIndexOfVermittlernummer(DataGrid1)
            For Each cell In item.Cells

                Dim c As System.Web.UI.Control
                For Each c In cell.Controls

                    If TypeOf c Is CheckBox Then
                        Dim chk As CheckBox = CType(c, CheckBox)

                        If chk.ID = "chkAdresseAnzeigen" Then
                            If listOfVermittlernummern.Contains(item.Cells(idxColVermittlernummer).Text) Then
                                chk.Checked = True
                            Else
                                chk.Checked = False
                            End If
                        End If
                    End If

                Next

            Next

        Next

    End Sub

    'Liefert 'TRUE', wenn Vermittlernummern als Sessionvariable existieren. In diesem
    'Fall wird der übergebene Parameter mit der Liste aus der Sessionvariablen gefüllt.
    'Wenn keine keine Sessionvariable existiert, wird 'FALSE' geliefert und der übergebene
    'Parameter bleibt unveränder
    Private Function TryGetListOfVermittlernummernFromSession(ByRef pListOfVermittlernummern As System.Collections.ArrayList) As Boolean
        Dim tmpListOfVermittlernummern As System.Collections.ArrayList

        tmpListOfVermittlernummern = CType(Session("App_ADR_VFS01"), System.Collections.ArrayList)
        If Not tmpListOfVermittlernummern Is Nothing Then
            pListOfVermittlernummern = tmpListOfVermittlernummern
            Return True
        End If
        Return False
    End Function

    'Liefert die Anzahl der Vermittler, für die keine Addressdaten gefunden wurden
    Private Function GetCountOfAddressesNotFound(ByVal pAddressTable As DataTable) As Int32
        If pAddressTable.Rows.Count = 0 Then
            Return 0
        End If

        Dim aRow As DataRow
        Dim aCount As Int32 = 0

        For Each aRow In pAddressTable.Rows
            If aRow("Ort") = VFS01.CONST_NO_ADDRESS_DATA Then
                aCount = aCount + 1
            End If
        Next
        Return aCount
    End Function

    'Erzeugt die Ausgabemeldung für den Addressreport
    Private Function BuildOutputStringForReportAddresses(ByVal pCountItems As Int32, ByVal pCountAddressesNotFound As Int32) As String
        Dim outputString1 As String
        Dim outputString2 As String

        If pCountItems = pCountAddressesNotFound Then
            outputString1 = "Für die ausgewählten Vermittler in 'Kennzeichenbestand' wurden keine Adressdaten gefunden."
            Return outputString1
        End If

        If pCountItems - pCountAddressesNotFound > 1 Then
            outputString1 = String.Format("Es wurden {0} Adressen für 'Kennzeichenbestand' gefunden.", pCountItems - pCountAddressesNotFound)
        Else
            outputString1 = "Es wurde 1 Adresse für 'Kennzeichenbestand' gefunden."
        End If

        outputString2 = ""
        If pCountAddressesNotFound = 1 Then
            outputString2 = " Für 1 Vermittler wurde keine Adresse gefunden."
        ElseIf pCountAddressesNotFound > 1 Then
            outputString2 = String.Format(" Für {0} Vermittler wurden keine Adressen gefunden.", pCountAddressesNotFound)
        End If

        Return outputString1 & outputString2
    End Function
#End Region

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub lnkCreateExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCreateExcel.Click
        Dim reportExcel As DataTable

        Try
            GetObjHaendler()

            reportExcel = objHandler.DetailTable.Copy

            Dim intCount As Integer
            For intCount = 0 To 6
                reportExcel.Columns(intCount).ColumnName = ClearBrForExcelOutput(DataGrid1.Columns(intCount).HeaderText)
            Next

            reportExcel.AcceptChanges()

            Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()

            excelFactory.CreateDocumentAndSendAsResponse("Report", reportExcel, Me.Page)
        Catch
        End Try
    End Sub

   
End Class

' ************************************************
' $History: Report01_2.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 28.11.08   Time: 16:46
' Updated in $/CKAG/Applications/appvfs/Forms
' ita 2317 im testfertig
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 26.11.08   Time: 17:11
' Updated in $/CKAG/Applications/appvfs/Forms
' ITA 2317 unfertig
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 14.05.08   Time: 15:21
' Updated in $/CKAG/Applications/appvfs/Forms
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 13.05.08   Time: 10:13
' Updated in $/CKAG/Applications/appvfs/Forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 29.04.08   Time: 12:27
' Updated in $/CKAG/Applications/appvfs/Forms
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:43
' Created in $/CKAG/Applications/appvfs/Forms
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 6.09.07    Time: 12:47
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 22.06.07   Time: 16:44
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' 
' *****************  Version 13  *****************
' User: Uha          Date: 21.06.07   Time: 18:45
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' Bugfixing VFS 2
' 
' *****************  Version 12  *****************
' User: Uha          Date: 20.06.07   Time: 18:58
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' Bug fixing 1
' 
' *****************  Version 11  *****************
' User: Uha          Date: 20.06.07   Time: 16:21
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 10  *****************
' User: Uha          Date: 3.05.07    Time: 19:07
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************
