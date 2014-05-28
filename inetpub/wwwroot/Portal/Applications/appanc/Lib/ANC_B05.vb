Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel


Public Class ANC_B05
    REM § Status-Report, Kunde: ANC, BAPI: Z_M_Fahrzeugbestand_Vanguard,
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

    'Public Overloads Overrides Sub Fill()
    '    Fill(m_strAppID, m_strSessionID, m_strHaendlerNr, m_datAbmeldedatumVon, m_datAbmeldedatumBis)
    'End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "ANC_B05.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        Dim flag As String = ""

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_ANC.SAPProxy_ANC()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim intID As Int32 = -1

            Try
                Dim SAPTable As New SAPProxy_ANC.ZDAD_S_FAHRZEUGBEST_VANGUARDTable()

                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Fahrzeugbestand_Vanguard", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Fahrzeugbestand_Vanguard(Right("0000000000" & m_objUser.KUNNR, 10), SAPTable)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp2 As DataTable = SAPTable.ToADODataTable
                Dim rowTemp As DataRow
                tblTemp2.AcceptChanges()
                Dim strTemp As String = ""
                For Each rowTemp In tblTemp2.Rows
                    Select Case CStr(rowTemp("ABCKZ"))
                        Case "1"
                            strTemp = "temp."
                        Case "2"
                            strTemp = "endg."
                        Case Else
                            strTemp = "-"
                    End Select
                    rowTemp("ABCKZ") = strTemp

                    'Ersetze Bindestrich durch Leerzeichen in Kfz-Kennzeichen
                    If Not TypeOf rowTemp("LICENSE_NUM") Is System.DBNull Then
                        rowTemp("LICENSE_NUM") = Replace(CStr(rowTemp("LICENSE_NUM")), "-", "")
                    End If
                Next
                tblTemp2.AcceptChanges()

                CreateOutPut(tblTemp2, strAppID)

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_DATA"
                        m_intStatus = -3332
                        m_strMessage = "Keine Daten gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                If intID > -1 Then
                    m_objlogApp.WriteStandardDataAccessSAP(intID)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: ANC_B05.vb $
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
' *****************  Version 6  *****************
' User: Uha          Date: 20.08.07   Time: 15:17
' Updated in $/CKG/Applications/AppANC/AppANCWeb/Lib
' ITA 1250: In Kennzeichen werden die Bindestriche nicht durch
' Leerzeichen ersetzt, sondern ersatzlos gelöscht.
' 
' *****************  Version 5  *****************
' User: Uha          Date: 20.08.07   Time: 13:30
' Updated in $/CKG/Applications/AppANC/AppANCWeb/Lib
' ITA 1250 Testversion
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 15:47
' Updated in $/CKG/Applications/AppANC/AppANCWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 6.03.07    Time: 13:45
' Updated in $/CKG/Applications/AppANC/AppANCWeb/Lib
' 
' ************************************************
