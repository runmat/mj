Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class SixtLease_10
    REM § Status-Report, Kunde: Arval, BAPI: Z_M_Abm_Fehl_Unterl_Sixt_Leas,
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

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, page As Page)
        m_strClassAndMethod = "SixtLease_10.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Abm_Fehl_Unterl_Sixt_Leas", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_Abm_Fehl_Unterl_Sixt_Leas", "KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                'Dim tblTemp2 As DataTable = myProxy.getExportTable("AUSGABE")
                Dim tblTemp2 As DataTable = S.AP.GetExportTable("AUSGABE")

                CreateOutPut(tblTemp2, strAppID)

                'Spalte "Status" einfügen...
                m_tblResult.Columns.Add("Status", GetType(System.String))
                'Spalte "Auswahl" einfügen
                m_tblResult.Columns.Add("Auswahl", GetType(System.String))
                m_tblResult.AcceptChanges()

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"

                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub Absenden(ByRef status As String, ByVal equnr As String, ByVal lvnr As String, byval page As Page)
        m_strClassAndMethod = "SixtLease_10.Absenden"
        Dim intStatus As String

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Eingabe_Lvnummer_Sixtleas", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("LF_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                'myProxy.setImportParameter("LF_EQUNR", equnr)
                'myProxy.setImportParameter("LF_LIZNR", lvnr)

                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_Eingabe_Lvnummer_Sixtleas", "LF_KUNNR, LF_EQUNR, LF_LIZNR", Right("0000000000" & m_objUser.KUNNR, 10), equnr, lvnr)

                'intStatus = myProxy.getExportParameter("LF_RETURN")
                intStatus = S.AP.GetExportParameter("LF_RETURN")

                Select Case intStatus
                    Case "0"
                        status = "Vorgang OK"
                    Case "5"
                        status = "FEHLER: LV-Nr. bereits vorhanden."
                    Case Else
                        status = "WARNUNG: Status unbekannt."
                End Select

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = "Beim Speichern ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                status = "Unbekannter Fehler!"
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub Absenden(ByRef webTable As DataTable, ByVal page As Page)
        m_strClassAndMethod = "SixtLease_10.Absenden"
        Dim tblReturnSAP As DataTable

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                Dim row As DataRow
                Dim rowWeb As DataRow
                Dim ImpTable As DataTable

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ABMELDUNG_SIXT_LEASING", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))

                S.AP.Init("Z_M_ABMELDUNG_SIXT_LEASING", "I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))

                'ImpTable = myProxy.getImportTable("I_AUF")
                ImpTable = S.AP.GetImportTable("I_AUF")

                'Übergabetabelle füllen******
                For Each row In webTable.Rows
                    If Not (TypeOf row("Auswahl") Is System.DBNull) AndAlso (CType(row("Auswahl"), String) = "X") Then

                        Dim NewRow As DataRow = ImpTable.NewRow()

                        NewRow("ZZKENN") = row("Kennzeichen").ToString()
                        NewRow("LIZNR") = row("Leasingvertragsnummer").ToString()
                        NewRow("TIDNR") = row("Briefnummer").ToString()
                        NewRow("CHASSIS_NUM") = row("Fahrgestellnummer").ToString()
                        NewRow("KZINDAT") = row("Abmeldeeingang").ToString()
                        NewRow("FREIGABE") = "X"

                        ImpTable.Rows.Add(NewRow)

                    End If
                Next

                'myProxy.callBapi()

                'Fehlerwerte füllen****
                'tblReturnSAP = myProxy.getExportTable("I_AUF")

                tblReturnSAP = S.AP.GetExportTableWithExecute("I_AUF")

                Dim errMsg As String

                For Each row In tblReturnSAP.Rows
                    rowWeb = webTable.Select("Fahrgestellnummer='" & CType(row("CHASSIS_NUM"), String) & "'")(0)

                    errMsg = CType(row("FEHL_AUF_ANL"), String)

                    Select Case errMsg
                        Case "1"
                            errMsg = "Fehler: Datenaktualisieung fehlgeschlagen."
                        Case "2"
                            errMsg = "Fehler: Brief bereits angefordert."
                        Case "3"
                            errMsg = "Fehler: Keine Daten gefunden."
                        Case "4"
                            errMsg = "Fehler: Kein Kundenauftrag angelegt."
                        Case "5"
                            errMsg = "Fehler: Brief bereits in Abmeldung."
                        Case "6"
                            errMsg = "Fehler: Datensatz gesperrt."
                        Case Else
                            errMsg = "Unbekannter Fehler."
                    End Select

                    If CType(row("FEHL_AUF_ANL"), String) = String.Empty Then
                        rowWeb("Status") = "Vorgang OK."
                    Else
                        rowWeb("Status") = errMsg
                    End If
                    rowWeb.AcceptChanges()
                Next

                '-------------------------------------------------------------
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"

                'Status = "Unbekannter Fehler!"
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally     
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: SixtLease_10.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 10:47
' Updated in $/CKAG/Applications/appsixtl/Lib
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:27
' Created in $/CKAG/Applications/appsixtl/Lib
' 
' *****************  Version 5  *****************
' User: Uha          Date: 3.07.07    Time: 9:34
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 8.03.07    Time: 13:42
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Lib
' 
' ************************************************
