Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Common
Imports CKG.Base.Kernel

Public Class Arval_01
    REM § Status-Report, Kunde: Arval, BAPI: Z_M_Abm_Abgemeldete_Kfz,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    'Private m_datAbmeldedatumVon As DateTime
    'Private m_datAbmeldedatumBis As DateTime
#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    'Public Overloads Overrides Sub Fill()
    '    Fill(m_strAppID, m_strSessionID, m_datAbmeldedatumVon, m_datAbmeldedatumBis)
    'End Sub
    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal datAbmeldedatumVon As DateTime, ByVal datAbmeldedatumBis As DateTime)
        m_strClassAndMethod = "Arval_01.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            'Dim intID As Int32 = -1

            Try
                S.AP.Init("Z_M_Abm_Abgemeldete_Kfz")
                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Abm_Abgemeldete_Kfz", m_objApp, m_objUser, Page)

                S.AP.SetImportParameter("KUNNR", Right("0000000000" & m_objUser.KUNNR, 10)) 'myProxy.setImportParameter("KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                If IsDate(datAbmeldedatumVon) Then
                    S.AP.SetImportParameter("PICKDATAB", CStr(datAbmeldedatumVon)) 'myProxy.setImportParameter("PICKDATAB", CStr(datAbmeldedatumVon))
                End If
                If IsDate(datAbmeldedatumBis) Then
                    S.AP.SetImportParameter("PICKDATBI", CStr(datAbmeldedatumBis)) 'myProxy.setImportParameter("PICKDATBI", CStr(datAbmeldedatumBis))
                End If
                S.AP.Execute()
                'myProxy.callBapi()

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("AUSGABE") 'myProxy.getExportTable("AUSGABE")
                CreateOutPut(tblTemp2, m_strAppID)
                WriteLogEntry(True, "ABMELDEDATAB=" & datAbmeldedatumVon.ToShortDateString & ", ABMELDEDATBI=" & datAbmeldedatumBis.ToShortDateString &
                              ", KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                WriteLogEntry(False, "ABMELDEDATAB=" & datAbmeldedatumVon.ToShortDateString & ", ABMELDEDATBI=" & datAbmeldedatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region

End Class

' ************************************************
' $History: Arval_01.vb $
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
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 16:18
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 6.03.07    Time: 14:48
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Lib
' 
' ************************************************
