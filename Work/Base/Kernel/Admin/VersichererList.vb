Namespace Kernel.Admin
    Public Class VersichererList
        REM § Liste von Versicherern zur Verwendung auf Admin-Seiten

        Inherits DataTable

#Region " Membervariables "
        Private m_cn As SqlClient.SqlConnection
        Private m_strSAPNr As String
        Private m_strName1 As String
        Private m_blnTableFilled As Boolean = False
#End Region

#Region " Constructor "
        'Public Sub New(ByVal strSAPNrFilter As String, ByVal strName1Filter As String, ByVal strConnectionString As String)
        '    Me.New(strSAPNrFilter, strName1Filter, New SqlClient.SqlConnection(strConnectionString))
        'End Sub
        'Public Sub New(ByVal strSAPNrFilter As String, ByVal strName1Filter As String, ByVal cn As SqlClient.SqlConnection)
        '    If strSAPNrFilter = String.Empty Then strSAPNrFilter = "%"
        '    If cn.State = ConnectionState.Closed Then
        '        cn.Open()
        '    End If
        '    Dim daApp As New SqlClient.SqlDataAdapter("SELECT * " & _
        '                                              "FROM Versicherer " & _
        '                                                "WHERE [SAP-Nr] LIKE @SAPNr " & _
        '                                                  "AND Name1 LIKE @Name1", cn)
        '    With daApp.SelectCommand.Parameters
        '        .Add("@SAPNr", Replace(strSAPNrFilter, "*", "%"))
        '        .Add("@Name1", Replace(strName1Filter, "*", "%"))
        '    End With
        '    daApp.Fill(Me)
        '    m_blnTableFilled = True
        'End Sub


        '§§§ JVE 11.07.2006 ###########################################################################################

        Public Sub New(ByVal strSAPNrFilter As String, ByVal strName1Filter As String, ByVal cn As SqlClient.SqlConnection, ByVal intCustomerID As Integer)
            Try
                If strSAPNrFilter = String.Empty Then strSAPNrFilter = "%"
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                Dim daApp As New SqlClient.SqlDataAdapter("SELECT * FROM Versicherer WHERE [SAP-Nr] LIKE @SAPNr AND Name1 LIKE @Name1 AND CustomerID = @CustomerID", cn)
                With daApp.SelectCommand.Parameters
                    .AddWithValue("@SAPNr", Replace(strSAPNrFilter, "*", "%"))
                    .AddWithValue("@Name1", Replace(strName1Filter, "*", "%"))
                    .AddWithValue("@CustomerID", intCustomerID)
                End With
                daApp.Fill(Me)
                m_blnTableFilled = True
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub

        Public Sub New(ByVal cn As SqlClient.SqlConnection, ByVal objUser As Base.Kernel.Security.User, ByVal intCustomerID As Integer)
            Try
                '§§§ JVE 19.10.2006: Nur für DAD-Admin.
                If (objUser.HighestAdminLevel = Base.Kernel.Security.AdminLevel.Master) Then
                    If cn.State = ConnectionState.Closed Then
                        cn.Open()
                    End If
                    Dim daApp As New SqlClient.SqlDataAdapter("SELECT * FROM Versicherer WHERE CustomerID = @CustomerID ", cn)
                    With daApp.SelectCommand.Parameters
                        .AddWithValue("@CustomerID", intCustomerID)
                    End With
                    daApp.Fill(Me)
                    m_blnTableFilled = True
                End If
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub

        '##############################################################################################################

        'Public Sub New(ByVal strConnectionString As String)    '§§§ JVE 19.10.2006: obsolet!!!
        '    Me.New(New SqlClient.SqlConnection(strConnectionString))
        'End Sub

        Public Sub New(ByVal cn As SqlClient.SqlConnection, ByVal intCustomerID As Integer)
            Try
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                Dim daApp As New SqlClient.SqlDataAdapter("SELECT * " & _
                                                          "FROM Versicherer WHERE CustomerID = " & intCustomerID, cn)
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
' $History: VersichererList.vb $
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
' *****************  Version 8  *****************
' User: Rudolpho     Date: 6.12.07    Time: 12:47
' Updated in $/CKG/Base/Base/Kernel/Admin
' ITA:1440
' 
' *****************  Version 7  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Kernel/Admin
' 
' ************************************************