Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Common
Imports CKG.Base.Kernel

Public Class Arval_08
    REM § Status-Report, Kunde: Arval, BAPI: Z_M_UNZUGELASSENE_FZGE_ARVAL,
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

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "Arval_08.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            
            Try

                S.AP.InitExecute("Z_M_Unzugelassene_Fzge_Arval", "I_AG", Right("0000000000" & m_objUser.KUNNR, 10))

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Unzugelassene_Fzge_Arval", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))


                'myProxy.callBapi()

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("T_DATA") 'myProxy.getExportTable("T_DATA")

                CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception

                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "ERR_NO_DATA"
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
' $History: Arval_08.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 30.06.09   Time: 13:03
' Updated in $/CKAG/Applications/apparval/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/apparval/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:18
' Created in $/CKAG/Applications/apparval/Lib
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 16:18
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 5  *****************
' User: Uha          Date: 6.03.07    Time: 14:48
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Lib
' 
' ************************************************
