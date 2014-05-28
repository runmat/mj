Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class ANC_B04
    REM § Status-Report, Kunde: ANC, BAPI: Z_M_Briefeing_Fz_ANC_Fibu,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_datZulassungsdatumVon As DateTime
    Private m_datZulassungsdatumBis As DateTime
#End Region

#Region " Properties"
    Public Property ZulassungsdatumVon() As DateTime
        Get
            Return m_datZulassungsdatumVon
        End Get
        Set(ByVal Value As DateTime)
            m_datZulassungsdatumVon = Value
        End Set
    End Property

    Public Property ZulassungsdatumBis() As DateTime
        Get
            Return m_datZulassungsdatumBis
        End Get
        Set(ByVal Value As DateTime)
            m_datZulassungsdatumBis = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
        FILL(m_strAppID, m_strSessionID, m_datZulassungsdatumVon, m_datZulassungsdatumBis)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal datZulassungsdatumVon As DateTime, ByVal datZulassungsdatumBis As DateTime)
        m_strClassAndMethod = "ANC_B04.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_ANC.SAPProxy_ANC()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim intID As Int32 = -1
            Dim strDatZulVon As String = ""
            Dim strDatZulBis As String = ""

            Try
                Dim SAPTable As New SAPProxy_ANC.ZDAD_S_ZULASSUNG_VANGUARDTable()
                If IsDate(datZulassungsdatumVon) Then
                    strDatZulVon = MakeDateSAP(datZulassungsdatumVon)
                    If strDatZulVon = "10101" Then
                        strDatZulVon = "|"
                    End If
                Else
                    strDatZulVon = "|"
                End If
                If IsDate(datZulassungsdatumBis) Then
                    strDatZulBis = MakeDateSAP(datZulassungsdatumBis)
                    If strDatZulBis = "10101" Then
                        strDatZulBis = "|"
                    End If
                Else
                    strDatZulBis = "|"
                End If

                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Zulassung_Vanguard", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Zulassung_Vanguard(Right("0000000000" & m_objUser.Customer.KUNNR, 10), strDatZulBis, strDatZulVon, SAPTable)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp2 As DataTable = SAPTable.ToADODataTable

                'Ersetze Bindestrich durch Leerzeichen in Kfz-Kennzeichen
                Dim tmpRow As DataRow
                For Each tmpRow In tblTemp2.Rows
                    If Not TypeOf tmpRow("LICENSE_NUM") Is System.DBNull Then
                        tmpRow("LICENSE_NUM") = Replace(CStr(tmpRow("LICENSE_NUM")), "-", "")
                    End If
                Next

                CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "REPLADATE_LOW=" & strDatZulVon & ", REPLADATE_HIGH=" & strDatZulBis, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -5555
                Select Case ex.Message
                    Case "NO_KUNNR"
                        m_strMessage = "Es wurden keine Kundennummern übergeben."
                    Case "NO_DATA"
                        m_strMessage = "Es konnten keine Daten selektiert werden."
                    Case "NO_PARAMETER"
                        m_strMessage = "Es muss ein Parameter gepflegt sein."
                    Case "NO_ZZFAHRG"
                        m_strMessage = "Die Fahrgestellnummer konnte nicht gefunden werden."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "REPLADATE_LOW=" & strDatZulVon & ", REPLADATE_HIGH=" & strDatZulBis & " , " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
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
' $History: ANC_B04.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 29.04.09   Time: 15:28
' Updated in $/CKAG/Applications/appanc/Lib
' Warnungen entfernt.
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:41
' Created in $/CKAG/Applications/appanc/Lib
' 
' *****************  Version 7  *****************
' User: Uha          Date: 20.08.07   Time: 15:17
' Updated in $/CKG/Applications/AppANC/AppANCWeb/Lib
' ITA 1250: In Kennzeichen werden die Bindestriche nicht durch
' Leerzeichen ersetzt, sondern ersatzlos gelöscht.
' 
' *****************  Version 6  *****************
' User: Uha          Date: 20.08.07   Time: 13:30
' Updated in $/CKG/Applications/AppANC/AppANCWeb/Lib
' ITA 1250 Testversion
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 15:47
' Updated in $/CKG/Applications/AppANC/AppANCWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 6.03.07    Time: 13:45
' Updated in $/CKG/Applications/AppANC/AppANCWeb/Lib
' 
' ************************************************
