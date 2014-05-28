Imports CKG.Base.Kernel.Security

Namespace Kernel
    Public Class ColumnTranslation
        REM § Enthält Übersetzungen einer SAP-Tabellen-Spalte zu einer Reporttabellenspalte
        REM § (Übersetzung von Namen und z.T. Datentyp)

#Region " Membervariables "
        Private m_strConnectionstring As String
        Private m_intAppID As Integer
        Private m_strOrgNameAlt As String
        Private m_strOrgNameNeu As String
        Private m_strNewName As String
        Private m_intDisplayOrder As Integer
        Private m_blnNullenEntfernen As Boolean
        Private m_blnTextBereinigen As Boolean
        Private m_blnIstDatum As Boolean
        Private m_blnIstZeit As Boolean
        Private m_blnABEDaten As Boolean
        Private m_strAlignment As String
#End Region

#Region " Constructor "
        Public Sub New(ByVal intAppID As Integer, ByVal strOrgName As String)
            m_intAppID = intAppID
            m_strOrgNameAlt = strOrgName
        End Sub
        Public Sub New(ByVal intAppID As Integer, _
                       ByVal strOrgNameAlt As String, _
                       ByVal strOrgNameNeu As String, _
                       ByVal strNewName As String, _
                       ByVal intDisplayOrder As Integer, _
                       ByVal blnNullenEntfernen As Boolean, _
                       ByVal blnTextBereinigen As Boolean, _
                       ByVal blnIstDatum As Boolean, _
                       ByVal blnIstZeit As Boolean, _
                       ByVal blnABEDaten As Boolean, _
                       ByVal strAlignment As String)
            m_intAppID = intAppID
            m_strOrgNameAlt = strOrgNameAlt
            m_strOrgNameNeu = strOrgNameNeu
            m_strNewName = strNewName
            m_intDisplayOrder = intDisplayOrder
            m_blnNullenEntfernen = blnNullenEntfernen
            m_blnTextBereinigen = blnTextBereinigen
            m_blnIstDatum = blnIstDatum
            m_blnIstZeit = blnIstZeit
            m_blnABEDaten = blnABEDaten
            m_strAlignment = strAlignment
        End Sub
        Public Sub New(ByVal intAppID As Integer, ByVal strOrgName As String, ByVal _user As User)
            Me.New(intAppID, strOrgName, _user.App.Connectionstring)
        End Sub
        Public Sub New(ByVal intAppID As Integer, ByVal strOrgName As String, ByVal strConnectionString As String)
            Me.New(intAppID, strOrgName, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal intAppID As Integer, ByVal strOrgName As String, ByVal cn As SqlClient.SqlConnection)
            m_intAppID = intAppID
            m_strOrgNameAlt = strOrgName
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            GetCol(cn)
        End Sub
#End Region

#Region " Properties "
        Public ReadOnly Property AppId() As Integer
            Get
                Return m_intAppID
            End Get
        End Property

        Public ReadOnly Property OrgNameAlt() As String
            Get
                Return m_strOrgNameAlt
            End Get
        End Property

        Public ReadOnly Property OrgNameNeu() As String
            Get
                Return m_strOrgNameNeu
            End Get
        End Property

        Public ReadOnly Property NewName() As String
            Get
                Return m_strNewName
            End Get
        End Property

        Public ReadOnly Property DisplayOrder() As Integer
            Get
                Return m_intDisplayOrder
            End Get
        End Property

        Public ReadOnly Property NullenEntfernen() As Boolean
            Get
                Return m_blnNullenEntfernen
            End Get
        End Property

        Public ReadOnly Property TextBereinigen() As Boolean
            Get
                Return m_blnTextBereinigen
            End Get
        End Property

        Public ReadOnly Property IstDatum() As Boolean
            Get
                Return m_blnIstDatum
            End Get
        End Property

        Public ReadOnly Property IstZeit() As Boolean
            Get
                Return m_blnIstZeit
            End Get
        End Property

        Public ReadOnly Property ABEDaten() As Boolean
            Get
                Return m_blnABEDaten
            End Get
        End Property

        Public ReadOnly Property Alignment() As String
            Get
                Return m_strAlignment
            End Get
        End Property
#End Region

#Region " Functions "
        Private Sub GetCol(ByVal cn As SqlClient.SqlConnection)
            Dim dr As SqlClient.SqlDataReader
            Dim cmdGetCustomer As New SqlClient.SqlCommand("SELECT * " & _
                                                           "FROM ColumnTranslation " & _
                                                           "WHERE AppID=@AppID " & _
                                                             "AND OrgName=@OrgName", cn)
            cmdGetCustomer.Parameters.AddWithValue("@AppID", m_intAppID)
            cmdGetCustomer.Parameters.AddWithValue("@OrgName", m_strOrgNameAlt)
            dr = cmdGetCustomer.ExecuteReader
            Try

                While dr.Read
                    m_intAppID = CInt(dr("AppID"))
                    m_strOrgNameAlt = dr("OrgName").ToString
                    m_strOrgNameNeu = m_strOrgNameAlt
                    m_strNewName = dr("NewName").ToString
                    If Not TypeOf dr("DisplayOrder") Is System.DBNull Then m_intDisplayOrder = CInt(dr("DisplayOrder"))
                    m_blnNullenEntfernen = CBool(dr("NULLENENTFERNEN"))
                    m_blnTextBereinigen = CBool(dr("TEXTBEREINIGEN"))
                    m_blnIstDatum = CBool(dr("ISTDATUM"))
                    m_blnIstZeit = CBool(dr("ISTZEIT"))
                    m_blnABEDaten = CBool(dr("ABEDaten"))
                    m_strAlignment = dr("Alignment").ToString
                End While
                dr.Close()
                cn.Close()
            Catch ex As Exception
                dr.Close()
                cn.Close()
                Throw ex
            End Try
        End Sub

        Public Sub Delete(ByVal strConnectionString As String)
            Try
                m_strConnectionstring = strConnectionString
                Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
                cn.Open()
                Delete(cn)
            Catch ex As Exception
                Throw New Exception("Fehler beim Löschen der Spaltenübersetzung!", ex)
            End Try
        End Sub
        Public Sub Delete(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim strDelete As String = "DELETE " & _
                                          "FROM ColumnTranslation " & _
                                          "WHERE AppID=@AppID " & _
                                            "AND OrgName=@OrgName"

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn
                cmd.Parameters.AddWithValue("@AppID", m_intAppID)
                cmd.Parameters.AddWithValue("@OrgName", m_strOrgNameAlt)
                cmd.CommandText = strDelete
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Throw New Exception("Fehler beim Löschen der Spaltenübersetzung!", ex)
            End Try
        End Sub

        Public Sub Save(ByVal strConnectionString As String)
            Try
                m_strConnectionstring = strConnectionString
                Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
                cn.Open()
                Save(cn)
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern der Spaltenübersetzung!", ex)
            End Try
        End Sub
        Public Sub Save(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim strInsert As String = "INSERT INTO ColumnTranslation(AppID, " & _
                                                      "OrgName, " & _
                                                      "NewName, " & _
                                                      "DisplayOrder, " & _
                                                      "NULLENENTFERNEN, " & _
                                                      "TEXTBEREINIGEN, " & _
                                                      "ISTDATUM, " & _
                                                      "ISTZEIT, " & _
                                                      "ABEDaten, " & _
                                                      "Alignment) " & _
                                          "VALUES(@AppID, " & _
                                                 "@OrgNameNeu, " & _
                                                 "@NewName, " & _
                                                 "@DisplayOrder, " & _
                                                 "@NULLENENTFERNEN, " & _
                                                 "@TEXTBEREINIGEN, " & _
                                                 "@ISTDATUM, " & _
                                                 "@ISTZEIT, " & _
                                                 "@ABEDaten, " & _
                                                 "@Alignment)"

                Dim strUpdate As String = "UPDATE ColumnTranslation " & _
                                          "SET OrgName=@OrgNameNeu, " & _
                                              "NewName=@NewName, " & _
                                              "DisplayOrder=@DisplayOrder, " & _
                                              "NULLENENTFERNEN=@NULLENENTFERNEN, " & _
                                              "TEXTBEREINIGEN=@TEXTBEREINIGEN, " & _
                                              "ISTDATUM=@ISTDATUM, " & _
                                              "ISTZEIT=@ISTZEIT, " & _
                                              "ABEDaten=@ABEDaten, " & _
                                              "Alignment=@Alignment " & _
                                          "WHERE AppID=@AppID " & _
                                            "AND OrgName=@OrgNameAlt"

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn

                'Speichern
                If m_strOrgNameAlt = String.Empty Then
                    cmd.CommandText = strInsert
                Else
                    cmd.CommandText = strUpdate
                    cmd.Parameters.AddWithValue("@OrgNameAlt", m_strOrgNameAlt)
                End If
                With cmd.Parameters
                    .AddWithValue("@AppID", m_intAppID)
                    .AddWithValue("@OrgNameNeu", m_strOrgNameNeu)
                    .AddWithValue("@NewName", m_strNewName)
                    If m_intDisplayOrder > 0 Then
                        .AddWithValue("@DisplayOrder", m_intDisplayOrder)
                    Else
                        .AddWithValue("@DisplayOrder", System.DBNull.Value)
                    End If
                    .AddWithValue("@NULLENENTFERNEN", m_blnNullenEntfernen)
                    .AddWithValue("@TEXTBEREINIGEN", m_blnTextBereinigen)
                    .AddWithValue("@ISTDATUM", m_blnIstDatum)
                    .AddWithValue("@ISTZEIT", m_blnIstZeit)
                    .AddWithValue("@ABEDaten", m_blnABEDaten)
                    .AddWithValue("@Alignment", m_strAlignment)
                End With

                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern der Spaltenübersetzung!", ex)
            End Try
        End Sub
#End Region

    End Class
End Namespace

' ************************************************
' $History: ColumnTranslation.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 15:47
' Updated in $/CKAG/admin/Kernel
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 14:47
' Created in $/CKAG/admin/Kernel
' 
' *****************  Version 4  *****************
' User: Uha          Date: 9.08.07    Time: 11:38
' Updated in $/CKG/Admin/AdminWeb/Kernel
' Spalte "IstZeit" in Translation übernommen
' 
' *****************  Version 3  *****************
' User: Uha          Date: 1.03.07    Time: 16:49
' Updated in $/CKG/Admin/AdminWeb/Kernel
' 
' ************************************************