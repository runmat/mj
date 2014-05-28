Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Common
Imports CKG.Base.Business
Imports CKG.Base.Kernel

<Serializable()> _
Public Class Change_01
    Inherits Base.Business.BankBase
    REM § Lese-/Schreibfunktion, Kunde: DAD,
    REM § Show - BAPI: Z_M_Cust_Get_Children_001,
    REM § Change - BAPI: Z_M_Customer_Change_001.

#Region "Declarations"
    Private m_strKatr9 As String
    Private m_strKunnr_I As String
    Private m_strLand1 As String
    Private m_strName1 As String
    Private m_strName2 As String
    Private m_strOrt01 As String
    Private m_strPstlz As String
    Private m_strSmtp_Addr As String
    Private m_strStras As String
    Private m_strTelf1 As String
    Private m_strTelfx As String
    Private m_tblPruefIntervalle As DataTable
#End Region

#Region "Properties"
    Public ReadOnly Property PruefIntervalle() As DataTable
        Get
            Return m_tblPruefIntervalle
        End Get
    End Property

    Public Property Telf1() As String
        Get
            Return m_strTelf1
        End Get
        Set(ByVal Value As String)
            m_strTelf1 = Value
        End Set
    End Property

    Public Property Telfx() As String
        Get
            Return m_strTelfx
        End Get
        Set(ByVal Value As String)
            m_strTelfx = Value
        End Set
    End Property

    Public Property Stras() As String
        Get
            Return m_strStras
        End Get
        Set(ByVal Value As String)
            m_strStras = Value
        End Set
    End Property

    Public Property Smtp_Addr() As String
        Get
            Return m_strSmtp_Addr
        End Get
        Set(ByVal Value As String)
            m_strSmtp_Addr = Value
        End Set
    End Property

    Public Property Pstlz() As String
        Get
            Return m_strPstlz
        End Get
        Set(ByVal Value As String)
            m_strPstlz = Value
        End Set
    End Property

    Public Property Ort01() As String
        Get
            Return m_strOrt01
        End Get
        Set(ByVal Value As String)
            m_strOrt01 = Value
        End Set
    End Property

    Public Property Name1() As String
        Get
            Return m_strName1
        End Get
        Set(ByVal Value As String)
            m_strName1 = Value
        End Set
    End Property

    Public Property Name2() As String
        Get
            Return m_strName2
        End Get
        Set(ByVal Value As String)
            m_strName2 = Value
        End Set
    End Property

    Public Property Katr9() As String
        Get
            Return m_strKatr9
        End Get
        Set(ByVal Value As String)
            m_strKatr9 = Value
        End Set
    End Property

    Public Property Kunnr_I() As String
        Get
            Return m_strKunnr_I
        End Get
        Set(ByVal Value As String)
            m_strKunnr_I = Right("00000000000" & Value, 10)
        End Set
    End Property

    Public Property Land1() As String
        Get
            Return m_strLand1
        End Get
        Set(ByVal Value As String)
            m_strLand1 = Value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

        Customer = objUser.KUNNR
    End Sub

#End Region

#Region "Methods"

    Public Overrides Sub Show()

        If Not m_blnGestartet Then
            m_blnGestartet = True

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""

                S.AP.InitExecute("Z_M_Cust_Get_Children_001", "I_KUNNR", m_strCustomer.PadLeft(10, "0"c))

                m_tblResult = S.AP.GetExportTable("GT_WEB")
                m_tblPruefIntervalle = S.AP.GetExportTable("GT_RHYT")

                Dim row As DataRow
                For Each row In m_tblResult.Rows
                    row("Kunnr") = CStr(row("Kunnr")).TrimStart("0"c)
                Next

                WriteLogEntry(True, "KUNNR=" & m_strCustomer.TrimStart("0"c), Nothing)
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_intStatus = -1402
                        m_strMessage = "Keine Daten gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                WriteLogEntry(False, "KUNNR=" & m_strCustomer.TrimStart("0"c) & ",   " & Replace(m_strMessage, "<br>", " "), Nothing)
            Finally
                m_blnGestartet = False
            End Try

        End If

    End Sub

    Public Overrides Sub Change()

        If Not m_blnGestartet Then
            m_blnGestartet = True

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""

                S.AP.Init("Z_M_Customer_Change_001")
                S.AP.SetImportParameter("I_KUNNR", m_strKunnr_I)
                S.AP.SetImportParameter("I_LAND1", m_strLand1)
                S.AP.SetImportParameter("I_NAME1", m_strName1)
                S.AP.SetImportParameter("I_NAME2", m_strName2)
                S.AP.SetImportParameter("I_ORT01", m_strOrt01)
                S.AP.SetImportParameter("I_PSTLZ", m_strPstlz)
                S.AP.SetImportParameter("I_STRAS", m_strStras)
                S.AP.SetImportParameter("I_TELF1", m_strTelf1)
                S.AP.SetImportParameter("I_TELFX", m_strTelfx)
                S.AP.SetImportParameter("I_SMTP_ADDR", m_strSmtp_Addr)
                S.AP.SetImportParameter("I_KATR9", m_strKatr9)
                S.AP.Execute()

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "ERR_NOT_CHANGE"
                        m_intStatus = -9998
                        m_strMessage = "Änderungen konnten nicht verarbeitet werden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If

    End Sub

#End Region

End Class

' ************************************************
' $History: Change_01.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Kernel
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Kernel
' 
' *****************  Version 3  *****************
' User: Uha          Date: 19.09.07   Time: 13:19
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Kernel
'  ITA 1261: Testfähige Version
' 
' *****************  Version 2  *****************
' User: Uha          Date: 18.09.07   Time: 18:15
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Kernel
' ITA 1261: Erste Testversion (Excelspalten müssen noch übersetzt werden;
' Rückschreiben wirft SAP-Fehler)
' 
' *****************  Version 1  *****************
' User: Uha          Date: 17.09.07   Time: 18:14
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Kernel
' ITA 1261: Under Construction - Keine Funktion
' 
' ************************************************
