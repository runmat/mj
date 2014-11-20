Option Explicit On 
Option Infer On
Option Strict On

Imports System
Imports CKG.Base.Kernel.Common
Imports CKG.Base.Common

Namespace Business
    Public Class TempMahn
        REM § Mahn-Report, Kunde: FFD, BAPI: Z_M_Temporaer_Zu_Mahnen,
        REM § Ausgabetabelle per Zuordnung in Web-DB.

        Inherits DatenimportBase

#Region " Declarations"

#End Region

#Region " Properties"

#End Region

#Region " Methods"
        Public Sub New(ByRef objUser As Kernel.Security.User, ByVal objApp As Kernel.Security.App, ByVal strFilename As String)
            MyBase.New(objUser, objApp, strFilename)
        End Sub

        Public Overloads Overrides Sub Fill()
            Fill(m_strAppID, m_strsessionid)
        End Sub

        Public Overloads Sub FILL(ByVal strAppID As String)
            m_strClassAndMethod = "TempMahn.FILL"
            m_strAppID = strAppID
            If Not m_blnGestartet Then
                m_blnGestartet = True


                Dim strKUNNR As String = m_objUser.Reference ' Right("0000000000" & m_objUser.KUNNR, 10)
                Dim strKNRZE As String = m_strFiliale
                Dim strKONZS As String = "0000324562"

                Try
                    Dim strVKORG As String = "1510"

                    Dim proxy = DynSapProxy.getProxy("Z_M_TEMPORAER_ZU_MAHNEN", m_objApp, m_objUser, PageHelper.GetCurrentPage())
                    proxy.setImportParameter("I_KNRZE", strKNRZE)
                    proxy.setImportParameter("I_KONZS", strKONZS)
                    proxy.setImportParameter("I_KUNNR", strKUNNR)
                    proxy.setImportParameter("I_VKORG", strVKORG)
                    proxy.callBapi()

                    Dim tblTemp2 As DataTable = proxy.getExportTable("GT_WEB")

                    Dim rowTemp As DataRow
                    For Each rowTemp In tblTemp2.Rows
                        Select Case CStr(rowTemp("ZZKKBER"))
                            Case "0001"
                                rowTemp("ZZKKBER") = "Standard temporär"
                            Case "0002"
                                rowTemp("ZZKKBER") = "Standard endgültig"
                        End Select
                    Next

                    CreateOutPut(tblTemp2, strAppID)
                    WriteLogEntry(True, "KNRZE=" & strKNRZE & ", KONZS=" & strKONZS & ", KUNNR=" & strKUNNR, m_tblResult, False)
                Catch ex As Exception
                    m_intStatus = -9999
                    Select Case ex.Message
                        Case "NO_DATA"
                            m_strMessage = "Keine Eingabedaten vorhanden."
                        Case "NO_WEB"
                            m_strMessage = "Keine Web-Tabelle erstellt."
                        Case "NO_HAENDLER"
                            m_strMessage = "Händler nicht vorhanden."
                        Case Else
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                    End Select
                    WriteLogEntry(False, "KNRZE=" & strKNRZE & ", KONZS=" & strKONZS & ", KUNNR=" & strKUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
                Finally
                    m_blnGestartet = False
                End Try
            End If
        End Sub
#End Region
    End Class
End Namespace

' ************************************************
' $History: TempMahn.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Business
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 15:39
' Updated in $/CKG/Base/Base/Business
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 22.05.07   Time: 9:31
' Updated in $/CKG/Base/Base/Business
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 3  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Business
' 
' ************************************************