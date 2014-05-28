Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Partial Public Class Change18_2
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private objSuche As FFE_Search
    Private objFDDBank As FFE_BankBase
    Private objFDDBank2 As FFE_Bank_2
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        Try
            lnkKreditlimit.NavigateUrl = "Change18.aspx?AppID=" & Session("AppID").ToString
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            m_App = New Base.Kernel.Security.App(m_User)
            Dim DistrictCount As Integer = Session("DistrictCount")
            If DistrictCount > 0 Then
                lnkKreditlimit.Text = "Distriktsuche"
                lnkKreditlimit.Visible = True
            Else
                If m_User.Organization.AllOrganizations Then
                    lnkKreditlimit.Visible = True
                Else
                    lnkKreditlimit.Visible = False
                End If
            End If


            If (Request.QueryString("ShowAll") = "True") And (Not IsPostBack) Then
                Session("SelectedDealer") = Nothing
            End If

            If (Session("objSuche") Is Nothing) OrElse _
                CType(Session("objSuche"), FFE_Search).HaendlerFiliale.Length = 0 Then
                'Keine Filialinformation vorhanden = Abbruch
                Response.Redirect("Change18.aspx?AppID=" & Session("AppID").ToString)
            Else
                'Filialinformation vorhanden
                objSuche = CType(Session("objSuche"), FFE_Search)

                If Session("SelectedDealer") Is Nothing Then
                    'Noch kein Händler ausgewählt
                    ' => Auswahltabelle
                    trKopfdaten.Visible = False
                    trVorgangsArt.Visible = False
                    'trPageSize.Visible = True
                    trDataGrid1.Visible = True
                    cmdSave.Visible = False

                    If (Not IsPostBack) Or (Session("objFDDBank2") Is Nothing) Then
                        'Daten aus SAP laden
                        objFDDBank2 = New FFE_Bank_2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                        objFDDBank2.AppID = Session("AppID").ToString
                        objFDDBank2.CreditControlArea = "ZDAD"
                        objFDDBank2.Filiale = objSuche.HaendlerFiliale
                        objFDDBank2.Customer = m_User.KUNNR
                        objFDDBank2.Show_Retail()
                    Else
                        objFDDBank2 = CType(Session("objFDDBank2"), FFE_Bank_2)
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

                        Select Case objFDDBank2.Status
                            Case 0
                                FillGrid(objFDDBank2, 0)
                                Session("objFDDBank2") = objFDDBank2
                            Case -9999
                                'trPageSize.Visible = False
                                trDataGrid1.Visible = False
                                lblError.Text = "Fehler bei der Ermittlung der gesperrten Aufträge.<br>(" & objFDDBank2.Message & ")"
                            Case Else
                                'trPageSize.Visible = False
                                trDataGrid1.Visible = False
                                lblError.Text = objFDDBank2.Message
                        End Select
                    End If
                Else
                    trKopfdaten.Visible = True
                    trVorgangsArt.Visible = True        'Hier wird die Zeile der Vorgangsarten eingeblendet!
                    'trPageSize.Visible = False
                    trDataGrid1.Visible = False
                    cmdSave.Visible = True

                    objFDDBank = CType(Session("objFDDBank"), FFE_BankBase)
                    If objFDDBank.Status = 0 Then
                        Kopfdaten1.Kontingente = objFDDBank.Kontingente
                        cmdSave.Enabled = True
                        Session("objFDDBank") = objFDDBank
                    Else
                        ddlPageSize.Visible = False
                        lblError.Text = objFDDBank.Message
                    End If

                    If Not IsPostBack Then
                        If objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, Session("SelectedDealer").ToString) Then
                            Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
                            Dim strTemp As String = objSuche.NAME
                            If objSuche.NAME_2.Length > 0 Then
                                strTemp &= "<br>" & objSuche.NAME_2
                            End If
                            Kopfdaten1.HaendlerName = strTemp
                            Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET
                        Else
                            lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
                        End If
                    End If

                    objFDDBank2 = CType(Session("objFDDBank2"), FFE_Bank_2)
                    Dim vwTemp As DataView = objFDDBank2.AuftragsUebersicht.DefaultView
                    vwTemp.RowFilter = "Händlernummer = '" & Session("SelectedDealer").ToString & "'"
                    Dim item As ListItem
                    Dim str As String
                    str = String.Empty

                    If (objFDDBank2.ZeigeAlle) Then                 '?????
                        item = New ListItem()
                        item.Value = "0"
                        item.Text = ""
                        ddlKontingentart.Items.Add(item)
                    End If
                    If (objFDDBank2.ZeigeStandard) Then
                        item = New ListItem()
                        item.Value = "1"
                        item.Text = CStr(CInt(vwTemp(0)(2)) + CInt(vwTemp(0)(3))) & " Vorgänge 'Standard'"
                        str &= "<u><b>" & CStr(CInt(vwTemp(0)(2)) + CInt(vwTemp(0)(3))) & "</b></u> Vorgänge 'Standard'<br>"
                        ddlKontingentart.Items.Add(item)
                    End If
                    If (objFDDBank2.ZeigeFlottengeschaeft) Then
                        item = New ListItem()
                        item.Value = "2"
                        item.Text = CStr(vwTemp(0)(4)) & " Vorgänge 'erweitertes Zahlungsziel' (Delayed Payment)"
                        str &= "<u><b>" & CStr(vwTemp(0)(4)) & "</b></u> Vorgänge 'Erweitertes Zahlungsziel' (Delayed Payment)<br>"
                        ddlKontingentart.Items.Add(item)
                    End If
                    If (objFDDBank2.ZeigeHEZ) Then
                        item = New ListItem()
                        item.Value = "3"
                        item.Text = CStr(vwTemp(0)(5)) & " Vorgänge 'HEZ'" 'HEZ
                        str &= "<u><b>" & CStr(vwTemp(0)(5)) & "</b></u> Vorgänge 'HEZ'<br><br>"
                        ddlKontingentart.Items.Add(item)
                    End If
                    lblAnzeige.Text = str
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim ddlVal As String

        objFDDBank2.ZeigeAlle = False
        objFDDBank2.ZeigeFlottengeschaeft = False
        objFDDBank2.ZeigeHEZ = False
        objFDDBank2.ZeigeStandard = False

        ddlVal = ddlKontingentart.SelectedItem.Value
        If (ddlVal = "0") Then 'Alle
            '??? Was soll hier passieren?
        End If
        If (ddlVal = "1") Then 'Standard
            objFDDBank2.ZeigeStandard = True 'rbStandard.Checked  'HEZ
        End If
        If (ddlVal = "2") Then 'Flottengeschäft
            objFDDBank2.ZeigeFlottengeschaeft = True
        End If
        If (ddlVal = "3") Then 'HEZ
            objFDDBank2.ZeigeHEZ = True
        End If

        objFDDBank2.Haendler = Session("SelectedDealer").ToString

        Session("objFDDBank2") = objFDDBank2
        Response.Redirect("Change02Edit.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub FillGrid(ByVal objBank As FFE_Bank_2, ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If objBank.Status = 0 Then
            If objBank.AuftragsUebersicht.Rows.Count = 0 Then
                trDataGrid1.Visible = False
                'trPageSize.Visible = False
                lblNoData.Visible = True
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                ShowScript.Visible = False
            Else
                trDataGrid1.Visible = True
                'trPageSize.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As New DataView()
                tmpDataView = objBank.AuftragsUebersicht.DefaultView

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

                lblNoData.Text = "Es wurden " & objBank.AuftraegeAlle.Rows.Count.ToString & " gesperrte Aufträge gefunden."
                lblNoData.Visible = True

                If DataGrid1.PageCount > 1 Then
                    DataGrid1.PagerStyle.CssClass = "PagerStyle"
                    DataGrid1.DataBind()
                    DataGrid1.PagerStyle.Visible = True
                Else
                    DataGrid1.PagerStyle.Visible = False
                End If

            End If
        Else
            lblError.Text = objBank.Message
            lblNoData.Visible = True
            ShowScript.Visible = False
        End If
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        If Not e.Item.Cells(1).Text.Length = 0 Then
            Dim strRedirectURL As String = "Change18_2.aspx?AppID=" & Session("AppID").ToString
            Session("SelectedDealer") = e.Item.Cells(1).Text
            objFDDBank = New FFE_BankBase(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", True)
            objFDDBank.CreditControlArea = "ZDAD"
            objFDDBank.Customer = Session("SelectedDealer").ToString
            objFDDBank.Show()
            Session("objFDDBank") = objFDDBank

            'Nur Retail enthält Daten. Direkt nach Change18Edit.aspx gehen!
            objFDDBank2.Haendler = Session("SelectedDealer").ToString
            Session("objFDDBank2") = objFDDBank2
            strRedirectURL = "Change18Edit.aspx?AppID=" & Session("AppID").ToString
            Response.Redirect(strRedirectURL)
        End If
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(objFDDBank2, e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(objFDDBank2, DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(objFDDBank2, 0)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class
' ************************************************
' $History: Change18_2.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA:1865
' 
' ************************************************
