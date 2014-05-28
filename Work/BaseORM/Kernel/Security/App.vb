Imports System.Configuration

Namespace Kernel.Security
    <Serializable()> Public Class App
        REM § Dient der Haltung zentraler Anwendungsdaten 
        REM § (Aus der web.config)


#Region " Membervariables "
        Private m_strConnectionstring As String
        Private m_strLogLevel As String
        Private m_strWebAppPath As String
        Private m_strExcelPath As String
        Private m_blnTestModus As Boolean
        Private m_strSAPAppServerHost As String
        Private m_shortSAPSystemNumber As Short
        Private m_shortSAPClient As Short
        Private m_strSAPUsername As String
        Private m_strSAPPassword As String
        Private m_blnSaveLogAccessSAP As Boolean
        Private m_intWriteErrorTextLevel As Integer
        Private m_intAppAutLevel As Integer
        Private m_intWithAuthorization As Integer
        Private m_AutLevel As String
#End Region

#Region " Constructor "
        Public Sub New(ByVal User As User)
            m_strConnectionstring = ConfigurationManager.AppSettings("Connectionstring")
            m_strLogLevel = ConfigurationManager.AppSettings("LogLevel")
            m_strWebAppPath = ConfigurationManager.AppSettings("WebAppPath")
            m_strExcelPath = ConfigurationManager.AppSettings("ExcelPath")
            If ConfigurationManager.AppSettings("TestModus") = "ON" Then
                m_blnTestModus = True
            Else
                m_blnTestModus = False
            End If
            If ConfigurationManager.AppSettings("SaveLogAccessSAP") = "ON" Then
                m_blnSaveLogAccessSAP = True
            Else
                m_blnSaveLogAccessSAP = False
            End If
            Select Case ConfigurationManager.AppSettings("WriteErrorTextLevel")
                Case "2"
                    m_intWriteErrorTextLevel = 2
                Case "1"
                    m_intWriteErrorTextLevel = 1
                Case Else
                    m_intWriteErrorTextLevel = 0
            End Select

            If Not User.IsTestUser Then
                m_strSAPAppServerHost = ConfigurationManager.AppSettings("SAPAppServerHost")
                m_shortSAPSystemNumber = CShort(ConfigurationManager.AppSettings("SAPSystemNumber"))
                m_shortSAPClient = CShort(ConfigurationManager.AppSettings("SAPClient"))
                m_strSAPUsername = ConfigurationManager.AppSettings("SAPUsername")
                m_strSAPPassword = ConfigurationManager.AppSettings("SAPPassword")
            Else
                m_strSAPAppServerHost = ConfigurationManager.AppSettings("TESTSAPAppServerHost")
                m_shortSAPSystemNumber = CShort(ConfigurationManager.AppSettings("TESTSAPSystemNumber"))
                m_shortSAPClient = CShort(ConfigurationManager.AppSettings("TESTSAPClient"))
                m_strSAPUsername = ConfigurationManager.AppSettings("TESTSAPUsername")
                m_strSAPPassword = ConfigurationManager.AppSettings("TESTSAPPassword")
            End If

        End Sub
#End Region

#Region " Properties "
        Public ReadOnly Property Connectionstring() As String
            Get
                Return m_strConnectionstring
            End Get
        End Property

        Public Property LogLevel() As String
            Get
                Return m_strLogLevel
            End Get
            Set(ByVal Value As String)
                m_strLogLevel = Value
            End Set
        End Property

        Public ReadOnly Property WebAppPath() As String
            Get
                Return m_strWebAppPath
            End Get
        End Property

        Public ReadOnly Property ExcelPath() As String
            Get
                Return m_strExcelPath
            End Get
        End Property

        Public ReadOnly Property TestModus() As Boolean
            Get
                Return m_blnTestModus
            End Get
        End Property

        Public ReadOnly Property SaveLogAccessSAP() As Boolean
            Get
                Return m_blnSaveLogAccessSAP
            End Get
        End Property

        Public Property SAPAppServerHost() As String
            Get
                Return m_strSAPAppServerHost
            End Get
            Set(ByVal Value As String)
                m_strSAPAppServerHost = Value
            End Set
        End Property

        Public Property SAPSystemNumber() As Short
            Get
                Return m_shortSAPSystemNumber
            End Get
            Set(ByVal Value As Short)
                m_shortSAPSystemNumber = Value
            End Set
        End Property

        Public Property SAPClient() As Short
            Get
                Return m_shortSAPClient
            End Get
            Set(ByVal Value As Short)
                m_shortSAPClient = Value
            End Set
        End Property

        Public Property SAPUsername() As String
            Get
                Return m_strSAPUsername
            End Get
            Set(ByVal Value As String)
                m_strSAPUsername = Value
            End Set
        End Property

        Public Property SAPPassword() As String
            Get
                Return m_strSAPPassword
            End Get
            Set(ByVal Value As String)
                m_strSAPPassword = Value
            End Set
        End Property

        Public ReadOnly Property AppAutLevel() As Integer
            Get
                Return m_intAppAutLevel
            End Get
        End Property


        Public ReadOnly Property WithAuthorization() As Integer
            Get
                Return m_intWithAuthorization
            End Get
        End Property

        Public ReadOnly Property AutorisierungsLevel() As String
            Get
                Return m_AutLevel
            End Get
        End Property

#End Region

#Region " Functions "
        Public Function ColumnTranslation(ByVal strAppID As String) As DataTable
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            Dim cmdTranslation As SqlClient.SqlCommand
            Dim dsTranslationData As DataSet
            Dim adTranslation As SqlClient.SqlDataAdapter

            Try
                cn.Open()

                dsTranslationData = New DataSet()

                adTranslation = New SqlClient.SqlDataAdapter()
                adTranslation.TableMappings.Add("Table", "Translations")

                cmdTranslation = New SqlClient.SqlCommand("SELECT * FROM ColumnTranslation WHERE AppID = " & strAppID, cn)
                cmdTranslation.CommandType = CommandType.Text

                adTranslation.SelectCommand = cmdTranslation
                adTranslation.Fill(dsTranslationData)

                'cn.Close()
                Return dsTranslationData.Tables("Translations")
            Catch ex As Exception
                Throw ex
            Finally
                cn.Close()
            End Try
        End Function

        Public Sub CheckForPendingAuthorization(ByVal AppID As Int32, ByVal OrganizationID As Int32, ByVal CustomerReference As String, ByVal ProcessReference As String, ByVal TestUser As Boolean, ByRef Initiator As String, ByRef AuthorizationID As Int32)
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            Dim cmdTable As New SqlClient.SqlCommand


            Try
                Initiator = ""
                AuthorizationID = -1

                cn.Open()

                cmdTable = New SqlClient.SqlCommand()
                cmdTable.Connection = cn


                If OrganizationID = -22 Then
                    cmdTable.CommandText = "SELECT InitializedBy,AuthorizationID FROM [Authorization]" & _
                    " WHERE AppID = @AppID" & _
                    " AND CustomerReference = @CustomerReference" & _
                    " AND ProcessReference = @ProcessReference" & _
                    " AND TestUser = @TestUser"
                Else
                    cmdTable.CommandText = "SELECT InitializedBy,AuthorizationID FROM [Authorization]" & _
                    " WHERE AppID = @AppID" & _
                    " AND OrganizationID = @OrganizationID" & _
                    " AND CustomerReference = @CustomerReference" & _
                    " AND ProcessReference = @ProcessReference" & _
                    " AND TestUser = @TestUser"
                End If
                cmdTable.Parameters.AddWithValue("@AppID", AppID)
                If Not OrganizationID = -22 Then
                    cmdTable.Parameters.AddWithValue("@OrganizationID", OrganizationID)
                End If
                cmdTable.Parameters.AddWithValue("@CustomerReference", CustomerReference)
                cmdTable.Parameters.AddWithValue("@ProcessReference", ProcessReference)
                cmdTable.Parameters.AddWithValue("@TestUser", TestUser)

                Dim readerTmp As SqlClient.SqlDataReader
                readerTmp = cmdTable.ExecuteReader

                While readerTmp.Read()
                    Initiator = readerTmp.GetString(0)
                    AuthorizationID = readerTmp.GetInt32(1)
                End While
            Catch ex As Exception
                Throw ex
            Finally
                cn.Close()
            End Try
        End Sub
        Public Sub GetAppAutLevel(ByVal intGroupID As Integer, ByVal AppID As Integer)
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)

            Try
                cn.Open()
                Dim cmdSql As New SqlClient.SqlCommand("Select AuthorizationLevel, WithAuthorization, NewLevel From dbo.Rights " & _
                "WHERE AppID=" & AppID & " And GroupID= " & intGroupID, cn)


                Dim readerTmp As SqlClient.SqlDataReader
                readerTmp = cmdSql.ExecuteReader

                While readerTmp.Read()
                    m_intAppAutLevel = readerTmp.GetInt32(0)
                    m_intWithAuthorization = readerTmp.GetInt32(1)
                    m_AutLevel = readerTmp.GetValue(2).ToString
                End While
            Catch ex As Exception
                Throw New Exception("Fehler beim Laden des Gruppen-AuthorizationLevel!", ex)

            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
        Public Sub CheckForPendingAuthorization(ByVal AppID As Int32, ByVal CustomerReference As String, ByVal ProcessReference As String, ByVal TestUser As Boolean, ByRef Initiator As String, ByRef AuthorizationID As Int32)
            CheckForPendingAuthorization(AppID, -22, CustomerReference, ProcessReference, TestUser, Initiator, AuthorizationID)
        End Sub

        Public Sub WriteErrorText(ByVal intLevel As Integer, ByVal strWebUser As String, ByVal strObject As String, ByVal strTask As String, ByVal strExceptionToString As String)
            If m_intWriteErrorTextLevel >= intLevel Then
                Try
                    If InStr(strExceptionToString, "System.Threading.ThreadAbortException") = 0 Then
                        Dim FileNumber As Short = CShort(FreeFile())
                        Dim FileName As String = m_strExcelPath & Format(Now, "yyyyMMdd") & ".txt"

                        'File öffnen
                        Try
                            Dim FileAttribut As FileAttribute = GetAttr(FileName)
                            FileOpen(FileNumber, FileName, OpenMode.Append, OpenAccess.Default, OpenShare.Shared)
                        Catch ex0 As Exception
                            Try
                                If TypeOf ex0 Is System.IO.FileNotFoundException Then
                                    FileOpen(FileNumber, FileName, OpenMode.Output, OpenAccess.Default, OpenShare.Shared)
                                Else
                                    Throw ex0
                                End If
                            Catch ex As Exception
                                Throw ex
                            End Try
                        End Try

                        'Zeile schreiben
                        PrintLine(FileNumber, Format(Now, "hh:mm:ss,fff") & vbTab & strWebUser & vbTab & strObject & vbTab & strTask & vbTab & strExceptionToString)

                        'File schließen
                        FileClose(FileNumber)
                    End If
                Catch exOut As Exception
                    'Fehler beim Fehler schreiben
                    '=> keine Idee!
                End Try
            End If
        End Sub
#End Region

    End Class
End Namespace

' ************************************************
' $History: App.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 25.02.11   Time: 16:32
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 25.08.10   Time: 17:57
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 29.04.08   Time: 8:33
' Updated in $/CKAG/Base/Kernel/Security
' Migration
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
' User: Uha          Date: 23.05.07   Time: 12:45
' Updated in $/CKG/Base/Base/Kernel/Security
' TESTSAPUsername und SAPUsername aus Tabelle Customer entfernt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Kernel/Security
' 
' ************************************************