Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Report01Neu
    Inherits System.Web.UI.Page

    Private m_User As Security.User
    Private m_App As Security.App
    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation
    Private objhandler As VFS01

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GridNavigation1.setGridElment(GridView1)
        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            m_App = New Security.App(m_User)
            lblError.Text = ""

            If IsPostBack Then
                If Not Session("objHandler") Is Nothing Then
                    objhandler = CType(Session("objHandler"), VFS01)
                    If objhandler.Versicherungsjahr <> txtVJahr.Text Then
                        objhandler.Versicherungsjahr = txtVJahr.Text
                    Else
                        txtVJahr.Text = objhandler.Versicherungsjahr
                    End If

                    If txtOrgNr.Text <> objhandler.OrgNr Then
                        objhandler.OrgNr = txtOrgNr.Text
                    End If
                End If

            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        UpdateSessionListOfCheckedVermittlernummern()

        GetObjHaendler()
        Dim strliteral As String
        strliteral = "						<script language=""Javascript"">" & vbCrLf
        strliteral &= "						  <!-- //" & vbCrLf
        strliteral &= "							document.getElementById('tab1').style.display = 'block';" & vbCrLf
        strliteral &= "						  //-->" & vbCrLf
        strliteral &= "						</script>" & vbCrLf

        Dim tmpDataView As New DataView()
        If objhandler.DetailTable Is Nothing Then
            Result.Visible = False
            lblError.Text = "Keine Daten zur Anzeige gefunden."
            lblError.Visible = True
            Literal1.Text = strliteral
            tabStats.Visible = False

            Exit Sub
        Else
            tmpDataView = objhandler.DetailTable.DefaultView
        End If

        tmpDataView.RowFilter = ""


        If tmpDataView.Count = 0 Then
            Result.Visible = False
            lblError.Text = "Keine Daten zur Anzeige gefunden."
            lblError.Visible = True
            Literal1.Text = strliteral
            tabStats.Visible = False

        Else
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

            GridView1.PageIndex = intTempPageIndex

            GridView1.DataSource = tmpDataView
            GridView1.DataBind()

            With objhandler
                Me.lblKennzeichenGesamtbestand.Text = .KennzeichenGesamtbestand
                Me.lblRuecklaeufer.Text = .RuecklaeuferAnzahl
                Me.lblUnverkaufteKennzeichen.Text = .UnverkaufteAnzahl
                Me.lblVerkaufteKennzeichen.Text = .VerkaufteAnzahl
                Me.lblVermittlerAnzahl.Text = .VermittlerAnzahl
                Me.lblStand.Text = .Erdat
                Me.lblVerlustKennzeichen.Text = .VerloreneAnzahl
                Me.lblBestandDAD.Text = .LagerAnzahl
            End With

            objhandler = Nothing

            If GridView1.Rows.Count > 0 Then
                'GridView1.PagerStyle.CssClass = "PagerStyle"
                GridView1.DataBind()

                Result.Visible = True
                tabStats.Visible = True
            Else
                Result.Visible = False
                tabStats.Visible = True
            End If
            Dim reportExcel As DataTable

            GetObjHaendler()

            reportExcel = objhandler.DetailTable.Copy

            Dim intCount As Integer
            For intCount = 0 To 6
                reportExcel.Columns(intCount).ColumnName = ClearBrForExcelOutput(GridView1.Columns(intCount).HeaderText)
            Next

            reportExcel.AcceptChanges()

            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim sPath As String = ConfigurationManager.AppSettings("ExcelPath")
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndWriteToFilesystemWithPath(sPath + strFileName, reportExcel)
            tab1.Visible = False
            btnConfirm.Visible = False
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
            Response.Redirect("/Services/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString() & "&legende=AppVFS-ADR")
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

        Dim item As GridViewRow
        Dim idxColVermittlernummer As Integer = GetDataGridColumnIndexOfVermittlernummer(GridView1)
        For Each item In GridView1.Rows
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
    Private Function GetDataGridColumnIndexOfVermittlernummer(ByVal pDataGrid As GridView) As Integer
        Dim tmpColumn As BoundField
        Dim i As Int32 = 0

        For Each tmpColumn In pDataGrid.Columns
            If tmpColumn.HeaderText = "Agenturnr." And tmpColumn.SortExpression = "KUN_EXT_VM" Then
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

        Dim item As GridViewRow
        For Each item In GridView1.Rows
            'Werte ermitteln

            Dim cell As TableCell
            Dim idxColVermittlernummer As Integer = GetDataGridColumnIndexOfVermittlernummer(GridView1)
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
    Private Sub GetObjHaendler()
        If Session("objHandler") Is Nothing = False Then
            objHandler = Session("objHandler")
        End If

        'Daten holen
        Dim tmpTable As DataTable

        tmpTable = objHandler.DetailTable
        If tmpTable Is Nothing OrElse tmpTable.Rows.Count = 0 Then
            objhandler.GiveDataDyn(Session("AppID").ToString, Session.SessionID, Me)
        End If
    End Sub
#End Region

    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex, e.SortExpression)
    End Sub


    Protected Sub btnConfirm0Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConfirm.Click
        objhandler = New VFS01(m_User, m_App, "")

        objhandler.Versicherungsjahr = txtVJahr.Text

        Dim Orgnummer As String = ""
        Dim sNummerTrenn As String = ""
        For i As Integer = 1 To txtOrgNr.Text.Length
            If Not (Mid(txtOrgNr.Text, i, 1)) = "-" AndAlso Not (Mid(txtOrgNr.Text, i, 1)) = "_" Then
                sNummerTrenn = txtOrgNr.Text
                Orgnummer &= Mid(txtOrgNr.Text, i, 1)
            End If
        Next

        objhandler.OrgNr = Orgnummer

        objhandler.GiveDataDyn(Session("AppID").ToString, Session.SessionID, Me)
        Session("objhandler") = objhandler
        FillGrid(0)
        Dim tmpListeVermittlernummer As New System.Collections.ArrayList
        If TryGetListOfVermittlernummernFromSession(tmpListeVermittlernummer) Then
            SetVermittlernummerInDataGridCheckedOrUnchecked()
        End If

    End Sub

    Private Sub GridNavigation1_ddlPageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub

    Private Sub GridView1_PageIndexChanged(ByVal pageindex As Int32) Handles GridNavigation1.PagerChanged
        GridView1.EditIndex = -1
        FillGrid(pageindex)
    End Sub

    Protected Sub NewSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles NewSearch.Click
        tab1.Visible = Not tab1.Visible
        btnConfirm.Visible = Not btnConfirm.Visible
    End Sub

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
        GetObjHaendler()
        Dim reportExcel As DataTable
        Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
        Dim sPath As String = ConfigurationManager.AppSettings("ExcelPath")
        reportExcel = objhandler.DetailTable.Copy

        reportExcel.AcceptChanges()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
        ' excelFactory.CreateDocumentAndWriteToFilesystemWithPath(sPath + strFileName, reportExcel)
        excelFactory.CreateDocumentAndSendAsResponse(strFileName, reportExcel, Me)
    End Sub

    Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub
End Class