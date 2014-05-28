Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class Report_200s
    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_datAbmeldedatumVon As DateTime
    Private m_datAbmeldedatumBis As DateTime
#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    'Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal datAbmeldedatumVon As DateTime, ByVal datAbmeldedatumBis As DateTime)
    '    m_strClassAndMethod = "Report_200.FILL"
    '    m_strAppID = strAppID
    '    m_strSessionID = strSessionID
    '    If Not m_blnGestartet Then
    '        m_blnGestartet = True


    '        Dim con As New SAPConnection(m_BizTalkSapConnectionString)

    '        con.Open()
    '        Dim cmd As New SAPCommand()
    '        Dim strCom As String


    '        Dim intID As Int32 = -1

    '        Try
    '            Dim strDatTempVon As String
    '            If IsDate(datAbmeldedatumVon) Then
    '                strDatTempVon = MakeDateSAP(datAbmeldedatumVon)
    '                If strDatTempVon = "10101" Then
    '                    strDatTempVon = "|"
    '                End If
    '            Else
    '                strDatTempVon = "|"
    '            End If
    '            Dim strDatTempBis As String
    '            If IsDate(datAbmeldedatumBis) Then
    '                strDatTempBis = MakeDateSAP(datAbmeldedatumBis)
    '                If strDatTempBis = "10101" Then
    '                    strDatTempBis = "|"
    '                End If
    '            Else
    '                strDatTempBis = "|"
    '            End If

    '            intID = _logApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Abm_Abgemeldete_Kfz", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)



    '            strCom = "EXEC Z_M_Abm_Abgemeldete_Kfz @KUNNR='" & Right("0000000000" & m_objUser.KUNNR, 10) & "'," _
    '                        & "@TIDNR=''," _
    '                        & "@ZFAHRG=''," _
    '                        & "@ZZKENN=''," _
    '                        & "@ABMDATAB='00000000'," _
    '                        & "@ABMDATBI='00000000'," _
    '                        & "@PICKDATAB='" & strDatTempVon & "'," _
    '                        & "@PICKDATBI='" & strDatTempBis & "'," _
    '                        & "@ANZAHL=@Counter OUTPUT," _
    '                        & "@AUSGABE=@ExportFahrzeuge OUTPUT OPTION 'disabledatavalidation'"

    '            cmd.Connection = con
    '            cmd.CommandText = strCom


    '            'Exportparameter
    '            Dim Fahrzeuge As New SAPParameter("@ExportFahrzeuge", ParameterDirection.Output)
    '            cmd.Parameters.Add(Fahrzeuge)

    '            Dim Anzahl As New SAPParameter("@Counter", ParameterDirection.Output)
    '            cmd.Parameters.Add(Anzahl)

    '            'Abschicken
    '            cmd.ExecuteNonQuery()


    '            If intID > -1 Then
    '                _logApp.WriteEndDataAccessSAP(intID, True)
    '            End If

    '            Dim tblTemp2 As DataTable = DirectCast(Fahrzeuge.Value, DataTable)

    '            Me.m_tblResult = tblTemp2
    '            WriteLogEntry(True, "ABMELDEDATAB=" & datAbmeldedatumVon.ToShortDateString & ", ABMELDEDATBI=" & datAbmeldedatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
    '        Catch ex As Exception

    '            m_intStatus = -9999
    '            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten."

    '            If intID > -1 Then
    '                _logApp.WriteEndDataAccessSAP(intID, False, ex.Message)
    '            End If
    '            WriteLogEntry(False, "ABMELDEDATAB=" & datAbmeldedatumVon.ToShortDateString & ", ABMELDEDATBI=" & datAbmeldedatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
    '        Finally
    '            If intID > -1 Then
    '                _logApp.WriteStandardDataAccessSAP(intID)
    '            End If

    '            con.Close()
    '            con.Dispose()

    '            cmd.Dispose()

    '            m_blnGestartet = False
    '        End Try
    '    End If
    'End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal datDurchfuehrungsdatumVon As String, _
                              ByVal datDurchfuehrungsdatumBis As String, _
                              ByVal datAbmeldedatumVon As String, _
                              ByVal datAbmeldedatumBis As String, _
                              ByVal Kennzeichen As String, _
                              ByVal FIN As String, _
                              ByVal Briefnummer As String, _
                              ByVal page As System.Web.UI.Page)
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                m_intStatus = 0
                m_strMessage = ""

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Abm_Abgemeldete_Kfz", m_objApp, m_objUser, page)

                myProxy.setImportParameter("KUNNR", m_objUser.KUNNR.PadLeft(10, "0"c))
                myProxy.setImportParameter("PICKDATAB", datDurchfuehrungsdatumVon)
                myProxy.setImportParameter("PICKDATBI", datDurchfuehrungsdatumBis)
                myProxy.setImportParameter("ABMDATAB", datAbmeldedatumVon)
                myProxy.setImportParameter("ABMDATBI", datAbmeldedatumBis)
                myProxy.setImportParameter("ZFAHRG", FIN)
                myProxy.setImportParameter("ZZKENN", Kennzeichen)
                myProxy.setImportParameter("TIDNR", Briefnummer)



                myProxy.callBapi()

                m_tblResult = myProxy.getExportTable("AUSGABE")



            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = ex.Message

            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub


#End Region
End Class

' ************************************************
' $History: Report_200s.vb $
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 19.11.09   Time: 17:40
' Created in $/CKAG2/Services/Components/ComCommon/Kernel
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 14.10.08   Time: 15:41
' Created in $/CKAG/Components/ComCommon/Kernel
' ************************************************