Option Explicit On
Option Strict On

Imports System

Namespace Business
    <Serializable()> Public MustInherit Class BankBase
        Implements Base.Common.ISapError

#Region " Declarations"
        Protected m_tblResult As DataTable
        Protected m_tblResultExcel As DataTable
        Protected m_strExcelFileName As String
        Protected m_objApp As Base.Kernel.Security.App
        Protected m_objUser As Base.Kernel.Security.User
        Protected m_strCustomer As String
        Protected m_strCreditControlArea As String
        Protected m_intStatus As Int32
        Protected m_strMessage As String
        Protected m_blnErrorOccured As Boolean
        Protected m_strInternetUser As String
        Protected m_blnGestartet As Boolean
        Protected m_strAppID As String
        Protected m_strFiliale As String
        Protected m_blnZeigeGesperrt As Boolean
        ' Kundennummer des Kunden auf 10 Stellen mit Nullen aufgefüllt
        Protected m_strKUNNR As String
        Protected m_hez As Boolean
        Protected m_zuldatum As Date
        Protected m_BizTalkSapConnectionString As String
        'Private Shared m_strAppKey As String = ConfigurationManager.AppSettings("ApplicationKey")

        <NonSerialized()> Protected m_intStandardLogID As Int32
        <NonSerialized()> Protected m_objLogApp As Base.Kernel.Logging.Trace
        <NonSerialized()> Protected m_strSessionID As String
        <NonSerialized()> Protected m_intIDSAP As Int32
        <NonSerialized()> Protected m_strFileName As String
        <NonSerialized()> Protected m_strClassAndMethod As String
#End Region

#Region " Properties"
        Public Property StandardLogID() As Int32
            Get
                Return m_intStandardLogID
            End Get
            Set(ByVal Value As Int32)
                m_intStandardLogID = Value
            End Set
        End Property

        Public ReadOnly Property IDSAP() As Int32
            Get
                Return m_intIDSAP
            End Get
        End Property

        Public Property Result() As DataTable
            Get
                Return m_tblResult
            End Get
            Set(ByVal Value As DataTable)
                m_tblResult = Value
            End Set
        End Property

        Public Property ResultExcel() As DataTable
            Get
                Return m_tblResultExcel
            End Get
            Set(ByVal Value As DataTable)
                m_tblResultExcel = Value
            End Set
        End Property

        Public Property ExcelFileName() As String
            Get
                Return m_strExcelFileName
            End Get
            Set(ByVal Value As String)
                m_strExcelFileName = Value
            End Set
        End Property

        Public Property Filiale() As String
            Get
                Return m_strFiliale
            End Get
            Set(ByVal Value As String)
                m_strFiliale = Right(Value, 6).TrimStart("0"c)
            End Set
        End Property

        Public Property ZeigeGesperrt() As Boolean
            Get
                Return m_blnZeigeGesperrt
            End Get
            Set(ByVal Value As Boolean)
                m_blnZeigeGesperrt = Value
            End Set
        End Property

        '''<summary>Spezielle Kundennummer nur 5-stellig</summary>
        Public Property Customer() As String
            Get
                Return m_strCustomer
            End Get
            Set(value As String)
                m_strCustomer = Right("0000000000" & value, 10)
            End Set
        End Property

        Public Property CreditControlArea() As String
            Get
                Return m_strCreditControlArea
            End Get
            Set(ByVal Value As String)
                m_strCreditControlArea = Value
            End Set
        End Property

        Public ReadOnly Property Status() As Int32
            Get
                Return m_intStatus
            End Get
        End Property

        Public ReadOnly Property Errorcode() As String Implements Common.ISapError.ErrorCode
            Get
                Return m_intStatus.ToString
            End Get
        End Property

        Public ReadOnly Property Message() As String Implements Common.ISapError.ErrorMessage
            Get
                Return m_strMessage
            End Get
        End Property

        Public ReadOnly Property ErrorOccured() As Boolean Implements Common.ISapError.ErrorOccured
            Get
                Return m_blnErrorOccured
            End Get
        End Property

        Public ReadOnly Property InternetUser() As String
            Get
                Return m_strInternetUser
            End Get
        End Property

        Public ReadOnly Property Gestartet() As Boolean
            Get
                Return m_blnGestartet
            End Get
        End Property

        Public Property AppID() As String
            Get
                Return m_strAppID
            End Get
            Set(ByVal Value As String)
                m_strAppID = Value
            End Set
        End Property

        Public Property SessionID() As String
            Get
                Return m_strSessionID
            End Get
            Set(ByVal Value As String)
                m_strSessionID = Value
            End Set
        End Property

        '''<summary>Kundennummer des aktuellen Kunden</summary>
        Public Property KUNNR() As String
            Get
                Return m_strKUNNR.Trim(" "c).PadLeft(10, "0"c)
            End Get
            Set(ByVal Value As String)
                m_strKUNNR = Value.Trim(" "c).PadLeft(10, "0"c)
            End Set
        End Property
#End Region

#Region " Methods"
        Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
            m_objUser = objUser
            m_objApp = objApp
            m_blnZeigeGesperrt = False
            m_strInternetUser = m_objUser.UserName
            m_strAppID = strAppID
            m_strSessionID = strSessionID
            m_strFileName = strFileName


            m_objUser = objUser
            m_objApp = objApp
            m_objLogApp = New Base.Kernel.Logging.Trace(objApp.Connectionstring, objApp.SaveLogAccessSAP, objApp.LogLevel)
            m_strFileName = strFileName

            m_BizTalkSapConnectionString = "ASHOST=" & m_objApp.SAPAppServerHost & _
                                            ";CLIENT=" & m_objApp.SAPClient & _
                                            ";SYSNR=" & m_objApp.SAPSystemNumber & _
                                            ";USER=" & m_objApp.SAPUsername & _
                                            ";PASSWD=" & m_objApp.SAPPassword & _
                                            ";LANG=DE"

            KUNNR = m_objUser.KUNNR
            ClearError()

        End Sub

        Public MustOverride Sub Show()

        Public MustOverride Sub Change()

        Public Function CheckCustomerData() As Boolean
            ClearError()

            If m_strCreditControlArea.Length < 4 Then
                RaiseError("-1001", "Kein gültiger Kreditkontrollbereich.")
                Return False
            End If

            If m_objUser.KUNNR.Length = 0 Then
                RaiseError("-1002", "Keine gültige Kundennummer.")
                Return False
            End If

            Return True
        End Function

        Public Sub CreateExcelFromFieldTranslation(ByVal objectFromSession As Object, ByRef tblInput As DataTable, Optional ByVal strFilter As String = "")
            If strFilter.Length = 0 Then
                m_tblResultExcel = tblInput.Copy
            Else
                m_tblResultExcel = tblInput.Clone
                Dim tmpRows() As DataRow = tblInput.Select(strFilter)
                Dim iCount As Integer

                If tmpRows.Length > 0 Then
                    For iCount = 0 To tmpRows.Length - 1
                        m_tblResultExcel.ImportRow(tmpRows(iCount))
                    Next
                End If
            End If

            If Not objectFromSession Is Nothing AndAlso TypeOf objectFromSession Is DataTable Then
                Dim tblTranslation As DataTable = CType(objectFromSession, DataTable)
                Dim iColCounter As Integer
                Dim rowsTranslation() As DataRow

                For iColCounter = m_tblResultExcel.Columns.Count - 1 To 0 Step -1
                    rowsTranslation = tblTranslation.Select("FieldType='col' AND LEN(Content)>0 AND FieldName='" & m_tblResultExcel.Columns(iColCounter).ColumnName & "'")
                    If rowsTranslation.Length = 0 Then
                        m_tblResultExcel.Columns.RemoveAt(iColCounter)
                    Else
                        m_tblResultExcel.Columns(iColCounter).ColumnName = CStr(rowsTranslation(0)("Content"))
                    End If
                Next
            End If
        End Sub

        Public Function CreateOutPut(ByVal tblTemp As DataTable, ByVal AppId As String) As DataTable
            Dim tbTranslation As DataTable = m_objApp.ColumnTranslation(AppId)
            Dim rowsTranslation As DataRow() = tbTranslation.Select("", "DisplayOrder")

            Dim tblResult As DataTable = New DataTable("ResultsSAP")

            For Each datatablerow As DataRow In rowsTranslation

                Dim datatablecolumn As DataColumn
                For Each datatablecolumn In tblTemp.Columns
                    If datatablecolumn.ColumnName.ToUpper = datatablerow("OrgName").ToString.ToUpper Then
                        If CBool(datatablerow("IstDatum")) = True Or CBool(datatablerow("IstZeit")) = True Then
                            tblResult.Columns.Add(Replace(datatablerow("NewName").ToString, ".", ""), Type.GetType("System.DateTime"))
                        Else
                            tblResult.Columns.Add(Replace(datatablerow("NewName").ToString, ".", ""), datatablecolumn.DataType)
                        End If
                        tblResult.Columns(tblResult.Columns.Count - 1).ExtendedProperties.Add("Alignment", datatablerow("Alignment").ToString)
                        tblResult.Columns(tblResult.Columns.Count - 1).ExtendedProperties.Add("HeadLine", datatablerow("NewName").ToString)
                    End If
                Next

            Next

            For Each rowResult As DataRow In tblTemp.Rows
                Dim rowNew As DataRow = tblResult.NewRow

                For Each datatablerow As DataRow In rowsTranslation
                    Dim orgName As String = datatablerow("OrgName").ToString
                    Dim newName As String = Replace(datatablerow("NewName").ToString, ".", "")

                    For Each datatablecolumn As DataColumn In tblTemp.Columns
                        If datatablecolumn.ColumnName.ToUpper = orgName.ToUpper Then
                            If CBool(datatablerow("NullenEntfernen")) = True Then
                                rowNew(newName) = rowResult(orgName).ToString.TrimStart("0"c)
                            ElseIf CBool(datatablerow("TextBereinigen")) = True Then
                                rowNew(newName) = Replace(rowResult(orgName).ToString, "<(>&<)>", "und")
                            ElseIf CBool(datatablerow("IstDatum")) = True Then
                                If Not IsDate(rowResult(orgName)) Then
                                    Dim strTempDate As String = Right(rowResult(orgName).ToString, 2) & "." & Mid(rowResult(orgName).ToString, 5, 2) & "." & Left(rowResult(orgName).ToString, 4)
                                    If IsDate(strTempDate) Then
                                        rowNew(newName) = CDate(strTempDate)
                                    End If
                                Else
                                    rowNew(newName) = rowResult(orgName)
                                End If
                            ElseIf CType(datatablerow("IstZeit"), System.Boolean) = True Then
                                Dim strTempDate As String = Left(rowResult(orgName).ToString, 2) & ":" & Mid(rowResult(orgName).ToString, 3, 2) & ":" & Right(rowResult(orgName).ToString, 2)
                                If IsDate(strTempDate) Then
                                    rowNew(newName) = CDate(strTempDate)
                                End If
                            ElseIf CType(datatablerow("ABEDaten"), System.Boolean) = True Then
                                rowNew(newName) = "<a href=""../Shared/Change06_3.aspx?EqNr=" & rowResult(orgName).ToString & """ Target=""_blank"">Anzeige</a>"
                            Else
                                rowNew(newName) = rowResult(orgName)
                            End If
                        End If
                    Next
                Next
                tblResult.Rows.Add(rowNew)
            Next

            Return tblResult
        End Function

        Public Sub WriteLogEntry(ByVal blnSuccess As Boolean, ByVal strComment As String, ByRef tblConsidered As DataTable)
            Try
                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If

                Dim p_strType As String = "ERR"
                Dim p_strComment As String = strComment

                If blnSuccess Then
                    p_strType = "DBG"
                    ' Keine Excelausgabe mehr -- CDI 26.04.12
                    'If (Not m_strFileName Is Nothing) AndAlso _
                    '  (m_strFileName.Trim(" "c).Length > 0) AndAlso _
                    '  (Not tblConsidered Is Nothing) AndAlso _
                    '  (tblConsidered.Rows.Count > 0) Then
                    '    p_strComment = strComment & " (<a href=""/" & m_strAppKey & "/Temp/Excel/" & m_strFileName & """ target=""_blank"">Excel</a>)"
                    'End If
                End If
                If (Not m_strClassAndMethod Is Nothing) AndAlso (m_strClassAndMethod.Length > 0) Then
                    p_strComment = m_strClassAndMethod & ": " & p_strComment
                End If
                m_objLogApp.WriteEntry(p_strType, m_objUser.UserName, m_strSessionID, CInt(m_strAppID), m_objUser.Applications.Select("AppID = '" & m_strAppID & "'")(0)("AppFriendlyName").ToString, "Report", p_strComment, m_objUser.CustomerName, m_objUser.Customer.CustomerId, m_objUser.IsTestUser, 0)
            Catch ex As Exception
                m_objApp.WriteErrorText(1, m_objUser.UserName, "DADReports", "WriteLogEntry", ex.ToString)
            End Try
        End Sub

        ''' <summary>
        ''' Schreibt Log-Einträge in die StandardLog-Tabelle ohne Verknüpfungen auf erzeugte Excel-Dateien zu generieren
        '''</summary>
        Public Sub WriteLogEntry(ByVal blnSuccess As Boolean, ByVal strComment As String)
            Try
                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If

                Dim p_strType As String = "ERR"
                Dim p_strComment As String = strComment

                If blnSuccess Then
                    p_strType = "DBG"
                End If
                If (Not m_strClassAndMethod Is Nothing) AndAlso (m_strClassAndMethod.Length > 0) Then
                    p_strComment = m_strClassAndMethod & ": " & p_strComment
                End If
                m_objLogApp.WriteEntry(p_strType, m_objUser.UserName, m_strSessionID, CInt(m_strAppID), m_objUser.Applications.Select("AppID = '" & m_strAppID & "'")(0)("AppFriendlyName").ToString, "Report", p_strComment, m_objUser.CustomerName, m_objUser.Customer.CustomerId, m_objUser.IsTestUser, 0)
            Catch ex As Exception
                m_objApp.WriteErrorText(1, m_objUser.UserName, "DADReports", "WriteLogEntry", ex.ToString)
            End Try
        End Sub

        '''<summary>
        ''' Setzt den Fehlerzustand der Klasse zurück
        '''</summary>
        Public Sub ClearError() Implements Common.ISapError.ClearError
            m_blnErrorOccured = False
            m_intStatus = 0
            m_strMessage = ""
        End Sub

        '''<summary>
        ''' Löst ein Fehlerereignis mit Fehlercode und Fehlermeldung aus
        '''</summary>
        Public Sub RaiseError(errorcode As String, message As String) Implements Common.ISapError.RaiseError
            m_blnErrorOccured = True
            m_intStatus = CInt(errorcode)
            m_strMessage = message
        End Sub

#End Region

#Region "Obsolete Functions"
        <Obsolete("Diese Funktion ist veraltet! Wenn nötig ist die Funktion Business.HelpProcedures.MakeDateSAP() zu verwenden!", True)>
        Public Function MakeDateSAP(ByVal datInput As Date) As String
            Return Year(datInput) & Right("0" & Month(datInput), 2) & Right("0" & Day(datInput), 2)
        End Function

        <Obsolete("Diese Funktion ist veraltet! Wenn nötig ist die Funktion Business.HelpProcedures.MakeDateSAP() zu verwenden!", True)>
        Public Function MakeDateSAP(ByVal datInput As String) As String
            REM $ Formt Date-Input in String YYYYMMDD um
            Dim dat As Date

            dat = CType(datInput, Date)
            Return Year(dat) & Right("0" & Month(dat), 2) & Right("0" & Day(dat), 2)
        End Function

        <Obsolete("Diese Funktion ist veraltet! Wenn nötig ist die Funktion Business.HelpProcedures.MakeDateStandard() zu verwenden!", True)>
        Public Function MakeDateStandard(ByVal strInput As String) As Date
            Dim strTemp As String = Right(strInput, 2) & "." & Mid(strInput, 5, 2) & "." & Left(strInput, 4)
            If IsDate(strTemp) Then
                Return CDate(strTemp)
            Else
                Return CDate("01.01.1900")
            End If
        End Function

        <Obsolete("Diese Funktion ist veraltet! Wenn nötig ist die Funktion Business.HelpProcedures.MakeTimeStandard() zu verwenden!", True)>
        Public Function MakeTimeStandard(ByVal strInput As String) As Date
            Dim strTemp As String = Left(strInput, 2) & ":" & Mid(strInput, 3, 2) & ":" & Right(strInput, 2)
            If IsDate(strTemp) Then
                Return CDate(strTemp)
            Else
                Return CDate("00:00:00")
            End If
        End Function

        <Obsolete("Leerer Funktionsrumpf! Überall zu entfernen", True)>
        Public Sub MakeDestination()
        End Sub
#End Region

    End Class
End Namespace

' ************************************************
' $History: BankBase.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 27.07.09   Time: 9:25
' Updated in $/CKAG/Base/Business
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 5.12.08    Time: 12:54
' Updated in $/CKAG/Base/Business
' Anpassung ColumnTranslation für DynProxy
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 16.06.08   Time: 15:07
' Updated in $/CKAG/Base/Business
' BizTalkConnectionString hinzugefügt
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Business
' 
' *****************  Version 9  *****************
' User: Uha          Date: 27.09.07   Time: 13:04
' Updated in $/CKG/Base/Base/Business
' CreateOutPut erzeugt wieder echte Datumsspalten
' 
' *****************  Version 8  *****************
' User: Uha          Date: 20.09.07   Time: 16:34
' Updated in $/CKG/Base/Base/Business
' ITA 1181: Testversion
' 
' *****************  Version 7  *****************
' User: Uha          Date: 19.09.07   Time: 13:19
' Updated in $/CKG/Base/Base/Business
'  ITA 1261: Testfähige Version
' 
' *****************  Version 6  *****************
' User: Uha          Date: 22.05.07   Time: 9:31
' Updated in $/CKG/Base/Base/Business
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 5  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Business
' 
' ************************************************