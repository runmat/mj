Public MustInherit Class BlkFahrzeuge
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


        Dim clsBlock As Sixt_B15
        clsBlock = CType(Session("clsBlock"), Sixt_B15)

        If clsBlock.Aktion = 4 Then
            Me.DG1.Columns(11).Visible = False
        End If


    End Sub

End Class

' ************************************************
' $History: BlkFahrzeuge.ascx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 10:16
' Updated in $/CKAG/Applications/appsixt/Templates
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:38
' Created in $/CKAG/Applications/appsixt/Templates
' 
' *****************  Version 2  *****************
' User: Uha          Date: 22.05.07   Time: 11:18
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Templates
' Nacharbeiten + Bereinigungen
' 
' ************************************************
