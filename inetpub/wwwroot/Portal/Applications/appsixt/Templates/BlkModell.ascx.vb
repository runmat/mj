Public MustInherit Class BlkModell
    Inherits System.Web.UI.UserControl
    <CLSCompliant(False)> Protected WithEvents HG1 As DBauer.Web.UI.WebControls.HierarGrid

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
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
        Dim clsBlock As Sixt_B15
        clsBlock = CType(Session("objBlocken"), Sixt_B15)

    End Sub

    Private Sub HG1_TemplateSelection(ByVal sender As Object, ByVal e As DBauer.Web.UI.WebControls.HierarGridTemplateSelectionEventArgs) Handles HG1.TemplateSelection
        Select Case (e.Row.Table.TableName)
            Case "Fahrzeuge"
                e.TemplateFilename = "..\Templates\BlkFahrzeuge.ascx"
            Case Else
                Throw New NotImplementedException("Unexpected child row in TemplateSelection event")
        End Select
    End Sub


End Class

' ************************************************
' $History: BlkModell.ascx.vb $
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
