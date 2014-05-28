Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Common
Imports CKG.Base.Business

<Serializable()> _
Public Class Adressaenderung
    Inherits Base.Business.BankBase
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

    End Sub

    Public Sub ShowNew(ByVal page As System.Web.UI.Page)

        If Not m_blnGestartet Then
            m_blnGestartet = True

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If

            Try
                m_intStatus = 0
                m_strMessage = ""

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Cust_Get_Children_001", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR", m_strCustomer)


                myProxy.callBapi()


                Dim tblSAP As DataTable = myProxy.getExportTable("GT_WEB")
                Dim tblSAPPruefIntervalle As DataTable = myProxy.getExportTable("GT_RHYT")

                m_tblResult = tblSAP.Copy
                m_tblPruefIntervalle = tblSAPPruefIntervalle.Copy
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

    End Sub

    Public Sub ChangeNew(ByVal page As System.Web.UI.Page)

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                m_intStatus = 0
                m_strMessage = ""

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Customer_Change_001", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_Katr9", m_strKatr9)
                myProxy.setImportParameter("I_Kunnr", m_strKunnr_I)
                myProxy.setImportParameter("I_Land1", m_strLand1)
                myProxy.setImportParameter("I_Name1", m_strName1)
                myProxy.setImportParameter("I_Name2", m_strName2)
                myProxy.setImportParameter("I_Ort01", m_strOrt01)
                myProxy.setImportParameter("I_Pstlz", m_strPstlz)
                myProxy.setImportParameter("I_Smtp_Addr", m_strSmtp_Addr)
                myProxy.setImportParameter("I_Stras", m_strStras)
                myProxy.setImportParameter("I_Telf1", m_strTelf1)
                myProxy.setImportParameter("I_Telfx", m_strTelfx)

                myProxy.callBapi()

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
' $History: Adressaenderung.vb $
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 15.03.10   Time: 10:18
' Updated in $/CKAG2/Services/Components/ComCommon/lib
' ITA:2918
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 21.09.09   Time: 15:44
' Created in $/CKAG2/Services/Components/ComCommon/lib
' ITA: 3112
' 