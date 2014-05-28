Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Partial Public Class Report29_22
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private objSuche As FFE_Search
    Private objFDDBank As FFE_BankBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        Dim showall As Boolean

        showall = False
        If Not (Request.QueryString("SHOWALL") Is Nothing) Then
            showall = True
        End If

        Try
            lnkKreditlimit.NavigateUrl = "Report29.aspx?AppID=" & Session("AppID").ToString
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            m_App = New Base.Kernel.Security.App(m_User)
            Dim DistrictCount As Integer = Session("DistrictCount")
            If DistrictCount > 0 Then
                lnkKreditlimit.Text = "Distriktsuche"
                lnkKreditlimit.Visible = True

            ElseIf m_User.Organization.AllOrganizations Then
                lnkKreditlimit.Visible = True
            Else
                lnkKreditlimit.Visible = False
            End If

            'If m_User.Organization.AllOrganizations Then
            '    lnkKreditlimit.Visible = True
            'Else
            '    lnkKreditlimit.Visible = False
            'End If
            lnkKreditlimit.Visible = False
            'If (Request.QueryString("ShowAll") = "True") And (Not IsPostBack) Then
            '    Session("SelectedDealer") = Nothing
            'End If

            If (Session("objSuche") Is Nothing) Then  'OrElse _
                '    CType(Session("objSuche"), Search).HaendlerFiliale.Length = 0 Then
                'Keine Filialinformation vorhanden = Abbruch
                Response.Redirect("Report29.aspx?AppID=" & Session("AppID").ToString)
            Else
                'Filialinformation vorhanden
                objSuche = CType(Session("objSuche"), FFE_Search)

                If Session("SelectedDealer") Is Nothing Then
                    'Noch kein Händler ausgewählt
                    ' => Auswahltabelle
                    trVorgangsArt.Visible = False
                    trPageSize.Visible = True
                    trDataGrid1.Visible = True
                    cmdSave.Visible = False

                    If (Not IsPostBack) Or (Session("objFDDBank4") Is Nothing) Then
                        'Daten aus SAP laden

                        objFDDBank = New FFE_BankBase(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                        objFDDBank.CreditControlArea = "ZDAD"
                        objFDDBank.Customer = "" 'Session("SelectedDealer").ToString

                        '§§§JVE </Händlerliste holen
                        'Dim objHaendler As FDD_Bank_2

                        'objHaendler = New FDD_Bank_2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

                        'Dim suche As New Base.Business.Search(m_User.App, m_User, CType(Session.SessionID, String), CType(Session("AppID"), String))
                        objSuche.LeseHaendlerSAP(CType(Session("AppID"), String), CType(Session.SessionID, String), , m_User.Organization.OrganizationReference)

                        If Not showall Then
                            objFDDBank.ShowAll(objSuche.Haendler)             'Nur eigene Haendler Zeigen
                        Else
                            objFDDBank.ShowAll(objSuche.Haendler, True)       'Alle Haendler zeigen
                        End If
                        'If Not showall Then
                        '    objFDDBank.ShowAll(objSuche.Filialen)           'Nur eigene Filialen Zeigen
                        'Else
                        '    objFDDBank.ShowAll(objSuche.Filialen, True)     'Alle Filialen zeigen
                        'End If
                        '/>





                        'objFDDBank4 = New FDD_Bank_4(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                        'objFDDBank4.AppID = Session("AppID").ToString
                        'objFDDBank4.CreditControlArea = "ZDAD"
                        'objFDDBank4.Filiale = objSuche.HaendlerFiliale
                        'objFDDBank4.Customer = m_User.KUNNR
                        'objFDDBank4.Show()
                        'Else
                        '    objFDDBank4 = CType(Session("objFDDBank4"), FDD_Bank_4)
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

                        Select Case objFDDBank.Status
                            Case 0
                                FillGrid(objFDDBank, 0)
                                Session("objFDDBank") = objFDDBank
                            Case -9999
                                trPageSize.Visible = False
                                trDataGrid1.Visible = False
                                lblError.Text = "Fehler bei der Ermittlung der gesperrten Aufträge.<br>(" & objFDDBank.Message & ")"
                            Case Else
                                trPageSize.Visible = False
                                trDataGrid1.Visible = False
                                lblError.Text = objFDDBank.Message
                        End Select
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    'Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
    '    objFDDBank4.Haendler = Session("SelectedDealer").ToString
    '    Session("objFDDBank4") = objFDDBank4
    '    Response.Redirect("Report29_3.aspx?AppID=" & Session("AppID").ToString)
    'End Sub

    Private Sub checkGrid(ByVal objBank As FFE_Bank_4)
        Dim i As Integer
        i = objBank.AuftragsUebersicht.Rows.Count()
    End Sub

    Private Sub FillGrid(ByVal objBank As FFE_BankBase, ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If objBank.Status = 0 Then
            If objBank.Kontingente.Rows.Count = 0 Then
                trDataGrid1.Visible = False
                trPageSize.Visible = False
                lblNoData.Visible = True
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                ShowScript.Visible = False
            Else
                'checkGrid(objBank)
                trDataGrid1.Visible = True
                trPageSize.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As New DataView()
                tmpDataView = objBank.Kontingente.DefaultView

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

                lblNoData.Text = "Es wurden " & objBank.Kontingente.Rows.Count.ToString & " Einträge gefunden."
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
        If Not e.Item.Cells(0).Text.Length = 0 Then
            Dim strRedirectURL As String = "Report29_23.aspx?AppID=" & Session("AppID").ToString
            Session("SelectedDealer") = e.Item.Cells(0).Text
            objFDDBank = New FFE_BankBase(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
            objFDDBank.CreditControlArea = "ZDAD"
            objFDDBank.Customer = Session("SelectedDealer").ToString
            objFDDBank.Show()
            Session("objFDDBank") = objFDDBank

            Response.Redirect(strRedirectURL)
        End If
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(objFDDBank, e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(objFDDBank, DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(objFDDBank, 0)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class
' ************************************************
' $History: Report29_22.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA:1865
' 
' ************************************************
