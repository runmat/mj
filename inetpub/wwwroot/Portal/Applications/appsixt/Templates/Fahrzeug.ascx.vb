Public MustInherit Class Fahrzeug
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
        Select Case objPDIs.Task
            Case "Zulassen"
                DG1.Columns(6).Visible = True       'Bemerkung Anzeige
                DG1.Columns(7).Visible = False      'Bemerkung Eingabe

                'For Each item In DG1.Items
                '    ctl = CType(item.Cells(7).FindControl("txtBemerkungDatum"), TextBox)
                '    ctl.Enabled = False
                '    ctl.Text = "MOIN!"
                'Next

                DG1.Columns(9).Visible = True       'Datum Erstzulassung Eingabe
            Case "Verschieben"
                'DG1.Columns(8).Visible = True       'Bemerkung Datum Eingabe
                DG1.Columns(10).Visible = True      'Ziel-PDI
            Case "Sperren"
                'DG1.Columns(8).Visible = True       'Bemerkung Datum Eingabe
            Case "Entsperren"
                'DG1.Columns(8).Visible = True       'Bemerkung Datum Eingabe
        End Select
        If objPDIs.ShowBelegnummer Then
            'DG1.Columns(DG1.Columns.Count - 1).Visible = True
            'DG1.Columns(DG1.Columns.Count - 2).Visible = False
            DG1.Columns(DG1.Columns.Count - 2).Visible = True
            DG1.Columns(DG1.Columns.Count - 3).Visible = False
        Else
            'DG1.Columns(DG1.Columns.Count - 1).Visible = False
            'DG1.Columns(DG1.Columns.Count - 2).Visible = True
            DG1.Columns(DG1.Columns.Count - 2).Visible = False
            DG1.Columns(DG1.Columns.Count - 3).Visible = True
        End If
    End Sub

End Class

' ************************************************
' $History: Fahrzeug.ascx.vb $
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
' *****************  Version 7  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Templates
' 
' ************************************************
