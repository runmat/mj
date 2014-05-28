Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel

Public Class Report_200
    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal datAbmeldedatumVon As DateTime, ByVal datAbmeldedatumBis As DateTime)
        m_strClassAndMethod = "Report_200.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                S.AP.Init("Z_M_Abm_Abgemeldete_Kfz")
                S.AP.SetImportParameter("KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                S.AP.SetImportParameter("PICKDATAB", datAbmeldedatumVon)
                S.AP.SetImportParameter("PICKDATBI", datAbmeldedatumBis)
                S.AP.Execute()

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("AUSGABE")

                CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "ABMELDEDATAB=" & datAbmeldedatumVon.ToShortDateString & ", ABMELDEDATBI=" & datAbmeldedatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten."

                WriteLogEntry(False, "ABMELDEDATAB=" & datAbmeldedatumVon.ToShortDateString & ", ABMELDEDATBI=" & datAbmeldedatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region

End Class

' ************************************************
' $History: Report_200.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 14.10.08   Time: 15:41
' Created in $/CKAG/Components/ComCommon/Kernel
' ************************************************