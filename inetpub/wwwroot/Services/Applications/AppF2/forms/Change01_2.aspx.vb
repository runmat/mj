Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Business

Partial Public Class Change01_2
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
        Kopfdaten1.Message = ""
        GridNavigation1.setGridElment(GridView1)
        GetAppIDFromQueryString(Me)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        lnkKreditlimit.NavigateUrl = "Change01.aspx?AppID=" & Session("AppID").ToString

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

        Kopfdaten1.UserReferenz = m_User.Reference
        Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
        Dim strTemp As String = objSuche.NAME
        'If objSuche.NAME_2.Length > 0 Then
        '    strTemp &= "<br>" & objSuche.NAME_2
        'End If
        Kopfdaten1.HaendlerName = strTemp
        Kopfdaten1.Adresse = objSuche.STREET & "<br>" & objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY
        objHaendler.Show(Session("AppID").ToString, Session.SessionID, Me) 'kontingente füllen
        Kopfdaten1.Kontingente = objHaendler.Kontingente

        Session("objSuche") = objSuche


        If Not IsPostBack Then

            GridView1.PageSize = 100

            GridNavigation1.PageSizeIndex = 3

            cmdSave.Visible = True
            Kopfdaten1.Kontingente = objHaendler.Kontingente


            FillGrid(0)

        End If

    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        'Fahrzeugtabelle instanziert?
        CheckFahrzeuge()

        Dim tmpDataView As New DataView()
        tmpDataView = objHaendler.Fahrzeuge.DefaultView
        tmpDataView.RowFilter = ""
        If tmpDataView.Count = 0 Then
            GridView1.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
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

            GridView1.DataSource = tmpDataView
            GridView1.PageIndex = intTempPageIndex
            GridView1.DataBind()


            If tmpDataView.Count = 1 Then
                lblNoData.Text = "Es wurde ein nicht angefordertes Dokument gefunden."
            Else
                lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " nicht angeforderte Dokumente gefunden."
            End If
            lblNoData.Visible = True
            'GridView1.DataBind()



            StandardDatagridView()
            If objHaendler.boolUpload = True Then
                GridView1.Columns(11).Visible = True
            End If
        End If
    End Sub

    Private Sub GridNavigation1_ddlPageSizeChanged() Handles GridNavigation1.PageSizeChanged
        CheckGrid()
        FillGrid(0)
    End Sub
    Private Sub DataGrid1_PageIndexChanged(ByVal pageindex As Int32) Handles GridNavigation1.PagerChanged
        CheckGrid()
        GridView1.EditIndex = -1
        FillGrid(pageindex)
    End Sub
    Public Function cmbAbrufgrund_ItemDataBound1(ByVal MANDT As String) As DataView

        If objHaendler Is Nothing Then
            objHaendler = CType(Session("AppHaendler"), Haendler)
        End If
        Dim vwAbrufgruende As DataView = objHaendler.Abrufgruende.DefaultView

        Select Case MANDT
            Case "1"
                vwAbrufgruende.RowFilter = "AbrufTyp='temp'"
            Case "2"
                vwAbrufgruende.RowFilter = "AbrufTyp='endg'"
        End Select
        Return vwAbrufgruende
    End Function

    Private Sub StandardDatagridView()
        GridView1.Columns(0).Visible = False
        GridView1.Columns(1).Visible = True
        GridView1.Columns(2).Visible = True
        GridView1.Columns(3).Visible = True
        GridView1.Columns(4).Visible = True
        GridView1.Columns(5).Visible = True
        GridView1.Columns(6).Visible = True
        GridView1.Columns(7).Visible = True
        GridView1.Columns(8).Visible = False
        GridView1.Columns(9).Visible = False
        GridView1.Columns(10).Visible = True
        GridView1.Columns(11).Visible = True



    End Sub

    Private Function CheckGrid() As Int32
        Dim item As GridViewRow
        Dim cell As TableCell
        Dim txtBox As TextBox
        Dim chkAuswahl As CheckBox
        Dim lbl As Label
        Dim control As Control
        Dim blnScriptFound As Boolean = False
        Dim intZaehl As Int32
        Dim intReturn As Int32 = 0

        Dim tmpRows As DataRow()

        'Fahrzeugtabelle instanziert?
        CheckFahrzeuge()

        For Each item In GridView1.Rows
            intZaehl = 1
            Dim strZZFAHRG As String = ""

            cell = item.Cells(5)
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
                    If TypeOf control Is CheckBox Then
                        chkAuswahl = CType(control, CheckBox)
                        If chkAuswahl.ID = "chkTemp" Then
                            If chkAuswahl.Checked Then
                                objHaendler.Fahrzeuge.AcceptChanges()
                                tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                                tmpRows(0).BeginEdit()
                                tmpRows(0).Item("MANDT") = "1"
                                tmpRows(0).EndEdit()
                                objHaendler.Fahrzeuge.AcceptChanges()
                            ElseIf Not chkAuswahl.Checked Then
                                objHaendler.Fahrzeuge.AcceptChanges()
                                tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                                tmpRows(0).BeginEdit()
                                tmpRows(0).Item("MANDT") = "0"
                                tmpRows(0).EndEdit()
                                objHaendler.Fahrzeuge.AcceptChanges()
                            End If
                        End If
                    End If
                    'Positionstexte lesen
                    If TypeOf control Is TextBox Then
                        txtBox = CType(control, TextBox)
                        If txtBox.ID = "txtPosition" Then
                            objHaendler.Fahrzeuge.AcceptChanges()
                            tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                            tmpRows(0).BeginEdit()
                            tmpRows(0).Item("POSITIONSTEXT") = txtBox.Text
                            tmpRows(0).EndEdit()
                            objHaendler.Fahrzeuge.AcceptChanges()
                        End If
                    End If
                Next
                intZaehl += 1
            Next
        Next
        Session("AppHaendler") = objHaendler
        Return intReturn
    End Function

    Private Function CheckGrid2() As Boolean
        Dim item As GridViewRow
        Dim cell As TableCell
        Dim ddl As DropDownList
        Dim txtBox As TextBox
        Dim control As control
        Dim intZaehl As Int32 = 0
        Dim lbl As Label
        Dim blnReturn As Boolean = False
        Dim tmpRows As DataRow()
        'Fahrzeugtabelle instanziert?
        CheckFahrzeuge()

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
            Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub

    Protected Sub cmdSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        CheckGrid()

        Dim tmpDataView As New DataView()
        tmpDataView = objHaendler.Fahrzeuge.DefaultView

        tmpDataView.RowFilter = "MANDT <> '0'"
        Dim intFahrzeugBriefe As Int32 = tmpDataView.Count
        tmpDataView.RowFilter = ""

        If intFahrzeugBriefe = 0 Then
            lblError.Text = "Bitte wählen Sie erst Fahrzeuge zur Anforderung aus."
            FillGrid(GridView1.PageIndex)

        Else
            Session("AppHaendler") = objHaendler
            Response.Redirect("Change01_3.aspx?AppID=" & Session("AppID").ToString)

        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(GridView1)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        CheckGrid()
        FillGrid(GridView1.PageIndex)
    End Sub


End Class