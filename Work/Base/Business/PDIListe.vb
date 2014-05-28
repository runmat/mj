Option Explicit On 
Option Strict On

Imports System

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

                Dim objSAP As New SAPProxy_Base.SAPProxy_Base()
                objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
                objSAP.Connection.Open()

                Dim intID As Int32 = -1

                Try
                    Dim SAPTable As New SAPProxy_Base.ZDAD_PDI_LISTETable()
                    Dim strVKORG As String = "1510"

                    intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Pdi_Liste", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                    objSAP.Z_M_Pdi_Liste(Right("0000000000" & m_objUser.KUNNR, 10), SAPTable)
                    objSAP.CommitWork()
                    If intID > -1 Then
                        m_objlogApp.WriteEndDataAccessSAP(intID, True)
                    End If
                    SAPTable.SortBy("NAME1", "ASC")
                    m_pdis = SAPTable.ToADODataTable

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

                    objSAP.Connection.Close()
                    objSAP.Dispose()

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