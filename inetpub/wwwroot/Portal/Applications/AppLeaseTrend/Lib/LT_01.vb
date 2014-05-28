Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class LT_01
    REM § Status-Report, Kunde: Sixt, BAPI: Z_M_Meldungen_Fahrzeuge_Fibu,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits DatenimportBase ' FFD_Bank_Datenimport

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal datErfassungsdatumVon As DateTime, ByVal datErfassungsdatumBis As DateTime, ByVal page As Page)
        m_strClassAndMethod = "LT_01.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try
            Dim strDatErfVon As String
            If IsDate(datErfassungsdatumVon) Then
                strDatErfVon = datErfassungsdatumVon.ToShortDateString
            Else
                strDatErfVon = ""
            End If
            Dim strDatErfBis As String
            If IsDate(datErfassungsdatumBis) Then
                strDatErfBis = datErfassungsdatumBis.ToShortDateString
            Else
                strDatErfBis = ""
            End If

            S.AP.Init("Z_M_Briefersteingang_Leasetr")

            S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            S.AP.SetImportParameter("I_ERDAT_LOW", strDatErfVon)
            S.AP.SetImportParameter("I_ERDAT_HIGH", strDatErfBis)
            S.AP.SetImportParameter("I_KONZS_ZK", m_objUser.Reference)

            S.AP.Execute()

            Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB")

            CreateOutPut(tblTemp2, strAppID)
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", ERDAT_LOW=" & datErfassungsdatumVon.ToShortDateString & ", ERDAT_HIGH=" & datErfassungsdatumBis.ToShortDateString, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_FLEET"
                    m_strMessage = "Keine Fleet Daten vorhanden."
                Case "NO_WEB"
                    m_strMessage = "Keine Web-Tabelle erstellt."
                Case "NO_DATA"
                    m_strMessage = "Keine Eingabedaten gefunden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ERDAT_LOW=" & datErfassungsdatumVon.ToShortDateString & ", ERDAT_HIGH=" & datErfassungsdatumBis.ToShortDateString & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

End Class

' ************************************************
' $History: LT_01.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 30.06.09   Time: 14:58
' Updated in $/CKAG/Applications/AppLeaseTrend/Lib
' ITA 2918  Z_M_FEHLENDE_VERTRAGSNUMMERN, Z_M_BRIEFLEBENSLAUF_LT,
' Z_M_BRIEFERSTEINGANG_LEASETR, Z_M_BRIEFANFORDERUNG_ALLG,
' Z_M_UNANGEFORDERT_L, Z_M_KUNDENDATEN_LT, Z_M_VERSENDETE_ZB2_ENDG_LT,
' Z_M_VERSENDETE_ZB2_TEMP_LT, Z_M_BRIEF_TEMP_VERS_MAHN_002,
' Z_M_DATEN_OHNE_ZB2_LT
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 15:07
' Created in $/CKAG/Applications/AppLeaseTrend/Lib
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 18:14
' Updated in $/CKG/Applications/AppLeaseTrend/AppLeaseTrendWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 8.03.07    Time: 10:46
' Updated in $/CKG/Applications/AppLeaseTrend/AppLeaseTrendWeb/Lib
' 
' ************************************************
