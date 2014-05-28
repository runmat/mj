Option Explicit On 
Option Strict On

Imports System
Imports System.Xml
Imports System.IO

Namespace Kernel.Logging
    Public Class Trace
        REM § Writes two types of trace entries into databae
        REM § 1) Standard (INF, APP, WRN and ERR)
        REM § 2) Application specific parameter values (tables)

        Protected m_strErrorMessage As String
        Protected m_strConnectionString As String
        Protected m_blnSaveLogAccessSAP As Boolean
        Protected m_strDebugLevel As String
        Protected m_tblStandardLog As DataTable
        Protected m_tblInputDetails As DataTable
        Protected m_objArray(2, 1) As Object
        Protected m_intLogStandardIdentity As Int32

        Public Property LogStandardIdentity() As Int32
            Get
                Return m_intLogStandardIdentity
            End Get
            Set(ByVal Value As Int32)
                m_intLogStandardIdentity = Value
            End Set
        End Property

        Public ReadOnly Property InputDetails() As DataTable
            Get
                m_tblInputDetails = New DataTable()
                Dim i As Int32
                For i = 0 To UBound(m_objArray, 2) - 1
                    'UH: 30.05.2006

                    'So war es bisher:
                    'Das hier ist falsch, weil so alle Einträge "String" werden,
                    'da in der 0. Spalte die Beschreibung (immer Typ "String") steht.
                    'm_tblInputDetails.Columns.Add(m_objArray(0, i).ToString, m_objArray(0, i).GetType)

                    'So wäre es richtig,
                    'da sich in der 1. Spalte das eigentliche, zu speichernde Objekt befindet.
                    'm_tblInputDetails.Columns.Add(m_objArray(0, i).ToString, m_objArray(1, i).GetType)

                    'Ich belasse bei String, um nicht alle historischen Einträge zu gefährden.
                    m_tblInputDetails.Columns.Add(m_objArray(0, i).ToString, System.Type.GetType("System.String"))
                Next
                Dim rowParameter As DataRow
                rowParameter = m_tblInputDetails.NewRow
                For i = 0 To UBound(m_objArray, 2) - 1
                    rowParameter(i) = m_objArray(1, i)
                Next
                m_tblInputDetails.Rows.Add(rowParameter)
                Return m_tblInputDetails
            End Get
        End Property

        Public ReadOnly Property StandardLog() As DataTable
            Get
                Return m_tblStandardLog
            End Get
        End Property

        Public ReadOnly Property ErrorMessage() As String
            Get
                Return m_strErrorMessage
            End Get
        End Property

        Public Sub New(ByVal strConnectionString As String, ByVal blnSaveLogAccessSAP As Boolean, Optional ByVal strDebugLevel As String = "APP")
            m_strErrorMessage = ""
            m_strConnectionString = strConnectionString
            m_strDebugLevel = strDebugLevel
            m_blnSaveLogAccessSAP = blnSaveLogAccessSAP
        End Sub

        Public Function InitEntry( _
        ByVal strUserName As String, ByVal strSessionID As String, _
        ByVal intSource As Int32, ByVal strTask As String, ByVal strCustomername As String, _
        ByVal intCustomerID As Int32, ByVal blnIsTestUser As Boolean, _
        Optional ByVal intSeverity As Int32 = 0) As Boolean

            Dim cn As SqlClient.SqlConnection
            Dim cmdTable As SqlClient.SqlCommand
            m_strErrorMessage = ""

            Try
                cn = New SqlClient.SqlConnection(m_strConnectionString)
                cn.Open()

                Dim strTemp As String
                If blnIsTestUser Then
                    strTemp = "1"
                Else
                    strTemp = "0"
                End If
                cmdTable = New SqlClient.SqlCommand("INSERT INTO LogStandard (Category, UserName, SessionID, Source, Task, Customername, CustomerID, TestZugriff, Severity, Description) VALUES ('ERR', '" & strUserName & "', '" & strSessionID & "', '" & CStr(intSource) & "', '" & strTask & "', '" & strCustomername & "', " & intCustomerID.ToString & ", " & strTemp & ", " & intSeverity & ", 'Eintrag initialisiert.');SELECT SCOPE_IDENTITY() AS 'Identity'", cn)
                cmdTable.CommandType = CommandType.Text
                m_intLogStandardIdentity = CType(cmdTable.ExecuteScalar, Int32)
                cn.Close()

                Return True
            Catch ex As Exception
                m_strErrorMessage = ex.Message
                Return False
            End Try
        End Function

        Public Function UpdateEntry( _
        ByVal strCategory As String, ByVal strIdentification As String, _
        ByVal strDescription As String, Optional ByVal tblParameters As DataTable = Nothing) As Boolean

            Dim cn As SqlClient.SqlConnection
            Dim cmdTable As SqlClient.SqlCommand
            Dim blnWriteLog As Boolean = False

            Select Case m_strDebugLevel
                Case "DBG"
                    blnWriteLog = True
                Case "INF"
                    If Not (strCategory = "DBG") Then
                        blnWriteLog = True
                    End If
                Case "APP"
                    If Not ((strCategory = "DBG") Or (strCategory = "INF")) Then
                        blnWriteLog = True
                    End If
                Case "WRN"
                    If (strCategory = "WRN") Or (strCategory = "ERR") Then
                        blnWriteLog = True
                    End If
                Case "ERR"
                    If strCategory = "ERR" Then
                        blnWriteLog = True
                    End If
            End Select

            m_strErrorMessage = ""

            Try
                cn = New SqlClient.SqlConnection(m_strConnectionString)
                cn.Open()

                If blnWriteLog Then
                    cmdTable = New SqlClient.SqlCommand("UPDATE LogStandard SET Category = '" & strCategory & "', Identification = '" & strIdentification & "', Description = '" & strDescription & "' WHERE ID = " & m_intLogStandardIdentity.ToString, cn)
                    cmdTable.CommandType = CommandType.Text
                    cmdTable.ExecuteNonQuery()

                    If Not tblParameters Is Nothing Then
                        Dim dsExport As New DataSet()
                        dsExport.Tables.Add(tblParameters)

                        Dim para As SqlClient.SqlParameter

                        Dim mstr As New MemoryStream()
                        dsExport.WriteXml(mstr, XmlWriteMode.WriteSchema)

                        Dim b() As Byte
                        b = mstr.ToArray
                        para = New SqlClient.SqlParameter("@AppData", SqlDbType.Image, b.Length, ParameterDirection.Input, False, 0, 0, Nothing, DataRowVersion.Current, b)

                        cmdTable = New SqlClient.SqlCommand()
                        cmdTable.Connection = cn
                        cmdTable.CommandText = "INSERT INTO LogApplication (LogStandardID, AppData) VALUES (" & m_intLogStandardIdentity.ToString & ", @AppData)"
                        cmdTable.Parameters.Add(para)
                        cmdTable.ExecuteNonQuery()
                    End If
                Else
                    cmdTable = New SqlClient.SqlCommand("DELETE FROM LogStandard WHERE ID = " & m_intLogStandardIdentity.ToString, cn)
                    cmdTable.CommandType = CommandType.Text
                    cmdTable.ExecuteNonQuery()
                End If

                cn.Close()

                Return True
            Catch ex As Exception
                m_strErrorMessage = ex.Message
                Return False
            End Try
        End Function

        Public Function WriteEntry( _
        ByVal strCategory As String, ByVal strUserName As String, ByVal strSessionID As String, _
        ByVal intSource As Int32, ByVal strTask As String, ByVal strIdentification As String, _
        ByVal strDescription As String, ByVal strCustomername As String, ByVal intCustomerID As Int32, _
        ByVal blnIsTestUser As Boolean, Optional ByVal intSeverity As Int32 = 0, _
        Optional ByVal tblParameters As DataTable = Nothing) As Boolean

            Dim cn As SqlClient.SqlConnection
            Dim cmdTable As SqlClient.SqlCommand
            Dim blnWriteLog As Boolean = False

            Select Case m_strDebugLevel
                Case "DBG"
                    blnWriteLog = True
                Case "INF"
                    If Not (strCategory = "DBG") Then
                        blnWriteLog = True
                    End If
                Case "APP"
                    If Not ((strCategory = "DBG") Or (strCategory = "INF")) Then
                        blnWriteLog = True
                    End If
                Case "WRN"
                    If (strCategory = "WRN") Or (strCategory = "ERR") Then
                        blnWriteLog = True
                    End If
                Case "ERR"
                    If strCategory = "ERR" Then
                        blnWriteLog = True
                    End If
            End Select

            m_strErrorMessage = ""

            If blnWriteLog Then
                Try
                    cn = New SqlClient.SqlConnection(m_strConnectionString)
                    cn.Open()

                    Dim strTemp As String
                    If blnIsTestUser Then
                        strTemp = "1"
                    Else
                        strTemp = "0"
                    End If
                    cmdTable = New SqlClient.SqlCommand("INSERT INTO LogStandard (Category, UserName, SessionID, Source, Task, Identification, Description, Customername, CustomerID, TestZugriff, Severity) VALUES ('" & strCategory & "', '" & strUserName & "', '" & strSessionID & "', '" & CStr(intSource) & "', '" & strTask & "', '" & strIdentification & "', '" & strDescription & "', '" & strCustomername & "', " & intCustomerID.ToString & ", " & strTemp & ", " & intSeverity & ");SELECT SCOPE_IDENTITY() AS 'Identity'", cn)
                    cmdTable.CommandType = CommandType.Text
                    strTemp = ""
                    m_intLogStandardIdentity = CType(cmdTable.ExecuteScalar, Int32)
                    strTemp = m_intLogStandardIdentity.ToString

                    If Not tblParameters Is Nothing Then
                        Dim dsExport As New DataSet()
                        dsExport.Tables.Add(tblParameters)

                        Dim para As SqlClient.SqlParameter

                        Dim mstr As New MemoryStream()
                        dsExport.WriteXml(mstr, XmlWriteMode.WriteSchema)

                        Dim b() As Byte
                        b = mstr.ToArray
                        para = New SqlClient.SqlParameter("@AppData", SqlDbType.Image, b.Length, ParameterDirection.Input, False, 0, 0, Nothing, DataRowVersion.Current, b)

                        cmdTable = New SqlClient.SqlCommand()
                        cmdTable.Connection = cn
                        cmdTable.CommandText = "INSERT INTO LogApplication (LogStandardID, AppData) VALUES (" & strTemp & ", @AppData)"
                        cmdTable.Parameters.Add(para)
                        cmdTable.ExecuteNonQuery()
                    End If

                    cn.Close()

                    Return True
                Catch ex As Exception
                    m_strErrorMessage = ex.Message
                    Return False
                End Try
            Else
                Return True
            End If
        End Function

        Public Function CollectDetails(ByVal strDecription As String, ByVal objContent As Object, Optional ByVal blnNewDetailsCollection As Boolean = False) As Boolean
            Try
                If blnNewDetailsCollection Then
                    ReDim m_objArray(2, 1)
                    m_objArray(0, 0) = CType(strDecription, Object)
                    m_objArray(1, 0) = objContent
                Else
                    ReDim Preserve m_objArray(2, UBound(m_objArray, 2) + 1)
                    m_objArray(0, UBound(m_objArray, 2) - 1) = CType(strDecription, Object)
                    m_objArray(1, UBound(m_objArray, 2) - 1) = objContent
                End If
                Return True
            Catch ex As Exception
                m_strErrorMessage = ex.Message
                Return False
            End Try
        End Function

        Public Function LogDetails(ByVal intLogStandardID As Int32) As DataTable
            Dim cn As SqlClient.SqlConnection
            Dim cmdOutPut As SqlClient.SqlCommand
            Dim dsAppData As DataSet
            Dim tblAppData As DataTable
            Dim intI As Int32 = 0

            m_strErrorMessage = ""

            Try
                cn = New SqlClient.SqlConnection(m_strConnectionString)
                cn.Open()

                cmdOutPut = New SqlClient.SqlCommand()
                cmdOutPut.Connection = cn
                cmdOutPut.CommandText = "SELECT AppData FROM LogApplication WHERE LogStandardID = " & intLogStandardID.ToString
                Dim drAppData As SqlClient.SqlDataReader = cmdOutPut.ExecuteReader

                If Not drAppData Is Nothing AndAlso drAppData.Read Then
                    If Not TypeOf drAppData(intI) Is System.DBNull Then
                        ' 1. Daten als bytearray aus der DB lesen:
                        Dim bytData(CInt(drAppData.GetBytes(intI, 0, Nothing, 0, Integer.MaxValue - 1) - 1)) As Byte
                        drAppData.GetBytes(intI, 0, bytData, 0, bytData.Length)
                        ' Dataset über einen Memory Stream aus dem ByteArray erzeugen:
                        Dim stmAppData As MemoryStream
                        stmAppData = New MemoryStream(bytData)
                        dsAppData = New DataSet()
                        'dsAppData.ReadXml(stmAppData, XmlReadMode.InferSchema)
                        dsAppData.ReadXml(stmAppData, XmlReadMode.ReadSchema)
                        tblAppData = dsAppData.Tables(0)
                    Else
                        m_strErrorMessage = "Keine Applikationsdaten für den Logeintrag vorhanden."
                        tblAppData = Nothing
                    End If
                Else
                    m_strErrorMessage = "Keine Applikationsdaten für den Logeintrag vorhanden."
                    tblAppData = Nothing
                End If

                cn.Close()

                Return tblAppData
            Catch ex As Exception
                m_strErrorMessage = ex.Message
                Return Nothing
            End Try
        End Function

        Public Function SessionData(ByVal strUserName As String, ByVal strSessionID As String) As Boolean
            Return GetLogData("SELECT * FROM LogStandard WHERE ( UserName = '" & strUserName & "' ) AND ( SessionID = '" & strSessionID & "' ) AND ( NOT Identification = 'Report' )")
        End Function

        Public Function UserData(ByVal strUserName As String, ByVal strInserted As String, Optional ByVal strInserted2 As String = "01.01.1900") As Boolean
            m_strErrorMessage = ""
            Try
                Dim strTemp As String
                If Not IsDate(strInserted) Then
                    m_strErrorMessage = "Bitte Datum/Startdatum übergeben."
                    Return Nothing
                Else
                    If IsDate(strInserted2) AndAlso (Not strInserted2 = "01.01.1900") Then
                        strTemp = CStr(DateAdd(DateInterval.Day, 1, CDate(strInserted2)))
                    Else
                        strTemp = CStr(DateAdd(DateInterval.Day, 1, CDate(strInserted)))
                    End If
                    Return GetLogData("SELECT * FROM LogStandard WHERE ( UserName = '" & strUserName & "' ) AND ( Inserted BETWEEN CONVERT ( Datetime , '" & strInserted & "' , 104 ) AND CONVERT ( Datetime , '" & strTemp & "' , 104 ) ) AND ( NOT Identification = 'Report' )")
                End If
            Catch ex As Exception
                m_strErrorMessage = ex.Message
                Return False
            End Try
        End Function

        Public Function UserData(ByVal strUserName As String, ByVal strTask As String, ByVal strInserted As String, ByVal strInserted2 As String) As Boolean
            m_strErrorMessage = ""
            Try
                Dim strTemp As String
                If Not IsDate(strInserted) Then
                    m_strErrorMessage = "Bitte Datum/Startdatum übergeben."
                    Return Nothing
                ElseIf Not IsDate(strInserted2) Then
                    m_strErrorMessage = "Bitte Datum/Enddatum übergeben."
                    Return Nothing
                Else
                    strTemp = CStr(DateAdd(DateInterval.Day, 1, CDate(strInserted2)))
                    Return GetLogData("SELECT * FROM LogStandard WHERE UserName = '" & strUserName & "' AND Task = '" & strTask & "' AND Inserted BETWEEN CONVERT ( Datetime , '" & strInserted & "' , 104 ) AND CONVERT ( Datetime , '" & strTemp & "' , 104 )")
                End If
            Catch ex As Exception
                m_strErrorMessage = ex.Message
                Return False
            End Try
        End Function

        Public Function GroupData(ByVal strCustomerName As String, ByVal strGroupID As String, ByVal strTask As String, ByVal strInserted As String, ByVal strInserted2 As String) As Boolean
            m_strErrorMessage = ""
            Try
                Dim strTemp As String
                If Not IsDate(strInserted) Then
                    m_strErrorMessage = "Bitte Datum/Startdatum übergeben."
                    Return Nothing
                ElseIf Not IsDate(strInserted2) Then
                    m_strErrorMessage = "Bitte Datum/Enddatum übergeben."
                    Return Nothing
                Else
                    If strTask = String.Empty Then strTask = "%"
                    strTemp = CStr(DateAdd(DateInterval.Day, 1, CDate(strInserted2)))
                    Return GetLogData("SELECT * FROM LogStandard INNER JOIN (SELECT '0' AS AppID UNION SELECT CAST(AppID AS nvarchar) FROM Rights WHERE (GroupID = " & strGroupID & ")) Tmp ON LogStandard.Source = Tmp.AppID WHERE CustomerName = '" & strCustomerName & "' AND Task LIKE '" & strTask & "' AND Inserted BETWEEN CONVERT ( Datetime , '" & strInserted & "' , 104 ) AND CONVERT ( Datetime , '" & strTemp & "' , 104 )")
                End If
            Catch ex As Exception
                m_strErrorMessage = ex.Message
                Return False
            End Try
        End Function

        Public Function CustomerData(ByVal strCustomerName As String, ByVal strTask As String, ByVal strInserted As String, ByVal strInserted2 As String) As Boolean
            m_strErrorMessage = ""
            Try
                Dim strTemp As String
                If Not IsDate(strInserted) Then
                    m_strErrorMessage = "Bitte Datum/Startdatum übergeben."
                    Return Nothing
                ElseIf Not IsDate(strInserted2) Then
                    m_strErrorMessage = "Bitte Datum/Enddatum übergeben."
                    Return Nothing
                Else
                    If strTask = String.Empty Then strTask = "%"
                    strTemp = CStr(DateAdd(DateInterval.Day, 1, CDate(strInserted2)))
                    Return GetLogData("SELECT * FROM LogStandard WHERE CustomerName = '" & strCustomerName & "' AND Task LIKE '" & strTask & "' AND Inserted BETWEEN CONVERT ( Datetime , '" & strInserted & "' , 104 ) AND CONVERT ( Datetime , '" & strTemp & "' , 104 )")
                End If
            Catch ex As Exception
                m_strErrorMessage = ex.Message
                Return False
            End Try
        End Function

        Public Function AllData(ByVal strTask As String, ByVal strInserted As String, ByVal strInserted2 As String) As Boolean
            m_strErrorMessage = ""
            Try
                Dim strTemp As String
                If Not IsDate(strInserted) Then
                    m_strErrorMessage = "Bitte Datum/Startdatum übergeben."
                    Return Nothing
                ElseIf Not IsDate(strInserted2) Then
                    m_strErrorMessage = "Bitte Datum/Enddatum übergeben."
                    Return Nothing
                Else
                    If strTask = String.Empty Then strTask = "%"
                    strTemp = CStr(DateAdd(DateInterval.Day, 1, CDate(strInserted2)))
                    Return GetLogData("SELECT * FROM LogStandard WHERE Task LIKE '" & strTask & "' AND Inserted BETWEEN CONVERT ( Datetime , '" & strInserted & "' , 104 ) AND CONVERT ( Datetime , '" & strTemp & "' , 104 )")
                End If
            Catch ex As Exception
                m_strErrorMessage = ex.Message
                Return False
            End Try
        End Function

        Private Function GetLogData(ByVal strSelectCommand As String) As Boolean
            Dim cn As SqlClient.SqlConnection
            Dim cmdOutPut As SqlClient.SqlCommand
            Dim adpOutPut As SqlClient.SqlDataAdapter = New SqlClient.SqlDataAdapter()

            m_strErrorMessage = ""

            Try
                cn = New SqlClient.SqlConnection(m_strConnectionString)
                cn.Open()

                cmdOutPut = New SqlClient.SqlCommand(strSelectCommand, cn)
                cmdOutPut.CommandType = CommandType.Text

                adpOutPut.SelectCommand = cmdOutPut
                m_tblStandardLog = New DataTable()
                adpOutPut.Fill(m_tblStandardLog)

                cn.Close()

                Return True
            Catch ex As Exception
                m_strErrorMessage = ex.Message
                Return False
            End Try
        End Function

        Public Function GetLogOverViewPerSource(ByVal strAppID As String, ByVal strVon As String, ByVal strBis As String, ByVal intCustomerID As Int32, Optional ByVal intOrganizationID As Integer = 0) As DataTable
            m_strErrorMessage = ""
            Dim tblReturn As DataTable
            tblReturn = New DataTable()
            Try
                Dim strSQL As String
                If intOrganizationID > 0 Then
                    'Eingeschraenkt nach Organization
                    strSQL = "SELECT * FROM  OrganizationMember om INNER JOIN WebUser wu ON om.UserID = wu.UserID INNER JOIN LogStandard l ON wu.Username = l.UserName WHERE (NOT l.Identification = 'Report') AND l.Source = '" & strAppID & "' AND l.CustomerID = " & intCustomerID.ToString & " AND (l.Inserted BETWEEN CONVERT ( Datetime , '" & strVon & "' , 104 ) AND CONVERT ( Datetime , '" & strBis & "' , 104 )) AND om.OrganizationID = " & intOrganizationID
                Else
                    'Immer alle
                    strSQL = "SELECT * FROM LogStandard WHERE (NOT Identification = 'Report') AND Source = '" & strAppID & "' AND CustomerID = " & intCustomerID.ToString & " AND Inserted BETWEEN CONVERT ( Datetime , '" & strVon & "' , 104 ) AND CONVERT ( Datetime , '" & strBis & "' , 104 )"
                End If
                If GetLogData(strSQL) Then
                    If m_tblStandardLog.Rows.Count > 0 Then
                        tblReturn.Columns.Add("LfdNr", System.Type.GetType("System.Int32"))
                        tblReturn.Columns.Add("Erstellung", System.Type.GetType("System.DateTime"))
                        tblReturn.Columns.Add("Benutzer", System.Type.GetType("System.String"))
                        'tblReturn.Columns.Add("Aufgabe", System.Type.GetType("System.String"))
                        'tblReturn.Columns.Add("Test", System.Type.GetType("System.String"))
                        Dim intStandardColumnCount As Int32 = tblReturn.Columns.Count
                        Dim intReturnTableColumnCountBefore As Int32 = tblReturn.Columns.Count
                        Dim i As Int32
                        For i = 0 To m_tblStandardLog.Rows.Count - 1
                            If tblReturn.Columns.Count < intStandardColumnCount + 1 Then
                                Dim tblTemp As DataTable
                                tblTemp = LogDetails(CInt(m_tblStandardLog.Rows(i)("ID")))
                                If (Not tblTemp Is Nothing) AndAlso (tblTemp.Rows.Count > 0) Then
                                    Dim j As Int32
                                    For j = 0 To tblTemp.Columns.Count - 1
                                        tblReturn.Columns.Add(tblTemp.Columns(j).ColumnName, tblTemp.Columns(j).DataType)
                                        intReturnTableColumnCountBefore += 1
                                    Next
                                End If
                            End If
                        Next
                        tblReturn.Columns.Add("Beschreibung", System.Type.GetType("System.String"))
                        '### Beschreibung soll immer eingeblendet werden 20100104 ORu für CRo ###
                        'Dim intExtraColumnCount As Integer = 0
                        'If intReturnTableColumnCountBefore = intStandardColumnCount Then
                        '    intStandardColumnCount += 1
                        '    'intExtraColumnCount = 1
                        '    tblReturn.Columns.Add("Beschreibung", System.Type.GetType("System.String"))
                        'End If
                        '###

                        For i = 0 To m_tblStandardLog.Rows.Count - 1
                            If tblReturn.Columns.Count > intStandardColumnCount Then
                                Dim tblTemp As DataTable
                                tblTemp = LogDetails(CInt(m_tblStandardLog.Rows(i)("ID")))
                                If (Not tblTemp Is Nothing) AndAlso (tblTemp.Rows.Count > 0) Then
                                    Dim j As Int32
                                    For j = 0 To tblTemp.rows.Count - 1
                                        Dim rowTemp As DataRow = tblReturn.NewRow
                                        rowTemp("LfdNr") = i + 1
                                        rowTemp("Erstellung") = m_tblStandardLog.Rows(i)("Inserted")
                                        rowTemp("Benutzer") = m_tblStandardLog.Rows(i)("UserName")
                                        'rowTemp("Aufgabe") = m_tblStandardLog.Rows(i)("Task")
                                        rowTemp("Beschreibung") = m_tblStandardLog.Rows(i)("Description")
                                        'If CType(m_tblStandardLog.Rows(i)("TestZugriff"), Boolean) Then
                                        '    rowTemp("Test") = "Ja"
                                        'Else
                                        '    rowTemp("Test") = "Nein"
                                        'End If
                                        Dim k As Int32
                                        For k = 0 To tblTemp.Columns.Count - 1
                                            If TypeOf tblTemp.Rows(j)(k) Is String Then
                                                Select Case UCase(CStr(tblTemp.Rows(j)(k)))
                                                    Case "TRUE"
                                                        rowTemp(k + intStandardColumnCount) = "X"
                                                    Case "FALSE"
                                                        rowTemp(k + intStandardColumnCount) = " "
                                                    Case Else
                                                        rowTemp(k + intStandardColumnCount) = tblTemp.Rows(j)(k)
                                                End Select
                                            Else
                                                rowTemp(k + intStandardColumnCount) = tblTemp.Rows(j)(k)
                                            End If
                                            'rowTemp(k + intStandardColumnCount - intExtraColumnCount) = tblTemp.Rows(j)(k)
                                        Next
                                        tblReturn.Rows.Add(rowTemp)
                                    Next
                                Else
                                    Dim rowTemp As DataRow = tblReturn.NewRow
                                    rowTemp("LfdNr") = i + 1
                                    rowTemp("Erstellung") = m_tblStandardLog.Rows(i)("Inserted")
                                    rowTemp("Benutzer") = m_tblStandardLog.Rows(i)("UserName")
                                    'rowTemp("Aufgabe") = CStr(m_tblStandardLog.Rows(i)("Task")) & ", " & CStr(m_tblStandardLog.Rows(i)("Description"))
                                    rowTemp("Beschreibung") = m_tblStandardLog.Rows(i)("Description")
                                    'If CType(m_tblStandardLog.Rows(i)("TestZugriff"), Boolean) Then
                                    '    rowTemp("Test") = "Ja"
                                    'Else
                                    '    rowTemp("Test") = "Nein"
                                    'End If
                                    tblReturn.Rows.Add(rowTemp)
                                End If
                            Else
                                Dim rowTemp As DataRow = tblReturn.NewRow
                                rowTemp("LfdNr") = i + 1
                                rowTemp("Erstellung") = m_tblStandardLog.Rows(i)("Inserted")
                                rowTemp("Benutzer") = m_tblStandardLog.Rows(i)("UserName")
                                'rowTemp("Aufgabe") = m_tblStandardLog.Rows(i)("Task")
                                rowTemp("Beschreibung") = m_tblStandardLog.Rows(i)("Description")
                                'If CType(m_tblStandardLog.Rows(i)("TestZugriff"), Boolean) Then
                                '    rowTemp("Test") = "Ja"
                                'Else
                                '    rowTemp("Test") = "Nein"
                                'End If
                                tblReturn.Rows.Add(rowTemp)
                            End If
                        Next
                    End If
                End If
            Catch ex As Exception
                m_strErrorMessage = ex.Message
            End Try
            Return tblReturn
        End Function

        Public Function GetLogOverView() As DataTable
            m_strErrorMessage = ""
            Dim tblReturn As DataTable
            tblReturn = New DataTable()
            Try
                If m_tblStandardLog.Rows.Count > 0 Then
                    tblReturn.Columns.Add("LfdNr", System.Type.GetType("System.Int32"))
                    tblReturn.Columns.Add("Erstellung", System.Type.GetType("System.DateTime"))
                    tblReturn.Columns.Add("Benutzer", System.Type.GetType("System.String"))
                    tblReturn.Columns.Add("Aufgabe", System.Type.GetType("System.String"))
                    tblReturn.Columns.Add("Identifikation", System.Type.GetType("System.String"))
                    tblReturn.Columns.Add("Beschreibung", System.Type.GetType("System.String"))
                    'tblReturn.Columns.Add("Test", System.Type.GetType("System.String"))
                    Dim intStandardColumnCount As Int32 = tblReturn.Columns.Count
                    Dim i As Int32
                    For i = 0 To m_tblStandardLog.Rows.Count - 1
                        If tblReturn.Columns.Count < intStandardColumnCount + 1 Then
                            Dim tblTemp As DataTable
                            tblTemp = LogDetails(CInt(m_tblStandardLog.Rows(i)("ID")))
                            If (Not tblTemp Is Nothing) AndAlso (tblTemp.Rows.Count > 0) Then
                                Dim j As Int32
                                Dim strColName As String = String.Empty

                                For j = 0 To tblTemp.Columns.Count - 1
                                    If IsNothing(tblReturn.Columns.Item(tblTemp.Columns(j).ColumnName)) = True Then
                                        tblReturn.Columns.Add(tblTemp.Columns(j).ColumnName, tblTemp.Columns(j).DataType)
                                    End If
                                Next
                            End If
                        End If
                    Next
                    For i = 0 To m_tblStandardLog.Rows.Count - 1
                        If tblReturn.Columns.Count > intStandardColumnCount Then
                            Dim tblTemp As DataTable
                            tblTemp = LogDetails(CInt(m_tblStandardLog.Rows(i)("ID")))

                            If (Not tblTemp Is Nothing) AndAlso (tblTemp.Rows.Count > 0) Then
                                Dim j As Int32
                                For j = 0 To tblTemp.rows.Count - 1
                                    Dim rowTemp As DataRow = tblReturn.NewRow
                                    rowTemp("LfdNr") = i + 1
                                    rowTemp("Erstellung") = m_tblStandardLog.Rows(i)("Inserted")
                                    rowTemp("Benutzer") = m_tblStandardLog.Rows(i)("UserName")
                                    rowTemp("Aufgabe") = m_tblStandardLog.Rows(i)("Task")
                                    rowTemp("Identifikation") = m_tblStandardLog.Rows(i)("Identification")
                                    rowTemp("Beschreibung") = m_tblStandardLog.Rows(i)("description")
                                    'If CType(m_tblStandardLog.Rows(i)("TestZugriff"), Boolean) Then
                                    '    rowTemp("Test") = "Ja"
                                    'Else
                                    '    rowTemp("Test") = "Nein"
                                    'End If
                                    Dim k As Int32
                                    For k = 0 To tblTemp.Columns.Count - 1

                                        If k = tblTemp.Columns.Count - 1 Then Exit For
                                        rowTemp(k + intStandardColumnCount) = tblTemp.Rows(j)(k + 1)



                                    Next
                                    tblReturn.Rows.Add(rowTemp)
                                Next
                            Else
                                Dim rowTemp As DataRow = tblReturn.NewRow
                                rowTemp("LfdNr") = i + 1
                                rowTemp("Erstellung") = m_tblStandardLog.Rows(i)("Inserted")
                                rowTemp("Benutzer") = m_tblStandardLog.Rows(i)("UserName")
                                rowTemp("Aufgabe") = m_tblStandardLog.Rows(i)("Task")
                                rowTemp("Identifikation") = m_tblStandardLog.Rows(i)("Identification")
                                rowTemp("Beschreibung") = m_tblStandardLog.Rows(i)("Description")
                                'If CType(m_tblStandardLog.Rows(i)("TestZugriff"), Boolean) Then
                                '    rowTemp("Test") = "Ja"
                                'Else
                                '    rowTemp("Test") = "Nein"
                                'End If
                                tblReturn.Rows.Add(rowTemp)
                            End If
                        Else
                            Dim rowTemp As DataRow = tblReturn.NewRow
                            rowTemp("LfdNr") = i + 1
                            rowTemp("Erstellung") = m_tblStandardLog.Rows(i)("Inserted")
                            rowTemp("Benutzer") = m_tblStandardLog.Rows(i)("UserName")
                            rowTemp("Aufgabe") = m_tblStandardLog.Rows(i)("Task")
                            rowTemp("Identifikation") = m_tblStandardLog.Rows(i)("Identification")
                            rowTemp("Beschreibung") = m_tblStandardLog.Rows(i)("Description")
                            'If CType(m_tblStandardLog.Rows(i)("TestZugriff"), Boolean) Then
                            '    rowTemp("Test") = "Ja"
                            'Else
                            '    rowTemp("Test") = "Nein"
                            'End If
                            tblReturn.Rows.Add(rowTemp)
                        End If
                    Next
                End If
            Catch ex As Exception
                m_strErrorMessage = ex.Message
            End Try
            Return tblReturn
        End Function

        Public Function WriteStartDataAccessSAP(ByVal strUsername As String, ByVal blnIsTestUser As Boolean, ByVal strBAPI As String, ByVal strSource As String, ByVal strSessionID As String, ByVal intLogAccessASPXID As Integer) As Int32
            Dim cn As New SqlClient.SqlConnection(m_strConnectionString)
            Dim cmdTable As SqlClient.SqlCommand
            Dim intTemp As Int32 = 0

            Try
                cn = New SqlClient.SqlConnection(m_strConnectionString)
                cn.Open()

                cmdTable = New SqlClient.SqlCommand("INSERT INTO LogAccessSAP (UserName, IsTestUser, BAPI, SessionID, [Source], LogAccessASPXID) VALUES (@UserName, @IsTestUser, @BAPI, @SessionID, @Source, @LogAccessASPXID); SELECT IDENT_CURRENT('LogAccessSAP')", cn)
                cmdTable.Parameters.AddWithValue("@UserName", strUsername)
                cmdTable.Parameters.AddWithValue("@IsTestUser", blnIsTestUser)
                cmdTable.Parameters.AddWithValue("@BAPI", strBAPI)
                cmdTable.Parameters.AddWithValue("@SessionID", strSessionID)
                cmdTable.Parameters.AddWithValue("@Source", strSource)
                cmdTable.Parameters.AddWithValue("@LogAccessASPXID", intLogAccessASPXID)
                cmdTable.CommandType = CommandType.Text
                intTemp = CType(cmdTable.ExecuteScalar, Int32)

            Catch ex As Exception
                m_strErrorMessage = ex.Message
                intTemp = -1
            End Try
            cn.Close()
            Return intTemp
        End Function

        Public Function WriteEndDataAccessSAP(ByVal intID As Int32, ByVal blnSucess As Boolean, Optional ByVal strErrorMessage As String = "") As Boolean
            Dim cn As New SqlClient.SqlConnection(m_strConnectionString)
            Dim cmdTable As SqlClient.SqlCommand
            Dim blnTemp As Boolean = False

            Try
                cn = New SqlClient.SqlConnection(m_strConnectionString)
                cn.Open()

                If m_blnSaveLogAccessSAP Then
                    Dim strTemp As String
                    If blnSucess Then
                        strTemp = "1"
                    Else
                        strTemp = "0"
                    End If
                    cmdTable = New SqlClient.SqlCommand("UPDATE LogAccessSAP SET EndTime = GetDate(), Sucess = " & strTemp & ", ErrorMessage = '" & strErrorMessage & "' WHERE ID=" & intID.ToString, cn)
                Else
                    cmdTable = New SqlClient.SqlCommand("DELETE FROM LogAccessSAP WHERE ID=" & intID.ToString, cn)
                End If
                cmdTable.CommandType = CommandType.Text
                cmdTable.ExecuteNonQuery()

                blnTemp = True
            Catch ex As Exception
                m_strErrorMessage = ex.Message
                blnTemp = False
            End Try
            cn.Close()
            Return blnTemp
        End Function

        Public Function WriteStandardDataAccessSAP(ByVal intID As Int32) As Boolean
            Dim cn As New SqlClient.SqlConnection(m_strConnectionString)
            Dim cmdTable As SqlClient.SqlCommand
            Dim blnTemp As Boolean = False

            Try
                cn = New SqlClient.SqlConnection(m_strConnectionString)
                cn.Open()

                If m_blnSaveLogAccessSAP Then
                    cmdTable = New SqlClient.SqlCommand("UPDATE LogAccessSAP SET StandardLogID = @StandardLogID WHERE ID = @ID", cn)
                    cmdTable.CommandType = CommandType.Text
                    cmdTable.Parameters.AddWithValue("@ID", intID)
                    cmdTable.Parameters.AddWithValue("@StandardLogID", m_intLogStandardIdentity)
                    cmdTable.ExecuteNonQuery()
                End If

                blnTemp = True
            Catch ex As Exception
                m_strErrorMessage = ex.Message
                blnTemp = False
            End Try
            cn.Close()
            Return blnTemp
        End Function

        Public Function PerformanceData_All() As Boolean
            m_strErrorMessage = ""
            Try
                Return GetLogData("SELECT * FROM vwPerformanceCounterAll ORDER BY PerformanceCounterID")
            Catch ex As Exception
                m_strErrorMessage = ex.Message
                Return False
            End Try
        End Function

        Public Function PerformanceData_Detail(ByVal intPerformanceCounterID As Int32, ByVal intScale As Int32, ByRef decMin As Decimal, ByRef decMax As Decimal) As Boolean
            m_strErrorMessage = ""
            Try
                Dim blnReturn As Boolean = GetLogData("SELECT PerformanceCounterID, InsertDate, PerformanceCounterValue FROM PerformanceCounterValues WHERE PerformanceCounterID=" & intPerformanceCounterID.ToString & " ORDER BY InsertDate DESC")
                decMax = 0
                decMin = 1000000000
                Dim rowTemp As DataRow
                For Each rowTemp In m_tblStandardLog.Rows
                    Dim decTemp As Decimal = CDec(rowTemp("PerformanceCounterValue"))
                    If decMax < decTemp Then
                        decMax = decTemp
                    End If
                    If decMin > decTemp Then
                        decMin = decTemp
                    End If
                Next
                Dim decDiff As Decimal = decMax - decMin
                If Not decDiff > 0 Then
                    decDiff = 1
                End If
                m_tblStandardLog.Columns.Add("IntValue", System.Type.GetType("System.Int32"))
                For Each rowTemp In m_tblStandardLog.Rows
                    rowTemp("IntValue") = CInt((CDec(rowTemp("PerformanceCounterValue")) - decMin) * intScale / decDiff)
                Next

                Return blnReturn
            Catch ex As Exception
                m_strErrorMessage = ex.Message
                Return False
            End Try
        End Function
    End Class
End Namespace

' ************************************************
' $History: Trace.vb $
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 4.01.10    Time: 17:08
' Updated in $/CKAG/Base/Kernel/Logging
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 4.01.10    Time: 17:02
' Updated in $/CKAG/Base/Kernel/Logging
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 4.01.10    Time: 16:35
' Updated in $/CKAG/Base/Kernel/Logging
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 4.01.10    Time: 14:43
' Updated in $/CKAG/Base/Kernel/Logging
' Beschreibung soll immer eingeblendet werden ORu für CRo
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Base/Kernel/Logging
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Kernel/Logging
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 4.12.07    Time: 12:51
' Updated in $/CKG/Base/Base/Kernel/Logging
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 15:39
' Updated in $/CKG/Base/Base/Kernel/Logging
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Kernel/Logging
' 
' ************************************************