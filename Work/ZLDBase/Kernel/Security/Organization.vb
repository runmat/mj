
Namespace Kernel.Security
    <Serializable()> Public Class Organization
        REM § Haltung und Verwaltung von Daten EINER administrativen Organization

#Region " Membervariables "
        Private m_strConnectionstring As String
        Private m_intOrganizationID As Integer
        Private m_strOrganizationName As String
        Private m_strOrganizationReference As String
        Private m_blnAllOrganizations As Boolean
        Private m_intCustomerID As Integer
        Private m_intUserId As Integer
        Private m_blnIsNew As Boolean = False
        Private m_blnDelete As Boolean = False
        Private m_blnOrganizationAdmin As Boolean
        Private m_OrganizationContact As Contact
        Private m_strLogoPath As String
        Private m_strCssPath As String
#End Region

#Region " Constructor "
        Public Sub New(ByVal intOrganizationID As Integer, ByVal intCustomerID As Integer)
            Me.new(intOrganizationID, "", intCustomerID, "", False, False, "", "", "", "", "", "", "", "", True)
        End Sub
        Public Sub New(ByVal intOrganizationID As Integer, _
                       ByVal strOrganizationName As String, _
                       ByVal intCustomerID As Integer, _
                       ByVal strOrganizationReference As String, _
                       ByVal blnAllOrganizations As Boolean, _
                       ByVal blnOrganizationAdmin As Boolean, _
                       ByVal strLogoPath As String, _
                       ByVal strCssPath As String, _
                       ByVal OName As String, _
                       ByVal OAddress As String, _
                       ByVal OMailDisplay As String, _
                       ByVal OMail As String, _
                       ByVal OWebDisplay As String, _
                       ByVal OWeb As String, _
                      ByVal blnNew As Boolean)
            m_blnIsNew = blnNew
            m_blnAllOrganizations = blnAllOrganizations
            m_intOrganizationID = intOrganizationID
            m_strOrganizationName = strOrganizationName
            m_strOrganizationReference = strOrganizationReference
            m_blnOrganizationAdmin = blnOrganizationAdmin
            m_intCustomerID = intCustomerID
            m_strLogoPath = strLogoPath
            m_strCssPath = strCssPath
            m_OrganizationContact = New Contact(OName, OAddress, OMailDisplay, OMail, OWebDisplay, OWeb, "", "", "")
        End Sub
        'Public Sub New(ByVal intUserID As Integer, ByVal strConnectionString As String)
        '    Me.New(intUserID, New SqlClient.SqlConnection(strConnectionString))
        'End Sub
        Public Sub New(ByVal intOrganizationID As Integer, ByVal strConnectionString As String, Optional ByVal intUserID As Int32 = -1)
            Me.New(intOrganizationID, New SqlClient.SqlConnection(strConnectionString), intUserID)
        End Sub
        Public Sub New(ByVal intOrganizationID As Integer, ByVal cn As SqlClient.SqlConnection, Optional ByVal intUserId As Int32 = -1)
            Dim blnCloseOnEnd As Boolean = False
            If intUserId = -1 Then
                m_intOrganizationID = intOrganizationID
                m_intUserId = -1
            Else
                m_intOrganizationID = -1
                m_intUserId = intUserId
            End If
            If cn.State = ConnectionState.Closed Then
                cn.Open()
                blnCloseOnEnd = True
            End If
            GetOrganization(cn)
            If blnCloseOnEnd Then
                cn.Close()
            End If
        End Sub
#End Region

#Region " Properties "
        'Public ReadOnly Property IsGroupAdmin() As Boolean
        '    Get
        '        If InnerList.BinarySearch(True, New AdminComparer()) < 0 Then
        '            Return False
        '        End If
        '        Return True

        '        'Dim gr As Group
        '        'For Each gr In Me
        '        '    If gr.IsGroupAdmin Then
        '        '        Return True
        '        '    End If
        '        'Next
        '        'Return False
        '    End Get
        'End Property

        Public ReadOnly Property OrganizationAdmin() As Boolean
            Get
                Return m_blnOrganizationAdmin
            End Get
        End Property

        Public ReadOnly Property AllOrganizations() As Boolean
            Get
                Return m_blnAllOrganizations
            End Get
        End Property

        Public ReadOnly Property OrganizationReference() As String
            Get
                Return m_strOrganizationReference
            End Get
        End Property

        Public ReadOnly Property OrganizationId() As Integer
            Get
                Return m_intOrganizationID
            End Get
        End Property

        Public ReadOnly Property OrganizationName() As String
            Get
                Return m_strOrganizationName
            End Get
        End Property

        'Public ReadOnly Property UserId() As Integer
        '    Get
        '        Return m_intUserId
        '    End Get
        'End Property

        Public ReadOnly Property CustomerId() As Integer
            Get
                Return m_intCustomerID
            End Get
        End Property

        Public ReadOnly Property IsNew() As Boolean
            Get
                Return m_blnIsNew
            End Get
        End Property

        Public ReadOnly Property IsDeleted() As Boolean
            Get
                Return m_blnDelete
            End Get
        End Property

        Public ReadOnly Property OrganizationContact() As Contact
            Get
                Return m_OrganizationContact
            End Get
        End Property

        Public ReadOnly Property LogoPath() As String
            Get
                Return m_strLogoPath
            End Get
        End Property

        Public ReadOnly Property CssPath() As String
            Get
                Return m_strCssPath
            End Get
        End Property
#End Region

#Region " Functions "
        Private Sub GetOrganization(ByVal cn As SqlClient.SqlConnection)
            Try
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                Dim cmdGetOrganization As New SqlClient.SqlCommand()
                cmdGetOrganization.Connection = cn
                If m_intOrganizationID = -1 Then
                    cmdGetOrganization.CommandText = "SELECT * FROM vwWebUserOrganizationMember WHERE UserID=@UserID"
                Else
                    cmdGetOrganization.CommandText = "SELECT * FROM vwOrganization WHERE OrganizationID=@OrganizationID"
                End If
                If m_intOrganizationID = -1 Then
                    cmdGetOrganization.Parameters.AddWithValue("@UserID", m_intUserId)
                Else
                    cmdGetOrganization.Parameters.AddWithValue("@OrganizationID", m_intOrganizationID)
                End If
                Dim dr As SqlClient.SqlDataReader
                dr = cmdGetOrganization.ExecuteReader
                While dr.Read
                    m_strOrganizationName = dr("OrganizationName").ToString
                    m_strOrganizationReference = dr("OrganizationReference").ToString
                    If m_intOrganizationID = -1 Then
                        m_blnOrganizationAdmin = CBool(dr("OrganizationAdmin"))
                    Else
                        m_blnOrganizationAdmin = False
                    End If
                    m_strLogoPath = dr("LogoPath").ToString
                    m_strCssPath = dr("CssPath").ToString
                    m_intCustomerID = CInt(dr("CustomerID"))
                    m_intOrganizationID = CInt(dr("OrganizationID"))
                    m_blnAllOrganizations = CBool(dr("AllOrganizations"))
                    m_OrganizationContact = New Contact(dr("OName").ToString, _
                                                 dr("OAddress").ToString, _
                                                 dr("OMailDisplay").ToString, _
                                                 dr("OMail").ToString, _
                                                 dr("OWebDisplay").ToString, _
                                                 dr("OWeb").ToString, "", "", "")
                End While
                dr.Close()
            Catch ex As Exception
                Throw ex
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub

        Public Sub ReAssignUserToOrganization(ByVal strLastChangedBy As String, ByVal strOldID As String, ByVal intUserID As Integer, ByVal intNewOrganizationID As Integer, ByVal blnOrganizationAdmin As Boolean, ByVal strConnectionString As String)
            ReAssignUserToOrganization(strLastChangedBy, strOldID, intUserID, intNewOrganizationID, blnOrganizationAdmin, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub ReAssignUserToOrganization(ByVal strLastChangedBy As String, ByVal strOldID As String, ByVal intUserID As Integer, ByVal intNewOrganizationID As Integer, ByVal blnOrganizationAdmin As Boolean, ByVal cn As SqlClient.SqlConnection)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim objOrganizationAdmin As New Object
            objOrganizationAdmin = Nothing
            CheckOldUserAssignment(intUserID, intNewOrganizationID, objOrganizationAdmin, cn)
            If objOrganizationAdmin Is Nothing Then
                'Noch nicht vorhanden
                'Alt löschen + Neu anlegen
                DeleteUserAssignment(intUserID, cn)
                m_intOrganizationID = intNewOrganizationID
                Dim cmdInsert As New SqlClient.SqlCommand("INSERT INTO OrganizationMember(UserID, " & _
                                                                    "OrganizationID, OrganizationAdmin, " & _
                                                                            "LastChangedBy) " & _
                                                             "VALUES(@UserID, " & _
                                                                    "@OrganizationID, @OrganizationAdmin, " & _
                                                                            "@LastChangedBy)", cn)
                With cmdInsert.Parameters
                    .Clear()
                    .AddWithValue("@UserId", intUserID)
                    .AddWithValue("@OrganizationID", m_intOrganizationID)
                    .AddWithValue("@OrganizationAdmin", blnOrganizationAdmin)
                    .AddWithValue("@LastChangedBy", strLastChangedBy)
                End With
                cmdInsert.ExecuteNonQuery()
            Else
                If blnOrganizationAdmin = CBool(objOrganizationAdmin) Then
                    'Nix geändert => Wech hier!
                    Exit Sub
                Else
                    'Update
                    Dim cmdUpdate As New SqlClient.SqlCommand("UPDATE OrganizationMember SET " & _
                                                                        "OrganizationAdmin = @OrganizationAdmin, " & _
                                                                        "LastChangedBy = @LastChangedBy " & _
                                                                 "WHERE " & _
                                                                        "(UserID = @UserID) AND " & _
                                                                        "(OrganizationID = @OrganizationID)", _
                                                                        cn)
                    With cmdUpdate.Parameters
                        .Clear()
                        .AddWithValue("@UserId", intUserID)
                        .AddWithValue("@OrganizationID", m_intOrganizationID)
                        .AddWithValue("@OrganizationAdmin", blnOrganizationAdmin)
                        .AddWithValue("@LastChangedBy", strLastChangedBy)
                    End With
                    cmdUpdate.ExecuteNonQuery()
                    'Keine weitere Historisierung in WebUserHistory
                    Exit Sub
                End If
            End If

            Dim cmdSelect2 As New SqlClient.SqlCommand("SELECT OrganizationName FROM Organization " & _
                                                                "WHERE OrganizationID=@OrganizationID", cn)
            With cmdSelect2.Parameters
                .Clear()
                .AddWithValue("@OrganizationID", m_intOrganizationID)
            End With
            Dim strOrganizationName As String = CStr(cmdSelect2.ExecuteScalar)

            Dim cmdSelect1 As New SqlClient.SqlCommand("SELECT UserHistoryID FROM WebUser " & _
                                                                "WHERE UserID=@UserID", cn)
            With cmdSelect1.Parameters
                .Clear()
                .AddWithValue("@UserID", intUserID)
            End With
            Dim intUserHistoryID As Integer = CInt(cmdSelect1.ExecuteScalar)

            Dim cmdUpdateHistory As SqlClient.SqlCommand
            If strOldID = "-1" Then
                cmdUpdateHistory = New SqlClient.SqlCommand("UPDATE WebUserHistory SET " & _
                                                                    "OrganizationName=@OrganizationName " & _
                                                                    "WHERE UserHistoryID=@UserHistoryID", cn)
                With cmdUpdateHistory.Parameters
                    .Clear()
                    .AddWithValue("@UserHistoryID", intUserHistoryID)
                    .AddWithValue("@OrganizationName", strOrganizationName)
                End With

                cmdUpdateHistory.ExecuteNonQuery()
            Else
                Dim histUser As New HistoryUser(intUserHistoryID, cn)
                If Not histUser.OrganizationName = strOrganizationName Then
                    cmdUpdateHistory = New SqlClient.SqlCommand("UPDATE WebUserHistory SET " & _
                                                                        "OrganizationName=@OrganizationName, " & _
                                                                        "LastChanged=@LastChanged, " & _
                                                                        "LastChange=@LastChange, " & _
                                                                        "LastChangedBy=@LastChangedBy " & _
                                                                        "WHERE UserHistoryID=@UserHistoryID", cn)
                    With cmdUpdateHistory.Parameters
                        .Clear()
                        .AddWithValue("@UserHistoryID", intUserHistoryID)
                        .AddWithValue("@OrganizationName", strOrganizationName)
                        .AddWithValue("@LastChanged", Now)
                        .AddWithValue("@LastChange", "Organisation geändert")
                        .AddWithValue("@LastChangedBy", strLastChangedBy)
                    End With

                    cmdUpdateHistory.ExecuteNonQuery()
                End If
            End If

            GetOrganization(cn)
        End Sub

        Public Sub CheckOldUserAssignment(ByVal intUserID As Integer, ByVal intOldOrganizationID As Integer, ByRef objOrgAdmin As Object, ByVal strConnectionString As String)
            CheckOldUserAssignment(intUserID, intOldOrganizationID, objOrgAdmin, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub CheckOldUserAssignment(ByVal intUserID As Integer, ByVal intOldOrganizationID As Integer, ByRef objOrgAdmin As Object, ByVal cn As SqlClient.SqlConnection)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            If m_intOrganizationID > 0 Then
                Dim cmdSelect As New SqlClient.SqlCommand("SELECT OrganizationAdmin " & _
                                                          "FROM OrganizationMember " & _
                                                          "WHERE UserID=@UserID " & _
                                                            "AND OrganizationID=@OrganizationID", cn)
                With cmdSelect
                    .Parameters.Clear()
                    .Parameters.AddWithValue("@UserId", intUserID)
                    .Parameters.AddWithValue("@OrganizationID", intOldOrganizationID)
                    objOrgAdmin = .ExecuteScalar
                End With
            End If
        End Sub

        Public Sub DeleteUserAssignment(ByVal intUserID As Integer, ByVal strConnectionString As String)
            DeleteUserAssignment(intUserID, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub DeleteUserAssignment(ByVal intUserID As Integer, ByVal cn As SqlClient.SqlConnection)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            If m_intOrganizationID > 0 Then
                Dim cmdDelete As New SqlClient.SqlCommand("DELETE " & _
                                                          "FROM OrganizationMember " & _
                                                          "WHERE UserID=@UserID " & _
                                                            "AND OrganizationID=@OrganizationID", cn)
                With cmdDelete.Parameters
                    .Clear()
                    .AddWithValue("@UserId", intUserID)
                    .AddWithValue("@OrganizationID", m_intOrganizationID)
                End With
                cmdDelete.ExecuteNonQuery()
            End If
        End Sub

        Public Sub Delete(ByVal strConnectionString As String)
            m_strConnectionstring = strConnectionString
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            Try
                cn.Open()
                Delete(cn)
            Catch ex As Exception
                Throw New Exception("Fehler beim Löschen der Organisation!", ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
        Public Sub Delete(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim strDeleteOrganization As String = "DELETE " & _
                                               "FROM Organization " & _
                                               "WHERE OrganizationID=@OrganizationID"

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn
                cmd.Parameters.AddWithValue("@OrganizationId", m_intOrganizationID)

                'Organization loeschen
                cmd.CommandText = strDeleteOrganization
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Throw New Exception("Fehler beim Löschen der Organisation!", ex)
            End Try
        End Sub

        Public Sub Save(ByVal strConnectionString As String)
            m_strConnectionstring = strConnectionString
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            Try
                cn.Open()
                Save(cn)
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern der Organisation!", ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
        Public Sub Save(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim strInsert As String = "INSERT INTO Organization(OrganizationName, " & _
                                                               "CustomerID, " & _
                                                               "AllOrganizations, " & _
                                                               "LogoPath, " & _
                                                               "CssPath, " & _
                                                               "OName, " & _
                                                               "OAddress, " & _
                                                               "OMailDisplay, " & _
                                                               "OMail, " & _
                                                               "OWebDisplay, " & _
                                                               "OWeb, " & _
                                                               "OrganizationReference) " & _
                                                 "VALUES(@OrganizationName, " & _
                                                        "@CustomerID, " & _
                                                        "@AllOrganizations, " & _
                                                        "@LogoPath, " & _
                                                        "@CssPath, " & _
                                                        "@OName, " & _
                                                        "@OAddress, " & _
                                                        "@OMailDisplay, " & _
                                                        "@OMail, " & _
                                                        "@OWebDisplay, " & _
                                                        "@OWeb, " & _
                                                        "@OrganizationReference); " & _
                                                 "SELECT SCOPE_IDENTITY()"

                Dim strUpdate As String = "UPDATE Organization " & _
                                          "SET OrganizationName=@OrganizationName, " & _
                                               "CustomerID=@CustomerID, " & _
                                               "AllOrganizations=@AllOrganizations, " & _
                                               "LogoPath=@LogoPath, " & _
                                               "CssPath=@CssPath, " & _
                                               "OName=@OName, " & _
                                               "OAddress=@OAddress, " & _
                                               "OMailDisplay=@OMailDisplay, " & _
                                               "OMail=@OMail, " & _
                                               "OWebDisplay=@OWebDisplay, " & _
                                               "OWeb=@OWeb, " & _
                                               "OrganizationReference=@OrganizationReference " & _
                                          "WHERE OrganizationID=@OrganizationID"

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn

                'Speichern
                If m_blnIsNew Then
                    cmd.CommandText = strInsert
                Else
                    cmd.CommandText = strUpdate
                    cmd.Parameters.AddWithValue("@OrganizationId", m_intOrganizationID)
                End If
                With cmd.Parameters
                    .AddWithValue("@OrganizationName", m_strOrganizationName)
                    .AddWithValue("@OrganizationReference", m_strOrganizationReference)
                    .AddWithValue("@CustomerID", m_intCustomerID)
                    .AddWithValue("@LogoPath", m_strLogoPath)
                    .AddWithValue("@CssPath", m_strCssPath)
                    .AddWithValue("@OName", m_OrganizationContact.Name)
                    .AddWithValue("@OAddress", m_OrganizationContact.Address)
                    .AddWithValue("@OMailDisplay", m_OrganizationContact.MailDisplay)
                    .AddWithValue("@OMail", m_OrganizationContact.Mail)
                    .AddWithValue("@OWebDisplay", m_OrganizationContact.WebDisplay)
                    .AddWithValue("@OWeb", m_OrganizationContact.Web)
                    .AddWithValue("@AllOrganizations", m_blnAllOrganizations)
                End With


                If m_blnIsNew Then
                    'Wenn Organization neu ist dann ID ermitteln, damit bei nachfolgendem Fehler und erneutem Speichern Datensatz nicht doppelt angelegt wird.
                    m_intOrganizationID = CInt(cmd.ExecuteScalar)
                Else
                    cmd.ExecuteNonQuery()
                End If
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern der Organisation!", ex)
            End Try
        End Sub

        Public Function HasUser(ByVal strConnectionString As String) As Boolean
            Dim cn As New SqlClient.SqlConnection(strConnectionString)
            Return HasUser(cn)
            cn.Close()
        End Function
        Public Function HasUser(ByVal cn As SqlClient.SqlConnection) As Boolean
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim cmd As New SqlClient.SqlCommand("SELECT COUNT(UserID) FROM OrganizationMember WHERE OrganizationID=@OrganizationID", cn)
            cmd.Parameters.AddWithValue("@OrganizationID", m_intOrganizationID)
            If CInt(cmd.ExecuteScalar) > 0 Then
                Return True
            End If
            Return False
        End Function

#End Region

    End Class
End Namespace

' ************************************************
' $History: Organization.vb $
' 
' *****************  Version 8  *****************
' User: Dittbernerc  Date: 9.05.11    Time: 13:39
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 8  *****************
' User: Dittbernerc  Date: 3.05.11    Time: 10:55
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 26.10.09   Time: 11:44
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 14.05.09   Time: 13:57
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 28.04.09   Time: 16:36
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 4  *****************
' User: Hartmannu    Date: 11.09.08   Time: 11:34
' Updated in $/CKAG/Base/Kernel/Security
' Fixing Admin-Änderungen
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
' *****************  Version 7  *****************
' User: Rudolpho     Date: 6.12.07    Time: 16:24
' Updated in $/CKG/Base/Base/Kernel/Security
' ITA: 1440
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 6.12.07    Time: 12:47
' Updated in $/CKG/Base/Base/Kernel/Security
' ITA:1440
' 
' *****************  Version 5  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Kernel/Security
' 
' ************************************************