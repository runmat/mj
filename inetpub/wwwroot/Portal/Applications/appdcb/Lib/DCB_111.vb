Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel

Public Class DCB_111
    REM § Status-Report, Kunde: ECAN, BAPI: Z_M_ABM_FEHLENDE_UNTERL_001,
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

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "DCB_111.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True

            'Dim intID As Int32 = -1
            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If

            Try

                S.AP.InitExecute("Z_M_ABM_KLAERF_TEMP_VERS", "I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                'Dim m_strKUNNR As String

                'm_strKUNNR = Right("0000000000" & m_objUser.KUNNR, 10)


                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ABM_KLAERF_TEMP_VERS", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR", m_strKUNNR)

                'myProxy.callBapi()

                Dim tblTemp As DataTable = S.AP.GetExportTable("GT_DATEN") 'myProxy.getExportTable("GT_DATEN")
                Dim tblTempText As DataTable = S.AP.GetExportTable("GT_TEXTE") 'myProxy.getExportTable("GT_TEXTE")

                CreateOutPut(tblTemp, strAppID)

                m_tblResult.Columns.Add("Langtext", GetType(System.String))

                'Texte über Chassis_Nummer zuordnen...
                Dim row As DataRow
                Dim rows As DataRow()
                Dim i As Integer
                Dim strChassis As String
                Dim strText As String
                Dim strGrund As String

                For Each row In m_tblResult.Rows
                    strText = String.Empty
                    'Klartext...
                    strChassis = CType(row("Fahrgestellnummer"), String)
                    rows = tblTempText.Select("CHASSIS_NUM = '" & strChassis & "'")
                    If (rows.Length > 0) Then
                        For i = 0 To rows.Length - 1
                            strText &= CType(rows(i)("TDLINE"), String) & "<br>"
                        Next
                    End If
                    row("Langtext") = strText

                    'Versandgrund im Klartext...
                    strGrund = CType(row("Versandgrund"), String)
                    If IsDate(row("Eingangsdatum").ToString) Then
                        row("Eingangsdatum") = Format(row("Eingangsdatum").ToString, "Short Date")
                    End If


                    Select Case strGrund
                        Case "001"
                            row("Versandgrund") = "Ummeldung"
                        Case "002"
                            row("Versandgrund") = "Umkennzeichnung"
                        Case "003"
                            row("Versandgrund") = "Techn. Eintragung"
                        Case "004"
                            row("Versandgrund") = "Abmeldung"
                        Case "005"
                            row("Versandgrund") = "Zulassung"
                        Case "006"
                            row("Versandgrund") = "Ersatzfahrzeugschein"
                        Case "007"
                            row("Versandgrund") = "vorüberg. Stillegung"
                        Case "008"
                            row("Versandgrund") = "sonstiges"
                        Case Else
                            row("Versandgrund") = "k.A."
                    End Select
                    m_tblResult.AcceptChanges()
                Next

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

            Catch ex As Exception
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_intStatus = -1111
                        m_strMessage = "keine Ergebnisse gefunden"
                    Case Else
                        m_intStatus = -2222
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
' $History: DCB_111.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 4.02.10    Time: 13:56
' Updated in $/CKAG/Applications/appdcb/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 8:39
' Updated in $/CKAG/Applications/appdcb/Lib
' ITA 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 11:52
' Created in $/CKAG/Applications/appdcb/Lib
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 16:50
' Updated in $/CKG/Applications/AppDCB/AppDCBWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 7.03.07    Time: 9:53
' Updated in $/CKG/Applications/AppDCB/AppDCBWeb/Lib
' 
' ************************************************
