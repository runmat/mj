Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Common
Imports CKG.Base.Business

<Serializable()> Public Class FFD_Bank_OffeneAnforderungen
    REM § Lese-/Schreibfunktion, Kunde: FFD,
    REM § Show - BAPI: Z_M_Offene_Anforderungen,
    REM § Change - BAPI: Z_M_Offeneanforderungen_Storno.

    Inherits Base.Business.BankBase

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
        m_hez = hez
    End Sub

    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "FFD_Bank_OffeneAnforderungen.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True
            m_intIDSAP = -1
            Dim rowTemp As DataRow

            Try
                m_intStatus = 0
                m_strMessage = ""
                'Dim strKKBER As String = Nothing



                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Offene_Anforderungen", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR", "00060" & Right(m_strHaendler, 5))
                'myProxy.setImportParameter("I_KONZS", m_strCustomer)

                'myProxy.setImportParameter("I_VKORG", "1510")

                S.AP.Init("Z_M_Offene_Anforderungen", "I_KUNNR,I_KONZS,I_VKORG", "00060" & Right(m_strHaendler, 5), m_strCustomer, "1510")

                'If (m_hez = False) Then
                '    myProxy.setImportParameter("I_HEZKZ", "")
                '    myProxy.setImportParameter("I_KKBER", strKKBER)
                'Else
                '    myProxy.setImportParameter("I_HEZKZ", "X")
                '    myProxy.setImportParameter("I_KKBER", strKKBER)
                'End If

                If m_hez Then
                    S.AP.SetImportParameter("I_HEZKZ", "X")
                    S.AP.SetImportParameter("I_KKBER", "0005")
                End If

                'myProxy.callBapi()
                S.AP.Execute()

                m_tblRaw = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")
                m_tblRaw.AcceptChanges()
                m_tblRaw.Columns("BSTZD").MaxLength = 75

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
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_intStatus = 0

                        If Not m_tblAuftraege Is Nothing Then
                            m_tblAuftraege.Clear()
                        End If

                        If m_hez = True Then
                            m_intStatus = -2501
                        End If
                        m_strMessage = "Keine Daten vorhanden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                WriteLogEntry(False, "KONZS=" & m_strCustomer.TrimStart("0"c) & ", KUNNR=" & Right(m_strHaendler, 5) & ", KKBER=" & " , " & Replace(m_strMessage, "<br>", " "), m_tblAuftraege)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overrides Sub Show()

    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""

                If m_strAuftragsNummer.Trim("0"c).Length = 0 Or (Not m_strAuftragsNummer.Length = 10) Then
                    m_intStatus = -1301
                    m_strMessage = "Keine gültige Auftragsnummer übergeben."
                Else
                    Dim rowFahrzeug() As DataRow = m_tblRaw.Select("VBELN = '" & m_strAuftragsNummer & "'")

                    If rowFahrzeug.Length > 0 Then

                        m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Offeneanforderungen_Storno", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                        Dim strEQUNR As String = rowFahrzeug(0)("EQUNR").ToString
                        Dim strKUNNR As String = rowFahrzeug(0)("KUNNR").ToString
                        Dim strKONZS As String = m_strCustomer
                        Dim strVBELN As String = m_strAuftragsNummer


                        'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Offeneanforderungen_Storno", m_objApp, m_objUser, page)

                        'myProxy.setImportParameter("I_KUNNR", strKUNNR)
                        'myProxy.setImportParameter("I_KONZS", strKONZS)
                        'myProxy.setImportParameter("I_EQUNR", strEQUNR)
                        'myProxy.setImportParameter("I_VBELN", strVBELN)

                        'myProxy.callBapi()

                        S.AP.InitExecute("Z_M_Offeneanforderungen_Storno", "I_KUNNR,I_KONZS,I_EQUNR,I_VBELN", strKUNNR, strKONZS, strEQUNR, strVBELN)

                    End If
                End If
            Catch ex As Exception
                Select Case ex.Message.Replace("Execution failed", "").Trim()
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
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overrides Sub Change()

    End Sub

#End Region

End Class

' ************************************************
' $History: FFD_Bank_OffeneAnforderungen.vb $
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 1.07.09    Time: 10:05
' Updated in $/CKAG/Applications/appffd/Lib
' ITA: 2956
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 26.06.09   Time: 10:33
' Updated in $/CKAG/Applications/appffd/Lib
' ITA: 2918
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Lib
' ITA: 2837
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 23.12.08   Time: 9:41
' Updated in $/CKAG/Applications/appffd/Lib
' ITA 2504 testfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 21.04.08   Time: 12:59
' Updated in $/CKAG/Applications/appffd/Lib
' BugFix Offene Anforderungen-Storno
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 17:40
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 13.06.07   Time: 17:03
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Abgleich Portal - Startapplication 13.06.2005
' 
' *****************  Version 3  *****************
' User: Uha          Date: 7.03.07    Time: 12:51
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' 
' ************************************************
