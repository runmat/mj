Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class MDR_01
    REM § Status-Report, Kunde: MDR, BAPI: Z_Dad_Cs_Mdr_Kfz_Ohne_Equi,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase

#Region " Declarations"

#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub


    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "MDR_01.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DAD_CS_MDR_KFZ_OHNE_EQUI", m_objApp, m_objUser, page)

                'myProxy.callBapi()

                S.AP.InitExecute("Z_DAD_CS_MDR_KFZ_OHNE_EQUI")

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_DATEN_OHNE_EQUI") 'myProxy.getExportTable("GT_DATEN_OHNE_EQUI")

                CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception

                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_intStatus = -1111
                        m_strMessage = "Keine Fahrzeuge gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select

                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region

End Class

' ************************************************
' $History: MDR_01.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 26.06.09   Time: 10:33
' Updated in $/CKAG/Applications/appffd/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Lib
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 2  *****************
' User: Uha          Date: 8.08.07    Time: 17:23
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Bugfixing "Daten ohne Brief"
' 
' *****************  Version 1  *****************
' User: Uha          Date: 8.08.07    Time: 13:48
' Created in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Daten ohne Brief
' 
' ************************************************
