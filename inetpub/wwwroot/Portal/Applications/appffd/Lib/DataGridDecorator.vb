Option Explicit On 
Option Strict On

Imports System
Imports System.Web.UI.WebControls
Imports System.Collections
Imports System.Web.UI

Public Class ASPNetDataGridDecorator

    Dim m_arrHeaderCells As ArrayList = Nothing
    Dim m_htblRowspanIndex As Hashtable = New Hashtable()
    Dim WithEvents m_dgDatagridToDecorate As DataGrid = Nothing

    Public Sub New()

    End Sub

    Private Sub New(ByVal DatagridToDecorate As DataGrid, ByVal HeaderCells As ArrayList)
        m_dgDatagridToDecorate = DatagridToDecorate
        AddMergeHeader(HeaderCells)
    End Sub

    Public Sub AddMergeHeader(ByVal arrHeaderCells As ArrayList)
        m_arrHeaderCells = arrHeaderCells
    End Sub

    Private Sub NewRenderMethod(ByVal writer As HtmlTextWriter, ByVal ctl As Control)
        Dim iCurrIndex As Int32 = 0
        Dim i As Int32

        For i = 0 To m_arrHeaderCells.Count - 1
            Dim item As TableCell = CType(m_arrHeaderCells(i), TableCell)
            If item.ColumnSpan > 1 Then
                iCurrIndex += item.ColumnSpan - 1
            End If
            If item.RowSpan > 1 Then
                m_htblRowspanIndex.Add(iCurrIndex + i, iCurrIndex + i)
            End If
            item.RenderControl(writer)
        Next i
        writer.WriteEndTag("TR")
        m_dgDatagridToDecorate.HeaderStyle.AddAttributesToRender(writer)
        writer.RenderBeginTag("TR")

        For i = 0 To ctl.Controls.Count - 1
            If m_htblRowspanIndex(i) Is Nothing Then
                ctl.Controls(i).RenderControl(writer)
            End If
        Next i
    End Sub

    Private Sub DatagridToDecorate_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles m_dgDatagridToDecorate.ItemCreated
        Dim lit As ListItemType = e.Item.ItemType

        If ListItemType.Header = lit Then
            e.Item.SetRenderMethodDelegate(AddressOf NewRenderMethod)
        End If
    End Sub

    Public Property DataGridToDecorate() As DataGrid
        Get
            Return m_dgDatagridToDecorate
        End Get
        Set(ByVal Value As DataGrid)
            m_dgDatagridToDecorate = Value
        End Set
    End Property

End Class

' ************************************************
' $History: DataGridDecorator.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 1  *****************
' User: Uha          Date: 21.05.07   Time: 14:22
' Created in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Änderungen im Vergleich zur Startapplikation zum Stand 21.05.2007
' 
' ************************************************
