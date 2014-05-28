Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class FFE_Bank_Imports
    Inherits Base.Business.DatenimportBase



#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
        FILL(m_strAppID, m_strSessionID)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)

        m_strClassAndMethod = "FFE_Bank_Imports.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim intID As Int32 = -1

            Try
                Dim SAPTable As New SAPProxy_FFE.ZDAD_WEB_FCEIMPORT_WORKFTable()

                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Ffdimports_Anzeigen", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Imports_Anzeigen_Fce(Right("0000000000" & m_objUser.KUNNR, 10), "Report44.aspx", SAPTable)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp2 As DataTable
                tblTemp2 = SAPTable.ToADODataTable

                Dim row As DataRow
                Dim strZeit As String

                For Each row In tblTemp2.Rows
                    Try
                        strZeit = CStr(row("UZEIT"))
                        If strZeit <> String.Empty Then
                            strZeit = Left(strZeit, 2) & ":" & strZeit.Substring(2, 2) & ":" & Right(strZeit, 2)
                        End If
                        row("UZEIT") = strZeit
                        row("ZDATENSAETZE") = CStr(row("ZDATENSAETZE")).TrimStart("0"c)
                        row("ZBEZAHLTE") = CStr(row("ZBEZAHLTE")).TrimStart("0"c)
                        row("ZUMFINANZ") = CStr(row("ZUMFINANZ")).TrimStart("0"c)
                        row("ERDAT") = FormatDateTime(MakeDateStandard(row("ERDAT").ToString), DateFormat.ShortDate)
                        row("UDATUM") = FormatDateTime(MakeDateStandard(row("UDATUM").ToString), DateFormat.ShortDate)


                        Select Case row("ERFOLG").ToString
                            Case "X"
                                row("ERFOLG") = "<img src=""/Portal/Images/erfolg.gif"">"
                            Case "W"
                                row("ERFOLG") = "<img src=""/Portal/Images/warnung.gif"">"
                            Case "F"
                                row("ERFOLG") = "<img src=""/Portal/Images/fehler.gif"">"
                        End Select
                        '----------------------------------------------------------
                    Catch ex As Exception
                        row("ERFOLG") = "Fehler."
                    End Try
                Next

                'CreateOutPut(tblTemp2, strAppID)
                ResultTable = tblTemp2
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -4444
                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten gefunden."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class
' ************************************************
' $History: FFE_Bank_Imports.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 7.07.08    Time: 10:40
' Updated in $/CKAG/Applications/AppFFE/lib
' SSV Historien hinzugef�gt
' 
' ************************************************
