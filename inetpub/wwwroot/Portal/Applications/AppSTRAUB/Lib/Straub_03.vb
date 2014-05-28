Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Public Class Straub_03

    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_datErfassungsdatumVon As DateTime
    Private m_datErfassungsdatumBis As DateTime
    Private m_pdi As String
    Private m_strHersteller As String
    Private m_strLieferant As String
#End Region

#Region " Properties"
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

    Property PHersteller() As String
        Get
            Return m_strHersteller
        End Get
        Set(ByVal Value As String)
            m_strHersteller = Value
        End Set
    End Property

    Property PLieferant() As String
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
        m_strClassAndMethod = "Straub_03.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Dim colorTextArray() As String = {"weiss", "gelb", "orange", "rot", "violett", "blau", "grün", "grau", "braun", "schwarz"}

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_STRAUB2.SAPProxy_STRAUB2()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim intID As Int32 = -1

            Try
                Dim SAPTable As New SAPProxy_STRAUB2.ZDAD_M_ST_AVM_PDI_BASTAND_LTXTTable()
                Dim SAPTableBestand As New SAPProxy_STRAUB2.ZDAD_M_ST_AVM_PDI_BASTANDTable()
                Dim SAPTableHersteller As New SAPProxy_STRAUB2.ZDAD_M_HERSTELLERTable()

                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Pdi_Bestand_001", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Pdi_Bestand_001(Right("0000000000" & m_objUser.KUNNR, 10),
                                           m_pdi,
                                           HelpProcedures.MakeDateSAP(m_datErfassungsdatumBis),
                                           HelpProcedures.MakeDateSAP(m_datErfassungsdatumVon),
                                           m_strLieferant,
                                           SAPTableBestand,
                                           SAPTable,
                                           SAPTableHersteller)

                objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp2 As DataTable = SAPTableBestand.ToADODataTable
                CreateOutPut(tblTemp2, strAppID)

                Dim row As DataRow

                For Each row In m_tblResult.Rows
                    If (Not TypeOf row("Farbe") Is System.DBNull) AndAlso (CStr(row("Farbe")) <> String.Empty) Then
                        row("Farbe") = colorTextArray(CInt(row("Farbe")))
                    End If
                Next
                m_tblResult.AcceptChanges()

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
#End Region
End Class

' ************************************************
' $History: Straub_03.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:38
' Created in $/CKAG/Applications/AppSTRAUB/Lib
' 
' *****************  Version 9  *****************
' User: Uha          Date: 3.07.07    Time: 9:51
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 8  *****************
' User: Uha          Date: 8.03.07    Time: 14:18
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Lib
' 
' ************************************************
