Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Common
Imports CKG.Base.Kernel

Namespace Business
    Public Class PDIListe
        Inherits DatenimportBase

        Private m_pdis As DataTable

        ReadOnly Property PPDIs() As DataTable
            Get
                Return m_pdis
            End Get
        End Property

        Public Sub New(ByRef objUser As Kernel.Security.User, ByVal objApp As Kernel.Security.App)
            MyBase.New(objUser, objApp, "")
        End Sub

        Public Sub getPDIs(ByVal strAppID As String, ByVal strSessionID As String)
            m_strClassAndMethod = "PDIListe.getPDIs"
            m_strAppID = strAppID
            m_strSessionID = strSessionID
            If Not m_blnGestartet Then
                m_blnGestartet = True

                'Dim intID As Int32 = -1

                Try

                    'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxyNoPage("Z_M_Pdi_Liste", m_objApp, m_objUser)

                    'intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Pdi_Liste", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

                    'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                    'myProxy.callBapiNoPage(m_strAppID, m_strSessionID)

                    S.AP.InitExecute("Z_M_Pdi_Liste", "I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                    'm_pdis = myProxy.getExportTable("GT_WEB")
                    m_pdis = S.AP.GetExportTable("GT_WEB")

                    m_pdis.DefaultView.Sort = "NAME1 ASC"
                    m_pdis = m_pdis.DefaultView.ToTable

                    'If intID > -1 Then
                    '    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                    'End If

                    WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_pdis, False)
                Catch ex As Exception
                    m_intStatus = -9999
                    Select Case ex.Message
                        Case "NO_DATA"
                            m_strMessage = "Keine Eingabedaten vorhanden."
                        Case Else
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                    End Select
                    'If intID > -1 Then
                    '    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                    'End If
                    WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_pdis, False)
                Finally
                    'If intID > -1 Then
                    '    m_objLogApp.WriteStandardDataAccessSAP(intID)
                    'End If

                    m_blnGestartet = False
                End Try
            End If
        End Sub
    End Class
End Namespace

' ************************************************
' $History: PDIListe.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Business
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 15:39
' Updated in $/CKG/Base/Base/Business
' Verbindung ASPX-Logging mit BAPI-Logging
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