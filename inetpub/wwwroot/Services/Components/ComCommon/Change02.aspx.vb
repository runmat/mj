Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Partial Public Class Change02
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation
    Private objHandler As Adressaenderung
    Private blnGridFilled As Boolean

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Session("ShowLink") = "False"
        m_User = GetUser(Me)

        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        GridNavigation1.setGridElment(GridView1)

        blnGridFilled = False

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        m_App = New Base.Kernel.Security.App(m_User)

        If Not Session("objHandler") Is Nothing Then
            objHandler = CType(Session("objHandler"), Adressaenderung)
            'Session("objHandler") = Nothing
        End If

        If Not IsPostBack Then
            HideInput()
            blnGridFilled = True
            'Session("lnkExcel") = ""
            PanelAdressAenderung.Visible = False

        End If

    End Sub

    Private Sub AusfuehrenSchreibenSAP()
        objHandler.Kunnr_I = Me.lblKunnr_IShow.Text
        objHandler.Name1 = Me.txtName1.Text
        objHandler.Name2 = Me.txtName2.Text
        objHandler.Land1 = Me.txtLand1.Text
        objHandler.Pstlz = Me.txtPstlz.Text
        objHandler.Ort01 = Me.txtOrt01.Text
        objHandler.Stras = Me.txtStras.Text
        objHandler.Telf1 = Me.txtTelf1.Text
        objHandler.Telfx = Me.txtTelfx.Text
        objHandler.Smtp_Addr = Me.txtSmtp_Addr.Text
        objHandler.Katr9 = ddlKatr9.SelectedItem.Value

        If objHandler.Name1.Length = 0 Or objHandler.Stras.Length = 0 Or objHandler.Land1.Length = 0 Or objHandler.Pstlz.Length = 0 Or objHandler.Ort01.Length = 0 Then
            Throw New Exception("Adresse unvollständig eingegeben.")
        End If

        'Aufruf Change
        objHandler.ChangeNew(Me.Page)

        'Neuer Excellink
        If objHandler.Status = 0 Then
            'Neue Suche
            DoSubmit()
        Else
            lblError.Text = "Fehler: " & objHandler.Message
        End If

        'Darstellung wiederherstellen
        HideAdressAenderung()
    End Sub

    Private Sub AuswahlAdressAenderung(ByVal strKundennummer As String)
        ShowAdressAenderung()

        Me.lblKunnr_IShow.Text = strKundennummer.Trim(" "c)

        Dim rows() As DataRow
        If lblKunnr_IShow.Text.Length = 0 Then
            lblError.Text = "Kein Schlüssel zur Adressänderung übergeben."
            Me.lb_Aendern2.Enabled = False
            Exit Sub
        Else
            rows = objHandler.Result.Select("Kunnr = '" & lblKunnr_IShow.Text & "'")
            If CStr(rows(0)("Katr9")).Length > 0 Then
                ddlKatr9.Items.FindByValue(CStr(rows(0)("Katr9"))).Selected = True
            End If
            Me.txtLand1.Text = CStr(rows(0)("Land1"))
            Me.txtName1.Text = CStr(rows(0)("Name1"))
            Me.txtName2.Text = CStr(rows(0)("Name2"))
            Me.txtOrt01.Text = CStr(rows(0)("Ort01"))
            Me.txtPstlz.Text = CStr(rows(0)("Pstlz"))
            Me.txtSmtp_Addr.Text = CStr(rows(0)("Smtp_Addr"))
            Me.txtStras.Text = CStr(rows(0)("Stras"))
            Me.txtTelf1.Text = CStr(rows(0)("Telf1"))
            Me.txtTelfx.Text = CStr(rows(0)("Telfx"))
            Me.lb_Aendern2.Enabled = True
        End If

        blnGridFilled = True
    End Sub

    Private Sub ShowAdressAenderung()
        PanelAdressAenderung.Visible = True
        Result.Visible = False
        
    End Sub

    Private Sub HideAdressAenderung()
        PanelAdressAenderung.Visible = False
        Result.Visible = True
        
    End Sub

    Private Sub ShowInput()
        GridView1.Visible = True
    End Sub

    Private Sub HideInput()
        GridView1.Visible = False
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        blnGridFilled = True

        Dim tmpDataView As New DataView()
        tmpDataView = objHandler.Result.DefaultView

        If tmpDataView.Count = 0 Then
            GridView1.Visible = False
        Else
            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            DivPlaceholder.Visible = False
            Result.Visible = True
            btnConfirm.Visible = False

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

            If GridView1.PageCount > 1 Then
                GridView1.PagerStyle.CssClass = "PagerStyle"
                GridView1.DataBind()

            End If

            For Each Row As GridViewRow In GridView1.Rows
                If objHandler.Kunnr_I = Row.Cells(1).Text Then
                    Row.CssClass = "GridTableHighlight"

                    SetLiteral1ForDatagrid2(Row.Cells(1).Text)
                End If
            Next
        End If
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub

    Private Sub gridview1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(0, e.SortExpression)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

        If Not objHandler Is Nothing Then
            Session.Add("objHandler", objHandler)
        End If

        SetEndASPXAccess(Me)

    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        DoSubmit()

    End Sub

    Private Sub DoSubmit()
        HideInput()
        'Session("lnkExcel") = ""
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        objHandler = New Adressaenderung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)

        lblError.Text = ""

        objHandler.ShowNew(Me.Page)

        If Not objHandler.Status = 0 Then
            lblError.Text = "Fehler: " & objHandler.Message
            blnGridFilled = True
        Else
            ShowInput()
            If objHandler.Result.Rows.Count = 0 Then
                lblError.Text = "Keine Daten vorhanden."
            Else
                Session("objHandler") = objHandler
                'Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()

                'Try
                'objHandler.CreateExcelFromFieldTranslation(Session(Replace(Request.Url.LocalPath, "/Services", "..")), objHandler.Result)
                '    excelFactory.CreateDocumentAndWriteToFilesystem(ConfigurationManager.AppSettings("ExcelPath") & strFileName, objHandler.ResultExcel, Me)
                '    Session("lnkExcel") = "/Services/Temp/Excel/" & strFileName
                'Catch ex2 As Exception
                '    Dim strError As String = ex2.ToString
                'End Try
                logApp.UpdateEntry("APP", Session("AppID").ToString, "Adressänderung")

                FillGrid(0)
                FillDdl()
            End If
        End If

    End Sub

    Private Sub FillDdl()
        ddlKatr9.DataSource = objHandler.PruefIntervalle
        ddlKatr9.DataValueField = "Katr9"
        ddlKatr9.DataTextField = "Vtext"
        ddlKatr9.DataBind()
    End Sub


    Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand


        Select Case e.CommandName
            Case "Aendern"
                Dim index As Integer = CType(e.CommandArgument, Integer)
                Dim GridRow As GridViewRow = GridView1.Rows(index)

                Dim lbl As Label

                lbl = CType(GridRow.Cells(1).FindControl("lblKunnr_IShow2"), Label)

                AuswahlAdressAenderung(lbl.Text)
            Case Else
                blnGridFilled = True
        End Select
    End Sub

    Private Sub SetLiteral1ForDatagrid2(ByVal strTarget As String)
        Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
        Literal1.Text &= "						  <!-- //" & vbCrLf
        Literal1.Text &= "						    window.document.location.href = ""#" & strTarget & """;" & vbCrLf
        Literal1.Text &= "						  //-->" & vbCrLf
        Literal1.Text &= "						</script>" & vbCrLf
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        DoBack()
    End Sub

    Private Sub DoBack()

        HideAdressAenderung()
        blnGridFilled = True

    End Sub


    Private Sub lb_Aendern2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Aendern2.Click

        AusfuehrenSchreibenSAP()

    End Sub

    Private Sub lb_Back2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Back2.Click
        DoBack()
    End Sub

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConfirm.Click
        DoSubmit()
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        'Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
        Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)

    End Sub

    Protected Sub lbCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreateExcel.Click


        objHandler = CType(Session("objHandler"), Adressaenderung)
        objHandler.CreateExcelFromFieldTranslation(Session(Replace(Request.Url.LocalPath, "/Services", "..")), objHandler.Result)
        Try

            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, objHandler.ResultExcel, Me)

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
End Class

' ************************************************
' $History: Change02.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 15.03.10   Time: 10:18
' Updated in $/CKAG2/Services/Components/ComCommon
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 28.09.09   Time: 16:20
' Updated in $/CKAG2/Services/Components/ComCommon
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 24.09.09   Time: 9:07
' Updated in $/CKAG2/Services/Components/ComCommon
' ITA: 3112
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 21.09.09   Time: 15:44
' Created in $/CKAG2/Services/Components/ComCommon
' ITA: 3112
' 