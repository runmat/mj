Option Strict Off

Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG

Public Class Change07_2
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objSuche As Search
    Private objHaendler As F1_Haendler
    Private bAnyAbrufgrundzusatz As Boolean = False

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents Kopfdaten1 As Kopfdaten

#Region "Methods"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Kopfdaten1.Message = ""
        lnkKreditlimit.NavigateUrl = "Change07_1.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugAuswahl.NavigateUrl = "Change07_2.aspx?AppID=" & Session("AppID").ToString

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change07_1.aspx?AppID=" & Session("AppID").ToString)
            End If

            If Session("objSuche") Is Nothing Then
                Response.Redirect("Change07_1.aspx?AppID=" & Session("AppID").ToString)
            Else
                objSuche = CType(Session("objSuche"), Search)
            End If

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

            'divMessage.Visible = False

            If Not IsPostBack Then
                trcmdSave.Visible = True
                trcmdSave2.Visible = False
                ddlAbrufgrund.Visible = False
                lnkFahrzeugAuswahl.Visible = False

                If ddlPageSize.Items.Count < 1 Then
                    ddlPageSize.Items.Add("10")
                    ddlPageSize.Items.Add("20")

                    ddlPageSize.SelectedIndex = 1
                End If

                'lblMessage.Visible = False

                FillGrid(0)
                If Not objHaendler.MassenAnforderung Is Nothing Then
                    lblInfo.Text = "Upload: " & objHaendler.Fahrzeuge.DefaultView.Count & " / " & objHaendler.MassenAnforderung.Rows.Count & " Fahrzeugen anforderungsbereit."
                    lblInfo.Visible = True
                End If

            End If
            If IsPostBack AndAlso trcmdSave2.Visible Then
                CheckGrid2()
                FillGrid(0)

            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Function CheckGrid2() As Boolean
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim ddl As DropDownList
        Dim txtBox As TextBox
        Dim control As Control
        Dim intZaehl As Int32 = 0
        Dim blnReturn As Boolean = False

        Dim tmpRows As DataRow()

        'Fahrzeugtabelle instanziert?
        CheckFahrzeuge()

        For Each item In DataGrid1.Items
            Dim strZZFAHRG As String = "ZZFAHRG = '" & CType(item.FindControl("lnkHistorie"), HyperLink).Text & "'"

            For Each cell In item.Cells
                For Each control In cell.Controls
                    If TypeOf control Is DropDownList Then
                        ddl = CType(control, DropDownList)
                        If ddl.ID = "cmbAbrufgrund" Then
                            If Not ddl.SelectedItem Is Nothing Then
                                If ddl.SelectedItem.Value = "000" Then
                                    intZaehl += 1
                                Else
                                    objHaendler.Fahrzeuge.AcceptChanges()
                                    tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                                    tmpRows(0).BeginEdit()
                                    tmpRows(0).Item("AUGRU") = ddl.SelectedItem.Value
                                    tmpRows(0).EndEdit()
                                    objHaendler.Fahrzeuge.AcceptChanges()
                                End If
                            End If
                        Else
                            intZaehl += 1
                        End If
                    End If
                    If TypeOf control Is TextBox Then
                        txtBox = CType(control, TextBox)
                        If txtBox.ID = "txtZusatztext" Then
                            If Not txtBox.Text.Trim Is String.Empty Then
                                tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                                tmpRows(0).BeginEdit()
                                tmpRows(0).Item("text50") = txtBox.Text
                                tmpRows(0).EndEdit()
                                objHaendler.Fahrzeuge.AcceptChanges()
                            Else
                                If txtBox.Visible = True Then
                                    'wenn nicht ausgefüllt fehler 
                                    intZaehl += 1
                                End If
                            End If
                        End If
                    End If
                Next
            Next
        Next
        If intZaehl = 0 Then
            blnReturn = True
        End If
        Session("AppHaendler") = objHaendler
        Return blnReturn
    End Function



    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        'If CheckGrid2() Then
        '    objHaendler.ErsetzeAbrufgruende()
        '    Session("AppHaendler") = objHaendler
        '    Response.Redirect("Change07_3.aspx?AppID=" & Session("AppID").ToString)
        'Else
        '    lblError.Text = "Bitte geben Sie die Informationen zu den Abrufgründen ein."
        '    FillGrid(DataGrid1.CurrentPageIndex)
        'End If
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim i As Integer
        i = CheckGrid()
        If i <> -99 Then
            Dim tmpDataView As New DataView()
            tmpDataView = objHaendler.Fahrzeuge.DefaultView

            tmpDataView.RowFilter = "MANDT <> '0'"
            Dim intFahrzeugBriefe As Int32 = tmpDataView.Count
            tmpDataView.RowFilter = ""

            If intFahrzeugBriefe = 0 Then
                lblError.Text = "Bitte wählen Sie erst Fahrzeuge zur Anforderung aus."
                FillGrid(0)
            Else
                trcmdSave.Visible = False
                trcmdSave2.Visible = True


                If ddlAbrufgrund.Items.Count = 0 Then
                    FillAbrufgruende()
                End If

                ddlAbrufgrund.Visible = True

                FillGrid(0)
                lnkFahrzeugAuswahl.Visible = True
                lblInfo.Visible = False

            End If
        End If
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        CheckGrid()
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        CheckGrid()
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Function CheckGrid() As Int32
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim chbox As CheckBox
        Dim txtBox As TextBox
        Dim control As Control
        Dim blnScriptFound As Boolean = False
        Dim intZaehl As Int32
        Dim intReturn As Int32 = 0

        Dim tmpRows As DataRow()
        Dim tmpRow As DataRow


        'Fahrzeugtabelle instanziert?
        CheckFahrzeuge()


        'Kopftext setzen
        If txtKopf.Text <> String.Empty Then
            For Each tmpRow In objHaendler.Fahrzeuge.Rows
                'tmpRow("TEXT50") = txtKopf.Text
                tmpRow("KOPFTEXT") = txtKopf.Text
            Next
        End If
        Try
            For Each item In DataGrid1.Items
                intZaehl = 1
                Dim strZZFAHRG As String = ""
                For Each cell In item.Cells
                    If intZaehl = 1 Then
                        strZZFAHRG = "ZZFAHRG = '" & CType(cell.FindControl("lnkHistorie"), HyperLink).Text & "'"
                    End If
                    For Each control In cell.Controls
                        If TypeOf control Is CheckBox Then
                            chbox = CType(control, CheckBox)
                            objHaendler.Fahrzeuge.AcceptChanges()
                            'Dim tmpRows As DataRow()
                            'Dim tmpRow As DataRow
                            tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                            tmpRows(0).BeginEdit()
                            Select Case chbox.ID
                                Case "chkNichtAnfordern"
                                    If chbox.Checked Then
                                        tmpRows(0).Item("MANDT") = "0"
                                    End If
                                Case "chk0001"
                                    If chbox.Checked Then
                                        tmpRows(0).Item("MANDT") = "1"
                                        intReturn += 1
                                    End If
                                Case "chk0002"
                                    If chbox.Checked Then
                                        tmpRows(0).Item("MANDT") = "2"
                                        intReturn += 1
                                    End If
                                Case "chk0003"
                                    If chbox.Checked Then
                                        tmpRows(0).Item("MANDT") = "3"
                                        intReturn += 1
                                    End If
                                Case "chk0004"
                                    If chbox.Checked Then
                                        tmpRows(0).Item("MANDT") = "4"
                                        intReturn += 1
                                    End If
                                Case "chk0006"
                                    If chbox.Checked Then
                                        tmpRows(0).Item("MANDT") = "6"
                                        intReturn += 1
                                    End If
                            End Select
                            tmpRows(0).EndEdit()
                            objHaendler.Fahrzeuge.AcceptChanges()
                        End If
                        'Positionstexte lesen
                        If TypeOf control Is TextBox Then
                            txtBox = CType(control, TextBox)
                            tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                            tmpRows(0).BeginEdit()
                            'tmpRows(0).Item("TEXT200") = txtBox.Text
                            If txtBox.ID = "txtPosition" Then
                                tmpRows(0).Item("POSITIONSTEXT") = txtBox.Text
                            End If




                            'If txtBox.ID = "txtAnfragenr" Then
                            '    ' Zusammensetzen der Anfragenummer
                            '    If tmpRows(0).Item("MANDT").ToString = "3" Then
                            '        If txtBox.Text.Length = 13 And IsNumeric(txtBox.Text) Then
                            '            Dim sTemp As String
                            '            Dim sKunnr As String
                            '            Dim sTemp1 As String
                            '            Dim sTemp2 As String
                            '            Dim sTemp3 As String
                            '            Dim sTemp4 As String
                            '            Dim sTemp5 As String
                            '            sTemp = Left(txtBox.Text, 10)
                            '            sKunnr = Left(sTemp, 5)
                            '            sTemp1 = Right(sTemp, 5)
                            '            sTemp2 = Left(sTemp1, 2)
                            '            sTemp3 = Right(sTemp1, 3)
                            '            sTemp4 = Right(txtBox.Text, 3)
                            '            'sTemp = kopfdaten1.HaendlerNummer
                            '            sTemp5 = sKunnr & "." & sTemp2 & "." & sTemp3 & "." & sTemp4
                            '            tmpRows(0).Item("TEXT300") = sTemp5
                            '        ElseIf txtBox.Text.Length >= 0 AndAlso tmpRows(0).Item("MANDT") = "3" Then
                            '            Throw New Exception("Anforderungsart Retail: Für das Feld ""Anfragenummer"" sind 13 numerische Zeichen erforderlich!")
                            '        End If
                            '    End If
                            'End If
                            tmpRows(0).EndEdit()
                            objHaendler.Fahrzeuge.AcceptChanges()
                        End If
                    Next
                    intZaehl += 1
                Next
            Next
            Session("objHaendler") = objHaendler
            Return intReturn
        Catch ex As Exception
            lblError.Text = ex.Message
            CheckGrid = -99
        End Try
    End Function

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        Dim strKKB As String = "0"
        Dim mitGrund As Boolean = False
        'Fahrzeugtabelle instanziert?
        CheckFahrzeuge()


        Dim tmpDataView As New DataView()

        tmpDataView = objHaendler.Fahrzeuge.DefaultView

        If trcmdSave2.Visible Then
            tmpDataView.RowFilter = "MANDT <> '0'"
        Else
            tmpDataView.RowFilter = ""
        End If

        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
            ddlPageSize.Visible = False
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            lblNoData.Visible = True
            ShowScript.Visible = False
        Else
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

            lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If


            If objHaendler.Kontingente Is Nothing OrElse objHaendler.Kontingente.Rows.Count = 2 Then  'Nur Standard-Kontingent->Spalte ausblenden
                DataGrid1.Columns(DataGrid1.Columns.Count - 8).Visible = False
            Else
                If objHaendler.Kontingente.Rows.Count > 2 Then  'Delayed Payment-Kontingent vorhanden?
                    If objHaendler.Kontingente.Rows(3)("Kreditkontrollbereich").ToString = "0004" Then  'Kann nur in Zeile 3 sein...
                        If CInt(objHaendler.Kontingente.Rows(3)("Richtwert_Alt")) = 0 Then 'Richtwert=0, ausblenden!
                            DataGrid1.Columns(DataGrid1.Columns.Count - 8).Visible = False
                        End If
                    Else
                        DataGrid1.Columns(DataGrid1.Columns.Count - 8).Visible = False  'Nein, dann sowieso ausblenden
                    End If
                    If objHaendler.Kontingente.Rows(2)("Kreditkontrollbereich").ToString = "0003" Then  'Kontingent = Retail
                        If CInt(objHaendler.Kontingente.Rows(2)("Richtwert_Alt")) = 0 Then 'Richtwert=0, ausblenden!
                            DataGrid1.Columns(DataGrid1.Columns.Count - 5).Visible = False
                            DataGrid1.Columns(DataGrid1.Columns.Count - 7).Visible = False
                        End If
                    Else
                        DataGrid1.Columns(DataGrid1.Columns.Count - 5).Visible = False
                        DataGrid1.Columns(DataGrid1.Columns.Count - 7).Visible = False
                    End If
                    If objHaendler.Kontingente.Rows(4)("Kreditkontrollbereich").ToString = "0006" Then  'Kontingent = Retail
                        If CInt(objHaendler.Kontingente.Rows(4)("Richtwert_Alt")) = 0 Then 'Richtwert=0, ausblenden!
                            DataGrid1.Columns(DataGrid1.Columns.Count - 6).Visible = False

                        End If
                    Else
                        DataGrid1.Columns(DataGrid1.Columns.Count - 6).Visible = False
                    End If
                End If
            End If

            Dim item As DataGridItem
            Dim cell As TableCell
            Dim chkBox As CheckBox
            Dim control As Control
            Dim blnScriptFound As Boolean = False
            Dim intZaehl As Int32

            For Each item In DataGrid1.Items
                intZaehl = 1

                Dim blnBezahlt As Boolean

                For Each cell In item.Cells
                    For Each control In cell.Controls
                        If TypeOf control Is CheckBox Then
                            chkBox = CType(control, CheckBox)
                            If chkBox.ID = "chkBezahlt" Then
                                blnBezahlt = chkBox.Checked
                            End If
                        End If
                    Next
                Next

                For Each cell In item.Cells
                    If intZaehl = 2 Then
                        strKKB = cell.Text
                    End If
                    For Each control In cell.Controls
                        If TypeOf control Is CheckBox Then
                            chkBox = CType(control, CheckBox)
                            Select Case chkBox.ID
                                Case "chkNichtAnfordern"
                                    If strKKB = "0" Then
                                        chkBox.Checked = True
                                    End If
                                Case "chk0001"
                                    If blnBezahlt Then
                                        chkBox.Enabled = False
                                    Else
                                        If strKKB = "1" Then
                                            chkBox.Checked = True
                                            mitGrund = True
                                        End If
                                    End If
                                Case "chk0002"
                                    If strKKB = "2" Then
                                        chkBox.Checked = True
                                        mitGrund = True
                                    End If
                                Case "chk0003"
                                    If blnBezahlt Then
                                        chkBox.Enabled = False
                                    Else
                                        If strKKB = "3" Then
                                            chkBox.Checked = True
                                        End If
                                    End If
                                Case "chk0004"
                                    If blnBezahlt Then
                                        chkBox.Enabled = False
                                    Else
                                        If strKKB = "4" Then
                                            chkBox.Checked = True
                                        End If
                                    End If
                                Case "chk0006"
                                    If blnBezahlt Then
                                        chkBox.Enabled = False
                                    Else
                                        If strKKB = "6" Then
                                            chkBox.Checked = True
                                        End If
                                    End If
                            End Select
                        End If
                    Next
                    intZaehl += 1
                Next
            Next
        End If

        For Each item As DataGridItem In DataGrid1.Items
            If Not item.FindControl("lnkHistorie") Is Nothing Then
                If Not m_User.Applications.Select("AppName = 'Report46'").Count = 0 Then
                    CType(item.FindControl("lnkHistorie"), HyperLink).NavigateUrl = "../../../Components/ComCommon/Finance/Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN=" & CType(item.FindControl("lnkHistorie"), HyperLink).Text
                End If
            End If
        Next

        If trcmdSave2.Visible Then
            AbrufDatagridView()
            If mitGrund = False Then 'was ist das? JJU20090406
                'DataGrid1.Columns(19).Visible = False
                'DataGrid1.Columns(20).Visible = False
            End If
        Else
            StandardDatagridView()
        End If
    End Sub

    Private Sub StandardDatagridView()
        DataGrid1.Columns(0).Visible = True
        DataGrid1.Columns(1).Visible = False
        DataGrid1.Columns(2).Visible = True
        DataGrid1.Columns(3).Visible = True
        DataGrid1.Columns(4).Visible = True
        DataGrid1.Columns(5).Visible = True
        'DataGrid1.Columns(6).Visible = True
        DataGrid1.Columns(7).Visible = True
        DataGrid1.Columns(8).Visible = True
        'DataGrid1.Columns(9).Visible = True
        'DataGrid1.Columns(10).Visible = True
        'DataGrid1.Columns(11).Visible = True
        'DataGrid1.Columns(12).Visible = True
        'DataGrid1.Columns(13).Visible = True
        'DataGrid1.Columns(14).Visible = True
        'DataGrid1.Columns(15).Visible = True
        'DataGrid1.Columns(16).Visible = True
        DataGrid1.Columns(18).Visible = False
        DataGrid1.Columns(19).Visible = False
        DataGrid1.Columns(20).Visible = False
    End Sub

    Private Sub AbrufDatagridView()
        DataGrid1.Columns(0).Visible = True
        DataGrid1.Columns(1).Visible = False
        DataGrid1.Columns(2).Visible = True
        DataGrid1.Columns(3).Visible = True
        DataGrid1.Columns(4).Visible = True
        DataGrid1.Columns(5).Visible = True
        DataGrid1.Columns(6).Visible = True
        DataGrid1.Columns(7).Visible = True
        DataGrid1.Columns(8).Visible = True
        DataGrid1.Columns(9).Visible = True
        DataGrid1.Columns(10).Visible = False
        DataGrid1.Columns(11).Visible = False
        DataGrid1.Columns(12).Visible = False
        DataGrid1.Columns(13).Visible = False
        DataGrid1.Columns(14).Visible = False
        DataGrid1.Columns(15).Visible = False
        DataGrid1.Columns(16).Visible = False
        DataGrid1.Columns(17).Visible = False
        DataGrid1.Columns(18).Visible = False
        DataGrid1.Columns(19).Visible = True
        'was ist das? JJU20090406, AbrufgrundInfos werden in der DB geregelt!
        ' DataGrid1.Columns(20).Visible = False
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub LinkButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        txtKopf.Visible = Not txtKopf.Visible
        If txtKopf.Visible = False Then
            txtKopf.Text = String.Empty
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub


    Private Sub CheckFahrzeuge()
        'Wenn die Tabelle Fahrzeuge Nothing ist, hat der User mehrmals die Navigation des Browsers benutzt
        'Dann in die Fahrzeugsuche zurückleiten
        If IsNothing(objHaendler.Fahrzeuge) = True Then
            Response.Redirect("Change07_1.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub

    Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim tmpddl As DropDownList
            tmpddl = CType(e.Item.FindControl("cmbAbrufgrund"), DropDownList)
            If Not tmpddl Is Nothing AndAlso tmpddl.Items.Count > 0 Then
                Dim tmprows() As DataRow 'für fahrzeugtabelle
                If objHaendler Is Nothing Then
                    objHaendler = CType(Session("AppHaendler"), F1_Haendler)
                End If
                tmprows = objHaendler.Fahrzeuge.Select("ZZFAHRG = '" & CType(e.Item.FindControl("lnkHistorie"), HyperLink).Text & "'")
                If Not tmprows.Length = 0 Then
                    Dim tmpListItem As ListItem
                    If Not tmprows(0).Item("AUGRU") Is DBNull.Value AndAlso Not CStr(tmprows(0).Item("AUGRU")) Is String.Empty Then
                        tmpListItem = tmpddl.Items.FindByValue(CStr(tmprows(0).Item("AUGRU")))
                        If Not tmpListItem Is Nothing Then
                            tmpddl.SelectedIndex = tmpddl.Items.IndexOf(tmpListItem)
                            'es muss festgestellt werden ob aktelle DDL auswahl einen Textbox anzeigen soll, db=MitZusatzText Boolean
                            'und das label lblZusatzinfo soll gefüllt werden 
                            Dim vwAbrufgruende As DataView = objHaendler.Abrufgruende.DefaultView
                            Select Case tmprows(0).Item("MANDT").ToString
                                Case "1"
                                    vwAbrufgruende.RowFilter = "AbrufTyp='temp'"
                                Case "2"
                                    vwAbrufgruende.RowFilter = "AbrufTyp='endg'"
                            End Select
                            Dim tmprows2() As DataRow 'für abrufgründe
                            tmprows2 = objHaendler.Abrufgruende.Select("SapWert='" & tmpddl.SelectedItem.Value & "'")
                            Dim tmpLabel As Label
                            tmpLabel = CType(e.Item.FindControl("lblZusatzinfo"), Label)
                            If Not tmprows2(0).Item("Zusatzbemerkung") Is DBNull.Value AndAlso Not tmprows2(0).Item("Zusatzbemerkung").ToString = "" Then
                                tmpLabel.Text = tmprows2(0).Item("Zusatzbemerkung")
                                DataGrid1.Columns(DataGrid1.Columns.Count - 1).Visible = True
                                bAnyAbrufgrundzusatz = True
                            Else
                                If Not bAnyAbrufgrundzusatz Then
                                    DataGrid1.Columns(DataGrid1.Columns.Count - 1).Visible = False
                                End If
                            End If
                            If tmprows2(0).Item("MitZusatzText") Then
                                Dim tmptxt As TextBox
                                tmptxt = e.Item.FindControl("txtZusatztext")
                                If Not tmptxt Is Nothing Then
                                    tmptxt.Visible = True
                                    If Not tmprows(0).Item("text50") Is DBNull.Value Then
                                        tmptxt.Text = tmprows(0).Item("text50")
                                    End If
                                End If
                                DataGrid1.Columns(DataGrid1.Columns.Count - 1).Visible = True
                                bAnyAbrufgrundzusatz = True
                            Else
                                Dim tmptxt As TextBox
                                tmptxt = e.Item.FindControl("txtZusatztext")
                                If Not tmptxt Is Nothing Then
                                    tmptxt.Visible = False
                                End If
                                If Not bAnyAbrufgrundzusatz Then
                                    DataGrid1.Columns(DataGrid1.Columns.Count - 1).Visible = False
                                End If
                            End If
                        End If
                    End If
                End If
            Else
                tmpddl.Visible = False
            End If
        End If
    End Sub

    Public Function cmbAbrufgrund_ItemDataBound1(ByVal MANDT As String) As DataView

        If objHaendler Is Nothing Then
            objHaendler = CType(Session("AppHaendler"), F1_Haendler)
        End If
        Dim vwAbrufgruende As DataView = objHaendler.Abrufgruende.DefaultView

        Select Case MANDT
            Case "1"
                vwAbrufgruende.RowFilter = "AbrufTyp='temp'"
            Case "2"
                vwAbrufgruende.RowFilter = "AbrufTyp='endg'"
            Case "3"
                vwAbrufgruende.RowFilter = "AbrufTyp='Ret'"
            Case "4"
                vwAbrufgruende.RowFilter = "AbrufTyp='DP'"
            Case "6"
                vwAbrufgruende.RowFilter = "AbrufTyp='kfkl'"
        End Select
        Return vwAbrufgruende
    End Function

    Private Sub cmdSave2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave2.Click
        If CheckGrid2() Then

            'If objHaendler.Fahrzeuge.Select("AUGRU = '129'").Length > 0 OrElse objHaendler.Fahrzeuge.Select("AUGRU = '130'").Length > 0 Then
            '    cmdSave2.Visible = False
            '    divMessage.Visible = True
            '    Exit Sub
            'End If



            objHaendler.ErsetzeAbrufgruende()
            Session("AppHaendler") = objHaendler
            Response.Redirect("Change07_3.aspx?AppID=" & Session("AppID").ToString)

        Else
            lblError.Text = "Bitte geben Sie die Informationen zu den Abrufgründen ein."
            FillGrid(0)
        End If
    End Sub
    Protected Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click

        Dim dRows As DataRow() = objHaendler.Fahrzeuge.Select("AUGRU <> '129' AND AUGRU <> '130'")

        For Each dRow As DataRow In dRows

            dRow("MANDT") = "0"

        Next

        objHaendler.ErsetzeAbrufgruende()
        Session("AppHaendler") = objHaendler
        Response.Redirect("Change07_3.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub FillAbrufgruende()

        If objHaendler Is Nothing Then
            objHaendler = CType(Session("AppHaendler"), F1_Haendler)
        End If
        Dim vwAbrufgruende As DataView = objHaendler.Abrufgruende.DefaultView

        vwAbrufgruende.RowFilter = "AbrufTyp='endg'"

        Dim AbrufItem As New ListItem

        AbrufItem.Value = "0"
        AbrufItem.Text = "----- Auswahl Abrufgrund -----"

        ddlAbrufgrund.Items.Add(AbrufItem)


        For i As Integer = 0 To vwAbrufgruende.Count - 1

            AbrufItem = New ListItem

            AbrufItem.Value = vwAbrufgruende(i)("SapWert").ToString
            AbrufItem.Text = vwAbrufgruende(i)("WebBezeichnung").ToString

            ddlAbrufgrund.Items.Add(AbrufItem)
        Next


    End Sub

#End Region

    Protected Sub DataGrid1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DataGrid1.SelectedIndexChanged


    End Sub

    Protected Sub ddlAbrufgrund_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAbrufgrund.SelectedIndexChanged


        If ddlAbrufgrund.SelectedValue <> "0" Then


            If objHaendler Is Nothing Then
                objHaendler = CType(Session("AppHaendler"), F1_Haendler)
            End If


            objHaendler.Fahrzeuge.DefaultView.RowFilter = "MANDT <> '0'"


            For i As Integer = 0 To objHaendler.Fahrzeuge.DefaultView.Count - 1

                objHaendler.Fahrzeuge.DefaultView(i)("AUGRU") = ddlAbrufgrund.SelectedValue



            Next

            Session("AppHaendler") = objHaendler


            bAnyAbrufgrundzusatz = False

            Dim tmpddl As DropDownList

            For Each item As DataGridItem In DataGrid1.Items

                tmpddl = CType(item.FindControl("cmbAbrufgrund"), DropDownList)
                tmpddl.SelectedValue = ddlAbrufgrund.SelectedValue '******


                If Not tmpddl Is Nothing AndAlso tmpddl.Items.Count > 0 Then
                    Dim tmprows() As DataRow 'für fahrzeugtabelle
                    If objHaendler Is Nothing Then
                        objHaendler = CType(Session("AppHaendler"), F1_Haendler)
                    End If
                    tmprows = objHaendler.Fahrzeuge.Select("ZZFAHRG = '" & CType(item.FindControl("lnkHistorie"), HyperLink).Text & "'")
                    If Not tmprows.Length = 0 Then
                        Dim tmpListItem As ListItem
                        If Not tmprows(0).Item("AUGRU") Is DBNull.Value AndAlso Not CStr(tmprows(0).Item("AUGRU")) Is String.Empty Then
                            'tmpListItem = tmpddl.Items.FindByValue(CStr(tmprows(0).Item("AUGRU")))
                            tmpListItem = tmpddl.Items.FindByValue(ddlAbrufgrund.SelectedValue)
                            If Not tmpListItem Is Nothing Then
                                tmpddl.SelectedIndex = tmpddl.Items.IndexOf(tmpListItem)
                                'es muss festgestellt werden ob aktelle DDL auswahl einen Textbox anzeigen soll, db=MitZusatzText Boolean
                                'und das label lblZusatzinfo soll gefüllt werden 
                                Dim vwAbrufgruende As DataView = objHaendler.Abrufgruende.DefaultView
                                Select Case tmprows(0).Item("MANDT").ToString
                                    Case "1"
                                        vwAbrufgruende.RowFilter = "AbrufTyp='temp'"
                                    Case "2"
                                        vwAbrufgruende.RowFilter = "AbrufTyp='endg'"
                                End Select
                                Dim tmprows2() As DataRow 'für abrufgründe
                                tmprows2 = objHaendler.Abrufgruende.Select("SapWert='" & tmpddl.SelectedItem.Value & "'")
                                Dim tmpLabel As Label
                                tmpLabel = CType(item.FindControl("lblZusatzinfo"), Label)
                                If Not tmprows2(0).Item("Zusatzbemerkung") Is DBNull.Value AndAlso Not tmprows2(0).Item("Zusatzbemerkung").ToString = "" Then
                                    tmpLabel.Text = tmprows2(0).Item("Zusatzbemerkung")
                                    DataGrid1.Columns(DataGrid1.Columns.Count - 1).Visible = True
                                    bAnyAbrufgrundzusatz = True
                                Else
                                    If Not bAnyAbrufgrundzusatz Then
                                        DataGrid1.Columns(DataGrid1.Columns.Count - 1).Visible = False
                                    End If
                                End If
                                If tmprows2(0).Item("MitZusatzText") Then
                                    Dim tmptxt As TextBox
                                    tmptxt = item.FindControl("txtZusatztext")
                                    If Not tmptxt Is Nothing Then
                                        tmptxt.Visible = True
                                        If Not tmprows(0).Item("text50") Is DBNull.Value Then
                                            tmptxt.Text = tmprows(0).Item("text50")
                                        End If
                                    End If
                                    DataGrid1.Columns(DataGrid1.Columns.Count - 1).Visible = True
                                    bAnyAbrufgrundzusatz = True
                                Else
                                    Dim tmptxt As TextBox
                                    tmptxt = item.FindControl("txtZusatztext")
                                    If Not tmptxt Is Nothing Then
                                        tmptxt.Visible = False
                                    End If
                                    If Not bAnyAbrufgrundzusatz Then
                                        DataGrid1.Columns(DataGrid1.Columns.Count - 1).Visible = False
                                    End If
                                End If
                            End If
                        End If
                    End If
                Else
                    tmpddl.Visible = False
                End If



            Next

        End If


    End Sub


    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        divMessage.Visible = False
        cmdSave2.Visible = True
    End Sub



    Private Sub ibtMsgCancel_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtMsgCancel.Click
        divMessage.Visible = False
        cmdSave2.Visible = True
    End Sub
End Class