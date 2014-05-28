Option Explicit On
Option Strict On

Imports System
Imports System.Configuration

Namespace Business
    Public MustInherit Class ReportBase
        REM § Basisklasse aller nur lesenden Reports, Fill-Methoden jeweils individuell,
        REM § Ergebnisse in Datatable-Property.

#Region " Declarations"
        Protected m_tblResult As DataTable
        Protected m_tableResult As DataTable
        Protected m_objUser As Base.Kernel.Security.User
        Protected m_objApp As Base.Kernel.Security.App
        Protected m_objLogApp As Base.Kernel.Logging.Trace
        Protected m_intStatus As Int32
        Protected m_strMessage As String
        Protected m_blnGestartet As Boolean
        Protected m_strAppID As String
        Protected m_strSessionID As String
        Protected m_strFileName As String
        Protected m_strClassAndMethod As String
        Protected m_BizTalkSapConnectionString As String
        Protected WithEvents m_objSAPDestination As SAP.Connector.Destination
        Private Shared m_strAppKey As String = ConfigurationManager.AppSettings("ApplicationKey")
#End Region

#Region " Properties"

        Public Property ResultTable() As DataTable
            Get
                Return m_tableResult
            End Get
            Set(ByVal Value As DataTable)
                m_tableResult = Value
            End Set
        End Property

        Public ReadOnly Property Result() As DataTable
            Get
                Return m_tblResult
            End Get
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
#End Region

#Region " Methods"
        Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFileName As String)
            REM § Constructor. Übernimmt User und Applikationsobjekt und Verbindungsobjekt (SAPDestination).
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

        Public MustOverride Sub Fill()

        Public Function MakeDateSAP(ByVal datInput As Date) As String
            REM $ Formt Date-Input in String YYYYMMDD um
            Return Year(datInput) & Right("0" & Month(datInput), 2) & Right("0" & Day(datInput), 2)
        End Function

        Public Function MakeDateSAP(ByVal datInput As String) As String
            REM $ Formt Date-Input in String YYYYMMDD um
            Dim dat As Date

            dat = CType(datInput, Date)
            Return Year(dat) & Right("0" & Month(dat), 2) & Right("0" & Day(dat), 2)
        End Function

        Public Function MakeDateStandard(ByVal strInput As String) As Date
            REM § Formt String-Input im SAP-Format in Standard-Date um. Gibt "01.01.1900" zurück, wenn Umwandlung nicht möglich ist.
            Dim strTemp As String = Right(strInput, 2) & "." & Mid(strInput, 5, 2) & "." & Left(strInput, 4)
            If IsDate(strTemp) Then
                Return CDate(strTemp)
            Else
                Return CDate("01.01.1900")
            End If
        End Function

        Public Sub WriteLogEntry(ByVal blnSuccess As Boolean, ByVal strComment As String, ByRef tblConsidered As DataTable, Optional ByVal blnWrite As Boolean = False)
            Try
                Dim p_strType As String = "ERR"
                Dim p_strComment As String = strComment
                If blnSuccess Then
                    p_strType = "DBG"
                    If blnWrite Then
                        p_strType = "APP"
                    End If
                    If (Not m_strFileName Is Nothing) AndAlso _
                      (m_strFileName.Trim(" "c).Length > 0) AndAlso _
                      (Not tblConsidered Is Nothing) AndAlso _
                      (tblConsidered.Rows.Count > 0) AndAlso _
                      (Not blnWrite) Then
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
' $History: ReportBase.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 27.07.09   Time: 9:25
' Updated in $/CKAG/Base/Business
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 4.06.08    Time: 16:42
' Updated in $/CKAG/Base/Business
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Business
' 
' *****************  Version 5  *****************
' User: Uha          Date: 22.05.07   Time: 9:31
' Updated in $/CKG/Base/Base/Business
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 4  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Business
' 
' ************************************************