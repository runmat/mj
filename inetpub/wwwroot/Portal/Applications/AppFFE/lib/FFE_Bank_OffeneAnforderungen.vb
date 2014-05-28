Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports Microsoft.Data.SAPClient

Public Class FFE_Bank_OffeneAnforderungen
    REM § Lese-/Schreibfunktion, Kunde: FFD,
    REM § Show - BAPI: Z_M_AUFTRAGSDATEN,
    REM § Change - BAPI: Zz_Sd_Order_Credit_Release.

    Inherits Base.Business.BankBase ' FDD_Bank_Base


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


    Private Sub createSAPTable(ByRef sapTable As DataTable)
        sapTable = New DataTable

        Dim dataColumns(35) As DataColumn

        dataColumns(0) = New DataColumn("MANDT", System.Type.GetType("System.String"))
        dataColumns(1) = New DataColumn("KONZS", System.Type.GetType("System.String"))
        dataColumns(2) = New DataColumn("KNRZE", System.Type.GetType("System.String"))
        dataColumns(3) = New DataColumn("KUNNR", System.Type.GetType("System.String"))
        dataColumns(4) = New DataColumn("VBELN", System.Type.GetType("System.String"))

        dataColumns(5) = New DataColumn("ERDAT", System.Type.GetType("System.String"))
        dataColumns(6) = New DataColumn("EQUNR", System.Type.GetType("System.String"))
        dataColumns(7) = New DataColumn("ZZFAHRG", System.Type.GetType("System.String"))
        dataColumns(8) = New DataColumn("ZZKENN", System.Type.GetType("System.String"))
        dataColumns(9) = New DataColumn("ZZREFNR", System.Type.GetType("System.String"))

        dataColumns(10) = New DataColumn("ZZBRIEF", System.Type.GetType("System.String"))
        dataColumns(11) = New DataColumn("BSTZD", System.Type.GetType("System.String"))
        dataColumns(12) = New DataColumn("BNAME", System.Type.GetType("System.String"))
        dataColumns(13) = New DataColumn("CMGST", System.Type.GetType("System.String"))
        dataColumns(14) = New DataColumn("ZZBEZAHLT", System.Type.GetType("System.String"))

        dataColumns(15) = New DataColumn("ZZTMPDT", System.Type.GetType("System.String"))
        dataColumns(16) = New DataColumn("ZZANFDT", System.Type.GetType("System.String"))
        dataColumns(17) = New DataColumn("ZZFAEDT", System.Type.GetType("System.String"))
        dataColumns(18) = New DataColumn("CRBLB", System.Type.GetType("System.String"))

        dataColumns(19) = New DataColumn("ZZSPERR_DAD", System.Type.GetType("System.String"))
        dataColumns(20) = New DataColumn("ZZINSOLVENZ", System.Type.GetType("System.String"))
        dataColumns(21) = New DataColumn("TEXT50", System.Type.GetType("System.String"))
        dataColumns(22) = New DataColumn("TEXT200", System.Type.GetType("System.String"))
        dataColumns(23) = New DataColumn("REPLA_DATE", System.Type.GetType("System.String"))

        dataColumns(24) = New DataColumn("NETWR", System.Type.GetType("System.String"))
        dataColumns(25) = New DataColumn("KVGR3", System.Type.GetType("System.String"))
        dataColumns(26) = New DataColumn("ZZFINART", System.Type.GetType("System.String"))
        dataColumns(27) = New DataColumn("ZZTXT2", System.Type.GetType("System.String"))
        dataColumns(28) = New DataColumn("ZZVSNR", System.Type.GetType("System.String"))

        dataColumns(29) = New DataColumn("ANSWT", System.Type.GetType("System.String"))
        dataColumns(30) = New DataColumn("AUGRU", System.Type.GetType("System.String"))
        dataColumns(31) = New DataColumn("ZBE01", System.Type.GetType("System.String"))
        dataColumns(32) = New DataColumn("ZBE02", System.Type.GetType("System.String"))

        dataColumns(33) = New DataColumn("ZBE03", System.Type.GetType("System.String"))
        dataColumns(34) = New DataColumn("ZBE04", System.Type.GetType("System.String"))



        sapTable.Columns.AddRange(dataColumns)
        sapTable.AcceptChanges()

    End Sub
    Public Overrides Sub Show()
        m_strClassAndMethod = "FFE_Bank_OffeneAnforderungen.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True


            If m_tblRaw Is Nothing Then
                createSAPTable(m_tblRaw)
            Else
                m_tblRaw.Clear()
                m_tblAuftraege.Clear()
            End If


            Dim con As New SAPConnection(m_BizTalkSapConnectionString)
            con.Open()
            Try
                Dim cmd As New SAPCommand()
                cmd.Connection = con

                Dim strCom As String

                strCom = "EXEC Z_M_Offene_Anforderungen_Fce @I_KUNNR=@pI_KUNNR,@I_KONZS=@pI_KONZS,"
                strCom = strCom & "@I_KKBER=@pI_KKBER,@I_VKORG=@pI_VKORG,"
                strCom = strCom & "@I_HEZKZ=@pI_HEZKZ,@GT_WEB=@pI_GT_WEB,"
                strCom = strCom & "@GT_WEB=@pO_GT_WEB OUTPUT OPTION 'disabledatavalidation'"

                cmd.CommandText = strCom

                Dim pI_KUNNR As New SAPParameter("@pI_KUNNR", ParameterDirection.Input)
                Dim pI_KONZS As New SAPParameter("@pI_KONZS", ParameterDirection.Input)
                Dim pI_KKBER As New SAPParameter("@pI_KKBER", ParameterDirection.Input)
                Dim pI_VKORG As New SAPParameter("@pI_VKORG", ParameterDirection.Input)
                Dim pI_HEZKZ As New SAPParameter("@pI_HEZKZ", ParameterDirection.Input)

                Dim pI_GT_WEB As New SAPParameter("@pI_GT_WEB", m_tblRaw)
                Dim pO_GT_WEB As New SAPParameter("@pO_GT_WEB", ParameterDirection.Output)

                'Importparameter hinzufügen
                cmd.Parameters.Add(pI_KUNNR)
                cmd.Parameters.Add(pI_KONZS)
                cmd.Parameters.Add(pI_KKBER)
                cmd.Parameters.Add(pI_VKORG)
                cmd.Parameters.Add(pI_HEZKZ)

                'tabelle Hinzufügen
                cmd.Parameters.Add(pI_GT_WEB)
                cmd.Parameters.Add(pO_GT_WEB)


                pI_KKBER.Value = ""
                If m_hez Then
                    pI_HEZKZ.Value = "X"
                    pI_KKBER.Value = "0005"
                Else
                    pI_HEZKZ.Value = ""
                End If


                pI_KONZS.Value = m_strCustomer
                pI_VKORG.Value = "1510"
                pI_KUNNR.Value = "00060" & Right(m_strHaendler, 5)

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                m_intIDSAP = -1
                Dim rowTemp As DataRow

                m_intStatus = 0
                m_strMessage = ""

                cmd.ExecuteNonQuery()

                m_tblRaw = DirectCast(pO_GT_WEB.Value, DataTable)

                '----------------------------------------------------------------
                'Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()

                'Dim tblAuftraegeSAP As New SAPProxy_FFE.ZDAD_M_WEB_AUFTRAEGETable()

                'MakeDestination()
                'objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
                'objSAP.Connection.Open()

                'If m_objLogApp Is Nothing Then
                '    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                'End If
                'm_intIDSAP = -1
                'Dim rowTemp As DataRow
                'Try
                '    m_intStatus = 0
                '    m_strMessage = ""
                '    Dim strKKBER As String = ""
                '    If Not m_tblAuftraege Is Nothing Then m_tblAuftraege.Clear()
                '    m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Offene_Anforderungen", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                '    If (m_hez = False) Then
                '        objSAP.Z_M_Offene_Anforderungen_Fce("", strKKBER, m_strCustomer, "00060" & Right(m_strHaendler, 5), "1510", tblAuftraegeSAP)
                '    Else
                '        strKKBER = "0005"   'für HEZ
                '        objSAP.Z_M_Offene_Anforderungen_Fce("X", strKKBER, m_strCustomer, "00060" & Right(m_strHaendler, 5), "1510", tblAuftraegeSAP)
                '    End If
                '    objSAP.CommitWork()
                '    If m_intIDSAP > -1 Then
                '        m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                '    End If

                '    m_tblRaw = tblAuftraegeSAP.ToADODataTable
                '----------------------------------------------------------------
                m_tblRaw.AcceptChanges()
                For Each rowTemp In m_tblRaw.Rows
                    Select Case rowTemp("BSTZD").ToString
                        Case "0001"
                            rowTemp("BSTZD") = "Standard temporär"
                        Case "0002"
                            rowTemp("BSTZD") = "Standard endgültig"
                        Case "0003"
                            rowTemp("BSTZD") = "Retail"
                        Case "0004"
                            rowTemp("BSTZD") = "Erweitertes Zahlungsziel (Delayed Payment) endgültig"
                        Case "0005"
                            rowTemp("BSTZD") = "Händlereigene Zulassung"
                        Case "0006"
                            rowTemp("BSTZD") = "KF/KL"
                    End Select
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

                If ex.Message.Contains("NO_DATA") Then
                    m_intStatus = 0
                    If m_hez = True Then
                        m_intStatus = -2501
                    End If
                    m_strMessage = "Keine Daten gefunden."
                Else
                    m_intStatus = -9999
                    m_strMessage = ex.Message
                End If
                'Select Case ex.Message
                '    Case "NO_DATA"
                '        m_intStatus = 0
                '        If m_hez = True Then
                '            m_intStatus = -2501
                '        End If
                '        m_strMessage = "Keine Daten gefunden."
                '    Case Else
                '        m_intStatus = -9999
                '        m_strMessage = ex.Message
                'End Select
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
                WriteLogEntry(False, "KONZS=" & m_strCustomer.TrimStart("0"c) & ", KUNNR=" & Right(m_strHaendler, 5) & ", KKBER=" & " , " & Replace(m_strMessage, "<br>", " "), m_tblAuftraege)
            Finally
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                End If


                con.Close()
                '-----------------------------------------
                'objSAP.Connection.Close()
                'objSAP.Dispose()
                '-----------------------------------------

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overrides Sub Change()
        If Not m_blnGestartet Then
            m_blnGestartet = True

            'Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()

            'MakeDestination()
            'objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            'objSAP.Connection.Open()

            Dim con As New SAPConnection(m_BizTalkSapConnectionString)
            con.Open()
            Try
                Dim cmd As New SAPCommand()
                cmd.Connection = con


                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                m_intIDSAP = -1


                m_intStatus = 0
                m_strMessage = ""

                If m_strAuftragsNummer.Trim("0"c).Length = 0 Or (Not m_strAuftragsNummer.Length = 10) Then
                    m_intStatus = -1301
                    m_strMessage = "Keine gültige Auftragsnummer übergeben."
                Else
                    Dim rowFahrzeug() As DataRow = m_tblRaw.Select("VBELN = '" & m_strAuftragsNummer & "'")
                    m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Offeneanforderungen_Storno", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                    Dim strCom As String

                    strCom = "EXEC Z_M_Offene_Anforder_Storno_Fce @I_KUNNR=@pI_KUNNR,@I_KONZS=@pI_KONZS,"
                    strCom = strCom & "@I_EQUNR=@pI_EQUNR,@I_VBELN=@pI_VBELN,"
                    strCom = strCom & "@I_ERNAM=@pI_ERNAM"

                    cmd.CommandText = strCom

                    Dim pI_KUNNR As New SAPParameter("@pI_KUNNR", ParameterDirection.Input)
                    Dim pI_KONZS As New SAPParameter("@pI_KONZS", ParameterDirection.Input)
                    Dim pI_EQUNR As New SAPParameter("@pI_EQUNR", ParameterDirection.Input)
                    Dim pI_VBELN As New SAPParameter("@pI_VBELN", ParameterDirection.Input)
                    Dim pI_ERNAM As New SAPParameter("@pI_ERNAM", ParameterDirection.Input)

                    'Importparameter hinzufügen
                    cmd.Parameters.Add(pI_KUNNR)
                    cmd.Parameters.Add(pI_KONZS)
                    cmd.Parameters.Add(pI_EQUNR)
                    cmd.Parameters.Add(pI_VBELN)
                    cmd.Parameters.Add(pI_ERNAM)

                    pI_KONZS.Value = m_strCustomer
                    pI_VBELN.Value = m_strAuftragsNummer
                    pI_KUNNR.Value = rowFahrzeug(0)("KUNNR").ToString
                    pI_EQUNR.Value = rowFahrzeug(0)("EQUNR").ToString
                    'pI_ERNAM.Value = m_objUser.UserName
                    pI_ERNAM.Value = "1234"

                    cmd.ExecuteNonQuery()



                    'Dim strEQUNR As String = rowFahrzeug(0)("EQUNR").ToString
                    'Dim strKUNNR As String = rowFahrzeug(0)("KUNNR").ToString
                    'Dim strKONZS As String = m_strCustomer
                    'Dim strVBELN As String = m_strAuftragsNummer

                    'objSAP.Z_M_Offene_Anforder_Storno_Fce(strEQUNR, strKONZS, strKUNNR, strVBELN)
                    'objSAP.CommitWork()

                    If m_intIDSAP > -1 Then
                        m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
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
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
            Finally
                con.Close()
                'objSAP.Connection.Close()
                'objSAP.Dispose()
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class
' ************************************************
' $History: FFE_Bank_OffeneAnforderungen.vb $
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 25.05.09   Time: 18:06
' Updated in $/CKAG/Applications/AppFFE/lib
' ZZKKBER wieder in BSTZD ?! Siehe SS historie, ZZKKBER gibt es in der
' Tabelle nicht
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 17.03.09   Time: 10:40
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 27.06.08   Time: 11:18
' Updated in $/CKAG/Applications/AppFFE/lib
' ITA 2043/2032 BugFix Übersicht Benutzeraktivitäten bei Punkt
' Autorisierung 
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 24.06.08   Time: 9:21
' Updated in $/CKAG/Applications/AppFFE/lib
' ITA 2020, 2031 - zusätzlich Z_M_Offene_Anforderungen_Fce auf Biztalk
' umgestellt
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 4.06.08    Time: 18:43
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 21.05.08   Time: 16:34
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 13.05.08   Time: 16:41
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Applications/AppFFE/lib
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 9.04.08    Time: 13:32
' Created in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 9.04.08    Time: 13:06
' Updated in $/CKG/Applications/AppFFE/AppFFEWeb/lib
' ITA: 1790
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 3.04.08    Time: 11:19
' Created in $/CKG/Applications/AppFFE/AppFFEWeb/lib
' ITA 1790
' 
' ************************************************