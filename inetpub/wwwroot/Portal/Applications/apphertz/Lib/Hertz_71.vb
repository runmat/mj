Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class Hertz_71
    REM § Status-Report, Kunde: Hertz, BAPI: Z_V_ZZULLISTE,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_datStillegungVon As DateTime
    Private m_datStillegungBis As DateTime
#End Region

#Region " Properties"
    Public Property StillegungVon() As DateTime
        Get
            Return m_datStillegungVon
        End Get
        Set(ByVal Value As DateTime)
            m_datStillegungVon = Value
        End Set
    End Property
    Public Property StillegungBis() As DateTime
        Get
            Return m_datStillegungBis
        End Get
        Set(ByVal Value As DateTime)
            m_datStillegungBis = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
        Fill(m_strAppID, m_strSessionID, m_datStillegungVon, m_datStillegungBis)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal datStillegungVon As DateTime, ByVal datStillegungBis As DateTime)
        m_strClassAndMethod = "Hertz_71.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_HERTZ.SAPProxy_HERTZ()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim intID As Int32 = -1
            Dim strDatStillVon As String = Nothing
            Dim strDatStillBis As String = Nothing

            Try
                Dim SAPTable As New SAPProxy_HERTZ.ZDADVZULLISTETable() ' ZDADVABMELDUNGTable() ' .ZDAD_S_BESTANDSLISTETable()
                If IsDate(datStillegungVon) Then
                    strDatStillVon = MakeDateSAP(datStillegungVon)
                    If strDatStillVon = "10101" Then
                        strDatStillVon = "|"
                    End If
                Else
                    strDatStillVon = "|"
                End If

                If IsDate(datStillegungBis) Then
                    strDatStillBis = MakeDateSAP(datStillegungBis)
                    If strDatStillBis = "10101" Then
                        strDatStillBis = "|"
                    End If
                Else
                    strDatStillBis = "|"
                End If

                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_V_Zzulliste", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_V_Zzulliste("DEZ", strDatStillVon, strDatStillBis, Right("0000000000" & m_objUser.Customer.KUNNR, 10), SAPTable)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp2 As DataTable = SAPTable.ToADODataTable

                CreateOutPut(tblTemp2, strAppID)

                WriteLogEntry(True, "EXPIRY_DATE_LOW=" & strDatStillVon & ", EXPIRY_DATE_HIGH=" & strDatStillBis, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -5555
                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Es wurden keine Daten gefunden."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "EXPIRY_DATE_LOW=" & strDatStillVon & ", EXPIRY_DATE_HIGH=" & strDatStillBis & " , " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
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
' $History: Hertz_71.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 16:17
' Updated in $/CKAG/Applications/apphertz/Lib
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 14:01
' Created in $/CKAG/Applications/apphertz/Lib
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 17:55
' Updated in $/CKG/Applications/AppHERTZ/AppHERTZWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 5  *****************
' User: Uha          Date: 22.05.07   Time: 13:07
' Updated in $/CKG/Applications/AppHERTZ/AppHERTZWeb/Lib
' Nacharbeiten + Bereinigungen
' 
' ************************************************
