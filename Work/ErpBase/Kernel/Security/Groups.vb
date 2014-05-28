
Namespace Kernel.Security
    <Serializable()> Public Class Groups
        REM § Collection von Gruppen-Objekten

        Inherits CollectionBase

#Region " Membervariables "
        Private m_strConnectionstring As String
        Private m_user As User
#End Region

#Region " Constructor "
        Public Sub New(ByRef _User As User, ByVal strConnectionString As String)
            Me.New(_User, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByRef _User As User, ByVal cn As SqlClient.SqlConnection)
            Dim blnCloseOnEnd As Boolean = False
            m_strConnectionstring = cn.ConnectionString
            m_user = _User
            If cn.State = ConnectionState.Closed Then
                cn.Open()
                blnCloseOnEnd = True
            End If
            GetGroups(cn)
            If blnCloseOnEnd Then
                cn.Close()
            End If
        End Sub
        Public Sub New(ByRef _User As User, ByVal customerId As Integer, ByVal cn As SqlClient.SqlConnection)
            Dim blnCloseOnEnd As Boolean = False
            m_strConnectionstring = cn.ConnectionString
            m_user = _User
            If cn.State = ConnectionState.Closed Then
                cn.Open()
                blnCloseOnEnd = True
            End If
            GetGroups(cn, customerId)
            If blnCloseOnEnd Then
                cn.Close()
            End If
        End Sub
#End Region

#Region " Properties "
        Default Public Property Item(ByVal index As Integer) As Group
            Get
                Return CType(List(index), Group)
            End Get
            Set(ByVal Value As Group)
                List(index) = Value
            End Set
        End Property

        Public ReadOnly Property ItemByID(ByVal intGroupId As Integer) As Group
            Get
                Dim gr As Group = DirectCast(List(InnerList.BinarySearch(intGroupId, New GroupComparer())), Group)
                Return gr
            End Get
        End Property

        Public ReadOnly Property IsInGroups(ByVal intGroupId As Integer) As Boolean
            Get
                If InnerList.BinarySearch(intGroupId, New GroupComparer()) < 0 Then
                    Return False
                End If
                Return True
            End Get
        End Property

        Public ReadOnly Property HasGroups() As Boolean
            Get
                If Me.Count > 0 Then Return True
                Return False
            End Get
        End Property
#End Region

#Region " Functions "
        Public Function Add(ByVal value As Group) As Integer
            Return List.Add(value)
        End Function

        Public Function IndexOf(ByVal value As Group) As Integer
            Return List.IndexOf(value)
        End Function

        Public Sub Insert(ByVal index As Integer, ByVal value As Group)
            List.Insert(index, value)
        End Sub

        Public Sub Remove(ByVal value As Group)
            List.Remove(value)
        End Sub

        Public Function Contains(ByVal value As Group) As Boolean
            Return List.Contains(value)
        End Function

        Protected Overrides Sub OnValidate(ByVal value As [Object])
            If Not value.GetType() Is Type.GetType("CKG.Base.Kernel.Security.Group") Then
                Throw New ArgumentException("value must be of type Group.", "value")
            End If
        End Sub

        'Public Function IsAdminOf(ByVal intGroupID As Integer) As Boolean
        '    Dim gr As Group = DirectCast(List(InnerList.BinarySearch(intGroupID, New GroupComparer())), Group)
        '    If gr.IsGroupAdmin Then Return True
        '    Return False

        '    'Dim gr As Group
        '    'For Each gr In Me
        '    '    If gr.GroupId = intGroupID AndAlso gr.IsGroupAdmin Then
        '    '        Return True
        '    '    End If
        '    'Next
        '    'Return False
        'End Function

        Private Sub GetGroups(ByVal cn As SqlClient.SqlConnection, Optional ByVal customerId As Integer = 0)
            Dim gr As Group
            Dim cmdGetCustomer As SqlClient.SqlCommand

            If customerId > 0 Then
                cmdGetCustomer = New SqlClient.SqlCommand("SELECT * FROM WebGroup WHERE customerId=@customerId", cn)
                cmdGetCustomer.Parameters.AddWithValue("@customerId", customerId)
            Else
                cmdGetCustomer = New SqlClient.SqlCommand("SELECT * FROM vwWebGroupWebMember WHERE UserID=@UserID", cn)
                cmdGetCustomer.Parameters.AddWithValue("@UserID", m_user.UserID)
            End If
            
            Dim dr As SqlClient.SqlDataReader = cmdGetCustomer.ExecuteReader

            Try
                While dr.Read
                    Dim strStartMethod As String = ""
                    If Not dr("StartMethod") Is System.DBNull.Value Then
                        strStartMethod = CStr(dr("StartMethod"))
                    End If
                    Dim strMessage As String = ""
                    If Not dr("Message") Is System.DBNull.Value Then
                        strMessage = CStr(dr("Message"))
                    End If
                    gr = New Group(CInt(dr("GroupID")), dr("GroupName").ToString, CInt(dr("CustomerID")), dr("DocuPath").ToString, CInt(dr("Authorizationright")), CBool(dr("IsCustomerGroup")), False, strStartMethod, strMessage, CInt(dr("MaxReadMessageCount")))
                    Me.Add(gr)
                End While
                dr.Close()
            Catch ex As Exception
                dr.Close()
                Throw ex
            End Try
        End Sub

        Public Sub Delete(ByVal intGroupID As Integer)
            Dim gr As Group = DirectCast(List(InnerList.BinarySearch(intGroupID, New GroupComparer())), Group)
            gr.MarkDeleted()
        End Sub

        Public Sub Save(ByVal strChangeUser As String)
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            Try
                cn.Open()
                Save(cn, strChangeUser)
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern der Gruppenzuordnungen!", ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub

        Public Sub Save(ByVal cn As SqlClient.SqlConnection, ByVal strChangeUser As String)
            Try
                Dim cmdDelete As New SqlClient.SqlCommand("DELETE " & _
                                                          "FROM WebMember " & _
                                                          "WHERE UserID=@UserID " & _
                                                            "AND GroupID=@GroupID", cn)

                Dim cmdInsert As New SqlClient.SqlCommand("INSERT INTO WebMember(UserID, " & _
                                                                    "GroupID,LastChangedBy) " & _
                                                             "VALUES(@UserID, " & _
                                                                    "@GroupID,@LastChangedBy)", cn)

                Dim gr As Group

                'Loeschen
                Dim al As New ArrayList()
                For Each gr In Me
                    If gr.IsDeleted Then
                        al.Add(gr)
                        With cmdDelete.Parameters
                            .Clear()
                            .AddWithValue("@UserId", m_user.UserID)
                            .AddWithValue("@GroupId", gr.GroupId)
                        End With
                        cmdDelete.ExecuteNonQuery()
                    End If
                Next
                For Each gr In al
                    Remove(gr)
                Next

                'Speichern
                For Each gr In Me
                    If gr.IsNew Then
                        With cmdInsert.Parameters
                            .Clear()
                            .AddWithValue("@UserId", m_user.UserID)
                            .AddWithValue("@GroupId", gr.GroupId)
                            .AddWithValue("@LastChangedBy", strChangeUser)
                        End With
                        cmdInsert.ExecuteNonQuery()
                    End If
                Next
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern der Gruppenzuordnungen!", ex)
            End Try
        End Sub
#End Region

#Region " SubClasses "
        Private Class GroupComparer
            Implements IComparer

            Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
                Dim intX As Integer
                Dim intY As Integer
                If TypeOf x Is Group Then
                    intX = DirectCast(x, Group).GroupId
                Else
                    intX = DirectCast(x, Integer)
                End If
                If TypeOf y Is Group Then
                    intY = DirectCast(y, Group).GroupId
                Else
                    intY = DirectCast(y, Integer)
                End If

                If intX = intY Then
                    Return 0
                ElseIf intX > intY Then
                    Return 1
                Else
                    Return -1
                End If
            End Function
        End Class

        'Private Class AdminComparer
        '    Implements IComparer

        '    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        '        Dim blnX As Boolean
        '        Dim blnY As Boolean
        '        If TypeOf x Is Group Then
        '            blnX = DirectCast(x, Group).IsGroupAdmin
        '        Else
        '            blnX = DirectCast(x, Boolean)
        '        End If
        '        If TypeOf y Is Group Then
        '            blnY = DirectCast(y, Group).IsGroupAdmin
        '        Else
        '            blnY = DirectCast(y, Boolean)
        '        End If

        '        If blnX = blnY Then
        '            Return 0
        '        ElseIf blnX > blnY Then
        '            Return 1
        '        Else
        '            Return -1
        '        End If
        '    End Function
        'End Class
#End Region

    End Class
End Namespace

' ************************************************
' $History: Groups.vb $
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 25.08.10   Time: 17:57
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 24.08.10   Time: 11:52
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 3  *****************
' User: Hartmannu    Date: 10.09.08   Time: 17:28
' Updated in $/CKAG/Base/Kernel/Security
' ITA 2027 - Anzeige der erweiterten Benutzerhistorie
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Base/Kernel/Security
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 6.12.07    Time: 12:47
' Updated in $/CKG/Base/Base/Kernel/Security
' ITA:1440
' 
' *****************  Version 3  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Kernel/Security
' 
' ************************************************