Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class CSC_Sperrliste
    REM § Lese-/Schreibfunktion, Kunde: CSC, 
    REM § Show - BAPI: Zz_Csc_Daten_Listen_Report,
    REM § Change - BAPI: Zz_Csc_Daten_Listen_Change.

    Inherits BankBase ' FDD_Bank_Base

#Region " Declarations"
    Private m_tblFahrzeuge As DataTable
    Private m_blnIstNullliste As Boolean
#End Region

#Region " Properties"
    Public Property IstNullliste() As Boolean
        Get
            Return m_blnIstNullliste
        End Get
        Set(ByVal Value As Boolean)
            m_blnIstNullliste = Value
        End Set
    End Property

    Public Property Fahrzeuge() As DataTable
        Get
            Return m_tblFahrzeuge
        End Get
        Set(ByVal Value As DataTable)
            m_tblFahrzeuge = Value
        End Set
    End Property
#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        m_blnIstNullliste = False
    End Sub

    Public Overrides Sub Show()

    End Sub

    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "CSC_Sperrliste.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim CarDetail As DataRow
            Dim CarDetailTable As DataTable

            m_intIDSAP = -1
            Dim strVorgang As String
            If m_blnIstNullliste Then
                strVorgang = "08"
            Else
                strVorgang = "09"
            End If

            Try
                m_tblFahrzeuge = New DataTable()
                m_tblFahrzeuge.Columns.Add("Kontonummer", System.Type.GetType("System.String"))
                m_tblFahrzeuge.Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                m_tblFahrzeuge.Columns.Add("Briefnummer", System.Type.GetType("System.String"))
                m_tblFahrzeuge.Columns.Add("Kennzeichen", System.Type.GetType("System.String"))
                m_tblFahrzeuge.Columns.Add("Label", System.Type.GetType("System.String"))
                m_tblFahrzeuge.Columns.Add("Modellbezeichnung", System.Type.GetType("System.String"))
                If m_blnIstNullliste Then
                    m_tblFahrzeuge.Columns.Add("Datum_Nullliste", System.Type.GetType("System.DateTime"))
                Else
                    m_tblFahrzeuge.Columns.Add("Datum_Sperrliste", System.Type.GetType("System.DateTime"))
                End If
                m_tblFahrzeuge.Columns.Add("Datum_Briefeingang", System.Type.GetType("System.DateTime"))
                m_tblFahrzeuge.Columns.Add("Datum_Versand", System.Type.GetType("System.DateTime"))
                m_tblFahrzeuge.Columns.Add("Problem", System.Type.GetType("System.String"))
                m_tblFahrzeuge.Columns.Add("ProblemCode", System.Type.GetType("System.String"))
                m_tblFahrzeuge.Columns.Add("ActionNOTHING", System.Type.GetType("System.Boolean"))
                m_tblFahrzeuge.Columns.Add("ActionDELE", System.Type.GetType("System.Boolean"))
                m_tblFahrzeuge.Columns.Add("Bemerkung", System.Type.GetType("System.String"))
                m_tblFahrzeuge.Columns.Add("Action", System.Type.GetType("System.String"))

                m_intStatus = 0
                m_strMessage = ""

                S.AP.Init("Zz_Csc_Daten_Listen_Report")

                S.AP.SetImportParameter("KUNNR", m_strCustomer)
                S.AP.SetImportParameter("VORGANG", strVorgang)

                S.AP.Execute()

                CarDetailTable = S.AP.GetExportTable("AUSGABE")

                If CarDetailTable.Rows.Count > 0 Then
                    Dim rowNew As DataRow
                    For Each CarDetail In CarDetailTable.Rows
                        rowNew = m_tblFahrzeuge.NewRow
                        rowNew("Kontonummer") = CarDetail("Liznr")
                        rowNew("Fahrgestellnummer") = CarDetail("Chassis_Num")
                        rowNew("Briefnummer") = CarDetail("Tidnr")
                        rowNew("Kennzeichen") = CarDetail("License_Num")
                        rowNew("Label") = CarDetail("Zzlabel")
                        rowNew("Modellbezeichnung") = CarDetail("Zzmodid")
                        Dim tmpDat As DateTime
                        If IsDate(CarDetail("Erdat").ToString) Then
                            tmpDat = CDate((CarDetail("Erdat")))
                            If Not tmpDat = CDate("01.01.1900") Then
                                rowNew("Datum_Briefeingang") = tmpDat.ToShortDateString
                            End If
                        End If
                        If IsDate(CarDetail("Erfdat").ToString) Then
                            tmpDat = CDate((CarDetail("Erfdat")))
                            If Not tmpDat = CDate("01.01.1900") Then
                                rowNew("Datum_Nullliste") = tmpDat.ToShortDateString
                            End If
                        End If
                        If IsDate(CarDetail("Zztmpdt").ToString) Then
                            tmpDat = CDate((CarDetail("Zztmpdt")))
                            If Not tmpDat = CDate("01.01.1900") Then
                                rowNew("Datum_Versand") = tmpDat.ToShortDateString
                            End If
                        Else
                            tmpDat = CDate("01.01.1899")

                        End If

                        Select Case CarDetail("Fehler").ToString.ToUpper
                            Case "080"
                                rowNew("Problem") = "Brief nicht bekannt."
                                rowNew("ProblemCode") = "080"
                            Case "081"
                                If tmpDat > CDate("01.01.1900") Then
                                    rowNew("Problem") = "Brief zur Zeit nicht im Archiv."
                                    rowNew("ProblemCode") = "082"
                                Else
                                    rowNew("Problem") = "Versandsperre"
                                    rowNew("ProblemCode") = "081"
                                End If
                            Case "082"
                                rowNew("Problem") = "Brief zur Zeit nicht im Archiv."
                                rowNew("ProblemCode") = "082"
                            Case "083"
                                rowNew("Problem") = "Fehlercode z.Zt. nicht benutzt"
                                rowNew("ProblemCode") = "083"
                            Case "084"
                                rowNew("Problem") = "Kein Versandschreiben."
                                rowNew("ProblemCode") = "084"
                            Case "085"
                                rowNew("Problem") = "Briefverlust"
                                rowNew("ProblemCode") = "085"
                            Case "090"
                                rowNew("Problem") = "Kein Brief erfasst."
                                rowNew("ProblemCode") = "090"
                            Case "091"
                                rowNew("Problem") = "Brief schon versendet."
                                rowNew("ProblemCode") = "091"
                            Case Else
                                rowNew("Problem") = "Unbekannt (Code: " & CarDetail("Fehler").ToString & ")."
                                rowNew("ProblemCode") = "999"
                        End Select
                        rowNew("Bemerkung") = ""
                        rowNew("Action") = ""
                        rowNew("ActionNOTHING") = True
                        rowNew("ActionDELE") = False
                        m_tblFahrzeuge.Rows.Add(rowNew)

                        m_tblFahrzeuge.AcceptChanges()
                    Next
                Else
                    m_intStatus = -2202
                    m_strMessage = "Keine Vorgangsinformationen vorhanden."
                End If

                Dim col As DataColumn
                For Each col In m_tblFahrzeuge.Columns
                    col.ReadOnly = False
                Next

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace("Execution failed", "").Trim()
                    Case "NO_CREDITLIMIT"
                        m_intStatus = -2201
                        m_strMessage = "Es konnte kein Kreditlimit ermittelt werden."
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
            tmpDataView = m_tblFahrzeuge.DefaultView

            tmpDataView.RowFilter = "ActionNOTHING = 0"

            m_intIDSAP = -1

            For i As Integer = 0 To tmpDataView.Count - 1
                Dim strBemerkung As String = "Vorgang erfolgreich"
                Dim strActionText As String = ""
                Try
                    strActionText = "Löschen"

                    S.AP.Init("Zz_Csc_Daten_Listen_Change")

                    S.AP.SetImportParameter("KUNNR", m_strCustomer)
                    S.AP.SetImportParameter("LIZNR", CType(tmpDataView.Item(i)("Kontonummer"), String))

                    S.AP.Execute()

                Catch ex As Exception
                    m_intStatus = -9999
                    m_strMessage = "Fehler bei der Speicherung der Vorgänge"
                    strBemerkung = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                End Try

                m_tblFahrzeuge.AcceptChanges()
                Dim tmpRows As DataRow()
                tmpRows = m_tblFahrzeuge.Select("Kontonummer = '" & CType(tmpDataView.Item(i)("Kontonummer"), String) & "'")
                tmpRows(0).BeginEdit()
                tmpRows(0).Item("Action") = strActionText
                tmpRows(0).Item("Bemerkung") = strBemerkung
                tmpRows(0).EndEdit()
                m_tblFahrzeuge.AcceptChanges()
            Next

            m_blnGestartet = False
        End If
    End Sub

#End Region

End Class

' ************************************************
' $History: CSC_Sperrliste.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 21.10.10   Time: 10:51
' Updated in $/CKAG/Applications/appcsc/Lib
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 19.03.10   Time: 17:40
' Updated in $/CKAG/Applications/appcsc/Lib
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
