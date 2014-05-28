Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class FMS_01
    REM § Status-Report, Kunde: FMS, BAPI: Z_M_Abm_Abgemeldete_Kfz,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"

#End Region

#Region " Properties"

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal page As Web.UI.Page, ByVal strAppID As String, ByVal strSessionID As String, ByVal datAbmeldedatumVon As DateTime, ByVal datAbmeldedatumBis As DateTime)
        m_strClassAndMethod = "FMS_01.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                Dim strDatTempVon As String
                If IsDate(datAbmeldedatumVon) Then
                    strDatTempVon = datAbmeldedatumVon.ToShortDateString
                Else
                    strDatTempVon = ""
                End If
                Dim strDatTempBis As String
                If IsDate(datAbmeldedatumBis) Then
                    strDatTempBis = datAbmeldedatumBis.ToShortDateString
                Else
                    strDatTempBis = ""
                End If

                S.AP.Init("Z_M_ABM_ABGEMELDETE_KFZ", "KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                If strDatTempVon.Length > 0 Then
                    S.AP.SetImportParameter("ABMDATAB", strDatTempVon)
                End If
                If strDatTempBis.Length > 0 Then
                    S.AP.SetImportParameter("ABMDATBI", strDatTempBis)
                End If

                S.AP.Execute()

                Dim strAnzahl As String = S.AP.GetExportParameter("ANZAHL")

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("AUSGABE")

                CreateOutPut(tblTemp2, strAppID)

            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            Finally
                m_blnGestartet = False
            End Try
        End If

    End Sub

#End Region

End Class

' ************************************************
' $History: FMS_01.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 8.03.10    Time: 16:29
' Updated in $/CKAG/Applications/appfms/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 15:58
' Updated in $/CKAG/Applications/appfms/Lib
' ITA 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:44
' Created in $/CKAG/Applications/appfms/Lib
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 17:52
' Updated in $/CKG/Applications/AppFMS/AppFMSWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 7.03.07    Time: 13:11
' Updated in $/CKG/Applications/AppFMS/AppFMSWeb/Lib
' 
' ******************************************
