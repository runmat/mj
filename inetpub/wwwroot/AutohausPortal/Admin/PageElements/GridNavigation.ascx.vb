Partial Public Class GridNavigation
    Inherits System.Web.UI.UserControl

    Protected WithEvents mGridView As GridView
    Protected WithEvents mDataGrid As DataGrid
    Public Event PageSizeChanged As PageSize_Changed
    Public Event PagerChanged As Pager_Changed
    Delegate Sub PageSize_Changed()
    Delegate Sub Pager_Changed(ByVal PageIndex As Integer)
    Private PageCount As Integer
    Private PagerStart As Integer
    Private m_PagerSize As Int32

    Public Property PagerSize As Integer
        Get
            Return m_PagerSize
        End Get
        Set(ByVal Value As Integer)
            m_PagerSize = Value
        End Set


    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            fillPageSizeDropDown()
        End If

    End Sub

    Public Sub setGridElment(ByRef GridElement As GridView)
        mGridView = GridElement
    End Sub

    Public Sub setGridElment(ByRef GridElement As DataGrid)
        mDataGrid = GridElement
    End Sub

    Public Sub setGridTitle(ByVal Title As String)
        lbltitle.Text = Title
        If Title.Trim.Length > 0 Then
            lbltitle.Visible = True
        End If

    End Sub

    Public Sub ProofPager()
        If Not mGridView Is Nothing Then

            If mGridView.PageCount = 1 Then
                lbtnNext.Visible = False
                lbtnPrevious.Visible = False
                Repeater1.Visible = False
                Exit Sub
            Else
                lbtnNext.Visible = True
                lbtnPrevious.Visible = True
                Repeater1.Visible = True
            End If

            If mGridView.PageCount - 1 = mGridView.PageIndex Then
                lbtnNext.Visible = False
                lbtnPrevious.Visible = True
            Else
                lbtnNext.Visible = True
            End If

            If mGridView.PageIndex > 0 Then
                lbtnPrevious.Visible = True
            ElseIf mGridView.PageIndex = 0 Then
                lbtnPrevious.Visible = False
                lbtnNext.Visible = True
            End If
        End If

        If Not mDataGrid Is Nothing Then

            If mDataGrid.PageCount = 1 Then
                lbtnNext.Visible = False
                lbtnPrevious.Visible = False
                Repeater1.Visible = False
                Exit Sub
            Else
                lbtnNext.Visible = True
                lbtnPrevious.Visible = True
                Repeater1.Visible = True
            End If


            If mDataGrid.PageCount - 1 = mDataGrid.CurrentPageIndex Then
                lbtnNext.Visible = False
                lbtnPrevious.Visible = True
            Else
                lbtnNext.Visible = True
            End If
            If mDataGrid.CurrentPageIndex > 0 Then
                lbtnPrevious.Visible = True

            ElseIf mDataGrid.CurrentPageIndex = 0 Then
                lbtnPrevious.Visible = False
                lbtnNext.Visible = True
            End If

        End If

    End Sub

    Private Sub fillPageSizeDropDown()
        ddlPageSize.Items.Add("10")
        ddlPageSize.Items.Add("20")
        ddlPageSize.Items.Add("50")
        ddlPageSize.Items.Add("100")
        ddlPageSize.Items.Add("200")
        ddlPageSize.SelectedIndex = m_PagerSize
    End Sub

    Private Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles mGridView.DataBound
        ' Retrieve the pager row.

        Dim i As Integer
        Dim LinkTable As New DataTable

        LinkTable.Columns.Add("Index", System.Type.GetType("System.String"))
        LinkTable.Columns.Add("Page", System.Type.GetType("System.String"))
        Dim RepRow As DataRow


        If mGridView.PageCount > 9 Then
            lbtnNext10.Visible = True
        Else
            lbtnNext10.Visible = False
        End If
        If ((mGridView.PageIndex).ToString.Length > 1) Then
            Dim sPage As String = Left((mGridView.PageIndex).ToString, 1)
            sPage += "0"
            PagerStart = CInt(sPage)
            PageCount = CInt(sPage) + 9
            lbtnPrevious10.Visible = True
        Else
            PagerStart = 0
            If mGridView.PageCount > 9 Then
                PageCount = 9
            Else
                PageCount = mGridView.PageCount - 1
            End If

            lbtnPrevious10.Visible = False
        End If


        For i = PagerStart To PageCount

            Dim pageNumber As Integer = i
            RepRow = LinkTable.NewRow
            RepRow("Index") = pageNumber
            RepRow("Page") = pageNumber + 1
            LinkTable.Rows.Add(RepRow)

        Next i


        If LinkTable.Rows.Count > 0 Then
            Repeater1.DataSource = LinkTable
            Repeater1.DataBind()

            For i = 0 To LinkTable.Rows.Count - 1
                If i + mGridView.PageIndex = mGridView.PageIndex Then
                    Dim lbtn As LinkButton
                    If mGridView.PageIndex = 0 Then
                        lbtn = CType(Repeater1.Controls(0).Controls(0).FindControl("lbtnPage"), LinkButton)
                    Else
                        lbtn = CType(Repeater1.Controls(mGridView.PageIndex - PagerStart).Controls(0).FindControl("lbtnPage"), LinkButton)
                    End If

                    lbtn.CssClass = "active"

                End If
            Next
        End If

        ProofPager()
    End Sub

    Private Sub DataGrid1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles mDataGrid.ItemDataBound

        Dim i As Integer
        Dim LinkTable As New DataTable

        LinkTable.Columns.Add("Index", System.Type.GetType("System.String"))
        LinkTable.Columns.Add("Page", System.Type.GetType("System.String"))
        Dim RepRow As DataRow


        If mDataGrid.PageCount > 9 Then
            lbtnNext10.Visible = True
        Else
            lbtnNext10.Visible = False
        End If
        If ((mDataGrid.CurrentPageIndex).ToString.Length > 1) Then
            Dim sPage As String = Left((mDataGrid.CurrentPageIndex).ToString, 1)
            sPage += "0"
            PagerStart = CInt(sPage)
            PageCount = CInt(sPage) + 9
            lbtnPrevious10.Visible = True
        Else
            PagerStart = 0
            If mDataGrid.PageCount > 9 Then
                PageCount = 9
            Else
                PageCount = mDataGrid.PageCount - 1
            End If

            lbtnPrevious10.Visible = False
        End If


        For i = PagerStart To PageCount

            Dim pageNumber As Integer = i
            RepRow = LinkTable.NewRow
            RepRow("Index") = pageNumber
            RepRow("Page") = pageNumber + 1
            LinkTable.Rows.Add(RepRow)

        Next i


        If LinkTable.Rows.Count > 0 Then
            Repeater1.DataSource = LinkTable
            Repeater1.DataBind()

            For i = 0 To LinkTable.Rows.Count - 1
                If i + mDataGrid.CurrentPageIndex = mDataGrid.CurrentPageIndex Then
                    Dim lbtn As LinkButton
                    If mDataGrid.CurrentPageIndex = 0 Then
                        lbtn = CType(Repeater1.Controls(0).Controls(0).FindControl("lbtnPage"), LinkButton)
                    Else
                        lbtn = CType(Repeater1.Controls(mDataGrid.CurrentPageIndex - PagerStart).Controls(0).FindControl("lbtnPage"), LinkButton)
                    End If

                    lbtn.CssClass = "active"

                End If
            Next
        End If

        ProofPager()

    End Sub


    Private Sub ddlPageSize_SelectedIndexChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        Select Case False
            Case mDataGrid Is Nothing
                mDataGrid.PageSize = ddlPageSize.SelectedValue
            Case mGridView Is Nothing
                mGridView.PageSize = ddlPageSize.SelectedValue
            Case Else
                Throw New Exception("GridNavigation: noch nicht initialisiert")
        End Select
        RaiseEvent PageSizeChanged()
    End Sub


    Public Sub lbtnPage_PageIndexChanging(ByVal sender As Object, ByVal e As System.EventArgs)
        RaiseEvent PagerChanged(CInt(sender.CommandArgument))
    End Sub

    Private Sub lbtnPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnPrevious.Click

        Dim tmpIntPageintex As Integer
        Select Case False
            Case mDataGrid Is Nothing
                tmpIntPageintex = mDataGrid.CurrentPageIndex - 1
            Case mGridView Is Nothing
                tmpIntPageintex = mGridView.PageIndex - 1
            Case Else
                Throw New Exception("GridNavigation: noch nicht initialisiert")
        End Select
        RaiseEvent PagerChanged(tmpIntPageintex)
    End Sub

    Private Sub lbtnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnNext.Click
        Dim tmpIntPageintex As Integer
        Select Case False
            Case mDataGrid Is Nothing
                tmpIntPageintex = mDataGrid.CurrentPageIndex + 1

            Case mGridView Is Nothing
                tmpIntPageintex = mGridView.PageIndex + 1
            Case Else
                Throw New Exception("GridNavigation: noch nicht initialisiert")
        End Select
        RaiseEvent PagerChanged(tmpIntPageintex)
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ProofPager()

        lblAnzahl.Visible = True
        Select Case False
            Case mDataGrid Is Nothing
                If TypeOf mDataGrid.DataSource Is DataView Then
                    lblAnzahl.Text = "Gesamtanzahl: " & CType(mDataGrid.DataSource, DataView).Count
                ElseIf TypeOf mDataGrid.DataSource Is DataTable Then
                    lblAnzahl.Text = "Gesamtanzahl: " & CType(mDataGrid.DataSource, DataTable).Rows.Count
                Else
                    lblAnzahl.Visible = False
                End If
            Case mGridView Is Nothing
                If TypeOf mGridView.DataSource Is DataView Then
                    lblAnzahl.Text = "Gesamtanzahl: " & CType(mGridView.DataSource, DataView).Count
                ElseIf TypeOf mGridView.DataSource Is DataTable Then
                    lblAnzahl.Text = "Gesamtanzahl: " & CType(mGridView.DataSource, DataTable).Rows.Count
                Else
                    lblAnzahl.Visible = False
                End If
            Case Else
                Throw New Exception("GridNavigation: noch nicht initialisiert")
        End Select

    End Sub

    Protected Sub lbtnNext10_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnNext10.Click
        Dim tmpIntPageintex As Integer
        Select Case False
            Case mDataGrid Is Nothing
                tmpIntPageintex = mDataGrid.CurrentPageIndex + 9
                If ((tmpIntPageintex + 1).ToString.Length > 1) Then
                    Dim sPage As String = Left((tmpIntPageintex + 1).ToString, 1)
                    sPage += "0"
                    PagerStart = CInt(sPage)
                End If
                tmpIntPageintex = PagerStart
            Case mGridView Is Nothing
                tmpIntPageintex = mGridView.PageIndex + 9
                If ((tmpIntPageintex + 1).ToString.Length > 1) Then
                    Dim sPage As String = Left((tmpIntPageintex + 1).ToString, 1)
                    sPage += "0"
                    PagerStart = CInt(sPage)
                End If
                tmpIntPageintex = PagerStart
            Case Else
                Throw New Exception("GridNavigation: noch nicht initialisiert")
        End Select
        RaiseEvent PagerChanged(tmpIntPageintex)
    End Sub

    Protected Sub lbtnPrevious10_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnPrevious10.Click
        Dim tmpIntPageintex As Integer
        Select Case False
            Case mDataGrid Is Nothing
                tmpIntPageintex = mDataGrid.CurrentPageIndex - 9
                If ((tmpIntPageintex).ToString.Length > 1) Then
                    Dim sPage As String = Left((tmpIntPageintex).ToString, 1)
                    sPage += "0"
                    PagerStart = CInt(sPage)
                End If
                tmpIntPageintex = PagerStart
            Case mGridView Is Nothing
                tmpIntPageintex = mGridView.PageIndex - 9
                If ((tmpIntPageintex).ToString.Length > 1) Then
                    Dim sPage As String = Left((tmpIntPageintex).ToString, 1)
                    sPage += "0"
                    PagerStart = CInt(sPage)
                End If
                tmpIntPageintex = PagerStart
            Case Else
                Throw New Exception("GridNavigation: noch nicht initialisiert")
        End Select
        RaiseEvent PagerChanged(tmpIntPageintex)
    End Sub
End Class

' ************************************************
' $History: GridNavigation.ascx.vb $
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 11.03.11   Time: 13:59
' Created in $/CKPortalZLD/PortalZLD/PageElements
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 24.08.10   Time: 16:21
' Updated in $/CKAG2/Services/PageElements
' 
' *****************  Version 12  *****************
' User: Dittbernerc  Date: 29.07.09   Time: 11:48
' Updated in $/CKAG2/Services/PageElements
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 5.05.09    Time: 17:16
' Updated in $/CKAG2/Services/PageElements
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 28.04.09   Time: 15:45
' Updated in $/CKAG2/Services/PageElements
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 23.04.09   Time: 13:36
' Updated in $/CKAG2/Services/PageElements
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 14.04.09   Time: 17:19
' Updated in $/CKAG2/Services/PageElements
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 14.04.09   Time: 16:52
' Updated in $/CKAG2/Services/PageElements
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 14.04.09   Time: 16:48
' Updated in $/CKAG2/Services/PageElements
' 
' ************************************************