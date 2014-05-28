Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Common
Imports CKG.Base.Business

Public Class FDD_Haendlerstatus
    Inherits Base.Business.DatenimportBase

#Region "Deklarationen"
    Private m_intIDSAP As Int32 = 0
    Private m_Kontingente As DataTable
#End Region

#Region "Konstruktoren"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strFileName)
    End Sub

#End Region

#Region "Eigenschaften"
    Public ReadOnly Property Kontingente() As DataTable
        Get
            Return (m_Kontingente)
        End Get
    End Property
#End Region

#Region "Methoden"

    Public Sub ReadHandlerstatusAll(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        m_strClassAndMethod = "BankBaseCredit.ReadHandlerstatusAll"

        Dim tblCreditLimit As New DataTable


        m_intIDSAP = -1

        Try
            m_intStatus = 0
            m_strMessage = ""
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_Creditlimit_Detail_001", m_objApp, m_objUser, page)


            'myProxy.setImportParameter("I_KUNNR", "     ")
            'myProxy.setImportParameter("I_KNRZE", "  ")
            'myProxy.setImportParameter("I_KONZS", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_VKORG", "1510")

            'myProxy.callBapi()

            S.AP.InitExecute("Z_V_Creditlimit_Detail_001", "I_KUNNR,I_KNRZE,I_KONZS,I_VKORG", "", "", Right("0000000000" & m_objUser.KUNNR, 10), "1510")

            tblCreditLimit = New DataTable
            tblCreditLimit = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

            m_Kontingente = CreateDataTableKontingente()
            Dim creditLimitDetail As DataRow
            Dim bFirstRow As Boolean = True
            If tblCreditLimit.Rows.Count > 0 Then
                Dim rowNew As DataRow = m_Kontingente.NewRow()
                InitializeRowKontingente(rowNew)
                Dim aKunnr As Int32 = 0
                For Each creditLimitDetail In tblCreditLimit.Rows
                    If aKunnr <> CInt(creditLimitDetail("Kunnr")) Then
                        aKunnr = CInt(creditLimitDetail("Kunnr"))
                        If Not bFirstRow Then
                            m_Kontingente.Rows.Add(rowNew)
                            rowNew = m_Kontingente.NewRow()
                            InitializeRowKontingente(rowNew)
                        Else
                            bFirstRow = False
                        End If
                    End If

                    rowNew("HaendlerNr") = creditLimitDetail("Kunnr").ToString.Substring(5, 5)
                    Select Case creditLimitDetail("Kkber").ToString
                        Case "0001"
                            rowNew("TmpKontingent") = CInt(creditLimitDetail("Klimk"))
                            rowNew("TmpInanspruchnahme") = CInt(creditLimitDetail("Skfor"))
                            rowNew("TmpFreiesKontingent") = CInt(creditLimitDetail("Klimk")) - CInt(creditLimitDetail("Skfor"))
                        Case "0002"
                            rowNew("EndgKontingent") = CInt(creditLimitDetail("Klimk"))
                            rowNew("EndgInanspruchnahme") = CInt(creditLimitDetail("Skfor"))
                            rowNew("EndgFreiesKontingent") = CInt(creditLimitDetail("Klimk")) - CInt(creditLimitDetail("Skfor"))
                        Case "0003"
                            rowNew("RetailRichtwert") = CInt(creditLimitDetail("Zzrwert"))
                            rowNew("RetailAusschoepfung") = CInt(creditLimitDetail("Skfor"))
                        Case "0004"
                            rowNew("DelayedRichtwert") = CInt(creditLimitDetail("Zzrwert"))
                            rowNew("DelayedAusschoepfung") = CInt(creditLimitDetail("Skfor"))
                        Case "0005"
                            rowNew("HEZRichtwert") = CInt(creditLimitDetail("Zzrwert"))
                            rowNew("HEZAusschoepfung") = CInt(creditLimitDetail("Skfor"))
                    End Select
                Next
                m_Kontingente.Rows.Add(rowNew)
                WriteLogEntry(True, "KUNNR=" & Right(m_objUser.KUNNR, 5), m_Kontingente)
            End If
        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_intStatus = -2201
                    m_strMessage = "Es konnten keine Kontingente ermittelt werden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = ex.Message
            End Select
            If m_intIDSAP > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & Right(m_objUser.KUNNR, 5) & " , " & Replace(m_strMessage, "<br>", " "), m_Kontingente)
        End Try
    End Sub

    Private Function CreateDataTableKontingente() As DataTable
        Dim aTable As DataTable = New DataTable()

        With aTable.Columns
            .Add("HaendlerNr", System.Type.GetType("System.String"))
            .Add("TmpKontingent", System.Type.GetType("System.Int32"))
            .Add("TmpInanspruchnahme", System.Type.GetType("System.Int32"))
            .Add("TmpFreiesKontingent", System.Type.GetType("System.Int32"))
            .Add("EndgKontingent", System.Type.GetType("System.Int32"))
            .Add("EndgInanspruchnahme", System.Type.GetType("System.Int32"))
            .Add("EndgFreiesKontingent", System.Type.GetType("System.Int32"))
            .Add("RetailRichtwert", System.Type.GetType("System.Int32"))
            .Add("RetailAusschoepfung", System.Type.GetType("System.Int32"))
            .Add("DelayedRichtwert", System.Type.GetType("System.Int32"))
            .Add("DelayedAusschoepfung", System.Type.GetType("System.Int32"))
            .Add("HEZRichtwert", System.Type.GetType("System.Int32"))
            .Add("HEZAusschoepfung", System.Type.GetType("System.Int32"))
        End With

        Return aTable
    End Function

    Private Sub InitializeRowKontingente(ByVal aRow As DataRow)
        aRow("HaendlerNr") = 0
        aRow("TmpKontingent") = 0
        aRow("TmpInanspruchnahme") = 0
        aRow("TmpFreiesKontingent") = 0
        aRow("EndgKontingent") = 0
        aRow("EndgInanspruchnahme") = 0
        aRow("EndgFreiesKontingent") = 0
        aRow("RetailRichtwert") = 0
        aRow("RetailAusschoepfung") = 0
        aRow("DelayedRichtwert") = 0
        aRow("DelayedAusschoepfung") = 0
        aRow("HEZRichtwert") = 0
        aRow("HEZAusschoepfung") = 0
    End Sub
#End Region
End Class

' ************************************************
' $History: FDD_Haendlerstatus.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 15.06.09   Time: 17:08
' Updated in $/CKAG/Applications/appffd/Lib
' ITA 2918
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 2  *****************
' User: Uha          Date: 2.07.07    Time: 17:40
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 1  *****************
' User: Uha          Date: 21.05.07   Time: 14:22
' Created in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Änderungen im Vergleich zur Startapplikation zum Stand 21.05.2007
' 
' ************************************************
