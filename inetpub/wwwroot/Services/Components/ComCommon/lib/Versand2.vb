Option Explicit On
Option Infer On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Common
Imports CKG.Base.Kernel.Common

Public Class Versand2
    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_strBriefnummer As String
    Private m_datEingangsdatumVon As DateTime
    Private m_datEingangsdatumBis As DateTime
    Private m_strFahrgestellnummer As String
    Private m_strHaendlerID As String
    Private m_tblHistory As DataTable
    Private m_tblResultModelle As DataTable
    Private m_intResultCount As Integer
    Private configurationAppSettings As System.Configuration.AppSettingsReader
#End Region

#Region " Properties"
    Public ReadOnly Property ResultCount() As Integer
        Get
            Return m_intResultCount
        End Get
    End Property

    Public ReadOnly Property ResultModelle() As DataTable
        Get
            Return m_tblResultModelle
        End Get
    End Property

    Public ReadOnly Property History() As DataTable
        Get
            Return m_tblHistory
        End Get
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
        Fill(m_strAppID, m_strSessionID, m_strBriefnummer, m_datEingangsdatumVon, m_datEingangsdatumBis, m_strFahrgestellnummer, m_strHaendlerID)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal strBriefnummer As String, ByVal datEingangsdatumVon As DateTime, ByVal datEingangsdatumBis As DateTime, ByVal strFahrgestellnummer As String, ByVal strHaendlerID As String)

    End Sub

    Public Sub FillHistory(ByVal strAppID As String, ByVal strSessionID As String, ByVal strAmtlKennzeichen As String, ByVal strFahrgestellnummer As String, ByVal strBriefnummer As String, ByVal strOrdernummer As String)
        m_strClassAndMethod = "Leaseplan_07.FillHistory"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1

            Try
                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Fahrzeugbriefhistorie", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

                Dim proxy = DynSapProxy.getProxy("Z_M_Fahrzeugbriefhistorie", m_objApp, m_objUser, PageHelper.getcurrentpage)
                proxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                proxy.setImportParameter("I_ZZFAHRG", UCase(strFahrgestellnummer))
                proxy.setImportParameter("I_ZZREF1", UCase(strOrdernummer))
                proxy.setImportParameter("I_TIDNR", "")

                proxy.callBapi()

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                m_intResultCount = CInt(proxy.getExportParameter("E_COUNTER"))
                m_tblHistory = proxy.getExportTable("GT_WEB")

                Dim row As DataRow
                If m_intResultCount > 1 Then
                    m_tblResult = proxy.getExportTable("ET_FAHRG")
                    m_tblResult.Columns.Add("DISPLAY")
                    If m_tblResult.Rows.Count > 0 Then

                        For Each row In m_tblResult.Rows
                            Dim strTemp As String = CStr(row("ZZFAHRG")) & ", " & CStr(row("LIZNR")) & ", " & CStr(row("ZZKENN"))
                            row("DISPLAY") = strTemp
                        Next
                    End If
                End If

                '§§§ JVE 26.04.2006: Abmeldedatum formatieren
                For Each row In m_tblHistory.Rows
                    If Not (TypeOf row("EXPIRY_DATE") Is DBNull) Then
                        If Not ((row("EXPIRY_DATE").ToString = String.Empty) Or (row("EXPIRY_DATE").ToString.Trim("0"c) = String.Empty)) Then
                            row("EXPIRY_DATE") = MakeDateStandard(row("EXPIRY_DATE").ToString).ToShortDateString
                        End If
                    End If
                Next

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", ZZBRIEF=" & UCase(strBriefnummer) & ", ZFAHRG=" & UCase(strFahrgestellnummer) & ", ZZREF1=" & UCase(strOrdernummer) & ", ZZKENN=" & UCase(strAmtlKennzeichen) & ", Count=" & m_intResultCount.ToString, m_tblHistory, False)
            Catch ex As Exception
                m_intResultCount = 0
                m_intStatus = -2222
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten gefunden."
                    Case "NO_WEB"
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case Else
                        m_strMessage = ex.Message
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ZZBRIEF=" & UCase(strBriefnummer) & ", ZFAHRG=" & UCase(strFahrgestellnummer) & ", ZZREF1=" & UCase(strOrdernummer) & ", ZZKENN=" & UCase(strAmtlKennzeichen) & ", " & Replace(m_strMessage, "<br>", " "), m_tblHistory, False)
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If

                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class
' ************************************************
' $History: Versand2.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 17.09.09   Time: 16:38
' Created in $/CKAG2/Services/Components/ComCommon/lib
' ITA: 3112
' 
