Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel

Public Class CK_02
    REM § Status-Report, Zulassungen
    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_strReferenz As String
    Private m_strKennzeichen As String
    Private m_Kunnr As String
    Private m_strLoesch As String
    Private m_datZulassungssdatumVon As DateTime
    Private m_datZulassungssdatumBis As DateTime
    Private m_tblOutput As DataTable
#End Region

#Region " Properties"
    Public ReadOnly Property History() As DataTable
        Get
            Return m_tblOutput
        End Get
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub Fill()
        Fill(m_strAppID, m_strSessionID, m_strReferenz, m_strKennzeichen, m_datZulassungssdatumVon, m_datZulassungssdatumBis, m_strLoesch, m_Kunnr)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, _
                            ByVal datReferenz As String, ByVal datKennzeichen As String, _
                            ByVal datZulassungssdatumVon As DateTime, ByVal datZulassungssdatumBis As DateTime, _
                            ByVal datLoesch As String, ByVal sKunnr As String)

        m_strClassAndMethod = "CK_02.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True

            'Dim intID As Int32 = -1

            Try

                Dim strDatZulVon As String
                If IsDate(datZulassungssdatumVon) Then
                    strDatZulVon = datZulassungssdatumVon.ToShortDateString
                    If strDatZulVon = "10101" Then
                        strDatZulVon = "|"
                    End If
                Else
                    strDatZulVon = "|"
                End If
                Dim strDatZulBis As String
                If IsDate(datZulassungssdatumBis) Then
                    strDatZulBis = datZulassungssdatumBis.ToShortDateString
                    If strDatZulBis = "10101" Then
                        strDatZulBis = "|"
                    End If
                Else
                    strDatZulBis = "|"
                End If

                'intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_V_Kcl_Zulassungsdaten", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

                'Dim myproxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_Kcl_Zulassungsdaten", m_objApp, m_objUser, page)

                S.AP.Init("Z_V_Kcl_Zulassungsdaten")

                S.AP.SetImportParameter("KUNNR", Right("0000000000" & sKunnr.Trim, 10))
                S.AP.SetImportParameter("ZZREFNR", datReferenz)
                S.AP.SetImportParameter("ZZKENN", datKennzeichen)
                S.AP.SetImportParameter("ZZZLDAT_VON", strDatZulVon)
                S.AP.SetImportParameter("ZZZLDAT_BIS", strDatZulBis)
                S.AP.SetImportParameter("ZZLOESCH", datLoesch)

                'myproxy.callBapi()

                S.AP.Execute()

                'If intID > -1 Then
                '    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                'End If

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("EXTAB")

                CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", ERDAT_LOW=" & datZulassungssdatumVon.ToShortDateString & ", ERDAT_HIGH=" & datZulassungssdatumVon.ToShortDateString, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -5555
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_FLEET"
                        m_strMessage = "Keine Fleet Daten vorhanden."
                    Case "NO_WEB"
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case "NO_DATA"
                        m_strMessage = "Keine Eingabedaten gefunden."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                'If intID > -1 Then
                '    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                'End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ERDAT_LOW=" & datZulassungssdatumVon.ToShortDateString & ", ERDAT_HIGH=" & datZulassungssdatumBis.ToShortDateString & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                'If intID > -1 Then
                '    m_objLogApp.WriteStandardDataAccessSAP(intID)
                'End If

                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: CK_02.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 14:30
' Created in $/CKAG/Applications/AppKroschke/Lib
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 11.07.07   Time: 9:20
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Lib
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 6.07.07    Time: 14:29
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Lib
' ITA: 1130
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 18:01
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 8.03.07    Time: 9:25
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Lib
' 
' ************************************************
