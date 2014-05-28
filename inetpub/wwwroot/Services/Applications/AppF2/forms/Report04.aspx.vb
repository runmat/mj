Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Partial Public Class Report04
    Inherits System.Web.UI.Page


#Region "Declarations"
    Private m_App As App
    Private m_User As User
    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation
    Private objVersendungen As VersendeteZB2
    Private objHaendler As Haendler
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        m_App = New App(m_User)
        GridNavigation1.setGridElment(gvVersendungen)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            m_App = New App(m_User)


            lblError.Text = ""
            If Not IsPostBack Then

                FillAbrufgreunde(String.Empty)
            Else
                If Not Session("App_objVersendungen") Is Nothing Then
                    objVersendungen = CType(Session("App_objVersendungen"), VersendeteZB2)
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try



    End Sub

    Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click
        DoSubmit()
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(gvVersendungen)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        gvVersendungen.PageIndex = PageIndex
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub
    Private Sub gvVersendungen_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvVersendungen.Sorting
        FillGrid(gvVersendungen.PageIndex, e.SortExpression)
    End Sub

    Protected Sub lnkCreatePDF1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreatePDF1.Click

        Dim reportExcel As DataTable = Session("App_ResultTable")

        Try
            For Each row As DataRow In reportExcel.Rows
                row("Versandadresse") = Replace(row("Versandadresse").ToString, "<br />", ", ")
            Next
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateExcelDocumentAsPDFAndSendAsResponse(strFileName, reportExcel, Me)

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        Dim reportExcel As DataTable = Session("App_ResultTable")

        Try
            For Each row As DataRow In reportExcel.Rows
                row("Versandadresse") = Replace(row("Versandadresse").ToString, "<br />", ", ")
            Next
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, reportExcel, Me)

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub NewSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles NewSearch.Click
        lbCreate.Visible = Not lbCreate.Visible
        tab1.Visible = Not tab1.Visible
        Queryfooter.Visible = Not Queryfooter.Visible
        Result.Visible = Not Result.Visible
    End Sub


    Protected Sub btnEmpty_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEmpty.Click
        DoSubmit()
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Protected Sub rb_Alle_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_Alle.CheckedChanged
        FillAbrufgreunde("VersandAdressArt <> '6' ")
        trWiedereingang.Visible = False
        cbxWiedereingang.Checked = False
    End Sub

    Protected Sub rb_Temp_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_Temp.CheckedChanged
        FillAbrufgreunde("AbrufTyp='temp' AND VersandAdressArt <> '6' ")
        trWiedereingang.Visible = False
        cbxWiedereingang.Checked = False
    End Sub

    Protected Sub rb_End_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_End.CheckedChanged
        FillAbrufgreunde("AbrufTyp='endg'")
        trWiedereingang.Visible = True
    End Sub
#End Region

#Region "Methods"
    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = objVersendungen.Result.DefaultView

        tmpDataView.RowFilter = ""

        If tmpDataView.Count = 0 Then
            Result.Visible = False
        Else
            Result.Visible = True
            lbCreate.Visible = False
            tab1.Visible = False
            Queryfooter.Visible = False
            Dim strTempSort As String = ""
            Dim strDirection As String = ""
            Dim intTempPageIndex As Int32 = intPageIndex

            If strSort.Trim(" "c).Length > 0 Then
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
            gvVersendungen.PageIndex = intTempPageIndex
            gvVersendungen.DataSource = tmpDataView
            gvVersendungen.DataBind()
            Session("App_objVersendungen") = objVersendungen
            objVersendungen = Nothing



        End If

    End Sub


    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Try
            'If checkDate() Then

            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            objVersendungen = New VersendeteZB2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
            If Not rb_Temp.Checked Then
                If txtDatumVon.Text.Length = 0 AndAlso txtDatumBis.Text.Length = 0 Then
                    lblError.Text = "Bitte geben Sie einen Zeitraum an!"
                    Exit Sub
                End If

                If txtDatumVon.Text.Length = 0 Then
                    lblError.Text = "Bitte geben Sie das ""Versanddatum von"" an!"
                    Exit Sub
                End If

                If txtDatumBis.Text.Length = 0 Then
                    lblError.Text = "Bitte geben Sie das ""Versanddatum bis"" an!"
                    Exit Sub
                End If

            End If


            If IsDate(txtDatumVon.Text) Then objVersendungen.DatumVon = txtDatumVon.Text
            If IsDate(txtDatumBis.Text) Then objVersendungen.DatumBis = txtDatumBis.Text
            If ddlAbrufgrund.SelectedIndex <> 0 Then objVersendungen.Abrufgrund = ddlAbrufgrund.SelectedValue
            If rb_Alle.Checked = True Then
                objVersendungen.Versandart = "A"
            ElseIf rb_Temp.Checked = True Then
                objVersendungen.Versandart = "1"
            ElseIf rb_End.Checked = True Then
                objVersendungen.Versandart = "2"
            End If

            If txt_Empf.Text.Length > 0 Then
                objVersendungen.Haendlername = txt_Empf.Text
            End If
            If txt_Haendlernr.Text.Length > 0 Then
                objVersendungen.Haendlernr = txt_Haendlernr.Text
            End If


            If cbxWiedereingang.Checked = True Then
                objVersendungen.Wiedereingang = True
            End If

            objVersendungen.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)

            Session("App_ResultTable") = objVersendungen.Result

            If Not objVersendungen.Status = 0 Then
                lblError.Text = objVersendungen.Message
            ElseIf objVersendungen.Versendungen.Rows.Count = 0 Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            Else
                FillGrid(0)
            End If
            'End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub FillAbrufgreunde(ByVal sFilter As String)


        If Session("AppHaendler") Is Nothing Then
            objHaendler = New Haendler(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", m_User.Reference, m_User.KUNNR)
            Session.Add("AppHaendler", objHaendler)
        Else
            objHaendler = Session("AppHaendler")
        End If
        If Not objHaendler Is Nothing Then
            objHaendler.VersandAdressArt = "AND VersandAdressArt <> 6"
            Dim vwAbrufgruende As DataView = objHaendler.Abrufgruende.DefaultView

            If sFilter.Trim.Length > 0 Then
                vwAbrufgruende.RowFilter = sFilter
            Else
                vwAbrufgruende.RowFilter = ""
            End If
            vwAbrufgruende.Sort = "WebBezeichnung"

            If vwAbrufgruende.Table.Rows.Count > 0 Then


                ddlAbrufgrund.DataSource = vwAbrufgruende
                ddlAbrufgrund.DataTextField = "WebBezeichnung"
                ddlAbrufgrund.DataValueField = "SapWert"
                ddlAbrufgrund.DataBind()
                ' Doppelte Einträge entfernen
                Dim proofItem As New ListItemCollection
                For iCount As Integer = ddlAbrufgrund.Items.Count - 1 To 0 Step -1

                    Dim proofStr As String = ""

                    proofStr = ddlAbrufgrund.Items(iCount).Value
                    Dim i As Integer = 0
                    For Each abrufItem2 As ListItem In ddlAbrufgrund.Items

                        If proofStr = abrufItem2.Value Then
                            i = i + 1
                        End If
                    Next
                    If i > 1 Then
                        ddlAbrufgrund.Items.RemoveAt(iCount)
                    End If
                Next
            Else
                lblError.Text = "Es konnten keine Abrufgründe ermittelt werden!"
            End If
        Else
            lblError.Text = "Es konnten keine Abrufgründe ermittelt werden!"
        End If



    End Sub

#End Region


End Class
