Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Components.ComCommon

Public Class Change42_2
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
    Private objSuche As CKG.Components.ComCommon.Finance.Search
    Private objHaendler As fin_06
    Private mstrHDL As String
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents kopfdaten1 As PageElements.Kopfdaten
    Protected WithEvents ucHeader As Header
    Protected WithEvents txtKopf As System.Web.UI.WebControls.TextBox
    Protected WithEvents LinkButton1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdSave2 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents trcmdSave As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trcmdSave2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lnkFahrzeugAuswahl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        kopfdaten1.Message = ""
        lnkKreditlimit.NavigateUrl = "Change42.aspx?AppID=" & Session("AppID").ToString & "&back=1"
        lnkFahrzeugAuswahl.NavigateUrl = "Change42_2.aspx?AppID=" & Session("AppID").ToString

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

        If Session("AppHaendler") Is Nothing Then
            Response.Redirect("Change42.aspx?AppID=" & Session("AppID").ToString)
        Else
            If objHaendler Is Nothing Then
                objHaendler = CType(Session("AppHaendler"), fin_06)
            End If

        End If

        If Session("objSuche") Is Nothing Then
            objSuche = New Finance.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
            Session("objSuche") = objSuche
        Else
            If TypeOf Session("objSuche") Is Finance.Search Then
                objSuche = CType(Session("objSuche"), Finance.Search)
            Else
                objSuche = New Finance.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                Session("objSuche") = objSuche
            End If
        End If



        If Not objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, objHaendler.Customer) Then
            lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
        End If

        kopfdaten1.UserReferenz = m_User.Reference
        kopfdaten1.HaendlerNummer = objSuche.REFERENZ
        Dim strTemp As String = objSuche.NAME
        If objSuche.NAME_2.Length > 0 Then
            strTemp &= "<br>" & objSuche.NAME_2
        End If
        kopfdaten1.HaendlerName = strTemp
        kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET
        objHaendler.Show(Session("AppID").ToString, Session.SessionID) 'kontingente füllen
        kopfdaten1.Kontingente = objHaendler.Kontingente

        Session("objSuche") = objSuche


        If Not IsPostBack Then


            trcmdSave.Visible = True
            trcmdSave2.Visible = False
            lnkFahrzeugAuswahl.Visible = False

            ddlPageSize.Items.Add("10")
            ddlPageSize.Items.Add("20")
            ddlPageSize.Items.Add("50")
            ddlPageSize.Items.Add("100")
            ddlPageSize.Items.Add("200")
            ddlPageSize.Items.Add("500")
            ddlPageSize.Items.Add("1000")
            ddlPageSize.SelectedIndex = 2
            kopfdaten1.Kontingente = objHaendler.Kontingente


            FillGrid(0)
        End If

        If IsPostBack AndAlso trcmdSave2.Visible Then
            CheckGrid2()
            FillGrid(0)
        End If

    End Sub


    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        DoSubmit()
    End Sub

    Private Sub cmdSave2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave2.Click
        If CheckGrid2() Then
            objHaendler.ErsetzeAbrufgruende()
            Session("AppHaendler") = objHaendler
            Response.Redirect("Change42_3.aspx?AppID=" & Session("AppID").ToString)
        Else
            lblError.Text = "Bitte geben Sie die Informationen zu den Abrufgründen ein."
            FillGrid(DataGrid1.CurrentPageIndex)
        End If
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
            FillGrid(DataGrid1.CurrentPageIndex)
        Else
            trcmdSave.Visible = False
            trcmdSave2.Visible = True

            FillGrid(DataGrid1.CurrentPageIndex)

            lnkFahrzeugAuswahl.Visible = True
        End If
    End Sub

    Private Sub StandardDatagridView()
        DataGrid1.Columns(0).Visible = False
        DataGrid1.Columns(1).Visible = False
        DataGrid1.Columns(2).Visible = True
        DataGrid1.Columns(3).Visible = True
        DataGrid1.Columns(4).Visible = True
        DataGrid1.Columns(5).Visible = True
        DataGrid1.Columns(6).Visible = True
        DataGrid1.Columns(7).Visible = False
        DataGrid1.Columns(8).Visible = False
        DataGrid1.Columns(9).Visible = True
        DataGrid1.Columns(10).Visible = True
        DataGrid1.Columns(11).Visible = True
        DataGrid1.Columns(12).Visible = True
        DataGrid1.Columns(13).Visible = True
        DataGrid1.Columns(14).Visible = False
        DataGrid1.Columns(15).Visible = False
        DataGrid1.Columns(16).Visible = False
    End Sub

    Private Sub AbrufDatagridView()
        DataGrid1.Columns(0).Visible = False
        DataGrid1.Columns(1).Visible = False
        DataGrid1.Columns(2).Visible = True
        DataGrid1.Columns(3).Visible = False
        DataGrid1.Columns(4).Visible = False
        DataGrid1.Columns(5).Visible = True
        DataGrid1.Columns(6).Visible = False
        DataGrid1.Columns(7).Visible = False
        DataGrid1.Columns(8).Visible = False
        DataGrid1.Columns(9).Visible = False
        DataGrid1.Columns(10).Visible = False
        DataGrid1.Columns(11).Visible = False
        DataGrid1.Columns(12).Visible = False
        DataGrid1.Columns(13).Visible = False
        DataGrid1.Columns(14).Visible = True
        DataGrid1.Columns(15).Visible = True
        DataGrid1.Columns(16).Visible = True
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        CheckGrid()
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        CheckGrid()
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Function CheckGrid2() As Boolean
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim ddl As DropDownList
        Dim txtBox As TextBox
        Dim control As control
        Dim intZaehl As Int32 = 0
        Dim blnReturn As Boolean = False
        Dim tmpRows As DataRow()
        'Fahrzeugtabelle instanziert?
        CheckFahrzeuge()

        For Each item In DataGrid1.Items
            Dim strZZFAHRG As String = "ZZFAHRG = '" & item.Cells(0).Text & "'"

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

                                'For Each cell2 In item.Cells
                                '    For Each control2 In cell2.Controls
                                '        If TypeOf control2 Is TextBox Then
                                '            txtBox = CType(control2, TextBox)
                                '            If txtBox.ID = "txtZusatztext" Then
                                '                If CBool(objHaendler.Abrufgruende.Select("SapWert='" & ddl.SelectedItem.Value & "'")(0)("MitZusatzText")) Then
                                '                    txtBox.Enabled = True
                                '                    If txtBox.Text.Length = 0 Then
                                '                        'wenn kein Text eingegeben ist, ist ok, laut Rothe JJ20080123
                                '                        'intZaehl += 1
                                '                    Else
                                '                        objHaendler.Fahrzeuge.AcceptChanges()
                                '                        tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                                '                        tmpRows(0).BeginEdit()
                                '                        tmpRows(0).Item("ANFNR") = txtBox.Text
                                '                        tmpRows(0).EndEdit()
                                '                        objHaendler.Fahrzeuge.AcceptChanges()
                                '                    End If
                                '                End If
                                '            End If
                                '        End If
                                '    Next
                                'Next
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

    Private Function CheckGrid() As Int32
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim radiobutton As radiobutton
        Dim txtBox As TextBox
        Dim control As control
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

        For Each item In DataGrid1.Items
            intZaehl = 1
            Dim strZZFAHRG As String = ""
            For Each cell In item.Cells
                If intZaehl = 1 Then
                    strZZFAHRG = "ZZFAHRG = '" & cell.Text & "'"
                End If
                For Each control In cell.Controls
                    If TypeOf control Is radiobutton Then
                        radiobutton = CType(control, radiobutton)
                        If radiobutton.Checked Then
                            objHaendler.Fahrzeuge.AcceptChanges()
                            tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                            tmpRows(0).BeginEdit()
                            Select Case radiobutton.ID
                                Case "rbNichtAnfordern"
                                    tmpRows(0).Item("MANDT") = "0"
                                Case "rbTemporaer"
                                    tmpRows(0).Item("MANDT") = "1"
                                Case "rbEndgueltig"
                                    tmpRows(0).Item("MANDT") = "2"
                            End Select
                            tmpRows(0).EndEdit()
                            objHaendler.Fahrzeuge.AcceptChanges()
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

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")


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
            If trcmdSave2.Visible Then
                ddlPageSize.Visible = False
                DataGrid1.AllowPaging = False
                DataGrid1.AllowSorting = False
            Else
                ddlPageSize.Visible = True
                DataGrid1.AllowPaging = True
                DataGrid1.AllowSorting = True
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

            DataGrid1.CurrentPageIndex = intTempPageIndex

            DataGrid1.DataSource = tmpDataView
            DataGrid1.DataBind()


            If tmpDataView.Count = 1 Then
                lblNoData.Text = "Es wurde ein nicht angefordertes Dokument gefunden."
            Else
                lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " nicht angeforderte Dokumente gefunden."
            End If
            lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If

            If trcmdSave2.Visible Then
                AbrufDatagridView()
            Else
                StandardDatagridView()
            End If
        End If
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
            Response.Redirect("Change42.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub

    Public Function cmbAbrufgrund_ItemDataBound1(ByVal MANDT As String) As DataView

        If objHaendler Is Nothing Then
            objHaendler = CType(Session("AppHaendler"), fin_06)
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

    Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim tmpddl As DropDownList
            tmpddl = CType(e.Item.FindControl("cmbAbrufgrund"), DropDownList)
            If Not tmpddl Is Nothing Then
                Dim tmprows() As DataRow 'für fahrzeugtabelle
                If objHaendler Is Nothing Then
                    objHaendler = CType(Session("AppHaendler"), fin_06)
                End If
                tmprows = objHaendler.Fahrzeuge.Select("ZZFAHRG = '" & e.Item.Cells(0).Text & "'")
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
                            If Not tmprows2(0).Item("Zusatzbemerkung") Is DBNull.Value Then
                                tmpLabel.Text = tmprows2(0).Item("Zusatzbemerkung")
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
                            Else
                                Dim tmptxt As TextBox
                                tmptxt = e.Item.FindControl("txtZusatztext")
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

End Class

' ************************************************
' $History: Change42_2.aspx.vb $
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 22.06.09   Time: 13:18
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 Z_M_Creditlimit_Detail_001
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 19.06.09   Time: 10:10
' Updated in $/CKAG/Components/ComCommon/Finance
' ITa 2918 testfertig
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 21.08.08   Time: 13:37
' Updated in $/CKAG/Components/ComCommon/Finance
' BugFix Briefanforderung Rothe Mail 2008.08.21
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 30.07.08   Time: 12:35
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2119
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 5.05.08    Time: 14:01
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 1855
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 30.04.08   Time: 20:57
' Updated in $/CKAG/Components/ComCommon/Finance
' hotfix
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 29.04.08   Time: 16:24
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 1855
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:47
' Created in $/CKAG/portal/Components/ComCommon/Finance
' 
' *****************  Version 17  *****************
' User: Jungj        Date: 28.02.08   Time: 11:44
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ita 1733
' 
' *****************  Version 16  *****************
' User: Jungj        Date: 27.02.08   Time: 8:00
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ita 1733
' 
' *****************  Version 15  *****************
' User: Jungj        Date: 26.02.08   Time: 16:59
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1733
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 18.02.08   Time: 8:51
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' akf änderungen
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 15.02.08   Time: 11:20
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA:1677
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 12.02.08   Time: 9:15
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 1.02.08    Time: 16:01
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 31.01.08   Time: 16:44
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Bugfix AKF
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 23.01.08   Time: 12:59
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Bugfixes Rothe 
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 8.01.08    Time: 16:35
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 8.01.08    Time: 16:34
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 6  *****************
' User: Uha          Date: 8.01.08    Time: 16:07
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 5  *****************
' User: Uha          Date: 8.01.08    Time: 9:24
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 4  *****************
' User: Uha          Date: 7.01.08    Time: 18:47
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 3  *****************
' User: Uha          Date: 19.12.07   Time: 14:10
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1510,1512,1511 Anforderung / Zulassung
' 
' *****************  Version 2  *****************
' User: Uha          Date: 17.12.07   Time: 17:58
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Versand ZB II / Briefe - komplierfähige Zwischenversion
' 
' *****************  Version 1  *****************
' User: Uha          Date: 13.12.07   Time: 17:18
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Elemente für Temp./Endg. bzw. HEZ Anforderung hinzugefügt (Change42ff,
' fin_06, Change43ff und fin_08)
' 
' ************************************************

