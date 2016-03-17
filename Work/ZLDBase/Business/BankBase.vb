Option Explicit On
Option Strict On

Imports System

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

        <NonSerialized()> Protected m_intStandardLogID As Int32
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
            m_strFileName = strFileName

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

#End Region

    End Class
End Namespace
