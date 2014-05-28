Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class ALD_02
    REM § Status-Report, Kunde: ALD, BAPI: Z_M_VERSBRIEFE,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_datAbmeldedatumVon As DateTime
    Private m_datAbmeldedatumBis As DateTime
    Private m_strABC_KZ As String
#End Region

#Region " Properties"
    Public Property AbmeldedatumVon() As DateTime
        Get
            Return m_datAbmeldedatumVon
        End Get
        Set(ByVal Value As DateTime)
            m_datAbmeldedatumVon = Value
        End Set
    End Property

    Public Property AbmeldedatumBis() As DateTime
        Get
            Return m_datAbmeldedatumBis
        End Get
        Set(ByVal Value As DateTime)
            m_datAbmeldedatumBis = Value
        End Set
    End Property

    Public Property ABC_KZ() As String
        Get
            Return m_strABC_KZ
        End Get
        Set(ByVal Value As String)
            m_strABC_KZ = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    'Public Overloads Overrides Sub Fill()
    '    Fill(m_strAppID, m_strSessionID)
    'End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "ALD_02.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_ALD.SAPProxy_ALD()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim intID As Int32 = -1

            Try
                Dim SAPTable As New SAPProxy_ALD.ZDAD_ALD_VERSBRIEFETable()
                Dim strDatTempVon As String = MakeDateSAP(m_datAbmeldedatumVon)
                Dim strDatTempBis As String = MakeDateSAP(m_datAbmeldedatumBis)


                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Versbriefe", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Versbriefe(Right("0000000000" & m_objUser.KUNNR, 10), strDatTempBis, strDatTempVon, m_strABC_KZ, SAPTable)
                'objSAP.Z_M_Versbriefe(Right("0000000000" & m_objUser.KUNNR, 10), SAPTable)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp2 As DataTable = SAPTable.ToADODataTable
                tblTemp2.Columns.Add("ADRESSE")
                If tblTemp2.Rows.Count > 0 Then
                    Dim row As DataRow
                    For Each row In tblTemp2.Rows
                        Dim strTemp As String = CStr(row("NAME1")) & ", " & CStr(row("NAME2")) & ", " & CStr(row("STRAS")) & " " & CStr(row("HNR")) & ", " & CStr(row("PLZ")) & " " & CStr(row("ORT"))
                        row("ADRESSE") = Replace(strTemp, ", ,  ,  ", "")
                        If CStr(row("VERSART")) = "1" Then
                            row("VERSART") = "Temporär"
                        Else
                            row("VERSART") = "Endgültig"
                        End If
                    Next
                End If

                CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception

                Select Case ex.Message
                    Case "ERR_NO_DATA"
                        m_intStatus = -1111
                        m_strMessage = "Keine Fahrzeuge im vorgegebenen Zeitraum gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select

                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
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
' $History: ALD_02.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 29.04.09   Time: 14:58
' Updated in $/CKAG/Applications/appALD/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 17.04.08   Time: 15:15
' Created in $/CKAG/Applications/appald/Lib
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 15:39
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 5.03.07    Time: 16:59
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Lib
' 
' ************************************************
