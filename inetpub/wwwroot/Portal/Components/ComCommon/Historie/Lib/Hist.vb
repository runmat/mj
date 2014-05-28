Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class Hist
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
        FILL(m_strAppID, m_strSessionID, m_strBriefnummer, m_datEingangsdatumVon, m_datEingangsdatumBis, m_strFahrgestellnummer, m_strHaendlerID)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal strBriefnummer As String, ByVal datEingangsdatumVon As DateTime, ByVal datEingangsdatumBis As DateTime, ByVal strFahrgestellnummer As String, ByVal strHaendlerID As String)

    End Sub

    Public Sub FillHistory(ByVal strAppID As String, ByVal strSessionID As String, ByVal strAmtlKennzeichen As String, ByVal strFahrgestellnummer As String, ByVal strBriefnummer As String, ByVal strOrdernummer As String)
        m_strClassAndMethod = "Hist.FillHistory"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                S.AP.Init("Z_M_FAHRZEUGBRIEFHISTORIE")
                S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                S.AP.SetImportParameter("I_ZZKENN", strAmtlKennzeichen)
                S.AP.SetImportParameter("I_ZZFAHRG", strFahrgestellnummer)
                S.AP.SetImportParameter("I_ZZREF1", strOrdernummer)
                S.AP.SetImportParameter("I_TIDNR", strBriefnummer)
                S.AP.Execute()

                m_tblHistory = S.AP.GetExportTable("GT_WEB")

                Dim row As DataRow
                If m_intResultCount > 1 Then
                    m_tblResult = S.AP.GetExportTable("ET_FAHRG")
                    m_tblResult.Columns.Add("DISPLAY")
                    If m_tblResult.Rows.Count > 0 Then

                        For Each row In m_tblResult.Rows
                            Dim strTemp As String = CStr(row("ZZFAHRG")) & ", " & CStr(row("LIZNR")) & ", " & CStr(row("ZZKENN"))
                            row("DISPLAY") = strTemp
                        Next
                    End If
                End If

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", ZZBRIEF=" & UCase(strBriefnummer) & ", ZFAHRG=" & UCase(strFahrgestellnummer) & ", ZZREF1=" & UCase(strOrdernummer) & ", ZZKENN=" & UCase(strAmtlKennzeichen) & ", Count=" & m_intResultCount.ToString, m_tblHistory, False)
            Catch ex As Exception
                m_intResultCount = 0
                m_intStatus = -2222
                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten gefunden."
                    Case "NO_WEB"
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case Else
                        m_strMessage = ex.Message
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ZZBRIEF=" & UCase(strBriefnummer) & ", ZFAHRG=" & UCase(strFahrgestellnummer) & ", ZZREF1=" & UCase(strOrdernummer) & ", ZZKENN=" & UCase(strAmtlKennzeichen) & ", " & Replace(m_strMessage, "<br>", " "), m_tblHistory, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class
