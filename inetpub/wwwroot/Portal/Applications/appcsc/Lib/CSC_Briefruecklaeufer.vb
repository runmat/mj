Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class CSC_Briefruecklaeufer
    REM § Lese-/Schreibfunktion, Kunde: CSC, 
    REM § Show - BAPI: Zz_Csc_Daten_Ueb_Report_001,
    REM § Change - BAPI: Zz_Csc_Daten_Ueb_Change_001.

    Inherits BankBase ' FDD_Bank_Base

#Region " Declarations"

#End Region

#Region " Properties"

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        m_blnGestartet = False
    End Sub

    Public Overrides Sub Show()

    End Sub

    Public Overrides Sub Change()

    End Sub

    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "CSC_Briefruecklaeufer.Show"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)
            Dim CarDetailTable As DataTable

            m_tblResult = New DataTable()
            With m_tblResult.Columns
                .Add("Equipment", System.Type.GetType("System.String"))
                .Add("Vertragsnummer", System.Type.GetType("System.String"))
                .Add("VertragsnummerNeu", System.Type.GetType("System.String"))
                .Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                .Add("Briefnummer", System.Type.GetType("System.String"))
                .Add("Kennzeichen", System.Type.GetType("System.String"))
                .Add("DatumAenderung", System.Type.GetType("System.String"))
                .Add("Fehler", System.Type.GetType("System.String"))
                .Add("Fehlertext", System.Type.GetType("System.String"))
                .Add("ActionNOTHING", System.Type.GetType("System.Boolean"))
                .Add("ActionDELE", System.Type.GetType("System.Boolean"))
                .Add("ActionVERS", System.Type.GetType("System.Boolean"))
                .Add("ActionCHAN", System.Type.GetType("System.Boolean"))
                .Add("Bemerkung", System.Type.GetType("System.String"))
                .Add("Action", System.Type.GetType("System.String"))
            End With

            Try
                S.AP.InitExecute("Z_M_Briefruecklaeufer", "I_KUNNR", strKUNNR)

                If m_blnZeigeGesperrt Then
                    CarDetailTable = S.AP.GetExportTable("GT_WEB_DATENOHNEKB")
                Else
                    CarDetailTable = S.AP.GetExportTable("GT_WEB_KBOHNEDATEN")
                End If

                If CarDetailTable.Rows.Count > 0 Then
                    Dim rowNew As DataRow
                    Dim row As DataRow

                    For Each row In CarDetailTable.Rows

                        rowNew = m_tblResult.NewRow
                        rowNew("Equipment") = CType(row("EQUNR"), String)
                        rowNew("Vertragsnummer") = CType(row("LIZNR"), String)
                        rowNew("Fahrgestellnummer") = CType(row("CHASSIS_NUM"), String)
                        rowNew("Briefnummer") = CType(row("TIDNR"), String)
                        rowNew("Kennzeichen") = CType(row("LICENSE_NUM"), String)

                        rowNew("DatumAenderung") = CDate(CType(row("ERDAT"), String)).ToShortDateString

                        If rowNew("DatumAenderung").ToString = "01.01.1900" Then
                            rowNew("DatumAenderung") = String.Empty
                        End If
                        rowNew("Fehler") = CType(row("FEHLER"), String)
                        rowNew("Fehlertext") = CType(row("FEHLERTEXT"), String)
                        rowNew("Bemerkung") = ""
                        rowNew("VertragsnummerNeu") = ""
                        rowNew("Action") = ""
                        rowNew("ActionNOTHING") = True
                        rowNew("ActionDELE") = False
                        rowNew("ActionVERS") = False
                        rowNew("ActionCHAN") = False
                        m_tblResult.Rows.Add(rowNew)

                    Next

                    m_tblResultExcel = New DataTable()

                    With m_tblResultExcel.Columns
                        '§§§ JVE 20.09.2006: Equi-Nr. raus!
                        '.Add("Equipment", System.Type.GetType("System.String"))
                        .Add("Kontonummer", System.Type.GetType("System.String"))
                        .Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                        .Add("Briefnummer", System.Type.GetType("System.String"))
                        .Add("Kennzeichen", System.Type.GetType("System.String"))
                        .Add("Datum WV-Liste", System.Type.GetType("System.String"))
                        '§§§ JVE 20.09.2006: Status rein!
                        .Add("Status", System.Type.GetType("System.String"))
                    End With

                    'Dim i As Int32
                    For Each row In m_tblResult.Rows
                        rowNew = m_tblResultExcel.NewRow
                        '§§§ JVE 20.09.2006: Equi-Nr. raus!
                        'rowNew("Equipment") = CType(row("EQUNR"), String)
                        rowNew("Kontonummer") = CType(row("Vertragsnummer"), String)
                        rowNew("Fahrgestellnummer") = CType(row("Fahrgestellnummer"), String)
                        rowNew("Briefnummer") = CType(row("Briefnummer"), String)
                        rowNew("Kennzeichen") = CType(row("Kennzeichen"), String)
                        rowNew("Datum WV-Liste") = CType(row("DatumAenderung"), String)
                        'Seidel, 26.10.2004: Verhindert Anzeige des Datensatzes in Excel, wenn Kontonummer leer:
                        If Not CStr(rowNew("Kontonummer")) = String.Empty Then
                            m_tblResultExcel.Rows.Add(rowNew)
                        End If
                        '§§§ JVE 20.09.2006: Status rein!
                        rowNew("Status") = CType(row("Fehlertext"), String)
                    Next

                    m_strExcelFileName = Format(Now, "yyyyMMdd_HHmmss_") & m_objUser.UserName & ".xls"
                    m_strFileName = m_strExcelFileName
                    Try
                        Excel.ExcelExport.WriteExcel(m_tblResultExcel, m_objApp.ExcelPath & m_strExcelFileName)
                    Catch
                    End Try
                Else
                    m_intStatus = -2202
                    m_strMessage = "Keine Vorgangsinformationen vorhanden."
                End If

            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace("Execution failed", "").Trim()
                    Case "NO_FORD"
                        m_intStatus = -2201
                        m_strMessage = "Falsche Kundennummer."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "CSC_Briefkopien.Change"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)
            Dim tmpDataView As DataView
            tmpDataView = m_tblResult.DefaultView

            tmpDataView.RowFilter = "ActionNOTHING = 0"

            For i As Integer = 0 To tmpDataView.Count - 1
                Dim strBemerkung As String = "Vorgang erfolgreich"
                Dim strActionText As String = ""
                Try
                    Dim strAction As String = ""
                    If CType(tmpDataView.Item(i)("ActionDELE"), Boolean) Then
                        strAction = "DEL"
                        strActionText = "Löschen"
                    End If
                    If CType(tmpDataView.Item(i)("ActionVERS"), Boolean) Then
                        strAction = "VER"
                        strActionText = "Versenden"
                    End If
                    If CType(tmpDataView.Item(i)("ActionCHAN"), Boolean) Then
                        strAction = "AEN"
                        strActionText = "Ändern Kto.Nr.<br>" & CType(tmpDataView.Item(i)("VertragsnummerNeu"), String)
                    End If

                    Dim strCHASSIS_NUM As String = CType(tmpDataView.Item(i)("Fahrgestellnummer"), String)
                    Dim strLIZNR As String = CType(tmpDataView.Item(i)("Vertragsnummer"), String)
                    Dim strLIZNRNew As String = CType(tmpDataView.Item(i)("VertragsnummerNeu"), String)
                    Dim strBRIEFNR As String = CType(tmpDataView.Item(i)("Briefnummer"), String)
                    Dim strKENNZ As String = CType(tmpDataView.Item(i)("Kennzeichen"), String)
                    Dim strEQU As String = CType(tmpDataView.Item(i)("Equipment"), String)
                    Dim strERDAT As String = CType(tmpDataView.Item(i)("DatumAenderung"), String)
                    Dim strFEHLER As String = CType(tmpDataView.Item(i)("Fehler"), String)

                    S.AP.Init("Z_M_Briefruecklaeufer_Aendern")

                    S.AP.SetImportParameter("I_KUNNR", strKUNNR)
                    S.AP.SetImportParameter("I_CHASSIS_NUM", strCHASSIS_NUM)
                    S.AP.SetImportParameter("I_LIZNR", strLIZNR)
                    S.AP.SetImportParameter("I_EQUNR", strEQU)
                    S.AP.SetImportParameter("I_TIDNR", strBRIEFNR)
                    S.AP.SetImportParameter("I_LICENSE_NUM", strKENNZ)
                    S.AP.SetImportParameter("I_ERDAT", strERDAT)
                    S.AP.SetImportParameter("I_FEHLER", strFEHLER)
                    S.AP.SetImportParameter("I_KENNZEICHEN", strAction)
                    S.AP.SetImportParameter("I_LIZNR_NEW", strLIZNRNew)

                    S.AP.Execute()

                Catch ex As Exception
                    Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace("Execution failed", "").Trim()
                        Case "NO_CHANGE"
                            m_intStatus = -2822
                            m_strMessage = "Ändern des Satzes war nicht möglich."
                            strBemerkung = m_strMessage
                        Case "NO_DELETE"
                            m_intStatus = -2823
                            m_strMessage = "Löschen des Satzes war nicht möglich."
                            strBemerkung = m_strMessage
                        Case "NO_FORD"
                            m_intStatus = -2824
                            m_strMessage = "Falsche Kundennummer."
                            strBemerkung = m_strMessage
                        Case "NO_ZCSCUEBER_005"
                            m_intStatus = -2825
                            m_strMessage = "Kein Eintrag in Tabelle."
                            strBemerkung = m_strMessage
                        Case "NO_UPDATE_EQUI"
                            m_intStatus = -2826
                            m_strMessage = "Equipment konnte nicht geändert werden."
                            strBemerkung = m_strMessage
                        Case "NO_AUFTRAG"
                            m_intStatus = -2827
                            m_strMessage = "Kein Auftrag angelegt."
                            strBemerkung = m_strMessage
                        Case "NO_ZDADVERSAND"
                            m_intStatus = -2828
                            m_strMessage = "Keine Einträge für Versanddatei."
                            strBemerkung = m_strMessage
                        Case "VERTRAG_VORHANDEN"
                            m_intStatus = -2829
                            m_strMessage = "Vertragsnummer bereits vorhanden."
                            strBemerkung = m_strMessage
                        Case "VERTRAG_UNGUELTIG"
                            m_intStatus = -2830
                            m_strMessage = "Vertragsnummer ungültig."
                            strBemerkung = m_strMessage
                        Case "NO_GESPERRT"
                            m_intStatus = -2831
                            m_strMessage = "Equipment gesperrt."
                            strBemerkung = m_strMessage
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = "Fehler beim Speichern."
                            strBemerkung = m_strMessage
                    End Select
                End Try

                m_tblResult.AcceptChanges()
                Dim tmpRows As DataRow()
                tmpRows = m_tblResult.Select("Vertragsnummer = '" & CType(tmpDataView.Item(i)("Vertragsnummer"), String) & "'")
                tmpRows(0).BeginEdit()
                tmpRows(0).Item("Action") = strActionText
                tmpRows(0).Item("Bemerkung") = strBemerkung
                tmpRows(0).EndEdit()
                m_tblResult.AcceptChanges()
            Next

        End If
    End Sub

#End Region

End Class

' ************************************************
' $History: CSC_Briefruecklaeufer.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.03.10   Time: 13:34
' Updated in $/CKAG/Applications/appcsc/Lib
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 2.05.08    Time: 10:37
' Updated in $/CKAG/Applications/appcsc/Lib
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 11:37
' Created in $/CKAG/Applications/appcsc/Lib
' 
' *****************  Version 8  *****************
' User: Uha          Date: 2.07.07    Time: 16:46
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 7  *****************
' User: Uha          Date: 7.03.07    Time: 9:28
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Lib
' 
' ************************************************
