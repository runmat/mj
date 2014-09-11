Public MustInherit Class DZFahrzeuge
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
        DG1.DataMember = "Fahrzeuge"
        DG1.DataBind()
        Dim objPDIs As SIXT_PDI
        objPDIs = CType(Session("objPDIs"), SIXT_PDI)
        If objPDIs.ShowBelegnummer Then
            DG1.Columns(DG1.Columns.Count - 1).Visible = True
            DG1.Columns(DG1.Columns.Count - 2).Visible = False
        Else
            DG1.Columns(DG1.Columns.Count - 1).Visible = False
            DG1.Columns(DG1.Columns.Count - 2).Visible = True
        End If
    End Sub
End Class

' ************************************************
' $History: DZFahrzeuge.ascx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:38
' Created in $/CKAG/Applications/appsixt/Templates
' 
' *****************  Version 3  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Templates
' 
' ************************************************
