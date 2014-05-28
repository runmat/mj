Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel

Public Class DCB_444
    REM § BAPI: Z_M_FAHRZEUGBRIEFHISTORIE_001 (FillHistory),
    REM § Direkte Rückgabe (FillHistory).

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

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
        Fill(m_strAppID, m_strSessionID, m_strBriefnummer, m_datEingangsdatumVon, m_datEingangsdatumBis, m_strFahrgestellnummer, m_strHaendlerID)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal strBriefnummer As String,
                              ByVal datEingangsdatumVon As DateTime, ByVal datEingangsdatumBis As DateTime, ByVal strFahrgestellnummer As String,
                              ByVal strHaendlerID As String)

    End Sub

    Public Overloads Sub FillHistory(ByVal strAppID As String, ByVal strSessionID As String,
                              ByVal strAmtlKennzeichen As String, ByVal strFahrgestellnummer As String,
                              ByVal strBriefnummer As String)

        m_strClassAndMethod = "DCB_444.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try

                Dim m_strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

                S.AP.Init("Z_M_FAHRZEUGBRIEFHISTORIE_001")

                S.AP.SetImportParameter("I_KUNNR", m_strKUNNR)
                S.AP.SetImportParameter("I_ZZKENN", UCase(strAmtlKennzeichen))
                S.AP.SetImportParameter("I_ZZFAHRG", UCase(strFahrgestellnummer))
                S.AP.SetImportParameter("I_TIDNR", UCase(strBriefnummer))

                S.AP.Execute()

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_FAHRZEUGBRIEFHISTORIE_001", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR", m_strKUNNR)
                'myProxy.setImportParameter("I_ZZKENN", UCase(strAmtlKennzeichen))
                'myProxy.setImportParameter("I_ZZFAHRG", UCase(strFahrgestellnummer))
                'myProxy.setImportParameter("I_TIDNR", UCase(strBriefnummer))

                'myProxy.callBapi()

                Dim StrResult As String = S.AP.GetExportParameter("E_COUNTER") 'myProxy.getExportParameter("E_COUNTER")

                If IsNumeric(StrResult) Then
                    m_intResultCount = CType(StrResult, Integer)
                End If

                m_tblHistory = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")
                m_tblResult = S.AP.GetExportTable("ET_FAHRG") 'myProxy.getExportTable("ET_FAHRG")

                If m_intResultCount > 1 Then
                    m_tblResult.Columns.Add("DISPLAY")
                    If m_tblResult.Rows.Count > 0 Then
                        Dim row As DataRow
                        For Each row In m_tblResult.Rows
                            Dim strTemp As String = CStr(row("ZZFAHRG")) & ", " & CStr(row("TIDNR")) & ", " & CStr(row("ZZKENN"))
                            row("DISPLAY") = strTemp
                        Next
                    End If
                End If

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", ZZBRIEF=" & UCase(strBriefnummer) & ", ZFAHRG=" & UCase(strFahrgestellnummer) &
                              ", ZZKENN=" & UCase(strAmtlKennzeichen) & ", Count=" & m_intResultCount.ToString, m_tblHistory, False)

            Catch ex As Exception

                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_intStatus = -1111
                        m_strMessage = "Keine Fahrzeuge gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ZZBRIEF=" & UCase(strBriefnummer) & ", ZFAHRG=" & UCase(strFahrgestellnummer) & ", ZZKENN=" & UCase(strAmtlKennzeichen) & ", " & Replace(m_strMessage, "<br>", " "), m_tblHistory, False)
            Finally
                m_blnGestartet = False
            End Try
        End If

    End Sub

#End Region

End Class

' ************************************************
' $History: DCB_444.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.02.10    Time: 13:56
' Updated in $/CKAG/Applications/appdcb/Lib
' ITA: 2918
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 11:52
' Created in $/CKAG/Applications/appdcb/Lib
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 16:50
' Updated in $/CKG/Applications/AppDCB/AppDCBWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 7.03.07    Time: 9:53
' Updated in $/CKG/Applications/AppDCB/AppDCBWeb/Lib
' 
' ************************************************
