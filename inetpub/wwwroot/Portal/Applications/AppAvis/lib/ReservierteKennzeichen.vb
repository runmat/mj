Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts


'#################################################################
' Klasse für das Lesen der Avis-Grundaten#
' BAPI: Z_AVM_KENNZ_RES_001
'           
'#################################################################
Public Class ReservierteKennzeichen
    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private mDatEingangBis As String = ""
    Private mDatEingangVon As String = ""
    Private mPDI As String = ""
    Private mFahrgestellnummer As String = ""
    Private mFahrzeugmodell As String = ""

#End Region

#Region " Properties"

    Public Property EingangBis() As String
        Get
            Return mDatEingangBis
        End Get
        Set(ByVal value As String)
            mDatEingangBis = value
        End Set
    End Property


    Public Property EingangVon() As String
        Get
            Return mDatEingangVon
        End Get
        Set(ByVal value As String)
            mDatEingangVon = value
        End Set
    End Property


    Public Property PDI() As String
        Get
            Return mPDI
        End Get
        Set(ByVal value As String)
            mPDI = value
        End Set
    End Property



    Public Property Fahrgestellnummer() As String
        Get
            Return mFahrgestellnummer
        End Get
        Set(ByVal value As String)
            mFahrgestellnummer = value
        End Set
    End Property


    Public Property Fahrzeugmodell() As String
        Get
            Return mFahrzeugmodell
        End Get
        Set(ByVal value As String)
            mFahrzeugmodell = value
        End Set
    End Property


#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    '----------------------------------------------------------------------
    ' Methode: Fill
    ' Autor: J.Jung
    ' Beschreibung: füllen der Ausgabetabelle für reservierte Kennzeichen (Report05)
    ' Erstellt am: 24.11.2008
    ' ITA: 2421
    '----------------------------------------------------------------------

    Public Overloads Sub Fill(ByVal strAppID As String, ByVal strSessionID As String)

        m_strClassAndMethod = "ReservierteKennzeichen.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try

            Dim dtTemp As DataTable = S.AP.GetExportTableWithInitExecute("Z_AVM_KENNZ_RES_001.GT_WEB",
                        "I_KUNNR, I_ZZDAT_EIN_VON, I_ZZDAT_EIN_BIS, I_PDI, I_ZZFAHRG, I_ZMODELL",
                        m_objUser.KUNNR.ToSapKunnr(), mDatEingangVon, mDatEingangBis, mPDI, mFahrgestellnummer, mFahrzeugmodell)

            CreateOutPut(dtTemp, strAppID)
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", I_EING_DAT_VON=" & mDatEingangVon & ", I_EING_DAT_BIS=" & mDatEingangBis, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", I_EING_DAT_VON=" & mDatEingangVon & ", I_EING_DAT_BIS=" & mDatEingangBis & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        Finally
            m_blnGestartet = False
        End Try
    End Sub
#End Region
End Class

' ************************************************
' $History: ReservierteKennzeichen.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 24.11.08   Time: 13:15
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA 2421 nachbesserungen
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 24.11.08   Time: 9:35
' Created in $/CKAG/Applications/AppAvis/lib
' ITA 2421 testfertig
'
' ************************************************