Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Public Class Sixt_B07
    REM § Status-Report, Kunde: Sixt, BAPI: Z_M_Abm_Abgemeldete_Kfz,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_datAbmeldedatumVon As DateTime
    Private m_datAbmeldedatumBis As DateTime
    Private m_strFahrgestellnummer As String
    Private m_strKennzeichen As String
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
                                ByVal datAbmeldedatumVon As DateTime, ByVal datAbmeldedatumBis As DateTime, _
                                ByVal strKennzeichen As String, ByVal strFahrgestellnummer As String)
        m_strClassAndMethod = "Sixt_B07.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1

            Try
                Dim strAnzahl As String = ""
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


                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ABM_ABGEMELDETE_KFZ", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                'myProxy.setImportParameter("ZFAHRG", strFahrgestellnummer)
                'myProxy.setImportParameter("ZZKENN", strKennzeichen)
                'If strDatTempVon.Length > 0 Then
                '    myProxy.setImportParameter("ABMDATAB", strDatTempVon)
                'End If
                'If strDatTempBis.Length > 0 Then
                '    myProxy.setImportParameter("ABMDATBI", strDatTempVon)
                'End If

                'myProxy.callBapi()

                S.AP.Init("Z_M_ABM_ABGEMELDETE_KFZ", "KUNNR, ZFAHRG, ZZKENN", Right("0000000000" & m_objUser.KUNNR, 10), strFahrgestellnummer, strKennzeichen)

                If strDatTempVon.Length > 0 Then
                    S.AP.SetImportParameter("ABMDATAB", strDatTempVon)
                End If
                If strDatTempBis.Length > 0 Then
                    S.AP.SetImportParameter("ABMDATBI", strDatTempVon)
                End If

                S.AP.Execute()

                'strAnzahl = myProxy.getExportParameter("ANZAHL")

                'Dim tblTemp2 As DataTable = myProxy.getExportTable("AUSGABE")

                strAnzahl = S.AP.GetExportParameter("ANZAHL")

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("AUSGABE")

                CreateOutPut(tblTemp2, strAppID)

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", TIDNR=, ZFAHRG=" & strFahrgestellnummer & ", ZZKENN=" & strKennzeichen & ", ABMDATAB=" & datAbmeldedatumVon.ToShortDateString & ", ABMDATBI=" & datAbmeldedatumBis.ToShortDateString, m_tblResult, False)


            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", TIDNR=, ZFAHRG=" & strFahrgestellnummer & ", ZZKENN=" & strKennzeichen & ", ABMDATAB=" & datAbmeldedatumVon.ToShortDateString & ", ABMDATBI=" & datAbmeldedatumBis.ToShortDateString & ", " & ex.Message, m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: Sixt_B07.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 16.02.10   Time: 11:40
' Updated in $/CKAG/Applications/appsixt/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 10:16
' Updated in $/CKAG/Applications/appsixt/Lib
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:37
' Created in $/CKAG/Applications/appsixt/Lib
' 
' *****************  Version 6  *****************
' User: Uha          Date: 3.07.07    Time: 9:25
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 5  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' 
' ************************************************
