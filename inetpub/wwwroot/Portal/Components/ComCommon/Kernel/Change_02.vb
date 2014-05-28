Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel

<Serializable()> _
Public Class Change_02
    Inherits Base.Business.BankBase
    REM § Lese-/Schreibfunktion, Kunde: DAD,
    REM § Show - BAPI: ohne,
    REM § Change - BAPI: Z_M_Flc_Create_Auftr_003.

#Region "Declarations"
    Private m_strRegion As String
    Private m_strKunnr_I As String
    Private m_strWldat As String
    Private m_strWldat_Typ As String
    Private m_strDienstleistung As String
    Private m_strVbeln As String
#End Region

#Region "Properties"
    Public ReadOnly Property Vbeln() As String
        Get
            Return m_strVbeln
        End Get
    End Property

    Public Property Region() As String
        Get
            Return m_strRegion
        End Get
        Set(ByVal Value As String)
            m_strRegion = Value
        End Set
    End Property

    Public Property Wldat_Typ() As String
        Get
            Return m_strWldat_Typ
        End Get
        Set(ByVal Value As String)
            m_strWldat_Typ = Value
        End Set
    End Property

    Public Property Wldat() As String
        Get
            Return m_strWldat
        End Get
        Set(ByVal Value As String)
            m_strWldat = Value
        End Set
    End Property

    Public Property Dienstleistung() As String
        Get
            Return m_strDienstleistung
        End Get
        Set(ByVal Value As String)
            m_strDienstleistung = Value
        End Set
    End Property

    Public Property Kunnr_I() As String
        Get
            Return m_strKunnr_I
        End Get
        Set(ByVal Value As String)
            m_strKunnr_I = Value
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

    Public Overrides Sub Change()

        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_strVbeln = ""

            MakeDestination()
            Dim objSAP As New SAPProxy_ComCommon.SAPProxy_ComCommon()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""

                Dim strKunnr As String = " "
                If m_strKunnr_I.Trim(" "c).Length > 0 Then
                    strKunnr = Right("00000000000" & m_strKunnr_I.Trim(" "c), 10)
                End If

                m_intIDSAP = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Flc_Create_Auftr_003", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Flc_Create_Auftr_003(m_strDienstleistung, m_strRegion, Right("0000000000" & m_objUser.KUNNR, 10), strKunnr, m_strWldat, m_strWldat_Typ, m_strVbeln)
                objSAP.CommitWork()

                If m_intIDSAP > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If

                m_strVbeln = m_strVbeln.TrimStart("0"c)
            Catch ex As Exception
                Select Case ex.Message
                    Case "FEHL_DAT"
                        m_intStatus = -9990
                        m_strMessage = "Datum / Datumstyp unzulässig."
                    Case "FEHL_ZF"
                        m_intStatus = -9991
                        m_strMessage = "Händler existiert nicht."
                    Case "FEHL_AG"
                        m_intStatus = -9992
                        m_strMessage = "Auftraggeber existiert nicht."
                    Case "NO_MATERIAL"
                        m_intStatus = -9993
                        m_strMessage = "Kein Material zugeordnet."
                    Case "FEHL_SD"
                        m_intStatus = -9994
                        m_strMessage = "Fehler bei Auftragsanlage."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If m_intIDSAP > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
            Finally
                objSAP.Connection.Close()
                objSAP.Dispose()
                m_blnGestartet = False
            End Try
        End If

    End Sub

#End Region

End Class

' ************************************************
' $History: Change_02.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Kernel
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Kernel
' 
' *****************  Version 2  *****************
' User: Uha          Date: 27.09.07   Time: 16:20
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Kernel
' ITA 1238 Bugfixing
' 
' *****************  Version 1  *****************
' User: Uha          Date: 24.09.07   Time: 17:01
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Kernel
' ITA 1238: Anlage Floorcheckauftrag - Testversion
' 
' ************************************************
