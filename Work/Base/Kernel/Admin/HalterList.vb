Namespace Kernel.Admin
    Public Class HalterList
        REM § Liste von Haltern zur Verwendung auf Admin-Seiten

        Inherits DataTable

#Region " Membervariables "
        Private m_cn As SqlClient.SqlConnection
        Private m_strSAPNr As String
        Private m_strName1 As String
        Private m_blnTableFilled As Boolean = False
#End Region

#Region " Constructor "
        Public Sub New(ByVal strSAPNrFilter As String, ByVal strName1Filter As String, ByVal strConnectionString As String)
            Me.New(strSAPNrFilter, strName1Filter, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal strSAPNrFilter As String, ByVal strName1Filter As String, ByVal cn As SqlClient.SqlConnection)
            Try
                If strSAPNrFilter = String.Empty Then strSAPNrFilter = "%"
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                Dim daApp As New SqlClient.SqlDataAdapter("SELECT * " & _
                                                          "FROM Halter " & _
                                                            "WHERE [SAP-Nr] LIKE @SAPNr " & _
                                                              "AND Name1 LIKE @Name1", cn)
                With daApp.SelectCommand.Parameters
                    .AddWithValue("@SAPNr", Replace(strSAPNrFilter, "*", "%"))
                    .AddWithValue("@Name1", Replace(strName1Filter, "*", "%"))
                End With
                daApp.Fill(Me)
                m_blnTableFilled = True
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
        '############## §§§ JVE 14.07.2006  ############################
        Public Sub New(ByVal strSAPNrFilter As String, ByVal strName1Filter As String, ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection)
            Try
                If strSAPNrFilter = String.Empty Then strSAPNrFilter = "%"
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                Dim daApp As New SqlClient.SqlDataAdapter("SELECT * " & _
                                                          "FROM Halter " & _
                                                            "WHERE CustomerID = @Customer AND [SAP-Nr] LIKE @SAPNr " & _
                                                              "AND Name1 LIKE @Name1", cn)
                With daApp.SelectCommand.Parameters
                    .AddWithValue("@Customer", intCustomerID)
                    .AddWithValue("@SAPNr", Replace(strSAPNrFilter, "*", "%"))
                    .AddWithValue("@Name1", Replace(strName1Filter, "*", "%"))
                End With
                daApp.Fill(Me)
                m_blnTableFilled = True
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
        '###############################################################

        Public Sub New(ByVal strConnectionString As String, ByVal CustomerID As Integer)    '§§§ JVE 19.10.2006: CustomerID eingefügt
            Me.New(New SqlClient.SqlConnection(strConnectionString), CustomerID)
        End Sub
        Public Sub New(ByVal cn As SqlClient.SqlConnection, ByVal CustomerID As Integer)    '§§§ JVE 19.10.2006: CustomerID eingefügt
            Try
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                Dim daApp As New SqlClient.SqlDataAdapter("SELECT * " & _
                                                          "FROM Halter WHERE CustomerID = " & CustomerID, cn)
                daApp.Fill(Me)
                m_blnTableFilled = True
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
#End Region

#Region " Functions "

#End Region
    End Class
End Namespace

' ************************************************
' $History: HalterList.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Base/Kernel/Admin
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Kernel/Admin
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 6.12.07    Time: 12:47
' Updated in $/CKG/Base/Base/Kernel/Admin
' ITA:1440
' 
' *****************  Version 5  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Kernel/Admin
' 
' ************************************************