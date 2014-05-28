Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel

Public Class fin_12
    REM § Lese-/Schreibfunktion, Kunde: Übergreifend, 
    REM § GiveCars - BAPI: Z_M_Unangefordert_002,


    Inherits BankBaseCredit

#Region " Declarations"
    Private m_strHaendler As String
    Private m_tblAuftraege As DataTable
    Private m_strAuftragsNummer As String
    Private m_tblRaw As DataTable
#End Region

#Region " Properties"
    Public ReadOnly Property Auftraege() As DataTable
        Get
            Return m_tblAuftraege
        End Get
    End Property

    Public Property AuftragsNummer() As String
        Get
            Return m_strAuftragsNummer
        End Get
        Set(ByVal Value As String)
            m_strAuftragsNummer = Right("0000000000" & Value, 10)
        End Set
    End Property

    Public Property Haendler() As String
        Get
            Return m_strHaendler
        End Get
        Set(ByVal Value As String)
            m_strHaendler = Right(Value, 6).TrimStart("0"c)
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String, Optional ByVal hez As Boolean = False)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        m_strHaendler = ""
        m_HEZ = hez
    End Sub

    Public Overrides Sub Show()
        m_strClassAndMethod = "FFD_Bank_OffeneAnforderungen.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_ComCommon_Finance.SAPProxy_ComCommon_Finance()

            Dim tblAuftraegeSAP As New SAPProxy_ComCommon_Finance.ZDAD_M_WEB_EQUIDATENTable()

            MakeDestination()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Dim rowTemp As DataRow

            Try
                m_intStatus = 0
                m_strMessage = ""


                m_intIDsap = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Unangefordert_002", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                objSAP.Z_M_Unangefordert_002(Right(m_strCustomer, 10), "", Haendler, "", "", "1510", "", tblAuftraegeSAP)
                objSAP.CommitWork()
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True)
                End If

                m_tblRaw = tblAuftraegeSAP.ToADODataTable
                m_tblRaw.AcceptChanges()
                For Each rowTemp In m_tblRaw.Rows
                    Select Case rowTemp("ZZKKBER").ToString    'Wieso war hier Kontingentart <> 5 auskommentiert???
                        Case "0001"
                            rowTemp("ZZKKBER") = "Standard temporär"
                        Case "0002"
                            rowTemp("ZZKKBER") = "Standard endgültig"
                        Case "0003"
                            rowTemp("ZZKKBER") = "Retail"
                        Case "0004"
                            rowTemp("ZZKKBER") = "Erweitertes Zahlungsziel (Delayed Payment) endgültig"
                        Case "0005"
                            rowTemp("ZZKKBER") = "Händlereigene Zulassung"
                    End Select
                    If CStr(rowTemp("CMGST")) = "B" Then
                        rowTemp("CMGST") = "X"
                    Else
                        rowTemp("CMGST") = ""
                    End If
                Next
                m_tblRaw.AcceptChanges()
                m_tblAuftraege = CreateOutPut(m_tblRaw, m_strAppID)
                If m_tblAuftraege.Rows.Count = 0 Then
                    m_intStatus = 0
                    m_strMessage = "Keine Daten gefunden."
                End If

                If m_strHaendler.Length > 0 Then
                    Haendler = m_strHaendler
                End If

                WriteLogEntry(True, "KONZS=" & m_strCustomer.TrimStart("0"c) & ", KUNNR=" & Right(m_strHaendler, 5) & ", KKBER=", m_tblAuftraege)
            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_DATA"
                        m_intStatus = 0
                        If m_hez = True Then
                            m_intStatus = -2501
                        End If
                        m_strMessage = "Keine Daten gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
                End If
                WriteLogEntry(False, "KONZS=" & m_strCustomer.TrimStart("0"c) & ", KUNNR=" & Right(m_strHaendler, 5) & ", KKBER=" & " , " & Replace(m_strMessage, "<br>", " "), m_tblAuftraege)
            Finally
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overrides Sub Change()
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_ComCommon_Finance.SAPProxy_ComCommon_Finance()

            MakeDestination()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""

                If m_strAuftragsnummer.Trim("0"c).Length = 0 Or (Not m_strAuftragsnummer.Length = 10) Then
                    m_intStatus = -1301
                    m_strMessage = "Keine gültige Auftragsnummer übergeben."
                Else
                    Dim rowFahrzeug() As DataRow = m_tblRaw.Select("VBELN = '" & m_strAuftragsnummer & "'")
                    m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Offeneanforderungen_Storno", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                    Dim strEQUNR As String = rowFahrzeug(0)("EQUNR").ToString
                    Dim strKUNNR As String = rowFahrzeug(0)("KUNNR").ToString
                    Dim strKONZS As String = m_strCustomer
                    Dim strVBELN As String = m_strAuftragsnummer

                    objSAP.Z_M_Offeneanfor_Storno_001(strEQUNR, strKONZS, m_objUser.UserName, strKUNNR, strVBELN)
                    objSAP.CommitWork()

                    If m_intIDsap > -1 Then
                        m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True)
                    End If
                End If
            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_UPDATE"
                        m_intStatus = -3501
                        m_strMessage = "Kein EQUI-UPDATE."
                    Case "NO_ZDADVERSAND"
                        m_intStatus = -3502
                        m_strMessage = "Kein ZDADVERSAND-STORNO."
                    Case "NO_UPDATE_SALESDOCUMENT"
                        m_intStatus = -3503
                        m_strMessage = "Keine Auftragsänderung."
                    Case "ZVERSAND_SPERRE"
                        m_intStatus = -3504
                        m_strMessage = "ZVERSAND vom DAD gesperrt."
                    Case "NO_PICKLISTE"
                        m_intStatus = -3505
                        m_strMessage = "Kein Picklisteneintrag gefunden."
                    Case "NO_ZCREDITCONTROL"
                        m_intStatus = -3506
                        m_strMessage = "Kein Creditcontroleintrag gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
                End If
            Finally
                objSAP.Connection.Close()
                objSAP.Dispose()
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region

End Class
' ************************************************
' $History: fin_12.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 22.06.09   Time: 17:13
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ssv historie hinzugefügt
' 
' ************************************************
