Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Public Class ANC_B12
    REM � Status-Report, Kunde: Sixt, BAPI: Z_M_Brieflebenslauf_Tuete,
    REM � Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_strFahrgestellnummer As String
#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub
    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page, _
                            ByVal strFahrgestellnummer As String)
        m_strClassAndMethod = "ANC_B12.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            Dim strKUNNR As String = Right("0000000000" & m_objUser.KUNNR, 10)
            Dim strZZFAHRG As String = strFahrgestellnummer
            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BRIEFLEBENSLAUF_TUETE", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                myProxy.setImportParameter("I_VKORG", "1510")
                myProxy.setImportParameter("I_ZZFAHRG", strZZFAHRG)

                myProxy.callBapi()

                m_tblResult = myProxy.getExportTable("GT_WEB")

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", ZZFAHRG=" & strZZFAHRG, m_tblResult, False)

            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ZZFAHRG=" & strZZFAHRG & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)

            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
    
#End Region
End Class

' ************************************************
' $History: ANC_B12.vb $
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 11.03.10   Time: 12:55
' Updated in $/CKAG/Applications/appanc/Lib
' ITA: 2918
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:41
' Created in $/CKAG/Applications/appanc/Lib
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 15:47
' Updated in $/CKG/Applications/AppANC/AppANCWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 6.03.07    Time: 13:45
' Updated in $/CKG/Applications/AppANC/AppANCWeb/Lib
' 
' ************************************************
