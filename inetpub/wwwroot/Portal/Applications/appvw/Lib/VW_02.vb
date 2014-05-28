Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts

Public Class VW_02
    REM § BAPI: Z_M_Fahrzeugbriefhistorie_Vw (FillHistory),
    REM § Direkte Rückgabe (FillHistory).

    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_tblHistory As DataTable
    Private m_intResultCount As Integer
#End Region

#Region " Properties"
    Public ReadOnly Property ResultCount() As Integer
        Get
            Return m_intResultCount
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
        FILL(m_strAppID, m_strSessionID)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)

    End Sub

    Public Sub FillHistory(ByVal strAppID As String, ByVal strSessionID As String, ByVal datBriefeingangVon As DateTime, ByVal datBriefeingangBis As DateTime, ByVal strFahrgestellnummer As String, ByVal strReferenznummer As String)
        m_strClassAndMethod = "VW_02.FillHistory"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            'Dim objSAP As New SAPProxy_VW.SAPProxy_VW()
            'objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            'objSAP.Connection.Open()

            Dim intID As Int32 = -1

            Try
                'Dim SAPTable As New SAPProxy_VW.ZDAD_M_WEB_BRIEFDATEN_VWTable()
                'Dim SAPTableFleet As New SAPProxy_VW.ZMEHRFACHAUSWAHLTable()

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Fahrzeugbriefhistorie_Vw", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

                S.AP.InitExecute("Z_M_Fahrzeugbriefhistorie_Vw",
                                    "I_KUNNR, I_ERDAT_VON, I_ERDAT_BIS, I_ZZFAHRG, I_ZZREF1",
                                    m_objUser.KUNNR.ToSapKunnr, datBriefeingangVon, datBriefeingangBis, UCase(strFahrgestellnummer), UCase(strReferenznummer))
                m_tblHistory = S.AP.GetExportTable("GT_WEB")
                m_tblResult = S.AP.GetExportTable("ET_FAHRG")
                m_intResultCount = m_tblHistory.Rows.Count

                'objSAP.Z_M_Fahrzeugbriefhistorie_Vw(MakeDateSAP(datBriefeingangBis), MakeDateSAP(datBriefeingangVon), Right("0000000000" & m_objUser.KUNNR, 10), UCase(strFahrgestellnummer), UCase(strReferenznummer), m_intResultCount, SAPTableFleet, SAPTable)
                'objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                'm_tblHistory = SAPTable.ToADODataTable

                If m_intResultCount > 1 Then

                    'm_tblResult = SAPTableFleet.ToADODataTable

                    m_tblResult.Columns.Add("Briefeingang", System.Type.GetType("System.DateTime"))
                    m_tblResult.Columns.Add("Zulassungsdatum", System.Type.GetType("System.DateTime"))
                    If m_tblResult.Rows.Count > 0 Then
                        Dim row As DataRow
                        For Each row In m_tblResult.Rows
                            If IsNumeric(row("Erdat")) AndAlso CInt(row("Erdat")) > 0 Then
                                'row("Briefeingang") = MakeDateStandard(CStr(row("Erdat")))
                                row("Briefeingang") = row("Erdat")
                            End If
                            If IsNumeric(row("Repla_Date")) AndAlso CInt(row("Repla_Date")) > 0 Then
                                'row("Zulassungsdatum") = MakeDateStandard(CStr(row("Repla_Date")))
                                row("Zulassungsdatum") = row("Repla_Date")
                            End If
                        Next
                    End If
                End If
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", Briefeingang Von=" & datBriefeingangVon.ToShortDateString & ", Briefeingang Bis=" & datBriefeingangBis.ToShortDateString & ", Fahrgestellnummer=" & UCase(strFahrgestellnummer) & ", Referenznummer=" & UCase(strReferenznummer) & ", Count=" & m_intResultCount.ToString, m_tblHistory, False)
            Catch ex As Exception
                m_intResultCount = 0
                m_intStatus = -2222
                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Kein Brief gefunden."
                    Case "NO_WEB"
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case "NO_CLASSIFICATION"
                        m_strMessage = "Keine Klassifizierung des Fahrzeugs."
                    Case Else
                        m_strMessage = ex.Message
                End Select
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", Briefeingang Von=" & datBriefeingangVon.ToShortDateString & ", Briefeingang Bis=" & datBriefeingangBis.ToShortDateString & ", Fahrgestellnummer=" & UCase(strFahrgestellnummer) & ", Referenznummer=" & UCase(strReferenznummer) & ", " & Replace(m_strMessage, "<br>", " "), m_tblHistory, False)
            Finally
                If intID > -1 Then
                    m_objlogApp.WriteStandardDataAccessSAP(intID)
                End If

                'objSAP.Connection.Close()
                'objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: VW_02.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:49
' Created in $/CKAG/Applications/appvw/Lib
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 3.07.07    Time: 9:13
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 5  *****************
' User: Uha          Date: 8.03.07    Time: 15:06
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' 
' ************************************************
