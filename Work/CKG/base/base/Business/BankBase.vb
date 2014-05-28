Option Explicit On
Option Strict On

Imports System
Imports System.Configuration

Namespace Business
    <Serializable()> Public MustInherit Class BankBase
        REM § Lese-/Schreibfunktion, Basisklasse.

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
        Protected m_strInternetUser As String
        Protected m_blnGestartet As Boolean
        Protected m_strAppID As String
        Protected m_strFiliale As String
        Protected m_blnZeigeGesperrt As Boolean
        Protected m_strKUNNR As String
        Protected m_hez As Boolean          '23.03.05 True, Wenn HEZ
        Protected m_zuldatum As Date        'für HEZ
        Protected m_BizTalkSapConnectionString As String
        Private Shared m_strAppKey As String = ConfigurationManager.AppSettings("ApplicationKey")

        <NonSerialized()> Protected m_intStandardLogID As Int32
        <NonSerialized()> Protected m_objLogApp As Base.Kernel.Logging.Trace
        <NonSerialized()> Protected m_strSessionID As String
        <NonSerialized()> Protected m_intIDSAP As Int32
        <NonSerialized()> Protected m_strFileName As String
        <NonSerialized()> Protected m_strClassAndMethod As String
        <NonSerialized()> Protected WithEvents m_objSAPDestination As SAP.Connector.Destination
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

        Public Property Customer() As String
            Get
                Return m_strCustomer
            End Get
            Set(ByVal Value As String)
                m_strCustomer = Right("0000000000" & Value, 10)
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

        Public ReadOnly Property Message() As String
            Get
                Return m_strMessage
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

        Public Property KUNNR() As String
            Get
                Return m_strKUNNR
            End Get
            Set(ByVal Value As String)
                m_strKUNNR = Right("0000000000" & Value.Trim(" "c), 10)
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
            m_objSAPDestination = New SAP.Connector.Destination()
            With m_objSAPDestination
                .AppServerHost = m_objApp.SAPAppServerHost
                .SystemNumber = m_objApp.SAPSystemNumber
                .Client = m_objApp.SAPClient
                .Username = m_objApp.SAPUsername
                .Password = m_objApp.SAPPassword
            End With

            m_BizTalkSapConnectionString = "ASHOST=" & m_objApp.SAPAppServerHost & _
                                            ";CLIENT=" & m_objApp.SAPClient & _
                                            ";SYSNR=" & m_objApp.SAPSystemNumber & _
                                            ";USER=" & m_objApp.SAPUsername & _
                                            ";PASSWD=" & m_objApp.SAPPassword & _
                                            ";LANG=DE"

            m_intStatus = 0
            m_strMessage = ""

        End Sub

        Public MustOverride Sub Show()

        Public MustOverride Sub Change()

        Public Function CheckCustomerData() As Boolean
            If m_strCreditControlArea.Length < 4 Then
                m_intStatus = -1001
                m_strMessage = "Kein gültiger Kreditkontrollbereich."
                Return False
            End If

            If m_strCustomer.Trim("0"c).Length = 0 Then
                m_intStatus = -1002
                m_strMessage = "Keine gültige Kundennummer."
                Return False
            End If

            Return True
        End Function

        Public Function MakeDateSAP(ByVal datInput As Date) As String
            Return Year(datInput) & Right("0" & Month(datInput), 2) & Right("0" & Day(datInput), 2)
        End Function

        Public Function MakeDateSAP(ByVal datInput As String) As String
            REM $ Formt Date-Input in String YYYYMMDD um
            Dim dat As Date

            dat = CType(datInput, Date)
            Return Year(dat) & Right("0" & Month(dat), 2) & Right("0" & Day(dat), 2)
        End Function

        Public Function MakeDateStandard(ByVal strInput As String) As Date
            Dim strTemp As String = Right(strInput, 2) & "." & Mid(strInput, 5, 2) & "." & Left(strInput, 4)
            If IsDate(strTemp) Then
                Return CDate(strTemp)
            Else
                Return CDate("01.01.1900")
            End If
        End Function

        Public Function MakeTimeStandard(ByVal strInput As String) As Date
            Dim strTemp As String = Left(strInput, 2) & ":" & Mid(strInput, 3, 2) & ":" & Right(strInput, 2)
            If IsDate(strTemp) Then
                Return CDate(strTemp)
            Else
                Return CDate("00:00:00")
            End If
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
            Dim datatablerow As DataRow
            Dim tbTranslation As New DataTable()
            Dim tblResult As DataTable

            tbTranslation = m_objApp.ColumnTranslation(AppId)
            Dim rowsTranslation As DataRow()
            rowsTranslation = tbTranslation.Select("", "DisplayOrder")

            tblResult = New DataTable("ResultsSAP")
            For Each datatablerow In rowsTranslation
                Dim datatablecolumn As DataColumn
                For Each datatablecolumn In tblTemp.Columns
                    If datatablecolumn.ColumnName.ToUpper = datatablerow("OrgName").ToString.ToUpper Then
                        If CType(datatablerow("IstDatum"), System.Boolean) = True Or CType(datatablerow("IstZeit"), System.Boolean) = True Then
                            tblResult.Columns.Add(Replace(datatablerow("NewName").ToString, ".", ""), System.Type.GetType("System.DateTime"))
                        Else
                            tblResult.Columns.Add(Replace(datatablerow("NewName").ToString, ".", ""), datatablecolumn.DataType)
                        End If
                        tblResult.Columns(tblResult.Columns.Count - 1).ExtendedProperties.Add("Alignment", datatablerow("Alignment").ToString)
                        tblResult.Columns(tblResult.Columns.Count - 1).ExtendedProperties.Add("HeadLine", datatablerow("NewName").ToString)
                    End If
                Next
            Next

            Dim rowResult As DataRow
            For Each rowResult In tblTemp.Rows
                Dim rowNew As DataRow
                rowNew = tblResult.NewRow

                For Each datatablerow In rowsTranslation
                    Dim datatablecolumn As DataColumn
                    For Each datatablecolumn In tblTemp.Columns
                        If datatablecolumn.ColumnName.ToUpper = datatablerow("OrgName").ToString.ToUpper Then
                            If CType(datatablerow("NullenEntfernen"), System.Boolean) = True Then
                                rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = rowResult(datatablerow("OrgName").ToString).ToString.TrimStart("0"c)
                            ElseIf CType(datatablerow("TextBereinigen"), System.Boolean) = True Then
                                rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = Replace(rowResult(datatablerow("OrgName").ToString).ToString, "<(>&<)>", "und")
                            ElseIf CType(datatablerow("IstDatum"), System.Boolean) = True Then
                                If Not IsDate(rowResult(datatablerow("OrgName").ToString)) Then
                                    Dim strTempDate As String = Right(rowResult(datatablerow("OrgName").ToString).ToString, 2) & "." & Mid(rowResult(datatablerow("OrgName").ToString).ToString, 5, 2) & "." & Left(rowResult(datatablerow("OrgName").ToString).ToString, 4)
                                    If IsDate(strTempDate) Then
                                        rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = CDate(strTempDate)
                                        'rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = Format(CDate(strTempDate), "dd.MM.yyyy")
                                    End If
                                Else
                                    rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = rowResult(datatablerow("OrgName").ToString)
                                End If
                            ElseIf CType(datatablerow("IstZeit"), System.Boolean) = True Then
                                Dim strTempDate As String = Left(rowResult(datatablerow("OrgName").ToString).ToString, 2) & ":" & Mid(rowResult(datatablerow("OrgName").ToString).ToString, 3, 2) & ":" & Right(rowResult(datatablerow("OrgName").ToString).ToString, 2)
                                If IsDate(strTempDate) Then
                                    rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = CDate(strTempDate)
                                    'rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = Format(CDate(strTempDate), "hh:mm:ss")
                                End If
                            ElseIf CType(datatablerow("ABEDaten"), System.Boolean) = True Then
                                rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = "<a href=""../Shared/Change06_3.aspx?EqNr=" & rowResult(datatablerow("OrgName").ToString).ToString & """ Target=""_blank"">Anzeige</a>"
                            Else
                                rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = rowResult(datatablerow("OrgName").ToString)
                            End If
                        End If
                    Next
                Next
                tblResult.Rows.Add(rowNew)
            Next
            Return tblResult
        End Function

        Public Sub MakeDestination()
            m_objSAPDestination = New SAP.Connector.Destination()
            m_objSAPDestination.AppServerHost = m_objApp.SAPAppServerHost
            m_objSAPDestination.SystemNumber = m_objApp.SAPSystemNumber
            m_objSAPDestination.Client = m_objApp.SAPClient
            m_objSAPDestination.Username = m_objApp.SAPUsername
            m_objSAPDestination.Password = m_objApp.SAPPassword
        End Sub

        Public Sub WriteLogEntry(ByVal blnSuccess As Boolean, ByVal strComment As String, ByRef tblConsidered As DataTable)
            Try
                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If

                Dim p_strType As String = "ERR"
                Dim p_strComment As String = strComment
                If blnSuccess Then
                    p_strType = "DBG"
                    If (Not m_strFileName Is Nothing) AndAlso _
                      (m_strFileName.Trim(" "c).Length > 0) AndAlso _
                      (Not tblConsidered Is Nothing) AndAlso _
                      (tblConsidered.Rows.Count > 0) Then
                        p_strComment = strComment & " (<a href=""/" & m_strAppKey & "/Temp/Excel/" & m_strFileName & """ target=""_blank"">Excel</a>)"
                    End If
                End If
                If (Not m_strClassAndMethod Is Nothing) AndAlso (m_strClassAndMethod.Length > 0) Then
                    p_strComment = m_strClassAndMethod & ": " & p_strComment
                End If
                m_objLogApp.WriteEntry(p_strType, m_objUser.UserName, m_strSessionID, CInt(m_strAppID), m_objUser.Applications.Select("AppID = '" & m_strAppID & "'")(0)("AppFriendlyName").ToString, "Report", p_strComment, m_objUser.CustomerName, m_objUser.Customer.CustomerId, m_objUser.IsTestUser, 0)
            Catch ex As Exception
                m_objApp.WriteErrorText(1, m_objUser.UserName, "DADReports", "WriteLogEntry", ex.ToString)
            End Try
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