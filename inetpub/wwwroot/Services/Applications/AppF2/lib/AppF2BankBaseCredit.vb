Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common
Imports CKG.Base.Kernel.Security

<Serializable()> Public Class AppF2BankBaseCredit
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
    Public Sub New(ByRef objUser As User, ByRef objApp As App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        m_hez = False
    End Sub



    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        Dim intID As Int32 = -1
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
                .Add("Richtwert_Alt", System.Type.GetType("System.Int32"))
                .Add("Richtwert_Neu", System.Type.GetType("System.Int32"))
            End With
            m_intStatus = 0
            m_strMessage = ""

            If CheckCustomerData() Then
                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Creditlimit_Detail_001", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                myProxy.setImportParameter("I_HAENDLER", Right("0000000000" & m_strCustomer, 10))
                myProxy.setImportParameter("I_VKORG", "1510")

                myProxy.callBapi()


                Dim CreditAccountDetailTable As DataTable = myProxy.getExportTable("GT_WEB")
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
                                    '    rowNew("Kontingentart") = "Standard endgültig"
                                    '    rowNew("ZeigeKontingentart") = True
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


                            If Not CreditAccountDetail("Kkber").ToString = "0002" Then
                                m_tblKontingente.Rows.Add(rowNew)
                            End If


                        End If
                    Next
                Else
                    m_intStatus = -2202
                    m_strMessage = "Fehler: Keine Kontingentinformationen vorhanden."
                End If

                Dim col As DataColumn
                For Each col In m_tblKontingente.Columns
                    col.ReadOnly = False
                Next
                WriteLogEntry(True, "KUNNR=" & m_strCustomer, m_tblKontingente)
            End If
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -2201
                    m_strMessage = "Es konnten keine Kontingente ermittelt werden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select
            WriteLogEntry(False, "KUNNR=" & m_strCustomer & " , " & Replace(m_strMessage, "<br>", " "), m_tblKontingente)
        End Try
    End Sub


    Public Overrides Sub Show()
    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        Dim strKontingentart As String = ""

        Try
            m_intStatus = 0
            m_strMessage = ""

            If CheckCustomerData() Then

                Dim tmpRow As DataRow
                For Each tmpRow In m_tblKontingente.Rows

                    Dim decKreditlimit_Alt As Decimal = CType(tmpRow("Kontingent_Alt"), Decimal)
                    Dim decKreditlimit_Neu As Decimal = CType(tmpRow("Kontingent_Neu"), Decimal)
                    Dim decRichtwert_Alt As Decimal = CType(tmpRow("Richtwert_Alt"), Decimal)
                    Dim decRichtwert_Neu As Decimal = CType(tmpRow("Richtwert_Neu"), Decimal)
                    Dim blnGesperrt_Alt As Boolean = CType(tmpRow("Gesperrt_Alt"), Boolean)
                    Dim blnGesperrt_Neu As Boolean = CType(tmpRow("Gesperrt_Neu"), Boolean)
                    strKontingentart = CType(tmpRow("Kontingentart"), String)

                    If Not ((decKreditlimit_Alt = decKreditlimit_Neu) And (decRichtwert_Alt = decRichtwert_Neu) And (blnGesperrt_Alt = blnGesperrt_Neu)) Then
                        m_hez = False
                        Dim strHez As String = " "
                        If (CType(tmpRow("Kreditkontrollbereich"), String) = "0005") Then
                            m_hez = True
                            strHez = "X"
                        End If

                        Dim strGesperrt As String = " "


                        If blnGesperrt_Neu <> blnGesperrt_Alt Then
                            If blnGesperrt_Neu Then strGesperrt = "X"
                            For i As Integer = 1 To 2
                                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Creditlimit_Change_001", m_objApp, m_objUser, page)
                                myProxy.setImportParameter("I_AG", m_strKUNNR)
                                myProxy.setImportParameter("I_HAENDLER", m_strCustomer)

                                myProxy.setImportParameter("I_KLIMK", CInt(decKreditlimit_Neu).ToString)
                                myProxy.setImportParameter("I_ZZRWERT", CInt(decRichtwert_Neu).ToString)
                                myProxy.setImportParameter("I_ERNAM", Left(m_strInternetUser, 12))
                                myProxy.setImportParameter("I_HEZKZ", strHez)

                                myProxy.setImportParameter("I_KKBER", "000" & i)
                                myProxy.setImportParameter("I_CRBLB", strGesperrt)
                                myProxy.callBapi()
                            Next
                        Else
                            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Creditlimit_Change_001", m_objApp, m_objUser, page)
                            myProxy.setImportParameter("I_AG", m_strKUNNR)
                            myProxy.setImportParameter("I_HAENDLER", m_strCustomer)

                            myProxy.setImportParameter("I_KLIMK", CInt(decKreditlimit_Neu).ToString)
                            myProxy.setImportParameter("I_ZZRWERT", CInt(decRichtwert_Neu).ToString)
                            myProxy.setImportParameter("I_ERNAM", Left(m_strInternetUser, 12))
                            myProxy.setImportParameter("I_HEZKZ", strHez)
                            myProxy.setImportParameter("I_KKBER", CType(tmpRow("Kreditkontrollbereich"), String))
                            myProxy.setImportParameter("I_CRBLB", strGesperrt)
                            myProxy.callBapi()
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -2221
                    m_strMessage = "Kontingentart """ & strKontingentart & """ nicht gefunden."
                Case "NO_ZCREDITCONTROL"
                    m_intStatus = -2222
                    m_strMessage = "Kontingentart """ & strKontingentart & """ konnte nicht geändert werden. (Update-Fehler ZCREDITCONTROL)"
                Case "NO_ZDADVERSAND"
                    m_intStatus = -2223
                    m_strMessage = "Kontingentart """ & strKontingentart & """ konnte nicht geändert werden. (Insert-Fehler ZDADVERSAND)"
                Case "ZCREDITCONTROL_SPERRE"
                    m_intStatus = -2224
                    m_strMessage = "Kontingentart """ & strKontingentart & """ konnte nicht geändert werden. (ZCREDITCONTROL vom DAD gesperrt)"
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select
        End Try
    End Sub

    Public Overrides Sub Change()
    End Sub

#End Region
End Class
