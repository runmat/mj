Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Public Class Straub_02

    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_strPdi As String
    Private m_datErfassungsdatumVon As DateTime
    Private m_datErfassungsdatumBis As DateTime
    Private m_strHersteller As String
    Private m_strLieferant As String
    Private m_tblHistory As DataTable
#End Region

#Region " Properties"
    Public ReadOnly Property History() As DataTable
        Get
            Return m_tblHistory
        End Get
    End Property

    Public Property PPdi() As String
        Get
            Return m_strPdi
        End Get
        Set(ByVal Value As String)
            m_strPdi = Value
        End Set
    End Property

    Public Property PDatumVon() As DateTime
        Get
            Return m_datErfassungsdatumVon
        End Get
        Set(ByVal Value As DateTime)
            m_datErfassungsdatumVon = Value
        End Set
    End Property

    Public Property PDatumBis() As DateTime
        Get
            Return m_datErfassungsdatumBis
        End Get
        Set(ByVal Value As DateTime)
            m_datErfassungsdatumBis = Value
        End Set
    End Property

    Public Property PHersteller() As String
        Get
            Return m_strHersteller
        End Get
        Set(ByVal Value As String)
            m_strHersteller = Value
        End Set
    End Property

    Public Property PLieferant() As String
        Get
            Return m_strLieferant
        End Get
        Set(ByVal Value As String)
            m_strLieferant = Value
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
        m_strClassAndMethod = "Straub_02.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_STRAUB.SAPProxy_STRAUB()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim intID As Int32 = -1

            Try
                Dim SAPTable As New SAPProxy_STRAUB.ZDAD_FZG_OHNE_BRIEF_001Table()
                Dim SAPTableHerst As New SAPProxy_STRAUB.ZDAD_M_HERSTELLERTable()

                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Fzg_Ohne_Brief_001", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Fzg_Ohne_Brief_001(Right("0000000000" & m_objUser.KUNNR, 10),
                                              m_strPdi, m_strHersteller,
                                              HelpProcedures.MakeDateSAP(m_datErfassungsdatumBis),
                                              HelpProcedures.MakeDateSAP(m_datErfassungsdatumVon),
                                              "",
                                              m_strLieferant,
                                              SAPTable,
                                              SAPTableHerst)

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
                    Case "NO_FLEET"
                        m_strMessage = "Fehler! Keine Fleet Daten vorhanden."
                    Case "NO_WEB"
                        m_strMessage = "Fehler! Keine Web-Tabelle erstellt."
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten gefunden."
                    Case Else
                        m_strMessage = "Unbekannter Fehler."
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
#End Region
End Class

' ************************************************
' $History: Straub_02.vb $
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
