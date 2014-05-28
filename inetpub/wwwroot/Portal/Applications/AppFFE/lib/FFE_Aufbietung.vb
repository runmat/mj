Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel


Public Class FFE_Aufbietung

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_datAbDatum As String
    Private m_datBisDatum As String
    Private m_strABCKZ As String = ""
#End Region

#Region " Properties"

    Public Property datVON() As String
        Get
            Return m_datAbDatum
        End Get
        Set(ByVal Value As String)
            m_datAbDatum = Value
        End Set
    End Property

    Public Property datBIS() As String
        Get
            Return m_datBisDatum
        End Get
        Set(ByVal Value As String)
            m_datBisDatum = Value
        End Set
    End Property

    Public Property abckz() As String
        Get
            Return m_strABCKZ
        End Get
        Set(ByVal Value As String)
            m_strABCKZ = Value
        End Set
    End Property

#End Region

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
        Fill(m_strAppID, m_strsessionid)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "FFE_Aufbietung.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            Dim intID As Int32 = -1

            Try
                Dim SAPTable As New SAPProxy_FFE.ZDAD_AUFBIETUNG_FCETable
                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Zz_Csc_Daten_Einaus_Report_001", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                Dim tmpDateab As Date
                Dim tmpDatebis As Date
                If IsDate(m_datAbDatum) Then
                    tmpDateab = CDate(m_datAbDatum)
                    m_datAbDatum = MakeDateSAP(tmpDateab)
                Else
                    m_datAbDatum = Nothing
                End If
                If IsDate(m_datBisDatum) Then
                    tmpDatebis = CDate(m_datBisDatum)
                    m_datBisDatum = MakeDateSAP(tmpDatebis)
                Else
                    m_datBisDatum = Nothing
                End If
                'Right("0000000000" & m_objUser.KUNNR, 10),
                objSAP.Z_M_Aufbietungen_Fce(m_strABCKZ, Right("0000000000" & m_objUser.KUNNR, 10), m_datAbDatum, m_datBisDatum, Nothing, "1510", "", SAPTable)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If
                CreateOutPut(SAPTable.ToADODataTable, strAppID)

                WriteLogEntry(True, " DATANF=" & m_datAbDatum & ", DATEND=" & m_datBisDatum & ", KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                Select Case ex.Message

                    Case "NOT_FOUND"
                        m_intStatus = -1111
                        m_strMessage = "Keine Daten gefunden!"
                    Case "NO_HAENDLER"
                        m_intStatus = -1111
                        m_strMessage = "Keine Daten gefunden!"

                End Select


                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "DATANF=" & m_datAbDatum & ", DATEND=" & m_datBisDatum & ", KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
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
End Class
' ************************************************
' $History: FFE_Aufbietung.vb $
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 7.07.08    Time: 10:40
' Updated in $/CKAG/Applications/AppFFE/lib
' SSV Historien hinzugefügt
' 
' ************************************************
