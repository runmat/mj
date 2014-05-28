Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Public Class Straub_01

    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_datErfassungsdatumVon As DateTime
    Private m_datErfassungsdatumBis As DateTime
    Private m_tblHistory As DataTable
#End Region

#Region " Properties"
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
        FILL(m_strAppID, m_strSessionID, m_datErfassungsdatumVon, m_datErfassungsdatumBis)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal datErfassungsdatumVon As DateTime, ByVal datErfassungsdatumBis As DateTime)
        m_strClassAndMethod = "Straub_01.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_STRAUB2.SAPProxy_STRAUB2()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim intID As Int32 = -1

            Try
                Dim SAPTable As New SAPProxy_STRAUB2.ZDAD_M_WEB_REPORTING_SIXTTable()
                Dim strDatErfVon As String
                If IsDate(datErfassungsdatumVon) Then
                    strDatErfVon = HelpProcedures.MakeDateSAP(datErfassungsdatumVon)
                    If strDatErfVon = "10101" Then
                        strDatErfVon = "|"
                    End If
                Else
                    strDatErfVon = "|"
                End If
                Dim strDatErfBis As String
                If IsDate(datErfassungsdatumBis) Then
                    strDatErfBis = HelpProcedures.MakeDateSAP(datErfassungsdatumBis)
                    If strDatErfBis = "10101" Then
                        strDatErfBis = "|"
                    End If
                Else
                    strDatErfBis = "|"
                End If

                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Briefeohnefzg2", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Briefeohnefzg2("|", strDatErfBis, strDatErfVon, "|", "|", Right("0000000000" & m_objUser.KUNNR, 10), SAPTable)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp2 As DataTable = SAPTable.ToADODataTable
                Dim row As DataRow

                'Detaildatensätze
                tblTemp2.Columns.Add("Typdaten", System.Type.GetType("System.String"))
                '§§§ JVE 24.07.2006...........
                For Each row In tblTemp2.Rows
                    'row.BeginEdit()
                    If TypeOf row("EQUNR") Is System.DBNull = False Then
                        row("Typdaten") = "<A href=Change06_3NEU.aspx?EqNr=" & CStr(row("EQUNR")) & " target=_blank>Anzeige</A>"
                    End If
                    'row.EndEdit()
                Next
                '§§§ JVE 24.07.2006
                tblTemp2.AcceptChanges()

                CreateOutPut(tblTemp2, strAppID)

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", ERDAT_LOW=" & datErfassungsdatumVon.ToShortDateString & ", ERDAT_HIGH=" & datErfassungsdatumBis.ToShortDateString, m_tblResult, False)
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
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ERDAT_LOW=" & datErfassungsdatumVon.ToShortDateString & ", ERDAT_HIGH=" & datErfassungsdatumBis.ToShortDateString & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
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
#End Region
End Class

' ************************************************
' $History: Straub_01.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:38
' Created in $/CKAG/Applications/AppSTRAUB/Lib
' 
' *****************  Version 8  *****************
' User: Uha          Date: 3.07.07    Time: 9:51
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 7  *****************
' User: Uha          Date: 8.03.07    Time: 14:18
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Lib
' 
' ************************************************
