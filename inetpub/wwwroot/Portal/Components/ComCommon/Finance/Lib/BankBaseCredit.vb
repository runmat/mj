Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

<Serializable()> Public Class BankBaseCredit
    REM § Lese-/Schreibfunktion, Kunde: Übergreifend, 
    REM § Show - BAPI: Z_M_Creditlimit_Detail_001,
    REM § Change - BAPI: Z_M_Creditlimit_Change_001.

    Inherits BankBase

#Region " Declarations"
    Private m_tblKontingente As DataTable
#End Region

#Region " Properties"
    Public Property Kontingente() As DataTable
        Get
            Return m_tblKontingente
        End Get
        Set(ByVal Value As DataTable)
            m_tblKontingente = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String,
                    ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        m_hez = False
    End Sub

    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String)
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try
            m_tblKontingente = New DataTable()
            With m_tblKontingente.Columns
                .Add("Kreditkontrollbereich", System.Type.GetType("System.String"))
                .Add("ZeigeKontingentart", System.Type.GetType("System.Boolean"))
                .Add("Kontingentart", System.Type.GetType("System.String"))
                .Add("Kontingent_Alt", System.Type.GetType("System.Int32"))
                .Add("Kontingent_Neu", System.Type.GetType("System.Int32"))
                .Add("Ausschoepfung", System.Type.GetType("System.Int32"))
                .Add("Frei", System.Type.GetType("System.Int32"))
                .Add("Gesperrt_Alt", System.Type.GetType("System.Boolean"))
                .Add("Gesperrt_Neu", System.Type.GetType("System.Boolean"))
                .Add("Insolvent_Alt", System.Type.GetType("System.Boolean"))
                .Add("Insolvent_Neu", System.Type.GetType("System.Boolean"))
                .Add("Richtwert_Alt", System.Type.GetType("System.Int32"))
                .Add("Richtwert_Neu", System.Type.GetType("System.Int32"))
            End With

            ClearError()

            If CheckCustomerData() Then

                S.AP.InitExecute("Z_M_Creditlimit_Detail_001", "I_KUNNR, I_HAENDLER, I_VKORG",
                                 Right("0000000000" & m_objUser.KUNNR, 10), Right("0000000000" & m_strCustomer, 10), "1510")

                Dim CreditAccountDetailTable As DataTable = S.AP.GetExportTable("GT_WEB")
                Dim CreditAccountDetail As DataRow

                If CreditAccountDetailTable.Rows.Count > 0 Then
                    Dim rowNew As DataRow
                    For Each CreditAccountDetail In CreditAccountDetailTable.Rows
                        If CreditAccountDetail("Kkber").ToString.Length > 0 Then
                            rowNew = m_tblKontingente.NewRow
                            rowNew("Kreditkontrollbereich") = CreditAccountDetail("Kkber")
                            Select Case CreditAccountDetail("Kkber").ToString
                                Case "0001"
                                    rowNew("Kontingentart") = "Standard temporär"
                                    rowNew("ZeigeKontingentart") = True
                                Case "0002"
                                    rowNew("Kontingentart") = "Standard endgültig"
                                    rowNew("ZeigeKontingentart") = True
                                Case "0005"
                                    rowNew("Kontingentart") = "HEZ (in standard temporär enthalten)"
                                    rowNew("ZeigeKontingentart") = False
                                Case Else
                                    m_intStatus = -2203
                                    m_strMessage = "Fehler: Unbekannte Kontingentart (" & CreditAccountDetail("Kkber").ToString & ")."
                                    Exit Try
                            End Select
                            rowNew("Richtwert_Alt") = CInt(CreditAccountDetail("Zzrwert"))
                            rowNew("Richtwert_Neu") = rowNew("Richtwert_Alt")
                            rowNew("Kontingent_Alt") = CInt(CreditAccountDetail("Klimk"))
                            rowNew("Kontingent_Neu") = rowNew("Kontingent_Alt")
                            If IsNumeric(CreditAccountDetail("Skfor").ToString.Trim(" "c)) Then
                                rowNew("Ausschoepfung") = CInt(CreditAccountDetail("Skfor"))
                            Else
                                rowNew("Ausschoepfung") = 0
                            End If
                            rowNew("Frei") = CInt(rowNew("Kontingent_Alt")) - CInt(rowNew("Ausschoepfung"))
                            If CreditAccountDetail("Crblb").ToString = "X" Then
                                rowNew("Gesperrt_Alt") = True
                            Else
                                rowNew("Gesperrt_Alt") = False
                            End If
                            rowNew("Gesperrt_Neu") = rowNew("Gesperrt_Alt")
                            If CreditAccountDetail("ZZINSOLVENZ").ToString = "X" Then
                                rowNew("Insolvent_Alt") = True
                            Else
                                rowNew("Insolvent_Alt") = False
                            End If
                            rowNew("Insolvent_Neu") = rowNew("Insolvent_Alt")

                            m_tblKontingente.Rows.Add(rowNew)
                        End If
                    Next
                Else
                    RaiseError("-2202", "Fehler: Keine Kontingentinformationen vorhanden.")
                End If

                Dim col As DataColumn
                For Each col In m_tblKontingente.Columns
                    col.ReadOnly = False
                Next
                WriteLogEntry(True, "KUNNR=" & m_strCustomer, m_tblKontingente)
            End If
        Catch ex As Exception
            Dim errormessage As String = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If errormessage.Contains("NO_DATA") Then
                RaiseError("-2201", "Es konnten keine Kontingente ermittelt werden.")
            Else
                RaiseError("-9999", HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
            End If
           
            WriteLogEntry(False, "KUNNR=" & m_strCustomer & " , " & Replace(m_strMessage, "<br>", " "), m_tblKontingente)
        End Try
    End Sub

    Public Overrides Sub Show()
    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String)
        Dim strKontingentart As String = ""

        Try
            ClearError()

            If CheckCustomerData() Then

                Dim tmpRow As DataRow
                For Each tmpRow In m_tblKontingente.Rows

                    Dim decKreditlimit_Alt As Decimal = CType(tmpRow("Kontingent_Alt"), Decimal)
                    Dim decKreditlimit_Neu As Decimal = CType(tmpRow("Kontingent_Neu"), Decimal)
                    Dim decRichtwert_Alt As Decimal = CType(tmpRow("Richtwert_Alt"), Decimal)
                    Dim decRichtwert_Neu As Decimal = CType(tmpRow("Richtwert_Neu"), Decimal)
                    Dim blnGesperrt_Alt As Boolean = CType(tmpRow("Gesperrt_Alt"), Boolean)
                    Dim blnGesperrt_Neu As Boolean = CType(tmpRow("Gesperrt_Neu"), Boolean)
                    Dim blnInsolvent_Alt As Boolean = CType(tmpRow("Insolvent_Alt"), Boolean)
                    Dim blnInsolvent_Neu As Boolean = CType(tmpRow("Insolvent_Neu"), Boolean)
                    strKontingentart = CType(tmpRow("Kontingentart"), String)

                    If Not ((blnInsolvent_Alt = blnInsolvent_Neu) And _
                                    (decKreditlimit_Alt = decKreditlimit_Neu) And _
                                            (decRichtwert_Alt = decRichtwert_Neu) And _
                                                    (blnGesperrt_Alt = blnGesperrt_Neu)) Then
                        m_hez = False
                        Dim strHez As String = " "
                        If (CType(tmpRow("Kreditkontrollbereich"), String) = "0005") Then
                            m_hez = True
                            strHez = "X"
                        End If

                        Dim strGesperrt As String = " "
                        If blnGesperrt_Neu Then
                            strGesperrt = "X"
                        End If

                        Dim strInsolvent_Neu As String = " "
                        If blnInsolvent_Neu Then
                            strInsolvent_Neu = "X"
                        End If

                        S.AP.Init("Z_M_Creditlimit_Change_001")
                        S.AP.SetImportParameter("I_AG", KUNNR)
                        S.AP.SetImportParameter("I_HAENDLER", m_strCustomer)
                        S.AP.SetImportParameter("I_KKBER", CType(tmpRow("Kreditkontrollbereich"), String))
                        S.AP.SetImportParameter("I_KLIMK", CInt(decKreditlimit_Neu).ToString)
                        S.AP.SetImportParameter("I_CRBLB", strGesperrt)
                        S.AP.SetImportParameter("I_ZZRWERT", CInt(decRichtwert_Neu).ToString)
                        S.AP.SetImportParameter("I_ERNAM", Left(m_strInternetUser, 12))
                        S.AP.SetImportParameter("I_HEZKZ", strHez)
                        S.AP.SetImportParameter("I_ZZINSOLVENZ", strInsolvent_Neu)
                        S.AP.Execute()

                    End If
                Next
            End If
        Catch ex As Exception
            Dim errormessage As String = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If errormessage.Contains("NO_DATA") Then
                RaiseError("-2221", "Kontingentart """ & strKontingentart & """ nicht gefunden.")
            ElseIf errormessage.Contains("NO_ZCREDITCONTROL") Then
                RaiseError("-2222", "Kontingentart """ & strKontingentart & """ konnte nicht geändert werden. (Update-Fehler ZCREDITCONTROL)")
            ElseIf errormessage.Contains("NO_ZDADVERSAND") Then
                RaiseError("-2223", "Kontingentart """ & strKontingentart & """ konnte nicht geändert werden. (Insert-Fehler ZDADVERSAND)")
            ElseIf errormessage.Contains("ZVERSAND_SPERRE") Then
                RaiseError("-2224", "Kontingentart """ & strKontingentart & """ konnte nicht geändert werden. (ZCREDITCONTROL vom DAD gesperrt)")
            Else
                RaiseError("-9999", HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
            End If
            
        End Try
    End Sub

    Public Overrides Sub Change()
    End Sub
   
#End Region

End Class

' ************************************************
' $History: BankBaseCredit.vb $
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 2.03.10    Time: 14:35
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA: 3509, 3515, 3522
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 22.06.09   Time: 14:20
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2918 Z_M_Creditlimit_Change_001
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 22.06.09   Time: 13:26
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' nachbesserung ita 2918
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 22.06.09   Time: 13:18
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2918 Z_M_Creditlimit_Detail_001
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance/Lib
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 11.03.08   Time: 14:48
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1765
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 6.03.08    Time: 15:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Eigener Finance Proxy Fertigstellung
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 1.03.08    Time: 13:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Änderung der Händlernummer auf 10 stellen mit führenden 0 
' 
' *****************  Version 6  *****************
' User: Uha          Date: 8.01.08    Time: 18:24
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 4.01.08    Time: 16:01
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Bugfix RTFS
' 
' *****************  Version 4  *****************
' User: Uha          Date: 13.12.07   Time: 15:47
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Kontingentart "HEZ (in standard temporär enthalten)" in lokale
' BankBaseCredit wieder eingefügt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 13.12.07   Time: 13:58
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1481/1509 (Änderung / Sperrung Händlerkontingent) Testversion
' 
' *****************  Version 2  *****************
' User: Uha          Date: 13.12.07   Time: 10:31
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' In BankBaseCredit überflüssige Methoden und Kontingentarten entfernt
' 
' *****************  Version 1  *****************
' User: Uha          Date: 13.12.07   Time: 9:49
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' fin_06.vb durch BankBaseCredit.vb ersetzt
' 
' ************************************************
