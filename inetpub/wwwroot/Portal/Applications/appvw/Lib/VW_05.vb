Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts

Public Class VW_05
    REM § Status-Report, Kunde: VW, BAPI: Z_M_Tab_Imp_Zul_002,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_strVorhaben As String
    Private m_strFahrgestellnummer As String
    Private m_strHaendlernummer As String
#End Region

#Region " Properties"
    Public Property Vorhaben() As String
        Get
            Return m_strVorhaben
        End Get
        Set(ByVal Value As String)
            m_strVorhaben = Value
        End Set
    End Property

    Public Property Fahrgestellnummer() As String
        Get
            Return m_strFahrgestellnummer
        End Get
        Set(ByVal Value As String)
            m_strFahrgestellnummer = Value
        End Set
    End Property

    Public Property Haendlernummer() As String
        Get
            Return m_strHaendlernummer
        End Get
        Set(ByVal Value As String)
            m_strHaendlernummer = Value
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
        m_strClassAndMethod = "VW_05.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            'Dim objSAP As New SAPProxy_VW.SAPProxy_VW()
            'objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            'objSAP.Connection.Open()

            Dim intID As Int32 = -1

            Try
                'Dim SAPTable As New SAPProxy_VW.ZDAD_WEB_TAB_IMP_ZUL_002Table()

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Tab_Imp_Zul_002", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

                Dim tblTemp2 As DataTable = S.AP.GetExportTableWithInitExecute("Z_M_Tab_Imp_Zul_002.GT_WEB",
                                                                                  "I_KUNNR, I_REFERENZ2, I_CHASSIS_NUM, I_HAENDLER",
                                                                                  m_objUser.KUNNR.ToSapKunnr, m_strVorhaben, m_strFahrgestellnummer, m_strHaendlernummer)

                'objSAP.Z_M_Tab_Imp_Zul_002(m_strFahrgestellnummer, m_strHaendlernummer, Right("0000000000" & m_objUser.KUNNR, 10), m_strVorhaben, SAPTable)
                'objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                'Dim tblTemp2 As DataTable = SAPTable.ToADODataTable

                CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", Vorhaben=" & m_strVorhaben, m_tblResult, False)
            Catch ex As Exception

                Select Case ex.Message
                    Case "NO_DATA"
                        m_intStatus = -1111
                        m_strMessage = "Keine Fahrzeuge gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select

                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", Vorhaben=" & m_strVorhaben & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                If intID > -1 Then
                    m_objlogApp.WriteStandardDataAccessSAP(intID)
                End If

                'objSAP.Connection.Close()
                'objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: VW_05.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:14
' Updated in $/CKAG/Applications/appvw/Lib
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:49
' Created in $/CKAG/Applications/appvw/Lib
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 6.11.07    Time: 14:32
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 6.11.07    Time: 12:58
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 12.09.07   Time: 17:03
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' 
' *****************  Version 1  *****************
' User: Uha          Date: 13.08.07   Time: 16:11
' Created in $/CKG/Applications/AppVW/AppVWWeb/Lib
' ITA 1125 "Werkstattzuordnungsliste" hinzugefügt
' 
' ************************************************
