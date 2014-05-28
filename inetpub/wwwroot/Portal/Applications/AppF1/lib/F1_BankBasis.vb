Option Explicit On
Option Strict On
Imports CKG
Imports System

<Serializable()> Public MustInherit Class F1_BankBasis
    ' ehemals BankBase

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

    Public Property Customer() As String
        Get
            Return m_strCustomer
        End Get
        Set(ByVal Value As String)
            m_strCustomer = Right(Value, 10)
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

    Public Property ZLST As String


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
    End Sub

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
                            Dim strTempDate As String = Right(rowResult(datatablerow("OrgName").ToString).ToString, 2) & "." & Mid(rowResult(datatablerow("OrgName").ToString).ToString, 5, 2) & "." & Left(rowResult(datatablerow("OrgName").ToString).ToString, 4)
                            If IsDate(strTempDate) Then
                                rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = CDate(strTempDate)
                            End If
                        ElseIf CType(datatablerow("IstZeit"), System.Boolean) = True Then
                            Dim strTempDate As String = Left(rowResult(datatablerow("OrgName").ToString).ToString, 2) & ":" & Mid(rowResult(datatablerow("OrgName").ToString).ToString, 3, 2) & ":" & Right(rowResult(datatablerow("OrgName").ToString).ToString, 2)
                            If IsDate(strTempDate) Then
                                rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = CDate(strTempDate)
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
                    p_strComment = strComment & " (<a href=""/Portal/Temp/Excel/" & m_strFileName & """ target=""_blank"">Excel</a>)"
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
' ************************************************
' $History: F1_BankBasis.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 9.03.09    Time: 14:51
' Updated in $/CKAG/Applications/AppF1/lib
' 2664 testfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 4.03.09    Time: 17:30
' Updated in $/CKAG/Applications/AppF1/lib
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 4.03.09    Time: 11:12
' Created in $/CKAG/Applications/AppF1/lib
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 12.09.08   Time: 15:31
' Updated in $/CKAG/Applications/AppFFE/lib
' ITA 2216 unfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 7.07.08    Time: 10:40
' Updated in $/CKAG/Applications/AppFFE/lib
' SSV Historien hinzugefgt
' 
' ************************************************
