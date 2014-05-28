Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business
Imports CKG.Components.ComCommon


Public Class Change50_1
    Inherits System.Web.UI.Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid


    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents lb_Auswahl As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Datagrid2 As System.Web.UI.WebControls.DataGrid




    Dim NewDatagrid As DataGrid
    Dim NewLiteral As Literal
    Dim m_change As fin_18

    Dim strEQUINR As String



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

        lblError.Text = ""
        m_User = GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte
        FormAuth(Me, m_User)
        m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
        ucHeader.InitUser(m_User)
        GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text


        If Page.IsPostBack = False Then
            If Not Session("objChange") Is Nothing Then
                m_change = CType(Session("objChange"), fin_18)
            Else
                Throw New Exception("Es wurde kein m_change Objekt in der Session gefunden")
            End If

            m_change.Show()
            FillGrid(0)

        End If
      
    End Sub
    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        If m_change Is Nothing Then
            m_change = CType(Session("objChange"), fin_18)
        End If


        If m_change.Result Is Nothing OrElse m_change.Result.Rows.Count = 0 Then
            DataGrid1.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
        Else
            DataGrid1.Visible = True
            lblNoData.Visible = False



            Dim tmpDataView As New DataView()
            tmpDataView = m_change.Result.DefaultView

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

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.Visible = True
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                If DataGrid1.CurrentPageIndex = DataGrid1.PageCount - 1 Then
                    DataGrid1.PagerStyle.NextPageText = "<img border=""0"" src=""/Portal/Images/empty.gif"" width=""12"" height=""11"">"
                Else
                    DataGrid1.PagerStyle.NextPageText = "<img border=""0"" src=""/Portal/Images/arrow_right.gif"" width=""12"" height=""11"">"
                End If

                If DataGrid1.CurrentPageIndex = 0 Then
                    DataGrid1.PagerStyle.PrevPageText = "<img border=""0"" src=""/Portal/Images/empty.gif"" width=""12"" height=""11"">"
                Else
                    DataGrid1.PagerStyle.PrevPageText = "<img border=""0"" src=""/Portal/Images/arrow_left.gif"" width=""12"" height=""11"">"
                End If
                DataGrid1.DataBind()
            Else
                DataGrid1.PagerStyle.Visible = False
            End If


            'Hinzufügen der verlinkung zur fahrzeughistorie JJU2008.08.20 ITA 2188
            '----------------------------------------------------
            For Each item As DataGridItem In DataGrid1.Items
                If Not item.FindControl("lnkHistorie") Is Nothing Then
                    CType(item.FindControl("lnkHistorie"), HyperLink).NavigateUrl = "Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN=" & CType(item.FindControl("lnkHistorie"), HyperLink).Text
                End If
            Next
            '----------------------------------------------------


            Dim tblDetails As DataTable
            Dim blnDetailsExist As Boolean = False
            If Not DataGrid1.EditItemIndex = -1 Then


                Dim item As DataGridItem = DataGrid1.Items(DataGrid1.EditItemIndex)


                If strEQUINR Is String.Empty Then
                    lblError.Text = "FEHLER: die Details Ansicht bekam keine EQUINR"
                    Exit Sub
                End If
                m_change.fillDetailsTable(strEQUINR)
                strEQUINR = String.Empty

                tblDetails = m_change.DetailsTable


                If Not tblDetails Is Nothing Then
                    If Not tblDetails.Rows.Count = 0 Then
                        blnDetailsExist = True
                    End If
                End If


                If blnDetailsExist Then

                    NewLiteral = New Literal()
                    NewLiteral.ID = "Entry"
                    NewLiteral.Text = "</TD></TR><TR><TD colspan=""" & item.Cells.Count & """>"
                    NewLiteral.EnableViewState = True
                    item.Cells(item.Cells.Count - 1).Controls.Add(NewLiteral)


                    NewDatagrid = New DataGrid()
                    NewDatagrid.ID = "NewDataGrid"
                    NewDatagrid.EnableViewState = True
                    NewDatagrid.Width = New Unit("100%")
                    NewDatagrid.AlternatingItemStyle.CssClass = "GridTableAlternate"
                    NewDatagrid.HeaderStyle.CssClass = "GridTableHeadSub"

                    NewDatagrid.DataSource = tblDetails
                    NewDatagrid.DataBind()
                    Dim newItem As DataGridItem
                    Dim newCell As TableCell

                    Dim templColumn1 As New TemplateColumn()




                    NewDatagrid.Columns.AddAt(NewDatagrid.Columns.Count - 1, templColumn1)


                    For Each newItem In NewDatagrid.Items
                        Dim x As Int32 = 0
                        For Each newCell In newItem.Cells


                            newCell.Font.Bold = True
                            newCell.HorizontalAlign = HorizontalAlign.Center
                            If x Mod 2 = 0 Then
                                newCell.BackColor = Color.Yellow
                            End If
                            x = x + 1
                        Next

                    Next
                    item.Cells(item.Cells.Count - 1).Controls.Add(NewDatagrid)
                End If
            End If
        End If
        
    End Sub



    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub



    Public Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand

        'sobald ein ItemCommand gefeuert wird, muss die aktuelle EQUNR für die Detailsansicht "herausgeholt werden" und für das festellen des richtigen Datensatzes
        strEQUINR = e.CommandArgument

        If e.CommandName = "Erledigt" Then

            If m_change Is Nothing Then
                m_change = CType(Session("objChange"), fin_18)
            End If

            'erst werte von altem Edit Modus Speichern 
            If Not DataGrid1.EditItemIndex = -1 Then

                If m_change Is Nothing Then
                    m_change = CType(Session("objChange"), fin_18)
                End If

                Dim txtmemo As TextBox = CType(DataGrid1.Items(DataGrid1.EditItemIndex).FindControl("txtMemo"), TextBox)

                If txtmemo.Text.Length > 120 Then
                    Dim script As String
                    script = "<" & "script language='javascript'>" & _
                              "alert('Es sind Maximal 120 Zeichen erlaubt, Ihr Text enthält " & txtmemo.Text.Length & " Zeichen' );" & _
                          "</" & "script>"
                    Response.Write(script)
                    Exit Sub
                End If
                m_change.Memo = txtmemo.Text
                m_change.Equipment = m_change.Result.Select("EQUNR='" & strEQUINR & "'")(0).Item("EQUNR")

                m_change.Erledigt = ""
                m_change.Dataus = m_change.Result.Select("EQUNR='" & strEQUINR & "'")(0).Item("Datum Ausgang")
                m_change.Change()
                m_change.Show()
            End If

            'dann erledigt Zeile schreiben
            m_change.Equipment = m_change.Result.Select("EQUNR='" & strEQUINR & "'")(0).Item("EQUNR")
            m_change.Memo = m_change.Result.Select("EQUNR='" & strEQUINR & "'")(0).Item("Memo")
            m_change.Erledigt = "X"
            m_change.Dataus = m_change.Result.Select("EQUNR='" & strEQUINR & "'")(0).Item("Datum Ausgang")
            m_change.Change()
            m_change.Show()
            lblError.Text = m_change.Message
            DataGrid1.EditItemIndex = -1
            FillGrid(DataGrid1.CurrentPageIndex)

        End If

        If e.CommandName = "Details" Then

            If Not DataGrid1.EditItemIndex = -1 Then



                If m_change Is Nothing Then
                    m_change = CType(Session("objChange"), fin_18)
                End If

                Dim txtmemo As TextBox = CType(DataGrid1.Items(DataGrid1.EditItemIndex).FindControl("txtMemo"), TextBox)

                If txtmemo.Text.Length > 120 Then
                    Dim script As String
                    script = "<" & "script language='javascript'>" & _
                              "alert('Es sind Maximal 120 Zeichen erlaubt, Ihr Text enthält " & txtmemo.Text.Length & " Zeichen' );" & _
                          "</" & "script>"
                    Response.Write(script)
                    Exit Sub
                End If
                m_change.Memo = txtmemo.Text
                m_change.Equipment = m_change.Result.Select("EQUNR='" & strEQUINR & "'")(0).Item("EQUNR").ToString

                m_change.Erledigt = ""
                m_change.Dataus = m_change.Result.Select("EQUNR='" & strEQUINR & "'")(0).Item("Datum Ausgang").ToString
                m_change.Change()
                m_change.Show()

            End If

            If DataGrid1.EditItemIndex = e.Item.ItemIndex Then
                DataGrid1.EditItemIndex = -1
            Else
                DataGrid1.EditItemIndex = e.Item.ItemIndex
            End If


            FillGrid(DataGrid1.CurrentPageIndex)

        End If
    End Sub

    Public Function SpaceRemover(ByVal wert As String) As String
        If wert = "&nbsp;" Then
            Return ""
        Else
            Return wert
        End If
    End Function

    Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        Try
            Dim Fahrgestellnummer As String = ""
            If Not e.Item.ItemIndex = -1 Then
                If m_change Is Nothing Then
                    m_change = CType(Session("objChange"), fin_18)
                End If

                'equnr immer als erste spalte bitte! es muss eine ReadOnly Spalte sein da sonst im editItem-Modus kein text vorhanden ist.JJ2008.04.21
                Fahrgestellnummer = m_change.Result.Select("EQUNR='" & e.Item.Cells(0).Text.ToString & "'")(0).Item("Fahrgestellnummer")

            End If
            Select Case e.Item.ItemType

                Case ListItemType.Item
                    Dim myDeleteButton As LinkButton
                    myDeleteButton = CType(e.Item.FindControl("lbErledigt"), LinkButton)
                    myDeleteButton.Attributes.Add("onclick", "return confirm('Dokument mit Fahrgestellnummer \n" & Fahrgestellnummer & "\n als erledigt kennzeichnen?');")
                Case ListItemType.AlternatingItem
                    Dim myDeleteButton As LinkButton
                    myDeleteButton = CType(e.Item.FindControl("lbErledigt"), LinkButton)
                    myDeleteButton.Attributes.Add("onclick", "return confirm('Dokument mit Fahrgestellnummer \n" & Fahrgestellnummer & "\n als erledigt kennzeichnen?');")
                Case ListItemType.EditItem
                    Dim myDeleteButton As LinkButton
                    myDeleteButton = CType(e.Item.FindControl("lbErledigt"), LinkButton)
                    myDeleteButton.Attributes.Add("onclick", "return confirm('Dokument mit Fahrgestellnummer \n" & Fahrgestellnummer & "\n als erledigt kennzeichnen?');")
            End Select
        Catch
            lblError.Text = "Vorsicht: Bestätigungsabfragen konnten nicht generiert werden"
        End Try
    End Sub

    Private Sub lb_Auswahl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Auswahl.Click
        Response.Redirect("Change50.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        DataGrid1.EditItemIndex = -1
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        DataGrid1.EditItemIndex = -1
        FillGrid(e.NewPageIndex)
    End Sub
End Class
' ************************************************
' $History: Change50_1.aspx.vb $
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 29.06.09   Time: 10:16
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA: 2946
' 
' *****************  Version 12  *****************
' User: Dittbernerc  Date: 19.06.09   Time: 14:29
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 - .Net Connector Umstellung
' 
' Bapis:
' Z_M_Abweich_abrufgrund
' Z_M_Save_ZABWVERGRUND
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:50
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 20.08.08   Time: 10:39
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2188 fertig
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 10.06.08   Time: 10:10
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 10.06.08   Time: 10:09
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 6.06.08    Time: 10:31
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 1954
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 26.05.08   Time: 12:42
' Updated in $/CKAG/Components/ComCommon/Finance
' AKF Änderung, Abweichungen jetzt mit gelb
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 28.04.08   Time: 17:15
' Updated in $/CKAG/Components/ComCommon/Finance
' Migrationsnachtrag
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 23.04.08   Time: 9:44
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1850
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 21.04.08   Time: 16:05
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' BUGFIX AKF Abweichungen
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 15.04.08   Time: 15:20
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Bugfix Abweichungen
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 27.02.08   Time: 15:33
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1733
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 11.02.08   Time: 9:16
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' History hinzugefügt
' 
' ************************************************
