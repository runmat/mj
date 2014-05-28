Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class fin_02
    REM § Status-Report, Kunde: Übergreifend, BAPI: Z_M_Unb_Haendler,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"

    Private m_strFilename2 As String

#End Region

#Region " Properties"
    Public ReadOnly Property FileName() As String
        Get
            Return m_strFilename2
        End Get
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
        m_strFilename2 = strFilename
    End Sub

    'Public Overloads Overrides Sub Fill()
    '    Fill(m_strAppID, m_strSessionID)
    'End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "fin_02.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                S.AP.InitExecute("Z_M_UNB_HAENDLER", "I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                Dim tblTemp As DataTable = S.AP.GetExportTable("GT_WEB")

                tblTemp.Columns.Add("Finanzierungsart", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("LoeschFlag", System.Type.GetType("System.String"))

                Dim rowTemp As DataRow
                For Each rowTemp In tblTemp.Rows
                    If Not TypeOf rowTemp("Zzfinart") Is System.DBNull Then
                        Select Case CStr(rowTemp("Zzfinart"))
                            Case "30"
                                rowTemp("Finanzierungsart") = "LZT"
                            Case "31"
                                rowTemp("Finanzierungsart") = "HEV"
                            Case "32"
                                rowTemp("Finanzierungsart") = "WHS"
                        End Select
                    End If
                    rowTemp("LoeschFlag") = ""
                Next
                CreateOutPut(tblTemp, m_strAppID)
                ResultTable = Result

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NOT_FOUND"
                        m_intStatus = -1111
                        m_strMessage = "Keine Ergebnisse zu den Kriterien gefunden."
                    Case "NO_DATA"
                        m_intStatus = -12
                        m_strMessage = "Keine unbekannten Händler gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "fin_02.Change"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                S.AP.Init("Z_DPM_SET_LOEVM_CARIMPORT_001", "I_AG, I_USER_LOE",
                          Right("0000000000" & m_objUser.KUNNR, 10), Left(m_objUser.UserName, 31))

                Dim tblTemp As DataTable = S.AP.GetImportTable("GT_ZB2")

                For Each dRow As DataRow In ResultTable.Select("Loeschflag='X'")
                    Dim NewRow As DataRow = tblTemp.NewRow
                    NewRow("TIDNR") = dRow("NUMMERZBII").ToString
                    tblTemp.Rows.Add(NewRow)
                Next

                S.AP.Execute()

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NOT_FOUND"
                        m_intStatus = -1111
                        m_strMessage = "Keine Ergebnisse zu den Kriterien gefunden."
                    Case "NO_DATA"
                        m_intStatus = -12
                        m_strMessage = "Keine unbekannten Händler gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region

End Class

' ************************************************
' $History: fin_02.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 2.03.10    Time: 14:35
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA: 3509, 3515, 3522
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 16.03.09   Time: 16:55
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' autorisierungsänderung/Berichtung RTFS
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 21.08.08   Time: 10:29
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2116 fertig
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance/Lib
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 6.03.08    Time: 15:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Eigener Finance Proxy Fertigstellung
' 
' *****************  Version 3  *****************
' User: Uha          Date: 13.12.07   Time: 16:29
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1470/1502 (Unbekannter Händler) - Spaltenübersetzungen hinzugefügt
' 
' *****************  Version 2  *****************
' User: Uha          Date: 12.12.07   Time: 10:20
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Kosmetik im Bereich Finance
' 
' *****************  Version 1  *****************
' User: Uha          Date: 11.12.07   Time: 15:47
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1470 bzw. 1473/1497 ASPX-Seite und Lib hinzugefügt
' 
' ************************************************
