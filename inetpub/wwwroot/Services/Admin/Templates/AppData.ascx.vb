
Public MustInherit Class AppData

    Inherits System.Web.UI.UserControl
    <CLSCompliant(False)> Protected WithEvents HG1 As DBauer.Web.UI.WebControls.HierarGrid

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TitleList_DataBind(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.DataBinding
        Dim dgi As DataGridItem = CType(Me.BindingContainer, DataGridItem)
        Dim ds As DataSet = CType(dgi.DataItem, DataSet)
        HG1.DataSource = ds
        HG1.DataMember = "AppDaten"
        HG1.DataBind()
        Dim item As DataGridItem
        Dim i As Int32 = 0
        For Each item In HG1.Items
            HG1.RowExpanded(i) = True
            i += 1
        Next
    End Sub

    Private Sub HG1_TemplateSelection(ByVal sender As Object, ByVal e As DBauer.Web.UI.WebControls.HierarGridTemplateSelectionEventArgs) Handles HG1.TemplateSelection
        Select Case (e.Row.Table.TableName)
            Case "SAPDaten"
                e.TemplateFilename = "Templates\\SAPData.ascx"
            Case Else
                Throw New NotImplementedException("Unexpected child row in TemplateSelection event")
        End Select
    End Sub

End Class

' ************************************************
' $History: AppData.ascx.vb $
' 
' *****************  Version 1  *****************
' User: Dittbernerc  Date: 16.11.09   Time: 15:43
' Created in $/CKAG2/Admin/Templates
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 15:47
' Updated in $/CKAG/admin/Templates
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 14:47
' Created in $/CKAG/admin/Templates
' 
' *****************  Version 3  *****************
' User: Uha          Date: 13.03.07   Time: 11:03
' Updated in $/CKG/Admin/AdminWeb/Templates
' 
' ************************************************
