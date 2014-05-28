Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change04_2
    Inherits System.Web.UI.Page
    Private m_User As Security.User
    Private m_App As Security.App
    Private objSuche As Search
    Private objHaendler As Porsche_05

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)

        kopfdaten1.Message = ""
        lnkKreditlimit.NavigateUrl = "Change04.aspx?AppID=" & Session("AppID").ToString
        GridNavigation1.setGridElment(GridView1)
        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            m_App = New Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change04.aspx?AppID=" & Session("AppID").ToString)
            End If

            If Session("objSuche") Is Nothing Then
                Response.Redirect("Change04.aspx?AppID=" & Session("AppID").ToString)
            Else
                objSuche = CType(Session("objSuche"), Search)
            End If

            kopfdaten1.UserReferenz = m_User.Reference
            kopfdaten1.HaendlerNummer = objSuche.REFERENZ
            Dim strTemp As String = objSuche.NAME
            If objSuche.NAME_2.Length > 0 Then
                strTemp &= "<br />" & objSuche.NAME_2
            End If
            kopfdaten1.HaendlerName = strTemp
            Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br />" & objSuche.STREET

            Session("objSuche") = objSuche

            objHaendler = CType(Session("objHaendler"), Porsche_05)

            If Not IsPostBack Then


                kopfdaten1.Kontingente = objHaendler.Kontingente
                FillGrid(0)

            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
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
            Session("objHaendler") = objHaendler
            Response.Redirect("Change04_3.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub

    Private Function CheckGrid() As Int32
        Dim item As GridViewRow
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
                tmpRow("KOPFTEXT") = txtKopf.Text
            Next
        End If

        For Each item In GridView1.Rows
            intZaehl = 1
            Dim strZZFAHRG As String = ""
            For Each cell In item.Cells
                If intZaehl = 1 Then
                    strZZFAHRG = "ZZFAHRG = '" & cell.Text & "'"
                End If
                For Each control In cell.Controls
                    If TypeOf control Is CheckBox Then
                        chbox = CType(control, CheckBox)
                        objHaendler.Fahrzeuge.AcceptChanges()
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
                        End Select
                        tmpRows(0).EndEdit()
                        objHaendler.Fahrzeuge.AcceptChanges()
                    End If
                    'Positionstexte lesen
                    If TypeOf control Is TextBox Then
                        txtBox = CType(control, TextBox)
                        tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                        tmpRows(0).BeginEdit()
                        tmpRows(0).Item("POSITIONSTEXT") = txtBox.Text
                    End If
                Next
                intZaehl += 1
            Next
        Next
        Session("objHaendler") = objHaendler
        Return intReturn
    End Function

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        'Fahrzeugtabelle instanziert?
        CheckFahrzeuge()

        Dim tmpDataView As New DataView()
        tmpDataView = objHaendler.Fahrzeuge.DefaultView
        tmpDataView.RowFilter = ""

        If tmpDataView.Count = 0 Then
            GridView1.Visible = False
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            lblNoData.Visible = True
            ShowScript.Visible = False
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


            lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " nicht angeforderte Briefe gefunden."
            lblNoData.Visible = True

            If objHaendler.Kontingente.Rows.Count = 2 Then  'Nur Standard-Kontingent->Spalte ausblenden
                GridView1.Columns(GridView1.Columns.Count - 2).Visible = False
            Else
                If objHaendler.Kontingente.Rows.Count > 2 Then  'Delayed Payment-Kontingent vorhanden?
                    If objHaendler.Kontingente.Rows(2)("Kreditkontrollbereich").ToString = "0004" Then  'Kann nur in Zeile 3 sein...
                        If CInt(objHaendler.Kontingente.Rows(2)("Richtwert_Alt")) = 0 Then 'Richtwert=0, ausblenden!
                            GridView1.Columns(GridView1.Columns.Count - 2).Visible = False
                        End If
                    Else
                        GridView1.Columns(GridView1.Columns.Count - 2).Visible = False  'Nein, dann sowieso ausblenden
                    End If
                End If
            End If

            Dim item As GridViewRow
            Dim cell As TableCell
            Dim chkBox As CheckBox
            Dim control As Control
            Dim blnScriptFound As Boolean = False
            Dim intZaehl As Int32
            Dim row As DataRow

            For Each item In GridView1.Rows
                intZaehl = 1
                Dim strKKB As String = "0"

                row = objHaendler.Fahrzeuge.Select("ZZFAHRG='" & item.Cells(0).Text & "'")(0)
                strKKB = CStr(row("MANDT"))

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
                                    If strKKB = "1" Then
                                        chkBox.Checked = True
                                    End If
                                Case "chk0002"
                                    If strKKB = "2" Then
                                        chkBox.Checked = True
                                    End If
                            End Select
                        End If
                    Next
                    intZaehl += 1
                Next
            Next
        End If
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
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
            Response.Redirect("Change04.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub

    Private Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        Dim datCell As TableCell
        Dim chkBox As CheckBox

        For Each datCell In e.Row.Cells
            chkBox = CType(datCell.FindControl("Checkbox1"), CheckBox)
            If Not chkBox Is Nothing Then
                chkBox.InputAttributes.Add("disabled", "disabled")
            End If

        Next
    End Sub


    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        CheckGrid()
        FillGrid(GridView1.PageIndex, e.SortExpression)
    End Sub


    Private Sub GridNavigation1_ddlPageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub
    Private Sub DataGrid1_PageIndexChanged(ByVal pageindex As Int32) Handles GridNavigation1.PagerChanged
        GridView1.EditIndex = -1
        FillGrid(pageindex)
    End Sub
End Class