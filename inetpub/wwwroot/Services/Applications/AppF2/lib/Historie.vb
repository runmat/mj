Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class Historie
    Inherits CKG.Base.Business.DatenimportBase

#Region " Declarations"

    Private m_tblBRIEFLEBENSLAUF_LPTable As DataTable
    Private m_tblQMEL_DATENTable As DataTable
    Private m_tblQMMIDATENTable As DataTable
    Private m_tblLLSCHDATENTable As DataTable
    Private mGT_ADDR As DataTable
    Private mGT_EQUI As DataTable
    Private mGT_TEXT As DataTable

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
    Private mAnzBemerkungen As String
    Private mVersandgrund As String

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

    Public ReadOnly Property LLSCHDATENTable() As DataTable
        Get
            Return m_tblLLSCHDATENTable
        End Get
    End Property

    Public ReadOnly Property diverseFahrzeuge() As DataTable
        Get
            Return mGT_EQUI
        End Get
    End Property

    Public ReadOnly Property GT_ADDR() As DataTable
        Get
            Return mGT_ADDR
        End Get
    End Property

    Public ReadOnly Property GT_TEXT() As DataTable
        Get
            Return mGT_TEXT
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

    Public Property AnzBemerkungen() As String
        Get
            Return mAnzBemerkungen
        End Get
        Set(ByVal value As String)
            mAnzBemerkungen = value
        End Set
    End Property

    Public Property Versandgrund() As String
        Get
            Return mVersandgrund
        End Get
        Set(ByVal value As String)
            mVersandgrund = value
        End Set
    End Property

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByVal objApp As CKG.Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal strAmtlKennzeichen As String, ByVal strFahrgestellnummer As String, ByVal strBriefnummer As String, ByVal strRef1 As String, ByVal equnr As String)
        m_strClassAndMethod = "History.FILL"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intStatus = 0

            Try
                Dim proxy = DynSapProxy.getProxy("Z_M_BRIEFLEBENSLAUF_001", m_objApp, m_objUser, CurrentPageHelper.GetCurrentPage())

                'befüllen der Importparameter
                proxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                proxy.setImportParameter("I_ZZKENN", strAmtlKennzeichen.ToUpper)
                proxy.setImportParameter("I_ZZFAHRG", strFahrgestellnummer.ToUpper)
                proxy.setImportParameter("I_ZZREF1", strRef1.ToUpper)
                proxy.setImportParameter("I_ZZBRIEF", strBriefnummer.ToUpper)
                proxy.setImportParameter("I_EQUNR", equnr)
                proxy.setImportParameter("I_FCE", "") '?
                proxy.setImportParameter("I_HAENDLER", m_objUser.Reference)

                proxy.callBapi()

                'auswerten der exportparameter
                mGT_ADDR = proxy.getExportTable("GT_ADDR")
                m_tblQMEL_DATENTable = proxy.getExportTable("GT_QMEL")
                m_tblQMMIDATENTable = proxy.getExportTable("GT_QMMA")
                m_tblLLSCHDATENTable = proxy.getExportTable("GT_LLSCH")
                m_tblBRIEFLEBENSLAUF_LPTable = proxy.getExportTable("GT_WEB")
                mGT_EQUI = proxy.getExportTable("GT_EQUI")
                mGT_TEXT = proxy.getExportTable("GT_TEXT")

                For Each row As DataRow In m_tblQMEL_DATENTable.Rows
                    If Not row("STRMN") Is Nothing Then
                        row("STRMN") = row("STRMN").ToString.TrimStart("0"c)
                    End If
                    If Not TypeOf row("ERDAT") Is DBNull Then
                        row("ERDAT") = row("ERDAT").ToString.TrimStart("0"c)
                    End If
                Next

                For Each row As DataRow In m_tblQMMIDATENTable.Rows
                    If Not row("ZZUEBER") Is Nothing Then
                        row("ZZUEBER") = row("ZZUEBER").ToString.TrimStart("0"c)
                    End If
                    If Not TypeOf row("PSTER") Is DBNull Then
                        row("PSTER") = row("PSTER").ToString.TrimStart("0"c)
                    End If
                    If Not TypeOf row("AEDAT") Is DBNull Then
                        row("AEDAT") = row("AEDAT").ToString.TrimStart("0"c)
                    End If
                    If Not TypeOf row("AEZEIT") Is DBNull Then
                        row("AEZEIT") = row("AEZEIT").ToString.TrimStart("0"c)
                        If row("AEZEIT").ToString <> String.Empty Then
                            row("AEZEIT") = Left(row("AEZEIT").ToString, 2) & ":" & row("AEZEIT").ToString.Substring(2, 2)
                        End If
                    End If
                Next

                For Each row As DataRow In m_tblLLSCHDATENTable.Rows
                    If Not row("STRMN") Is Nothing Then
                        row("STRMN") = row("STRMN").ToString.TrimStart("0"c)
                    End If
                    If Not TypeOf row("ERDAT") Is DBNull Then
                        row("ERDAT") = row("ERDAT").ToString.TrimStart("0"c)
                    End If
                Next

                'übersetzung navi 
                For Each row As DataRow In m_tblBRIEFLEBENSLAUF_LPTable.Rows
                    Select Case row("ZZNAVI").ToString
                        Case "N"
                            row("ZZNAVI") = "NAVI"
                        Case "K"
                            row("ZZNAVI") = "NEIN"
                    End Select
                Next

                'bearbeiten der Adresstabelle für die Ausgabe
                mGT_ADDR.Columns.Add("Anschrift", Type.GetType("System.String"))
                mGT_ADDR.Columns.Add("LieferantAdresse", Type.GetType("System.String"))
                For Each tmprow As DataRow In mGT_ADDR.Rows

                    Select Case tmprow("ADDRTYP").ToString
                        Case "ZP"
                            tmprow("LieferantAdresse") = CStr(tmprow("NAME1")) & " " & CStr(tmprow("NAME2")) & " " & CStr(tmprow("NAME3")) & "<br>" & CStr(tmprow("STREET")) & " " & CStr(tmprow("HOUSE_NUM1")) & "<br>" & CStr(tmprow("POST_CODE1")) & " " & CStr(tmprow("CITY1"))
                            tmprow("Anschrift") = CStr(tmprow("NAME1")) & " " & CStr(tmprow("NAME2")) & " " & CStr(tmprow("NAME3")) & "<br>" & CStr(tmprow("STREET")) & " " & CStr(tmprow("HOUSE_NUM1")) & "<br>" & CStr(tmprow("POST_CODE1")) & " " & CStr(tmprow("CITY1")) & "<br>" & CStr(tmprow("COUNTRY"))
                        Case Else
                            tmprow("Anschrift") = CStr(tmprow("NAME1")) & " " & CStr(tmprow("NAME2")) & " " & CStr(tmprow("NAME3")) & "<br>" & CStr(tmprow("STREET")) & " " & CStr(tmprow("HOUSE_NUM1")) & "<br>" & CStr(tmprow("POST_CODE1")) & " " & CStr(tmprow("CITY1")) & "<br>" & CStr(tmprow("COUNTRY"))
                    End Select

                Next
                mGT_ADDR.AcceptChanges()

                'bearbeitung der EQUI Tabelle für die Ausgabe
                If Not mGT_EQUI Is Nothing Then
                    CreateOutPut(mGT_EQUI, strAppID)
                    mGT_EQUI = m_tblResult
                End If

                mAnzBemerkungen = ""
                If mGT_TEXT IsNot Nothing AndAlso m_tblBRIEFLEBENSLAUF_LPTable.Rows.Count = 1 Then
                    'Bemerkungen in Property AnzBemerkungen
                    For Each row As DataRow In mGT_TEXT.Rows
                        mAnzBemerkungen &= row("TDLINE").ToString & vbCrLf
                    Next
                End If

            Catch ex As Exception
                mGT_ADDR = Nothing
                m_tblBRIEFLEBENSLAUF_LPTable = Nothing
                m_tblQMEL_DATENTable = Nothing
                m_tblQMMIDATENTable = Nothing
                m_tblLLSCHDATENTable = Nothing
                mGT_EQUI = Nothing
                mGT_TEXT = Nothing

                m_intStatus = -2222
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Ergebnisse für die angegebenen Kriterien."
                    Case "NO_WEB"
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case "HAENDLER_NOT_FOUND"
                        m_strMessage = "Händler konnte nicht gefunden werden"
                    Case Else
                        m_strMessage = ex.Message
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region

End Class
