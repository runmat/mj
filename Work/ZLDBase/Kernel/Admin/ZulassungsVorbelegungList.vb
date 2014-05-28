Namespace Kernel.Admin
    Public Class ZulassungsVorbelegungList
        REM § Liste von ZulassungsVorbelegungen zur Verwendung auf Admin-Seiten

        Inherits DataTable

#Region " Membervariables "
        Private m_cn As SqlClient.SqlConnection
        Private m_blnTableFilled As Boolean = False
        Private m_tblHersteller As New DataTable()
#End Region

#Region " Constructor "
        Public Sub New(ByVal strConnectionString As String, ByVal CustomerID As Integer)
            Me.New(New SqlClient.SqlConnection(strConnectionString), CustomerID)
        End Sub

        Public Sub New(ByVal cn As SqlClient.SqlConnection, ByVal CustomerID As Integer)
            Try
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                Dim daApp As New SqlClient.SqlDataAdapter("SELECT * " & _
                                                          "FROM wrkZulassungsVorbelegungen WHERE CustomerID = " & CustomerID, cn)
                daApp.Fill(Me)

                Dim daApp2 As New SqlClient.SqlDataAdapter("SELECT DISTINCT Hersteller AS HstValue, " & _
                                                                           "Hersteller AS HstText " & _
                                                           "FROM wrkZulassungsVorbelegungen WHERE CustomerID = " & CustomerID, cn)
                daApp2.Fill(m_tblHersteller)
                Dim dr As DataRow = m_tblHersteller.NewRow
                dr("HstValue") = "%"
                dr("HstText") = " - alle - "
                m_tblHersteller.Rows.Add(dr)
                m_blnTableFilled = True
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
#End Region

#Region " Properties "
        Public ReadOnly Property HerstellerList() As DataTable
            Get
                Return m_tblHersteller
            End Get
        End Property
#End Region

#Region " Functions "

#End Region
    End Class
End Namespace

' ************************************************
' $History: ZulassungsVorbelegungList.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Kernel/Admin
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 6.12.07    Time: 12:47
' Updated in $/CKG/Base/Base/Kernel/Admin
' ITA:1440
' 
' *****************  Version 4  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Kernel/Admin
' 
' ************************************************