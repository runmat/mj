Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Business

Partial Public Class Change04_4
    Inherits System.Web.UI.Page
    Private m_App As Security.App
    Private m_User As Security.User
    Private objHaendler As Haendler
    Private objBank As AppF2BankBaseCredit
    Private objSuche As AppF2.Search
    Private m_Change As AppF2_05

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        m_App = New Security.App(m_User)
        GridNavigation1.setGridElment(GridView1)
        GridNavigation1.PageSizeIndex = 1000
        GridView1.PageSize = 1000
        GetAppIDFromQueryString(Me)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        lnkFahrzeugAuswahl.NavigateUrl = "Change04_2.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugsuche.NavigateUrl = "Change04.aspx?AppID=" & Session("AppID").ToString
        lnkAdressAuswahl.NavigateUrl = "Change04_3.aspx?AppID=" & Session("AppID").ToString

        If Session("AppHaendler") Is Nothing Then
            Response.Redirect("Change42.aspx?AppID=" & Session("AppID").ToString)
        Else
            If objHaendler Is Nothing Then
                objHaendler = CType(Session("AppHaendler"), Haendler)
            End If

        End If

        If Session("objSuche") Is Nothing Then
            objSuche = New AppF2.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
            Session("objSuche") = objSuche
        Else
            If TypeOf Session("objSuche") Is AppF2.Search Then
                objSuche = CType(Session("objSuche"), AppF2.Search)
            Else
                objSuche = New AppF2.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                Session("objSuche") = objSuche
            End If
        End If



        If Not objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, objHaendler.Customer, Me) Then
            lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
        End If

        lnkFahrzeugAuswahl.NavigateUrl = "Change04_2.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugsuche.NavigateUrl = "Change04.aspx?AppID=" & Session("AppID").ToString
        lnkAdressAuswahl.NavigateUrl = "Change04_3.aspx?AppID=" & Session("AppID").ToString
        objHaendler.Show(Session("AppID").ToString, Session.SessionID, Me) 'Kontingente füllen

        Session("objSuche") = objSuche

        If Not IsPostBack Then
            FillFinanzierungsart()
            CheckGrid2()
            FillGrid(0)
        End If


    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        'Fahrzeugtabelle instanziert?
        CheckFahrzeuge()

        Dim tmpDataView As New DataView()
        tmpDataView = objHaendler.Fahrzeuge.DefaultView
        If cmdSave2.Visible Then
            tmpDataView.RowFilter = "MANDT <> '0'"
        Else
            tmpDataView.RowFilter = ""
        End If
        If tmpDataView.Count = 0 Then
            GridView1.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
        Else
            If cmdSave2.Visible Then
                GridNavigation1.Visible = False
                GridView1.AllowSorting = False
            Else
                GridNavigation1.Visible = True
                GridView1.AllowSorting = True
            End If

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

            GridView1.DataSource = tmpDataView
            GridView1.DataBind()


            If tmpDataView.Count = 1 Then
                lblNoData.Text = "Es wurde ein nicht angefordertes Dokument gefunden."
            Else
                lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " nicht angeforderte Dokumente gefunden."
            End If
            lblNoData.Visible = True
            GridView1.DataBind()

            AbrufDatagridView()
        End If
    End Sub

    Private Sub GridNavigation1_ddlPageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub
    Private Sub DataGrid1_PageIndexChanged(ByVal pageindex As Int32) Handles GridNavigation1.PagerChanged
        GridView1.EditIndex = -1
        FillGrid(pageindex)
    End Sub
    Public Function cmbAbrufgrund_ItemDataBound1(ByVal MANDT As String, ByVal FinArt As String) As DataView

        If objHaendler Is Nothing Then
            objHaendler = CType(Session("AppHaendler"), Haendler)
        End If
        objHaendler.VersandAdressArt = ""
        GenerateAbrufgruende(FinArt)
        If objHaendler.VersandAdressArt <> "" Then
            Dim vwAbrufgruende As DataView = objHaendler.Abrufgruende.DefaultView
            vwAbrufgruende.Sort = "WebBezeichnung"
            Select Case MANDT
                Case "1"
                    vwAbrufgruende.RowFilter = "AbrufTyp='temp'"
                Case "2"
                    vwAbrufgruende.RowFilter = "AbrufTyp='endg'"
            End Select


            Return vwAbrufgruende
        Else
            Dim tmpTable As New DataTable
            tmpTable.Columns.Add("SapWert", System.Type.GetType("System.String"))
            tmpTable.Columns.Add("WebBezeichnung", System.Type.GetType("System.String"))
            Dim datRow As DataRow
            datRow = tmpTable.NewRow
            datRow("SapWert") = "000"
            datRow("WebBezeichnung") = "Kein passender Abrufgrund gefunden!"
            tmpTable.Rows.Add(datRow)

            Return tmpTable.DefaultView


        End If

    End Function
    Private Sub GenerateAbrufgruende(ByVal FinArt As String)
        If FinArt = "02" Then 'Finanzierung
            If objHaendler.FlagVersand = "1" Then ' Haendlerauswahl
                objHaendler.VersandAdressArt = "AND VersandAdressArt = 2"
            End If

            If objHaendler.FlagVersand = "2" Then ' freie Adresse
                objHaendler.VersandAdressArt = "AND VersandAdressArt = 3"
            End If

            If objHaendler.FlagVersand = "3" Then ' Kundenadresse
                objHaendler.VersandAdressArt = "AND VersandAdressArt = 1"
            End If
        ElseIf FinArt = "01" Then 'Leasing

            If objHaendler.FlagVersand = "1" Then ' Haendlerauswahl
                objHaendler.VersandAdressArt = "AND VersandAdressArt = 5"
            End If

            If objHaendler.FlagVersand = "2" Then ' freie Adresse
                objHaendler.VersandAdressArt = "AND VersandAdressArt = 7"
            End If
        ElseIf FinArt = "03" Then 'BCA / Directsales
            If objHaendler.FlagVersand = "1" Then ' Haendlerauswahl
                objHaendler.VersandAdressArt = "AND VersandAdressArt = 4"
            End If
            If objHaendler.FlagVersand = "2" Then ' freie Adresse
                objHaendler.VersandAdressArt = "AND VersandAdressArt = 11"
            End If

        Else

        End If
    End Sub

    Private Sub AbrufDatagridView()
        GridView1.Columns(0).Visible = False
        GridView1.Columns(1).Visible = True
        GridView1.Columns(2).Visible = False
        GridView1.Columns(3).Visible = False
        GridView1.Columns(4).Visible = True
        GridView1.Columns(5).Visible = False
        GridView1.Columns(6).Visible = True
        GridView1.Columns(7).Visible = False
        GridView1.Columns(8).Visible = False
        GridView1.Columns(9).Visible = False
        GridView1.Columns(10).Visible = True
        GridView1.Columns(11).Visible = True
        GridView1.Columns(12).Visible = True

    End Sub

    Private Function CheckGrid2() As Boolean
        Dim item As GridViewRow
        Dim cell As TableCell
        Dim ddl As DropDownList
        Dim txtBox As TextBox
        Dim control As Control
        Dim intZaehl As Int32 = 0
        Dim lbl As Label
        Dim blnReturn As Boolean = False
        Dim tmpRows As DataRow()
        'Fahrzeugtabelle instanziert?
        CheckFahrzeuge()
        If txtKopf.Text <> String.Empty Then
            For Each tmpRow In objHaendler.Fahrzeuge.Rows
                tmpRow("KOPFTEXT") = txtKopf.Text
            Next
        End If
        For Each item In GridView1.Rows
            Dim strZZFAHRG As String = ""
            cell = item.Cells(4)
            For Each control In cell.Controls
                If TypeOf control Is Label Then
                    lbl = CType(control, Label)
                    If lbl.ID = "lblFahrg" Then
                        strZZFAHRG = "ZZFAHRG = '" & lbl.Text & "'"
                    End If
                End If
            Next
            For Each cell In item.Cells
                For Each control In cell.Controls
                    If TypeOf control Is DropDownList Then
                        ddl = CType(control, DropDownList)
                        If ddl.ID = "cmbAbrufgrund" Then
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


    Private Sub CheckFahrzeuge()
        'Wenn die Tabelle Fahrzeuge Nothing ist, hat der User mehrmals die Navigation des Browsers benutzt
        'Dann in die Fahrzeugsuche zurückleiten
        If IsNothing(objHaendler.Fahrzeuge) = True Then
            Response.Redirect("Change04.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub

    Protected Sub cmdSave2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave2.Click
        If CheckGrid2() Then
            objHaendler.ErsetzeAbrufgruende()
            Session("AppHaendler") = objHaendler
            Response.Redirect("Change04_5.aspx?AppID=" & Session("AppID").ToString)
        Else
            lblError.Text = "Bitte geben Sie die Informationen zu den Abrufgründen ein."
            FillGrid(GridView1.PageIndex)
        End If
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(GridView1)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub cmdSaveMemo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSaveMemo.Click
        txtKopf.Visible = Not txtKopf.Visible
        If txtKopf.Visible = False Then
            txtKopf.Text = String.Empty
        End If
    End Sub

    Private Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        Dim cell As TableCell
        Dim lbl As Label
        If e.Row.RowType = ListItemType.Item OrElse e.Row.RowType = ListItemType.AlternatingItem Then
            Dim tmpddl As DropDownList
            tmpddl = CType(e.Row.FindControl("cmbAbrufgrund"), DropDownList)
            If Not tmpddl Is Nothing Then
                Dim tmprows() As DataRow 'für fahrzeugtabelle
                If objHaendler Is Nothing Then
                    objHaendler = CType(Session("AppHaendler"), Haendler)
                End If
                Dim strZZFAHRG As String = ""
                cell = e.Row.Cells(4)
                For Each control In cell.Controls
                    If TypeOf control Is Label Then
                        lbl = CType(control, Label)
                        If lbl.ID = "lblFahrg" Then
                            strZZFAHRG = "ZZFAHRG = '" & lbl.Text & "'"
                        End If
                    End If
                Next
                tmprows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
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
                            tmpLabel = CType(e.Row.FindControl("lblZusatzinfo"), Label)
                            If Not tmprows2(0).Item("Zusatzbemerkung") Is DBNull.Value Then
                                tmpLabel.Text = tmprows2(0).Item("Zusatzbemerkung")
                            End If
                            If tmprows2(0).Item("MitZusatzText") Then
                                Dim tmptxt As TextBox
                                tmptxt = e.Row.FindControl("txtZusatztext")
                                If Not tmptxt Is Nothing Then
                                    tmptxt.Visible = True
                                    If Not tmprows(0).Item("text50") Is DBNull.Value Then
                                        tmptxt.Text = tmprows(0).Item("text50")
                                    End If
                                End If
                            Else
                                Dim tmptxt As TextBox
                                tmptxt = e.Row.FindControl("txtZusatztext")
                                If Not tmptxt Is Nothing Then
                                    tmptxt.Visible = False
                                End If

                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex)
    End Sub

    Protected Sub cmbAbrufgrund_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        CheckGrid2()
        FillGrid(GridView1.PageIndex)
    End Sub

    Protected Sub ddlFinanzierungsart_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFinanzierungsart.SelectedIndexChanged
        If ddlFinanzierungsart.SelectedValue <> "000" Then

            ddlAbrufgrund.Visible = True
            lblAbrufgrund.Visible = True

            ddlAbrufgrund.DataSource = cmbAbrufgrund_ItemDataBound1("2", ddlFinanzierungsart.SelectedValue)
            ddlAbrufgrund.DataValueField = "SapWert"
            ddlAbrufgrund.DataTextField = "WebBezeichnung"
            ddlAbrufgrund.DataBind()

        End If
    End Sub

    Protected Sub ddlAbrufgrund_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAbrufgrund.SelectedIndexChanged

        If ddlFinanzierungsart.SelectedValue = "000" Then
            lblError.Text = "Bitte wählen Sie zuerst eine Finanzierungart."
            Exit Sub
        End If

        If ddlAbrufgrund.SelectedValue <> "0" Then

            If objHaendler Is Nothing Then
                objHaendler = CType(Session("AppHaendler"), Haendler)
            End If

            objHaendler.Fahrzeuge.DefaultView.RowFilter = "MANDT <> '0' and ZZFINART_GRP = '" & ddlFinanzierungsart.SelectedValue & "'"


            For i As Integer = 0 To objHaendler.Fahrzeuge.DefaultView.Count - 1

                objHaendler.Fahrzeuge.DefaultView(i)("AUGRU") = ddlAbrufgrund.SelectedValue

            Next

            Session("AppHaendler") = objHaendler

            FillGrid(0)

            ddlAbrufgrund.Visible = False
            lblAbrufgrund.Visible = False
            ddlFinanzierungsart.SelectedValue = "000"
        End If

    End Sub




    Private Sub FillFinanzierungsart()

        Dim TempTable As New DataTable

        TempTable.Columns.Add("ZZFINART_GRP", GetType(System.String))
        TempTable.Columns.Add("ZZFINART_BEZ", GetType(System.String))

        TempTable.AcceptChanges()


        Dim NewRow As DataRow = TempTable.NewRow

        NewRow("ZZFINART_GRP") = "000"
        NewRow("ZZFINART_BEZ") = "- keine Auswahl -"

        TempTable.Rows.Add(NewRow)


        For Each Row As DataRow In objHaendler.Fahrzeuge.Rows

            If Row("MANDT") = "2" Then

                If TempTable.Select("ZZFINART_GRP ='" & Row("ZZFINART_GRP") & "'").Length = 0 Then
                    NewRow = TempTable.NewRow

                    NewRow("ZZFINART_GRP") = Row("ZZFINART_GRP")
                    NewRow("ZZFINART_BEZ") = Row("ZZFINART_BEZ")

                    TempTable.Rows.Add(NewRow)

                End If

            End If

        Next

        ddlFinanzierungsart.DataSource = TempTable.DefaultView

        ddlFinanzierungsart.DataValueField = "ZZFINART_GRP"
        ddlFinanzierungsart.DataTextField = "ZZFINART_BEZ"
        ddlFinanzierungsart.DataBind()

    End Sub
End Class