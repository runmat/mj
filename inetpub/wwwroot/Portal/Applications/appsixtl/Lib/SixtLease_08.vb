Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class SixtLease_08
    REM § Status-Report, Kunde: Arval, BAPI: Z_M_UNZUGELASSENE_FZGE_ARVAL,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase

#Region " Declarations"

#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

   
    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "SixtLease_08.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Unzugelassene_Fzge_Sixt_L", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Unzugelassene_Fzge_Sixt_L", "I_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            'Dim tblTemp2 As DataTable = myProxy.getExportTable("T_DATA")
            Dim tblTemp2 As DataTable = S.AP.GetExportTable("T_DATA")

            CreateOutPut(tblTemp2, strAppID)

            Dim row As DataRow

            For Each row In m_tblResult.Rows
                row("Leasingvertragsnummer") = String.Empty
            Next

            'Spalte "Status" einfügen...
            m_tblResult.Columns.Add("Status", GetType(System.String))
            'Spalte "Auswahl" einfügen
            m_tblResult.Columns.Add("Auswahl", GetType(System.String))
            m_tblResult.AcceptChanges()

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)

        Catch ex As Exception
            m_intStatus = -4444

            Select Case ex.Message
                Case "ERR_NO_DATA"
                    m_intStatus = -1111
                    m_strMessage = "Keine Fahrzeuge gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try

    End Sub

    Public Sub Absenden(ByRef status As String, ByVal equnr As String, ByVal lvnr As String, ByVal page As Page)
        m_strClassAndMethod = "SixtLease_08.Absenden"
        m_strAppID = AppID.ToString
        m_strSessionID = SessionID.ToString

        Dim intStatus As String

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Eingabe_Lvnummer_Sixtleas", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("LF_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("LF_EQUNR", equnr)
            'myProxy.setImportParameter("LF_LIZNR", lvnr)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Eingabe_Lvnummer_Sixtleas", "LF_KUNNR, LF_EQUNR, LF_LIZNR", Right("0000000000" & m_objUser.KUNNR, 10), equnr, lvnr)

            'intStatus = myProxy.getExportParameter("LF_RETURN")
            intStatus = S.AP.GetExportParameter("LF_RETURN")

            Select Case intStatus
                Case "0"
                    status = "Vorgang OK"
                Case "5"
                    status = "FEHLER: LV-Nr. bereits vorhanden."
                Case Else
                    status = "WARNUNG: Status unbekannt."
            End Select
            
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        Catch ex As Exception
            m_intStatus = -9999
            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"

            status = "Unbekannter Fehler!"
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try


    End Sub

    Public Sub sendMail(ByVal tblResult As DataTable, ByRef status As String)
        'Mailversand...
        Dim mailAdresse As String
        Dim row As DataRow
        Dim lvnr As String
        Dim fin As String
        Dim mailBody As String

        status = String.Empty
        mailBody = ConfigurationManager.AppSettings("SmtpMailTextFehlendeLVNR").ToString & vbCrLf & vbCrLf

        For Each row In tblResult.Rows
            If (row("Auswahl").ToString = "X") Then
                If (row("Status").ToString = "Vorgang OK") Then
                    lvnr = row("Leasingvertragsnummer").ToString
                    fin = row("Fahrgestellnummer").ToString
                    mailBody &= "Fahrgestellnummer:" & fin & " - Leasingvertragsnummer:" & lvnr & vbCrLf
                End If
            End If
        Next

        Try

            Dim Mail As System.Net.Mail.MailMessage
            Dim smtpMailSender As String = ""
            Dim smtpMailServer As String = ""

            smtpMailSender = ConfigurationManager.AppSettings("SmtpMailSender")
            mailAdresse = ConfigurationManager.AppSettings("SmtpMailAddress")
            Mail = New System.Net.Mail.MailMessage(smtpMailSender, mailAdresse, "Sixt Leasing LV-Nr. Erfassung (Benutzer: " & m_objUser.UserName & ")", mailBody)

            smtpMailServer = ConfigurationManager.AppSettings("SmtpMailServer")
            Dim client As New System.Net.Mail.SmtpClient(smtpMailServer)
            client.Send(Mail)

        Catch ex As Exception
            status = "Fehler bei eMail-Versand:<br>" & ex.Message
        End Try
    End Sub

#End Region
End Class

' ************************************************
' $History: SixtLease_08.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 10:47
' Updated in $/CKAG/Applications/appsixtl/Lib
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:27
' Created in $/CKAG/Applications/appsixtl/Lib
' 
' *****************  Version 9  *****************
' User: Uha          Date: 3.07.07    Time: 9:34
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 8  *****************
' User: Uha          Date: 8.03.07    Time: 13:42
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Lib
' 
' ************************************************
