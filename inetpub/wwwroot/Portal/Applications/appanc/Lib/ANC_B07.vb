Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Public Class ANC_B07
    REM § Status-Report, Kunde: Sixt, BAPI: Z_M_Abm_Abgemeldete_Kfz,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_datAbmeldedatumVon As DateTime
    Private m_datAbmeldedatumBis As DateTime
    Private m_strFahrgestellnummer As String
    Private m_strKennzeichen As String
    Private m_Page As System.Web.UI.Page
#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
        Fill(m_Page, m_strAppID, m_strSessionID, m_datAbmeldedatumVon, m_datAbmeldedatumBis, m_strKennzeichen, m_strFahrgestellnummer)
    End Sub

    Public Overloads Sub FILL(ByVal page As Web.UI.Page, ByVal strAppID As String, ByVal strSessionID As String, ByVal datAbmeldedatumVon As DateTime, ByVal datAbmeldedatumBis As DateTime, ByVal strKennzeichen As String, ByVal strFahrgestellnummer As String)

        m_strClassAndMethod = "ANC_B07.FILL"
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


                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ABM_ABGEMELDETE_KFZ", m_objApp, m_objUser, Page)

                myProxy.setImportParameter("KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                myProxy.setImportParameter("ZFAHRG", strFahrgestellnummer)
                myProxy.setImportParameter("ZZKENN", strKennzeichen)
                If strDatTempVon.Length > 0 Then
                    myProxy.setImportParameter("ABMDATAB", strDatTempVon)
                End If
                If strDatTempBis.Length > 0 Then
                    myProxy.setImportParameter("ABMDATBI", strDatTempVon)
                End If

                myProxy.callBapi()

                strAnzahl = myProxy.getExportParameter("ANZAHL")

                Dim tblTemp2 As DataTable = myProxy.getExportTable("AUSGABE")

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
' $History: ANC_B07.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 11.03.10   Time: 12:55
' Updated in $/CKAG/Applications/appanc/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 29.04.09   Time: 15:28
' Updated in $/CKAG/Applications/appanc/Lib
' Warnungen entfernt.
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:41
' Created in $/CKAG/Applications/appanc/Lib
' 
' *****************  Version 7  *****************
' User: Uha          Date: 20.08.07   Time: 15:17
' Updated in $/CKG/Applications/AppANC/AppANCWeb/Lib
' ITA 1250: In Kennzeichen werden die Bindestriche nicht durch
' Leerzeichen ersetzt, sondern ersatzlos gelöscht.
' 
' *****************  Version 6  *****************
' User: Uha          Date: 20.08.07   Time: 13:30
' Updated in $/CKG/Applications/AppANC/AppANCWeb/Lib
' ITA 1250 Testversion
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
