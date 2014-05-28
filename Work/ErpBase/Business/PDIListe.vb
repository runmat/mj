Option Explicit On 
Option Infer On
Option Strict On

Imports System
Imports CKG.Base.Common
Imports CKG.Base.Kernel.Common

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

                Dim intID = -1

                Try
                    intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Pdi_Liste", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

                    Dim proxy = DynSapProxy.getProxy("Z_M_PDI_LISTE", m_objApp, m_objUser, PageHelper.GetCurrentPage())

                    proxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                    proxy.callBapi()

                    If intID > -1 Then
                        m_objLogApp.WriteEndDataAccessSAP(intID, True)
                    End If

                    Dim gt_web = proxy.getExportTable("GT_WEB")
                    gt_web.DefaultView.Sort = "NAME1"

                    m_pdis = gt_web.DefaultView.ToTable()

                    WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_pdis, False)
                Catch ex As Exception
                    m_intStatus = -9999
                    Select Case ex.Message
                        Case "NO_DATA"
                            m_strMessage = "Keine Eingabedaten vorhanden."
                        Case Else
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                    End Select
                    If intID > -1 Then
                        m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                    End If
                    WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_pdis, False)
                Finally
                    If intID > -1 Then
                        m_objlogApp.WriteStandardDataAccessSAP(intID)
                    End If

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