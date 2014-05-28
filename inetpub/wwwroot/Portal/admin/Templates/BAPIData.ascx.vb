Public MustInherit Class BAPIData
    Inherits System.Web.UI.UserControl
    Protected WithEvents DG1 As System.Web.UI.WebControls.DataGrid

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

    Private Sub SalesList_DataBind(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.DataBinding
        Dim dgi As DataGridItem = CType(Me.BindingContainer, DataGridItem)
        Dim ds As DataSet = CType(dgi.DataItem, DataSet)
        DG1.DataSource = ds
        DG1.DataMember = "BAPI"
        DG1.DataBind()
    End Sub
End Class

' ************************************************
' $History: BAPIData.ascx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 14:47
' Created in $/CKAG/admin/Templates
' 
' *****************  Version 1  *****************
' User: Uha          Date: 3.07.07    Time: 16:00
' Created in $/CKG/Admin/AdminWeb/Templates
' SAPMonitoring mit hierarchischem Grid
' 
' ************************************************
