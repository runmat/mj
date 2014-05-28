Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class FFE_Bank_Historie
    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_tblBRIEFLEBENSLAUF_LPTable As DataTable
    Private m_tblQMEL_DATENTable As DataTable
    Private m_tblQMMIDATENTable As DataTable
    Private mGT_EQUI As DataTable

    Private mFahrgestellnummer As String
    Private mKennzeichen As String
    Private mBriefStatus As String
    Private mLagerort As String
    Private mHersteller As String
    Private mFahrzeugmodell As String
    Private mFarbe As String
    Private mHerstellerschluessel As String
    Private mTypschluessel As String
    Private mVarianteVersion As String
    Private mZBIINummer As String
    Private mZBIINummerBEZ As String
    Private mErstzulassungsdatum As String
    Private mAbmeldedatum As String
    Private mFinanzierungsnummer As String
    Private mFinanzierungsnummerBEZ As String
    Private mCOC As String
    Private mCOCBEZ As String
    Private mFahrzeughalterName1Name2 As String
    Private mFahrzeughalterStrasseNummer As String
    Private mFahrzeughalterPLZOrt As String
    Private mVersandadresseName1Name2 As String
    Private mVersandadresseStrasseNummer As String
    Private mVersandadressePLZOrt As String
    Private mVersandadresseBEZ As String
    Private mUmgemeldetAm As String
    Private mEhmaligesKennzeichen As String
    Private mZBIIAufbietung As String
    Private mEhmaligeZBIINummer As String
    Private mUsername As String
    Private mAusdruckDatumUhrzeit As String
    
#End Region


#Region " Properties"

    Public ReadOnly Property QMMIDATENTable() As DataTable
        Get
            Return m_tblQMMIDATENTable
        End Get
    End Property

    Public ReadOnly Property QMEL_DATENTable() As DataTable
        Get
            Return m_tblQMEL_DATENTable
        End Get
    End Property

    Public ReadOnly Property BRIEFLEBENSLAUF_LPTable() As DataTable
        Get
            Return m_tblBRIEFLEBENSLAUF_LPTable
        End Get
    End Property

    Public Property Fahrgestellnummer() As String
        Get
            Return mFahrgestellnummer
        End Get
        Set(ByVal value As String)
            mFahrgestellnummer = value
        End Set
    End Property

    Public Property Kennzeichen() As String
        Get
            Return mKennzeichen
        End Get
        Set(ByVal value As String)
            mKennzeichen = value
        End Set
    End Property

    Public Property BriefStatus() As String
        Get
            Return mBriefStatus
        End Get
        Set(ByVal value As String)
            mBriefStatus = value
        End Set
    End Property

    Public Property Username() As String
        Get
            Return mUsername
        End Get
        Set(ByVal value As String)
            mUsername = value
        End Set
    End Property

    Public Property AusdruckDatumUhrzeit() As String
        Get
            Return mAusdruckDatumUhrzeit
        End Get
        Set(ByVal value As String)
            mAusdruckDatumUhrzeit = value
        End Set
    End Property

    Public Property Lagerort() As String
        Get
            Return mLagerort
        End Get
        Set(ByVal value As String)
            mLagerort = value
        End Set
    End Property

    Public Property Hersteller() As String
        Get
            Return mHersteller
        End Get
        Set(ByVal value As String)
            mHersteller = value
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

    Public Property Farbe() As String
        Get
            Return mFarbe
        End Get
        Set(ByVal value As String)
            mFarbe = value
        End Set
    End Property

    Public Property Herstellerschluessel() As String
        Get
            Return mHerstellerschluessel
        End Get
        Set(ByVal value As String)
            mHerstellerschluessel = value
        End Set
    End Property

    Public Property Typschluessel() As String
        Get
            Return mTypschluessel
        End Get
        Set(ByVal value As String)
            mTypschluessel = value
        End Set
    End Property

    Public Property VarianteVersion() As String
        Get
            Return mVarianteVersion
        End Get
        Set(ByVal value As String)
            mVarianteVersion = value
        End Set
    End Property

    Public Property ZBIINummerBEZ() As String
        Get
            Return mZBIINummerBEZ
        End Get
        Set(ByVal value As String)
            mZBIINummerBEZ = value
        End Set
    End Property

    Public Property ZBIINummer() As String
        Get
            Return mZBIINummer
        End Get
        Set(ByVal value As String)
            mZBIINummer = value
        End Set
    End Property
    
    Public Property Erstzulassungsdatum() As String
        Get
            Return mErstzulassungsdatum
        End Get
        Set(ByVal value As String)
            mErstzulassungsdatum = value
        End Set
    End Property
    
    Public Property Abmeldedatum() As String
        Get
            Return mAbmeldedatum
        End Get
        Set(ByVal value As String)
            mAbmeldedatum = value
        End Set
    End Property

    Public Property FinanzierungsnummerBEZ() As String
        Get
            Return mFinanzierungsnummerBEZ
        End Get
        Set(ByVal value As String)
            mFinanzierungsnummerBEZ = value
        End Set
    End Property

    Public Property Finanzierungsnummer() As String
        Get
            Return mFinanzierungsnummer
        End Get
        Set(ByVal value As String)
            mFinanzierungsnummer = value
        End Set
    End Property

    Public Property FahrzeughalterName1Name2() As String
        Get
            Return mFahrzeughalterName1Name2
        End Get
        Set(ByVal value As String)
            mFahrzeughalterName1Name2 = value
        End Set
    End Property

    Public Property FahrzeughalterStrasseNummer() As String
        Get
            Return mFahrzeughalterStrasseNummer
        End Get
        Set(ByVal value As String)
            mFahrzeughalterStrasseNummer = value
        End Set
    End Property

    Public Property FahrzeughalterPLZOrt() As String
        Get
            Return mFahrzeughalterPLZOrt
        End Get
        Set(ByVal value As String)
            mFahrzeughalterPLZOrt = value
        End Set
    End Property
    
    Public Property VersandadresseBEZ() As String
        Get
            Return mVersandadresseBEZ
        End Get
        Set(ByVal value As String)
            mVersandadresseBEZ = value
        End Set
    End Property

    Public Property VersandadresseName1Name2() As String
        Get
            Return mVersandadresseName1Name2
        End Get
        Set(ByVal value As String)
            mVersandadresseName1Name2 = value
        End Set
    End Property

    Public Property VersandadresseStrasseNummer() As String
        Get
            Return mVersandadresseStrasseNummer
        End Get
        Set(ByVal value As String)
            mVersandadresseStrasseNummer = value
        End Set
    End Property

    Public Property VersandadressePLZOrt() As String
        Get
            Return mVersandadressePLZOrt
        End Get
        Set(ByVal value As String)
            mVersandadressePLZOrt = value
        End Set
    End Property

    Public Property COCBEZ() As String
        Get
            Return mCOCBEZ
        End Get
        Set(ByVal value As String)
            mCOCBEZ = value
        End Set
    End Property

    Public Property COC() As String
        Get
            Return mCOC
        End Get
        Set(ByVal value As String)
            mCOC = value
        End Set
    End Property

    Public Property UmgemeldetAM() As String
        Get
            Return mUmgemeldetAm
        End Get
        Set(ByVal value As String)
            mUmgemeldetAm = value
        End Set
    End Property

    Public Property EhmaligesKennzeichen() As String
        Get
            Return mEhmaligesKennzeichen
        End Get
        Set(ByVal value As String)
            mEhmaligesKennzeichen = value
        End Set
    End Property

    Public Property ZBIIAufbietung() As String
        Get
            Return mZBIIAufbietung
        End Get
        Set(ByVal value As String)
            mZBIIAufbietung = value
        End Set
    End Property
    
    Public Property EhmaligeZBIINummer() As String
        Get
            Return mEhmaligeZBIINummer
        End Get
        Set(ByVal value As String)
            mEhmaligeZBIINummer = value
        End Set
    End Property

    Public ReadOnly Property diverseFahrzeuge() As DataTable
        Get
            Return mGT_EQUI
        End Get
    End Property

#End Region


#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal strAmtlKennzeichen As String,
                              ByVal strFahrgestellnummer As String, ByVal strBriefnummer As String,
                              ByVal strOrdernummer As String, ByVal equnr As String)

        m_strClassAndMethod = "FFE_BANK_HISTORIE.Fill"

        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intStatus = 0

            Try

                S.AP.Init("Z_M_BRIEFLEBENSLAUF_001")

                S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                S.AP.SetImportParameter("I_ZZKENN", strAmtlKennzeichen)
                S.AP.SetImportParameter("I_ZZFAHRG", strFahrgestellnummer)
                S.AP.SetImportParameter("I_ZZREF1", strOrdernummer)
                S.AP.SetImportParameter("I_ZZBRIEF", strBriefnummer)
                S.AP.SetImportParameter("I_EQUNR", equnr)

                If m_objUser.Reference.Length > 0 Then
                    S.AP.SetImportParameter("I_HAENDLER", Right("0000000000" & m_objUser.Reference, 10))
                    S.AP.SetImportParameter("I_FCE", "X")
                End If

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If

                S.AP.Execute()

                m_tblQMEL_DATENTable = S.AP.GetExportTable("GT_QMEL")
                m_tblQMMIDATENTable = S.AP.GetExportTable("GT_QMMA")
                m_tblBRIEFLEBENSLAUF_LPTable = S.AP.GetExportTable("GT_WEB")
                mGT_EQUI = S.AP.GetExportTable("GT_EQUI")

                For Each row As DataRow In m_tblQMMIDATENTable.Rows
                    If Not TypeOf row("AEZEIT") Is System.DBNull Then
                        row("AEZEIT") = row("AEZEIT").ToString.TrimStart("0"c)
                        If row("AEZEIT").ToString <> String.Empty Then
                            row("AEZEIT") = Left(row("AEZEIT").ToString, 2) & ":" & row("AEZEIT").ToString.Substring(2, 2)
                        End If
                    End If
                Next

                'bearbeitung der EQUI Tabelle für die Ausgabe JJU20081127
                If Not mGT_EQUI Is Nothing Then
                    For Each tmprow As DataRow In mGT_EQUI.Rows
                        Select Case tmprow("ABCKZ").ToString
                            Case "1"
                                tmprow("ABCKZ") = "temporär versendet"
                            Case "2"
                                tmprow("ABCKZ") = "endgültig versendet"
                            Case Else
                                tmprow("ABCKZ") = "eingelagert"
                        End Select
                    Next

                    mGT_EQUI.AcceptChanges()
                    CreateOutPut(mGT_EQUI, strAppID)
                    mGT_EQUI = m_tblResult
                End If

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", ZZBRIEF=" & UCase(strBriefnummer) & ", ZFAHRG=" & UCase(strFahrgestellnummer) & ", ZZREF1=" & UCase(strOrdernummer) & ", ZZKENN=" & UCase(strAmtlKennzeichen), m_tblBRIEFLEBENSLAUF_LPTable, False)
            Catch ex As Exception
                m_intStatus = -2222
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_strMessage = "Keine Ergebnisse für die angegebenen Kriterien."
                    Case "NO_WEB"
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case "HAENDLER_NOT_FOUND"
                        m_strMessage = "Händler konnte nicht gefunden werden"
                    Case Else
                        m_strMessage = ex.Message
                End Select

                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ZZBRIEF=" & UCase(strBriefnummer) & ", ZFAHRG=" & UCase(strFahrgestellnummer) & ", ZZREF1=" & UCase(strOrdernummer) & ", ZZKENN=" & UCase(strAmtlKennzeichen) & ", " & Replace(m_strMessage, "<br>", " "), m_tblBRIEFLEBENSLAUF_LPTable, False)
            Finally
                m_blnGestartet = False
            End Try
        End If

    End Sub

#End Region

End Class
' ************************************************
' $History: FFE_Bank_Historie.vb $
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 16.02.09   Time: 16:50
' Updated in $/CKAG/Applications/AppFFE/lib
' ITA 2413
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 9.07.08    Time: 8:43
' Updated in $/CKAG/Applications/AppFFE/lib
' ITA 2035
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 7.07.08    Time: 10:40
' Updated in $/CKAG/Applications/AppFFE/lib
' SSV Historien hinzugefügt
' 
' ************************************************
