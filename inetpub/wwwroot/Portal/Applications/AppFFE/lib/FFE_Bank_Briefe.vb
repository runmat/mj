Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports Microsoft.Data.SAPClient
Public Class FFE_Bank_Briefe
    REM § Status-Report, Kunde: FFD, BAPI: Z_M_Briefe_Ohne_Datenimport,
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

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "FFD_Bank_Briefe.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim intID As Int32 = -1

            Try
                Dim SAPTable As New SAPProxy_FFE.ZDAD_M_WEB_EQUIDATENTable()
                Dim strKUNNR As String = ""

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Briefe_Ohne_Datenimport", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Briefe_Ohne_Datenimp_Fce(m_strFiliale, Right("0000000000" & m_objUser.KUNNR, 10), strKUNNR, "1510", SAPTable)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp2 As DataTable = SAPTable.ToADODataTable

                CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "KNRZE=" & m_strFiliale & ", KONZS=" & m_objUser.KUNNR & ", KUNNR=", m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -1111
                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Keine Eingabedaten vorhanden."
                    Case "NO_WEB"
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case "NO_HAENDLER"
                        m_strMessage = "Händler nicht vorhanden."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KNRZE=" & m_strFiliale & ", KONZS=" & m_objUser.KUNNR & ", KUNNR= , " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
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

    Public Overloads Sub FillData(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "FFD_Bank_Datenimport.FillData"
        Dim cmd As New SAPCommand()
        Dim strCom As String
        Dim intID As Int32 = -1

        Dim strKUNNR As String = ""
        Dim con As New SAPConnection(m_BizTalkSapConnectionString)
        con.Open()
        Try
            If Not m_blnGestartet Then
                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_DATENIMP_OHNE_BRIEFE_FCE", Me.m_strAppID, Me.m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                strCom = "EXEC Z_M_DATENIMP_OHNE_BRIEFE_FCE @I_KUNNR='" & strKUNNR & "'," _
                                                       & "@I_KNRZE='" & m_strFiliale & "'," _
                                                       & "@I_KONZS='" & Right("0000000000" & m_objUser.KUNNR, 10) & "'," _
                                                       & "@I_VKORG='1510'," _
                                                       & "@GT_WEB=@pSAPTable OUTPUT OPTION 'disabledatavalidation'"



                cmd.Connection = con
                cmd.CommandText = strCom

                Dim pSAPTable As New SAPParameter("@pSAPTable", ParameterDirection.Output)
                cmd.Parameters.Add(pSAPTable)


                cmd.ExecuteNonQuery()

                Dim SAPTable As DataTable = DirectCast(pSAPTable.Value, DataTable)

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp2 As DataTable = SAPTable.Copy

                Dim datarow As DataRow

                For Each datarow In tblTemp2.Rows
                    If datarow("ZZHAENR").ToString.TrimStart("0"c).StartsWith("60") Then
                        datarow("ZZHAENR") = datarow("ZZHAENR").ToString.TrimStart("0"c).Substring(2, datarow("ZZHAENR").ToString.TrimStart("0"c).Length - 2)
                    End If
                Next

                CreateOutPut(tblTemp2, strAppID)

                WriteLogEntry(True, "KNRZE=" & m_strFiliale & ", KONZS=" & m_objUser.KUNNR & ", KUNNR=", m_tblResult, False)
            End If
            Catch ex As Exception
            m_intStatus = -1111
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Keine Eingabedaten vorhanden."
                Case "NO_WEB"
                    m_strMessage = "Keine Web-Tabelle erstellt."
                Case "NO_HAENDLER"
                    m_strMessage = "Keine Ergebnisse gefunden."
                    'm_strMessage = "Händler nicht vorhanden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "KNRZE=" & m_strFiliale & ", KONZS=" & m_objUser.KUNNR & ", KUNNR= , " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If
            con.Close()
            con.Dispose()
            cmd.Dispose()
            m_blnGestartet = False
        End Try
    End Sub
#End Region
End Class
' ************************************************
' $History: FFE_Bank_Briefe.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 30.07.08   Time: 10:30
' Updated in $/CKAG/Applications/AppFFE/lib
' Leerzeichen Fahrgestellnummer entfernt
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 29.07.08   Time: 15:40
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 7.07.08    Time: 10:40
' Updated in $/CKAG/Applications/AppFFE/lib
' SSV Historien hinzugefgt
' 
' ************************************************
