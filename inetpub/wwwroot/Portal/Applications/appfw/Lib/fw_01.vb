Option Explicit On 
Option Strict On

Imports System
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.Configuration
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports Microsoft.Data.SAPClient

Public Class fw_01
    Inherits Base.Business.DatenimportBase
    Private strfahrgestellnummer As String
    Private strkennzeichen As String


#Region " Properties "

    Public Property kennzeichen() As String
        Get
            Return strkennzeichen
        End Get
        Set(ByVal value As String)
            strkennzeichen = value
        End Set
    End Property

    Public Property fahrgestellnummer() As String
        Get
            Return strfahrgestellnummer
        End Get
        Set(ByVal value As String)
            strfahrgestellnummer = value
        End Set
    End Property

  

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
        FILL(m_strAppID, m_strSessionID)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "fw_01.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1
            Dim con As New SAPConnection(m_BizTalkSapConnectionString)
            con.Open()
            Try
                Dim cmd As New SAPCommand()
                cmd.Connection = con

                Dim strCom As String

                strCom = "EXEC Z_M_DAD_FW_BASIS_001 @I_KUNNR=@pI_KUNNR,@I_CHASSIS_NUM=@pI_CHASSIS_NUM,@I_LICENSE_NUM=@pI_LICENSE_NUM,@GT_WEB=@pI_GT_WEB,"
                strCom = strCom & "@GT_WEB=@pE_GT_WEB OUTPUT OPTION 'disabledatavalidation'"

                cmd.CommandText = strCom

                'importparameter
                Dim pI_KUNNR As New SAPParameter("@pI_KUNNR", ParameterDirection.Input)
                Dim pI_CHASSIS_NUM As New SAPParameter("@pI_CHASSIS_NUM", ParameterDirection.Input)
                Dim pI_LICENSE_NUM As New SAPParameter("@pI_LICENSE_NUM", ParameterDirection.Input)
                Dim pI_GT_WEB As New SAPParameter("@pI_GT_WEB", New DataTable)

                'outputparameter
                Dim pE_GT_WEB As New SAPParameter("@pE_GT_WEB", ParameterDirection.Output)

                'Importparameter hinzufügen
                cmd.Parameters.Add(pI_KUNNR)
                cmd.Parameters.Add(pI_CHASSIS_NUM)
                cmd.Parameters.Add(pI_LICENSE_NUM)
                cmd.Parameters.Add(pI_GT_WEB)

                'exportparameter hinzufügen
                cmd.Parameters.Add(pE_GT_WEB)

                'befüllen der Importparameter
                pI_KUNNR.Value = Right("0000000000" & m_objUser.KUNNR, 10)
                pI_CHASSIS_NUM.Value = strfahrgestellnummer.ToUpper
                pI_LICENSE_NUM.Value = strkennzeichen.ToUpper

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Dad_Fw_Basis_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                cmd.ExecuteNonQuery()
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                If Not pE_GT_WEB.Value Is Nothing Then
                    CreateOutPut(DirectCast(pE_GT_WEB.Value, DataTable), m_strAppID)
                End If

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NOT_FOUND"
                        m_intStatus = -1111
                        m_strMessage = "Kein Fahrzeug gefunden."
                    Case "NO_DATA"
                        m_intStatus = -2222
                        m_strMessage = "Kein Fahrzeug gefunden."
                    Case "NO_IMP_DATA"
                        m_intStatus = -3333
                        m_strMessage = "geben Sie ein Suchkriterium ein"
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If
                con.Close()
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region


End Class

'*************************************************************************
'$History: fw_01.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 20.08.08   Time: 11:53
' Updated in $/CKAG/Applications/appfw/Lib
' ITA 2141 fertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 6.05.08    Time: 13:39
' Created in $/CKAG/Applications/appfw/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 6.05.08    Time: 12:49
' Created in $/CKAG/Applications/appfw/appfw/Lib
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 14.04.08   Time: 10:11
' Updated in $/CKG/Applications/AppFW/AppFWWeb/Lib
' ITA 1843
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 11.04.08   Time: 15:59
' Updated in $/CKG/Applications/AppFW/AppFWWeb/Lib
' ita 1843
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 11.04.08   Time: 14:47
' Updated in $/CKG/Applications/AppFW/AppFWWeb/Lib
' ITA 1843
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 11.04.08   Time: 13:59
' Created in $/CKG/Applications/AppFW/AppFWWeb/Lib
'
'*************************************************************************
