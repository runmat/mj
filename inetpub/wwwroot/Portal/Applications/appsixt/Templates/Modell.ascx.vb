Public MustInherit Class Modell
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
        HG1.DataMember = "Modelle"
        HG1.DataBind()
        Dim item As DataGridItem
        Dim i As Int32 = 0
        For Each item In HG1.Items
            HG1.RowExpanded(i) = True
            i += 1
        Next
        Dim objPDIs As SIXT_PDI
        objPDIs = CType(Session("objPDIs"), SIXT_PDI)
        Select Case objPDIs.Task
            Case "Zulassen"
                HG1.Columns(13).Visible = False         'Bemerkung
                HG1.Columns(14).Visible = False         'Bemerkung Datum 
                HG1.Columns(15).Visible = True          'Datum Erstzulassung
            Case "Verschieben"
                'HG1.Columns(14).Visible = True         'Bemerkung Datum
                HG1.Columns(16).Visible = True          'Ziel-PDI
            Case "Sperren"
                'HG1.Columns(14).Visible = True         'Bemerkung Datum
            Case "Entsperren"
                'HG1.Columns(14).Visible = True         'Bemerkung Datum
        End Select
    End Sub

    Private Sub HG1_TemplateSelection(ByVal sender As Object, ByVal e As DBauer.Web.UI.WebControls.HierarGridTemplateSelectionEventArgs) Handles HG1.TemplateSelection
        Select Case (e.Row.Table.TableName)
            Case "Fahrzeuge"
                e.TemplateFilename = "..\Templates\Fahrzeug.ascx"
            Case Else
                Throw New NotImplementedException("Unexpected child row in TemplateSelection event")
        End Select
    End Sub
End Class

' ************************************************
' $History: Modell.ascx.vb $
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
' *****************  Version 6  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Templates
' 
' ************************************************
