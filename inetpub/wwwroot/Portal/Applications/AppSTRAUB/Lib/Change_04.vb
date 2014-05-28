Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change_04

    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_tblModell As DataTable
#End Region

#Region " Properties"
    Public ReadOnly Property ModellTable() As DataTable
        Get
            Return m_tblModell
        End Get
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App)
        MyBase.New(objUser, objApp, "")
    End Sub

    Public Overloads Overrides Sub Fill()
        FILL(m_strAppID, m_strSessionID)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "Change_04.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_STRAUB.SAPProxy_STRAUB()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim intID As Int32 = -1

            Try
                Dim SAPTable As New SAPProxy_STRAUB.ZDAD_M_WEB_REPORTING_SIXTTable()
                Dim SAPTableText As New SAPProxy_STRAUB.ZDAD_M_WEB_MELDUNGSTEXTE_SIXTTable()

                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Meldungen_Pdi1", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Meldungen_Pdi1(Right("0000000000" & m_objUser.KUNNR, 10), "2", "1510", "", SAPTableText, SAPTable)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp2 As DataTable = SAPTable.ToADODataTable
                '§§§ JVE 23.08.2006: Modellcode 
                tblTemp2.Columns.Add("Modellcode", System.Type.GetType("System.String"))
                '------------------------------
                tblTemp2.Columns.Add("Status", System.Type.GetType("System.String"))
                m_tblModell = tblTemp2

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -5555
                Select Case ex.Message
                    Case "NO_FLEET"
                        m_strMessage = "Keine Fleet Daten vorhanden."
                    Case "NO_WEB"
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case "NO_DATA"
                        m_strMessage = "Keine Eingabedaten gefunden."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                If intID > -1 Then
                    m_objlogApp.WriteStandardDataAccessSAP(intID)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub SetModell(ByVal strAppID As String, ByVal strSessionID As String, ByVal dtModell As DataTable)
        m_strClassAndMethod = "Change_04.SetModell"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_STRAUB2.SAPProxy_STRAUB2()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim intID As Int32 = -1
            Try

                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Qmel_Model_Id_Update", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                Dim tblrow As DataRow

                For Each tblrow In dtModell.Rows
                    Try
                        If tblrow.Item("ZZMODELL") <> String.Empty Then
                            'TODO
                            objSAP.Z_M_Qmel_Model_Id_Update(Right("0000000000" & m_objUser.KUNNR, 10), tblrow.Item("Modellcode"), Right("000000000000" & tblrow.Item("QMNUM"), 12), tblrow.Item("ZZMODELL"))
                            m_strMessage = "OK"
                        Else
                            m_strMessage = ""
                        End If

                    Catch ex As Exception
                        m_intStatus = -5555
                        Select Case ex.Message
                            Case "NO_UPDATE"
                                m_strMessage = "Fehler bei Update!"
                            Case "NO_EXIT_MODELID"
                                m_strMessage = "Model-ID existiert zum Kunden nicht!"
                            Case Else
                                m_strMessage = "OK"
                        End Select
                    End Try

                    tblrow.BeginEdit()
                    tblrow.Item("Status") = m_strMessage
                    tblrow.EndEdit()
                Next

                objSAP.CommitWork()

                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If


                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -5555
                Select Case ex.Message
                    Case "NO_FLEET"
                        m_strMessage = "Keine Fleet Daten vorhanden."
                    Case "NO_WEB"
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case "NO_DATA"
                        m_strMessage = "Keine Eingabedaten gefunden."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                If intID > -1 Then
                    m_objlogApp.WriteStandardDataAccessSAP(intID)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

                m_tblModell = dtModell


                m_blnGestartet = False
            End Try
        End If
    End Sub









#End Region

End Class

' ************************************************
' $History: Change_04.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:38
' Created in $/CKAG/Applications/AppSTRAUB/Lib
' 
' *****************  Version 7  *****************
' User: Uha          Date: 3.07.07    Time: 9:51
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 6  *****************
' User: Uha          Date: 8.03.07    Time: 14:18
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Lib
' 
' ************************************************
