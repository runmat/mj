Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Namespace Treuhand

    Partial Public Class Change01s
        Inherits System.Web.UI.Page

#Region "Declarations"
        Private m_User As Base.Kernel.Security.User
        Private m_App As Base.Kernel.Security.App
        Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation
#End Region

#Region "Events"


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            m_User = GetUser(Me)
            FormAuth(Me, m_User)
            m_App = New Base.Kernel.Security.App(m_User)
            GetAppIDFromQueryString(Me)
            GridNavigation1.setGridElment(gvAusgabe)

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            If IsPostBack = False Then
                SetddlCustomer()
            End If

        End Sub

        Protected Sub rdbEinzel_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdbEinzel.CheckedChanged, rdbUpload.CheckedChanged
            tblEinzel.Visible = rdbEinzel.Checked
            tblUpload.Visible = rdbUpload.Checked
        End Sub

        Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
            ModalPopupExtender2.Show()
        End Sub

        Protected Sub ibtInfo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtInfo.Click
            ModalPopupExtender2.Show()
        End Sub

        Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
            Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
        End Sub

        Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click
            DoSubmit()
        End Sub

        Protected Sub NewSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles NewSearch.Click
            Result.Visible = False
            Panel1.Visible = True
            lbCreate.Visible = True
            cmdSave.Visible = False
        End Sub

        Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
            If CheckGrid() = False Then
                gvAusgabe.PageIndex = PageIndex
                FillGrid(PageIndex)
            End If
           
        End Sub

        Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
            If CheckGrid() = False Then
                FillGrid(0)
            End If

        End Sub
        Private Sub gvBestand_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvAusgabe.Sorting
            If CheckGrid() = False Then
                FillGrid(gvAusgabe.PageIndex, e.SortExpression)
            End If

        End Sub

        Protected Sub cmdSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave.Click

            If CheckGrid() = True Then Exit Sub





            Dim objBlock As New Block
            Dim tblExport As New DataTable


            Dim ImportTable As DataTable = CType(Session("Block"), DataTable)

            If ImportTable.Select("BLOCK_ALT_LOE = 'X'").Length = 0 AndAlso ImportTable.Select("BLOCK_NR_NEU <> ''").Length = 0 Then
                lblError.Text = "Es wurden keine Daten geändert."
                divError.Visible = True
                Exit Sub
            End If



            tblExport = objBlock.SetBlockData(m_User, m_App, Me.Page, ImportTable)

            divError.Visible = True
            cmdSave.Visible = False

            If tblExport.Rows.Count = 0 Then
                lblNoData.Text = "Die Änderung der Blocknummer/n ist erfolgt."

            Else

                For Each dr As DataRow In tblExport.Rows


                    ImportTable.Select("EQUNR = '" & dr("EQUNR").ToString & "'")(0)("NO_FOUND") = "X"


                Next

                lblError.Text = "Folgende Datensätze konnten nicht geändert werden:"

                ImportTable.DefaultView.RowFilter = "NO_FOUND = 'X'"

                Session("Block") = ImportTable.DefaultView.ToTable

                FillGrid(0)

            End If


        End Sub
        Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click

            Dim tblTemp As DataTable = CreateExcelTable()

            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Me.Page)
        End Sub
        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub

#End Region



#Region "Methods"


        Private Sub SetddlCustomer()

            Dim CustomerObject As New SperreFreigabe(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
            CustomerObject.alleTreugeber = "X"
            CustomerObject.GetCustomer(Me.Page, Session("AppID").ToString, Session.SessionID.ToString)

            If CustomerObject.Result.Rows.Count > 0 Then

                Dim rdbText As String
                Dim rdbValue As String

                Dim rdvitem As New ListItem
                rdbText = "-Auswahl-"
                rdbValue = "0"
                rdvitem.Text = rdbText
                rdvitem.Value = rdbValue
                ddlCustomer.Items.Add(rdvitem)
                rdvitem.Selected = True
                For xAGS As Integer = 0 To CustomerObject.Result.Rows.Count - 1

                    rdvitem = New ListItem
                    If CustomerObject.Result.Rows(xAGS)("ZSELECT").ToString = "TG" Then
                        rdbText = CustomerObject.Result.Rows(xAGS)("NAME1_AG").ToString
                    Else
                        rdbText = CustomerObject.Result.Rows(xAGS)("NAME1_TG").ToString
                    End If

                    rdbValue = CustomerObject.Result.Rows(xAGS)("TREU").ToString

                    rdvitem.Text = rdbText
                    rdvitem.Value = rdbValue
                    ddlCustomer.Items.Add(rdvitem)
                Next
            End If

            Session("CustomerObject") = CustomerObject
        End Sub

        Private Sub DoSubmit()
            Dim objBlock As New Block
            Dim tblExport As New DataTable


            Dim ImportTable As New DataTable
            ImportTable.Columns.Add("Fahrgestellnummer", GetType(String))
            ImportTable.Columns.Add("Kennzeichen", GetType(String))
            ImportTable.Columns.Add("Leasingvertragsnummer", GetType(String))
            ImportTable.Columns.Add("Blocknummer", GetType(String))





            If rdbEinzel.Checked Then

                If (txtBlocknummer.Text & _
                    txtFahrgestellnummer.Text & _
                    txtKennzeichen.Text & _
                    txtLeasingvertragsnummer.Text).Length = 0 AndAlso ddlCustomer.SelectedValue = "0" Then

                    lblError.Text = "Bitte geben Sie ein Suchkriterium an."
                    divError.Visible = True
                    Exit Sub


                End If

                If ddlCustomer.SelectedValue = "0" AndAlso txtBlocknummer.Text.Trim.Length = 0 Then

                    If (txtFahrgestellnummer.Text & _
                        txtKennzeichen.Text & _
                        txtLeasingvertragsnummer.Text).Length > 0 Then
                        Dim newRow As DataRow = ImportTable.NewRow

                        newRow("Fahrgestellnummer") = txtFahrgestellnummer.Text
                        newRow("Kennzeichen") = txtKennzeichen.Text
                        newRow("Leasingvertragsnummer") = txtLeasingvertragsnummer.Text

                        ImportTable.Rows.Add(newRow)

                        ImportTable.AcceptChanges()

                    Else
                        lblError.Text = "Bitte geben Sie entweder eine Blocknummer oder Bank Treuhand an."
                        divError.Visible = True
                        Exit Sub
                    End If

                End If





                    tblExport = objBlock.GetBlockData(m_User, m_App, Me.Page, txtBlocknummer.Text, ddlCustomer.SelectedValue, ImportTable)

                Else
                    'Prüfe Fehlerbedingung
                    If (Not upFile.PostedFile Is Nothing) AndAlso (Not (upFile.PostedFile.FileName = String.Empty)) Then
                        If Right(upFile.PostedFile.FileName.ToUpper, 4) <> ".XLS" Then
                            divError.Visible = True
                            lblError.Text = "Es können nur Dateien im .XLS - Format verarbeitet werden."
                            Exit Sub
                        End If
                    Else
                        divError.Visible = True
                        lblError.Text = "Keine Datei ausgewählt!"
                        Exit Sub
                    End If

                    'Lade Datei
                    Dim ExcelTable As DataTable = upload(upFile.PostedFile)

                    If ExcelTable.Rows.Count > 0 Then

                        Dim newRow As DataRow

                        For i = 0 To ExcelTable.Rows.Count - 1

                            newRow = ImportTable.NewRow

                            newRow("Leasingvertragsnummer") = ExcelTable.Rows(i)(0).ToString
                            newRow("Fahrgestellnummer") = ExcelTable.Rows(i)(1).ToString
                            newRow("Blocknummer") = ExcelTable.Rows(i)(2).ToString

                            ImportTable.Rows.Add(newRow)

                            ImportTable.AcceptChanges()

                        Next

                        tblExport = objBlock.GetBlockData(m_User, m_App, Me.Page, "", "", ImportTable)

                    Else
                        lblError.Text = "Die Datei enthält keine Blocknummern."
                        divError.Visible = True
                        Exit Sub

                    End If


                End If





            If Session("Block") Is Nothing Then
                Session.Add("Block", tblExport)
            Else
                Session("Block") = tblExport
            End If

            If tblExport.Rows.Count > 0 Then
                Panel1.Visible = False

                If tblExport.Select("NO_FOUND = 'X'").Length > 0 Then
                    cmdSave.Visible = False
                    lblError.Text = "Fehler beim Laden. Bitte überprüfen Sie den Status."
                    divError.Visible = True
                Else
                    cmdSave.Visible = True
                End If

                FillGrid(0)
            Else
                lblError.Text = "Keine Daten zur Anzeige gefunden."
            End If



        End Sub

        Private Function upload(ByVal uFile As System.Web.HttpPostedFile) As DataTable

            Dim filepath As String = ConfigurationManager.AppSettings("ExcelPath")
            Dim filename As String
            Dim info As System.IO.FileInfo

            Dim TempTable As New DataTable

            'Dateiname: User_yyyyMMddhhmmss.xls
            filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xls"

            If Not (uFile Is Nothing) Then
                uFile.SaveAs(ConfigurationManager.AppSettings("ExcelPath") & filename)
                info = New System.IO.FileInfo(filepath & filename)
                If Not (info.Exists) Then
                    lblError.Text = "Fehler beim Speichern."
                    Return TempTable
                End If



                Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()



                TempTable = excelFactory.ReadDataFromExcelFile(info.FullName, 1, 0, 2, 2)


            End If

            Return TempTable

        End Function


        Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
            Dim tmpDataView As New DataView()

            tmpDataView = CType(Session("Block"), DataTable).DefaultView

            tmpDataView.RowFilter = ""

            If tmpDataView.Count = 0 Then
                Result.Visible = False
            Else
                Result.Visible = True
                lbCreate.Visible = False
                'tab1.Visible = False
                Queryfooter.Visible = False
                Dim strTempSort As String = ""
                Dim strDirection As String = ""
                Dim intTempPageIndex As Int32 = intPageIndex

                If strSort.Trim(" "c).Length > 0 Then
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
                gvAusgabe.PageIndex = intTempPageIndex
                gvAusgabe.DataSource = tmpDataView
                gvAusgabe.DataBind()

            End If

        End Sub

        Private Function CheckGrid() As Boolean

            Dim txt As TextBox
            Dim chk As CheckBox
            Dim lbl As Label
            Dim lblStatus As Label
            Dim lblFound As Label
            Dim Err As Boolean = False

            Dim TempTable As DataTable = CType(Session("Block"), DataTable)


            For Each gvRow As GridViewRow In gvAusgabe.Rows

                lbl = CType(gvRow.FindControl("lblEqui"), Label)
                txt = CType(gvRow.FindControl("txtBlocknummerNeu"), TextBox)
                chk = CType(gvRow.FindControl("chkLoeschen"), CheckBox)
                lblStatus = CType(gvRow.FindControl("lblStatus"), Label)
                lblFound = CType(gvRow.FindControl("lblFound"), Label)

                If lblFound.Text.ToUpper = "X" Then
                    lblStatus.Text = "Die Fahrgestellnummer oder die Leasingvertragsnummer wurde nicht gefunden."
                    Err = True
                End If


                If txt.Text.Length > 0 AndAlso chk.Checked Then
                    lblStatus.Text = "Kombination nicht möglich: Neue Blocknummer und alte Blocknummer löschen."
                    Err = True
                Else

                    If chk.Checked = True Then

                        TempTable.Select("EQUNR = '" & lbl.Text & "'")(0)("BLOCK_ALT_LOE") = "X"

                    End If

                    If txt.Text.Length > 0 Then
                        TempTable.Select("EQUNR = '" & lbl.Text & "'")(0)("BLOCK_NR_NEU") = txt.Text
                    End If

                End If

            Next

            If Err = True Then
                lblError.Text = "Fehler: Bitte überprüfen Sie Ihre Eingaben."
            End If

            Return Err


        End Function

        Private Function CreateExcelTable() As DataTable

            Dim tblTemp As New DataTable
            Dim tblResult As DataTable

            tblResult = CType(Session("Block"), DataTable)

            tblTemp.Columns.Add("Bank Treuhand", GetType(String))
            tblTemp.Columns.Add("Blocknummer ALT", GetType(String))
            tblTemp.Columns.Add("Blocknummer NEU", GetType(String))
            tblTemp.Columns.Add("Leasingvertragsnummer", GetType(String))
            tblTemp.Columns.Add("Fahrgestellnummer", GetType(String))
            tblTemp.Columns.Add("Kennzeichen", GetType(String))
            tblTemp.Columns.Add("ZBII Nummer", GetType(String))
            tblTemp.Columns.Add("Hersteller", GetType(String))
            tblTemp.Columns.Add("Typ", GetType(String))
            tblTemp.Columns.Add("Ausführung", GetType(String))

            Dim tempNewRow As DataRow
            For i As Integer = 0 To tblResult.Rows.Count - 1
                tempNewRow = tblTemp.NewRow
                tempNewRow("Bank Treuhand") = tblResult.Rows(i)("NAME1_BANK_TH").ToString
                tempNewRow("Blocknummer ALT") = tblResult.Rows(i)("BLOCK_NR_ALT").ToString
                tempNewRow("Blocknummer NEU") = tblResult.Rows(i)("BLOCK_NR_NEU").ToString
                tempNewRow("Leasingvertragsnummer") = tblResult.Rows(i)("LIZNR").ToString
                tempNewRow("Fahrgestellnummer") = tblResult.Rows(i)("CHASSIS_NUM").ToString
                tempNewRow("Kennzeichen") = tblResult.Rows(i)("LICENSE_NUM").ToString
                tempNewRow("ZBII Nummer") = tblResult.Rows(i)("TIDNR").ToString
                tempNewRow("Hersteller") = tblResult.Rows(i)("ZZHERSTELLER_SCH").ToString
                tempNewRow("Typ") = tblResult.Rows(i)("ZZTYP_SCHL").ToString
                tempNewRow("Ausführung") = tblResult.Rows(i)("ZZVVS_SCHLUESSEL").ToString

                tblTemp.Rows.Add(tempNewRow)
            Next
            Return tblTemp
        End Function

#End Region

      
        

    End Class
End Namespace

' ************************************************
' $History: Change01s.aspx.vb $
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 30.07.10   Time: 8:54
' Updated in $/CKAG2/Services/Components/ComCommon/Treuhand
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 26.07.10   Time: 16:47
' Updated in $/CKAG2/Services/Components/ComCommon/Treuhand
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 16.07.10   Time: 15:14
' Updated in $/CKAG2/Services/Components/ComCommon/Treuhand
' ITA: 3788
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 30.06.10   Time: 13:09
' Updated in $/CKAG2/Services/Components/ComCommon/Treuhand
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 28.06.10   Time: 17:31
' Updated in $/CKAG2/Services/Components/ComCommon/Treuhand
' 