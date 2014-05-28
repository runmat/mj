Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Straub_05

    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_datErfassungsdatumVon As DateTime
    Private m_datErfassungsdatumBis As DateTime
    Private m_pdi As String
    Private m_Hersteller As String
    Private m_Herstellercode As String
    Private tblHersteller As DataTable
#End Region

#Region " Properties"
    ReadOnly Property PHerstellerliste() As DataTable
        Get
            Return tblHersteller
        End Get
    End Property

    Property PHersteller() As String
        Get
            Return m_Hersteller
        End Get
        Set(ByVal Value As String)
            m_Hersteller = Value
        End Set
    End Property

    Property PHerstellerCode() As String
        Get
            Return m_Herstellercode
        End Get
        Set(ByVal Value As String)
            m_Herstellercode = Value
        End Set
    End Property

    Property PDatumVon() As DateTime
        Get
            Return m_datErfassungsdatumVon
        End Get
        Set(ByVal Value As DateTime)
            m_datErfassungsdatumVon = Value
        End Set
    End Property

    Property PDatumBis() As DateTime
        Get
            Return m_datErfassungsdatumBis
        End Get
        Set(ByVal Value As DateTime)
            m_datErfassungsdatumBis = Value
        End Set
    End Property

    Property PPDI() As String
        Get
            Return m_pdi
        End Get
        Set(ByVal Value As String)
            m_pdi = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
        FILL(m_strAppID, m_strSessionID)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "Straub_05.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_STRAUB2.SAPProxy_STRAUB2()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim intID As Int32 = -1

            Try
                Dim SAPTable As New SAPProxy_STRAUB2.ZDAD_M_ST_AVM_ZULASSUNGEN_002Table()
                Dim tblSAP As New SAPProxy_STRAUB2.ZDAD_M_HERSTELLERTable()
                Dim tblRowSAP As SAPProxy_STRAUB2.ZDAD_M_HERSTELLER

                Dim strDatErfVon As String
                If IsDate(m_datErfassungsdatumVon) Then
                    strDatErfVon = HelpProcedures.MakeDateSAP(m_datErfassungsdatumVon)
                    If strDatErfVon = "10101" Then
                        strDatErfVon = "|"
                    End If
                Else
                    strDatErfVon = "|"
                End If
                Dim strDatErfBis As String
                If IsDate(m_datErfassungsdatumBis) Then
                    strDatErfBis = HelpProcedures.MakeDateSAP(m_datErfassungsdatumBis)
                    If strDatErfBis = "10101" Then
                        strDatErfBis = "|"
                    End If
                Else
                    strDatErfBis = "|"
                End If

                'Hersteller füllen
                If (m_Hersteller <> String.Empty) Then
                    tblRowSAP = New SAPProxy_STRAUB2.ZDAD_M_HERSTELLER()
                    tblRowSAP.Zherst = m_Hersteller
                    tblRowSAP.Domvalue_L = m_Herstellercode

                    tblSAP.Clear()
                    tblSAP.Add(tblRowSAP)
                End If
                '--------------------

                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Zulassungen_002", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Zulassungen_002(Right("0000000000" & m_objUser.KUNNR, 10), m_pdi, strDatErfBis, strDatErfVon, SAPTable, tblSAP)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp2 As DataTable = SAPTable.ToADODataTable
                CreateOutPut(tblTemp2, strAppID)

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", ERDAT_LOW=" & m_datErfassungsdatumVon.ToShortDateString & ", ERDAT_HIGH=" & m_datErfassungsdatumBis.ToShortDateString, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -5555
                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten gefunden."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten."
                End Select
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ERDAT_LOW=" & m_datErfassungsdatumVon.ToShortDateString & ", ERDAT_HIGH=" & m_datErfassungsdatumBis.ToShortDateString & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
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

    Public Sub getData(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "ec_01.getData"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_Base.SAPProxy_Base()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim intID As Int32 = -1

            Try
                Dim SAPTableReturnHersteller As New SAPProxy_Base.ZDAD_M_HERSTELLERTable()

                'SAP-Logeintrag (initialisieren)
                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Domaene_Herst", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                'BAPI-Aufruf

                objSAP.Z_M_Domaene_Herst(SAPTableReturnHersteller)

                objSAP.CommitWork()
                'SAP-Logeintrag (füllen)
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                'Tabellen formatieren
                tblHersteller = SAPTableReturnHersteller.ToADODataTable

                'Report-Logeintrag (ok)
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_DATA"
                        m_intStatus = -1234
                        m_strMessage = "Fehler: Keine Daten gefunden!"
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten."
                End Select
                tblHersteller = Nothing
                'tblVerwendung = Nothing
                'Report-Logeintrag (Fehler)
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
' $History: Straub_05.vb $
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
