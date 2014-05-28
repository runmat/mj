Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Common

<Serializable()> Public Class FFE_BankBase
    ' ehemals BankBaseCredit

    Inherits FFE_BankBasis

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
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String, Optional ByVal hez As Boolean = False)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        m_hez = hez
    End Sub

    Public Overrides Sub Change()

    End Sub

    Public Overrides Sub Show()
        m_strClassAndMethod = "FFE_BankBase.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

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
                    .Add("Lastschrift", System.Type.GetType("System.Boolean"))
                End With

                m_intStatus = 0
                m_strMessage = ""

                If CheckCustomerData() Then

                    S.AP.Init("Z_M_Creditlimit_Detail_Fce")

                    S.AP.SetImportParameter("I_KNRZE", Left(m_objUser.Reference, 2))
                    S.AP.SetImportParameter("I_KONZS", Right("0000000000" & m_objUser.KUNNR, 10))
                    S.AP.SetImportParameter("I_KUNNR", Right(m_strCustomer, 5))
                    S.AP.SetImportParameter("I_VKORG", "1510")

                    S.AP.Execute()

                    Dim CreditAccountDetailTable As DataTable = S.AP.GetExportTable("GT_WEB")

                    If CreditAccountDetailTable.Rows.Count > 0 Then
                        Dim rowNew As DataRow
                        For Each dr As DataRow In CreditAccountDetailTable.Rows
                            If dr("Kkber").ToString.Length > 0 Then
                                rowNew = m_tblKontingente.NewRow
                                rowNew("Kreditkontrollbereich") = dr("Kkber")
                                Select Case dr("Kkber").ToString
                                    Case "0001"
                                        rowNew("Kontingentart") = "Standard temporär"
                                        rowNew("ZeigeKontingentart") = True
                                    Case "0002"
                                        rowNew("Kontingentart") = "Standard endgültig"
                                        rowNew("ZeigeKontingentart") = True
                                    Case "0003"
                                        rowNew("Kontingentart") = "Retail"
                                        rowNew("ZeigeKontingentart") = False
                                        If dr("Zzfrist").ToString <> "000" Then
                                            rowNew("Lastschrift") = True
                                        End If
                                    Case "0004"
                                        rowNew("Kontingentart") = "Erweitertes Zahlungsziel (Delayed Payment) endgültig"
                                        rowNew("ZeigeKontingentart") = False
                                    Case "0005"
                                        rowNew("Kontingentart") = "HEZ (in standard temporär enthalten)"
                                        rowNew("ZeigeKontingentart") = False
                                    Case Else
                                        m_intStatus = -2203
                                        m_strMessage = "Fehler: Unbekannte Kontingentart (" & dr("Kkber").ToString & ")."
                                        Exit Try
                                End Select

                                rowNew("Richtwert_Alt") = CInt(dr("Zzrwert"))
                                rowNew("Richtwert_Neu") = rowNew("Richtwert_Alt")
                                rowNew("Kontingent_Alt") = CInt(dr("Klimk"))
                                rowNew("Kontingent_Neu") = rowNew("Kontingent_Alt")

                                If IsNumeric(dr("Skfor").ToString.Trim(" "c)) Then
                                    rowNew("Ausschoepfung") = CInt(dr("Skfor"))
                                Else
                                    rowNew("Ausschoepfung") = 0
                                End If

                                rowNew("Frei") = CInt(rowNew("Kontingent_Alt")) - CInt(rowNew("Ausschoepfung"))

                                If dr("Crblb").ToString = "X" Then
                                    rowNew("Gesperrt_Alt") = True
                                Else
                                    rowNew("Gesperrt_Alt") = False
                                End If

                                rowNew("Gesperrt_Neu") = rowNew("Gesperrt_Alt")

                                m_tblKontingente.Rows.Add(rowNew)
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

            Finally

                m_blnGestartet = False
            End Try
        End If
    End Sub
    
#End Region

End Class
' ************************************************
' $History: FFE_BankBase.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 7.07.08    Time: 10:40
' Updated in $/CKAG/Applications/AppFFE/lib
' SSV Historien hinzugef�gt
' 
' ************************************************
