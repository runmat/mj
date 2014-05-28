Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Partial Public Class Report07_2
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As App
    Private m_User As User
    Private objSuche As AppF2.Search
    Private objAppReport07 As TempZuEndg
    Private objHaendler As AppF2BankBaseCredit
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        GridNavigation1.setGridElment(GridView1)
        m_App = New App(m_User)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        lnkKreditlimit.NavigateUrl = "Report07.aspx?AppID=" & Session("AppID").ToString

        If objAppReport07 Is Nothing Then
            objAppReport07 = CType(Session("AppReport07"), TempZuEndg)
        End If


        If Session("AppReport07") Is Nothing Then
            Response.Redirect("Report07.aspx?AppID=" & Session("AppID").ToString)
        End If

        If Session("objNewHaendlerSuche") Is Nothing Then
            Response.Redirect("Report07.aspx?AppID=" & Session("AppID").ToString)
        Else
            objSuche = CType(Session("objNewHaendlerSuche"), AppF2.Search)
            'kann passieren wenn verlinkung von Briefanforderung wenn Ohne Händlernummer gesucht wurde
            If objSuche.REFERENZ Is Nothing OrElse objSuche.REFERENZ Is String.Empty Then
                If Not objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, objAppReport07.Customer, Me) Then
                    'lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
                End If
            End If
        End If


        If Not objSuche.NAME Is Nothing Then
            Kopfdaten1.UserReferenz = m_User.Reference
            Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
            Dim strTemp As String = objSuche.NAME
            If objSuche.NAME_2.Length > 0 Then
                strTemp &= "<br>" & objSuche.NAME_2
            End If
            Kopfdaten1.HaendlerName = strTemp
            Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET
        Else
            Kopfdaten1.Visible = False
        End If





        Session("objNewHaendlerSuche") = objSuche



        If Not IsPostBack Then

            objHaendler = New AppF2BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
            objHaendler.Customer = objSuche.REFERENZ
            objHaendler.KUNNR = m_User.KUNNR
            objHaendler.CreditControlArea = "ZDAD"
            objHaendler.Show(Session("AppID").ToString, Session.SessionID, Me)
            Kopfdaten1.Kontingente = objHaendler.Kontingente
            FillGrid(0)
            Session("App_objHaendler") = objHaendler
        Else
            objHaendler = CType(Session("App_objHaendler"), AppF2BankBaseCredit)
            Kopfdaten1.Kontingente = objHaendler.Kontingente
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(GridView1)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub GridNavigation1_ddlPageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub

    Private Sub GridView1_PageIndexChanged(ByVal pageindex As Int32) Handles GridNavigation1.PagerChanged
        GridView1.EditIndex = -1
        FillGrid(pageindex)
    End Sub

    Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand

        If e.CommandName = "Update" Then

            Dim index As Integer = CType(e.CommandArgument, Integer)

            Dim txtFleet20 As TextBox = CType(GridView1.Rows(index).FindControl("txtFleet20"), TextBox)
            Dim txtFleet21 As TextBox = CType(GridView1.Rows(index).FindControl("txtFleet21"), TextBox)
            Dim lblEqunr As Label = CType(GridView1.Rows(index).FindControl("lblEqunr"), Label)
            If objAppReport07 Is Nothing Then
                objAppReport07 = CType(Session("AppHaendler07"), TempZuEndg)
            End If

            objAppReport07.EQUNR = lblEqunr.Text
            objAppReport07.Fleet20 = txtFleet20.Text
            objAppReport07.Fleet21 = txtFleet21.Text

            objAppReport07.UpdateFleet(Me)

            If Not objAppReport07.Status = 0 Then
                lblError.Text = objAppReport07.Message
                Exit Sub
            End If

            Dim sMsg As String = ""
            If objAppReport07.Message.Length > 0 Then
                lblError.Text = objAppReport07.Message
                Exit Sub
            End If


            If objAppReport07.Status = 0 Then


                Dim lblFleet20 As Label = CType(GridView1.Rows(index).FindControl("lblFleet20"), Label)
                Dim lblFleet21 As Label = CType(GridView1.Rows(index).FindControl("lblFleet21"), Label)
                'objAppReport07.EQUNR
                txtFleet20.Visible = False
                txtFleet21.Visible = False
                lblFleet21.Visible = True
                lblFleet20.Visible = True


                lblFleet20.Text = txtFleet20.Text
                lblFleet21.Text = txtFleet21.Text
                Dim dRow As DataRow
                dRow = objAppReport07.Fahrzeuge.Select("EQUNR='" & objAppReport07.EQUNR & "'")(0)
                dRow("Fleet20") = txtFleet20.Text
                dRow = objAppReport07.Fahrzeuge.Select("EQUNR='" & objAppReport07.EQUNR & "'")(0)
                dRow("Fleet21") = txtFleet21.Text
                objAppReport07.Fahrzeuge.AcceptChanges()
            End If

        End If


    End Sub

    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex)
    End Sub

    Private Sub GridView1_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridView1.RowEditing

        Dim txtFleet20 As TextBox = CType(GridView1.Rows(e.NewEditIndex).FindControl("txtFleet20"), TextBox)
        Dim txtFleet21 As TextBox = CType(GridView1.Rows(e.NewEditIndex).FindControl("txtFleet21"), TextBox)
        Dim lblFleet20 As Label = CType(GridView1.Rows(e.NewEditIndex).FindControl("lblFleet20"), Label)
        Dim lblFleet21 As Label = CType(GridView1.Rows(e.NewEditIndex).FindControl("lblFleet21"), Label)


        txtFleet20.Visible = Not txtFleet20.Visible
        txtFleet21.Visible = Not txtFleet21.Visible
        lblFleet21.Visible = Not lblFleet21.Visible
        lblFleet20.Visible = Not lblFleet20.Visible

    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click

        If objAppReport07 Is Nothing Then
            objAppReport07 = CType(Session("AppReport07"), TempZuEndg)
        End If
        Dim reportExcel As DataTable = objAppReport07.Fahrzeuge

        Try
            If reportExcel.Columns.Contains("EQUNR") Then reportExcel.Columns.Remove("EQUNR")
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, reportExcel, Me)

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Sub GridView1_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridView1.RowUpdating

    End Sub




#End Region
    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        If objAppReport07 Is Nothing Then
            objAppReport07 = CType(Session("AppReport07"), TempZuEndg)
        End If
        If Not objAppReport07.Fahrzeuge Is Nothing Then
            If objAppReport07.Status = 0 Then
                If objAppReport07.Fahrzeuge.Rows.Count = 0 Then
                    'trDataGrid1.Visible = False
                    'trPageSize.Visible = False
                    lblNoData.Visible = True
                    lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                    Result.Visible = False
                Else
                    Result.Visible = True
                    lblNoData.Visible = False

                    Dim tmpDataView As New DataView(objAppReport07.Fahrzeuge)

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


                    If objHaendler Is Nothing Then
                        objHaendler = CType(Session("App_objHaendler"), AppF2BankBaseCredit)
                    End If
                    objHaendler.Show(Session("AppID").ToString, Session.SessionID, Me)
                    Kopfdaten1.Kontingente = objHaendler.Kontingente

                End If
            Else
                lblError.Text = objAppReport07.Message
                lblNoData.Visible = True
                ShowScript.Visible = False
            End If
        Else
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            Result.Visible = False
            lblNoData.Visible = True
        End If
    End Sub


End Class
