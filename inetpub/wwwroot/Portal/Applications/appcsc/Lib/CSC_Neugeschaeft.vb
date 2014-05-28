Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class CSC_Neugeschaeft
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

    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "CSC_Neugeschaeft.Show"
        If Not m_blnGestartet Then
            m_strExcelFileName = ""

            m_blnGestartet = True
            Dim CarDetail As DataRow
            Dim CarDetailTable As DataTable

            m_intIDSAP = -1

            m_tblResult = New DataTable()
            m_tblResult.Columns.Add("Kontonummer", System.Type.GetType("System.String"))
            m_tblResult.Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"))
            m_tblResult.Columns.Add("Briefnummer", System.Type.GetType("System.String"))
            m_tblResult.Columns.Add("Kennzeichen", System.Type.GetType("System.String"))
            m_tblResult.Columns.Add("Label", System.Type.GetType("System.String"))
            m_tblResult.Columns.Add("Modellbezeichnung", System.Type.GetType("System.String"))
            m_tblResult.Columns.Add("Erstellt", System.Type.GetType("System.DateTime"))
            m_tblResult.Columns.Add("Versendet", System.Type.GetType("System.DateTime"))
            m_tblResult.Columns.Add("NurDaten", System.Type.GetType("System.Boolean"))
            m_tblResult.Columns.Add("NurBrief", System.Type.GetType("System.Boolean"))
            m_tblResult.Columns.Add("KontonummerIdentisch", System.Type.GetType("System.Boolean"))
            m_tblResult.Columns.Add("KontonummerNeu", System.Type.GetType("System.String"))
            m_tblResult.Columns.Add("ActionNOTHING", System.Type.GetType("System.Boolean"))
            m_tblResult.Columns.Add("ActionDELE", System.Type.GetType("System.Boolean"))
            m_tblResult.Columns.Add("ActionVERS", System.Type.GetType("System.Boolean"))
            m_tblResult.Columns.Add("ActionCHAN", System.Type.GetType("System.Boolean"))
            m_tblResult.Columns.Add("Bemerkung", System.Type.GetType("System.String"))
            m_tblResult.Columns.Add("Action", System.Type.GetType("System.String"))

            m_intStatus = 0
            m_strMessage = ""

            Try
                S.AP.InitExecute("Zz_Csc_Daten_Ueb_Report_001", "I_KUNNR", m_strCustomer)

                CarDetailTable = S.AP.GetExportTable("GT_WEB")

                If CarDetailTable.Rows.Count > 0 Then
                    Dim rowNew As DataRow
                    For Each CarDetail In CarDetailTable.Rows
                        If CarDetail("Chassis_Num").ToString.Length > 0 Then
                            rowNew = m_tblResult.NewRow
                            rowNew("Kontonummer") = CarDetail("Liznr")
                            rowNew("Fahrgestellnummer") = CarDetail("Chassis_Num")
                            rowNew("Briefnummer") = CarDetail("Tidnr")
                            rowNew("Kennzeichen") = CarDetail("Zzkenn")
                            rowNew("Label") = CarDetail("Zzlabel")
                            rowNew("Modellbezeichnung") = CarDetail("Zzmodid")
                            Dim tmpDat As DateTime
                            If IsDate(CarDetail("Erdat").ToString) Then
                                tmpDat = CDate((CarDetail("Erdat")))
                                If Not tmpDat = CDate("01.01.1900") Then
                                    rowNew("Erstellt") = tmpDat.ToShortDateString
                                End If
                            End If
                            If IsDate(CarDetail("Zztmpdt").ToString) Then
                                tmpDat = CDate((CarDetail("Zztmpdt")))
                                If Not tmpDat = CDate("01.01.1900") Then
                                    rowNew("Versendet") = tmpDat.ToShortDateString
                                End If
                            End If
                            rowNew("KontonummerIdentisch") = False
                            rowNew("NurDaten") = False
                            rowNew("NurBrief") = False
                            Select Case CarDetail("Fehlerkz").ToString
                                Case "02"
                                    rowNew("KontonummerIdentisch") = True
                                Case "10"
                                    rowNew("NurDaten") = True
                                Case "20"
                                    rowNew("NurBrief") = True
                            End Select
                            rowNew("KontonummerNeu") = ""
                            rowNew("Bemerkung") = ""
                            rowNew("Action") = ""
                            rowNew("ActionNOTHING") = True
                            rowNew("ActionDELE") = False
                            rowNew("ActionVERS") = False
                            rowNew("ActionCHAN") = False
                            m_tblResult.Rows.Add(rowNew)
                        End If
                    Next

                    m_tblResultExcel = New DataTable()
                    m_tblResultExcel.Columns.Add("Kontonummer", System.Type.GetType("System.String"))
                    m_tblResultExcel.Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                    m_tblResultExcel.Columns.Add("Briefnummer", System.Type.GetType("System.String"))
                    m_tblResultExcel.Columns.Add("Kennzeichen", System.Type.GetType("System.String"))
                    m_tblResultExcel.Columns.Add("Label", System.Type.GetType("System.String"))
                    m_tblResultExcel.Columns.Add("Modellbezeichnung", System.Type.GetType("System.String"))
                    m_tblResultExcel.Columns.Add("Erstellt", System.Type.GetType("System.String"))
                    m_tblResultExcel.Columns.Add("Versendet", System.Type.GetType("System.String"))
                    m_tblResultExcel.Columns.Add("Doppelte VIN", System.Type.GetType("System.String"))
                    m_tblResultExcel.Columns.Add("Brief ohne Daten", System.Type.GetType("System.String"))
                    m_tblResultExcel.Columns.Add("Daten ohne Brief", System.Type.GetType("System.String"))
                    Dim i As Int32
                    For i = 0 To m_tblResult.Rows.Count - 1
                        rowNew = m_tblResultExcel.NewRow
                        rowNew("Kontonummer") = m_tblResult.Rows(i)("Kontonummer")
                        rowNew("Fahrgestellnummer") = m_tblResult.Rows(i)("Fahrgestellnummer")
                        rowNew("Briefnummer") = m_tblResult.Rows(i)("Briefnummer")
                        rowNew("Kennzeichen") = m_tblResult.Rows(i)("Kennzeichen")
                        rowNew("Label") = m_tblResult.Rows(i)("Label")
                        rowNew("Modellbezeichnung") = m_tblResult.Rows(i)("Modellbezeichnung")
                        If IsDate(m_tblResult.Rows(i)("Erstellt")) Then
                            rowNew("Erstellt") = CType(m_tblResult.Rows(i)("Erstellt"), DateTime).ToShortDateString
                        End If
                        If IsDate(m_tblResult.Rows(i)("Versendet")) Then
                            rowNew("Versendet") = CType(m_tblResult.Rows(i)("Versendet"), DateTime).ToShortDateString
                        End If
                        If CType(m_tblResult.Rows(i)("NurBrief"), Boolean) Then
                            rowNew("Brief ohne Daten") = "Ja"
                        Else
                            rowNew("Brief ohne Daten") = "Nein"
                        End If
                        If CType(m_tblResult.Rows(i)("NurDaten"), Boolean) Then
                            rowNew("Daten ohne Brief") = "Ja"
                        Else
                            rowNew("Daten ohne Brief") = "Nein"
                        End If
                        If CType(m_tblResult.Rows(i)("KontonummerIdentisch"), Boolean) Then
                            rowNew("Doppelte VIN") = "Ja"
                        Else
                            rowNew("Doppelte VIN") = "Nein"
                        End If
                        'Seidel, 26.10.2004: Verhindert Anzeige des Datensatzes in Excel, wenn Kontonummer leer:
                        If Not CStr(rowNew("Kontonummer")) = String.Empty Then
                            m_tblResultExcel.Rows.Add(rowNew)
                        End If
                    Next

                    m_strExcelFileName = Format(Now, "yyyyMMdd_HHmmss_") & m_objUser.UserName & ".xls"
                    m_strFileName = m_strExcelFileName
                    Try
                        Excel.ExcelExport.WriteExcel(m_tblResultExcel, m_objApp.ExcelPath & m_strExcelFileName)
                    Catch
                    End Try
                Else
                    m_intStatus = -2202
                    m_strMessage = "Fehler: Keine Vorgangsinformationen vorhanden."
                End If

                Dim col As DataColumn
                For Each col In m_tblResult.Columns
                    col.ReadOnly = False
                Next

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_intStatus = -2201
                        m_strMessage = "Keine Daten gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
            Finally

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overrides Sub Change()

    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intStatus = 0
            m_strMessage = ""

            Dim tmpDataView As DataView
            tmpDataView = m_tblResult.DefaultView

            tmpDataView.RowFilter = "ActionNOTHING = 0"

            m_intIDSAP = -1

            For i As Integer = 0 To tmpDataView.Count - 1
                Dim strBemerkung As String = "Vorgang erfolgreich"
                Dim strActionText As String = ""
                Try
                    Dim strAction As String = ""
                    If CType(tmpDataView.Item(i)("ActionDELE"), Boolean) Then
                        strAction = "DELE"
                        strActionText = "Löschen"
                    End If
                    If CType(tmpDataView.Item(i)("ActionVERS"), Boolean) Then
                        strAction = "VERS"
                        strActionText = "Versenden"
                    End If
                    If CType(tmpDataView.Item(i)("ActionCHAN"), Boolean) Then
                        strAction = "CHAN"
                        strActionText = "Ändern Kto.Nr.<br>" & CType(tmpDataView.Item(i)("KontonummerNeu"), String)
                    End If

                    Dim strCHASSIS_NUM As String = CType(tmpDataView.Item(i)("Fahrgestellnummer"), String)
                    Dim strKUNNR As String = m_strCustomer
                    Dim strLIZNR As String = CType(tmpDataView.Item(i)("Kontonummer"), String)

                    S.AP.Init("Zz_Csc_Daten_Ueb_Change_001")

                    S.AP.SetImportParameter("I_KUNNR", strKUNNR)
                    S.AP.SetImportParameter("I_CHASSIS_NUM", strCHASSIS_NUM)
                    S.AP.SetImportParameter("I_LIZNR", strLIZNR)
                    S.AP.SetImportParameter("I_ACTION", strAction)

                    S.AP.Execute()

                Catch ex As Exception
                    Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace("Execution failed", "").Trim()
                        Case "WRONG_ACTION"
                            m_intStatus = -2810
                            m_strMessage = "Fehler bei der Speicherung der Vorgänge"
                            strBemerkung = "Falscher Aktionscode"
                        Case "VERTRAG_FEHLT"
                            m_intStatus = -2811
                            m_strMessage = "Fehler bei der Speicherung der Vorgänge"
                            strBemerkung = "Neue Vertragsnummer fehlt"
                        Case "VERTRAG_VORHANDEN"
                            m_intStatus = -2812
                            m_strMessage = "Fehler bei der Speicherung der Vorgänge"
                            strBemerkung = "Neue Vertragsnummer vorhanden"
                        Case "VERTRAG_UNGUELTIG"
                            m_intStatus = -2813
                            m_strMessage = "Fehler bei der Speicherung der Vorgänge"
                            strBemerkung = "Neue Vertragsnummer ungültig"
                        Case "NO_EQUI"
                            m_intStatus = -2814
                            m_strMessage = "Fehler bei der Speicherung der Vorgänge"
                            strBemerkung = "Keinen Brief gefunden(EQUI)"
                        Case "NO_EQUZ"
                            m_intStatus = -2815
                            m_strMessage = "Fehler bei der Speicherung der Vorgänge"
                            strBemerkung = "Keinen Brief gefunden(EQUZ)"
                        Case "NO_INSERT_001"
                            m_intStatus = -2816
                            m_strMessage = "Fehler bei der Speicherung der Vorgänge"
                            strBemerkung = "Satz nicht hinzugefügt (ZCSCUEBER_001)"
                        Case "NO_INSERT_002"
                            m_intStatus = -2817
                            m_strMessage = "Fehler bei der Speicherung der Vorgänge"
                            strBemerkung = "Satz nicht hinzugefügt (ZCSCUEBER_002)"
                        Case "NO_DELETE_003"
                            m_intStatus = -2818
                            m_strMessage = "Fehler bei der Speicherung der Vorgänge"
                            strBemerkung = "Satz nicht gelöscht (ZCSCUEBER_003)"
                        Case "NO_DELETE"
                            m_intStatus = -2819
                            m_strMessage = "Fehler bei der Speicherung der Vorgänge"
                            strBemerkung = "Keine Löschung durchgeführt (Fehlerkz <> 10 oder 20)"
                        Case "NO_UPDATE_EQUI"
                            m_intStatus = -2820
                            m_strMessage = "Fehler bei der Speicherung der Vorgänge"
                            strBemerkung = "Fehler bei der Datenspeicherung (EQUI-UPDATE)"
                        Case "NO_AUFTRAG"
                            m_intStatus = -2821
                            m_strMessage = "Fehler bei der Speicherung der Vorgänge"
                            strBemerkung = "Kein Auftrag angelegt"
                        Case "NO_ZDADVERSAND"
                            m_intStatus = -2822
                            m_strMessage = "Fehler bei der Speicherung der Vorgänge"
                            strBemerkung = "Keine Einträge für die Versanddatei erstellt"
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = "Fehler bei der Speicherung der Vorgänge"
                            strBemerkung = ex.Message
                    End Select
                End Try

                m_tblResult.AcceptChanges()
                Dim tmpRows As DataRow()
                tmpRows = m_tblResult.Select("Kontonummer = '" & CType(tmpDataView.Item(i)("Kontonummer"), String) & "'")
                tmpRows(0).BeginEdit()
                tmpRows(0).Item("Action") = strActionText
                tmpRows(0).Item("Bemerkung") = strBemerkung
                tmpRows(0).EndEdit()
                m_tblResult.AcceptChanges()
            Next
            m_blnGestartet = False
        End If
    End Sub

#End Region

End Class

' ************************************************
' $History: CSC_Neugeschaeft.vb $
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
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 16:46
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 7.03.07    Time: 9:28
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Lib
' 
' ************************************************
