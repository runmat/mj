Option Explicit On
Option Strict On

Imports CKG.Base.Business

Public Class UeberfDAD
    Inherits Base.Business.DatenimportBase

#Region "Declarations"

    '***Auftragsart****
    Private Enum Auftragsarten
        Zulassung = 1
        Auslieferung = 2
        ZulassungAuslieferung = 3
        AuslieferungRueckfuehrung = 4
        Rueckfuehrung = 5
        Alles = 6
    End Enum

    Public tblKreis As DataTable
    'Allgemein
    Private mDatum As String

    '***Rückgabe Ausftragsnummern aus SAP***
    Private mAuftragsnummerUeberf As String
    Private mAuftragsnummerZul As String


    '***Leasingnehmer***
    Private mLeasingnehmernummer As String
    Private mLeasingnehmerName As String
    Private mLeasingnehmerStrasse As String
    Private mLeasingnehmerPLZ As String
    Private mLeasingnehmerOrt As String
    Private mLeasingnehmerLand As String
    Private mLeasingnehmerReferenz As String
    Private mBuchungscode As String

    '***Halter***
    Private mHalterName As String
    Private mHalterName2 As String
    Private mHalterStrasse As String
    Private mHalterPLZ As String
    Private mHalterOrt As String
    Private mHalterLand As String

    '***Händler***
    Private mHaendlerName1 As String
    Private mHaendlerStrasse As String
    Private mHaendlerPLZ As String
    Private mHaendlerOrt As String
    Private mHaendlerLand As String
    Private mHaendlerAnsprech As String
    Private mHaendlerTelefon As String
    Private mHaendlerTelefon2 As String
    Private mHaendlerMail As String

    '***VersandSchein und Schilder***
    Private mVssName As String
    Private mVssName2 As String
    Private mVssStrasse As String
    Private mVssPLZ As String
    Private mVssOrt As String

    '***Fahrzeugnutzer***
    Private mFnName As String
    Private mFnTelefon As String
    Private mFnMail As String

    '***Zulassung***
    Private mEquiNr As String
    Private mLnFahrgestellnummer As String
    Private mFahrzeugTyp As String
    Private mBriefnummer As String
    Private mZulassungsdatum As String
    Private mZulTerminart As String
    Private mWunschkennzeichen1 As String
    Private mWunschkennzeichen2 As String
    Private mWunschkennzeichen3 As String
    Private mZulassungskreis As String
    Private mResNummer As String
    Private mResName As String
    Private mFeinstaub As String
    Private mKfzSteuer As String

    '***Versicherung***
    Private mVersGesellschaft As String
    Private mEVBNummer As String
    Private mEVBVon As String
    Private mEVBBis As String
    Private mVersNehmer As String
    Private mVersNehmerArt As String
    Private mVersNehmerStrasse As String
    Private mVersNehmerPLZ As String
    Private mVersNehmerOrt As String
    Private mVersNehmerLand As String
    'Private mVersAnsprechpartner As String
    'Private mVersTelefon As String
    'Private mVersMail As String

    'Auslieferung
    Private mSomWi As String
    Private mKennzeichen As String
    Private mBemerkung5 As String = String.Empty

    Private mHandyadapter As String = String.Empty
    Private mVerbandskasten As String = String.Empty
    Private mNaviCD As String = String.Empty
    Private mWarndreieck As String = String.Empty
    Private mWarnweste As String = String.Empty
    Private mFussmatten As String = String.Empty
    Private mFKontrolle As String = String.Empty
    Private mServicekarten As String = ""
    Private mTankkarten As String = ""
    Private mTanken As String = ""
    Private mWaesche As String = ""
    Private mFzgEinweisung As String = ""



    Private mStatus As String = "Neu"
    Private mFahrzeugklasseText As String
    Private mFahrzeugklasseValue As String
    Private mTerminhinweisAus As String
    Private mTerminhinweisAusValue As String
    Private mAuslieferungDatum As String


    Private mBemerkungAus As String

    'Fahrzeugempfänger
    Private mEmpfName As String
    Private mEmpfStrasse As String
    Private mEmpfPLZ As String
    Private mEmpfOrt As String
    Private mEmpfLand As String
    Private mEmpfAnsprechpartner As String
    Private mEmpfTelefon As String
    Private mEmpfTelefon2 As String
    Private mEmpfMail As String

    'Überführung
    Private mWunschtermin As String

    'Adressen
    Public Adressen As DataTable

    'Autoland-Adressen
    Public CheckAdressen As DataTable

    'Winterreifenhandling
    Private mWinterHandling As Boolean
    Private mWinterText As String
    Private mWinterFelgen As String
    Private mWinterRadkappen As String
    Private mWinterZweiterRadsatz As String
    Private mWinterReifenquelle As String
    Private mWinterGroesser As Boolean
    Private mWinterGroesse As String

    'Länder
    Public Laender As DataTable

    '***Auftragsart
    Private mAuftragsart As String

    '***Rückführung/Anschlußfahrt

    'Leasingnehmer
    Private mRLeasingnehmernummer As String
    Private mRLeasingnehmerReferenz As String
    Private mRAnsprechLeasing As String



    'Fahrzeugnutzer
    Private mRFnName As String
    Private mRFnTelefon As String
    Private mRFnMail As String

    'Detaildaten
    Private mRFahrzeugtyp As String
    Private mRFahrgestellnummer As String
    Private mRSomWi As String
    Private mRKennzeichen As String
    Private mRStatus As String = String.Empty
    Private mRFahrzeugklasseText As String
    Private mRFahrzeugklasseValue As String
    Private mRWunschtermin As String
    Private mRWunschterminart As String
    Private mRBemerkung As String

    'Abholadresse
    Private mRAbName As String
    Private mRAbStrasse As String
    Private mRAbPLZ As String
    Private mRAbOrt As String
    Private mRAbLand As String
    Private mRAbAnsprechpartner As String
    Private mRAbTelefon As String
    Private mRAbHandy As String
    Private mRAbMail As String

    'Anlieferadresse
    Private mRAnlieferungArt As String
    Private mRAnName As String
    Private mRAnStrasse As String
    Private mRAnPLZ As String
    Private mRAnOrt As String
    Private mRAnLand As String
    Private mRAnAnsprechpartner As String
    Private mRAnTelefon As String
    Private mRAnHandy As String
    Private mRAnMail As String

#End Region

#Region "Properties"

    'Allgemein
    Public ReadOnly Property Datum() As String
        Get
            mDatum = Today.ToShortDateString

            Return mDatum

        End Get
    End Property

    '***Rückgabe Ausftragsnummern aus SAP***
    Public Property AuftragsnummerUeberf() As String
        Get
            Return mAuftragsnummerUeberf
        End Get
        Set(ByVal Value As String)
            mAuftragsnummerUeberf = Value
        End Set
    End Property

    Public Property AuftragsnummerZul() As String
        Get
            Return mAuftragsnummerZul
        End Get
        Set(ByVal Value As String)
            mAuftragsnummerZul = Value
        End Set
    End Property


    '***Leasingnehmer***
    Public Property Leasingnehmernummer() As String
        Get
            Return mLeasingnehmernummer
        End Get
        Set(ByVal Value As String)
            mLeasingnehmernummer = Value
        End Set
    End Property

    Public Property Leasingnehmer() As String
        Get
            Return mLeasingnehmerName
        End Get
        Set(ByVal Value As String)
            mLeasingnehmerName = Value
        End Set
    End Property

    Public Property LeasingnehmerStrasse() As String
        Get
            Return mLeasingnehmerStrasse
        End Get
        Set(ByVal Value As String)
            mLeasingnehmerStrasse = Value
        End Set
    End Property

    Public Property LeasingnehmerPLZ() As String
        Get
            Return mLeasingnehmerPLZ
        End Get
        Set(ByVal Value As String)
            mLeasingnehmerPLZ = Value
        End Set
    End Property

    Public Property LeasingnehmerOrt() As String
        Get
            Return mLeasingnehmerOrt
        End Get
        Set(ByVal Value As String)
            mLeasingnehmerOrt = Value
        End Set
    End Property

    Public Property LeasingnehmerLand() As String
        Get
            Return mLeasingnehmerLand
        End Get
        Set(ByVal Value As String)
            mLeasingnehmerLand = Value
        End Set
    End Property

    Public Property LeasingnehmerReferenz() As String
        Get
            Return mLeasingnehmerReferenz
        End Get
        Set(ByVal Value As String)
            mLeasingnehmerReferenz = Value
        End Set
    End Property

    Public Property Buchungscode() As String
        Get
            Return mBuchungscode
        End Get
        Set(ByVal Value As String)
            mBuchungscode = Value
        End Set
    End Property


    '***Halter***
    Public Property Halter() As String
        Get
            Return mHalterName
        End Get
        Set(ByVal Value As String)
            mHalterName = Value
        End Set
    End Property
    Public Property Halter2() As String
        Get
            Return mHalterName2
        End Get
        Set(ByVal Value As String)
            mHalterName2 = Value
        End Set
    End Property
    Public Property HalterStrasse() As String
        Get
            Return mHalterStrasse
        End Get
        Set(ByVal Value As String)
            mHalterStrasse = Value
        End Set
    End Property

    Public Property HalterPLZ() As String
        Get
            Return mHalterPLZ
        End Get
        Set(ByVal Value As String)
            mHalterPLZ = Value
        End Set
    End Property

    Public Property HalterOrt() As String
        Get
            Return mHalterOrt
        End Get
        Set(ByVal Value As String)
            mHalterOrt = Value
        End Set
    End Property

    Public Property HalterLand() As String
        Get
            Return mHalterLand
        End Get
        Set(ByVal Value As String)
            mHalterLand = Value
        End Set
    End Property


    '***Händler***
    Public Property HaendlerName1() As String
        Get
            Return mHaendlerName1
        End Get
        Set(ByVal Value As String)
            mHaendlerName1 = Value
        End Set
    End Property

    Public Property HaendlerStrasse() As String
        Get
            Return mHaendlerStrasse
        End Get
        Set(ByVal Value As String)
            mHaendlerStrasse = Value
        End Set
    End Property

    Public Property HaendlerPLZ() As String
        Get
            Return mHaendlerPLZ
        End Get
        Set(ByVal Value As String)
            mHaendlerPLZ = Value
        End Set
    End Property

    Public Property HaendlerOrt() As String
        Get
            Return mHaendlerOrt
        End Get
        Set(ByVal Value As String)
            mHaendlerOrt = Value
        End Set
    End Property

    Public Property HaendlerLand() As String
        Get
            Return mHaendlerLand
        End Get
        Set(ByVal Value As String)
            mHaendlerLand = Value
        End Set
    End Property

    Public Property HaendlerAnsprech() As String
        Get
            Return mHaendlerAnsprech
        End Get
        Set(ByVal Value As String)
            mHaendlerAnsprech = Value
        End Set
    End Property

    Public Property HaendlerTelefon() As String
        Get
            Return mHaendlerTelefon
        End Get
        Set(ByVal Value As String)
            mHaendlerTelefon = Value
        End Set
    End Property

    Public Property HaendlerTelefon2() As String
        Get
            Return mHaendlerTelefon2
        End Get
        Set(ByVal Value As String)
            mHaendlerTelefon2 = Value
        End Set
    End Property

    Public Property HaendlerMail() As String
        Get
            Return mHaendlerMail
        End Get
        Set(ByVal Value As String)
            mHaendlerMail = Value
        End Set
    End Property

    '***Versand Schein und Schilder***
    Public Property VssName() As String
        Get
            Return mVssName
        End Get
        Set(ByVal Value As String)
            mVssName = Value
        End Set
    End Property
    Public Property VssName2() As String
        Get
            Return mVssName2
        End Get
        Set(ByVal Value As String)
            mVssName2 = Value
        End Set
    End Property
    Public Property VssStrasse() As String
        Get
            Return mVssStrasse
        End Get
        Set(ByVal Value As String)
            mVssStrasse = Value
        End Set
    End Property

    Public Property VssPLZ() As String
        Get
            Return mVssPLZ
        End Get
        Set(ByVal Value As String)
            mVssPLZ = Value
        End Set
    End Property

    Public Property VssOrt() As String
        Get
            Return mVssOrt
        End Get
        Set(ByVal Value As String)
            mVssOrt = Value
        End Set
    End Property

    '***Fahrzeugempfänger***
    Public Property EmpfName() As String
        Get
            Return mEmpfName
        End Get
        Set(ByVal Value As String)
            mEmpfName = Value
        End Set
    End Property

    Public Property EmpfStrasse() As String
        Get
            Return mEmpfStrasse
        End Get
        Set(ByVal Value As String)
            mEmpfStrasse = Value
        End Set
    End Property

    Public Property EmpfPLZ() As String
        Get
            Return mEmpfPLZ
        End Get
        Set(ByVal Value As String)
            mEmpfPLZ = Value
        End Set
    End Property

    Public Property EmpfOrt() As String
        Get
            Return mEmpfOrt
        End Get
        Set(ByVal Value As String)
            mEmpfOrt = Value
        End Set
    End Property

    Public Property EmpfLand() As String
        Get
            Return mEmpfLand
        End Get
        Set(ByVal Value As String)
            mEmpfLand = Value
        End Set
    End Property

    Public Property EmpfAnsprechpartner() As String
        Get
            Return mEmpfAnsprechpartner
        End Get
        Set(ByVal Value As String)
            mEmpfAnsprechpartner = Value
        End Set
    End Property

    Public Property EmpfTelefon() As String
        Get
            Return mEmpfTelefon
        End Get
        Set(ByVal Value As String)
            mEmpfTelefon = Value
        End Set
    End Property

    Public Property EmpfTelefon2() As String
        Get
            Return mEmpfTelefon2
        End Get
        Set(ByVal Value As String)
            mEmpfTelefon2 = Value
        End Set
    End Property

    Public Property EmpfMail() As String
        Get
            Return mEmpfMail
        End Get
        Set(ByVal Value As String)
            mEmpfMail = Value
        End Set
    End Property

    '***Fahrzeugnutzer***
    Public Property FnName() As String
        Get
            Return mFnName
        End Get
        Set(ByVal Value As String)
            mFnName = Value
        End Set
    End Property

    Public Property FnTelefon() As String
        Get
            Return mFnTelefon
        End Get
        Set(ByVal Value As String)
            mFnTelefon = Value
        End Set
    End Property

    Public Property FnMail() As String
        Get
            Return mFnMail
        End Get
        Set(ByVal Value As String)
            mFnMail = Value
        End Set
    End Property

    '***Zulassung***
    Public Property EquiNr() As String
        Get
            Return mEquiNr
        End Get
        Set(ByVal Value As String)
            mEquiNr = Value
        End Set
    End Property


    Public Property LnFahrgestellnummer() As String
        Get
            Return mLnFahrgestellnummer
        End Get
        Set(ByVal Value As String)
            mLnFahrgestellnummer = Value
        End Set
    End Property

    Public Property Fahrzeugtyp() As String
        Get
            Return mFahrzeugTyp
        End Get
        Set(ByVal Value As String)
            mFahrzeugTyp = Value
        End Set
    End Property

    Public Property Briefnummer() As String
        Get
            Return mBriefnummer
        End Get
        Set(ByVal Value As String)
            mBriefnummer = Value
        End Set
    End Property

    Public Property Zulassungsdatum() As String
        Get
            Return mZulassungsdatum
        End Get
        Set(ByVal Value As String)
            mZulassungsdatum = Value
        End Set
    End Property

    Public Property ZulTerminart() As String
        Get
            Return mZulTerminart
        End Get
        Set(ByVal Value As String)
            mZulTerminart = Value
        End Set
    End Property

    Public Property Wunschkennzeichen1() As String
        Get
            Return mWunschkennzeichen1
        End Get
        Set(ByVal Value As String)
            mWunschkennzeichen1 = Value
        End Set
    End Property

    Public Property Wunschkennzeichen2() As String
        Get
            Return mWunschkennzeichen2
        End Get
        Set(ByVal Value As String)
            mWunschkennzeichen2 = Value
        End Set
    End Property

    Public Property Wunschkennzeichen3() As String
        Get
            Return mWunschkennzeichen3
        End Get
        Set(ByVal Value As String)
            mWunschkennzeichen3 = Value
        End Set
    End Property

    Public Property Zulassungskreis() As String
        Get
            Return mZulassungskreis
        End Get
        Set(ByVal Value As String)
            mZulassungskreis = Value
        End Set
    End Property

    Public Property ResNummer() As String
        Get
            Return mResNummer
        End Get
        Set(ByVal Value As String)
            mResNummer = Value
        End Set
    End Property

    Public Property ResName() As String
        Get
            Return mResName
        End Get
        Set(ByVal Value As String)
            mResName = Value
        End Set
    End Property

    Public Property Feinstaub() As String
        Get
            Return mFeinstaub
        End Get
        Set(ByVal Value As String)
            mFeinstaub = Value
        End Set
    End Property

    Public Property KfzSteuer() As String
        Get
            Return mKfzSteuer
        End Get
        Set(ByVal Value As String)
            mKfzSteuer = Value
        End Set
    End Property

    '***Versicherung***
    Public Property VersGesellschaft() As String
        Get
            Return mVersGesellschaft
        End Get
        Set(ByVal Value As String)
            mVersGesellschaft = Value
        End Set
    End Property
    Public Property EVBNummer() As String
        Get
            Return mEVBNummer
        End Get
        Set(ByVal Value As String)
            mEVBNummer = Value
        End Set
    End Property

    Public Property EVBVon() As String
        Get
            Return mEVBVon
        End Get
        Set(ByVal Value As String)
            mEVBVon = Value
        End Set
    End Property
    Public Property EVBBis() As String
        Get
            Return mEVBBis
        End Get
        Set(ByVal Value As String)
            mEVBBis = Value
        End Set
    End Property

    Public Property VersNehmerArt() As String
        Get
            Return mVersNehmerArt
        End Get
        Set(ByVal Value As String)
            mVersNehmerArt = Value
        End Set
    End Property

    Public Property VersNehmer() As String
        Get
            Return mVersNehmer
        End Get
        Set(ByVal Value As String)
            mVersNehmer = Value
        End Set
    End Property

    Public Property VersNehmerStrasse() As String
        Get
            Return mVersNehmerStrasse
        End Get
        Set(ByVal Value As String)
            mVersNehmerStrasse = Value
        End Set
    End Property

    Public Property VersNehmerPLZ() As String
        Get
            Return mVersNehmerPLZ
        End Get
        Set(ByVal Value As String)
            mVersNehmerPLZ = Value
        End Set
    End Property

    Public Property VersNehmerOrt() As String
        Get
            Return mVersNehmerOrt
        End Get
        Set(ByVal Value As String)
            mVersNehmerOrt = Value
        End Set
    End Property

    Public Property VersNehmerLand() As String
        Get
            Return mVersNehmerLand
        End Get
        Set(ByVal Value As String)
            mVersNehmerLand = Value
        End Set
    End Property

    'Public Property VersAnsprechpartner() As String
    '    Get
    '        Return mVersAnsprechpartner
    '    End Get
    '    Set(ByVal Value As String)
    '        mVersAnsprechpartner = Value
    '    End Set
    'End Property

    'Public Property VersTelefon() As String
    '    Get
    '        Return mVersTelefon
    '    End Get
    '    Set(ByVal Value As String)
    '        mVersTelefon = Value
    '    End Set
    'End Property

    'Public Property VersMail() As String
    '    Get
    '        Return mVersMail
    '    End Get
    '    Set(ByVal Value As String)
    '        mVersMail = Value
    '    End Set
    'End Property

    '***Auslieferung***

    Public Property SomWin() As String
        Get
            Return mSomWi
        End Get
        Set(ByVal Value As String)
            mSomWi = Value
        End Set
    End Property

    Public Property Tanken() As String
        Get
            Return mTanken
        End Get
        Set(ByVal Value As String)
            mTanken = Value
        End Set
    End Property

    Public Property Waesche() As String
        Get
            Return mWaesche
        End Get
        Set(ByVal Value As String)
            mWaesche = Value
        End Set
    End Property

    Public Property FzgEinweisung() As String
        Get
            Return mFzgEinweisung
        End Get
        Set(ByVal Value As String)
            mFzgEinweisung = Value
        End Set
    End Property
    Public Property Kennzeichen() As String
        Get
            Return mKennzeichen
        End Get
        Set(ByVal Value As String)
            mKennzeichen = Value
        End Set
    End Property

    Public Property HandyAdapter() As String
        Get
            Return mHandyadapter
        End Get
        Set(ByVal Value As String)
            mHandyadapter = Value
        End Set
    End Property

    Public Property Verbandskasten() As String
        Get
            Return mVerbandskasten
        End Get
        Set(ByVal Value As String)
            mVerbandskasten = Value
        End Set
    End Property

    Public Property NaviCD() As String
        Get
            Return mNaviCD
        End Get
        Set(ByVal Value As String)
            mNaviCD = Value
        End Set
    End Property

    Public Property Warndreieck() As String
        Get
            Return mWarndreieck
        End Get
        Set(ByVal Value As String)
            mWarndreieck = Value
        End Set
    End Property

    Public Property Warnweste() As String
        Get
            Return mWarnweste
        End Get
        Set(ByVal Value As String)
            mWarnweste = Value
        End Set
    End Property

    Public Property Fussmatten() As String
        Get
            Return mFussmatten
        End Get
        Set(ByVal Value As String)
            mFussmatten = Value
        End Set
    End Property

    Public Property FKontrolle() As String
        Get
            Return mFKontrolle
        End Get
        Set(ByVal Value As String)
            mFKontrolle = Value
        End Set
    End Property

    Public Property Bemerkung5() As String
        Get
            Return mBemerkung5
        End Get
        Set(ByVal Value As String)
            mBemerkung5 = Value
        End Set
    End Property

    Public Property FahrzeugStatus() As String
        Get
            Return mStatus
        End Get
        Set(ByVal Value As String)
            mStatus = Value
        End Set
    End Property

    Public Property FahrzeugklasseValue() As String
        Get
            Return mFahrzeugklasseValue
        End Get
        Set(ByVal Value As String)
            mFahrzeugklasseValue = Value
        End Set
    End Property

    Public Property FahrzeugklasseText() As String
        Get
            Return mFahrzeugklasseText
        End Get
        Set(ByVal Value As String)
            mFahrzeugklasseText = Value
        End Set
    End Property
    Public Property TerminhinweisAuslieferung() As String
        Get
            Return mTerminhinweisAus
        End Get
        Set(ByVal Value As String)
            mTerminhinweisAus = Value
        End Set
    End Property

    Public Property TerminhinweisAuslieferungValue() As String
        Get
            Return mTerminhinweisAusValue
        End Get
        Set(ByVal Value As String)
            mTerminhinweisAusValue = Value
        End Set
    End Property

    Public Property AuslieferungDatum() As String
        Get
            Return mAuslieferungDatum
        End Get
        Set(ByVal Value As String)
            mAuslieferungDatum = Value
        End Set
    End Property

    Public Property Tankkarten() As String
        Get
            Return mTankkarten
        End Get
        Set(ByVal Value As String)
            mTankkarten = Value
        End Set
    End Property

    Public Property Servicekarte() As String
        Get
            Return mServicekarten
        End Get
        Set(ByVal Value As String)
            mServicekarten = Value
        End Set
    End Property

    Public Property BemerkungAus() As String
        Get
            Return mBemerkungAus
        End Get
        Set(ByVal Value As String)
            mBemerkungAus = Value
        End Set
    End Property

    '***Winterreifenhandling***
    Public Property WinterText() As String
        Get
            Return mWinterText
        End Get
        Set(ByVal Value As String)
            mWinterText = Value
        End Set
    End Property

    Public Property WinterFelgen() As String
        Get
            Return mWinterFelgen
        End Get
        Set(ByVal Value As String)
            mWinterFelgen = Value
        End Set
    End Property

    Public Property WinterRadkappen() As String
        Get
            Return mWinterRadkappen
        End Get
        Set(ByVal Value As String)
            mWinterRadkappen = Value
        End Set
    End Property

    Public Property WinterZweiterRadsatz() As String
        Get
            Return mWinterZweiterRadsatz
        End Get
        Set(ByVal Value As String)
            mWinterZweiterRadsatz = Value
        End Set
    End Property

    Public Property WinterReifenquelle() As String
        Get
            Return mWinterReifenquelle
        End Get
        Set(ByVal Value As String)
            mWinterReifenquelle = Value
        End Set
    End Property

    Public Property WinterGroesse() As String
        Get
            Return mWinterGroesse
        End Get
        Set(ByVal Value As String)
            mWinterGroesse = Value
        End Set
    End Property

    Public Property WinterGroesser() As Boolean
        Get
            Return mWinterGroesser
        End Get
        Set(ByVal Value As Boolean)
            mWinterGroesser = Value
        End Set
    End Property

    Public Property WinterHandling() As Boolean
        Get
            Return mWinterHandling
        End Get
        Set(ByVal Value As Boolean)
            mWinterHandling = Value
        End Set
    End Property

    '***Überführung***
    Public Property Wunschtermin() As String
        Get
            Return mWunschtermin
        End Get
        Set(ByVal Value As String)
            mWunschtermin = Value
        End Set
    End Property


    '***Auftragsart
    Public Property Auftragsart() As String
        Get
            Return mAuftragsart
        End Get
        Set(ByVal Value As String)
            mAuftragsart = Value
        End Set
    End Property

    '***Rückführung/Anschlußfahrt

    'Leasingnehmer
    Public Property RLeasingnehmernummer() As String
        Get
            Return mRLeasingnehmernummer
        End Get
        Set(ByVal Value As String)
            mRLeasingnehmernummer = Value
        End Set
    End Property

    Public Property RLeasingnehmerReferenz() As String
        Get
            Return mRLeasingnehmerReferenz
        End Get
        Set(ByVal Value As String)
            mRLeasingnehmerReferenz = Value
        End Set
    End Property

    Public Property RAnsprechLeasing() As String
        Get
            Return mRAnsprechLeasing
        End Get
        Set(ByVal Value As String)
            mRAnsprechLeasing = Value
        End Set
    End Property

    'Fahrzeugnutzer
    Public Property RFnName() As String
        Get
            Return mRFnName
        End Get
        Set(ByVal Value As String)
            mRFnName = Value
        End Set
    End Property

    Public Property RFnTelefon() As String
        Get
            Return mRFnTelefon
        End Get
        Set(ByVal Value As String)
            mRFnTelefon = Value
        End Set
    End Property

    Public Property RFnMail() As String
        Get
            Return mRFnMail
        End Get
        Set(ByVal Value As String)
            mRFnMail = Value
        End Set
    End Property

    'Detaildaten
    Public Property RFahrzeugtyp() As String
        Get
            Return mRFahrzeugtyp
        End Get
        Set(ByVal Value As String)
            mRFahrzeugtyp = Value
        End Set
    End Property

    Public Property RFahrgestellnummer() As String
        Get
            Return mRFahrgestellnummer
        End Get
        Set(ByVal Value As String)
            mRFahrgestellnummer = Value
        End Set
    End Property

    Public Property RSomWin() As String
        Get
            Return mRSomWi
        End Get
        Set(ByVal Value As String)
            mRSomWi = Value
        End Set
    End Property

    Public Property RKennzeichen() As String
        Get
            Return mRKennzeichen
        End Get
        Set(ByVal Value As String)
            mRKennzeichen = Value
        End Set
    End Property

    Public Property RFahrzeugStatus() As String
        Get
            Return mRStatus
        End Get
        Set(ByVal Value As String)
            mRStatus = Value
        End Set
    End Property

    Public Property RFahrzeugklasseValue() As String
        Get
            Return mRFahrzeugklasseValue
        End Get
        Set(ByVal Value As String)
            mRFahrzeugklasseValue = Value
        End Set
    End Property

    Public Property RFahrzeugklasseText() As String
        Get
            Return mRFahrzeugklasseText
        End Get
        Set(ByVal Value As String)
            mRFahrzeugklasseText = Value
        End Set
    End Property
    Public Property RWunschtermin() As String
        Get
            Return mRWunschtermin
        End Get
        Set(ByVal Value As String)
            mRWunschtermin = Value
        End Set
    End Property

    Public Property RWunschterminart() As String
        Get
            Return mRWunschterminart
        End Get
        Set(ByVal Value As String)
            mRWunschterminart = Value
        End Set
    End Property

    Public Property RBemerkung() As String
        Get
            Return mRBemerkung
        End Get
        Set(ByVal Value As String)
            mRBemerkung = Value
        End Set
    End Property

    'Abholadresse
    Public Property RAbName() As String
        Get
            Return mRAbName
        End Get
        Set(ByVal Value As String)
            mRAbName = Value
        End Set
    End Property

    Public Property RAbStrasse() As String
        Get
            Return mRAbStrasse
        End Get
        Set(ByVal Value As String)
            mRAbStrasse = Value
        End Set
    End Property

    Public Property RAbPLZ() As String
        Get
            Return mRAbPLZ
        End Get
        Set(ByVal Value As String)
            mRAbPLZ = Value
        End Set
    End Property

    Public Property RAbOrt() As String
        Get
            Return mRAbOrt
        End Get
        Set(ByVal Value As String)
            mRAbOrt = Value
        End Set
    End Property

    Public Property RAbLand() As String
        Get
            Return mRAbLand
        End Get
        Set(ByVal Value As String)
            mRAbLand = Value
        End Set
    End Property

    Public Property RAbAnsprechpartner() As String
        Get
            Return mRAbAnsprechpartner
        End Get
        Set(ByVal Value As String)
            mRAbAnsprechpartner = Value
        End Set
    End Property

    Public Property RAbTelefon() As String
        Get
            Return mRAbTelefon
        End Get
        Set(ByVal Value As String)
            mRAbTelefon = Value
        End Set
    End Property

    Public Property RAbHandy() As String
        Get
            Return mRAbHandy
        End Get
        Set(ByVal Value As String)
            mRAbHandy = Value
        End Set
    End Property

    Public Property RAbMail() As String
        Get
            Return mRAbMail
        End Get
        Set(ByVal Value As String)
            mRAbMail = Value
        End Set
    End Property

    'Anlieferadresse

    Public Property RAnlieferungArt() As String
        Get
            Return mRAnlieferungArt
        End Get
        Set(ByVal Value As String)
            mRAnlieferungArt = Value
        End Set
    End Property

    Public Property RAnName() As String
        Get
            Return mRAnName
        End Get
        Set(ByVal Value As String)
            mRAnName = Value
        End Set
    End Property

    Public Property RAnStrasse() As String
        Get
            Return mRAnStrasse
        End Get
        Set(ByVal Value As String)
            mRAnStrasse = Value
        End Set
    End Property

    Public Property RAnPLZ() As String
        Get
            Return mRAnPLZ
        End Get
        Set(ByVal Value As String)
            mRAnPLZ = Value
        End Set
    End Property

    Public Property RAnOrt() As String
        Get
            Return mRAnOrt
        End Get
        Set(ByVal Value As String)
            mRAnOrt = Value
        End Set
    End Property

    Public Property RAnLand() As String
        Get
            Return mRAnLand
        End Get
        Set(ByVal Value As String)
            mRAnLand = Value
        End Set
    End Property

    Public Property RAnAnsprechpartner() As String
        Get
            Return mRAnAnsprechpartner
        End Get
        Set(ByVal Value As String)
            mRAnAnsprechpartner = Value
        End Set
    End Property

    Public Property RAnTelefon() As String
        Get
            Return mRAnTelefon
        End Get
        Set(ByVal Value As String)
            mRAnTelefon = Value
        End Set
    End Property

    Public Property RAnHandy() As String
        Get
            Return mRAnHandy
        End Get
        Set(ByVal Value As String)
            mRAnHandy = Value
        End Set
    End Property

    Public Property RAnMail() As String
        Get
            Return mRAnMail
        End Get
        Set(ByVal Value As String)
            mRAnMail = Value
        End Set
    End Property

    Public ReadOnly Property StrassenverkehrsamtText() As String
        Get
            If Not Me.tblKreis Is Nothing AndAlso Me.tblKreis.Rows.Count > 0 Then

                Return "Ermittelter Zulassungskreis: " & Me.tblKreis.Rows(0)("ZKFZKZ").ToString() & "/" & _
                                    Me.tblKreis.Rows(0)("ZKREIS").ToString()

            Else

                Return ""

            End If

        End Get
    End Property



#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strFileName)
    End Sub

    '----------------------------------------------------------------------
    ' Methode:      GetVertragsdaten
    ' Autor:        SFa
    ' Beschreibung: Vertragsdaten holen und mit der gefundenen Leasingnehmer-
    '               nummer die Methode FillLeasingnehmer aufrufen
    ' Erstellt am:  27.08.2008
    ' ITA:          2050
    '----------------------------------------------------------------------
    Public Sub GetVertragsdaten(ByVal Referenz As String, ByVal ReferenzRueck As String)
        Dim LnData As New UeberfDADTables(m_objUser, m_objApp, "")

        Dim LeasingnehmerDaten As DataTable

        If Not Referenz = String.Empty Then

            Try
                LeasingnehmerDaten = LnData.GetLnVertragsdaten(Referenz)

                If IsNothing(LeasingnehmerDaten) = False Then


                    With LeasingnehmerDaten

                        'Vertragsdaten-Properties füllen
                        LnFahrgestellnummer = .Rows(0)("CHASSIS_NUM").ToString
                        Fahrzeugtyp = .Rows(0)("HERST").ToString & " " & .Rows(0)("Modell").ToString
                        Kennzeichen = .Rows(0)("LICENSE_NUM").ToString


                        'Händler-Properties füllen
                        HaendlerName1 = .Rows(0)("HD_NAME1").ToString
                        'HaendlerName2 = .Rows(0)("HD_NAME2").ToString
                        HaendlerStrasse = .Rows(0)("HD_STRAS").ToString
                        HaendlerPLZ = .Rows(0)("HD_PSTLZ").ToString
                        HaendlerOrt = .Rows(0)("HD_ORT01").ToString

                        'Fahrzeugnutzer
                        FnName = .Rows(0)("FR_NNAME").ToString & " " & .Rows(0)("FR_VNAME").ToString
                        FnTelefon = .Rows(0)("FR_TELEF").ToString
                        FnMail = .Rows(0)("FR_EMAIL").ToString

                        'Leasingnehmeradressdaten holen
                        FillLeasingnehmer(.Rows(0)("LN_KUNNR").ToString)
                    End With
                End If
            Catch ex As Exception
                'Fehler hat an dieser Stelle keine Auswirkung
            End Try

        End If

        If Not ReferenzRueck = String.Empty Then
            Try
                LeasingnehmerDaten = LnData.GetLnVertragsdaten(ReferenzRueck)

                If IsNothing(LeasingnehmerDaten) = False Then


                    With LeasingnehmerDaten

                        'Vertragsdaten-Properties füllen
                        RFahrgestellnummer = .Rows(0)("CHASSIS_NUM").ToString
                        RFahrzeugtyp = .Rows(0)("HERST").ToString & " " & .Rows(0)("Modell").ToString
                        RAnsprechLeasing = .Rows(0)("KB_NNAME").ToString & " " & .Rows(0)("KB_VNAME").ToString
                        RLeasingnehmernummer = .Rows(0)("LN_KUNNR").ToString
                        RKennzeichen = .Rows(0)("LICENSE_NUM").ToString

                        If Auftragsart = Auftragsarten.Rueckfuehrung.ToString Then
                            'Leasingnehmeradressdaten holen
                            FillLeasingnehmer(.Rows(0)("LN_KUNNR").ToString)
                        End If



                        'Fahrzeugnutzer
                        RFnName = .Rows(0)("FR_NNAME").ToString & " " & .Rows(0)("FR_VNAME").ToString
                        RFnTelefon = .Rows(0)("FR_TELEF").ToString
                        RFnMail = .Rows(0)("FR_EMAIL").ToString

                        'Leasingnehmerdaten in die Anlieferadresse eintragen
                        Dim TempTable As DataTable

                        TempTable = LnData.GetLnKunde(.Rows(0)("LN_KUNNR").ToString)

                        If IsNothing(TempTable) = False Then
                            If TempTable.Rows.Count > 0 Then
                                RAnName = TempTable.Rows(0)("NAME1_RL").ToString
                                RAnStrasse = TempTable.Rows(0)("STRAS_RL").ToString
                                RAnPLZ = TempTable.Rows(0)("PSTLZ_RL").ToString
                                RAnOrt = TempTable.Rows(0)("ORT01_RL").ToString
                                RAnAnsprechpartner = TempTable.Rows(0)("ANSPP_RL").ToString
                                RAnTelefon = TempTable.Rows(0)("TELF1_RL").ToString
                                RAnMail = TempTable.Rows(0)("EMAIL_RL").ToString
                            End If
                        End If

                    End With
                End If
            Catch ex As Exception
                'Fehler hat an dieser Stelle keine Auswirkung
            End Try

        End If


    End Sub

    '----------------------------------------------------------------------
    ' Methode:      FillLeasingnehmer
    ' Autor:        SFa
    ' Beschreibung: Die Leasingnehmer-Properties füllen
    ' Erstellt am:  26.08.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Public Sub FillLeasingnehmer(ByVal LnNummer As String, Optional ByRef RefreshLN As Boolean = False)
        Dim LnData As New UeberfDADTables(m_objUser, m_objApp, "")

        Dim LeasingnehmerDaten As DataTable = LnData.GetLnKunde(LnNummer)

        If IsNothing(LeasingnehmerDaten) = False Then

            With LeasingnehmerDaten

                'Leasingnehmer-Properties füllen
                Leasingnehmernummer = .Rows(0)("EXKUNNR_ZL").ToString
                Leasingnehmer = .Rows(0)("NAME1_ZL").ToString
                LeasingnehmerStrasse = .Rows(0)("STRAS_ZL").ToString
                LeasingnehmerPLZ = .Rows(0)("PSTLZ_ZL").ToString
                LeasingnehmerOrt = .Rows(0)("ORT01_ZL").ToString

                If RefreshLN = False Then
                    'Halter-Properties füllen
                    Halter = .Rows(0)("NAME1_SH").ToString
                    HalterStrasse = .Rows(0)("STRAS_SH").ToString
                    HalterPLZ = .Rows(0)("PSTLZ_SH").ToString
                    HalterOrt = .Rows(0)("ORT01_SH").ToString

                    Wunschkennzeichen1 = .Rows(0)("DW_KENNZ").ToString

                    'VersicherungsProperties füllen
                    VersGesellschaft = .Rows(0)("ZZVERSICHERUNG").ToString
                    EVBNummer = .Rows(0)("DAUER_EVB").ToString



                    'fill Dienstleistungsdetails JJU20081103
                    Servicekarte = .Rows(0)("SVCKART").ToString
                    Tankkarten = .Rows(0)("TANKKARTE").ToString
                    Verbandskasten = .Rows(0)("ZVERBANDSKASTEN").ToString
                    Warnweste = .Rows(0)("ZWARNWESTE").ToString
                    Warndreieck = .Rows(0)("ZWARNDREIECK").ToString
                    Fussmatten = .Rows(0)("ZFUSSMATTEN").ToString
                    FKontrolle = .Rows(0)("ZKONTROLLEFS").ToString

                    'achtung alte felder hier benutzen
                    '------------------------
                    HandyAdapter = .Rows(0)("SV_HANDY_ADAPTER").ToString
                    NaviCD = .Rows(0)("SV_NAVICD").ToString
                    '------------------------

                    'keine Ahnung woher?!
                    'Waesche=.
                    'Tanken = .
                    'FzgEinweisung=.





                End If


            End With
        Else
            Throw New Exception("Es wurden keine Daten für die übergebene Nummer gefunden.")
        End If

    End Sub

    '----------------------------------------------------------------------
    ' Methode:      Adresssuche
    ' Autor:        SFa
    ' Beschreibung: Tabelle mit Adressdaten füllen
    ' Erstellt am:  04.09.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Public Sub Adresssuche(ByVal Kennung As String, _
                               ByVal Name As String, _
                               ByVal PLZ As String, _
                               ByVal Ort As String)

        Dim AdressData As New UeberfDADTables(m_objUser, m_objApp, "")


        Adressen = AdressData.GetAdresse(Kennung, Name, PLZ, Ort)


    End Sub

    '----------------------------------------------------------------------
    ' Methode:      GetLaender
    ' Autor:        SFa
    ' Beschreibung: Holen der Ländercodes
    ' Erstellt am:  19.09.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Public Sub GetLaender()
        Dim LaenderData As New UeberfDADTables(m_objUser, m_objApp, "")

        Laender = LaenderData.GetLaender
    End Sub

    '----------------------------------------------------------------------
    ' Methode:      Save
    ' Autor:        SFa
    ' Beschreibung: Speichervorgang auslösen
    ' Erstellt am:  19.09.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Public Sub Save()
        Dim SaveData As New UeberfDADTables(m_objUser, m_objApp, "")

        SaveData.Save(Me)

    End Sub

    '----------------------------------------------------------------------
    ' Methode:      CheckAdresse
    ' Autor:        SFa
    ' Beschreibung: Abholadressprüfung auslösen
    ' Erstellt am:  19.09.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Public Sub CheckAdresse(ByVal Strasse As String, ByVal PLZ As String, ByVal Ort As String)
        Dim Adresse As New UeberfDADTables(m_objUser, m_objApp, "")

        CheckAdressen = Adresse.GeoAdressen(Strasse, PLZ, Ort)

    End Sub

    '----------------------------------------------------------------------
    ' Methode:      GetAutoland
    ' Autor:        SFa
    ' Beschreibung: Holen der Autolandadresse anstoßen
    ' Erstellt am:  19.09.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Public Function GetAutoland(ByVal GeoX As String, ByVal GeoY As String, ByVal GeoAdresse As String) As DataTable
        Dim Adresse As New UeberfDADTables(m_objUser, m_objApp, "")
        Dim TempAdressen As DataTable

        TempAdressen = Adresse.GeoAutoland(GeoX, GeoY, GeoAdresse)

        Return TempAdressen
    End Function

    '----------------------------------------------------------------------
    ' Methode:      GetEquiDaten
    ' Autor:        SFa
    ' Beschreibung: Holen der Equidaten
    ' Erstellt am:  26.09.2008
    ' ITA:          2258
    '----------------------------------------------------------------------
    Public Function GetEquiDaten(ByVal Vertragsnummer As String) As DataTable
        Dim GetEqui As New UeberfDADTables(m_objUser, m_objApp, "")
        Dim TempAdressen As DataTable

        TempAdressen = GetEqui.GetEquiData(Vertragsnummer)

        Return TempAdressen
    End Function


#End Region

End Class
' ************************************************
' $History: UeberfDAD.vb $
' 
' *****************  Version 15  *****************
' User: Fassbenders  Date: 7.12.10    Time: 11:32
' Updated in $/CKAG/Applications/AppUeberf/Lib
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 3.11.08    Time: 11:15
' Updated in $/CKAG/Applications/AppUeberf/Lib
' ITA 2343 fertigstellung 
' 
' ************************************************