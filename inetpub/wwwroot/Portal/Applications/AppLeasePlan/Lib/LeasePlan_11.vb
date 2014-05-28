Option Explicit On 
Option Strict On

Imports System
'Imports Microsoft.Data.SAPClient
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class LeasePlan_11
    REM § Status-Report, Kunde: LeasePlan, BAPI: Z_M_Briefeohnefzg2 (Fill),
    REM § BAPI: Z_M_Brieflebenslauf (FillHistory),
    REM § Ausgabetabelle per Zuordnung in Web-DB (Fill), Direkte Rückgabe (FillHistory).

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_strBriefnummer As String
    Private m_datEingangsdatumVon As DateTime
    Private m_datEingangsdatumBis As DateTime
    Private m_strFahrgestellnummer As String
    Private m_strHaendlerID As String
    Private m_tblBRIEFLEBENSLAUF_LPTable As DataTable
    Private m_tblQMEL_DATENTable As DataTable
    Private m_tblQMMIDATENTable As DataTable
    Private mGT_EQUI As DataTable
#End Region

#Region " Properties"
    Public ReadOnly Property QMMIDATENTable() As DataTable
        Get
            Return m_tblQMMIDATENTable
        End Get
    End Property

    Public ReadOnly Property QMEL_DATENTable() As DataTable
        Get
            Return m_tblQMEL_DATENTable
        End Get
    End Property


    Public ReadOnly Property diverseFahrzeuge() As DataTable
        Get
            Return mGT_EQUI
        End Get
    End Property

    Public ReadOnly Property BRIEFLEBENSLAUF_LPTable() As DataTable
        Get
            Return m_tblBRIEFLEBENSLAUF_LPTable
        End Get
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
        FILL(m_strAppID, m_strSessionID, m_strBriefnummer, m_datEingangsdatumVon, m_datEingangsdatumBis, m_strFahrgestellnummer, m_strHaendlerID)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal strBriefnummer As String,
                              ByVal datEingangsdatumVon As DateTime, ByVal datEingangsdatumBis As DateTime,
                              ByVal strFahrgestellnummer As String, ByVal strHaendlerID As String)
       
    End Sub

    Public Sub FillHistory(ByVal strAppID As String, ByVal strSessionID As String, ByVal strAmtlKennzeichen As String,
                           ByVal strFahrgestellnummer As String, ByVal strBriefnummer As String, ByVal strOrdernummer As String,
                           ByVal equnr As String)

        m_strClassAndMethod = "LeasePlan_11.FillHistory"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intStatus = 0

            Try
                S.AP.Init("Z_M_BRIEFLEBENSLAUF_LP")

                S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                S.AP.SetImportParameter("I_ZZKENN", strAmtlKennzeichen)
                S.AP.SetImportParameter("I_ZZFAHRG", strFahrgestellnummer)
                S.AP.SetImportParameter("I_ZZBRIEF", strBriefnummer)
                S.AP.SetImportParameter("I_ZZREF1", strOrdernummer)
                S.AP.SetImportParameter("I_EQUNR", equnr)

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If


                S.AP.Execute()

                m_tblQMEL_DATENTable = S.AP.GetExportTable("GT_QMEL")
                m_tblQMMIDATENTable = S.AP.GetExportTable("GT_QMMA")
                m_tblBRIEFLEBENSLAUF_LPTable = S.AP.GetExportTable("GT_WEB")
                mGT_EQUI = S.AP.GetExportTable("GT_EQUI")

                Dim row As DataRow

                For Each row In m_tblQMMIDATENTable.Rows
                    If Not TypeOf row("AEZEIT") Is DBNull Then
                        row("AEZEIT") = row("AEZEIT").ToString.TrimStart("0"c)
                        If row("AEZEIT").ToString <> String.Empty Then
                            row("AEZEIT") = Left(row("AEZEIT").ToString, 2) & ":" & row("AEZEIT").ToString.Substring(2, 2)
                        End If
                    End If
                Next

                'bearbeitung der EQUI Tabelle für die Ausgabe JJU20081127
                If Not mGT_EQUI Is Nothing Then
                    For Each tmprow As DataRow In mGT_EQUI.Rows
                        Select Case tmprow("ABCKZ").ToString
                            Case "1"
                                tmprow("ABCKZ") = "temporär versendet"
                            Case "2"
                                tmprow("ABCKZ") = "endgültig versendet"
                            Case Else
                                tmprow("ABCKZ") = "eingelagert"
                        End Select
                    Next
                    mGT_EQUI.AcceptChanges()
                    CreateOutPut(mGT_EQUI, strAppID)
                    mGT_EQUI = m_tblResult
                End If

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", ZZBRIEF=" & UCase(strBriefnummer) & ", ZFAHRG=" & UCase(strFahrgestellnummer) & ", ZZREF1=" & UCase(strOrdernummer) & ", ZZKENN=" & UCase(strAmtlKennzeichen), m_tblBRIEFLEBENSLAUF_LPTable, False)
            Catch ex As Exception
                m_intStatus = -2222
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_strMessage = "Keine Ergebnisse für die angegebenen Kriterien."
                    Case "NO_WEB"
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case Else
                        m_strMessage = ex.Message
                End Select

                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ZZBRIEF=" & UCase(strBriefnummer) & ", ZFAHRG=" & UCase(strFahrgestellnummer) & ", ZZREF1=" & UCase(strOrdernummer) & ", ZZKENN=" & UCase(strAmtlKennzeichen) & ", " & Replace(m_strMessage, "<br>", " "), m_tblBRIEFLEBENSLAUF_LPTable, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region

End Class

' ************************************************
' $History: LeasePlan_11.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 24.02.09   Time: 15:31
' Updated in $/CKAG/Applications/AppLeasePlan/Lib
' fahrzeughistorien bugfix
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 16.02.09   Time: 17:29
' Updated in $/CKAG/Applications/AppLeasePlan/Lib
' ITA 2413
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 14:52
' Created in $/CKAG/Applications/AppLeasePlan/Lib
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 18:10
' Updated in $/CKG/Applications/AppLeasePlan/AppLeasePlanWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 5  *****************
' User: Uha          Date: 8.03.07    Time: 9:47
' Updated in $/CKG/Applications/AppLeasePlan/AppLeasePlanWeb/Lib
' 
' ************************************************
