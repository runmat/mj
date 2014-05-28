Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel

Public Class FFE_Haendlerstatus

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
    Public Sub ReadHandlerstatusAll()

        m_strClassAndMethod = "FFE_Haendlerstatus.ReadHandlerstatusAll"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()

            Dim creditLimitDetail As SAPProxy_FFE.ZCS_CREDITLIMIT_DETAIL_001
            Dim tblCreditLimit As New SAPProxy_FFE.ZCS_CREDITLIMIT_DETAIL_001Table()

            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If

            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""

                m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_V_Creditlimit_Detail_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Creditlimit_Detail_Fce("  ", Right("0000000000" & m_objUser.KUNNR, 10), "     ", "1510", tblCreditLimit)
                objSAP.CommitWork()
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If

                m_Kontingente = CreateDataTableKontingente()
                Dim bFirstRow As Boolean = True
                If tblCreditLimit.Count > 0 Then
                    Dim rowNew As DataRow = m_Kontingente.NewRow()
                    InitializeRowKontingente(rowNew)
                    Dim aKunnr As Int32 = 0
                    For Each creditLimitDetail In tblCreditLimit
                        If aKunnr <> CInt(creditLimitDetail.Kunnr) Then
                            aKunnr = CInt(creditLimitDetail.Kunnr)
                            If Not bFirstRow Then
                                m_Kontingente.Rows.Add(rowNew)
                                rowNew = m_Kontingente.NewRow()
                                InitializeRowKontingente(rowNew)
                            Else
                                bFirstRow = False
                            End If
                        End If

                        rowNew("HaendlerNr") = creditLimitDetail.Kunnr.Substring(5, 5)
                        Select Case creditLimitDetail.Kkber
                            Case "0001"
                                rowNew("TmpKontingent") = CInt(creditLimitDetail.Klimk)
                                rowNew("TmpInanspruchnahme") = CInt(creditLimitDetail.Skfor)
                                rowNew("TmpFreiesKontingent") = CInt(creditLimitDetail.Klimk) - CInt(creditLimitDetail.Skfor)
                                rowNew("TmpFrist") = CInt(creditLimitDetail.Zzfrist)
                                rowNew("TmpSperre") = creditLimitDetail.Crblb
                            Case "0002"
                                rowNew("EndgKontingent") = CInt(creditLimitDetail.Klimk)
                                rowNew("EndgInanspruchnahme") = CInt(creditLimitDetail.Skfor)
                                rowNew("EndgFreiesKontingent") = CInt(creditLimitDetail.Klimk) - CInt(creditLimitDetail.Skfor)
                                rowNew("EndgFrist") = CInt(creditLimitDetail.Zzfrist)
                                rowNew("EndgSperre") = creditLimitDetail.Crblb
                            Case "0003"
                                rowNew("RetailRichtwert") = CInt(creditLimitDetail.Zzrwert)
                                rowNew("RetailAusschoepfung") = CInt(creditLimitDetail.Skfor)
                                rowNew("RetailFrist") = CInt(creditLimitDetail.Zzfrist)
                                rowNew("RetailSperre") = creditLimitDetail.Crblb
                            Case "0004"
                                rowNew("DelayedRichtwert") = CInt(creditLimitDetail.Zzrwert)
                                rowNew("DelayedAusschoepfung") = CInt(creditLimitDetail.Skfor)
                                rowNew("DelayedFrist") = CInt(creditLimitDetail.Zzfrist)
                                rowNew("DelayedSperre") = creditLimitDetail.Crblb
                            Case "0005"
                                rowNew("HEZRichtwert") = CInt(creditLimitDetail.Zzrwert)
                                rowNew("HEZAusschoepfung") = CInt(creditLimitDetail.Skfor)
                                rowNew("HEZFrist") = CInt(creditLimitDetail.Zzfrist)
                                rowNew("TmpSperre") = creditLimitDetail.Crblb
                            Case "0006"
                                rowNew("KFKLRichtwert") = CInt(creditLimitDetail.Zzrwert)
                                rowNew("KFKLAusschoepfung") = CInt(creditLimitDetail.Skfor)
                                rowNew("KFKLFrist") = CInt(creditLimitDetail.Zzfrist)
                                rowNew("KFKLSperre") = creditLimitDetail.Crblb
                            Case Else
                                'Nothing to do
                        End Select
                    Next
                    m_Kontingente.Rows.Add(rowNew)
                    WriteLogEntry(True, "KUNNR=" & Right(m_objUser.KUNNR, 5), m_Kontingente)
                End If
            Catch ex As Exception
                Select Case ex.Message
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
            Finally
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Private Function CreateDataTableKontingente() As DataTable
        Dim aTable As DataTable = New DataTable()

        With aTable.Columns
            .Add("HaendlerNr", System.Type.GetType("System.String"))
            .Add("TmpKontingent", System.Type.GetType("System.Int32"))
            .Add("TmpInanspruchnahme", System.Type.GetType("System.Int32"))
            .Add("TmpFreiesKontingent", System.Type.GetType("System.Int32"))
            .Add("TmpFrist", System.Type.GetType("System.Int32"))
            .Add("TmpSperre", System.Type.GetType("System.String"))
            .Add("EndgKontingent", System.Type.GetType("System.Int32"))
            .Add("EndgInanspruchnahme", System.Type.GetType("System.Int32"))
            .Add("EndgFreiesKontingent", System.Type.GetType("System.Int32"))
            .Add("EndgFrist", System.Type.GetType("System.Int32"))
            .Add("EndgSperre", System.Type.GetType("System.String"))
            .Add("RetailRichtwert", System.Type.GetType("System.Int32"))
            .Add("RetailAusschoepfung", System.Type.GetType("System.Int32"))
            .Add("RetailFrist", System.Type.GetType("System.Int32"))
            .Add("RetailSperre", System.Type.GetType("System.String"))
            .Add("DelayedRichtwert", System.Type.GetType("System.Int32"))
            .Add("DelayedAusschoepfung", System.Type.GetType("System.Int32"))
            .Add("DelayedFrist", System.Type.GetType("System.Int32"))
            .Add("DelayedSperre", System.Type.GetType("System.String"))
            .Add("HEZRichtwert", System.Type.GetType("System.Int32"))
            .Add("HEZAusschoepfung", System.Type.GetType("System.Int32"))
            .Add("HEZFrist", System.Type.GetType("System.Int32"))
            .Add("HEZSperre", System.Type.GetType("System.String"))
            .Add("KFKLRichtwert", System.Type.GetType("System.Int32"))
            .Add("KFKLAusschoepfung", System.Type.GetType("System.Int32"))
            .Add("KFKLFrist", System.Type.GetType("System.Int32"))
            .Add("KFKLSperre", System.Type.GetType("System.String"))
        End With

        Return aTable
    End Function

    Private Sub InitializeRowKontingente(ByVal aRow As DataRow)
        aRow("HaendlerNr") = 0
        aRow("TmpKontingent") = 0
        aRow("TmpInanspruchnahme") = 0
        aRow("TmpFreiesKontingent") = 0
        aRow("TmpFrist") = 0
        aRow("TmpSperre") = 0
        aRow("EndgKontingent") = 0
        aRow("EndgInanspruchnahme") = 0
        aRow("EndgFreiesKontingent") = 0
        aRow("EndgFrist") = 0
        aRow("EndgSperre") = 0
        aRow("RetailRichtwert") = 0
        aRow("RetailAusschoepfung") = 0
        aRow("RetailFrist") = 0
        aRow("RetailSperre") = 0
        aRow("DelayedRichtwert") = 0
        aRow("DelayedAusschoepfung") = 0
        aRow("DelayedFrist") = 0
        aRow("DelayedSperre") = 0
        aRow("HEZRichtwert") = 0
        aRow("HEZAusschoepfung") = 0
        aRow("HEZFrist") = 0
        aRow("HEZSperre") = 0
        aRow("KFKLRichtwert") = 0
        aRow("KFKLAusschoepfung") = 0
        aRow("KFKLFrist") = 0
        aRow("KFKLSperre") = 0
    End Sub
#End Region
End Class
' ************************************************
' $History: FFE_Haendlerstatus.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 7.07.08    Time: 10:40
' Updated in $/CKAG/Applications/AppFFE/lib
' SSV Historien hinzugef�gt
' 
' ************************************************
