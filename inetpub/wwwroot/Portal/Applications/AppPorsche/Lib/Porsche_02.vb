Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Common

Public Class Porsche_02
    REM § Status-Report, Kunde: Sixt, BAPI: Z_M_Abm_Abgemeldete_Kfz,
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


    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "Porsche_02.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try
            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Daten_Ohne_Brief_Porsche", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            myProxy.setImportParameter("I_WEB_REPORT", "Report02.aspx")
            myProxy.callBapi()
          
            Dim tblTemp2 As DataTable = myProxy.getExportTable("GT_WEB")
            Dim row As DataRow
            Dim strFahrzeugklasse As String
            CreateOutPut(tblTemp2, strAppID)
            m_tblResult.Columns("Fahrzeugklasse").MaxLength = 30
            m_tblResult.AcceptChanges()
            For Each row In m_tblResult.Rows
                strFahrzeugklasse = CStr(row("Fahrzeugklasse"))
                Select Case strFahrzeugklasse
                    Case "10"
                        row("Fahrzeugklasse") = "Neuwagen (Kunde)"
                    Case "11"
                        row("Fahrzeugklasse") = "Neuwagen (Lager)"
                    Case "12"
                        row("Fahrzeugklasse") = "Vorführwagen"
                    Case "13"
                        row("Fahrzeugklasse") = "Gebrauchtwagen"
                    Case Else
                        row("Fahrzeugklasse") = "-unbekannt-"
                End Select
            Next

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -5555
                    m_strMessage = "Keine Daten gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        End Try
    End Sub


    'Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
    '    m_strClassAndMethod = "Porsche_02.FILL"
    '    m_strAppID = strAppID
    '    m_strSessionID = strSessionID
    '    If Not m_blnGestartet Then
    '        m_blnGestartet = True

    '        Dim objSAP As New SAPProxy_PORSCHE.SAPProxy_PORSCHE()
    '        objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
    '        objSAP.Connection.Open()

    '        Dim intID As Int32 = -1

    '        Try
    '            Dim SAPTable As New SAPProxy_PORSCHE.ZDAD_M_WEB_DATEN_O_BRIEFTable()

    '            intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Daten_Ohne_Brief_Porsche", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
    '            objSAP.Z_M_Daten_Ohne_Brief_Porsche(Right("0000000000" & m_objUser.KUNNR, 10), "Report02.aspx", SAPTable)

    '            objSAP.CommitWork()
    '            If intID > -1 Then
    '                m_objlogApp.WriteEndDataAccessSAP(intID, True)
    '            End If

    '            Dim tblTemp2 As DataTable = SAPTable.ToADODataTable
    '            Dim row As DataRow
    '            Dim strFahrzeugklasse As String

    '            CreateOutPut(tblTemp2, strAppID)

    '            For Each row In m_tblResult.Rows
    '                strFahrzeugklasse = CStr(row("Fahrzeugklasse"))
    '                Select Case strFahrzeugklasse
    '                    Case "10"
    '                        row("Fahrzeugklasse") = "Neuwagen (Kunde)"
    '                    Case "11"
    '                        row("Fahrzeugklasse") = "Neuwagen (Lager)"
    '                    Case "12"
    '                        row("Fahrzeugklasse") = "Vorführwagen"
    '                    Case "13"
    '                        row("Fahrzeugklasse") = "Gebrauchtwagen"
    '                    Case Else
    '                        row("Fahrzeugklasse") = "-unbekannt-"
    '                End Select
    '            Next

    '            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
    '        Catch ex As Exception
    '            Select Case ex.Message
    '                Case "NO_DATA"
    '                    m_intStatus = -5555
    '                    m_strMessage = "Keine Daten gefunden."
    '                Case Else
    '                    m_intStatus = -9999
    '                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
    '            End Select
    '            If intID > -1 Then
    '                m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
    '            End If
    '            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
    '        Finally
    '            If intID > -1 Then
    '                m_objlogApp.WriteStandardDataAccessSAP(intID)
    '            End If

    '            objSAP.Connection.Close()
    '            objSAP.Dispose()

    '            m_blnGestartet = False
    '        End Try
    '    End If
    'End Sub
#End Region
End Class

' ************************************************
' $History: Porsche_02.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 9.07.09    Time: 14:25
' Updated in $/CKAG/Applications/AppPorsche/Lib
' ITA 2918 nachbesserungen
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 3.07.09    Time: 15:37
' Updated in $/CKAG/Applications/AppPorsche/Lib
' ITA 2918 Z_M_BRIEFANFORDERUNG_PORSCHE, Z_M_BRIEF_OHNE_DATEN_PORSCHE,
' Z_M_CREDITLIMIT_CHANGE_PORSCHE, Z_M_CUST_GET_CHILDREN_PORSCHE,
' Z_M_DATEN_OHNE_BRIEF_PORSCHE, Z_M_FREIGEBEN_AUFTRAG_PORSCHE,
' Z_M_GESPERRT_AUFTRAG_PORSCHE, Z_M_HAENDLERBESTAND_PORSCHE
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 11:28
' Created in $/CKAG/Applications/AppPorsche/Lib
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 18:24
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 6  *****************
' User: Uha          Date: 5.03.07    Time: 12:50
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Lib
' 
' ************************************************