Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Common
Imports CKG.Base.Business

Public Class UeberfgStandard_01
    Inherits Base.Business.DatenimportBase


    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    'Je nach Beauftragungsart sollen im Frontend entsprechende Funktionalitäten gegeben sein.
    Public Enum Beauftragungsart
        ReineUeberfuehrung = 1
        ZulassungUndUeberfuehrung = 2
        OffeneUeberfuehrung = 3
        UeberfuehrungKCL = 4
        ZulassungKCL = 5
        ZulassungUndUeberfuehrungKCL = 6
    End Enum

    Public Enum AdressStatus
        Frei
        Gesperrt
    End Enum

#Region "Vars"
    Public AdressStatusAbholung As AdressStatus
    Public AdressStatusAnlieferung As AdressStatus
    Public AdressStatusRuecklieferung As AdressStatus
    Private strZP0, strZP1, strZP2, strZP3, strZP4, strZP5, strZP6, strZP7, strZP8, strZP9 As String
    Private strZ0, strZ1, strZ2, strZ3, strZ4, strZ5, strZ6, strZ7, strZ8, strZ9 As String

    Private mKunnrNL As String

    Private booHoldData As Boolean

    Private strKundeName1 As String
    Private strKundeStrasse As String
    Private strKundeOrt As String
    Private strKundeAnsprechpartner As String
    Private strKundeTelefon As String

    Private strRegName As String
    Private strRegStrasse As String
    Private strRegOrt As String
    Private strRechName As String
    Private strRechStrasse As String
    Private strRechOrt As String

    Private strAbName As String
    Private strAbStrasse As String
    Private strAbNr As String
    Private strAbPlz As String
    Private strAbOrt As String
    Private strAbTelefon As String
    Private strAbTelefon2 As String
    Private strAbFax As String
    Private strAbAnsprechpartner As String
    Private strAnName As String
    Private strAnStrasse As String
    Private strAnNr As String
    Private strAnPlz As String
    Private strAnOrt As String
    Private strAnTelefon As String
    Private strAnTelefon2 As String
    Private strAnFax As String
    Private strAnAnsprechpartner As String
    Private strHerst As String
    Private strKenn1 As String
    Private strKenn2 As String
    Private strWunschKennzeichen1 As String
    Private strWunschKennzeichen2 As String
    Private strWunschKennzeichen3 As String
    Private strExpress As String
    Private strFzgZugelassen As String
    Private strHin_KCL_zulassen As String
    Private strAn_KCL_zulassen As String
    Private strSomWin As String
    Private strVin As String
    Private strRef As String
    Private strFahrzeugklasse As String
    Private strFahrzeugklasseTxt As String
    Private booAnschluss As Boolean
    Private strDatumUeberf As String
    Private strZulHaltername As String
    Private strZulPLZ As String
    Private booTanken As Boolean
    Private booWaesche As Boolean
    Private booFzgEinweisung As Boolean
    Private booRotKenn As Boolean
    Private strBemerkung As String
    Private strReName As String
    Private strReStrasse As String
    Private strReNr As String
    Private strRePlz As String
    Private strReOrt As String
    Private strReTelefon As String
    Private strReTelefon2 As String
    Private strReFax As String
    Private strReAnsprechpartner As String
    Private strReHerst As String
    Private strReKenn1 As String
    Private strReKenn2 As String
    Private strReFzgZugelassen As String
    Private strReFahrzeugklasse As String
    Private strReFahrzeugklasseTxt As String
    Private strReSomWin As String
    Private strReVin As String
    Private strReRef As String
    Private strSelAbholung As String
    Private strSelAnlieferung As String
    Private strSelRegulierer As String
    Private strSelRechnungsempf As String
    Private strSelRetour As String
    '    Private strSelFahrzeugwert As String
    Private strVbeln As String
    Private strFahrzeugwert As String
    Private strFahrzeugwertTxt As String
    Private lngBeauftragungsart As Integer
    Private booFahrzeugVorhanden As Boolean
    Private strEqui As String
    Private strQmnum As String
    Private strVbeln1510 As String
    Private strVkorg As String
    Private nModus As Int16
    Private strAuftragsnummer As String
    Private strVBName1 As String
    Private strVBName2 As String
    Private strVBStrasse As String
    Private strVBPlzOrt As String
    Private strVBTel As String
    Private strVBMail As String
    Private strVB3100 As String

    Private strFooVBName1 As String
    Private strFooVBName2 As String
    Private strFooVBStrasse As String
    Private strFooVBPlzOrt As String
    Private strFooVBTel As String
    Private strFooVBFax As String
    Private strFooVBMail As String

    Private strZulassungsdatum As String
    Private strZulAuftrag As String
    Private strUnterlagenTxt As String
    Private booShow As Boolean
    Private strStVa As String
    Private strZulassungsdienstTxt As String

    Public strClassTrace As String
    Public tblKreis As DataTable
    Public tblStdTexte As DataTable
    Public tblUpload As DataTable
    Public tblUeberf As DataTable
    Public tblPartner As DataTable
    Public tblUeberfSel As DataTable
    Public tblPartnerSel As DataTable
    Public AufIDSel As String
    Public statusauftr As String
    Public selectedUser As String
    Public boonewdataset As Boolean = False

#End Region

#Region "Properties"

    Public Property KunnrNL() As String
        Get
            Return mKunnrNL
        End Get
        Set(ByVal Value As String)
            mKunnrNL = Value
        End Set
    End Property

    Public Property NewDataSet() As Boolean
        Get
            Return boonewdataset
        End Get
        Set(ByVal Value As Boolean)
            boonewdataset = Value
        End Set
    End Property

    Public Property HoldData() As Boolean
        Get
            Return booHoldData
        End Get
        Set(ByVal Value As Boolean)
            booHoldData = Value
        End Set
    End Property

    Public Property SelAufID() As String
        Get
            Return AufIDSel
        End Get
        Set(ByVal Value As String)
            AufIDSel = Value
        End Set
    End Property

    Public Property AuftragsStatus() As String
        Get
            Return statusauftr
        End Get
        Set(ByVal Value As String)
            statusauftr = Value
        End Set
    End Property

    Public Property SelUser() As String
        Get
            Return selectedUser
        End Get
        Set(ByVal Value As String)
            selectedUser = Value
        End Set
    End Property

    Public Property TabUeberfSel() As DataTable
        Get
            Return tblUeberfSel
        End Get
        Set(ByVal Value As DataTable)
            tblUeberfSel = Value
        End Set
    End Property

    Public Property TabPartnerSel() As DataTable
        Get
            Return tblPartnerSel
        End Get
        Set(ByVal Value As DataTable)
            tblPartnerSel = Value
        End Set
    End Property

    Public Property TabUeberf() As DataTable
        Get
            Return tblUeberf
        End Get
        Set(ByVal Value As DataTable)
            tblUeberf = Value
        End Set
    End Property

    Public Property TabUpload() As DataTable
        Get
            Return tblUpload
        End Get
        Set(ByVal Value As DataTable)
            tblUpload = Value
        End Set
    End Property

    Public Property TabPartner() As DataTable
        Get
            Return tblPartner
        End Get
        Set(ByVal Value As DataTable)
            tblPartner = Value
        End Set
    End Property

    Public Property Beauftragung() As Integer
        Get
            Return lngBeauftragungsart
        End Get
        Set(ByVal Value As Integer)
            lngBeauftragungsart = Value
        End Set
    End Property

    Public Property Equipment() As String
        Get
            Return strEqui
        End Get
        Set(ByVal Value As String)
            strEqui = Value
        End Set
    End Property

    Public Property Qmnum() As String
        Get
            Return strQmnum
        End Get
        Set(ByVal Value As String)
            strQmnum = Value
        End Set
    End Property

    Public Property Vbeln1510() As String
        Get
            Return strVbeln1510
        End Get
        Set(ByVal Value As String)
            strVbeln1510 = Value
        End Set
    End Property

    Public Property KundeName() As String
        Get
            Return strKundeName1
        End Get
        Set(ByVal Value As String)
            strKundeName1 = Value
        End Set
    End Property

    Public Property KundeStrasse() As String
        Get
            Return strKundeStrasse
        End Get
        Set(ByVal Value As String)
            strKundeStrasse = Value
        End Set
    End Property

    Public Property KundeOrt() As String
        Get
            Return strKundeOrt
        End Get
        Set(ByVal Value As String)
            strKundeOrt = Value
        End Set
    End Property

    Public Property KundeAnsprechpartner() As String
        Get
            Return strKundeAnsprechpartner
        End Get
        Set(ByVal Value As String)
            strKundeAnsprechpartner = Value
        End Set
    End Property

    Public Property KundeTelefon() As String
        Get
            Return strKundeTelefon
        End Get
        Set(ByVal Value As String)
            strKundeTelefon = Value
        End Set
    End Property

    Public Property RegName() As String
        Get
            Return strRegName
        End Get
        Set(ByVal Value As String)
            strRegName = Value
        End Set
    End Property

    Public Property RegStrasse() As String
        Get
            Return strRegStrasse
        End Get
        Set(ByVal Value As String)
            strRegStrasse = Value
        End Set
    End Property

    Public Property RegOrt() As String
        Get
            Return strRegOrt
        End Get
        Set(ByVal Value As String)
            strRegOrt = Value
        End Set
    End Property

    Public Property RechName() As String
        Get
            Return strRechName
        End Get
        Set(ByVal Value As String)
            strRechName = Value
        End Set
    End Property

    Public Property RechStrasse() As String
        Get
            Return strRechStrasse
        End Get
        Set(ByVal Value As String)
            strRechStrasse = Value
        End Set
    End Property

    Public Property RechOrt() As String
        Get
            Return strRechOrt
        End Get
        Set(ByVal Value As String)
            strRechOrt = Value
        End Set
    End Property

    Public Property AbName() As String
        Get
            Return strAbName
        End Get
        Set(ByVal Value As String)
            strAbName = Value
        End Set
    End Property

    Public Property AbStrasse() As String
        Get
            Return strAbStrasse
        End Get
        Set(ByVal Value As String)
            strAbStrasse = Value
        End Set
    End Property

    Public Property AbNr() As String
        Get
            Return strAbNr
        End Get
        Set(ByVal Value As String)
            strAbNr = Value
        End Set
    End Property

    Public Property AbPlz() As String
        Get
            Return strAbPlz
        End Get
        Set(ByVal Value As String)
            strAbPlz = Value
        End Set
    End Property

    Public Property AbOrt() As String
        Get
            Return strAbOrt
        End Get
        Set(ByVal Value As String)
            strAbOrt = Value
        End Set
    End Property

    Public Property AbTelefon() As String
        Get
            Return strAbTelefon
        End Get
        Set(ByVal Value As String)
            strAbTelefon = Value
        End Set
    End Property

    Public Property AbTelefon2() As String
        Get
            Return strAbTelefon2
        End Get
        Set(ByVal Value As String)
            strAbTelefon2 = Value
        End Set
    End Property

    Public Property AbFax() As String
        Get
            Return strAbFax
        End Get
        Set(ByVal Value As String)
            strAbFax = Value
        End Set
    End Property

    Public Property AbAnsprechpartner() As String
        Get
            Return strAbAnsprechpartner
        End Get
        Set(ByVal Value As String)
            strAbAnsprechpartner = Value
        End Set
    End Property

    Public Property SelAbholung() As String
        Get
            Return strSelAbholung
        End Get
        Set(ByVal Value As String)
            strSelAbholung = Value
        End Set
    End Property

    Public Property SelRegulierer() As String
        Get
            Return strSelRegulierer
        End Get
        Set(ByVal Value As String)
            strSelRegulierer = Value
        End Set
    End Property

    Public Property SelRechnungsempf() As String
        Get
            Return strSelRechnungsempf
        End Get
        Set(ByVal Value As String)
            strSelRechnungsempf = Value
        End Set
    End Property

    Public Property SelFahrzeugwert() As String
        Get
            Return strFahrzeugwert
        End Get
        Set(ByVal Value As String)
            strFahrzeugwert = Value
        End Set
    End Property

    Public Property AnName() As String
        Get
            Return strAnName
        End Get
        Set(ByVal Value As String)
            strAnName = Value
        End Set
    End Property

    Public Property AnStrasse() As String
        Get
            Return strAnStrasse
        End Get
        Set(ByVal Value As String)
            strAnStrasse = Value
        End Set
    End Property

    Public Property AnNr() As String
        Get
            Return strAnNr
        End Get
        Set(ByVal Value As String)
            strAnNr = Value
        End Set
    End Property

    Public Property AnPlz() As String
        Get
            Return strAnPlz
        End Get
        Set(ByVal Value As String)
            strAnPlz = Value
        End Set
    End Property

    Public Property AnOrt() As String
        Get
            Return strAnOrt
        End Get
        Set(ByVal Value As String)
            strAnOrt = Value
        End Set
    End Property

    Public Property AnTelefon() As String
        Get
            Return strAnTelefon
        End Get
        Set(ByVal Value As String)
            strAnTelefon = Value
        End Set
    End Property

    Public Property AnTelefon2() As String
        Get
            Return strAnTelefon2
        End Get
        Set(ByVal Value As String)
            strAnTelefon2 = Value
        End Set
    End Property

    Public Property AnFax() As String
        Get
            Return strAnFax
        End Get
        Set(ByVal Value As String)
            strAnFax = Value
        End Set
    End Property

    Public Property AnAnsprechpartner() As String
        Get
            Return strAnAnsprechpartner
        End Get
        Set(ByVal Value As String)
            strAnAnsprechpartner = Value
        End Set
    End Property

    Public Property SelAnlieferung() As String
        Get
            Return strSelAnlieferung
        End Get
        Set(ByVal Value As String)
            strSelAnlieferung = Value
        End Set
    End Property

    Public Property Herst() As String
        Get
            Return strHerst
        End Get
        Set(ByVal Value As String)
            strHerst = Value
        End Set
    End Property

    Public Property Kenn1() As String
        Get
            Return strKenn1
        End Get
        Set(ByVal Value As String)
            strKenn1 = Value
        End Set
    End Property

    Public Property Kenn2() As String
        Get
            Return strKenn2
        End Get
        Set(ByVal Value As String)
            strKenn2 = Value
        End Set
    End Property

    Public Property Wunschkennzeichen1() As String
        Get
            Return strWunschKennzeichen1
        End Get
        Set(ByVal Value As String)
            strWunschKennzeichen1 = Value
        End Set
    End Property

    Public Property Wunschkennzeichen2() As String
        Get
            Return strWunschKennzeichen2
        End Get
        Set(ByVal Value As String)
            strWunschKennzeichen2 = Value
        End Set
    End Property

    Public Property Wunschkennzeichen3() As String
        Get
            Return strWunschKennzeichen3
        End Get
        Set(ByVal Value As String)
            strWunschKennzeichen3 = Value
        End Set
    End Property

    Public Property FzgZugelassen() As String
        Get
            Return strFzgZugelassen
        End Get
        Set(ByVal Value As String)
            strFzgZugelassen = Value
        End Set
    End Property

    Public Property Hin_KCL_Zulassen() As String
        Get
            Return strHin_KCL_zulassen
        End Get
        Set(ByVal Value As String)
            strHin_KCL_zulassen = Value
        End Set
    End Property

    Public Property An_KCL_Zulassen() As String
        Get
            Return strAn_KCL_zulassen
        End Get
        Set(ByVal Value As String)
            strAn_KCL_zulassen = Value
        End Set
    End Property

    Public Property SomWin() As String
        Get
            Return strSomWin
        End Get
        Set(ByVal Value As String)
            strSomWin = Value
        End Set
    End Property

    Public Property Vin() As String
        Get
            Return strVin
        End Get
        Set(ByVal Value As String)
            strVin = Value
        End Set
    End Property

    Public Property Ref() As String
        Get
            Return strRef
        End Get
        Set(ByVal Value As String)
            strRef = Value
        End Set
    End Property

    Public Property Fahrzeugklasse() As String
        Get
            Return strFahrzeugklasse
        End Get
        Set(ByVal Value As String)
            strFahrzeugklasse = Value
        End Set
    End Property

    Public Property FahrzeugklasseTxt() As String
        Get
            Return strFahrzeugklasseTxt
        End Get
        Set(ByVal Value As String)
            strFahrzeugklasseTxt = Value
        End Set
    End Property

    Public Property ReFahrzeugklasse() As String
        Get
            Return strReFahrzeugklasse
        End Get
        Set(ByVal Value As String)
            strReFahrzeugklasse = Value
        End Set
    End Property

    Public Property ReFahrzeugklasseTxt() As String
        Get
            Return strReFahrzeugklasseTxt
        End Get
        Set(ByVal Value As String)
            strReFahrzeugklasseTxt = Value
        End Set
    End Property

    Public Property Express() As String
        Get
            Return strExpress
        End Get
        Set(ByVal Value As String)
            strExpress = Value
        End Set
    End Property

    Public Property Anschluss() As Boolean
        Get
            Return booAnschluss
        End Get
        Set(ByVal Value As Boolean)
            booAnschluss = Value
        End Set
    End Property

    Public Property ShowTables() As Boolean
        Get
            Return booShow
        End Get
        Set(ByVal Value As Boolean)
            booShow = Value
        End Set
    End Property

    Public Property StVa() As String
        Get
            Return strStVa
        End Get
        Set(ByVal Value As String)
            strStVa = Value
        End Set
    End Property

    Public Property ZulassungsdienstTxt() As String
        Get
            Return strZulassungsdienstTxt
        End Get
        Set(ByVal Value As String)
            strZulassungsdienstTxt = Value
        End Set
    End Property

    Public Property DatumUeberf() As String
        Get
            Return strDatumUeberf
        End Get
        Set(ByVal Value As String)
            strDatumUeberf = Value
        End Set
    End Property

    Public Property Tanken() As Boolean
        Get
            Return booTanken
        End Get
        Set(ByVal Value As Boolean)
            booTanken = Value
        End Set
    End Property

    Public Property Waesche() As Boolean
        Get
            Return booWaesche
        End Get
        Set(ByVal Value As Boolean)
            booWaesche = Value
        End Set
    End Property

    Public Property FzgEinweisung() As Boolean
        Get
            Return booFzgEinweisung
        End Get
        Set(ByVal Value As Boolean)
            booFzgEinweisung = Value
        End Set
    End Property

    Public Property RotKenn() As Boolean
        Get
            Return booRotKenn
        End Get
        Set(ByVal Value As Boolean)
            booRotKenn = Value
        End Set
    End Property

    Public Property Bemerkung() As String
        Get
            Return strBemerkung
        End Get
        Set(ByVal Value As String)
            strBemerkung = Value
        End Set
    End Property

    Public Property ZulHaltername() As String
        Get
            Return strZulHaltername
        End Get
        Set(ByVal Value As String)
            strZulHaltername = Value
        End Set
    End Property

    Public Property ZulPLZ() As String
        Get
            Return strZulPLZ
        End Get
        Set(ByVal Value As String)
            strZulPLZ = Value
        End Set
    End Property

    Public Property ReName() As String
        Get
            Return strReName
        End Get
        Set(ByVal Value As String)
            strReName = Value
        End Set
    End Property

    Public Property ReStrasse() As String
        Get
            Return strReStrasse
        End Get
        Set(ByVal Value As String)
            strReStrasse = Value
        End Set
    End Property

    Public Property ReNr() As String
        Get
            Return strReNr
        End Get
        Set(ByVal Value As String)
            strReNr = Value
        End Set
    End Property

    Public Property RePlz() As String
        Get
            Return strRePlz
        End Get
        Set(ByVal Value As String)
            strRePlz = Value
        End Set
    End Property

    Public Property ReOrt() As String
        Get
            Return strReOrt
        End Get
        Set(ByVal Value As String)
            strReOrt = Value
        End Set
    End Property

    Public Property ReTelefon() As String
        Get
            Return strReTelefon
        End Get
        Set(ByVal Value As String)
            strReTelefon = Value
        End Set
    End Property

    Public Property ReTelefon2() As String
        Get
            Return strReTelefon2
        End Get
        Set(ByVal Value As String)
            strReTelefon2 = Value
        End Set
    End Property

    Public Property ReFax() As String
        Get
            Return strReFax
        End Get
        Set(ByVal Value As String)
            strReFax = Value
        End Set
    End Property

    Public Property ReAnsprechpartner() As String
        Get
            Return strReAnsprechpartner
        End Get
        Set(ByVal Value As String)
            strReAnsprechpartner = Value
        End Set
    End Property

    Public Property SelRetour() As String
        Get
            Return strSelRetour
        End Get
        Set(ByVal Value As String)
            strSelRetour = Value
        End Set
    End Property

    Public Property ReHerst() As String
        Get
            Return strReHerst
        End Get
        Set(ByVal Value As String)
            strReHerst = Value
        End Set
    End Property

    Public Property ReKenn1() As String
        Get
            Return strReKenn1
        End Get
        Set(ByVal Value As String)
            strReKenn1 = Value
        End Set
    End Property

    Public Property ReKenn2() As String
        Get
            Return strReKenn2
        End Get
        Set(ByVal Value As String)
            strReKenn2 = Value
        End Set
    End Property

    Public Property ReFzgZugelassen() As String
        Get
            Return strReFzgZugelassen
        End Get
        Set(ByVal Value As String)
            strReFzgZugelassen = Value
        End Set
    End Property

    Public Property ReSomWin() As String
        Get
            Return strReSomWin
        End Get
        Set(ByVal Value As String)
            strReSomWin = Value
        End Set
    End Property

    Public Property ReVin() As String
        Get
            Return strReVin
        End Get
        Set(ByVal Value As String)
            strReVin = Value
        End Set
    End Property

    Public Property ReRef() As String
        Get
            Return strReRef
        End Get
        Set(ByVal Value As String)
            strReRef = Value
        End Set
    End Property

    Public Property Vbeln() As String
        Get
            Return strVbeln
        End Get
        Set(ByVal Value As String)
            strVbeln = Value
        End Set
    End Property

    Public Property Modus() As Int16
        Get
            Return nModus
        End Get
        Set(ByVal Value As Int16)
            nModus = Value
        End Set
    End Property

    Public Property FahrzeugwertTxt() As String
        Get
            Return strFahrzeugwertTxt
        End Get
        Set(ByVal Value As String)
            strFahrzeugwertTxt = Value
        End Set
    End Property

    Public Property FahrzeugVorhanden() As Boolean
        Get
            Return booFahrzeugVorhanden
        End Get
        Set(ByVal Value As Boolean)
            booFahrzeugVorhanden = Value
        End Set
    End Property

    Public Property Auftragsnummer() As String
        Get
            Return strAuftragsnummer
        End Get
        Set(ByVal Value As String)
            strAuftragsnummer = Value
        End Set
    End Property

    Public Property VBName1() As String
        Get
            Return strVBName1
        End Get
        Set(ByVal Value As String)
            strVBName1 = Value
        End Set
    End Property

    Public Property VBName2() As String
        Get
            Return strVBName2
        End Get
        Set(ByVal Value As String)
            strVBName2 = Value
        End Set
    End Property

    Public Property VBStrasse() As String
        Get
            Return strVBStrasse
        End Get
        Set(ByVal Value As String)
            strVBStrasse = Value
        End Set
    End Property

    Public Property VBPlzOrt() As String
        Get
            Return strVBPlzOrt
        End Get
        Set(ByVal Value As String)
            strVBPlzOrt = Value
        End Set
    End Property

    Public Property VBTel() As String
        Get
            Return strVBTel
        End Get
        Set(ByVal Value As String)
            strVBTel = Value
        End Set
    End Property

    Public Property VBMail() As String
        Get
            Return strVBMail
        End Get
        Set(ByVal Value As String)
            strVBMail = Value
        End Set
    End Property

    Public Property VB3100() As String
        Get
            Return strVB3100
        End Get
        Set(ByVal Value As String)
            strVB3100 = Value
        End Set
    End Property

    Public Property FooVBName1() As String
        Get
            Return strFooVBName1
        End Get
        Set(ByVal Value As String)
            strFooVBName1 = Value
        End Set
    End Property

    Public Property FooVBName2() As String
        Get
            Return strFooVBName2
        End Get
        Set(ByVal Value As String)
            strFooVBName2 = Value
        End Set
    End Property

    Public Property FooVBStrasse() As String
        Get
            Return strFooVBStrasse
        End Get
        Set(ByVal Value As String)
            strFooVBStrasse = Value
        End Set
    End Property

    Public Property FooVBPlzOrt() As String
        Get
            Return strFooVBPlzOrt
        End Get
        Set(ByVal Value As String)
            strFooVBPlzOrt = Value
        End Set
    End Property

    Public Property FooVBTel() As String
        Get
            Return strFooVBTel
        End Get
        Set(ByVal Value As String)
            strFooVBTel = Value
        End Set
    End Property

    Public Property FooVBFax() As String
        Get
            Return strFooVBFax
        End Get
        Set(ByVal Value As String)
            strFooVBFax = Value
        End Set
    End Property

    Public Property FooVBMail() As String
        Get
            Return strFooVBMail
        End Get
        Set(ByVal Value As String)
            strFooVBMail = Value
        End Set
    End Property

    Public Property UnterlagenTxt() As String
        Get
            Return strUnterlagenTxt
        End Get
        Set(ByVal Value As String)
            strUnterlagenTxt = Value
        End Set
    End Property

    Public Property Zulassungsdatum() As String
        Get
            Return strZulassungsdatum
        End Get
        Set(ByVal value As String)
            strZulassungsdatum = value
        End Set
    End Property

    Public Property ZulAuftrag() As String
        Get
            Return strZulAuftrag
        End Get
        Set(ByVal value As String)
            strZulAuftrag = value
        End Set
    End Property

    Public Property ZP0() As String
        Get
            Return strZP0
        End Get
        Set(ByVal Value As String)
            strZP0 = Value
        End Set
    End Property

    Public Property ZP1() As String
        Get
            Return strZP1
        End Get
        Set(ByVal Value As String)
            strZP1 = Value
        End Set
    End Property

    Public Property ZP2() As String
        Get
            Return strZP2
        End Get
        Set(ByVal Value As String)
            strZP2 = Value
        End Set
    End Property

    Public Property ZP3() As String
        Get
            Return strZP3
        End Get
        Set(ByVal Value As String)
            strZP3 = Value
        End Set
    End Property

    Public Property ZP4() As String
        Get
            Return strZP4
        End Get
        Set(ByVal Value As String)
            strZP4 = Value
        End Set
    End Property

    Public Property ZP5() As String
        Get
            Return strZP5
        End Get
        Set(ByVal Value As String)
            strZP5 = Value
        End Set
    End Property

    Public Property ZP6() As String
        Get
            Return strZP6
        End Get
        Set(ByVal Value As String)
            strZP6 = Value
        End Set
    End Property

    Public Property ZP7() As String
        Get
            Return strZP7
        End Get
        Set(ByVal Value As String)
            strZP7 = Value
        End Set
    End Property

    Public Property ZP8() As String
        Get
            Return strZP8
        End Get
        Set(ByVal Value As String)
            strZP8 = Value
        End Set
    End Property

    Public Property ZP9() As String
        Get
            Return strZP9
        End Get
        Set(ByVal Value As String)
            strZP9 = Value
        End Set
    End Property

    Public Property Z0() As String
        Get
            Return strZ0
        End Get
        Set(ByVal Value As String)
            strZ0 = Value
        End Set
    End Property

    Public Property Z1() As String
        Get
            Return strZ1
        End Get
        Set(ByVal Value As String)
            strZ1 = Value
        End Set
    End Property

    Public Property Z2() As String
        Get
            Return strZ2
        End Get
        Set(ByVal Value As String)
            strZ2 = Value
        End Set
    End Property

    Public Property Z3() As String
        Get
            Return strZ3
        End Get
        Set(ByVal Value As String)
            strZ3 = Value
        End Set
    End Property

    Public Property Z4() As String
        Get
            Return strZ4
        End Get
        Set(ByVal Value As String)
            strZ4 = Value
        End Set
    End Property

    Public Property Z5() As String
        Get
            Return strZ5
        End Get
        Set(ByVal Value As String)
            strZ5 = Value
        End Set
    End Property

    Public Property Z6() As String
        Get
            Return strZ6
        End Get
        Set(ByVal Value As String)
            strZ6 = Value
        End Set
    End Property

    Public Property Z7() As String
        Get
            Return strZ7
        End Get
        Set(ByVal Value As String)
            strZ7 = Value
        End Set
    End Property

    Public Property Z8() As String
        Get
            Return strZ8
        End Get
        Set(ByVal Value As String)
            strZ8 = Value
        End Set
    End Property

    Public Property Z9() As String
        Get
            Return strZ9
        End Get
        Set(ByVal Value As String)
            strZ9 = Value
        End Set
    End Property


#End Region

#Region "Methods"

    Public Function Save() As DataTable

        'Dim SAPTableRet As New SAPProxy_Ueberf.BAPIRET2Table()
        'Dim objSAP As New SAPProxy_Ueberf.SAPProxy_Ueberf()
        'Dim intID As Int32 = -1

        'Dim SAPTable As New SAPProxy_Ueberf.BAPIPARTNRTable()
        'Dim SAPTableRow As SAPProxy_Ueberf.BAPIPARTNR

        Dim SAPTable As DataTable
        Dim SAPTableRow As DataRow
        Dim SAPTableRet As DataTable = Nothing

        Dim strEinweisung As String = ""
        Dim strTanken As String = ""
        Dim strZugelassen As String = ""
        Dim strWaesche As String = ""
        Dim strMatnr As String = ""
        Dim strKennzeichen As String = ""
        Dim strAbKunnr As String = ""
        Dim strAnKunnr As String = ""
        Dim strRegKunnr As String = ""
        Dim strRechKunnr As String = ""
        Dim strReKCLZul As String = ""
        Dim strHinKCLZul As String = ""
        Dim strSoWi As String = ""
        Dim strReSoWi As String = ""
        Dim strReZugelassen As String = ""
        Dim strReKennzeichen As String = ""
        Dim strKunnr As String = ""
        Dim strDatUeberf As String = ""
        Dim strRotKenn As String = ""
        Dim strExpressX As String = ""

        Try
            ' Parameter füllen
            ' ------------------------------------------------------
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Order_Ueberfuehrung_Create", m_objApp, m_objUser, page)

            If KunnrNL IsNot Nothing Then
                strKunnr = KunnrNL.PadLeft(10, "0"c)
            Else
                strKunnr = m_objUser.KUNNR.PadLeft(10, "0"c)
            End If

            strKennzeichen = Kenn1 & "-" & Kenn2

            If Beauftragung = Beauftragungsart.ReineUeberfuehrung Then
                strVkorg = "1510"
            Else
                strVkorg = "1510"
            End If

            If booFzgEinweisung = True Then
                strEinweisung = "x"
            End If

            If booTanken = True Then
                strTanken = "x"
            End If

            If booWaesche = True Then
                strWaesche = "x"
            End If

            If booRotKenn = True Then
                strRotKenn = "x"
            End If

            If FzgZugelassen = "Nein" Then
                strZugelassen = "x"
            End If

            If SomWin = "Winter" Then
                strSoWi = "X"
            ElseIf SomWin = "Ganzjahresreifen" Then
                strSoWi = "G"
            End If

            If Anschluss = True Then
                strMatnr = "000000000000002340"
            Else
                strMatnr = "000000000000001911"
            End If

            If DatumUeberf <> "" Then
                strDatUeberf = DatumUeberf
            End If

            If ReSomWin = "Winter" Then
                strReSoWi = "X"
            ElseIf ReSomWin = "Ganzjahresreifen" Then
                strReSoWi = "G"
            End If

            If ReFzgZugelassen = "Nein" Then
                strReZugelassen = "x"
            End If

            If Hin_KCL_Zulassen = "Ja" Then
                strHinKCLZul = "X"
            End If

            strReKCLZul = ""

            If Anschluss = True Then
                strReKennzeichen = ReKenn1 & "-" & ReKenn2
            End If

            If SelRegulierer = "" Then
                strRegKunnr = strKunnr
            Else
                strRegKunnr = SelRegulierer
            End If

            If SelRechnungsempf = "" Then
                strRechKunnr = strKunnr
            Else
                strRechKunnr = SelRechnungsempf
            End If


            If SelAbholung = "" Then
                strAbKunnr = strKunnr
            Else
                strAbKunnr = SelAbholung
            End If

            If SelAnlieferung = "" Then
                strAnKunnr = strKunnr
            Else
                strAnKunnr = SelAnlieferung
            End If

            If Beauftragung = Beauftragungsart.ZulassungUndUeberfuehrung Or Beauftragung = Beauftragungsart.ZulassungUndUeberfuehrungKCL Then
                ZulAuftrag = "x"
            End If

            If Express = "Ja" Then
                strExpressX = "x"
            End If

            ' ------------------------------------------------------


            ' Tabellen füllen
            ' ------------------------------------------------------

            SAPTable = S.AP.GetImportTableWithInit("Z_M_Order_Ueberfuehrung_Create.PARTNER") 'myProxy.getImportTable("PARTNER")

            'Auftraggeber

            SAPTableRow = SAPTable.NewRow()

            SAPTableRow("PARTN_ROLE") = "AG"
            SAPTableRow("Itm_Number") = "000000"
            SAPTableRow("Partn_Numb") = strKunnr

            SAPTable.Rows.Add(SAPTableRow)

            'Regulierer

            SAPTableRow = SAPTable.NewRow()

            SAPTableRow("PARTN_ROLE") = "RG"
            SAPTableRow("Itm_Number") = "000000"
            SAPTableRow("Partn_Numb") = strRegKunnr

            SAPTable.Rows.Add(SAPTableRow)

            'Warenempfänger

            SAPTableRow = SAPTable.NewRow()

            SAPTableRow("PARTN_ROLE") = "WE"
            SAPTableRow("Itm_Number") = "000000"
            SAPTableRow("Partn_Numb") = strAnKunnr
            SAPTableRow("Name") = AnName
            SAPTableRow("Name_2") = AnAnsprechpartner
            SAPTableRow("Street") = AnStrasse & " " & AnNr
            SAPTableRow("Postl_Code") = AnPlz
            SAPTableRow("City") = AnOrt
            SAPTableRow("Telephone") = AnTelefon
            SAPTableRow("Telephone2") = AnTelefon2

            SAPTable.Rows.Add(SAPTableRow)

            'Abholung

            SAPTableRow = SAPTable.NewRow()

            SAPTableRow("PARTN_ROLE") = "ZB"
            SAPTableRow("Itm_Number") = "000000"
            SAPTableRow("Partn_Numb") = strAbKunnr
            SAPTableRow("Name") = AbName
            SAPTableRow("Name_2") = AbAnsprechpartner
            SAPTableRow("Street") = AbStrasse & " " & AbNr
            SAPTableRow("Postl_Code") = AbPlz
            SAPTableRow("City") = AbOrt
            SAPTableRow("Telephone") = AbTelefon
            SAPTableRow("Telephone2") = AbTelefon2

            SAPTable.Rows.Add(SAPTableRow)

            If Anschluss = True Then
                'Retour

                SAPTableRow = SAPTable.NewRow()

                SAPTableRow("PARTN_ROLE") = "ZR"
                SAPTableRow("Itm_Number") = "000000"
                SAPTableRow("Partn_Numb") = strKunnr
                SAPTableRow("Name") = ReName
                SAPTableRow("Name_2") = ReAnsprechpartner
                SAPTableRow("Street") = ReStrasse & " " & ReNr
                SAPTableRow("Postl_Code") = RePlz
                SAPTableRow("City") = ReOrt
                SAPTableRow("Telephone") = ReTelefon

                SAPTable.Rows.Add(SAPTableRow)
            End If

            SAPTable.AcceptChanges()

            ' ------------------------------------------------------

            S.AP.SetImportParameter("KUNNR", strKunnr)
            S.AP.SetImportParameter("VKORG", strVkorg)
            S.AP.SetImportParameter("MATNR", strMatnr)
            S.AP.SetImportParameter("ZZBRIEF", Herst)
            S.AP.SetImportParameter("ZZKENN", strKennzeichen)
            S.AP.SetImportParameter("ZZREFNR", Ref)
            S.AP.SetImportParameter("ZZFAHRG", Vin)
            S.AP.SetImportParameter("VDATU", strDatUeberf)
            S.AP.SetImportParameter("ZULGE", strZugelassen)
            S.AP.SetImportParameter("WASCHEN", strWaesche)
            S.AP.SetImportParameter("TANKE", strTanken)
            S.AP.SetImportParameter("EINW", strEinweisung)
            S.AP.SetImportParameter("SOWIHIN", strSoWi)
            S.AP.SetImportParameter("ROTKENN", strRotKenn)
            S.AP.SetImportParameter("ZZBEZEI", ReHerst)
            S.AP.SetImportParameter("ZZKENNRUCK", strReKennzeichen)
            S.AP.SetImportParameter("ZZFAHRGRUCK", ReVin)
            S.AP.SetImportParameter("ZZREFNRRUCK", ReRef)
            S.AP.SetImportParameter("ZULGRUCK", strReZugelassen)
            S.AP.SetImportParameter("SOWIRUCK", strReSoWi)
            S.AP.SetImportParameter("BEMERKUNG", "")
            S.AP.SetImportParameter("AUGRU", SelFahrzeugwert)
            S.AP.SetImportParameter("EQUNR", Equipment)
            S.AP.SetImportParameter("I_VBELN", Auftragsnummer)
            S.AP.SetImportParameter("USER", m_objUser.UserName)
            S.AP.SetImportParameter("LFSPERR", ZulAuftrag)
            S.AP.SetImportParameter("BEMERKUNG02", Bemerkung)
            S.AP.SetImportParameter("BEMERKUNG03", "")
            S.AP.SetImportParameter("WINTER", "")
            S.AP.SetImportParameter("FIX", "")
            S.AP.SetImportParameter("UNBKRUCK", "")
            S.AP.SetImportParameter("ZZFAHRZGTYP", Herst)
            S.AP.SetImportParameter("KFZ_KL", Fahrzeugklasse)
            S.AP.SetImportParameter("KFZ_KLR", ReFahrzeugklasse)
            S.AP.SetImportParameter("EXPRESS_VERSAND", strExpressX) 'Express
            S.AP.SetImportParameter("HIN_ZUL_KCL", strHinKCLZul)
            S.AP.SetImportParameter("ANSCHL_ZUL_KCL", strReKCLZul)
            S.AP.SetImportParameter("I_WMENG", "")

            'myProxy.callBapi()
            S.AP.Execute()

            Vbeln = S.AP.GetExportParameter("VBELN")
            Vbeln1510 = S.AP.GetExportParameter("VBELN_1510")
            Qmnum = S.AP.GetExportParameter("QMNUM")

            'SAPTable = S.AP.GetExportTable("PARTNER")
            SAPTableRet = S.AP.GetExportTable("RETURN")

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message.Replace("Execution failed", "")
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case "NO_PARAMETER"
                    m_strMessage = "Eingabedaten nicht ausreichend."
                Case "NO_ASL"
                    m_strMessage = "Falsche Kundennr."
                Case "NO_LANGTEXT"
                    m_strMessage = ""
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        End Try

        Return SAPTableRet

    End Function

    Public Function SaveUploaded() As DataTable

        Dim SAPTable As DataTable
        Dim SAPTableRow As DataRow
        Dim SAPTableRet As DataTable = Nothing

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Order_Ueberfuehrung_Create", m_objApp, m_objUser, page)

            ' Parameter anpassen
            ' ---------------------------------------------------

            Dim strEinweisung As String = ""
            Dim strTanken As String = ""
            Dim strZugelassen As String = ""
            Dim strWaesche As String = ""
            Dim strMatnr As String = ""
            Dim strKennzeichen As String = Kenn1 & "-" & Kenn2
            Dim strAbKunnr As String = ""
            Dim strAnKunnr As String = ""
            Dim strRegKunnr As String = ""
            Dim strRechKunnr As String = ""
            Dim strReKCLZul As String = ""
            Dim strHinKCLZul As String = ""
            Dim strSoWi As String = ""
            Dim strReSoWi As String = ""
            Dim strReZugelassen As String = ""
            Dim strReKennzeichen As String = ""
            Dim strKunnr As String = Right("0000000000" & m_objUser.KUNNR, 10)
            Dim strDatUeberf As DateTime = Nothing
            Dim strRotKenn As String = ""

            If Beauftragung = Beauftragungsart.ReineUeberfuehrung Then
                strVkorg = "1510"
            Else
                strVkorg = "1010"
            End If

            If booFzgEinweisung = True Then
                strEinweisung = "x"
            End If

            If booTanken = True Then
                strTanken = "x"
            End If

            If booWaesche = True Then
                strWaesche = "x"
            End If

            If booRotKenn = True Then
                strRotKenn = "x"
            End If

            If Express = "Ja" Then
                strExpress = "x"
            Else
                strExpress = ""
            End If

            If FzgZugelassen = "Nein" Then
                strZugelassen = "x"
            End If
            If ReFzgZugelassen = "Nein" Then
                strReZugelassen = "x"
            End If

            If SomWin = "Winter" Then
                strSoWi = "X"
            ElseIf SomWin = "Ganzjahresreifen" Then
                strSoWi = "G"
            End If

            If ReSomWin = "Winter" Then
                strReSoWi = "X"
            ElseIf ReSomWin = "Ganzjahresreifen" Then
                strSoWi = "G"
            End If

            If Anschluss = True Then
                strMatnr = "000000000000002340"
            Else
                strMatnr = "000000000000001911"
            End If

            If DatumUeberf <> "" Then
                strDatUeberf = CDate(DatumUeberf)
            End If

            If ReSomWin = "Winter" Then
                strReSoWi = "X"
            ElseIf ReSomWin = "Ganzjahresreifen" Then
                strReSoWi = "G"
            End If

            If ReFzgZugelassen = "Nein" Then
                strReZugelassen = "x"
            End If
            If Hin_KCL_Zulassen = "Ja" Then
                strHinKCLZul = "X"
            End If

            If Anschluss = True Then
                strReKennzeichen = ReKenn1 & "-" & ReKenn2
            End If

            If SelRegulierer = "" Then
                strRegKunnr = strKunnr
            Else
                strRegKunnr = SelRegulierer
            End If

            If SelRechnungsempf = "" Then
                strRechKunnr = strKunnr
            Else
                strRechKunnr = SelRechnungsempf
            End If

            If SelAbholung = "" Then
                strAbKunnr = strKunnr
            Else
                strAbKunnr = SelAbholung
            End If

            If SelAnlieferung = "" Then
                strAnKunnr = strKunnr
            Else
                strAnKunnr = SelAnlieferung
            End If

            ZulAuftrag = ""

            ' ---------------------------------------------------

            ' Tabellen füllen
            ' ---------------------------------------------------

            SAPTable = S.AP.GetImportTableWithInit("Z_M_Order_Ueberfuehrung_Create.PARTNER")

            'Auftraggeber

            SAPTableRow = SAPTable.NewRow()

            SAPTableRow("PARTN_ROLE") = "AG"
            SAPTableRow("ITM_NUMBER") = "000000"
            SAPTableRow("PARTN_NUMB") = strKunnr

            SAPTable.Rows.Add(SAPTableRow)


            'Regulierer

            SAPTableRow = SAPTable.NewRow()

            SAPTableRow("PARTN_ROLE") = "RG"
            SAPTableRow("ITM_NUMBER") = "000000"
            SAPTableRow("PARTN_NUMB") = strRegKunnr

            SAPTable.Rows.Add(SAPTableRow)


            'Rechnungsempfänger

            SAPTableRow = SAPTable.NewRow()

            SAPTableRow("PARTN_ROLE") = "RE"
            SAPTableRow("ITM_NUMBER") = "000000"
            SAPTableRow("PARTN_NUMB") = strRechKunnr

            SAPTable.Rows.Add(SAPTableRow)


            'Warenempfänger

            SAPTableRow = SAPTable.NewRow()

            SAPTableRow("PARTN_ROLE") = "WE"
            SAPTableRow("ITM_NUMBER") = "000000"
            SAPTableRow("PARTN_NUMB") = strAnKunnr
            SAPTableRow("Name") = AnName
            SAPTableRow("Name_2") = AnAnsprechpartner
            SAPTableRow("Street") = AnStrasse & " " & AnNr
            SAPTableRow("Postl_Code") = AnPlz
            SAPTableRow("City") = AnOrt
            SAPTableRow("Telephone") = AnTelefon
            SAPTableRow("Telephone2") = AnTelefon2

            SAPTable.Rows.Add(SAPTableRow)


            'Abholung

            SAPTableRow = SAPTable.NewRow()

            SAPTableRow("PARTN_ROLE") = "ZB"
            SAPTableRow("ITM_NUMBER") = "000000"
            SAPTableRow("PARTN_NUMB") = strAbKunnr
            SAPTableRow("Name") = AbName
            SAPTableRow("Name_2") = AbAnsprechpartner
            SAPTableRow("Street") = AbStrasse & " " & AbNr
            SAPTableRow("Postl_Code") = AbPlz
            SAPTableRow("City") = AbOrt
            SAPTableRow("Telephone") = AbTelefon
            SAPTableRow("Telephone2") = AbTelefon2

            SAPTable.Rows.Add(SAPTableRow)


            If Anschluss = True Then
                'Retour

                SAPTableRow = SAPTable.NewRow()

                SAPTableRow("PARTN_ROLE") = "ZR"
                SAPTableRow("ITM_NUMBER") = "000000"
                SAPTableRow("PARTN_NUMB") = strKunnr
                SAPTableRow("Name") = ReName
                SAPTableRow("Name_2") = ReAnsprechpartner
                SAPTableRow("Street") = ReStrasse & " " & ReNr
                SAPTableRow("Postl_Code") = RePlz
                SAPTableRow("City") = ReOrt
                SAPTableRow("Telephone") = ReTelefon

                SAPTable.Rows.Add(SAPTableRow)
            End If

            SAPTable.AcceptChanges()

            ' ---------------------------------------------------

            S.AP.SetImportParameter("KUNNR", strKunnr)
            S.AP.SetImportParameter("VKORG", strVkorg)
            S.AP.SetImportParameter("MATNR", strMatnr)
            S.AP.SetImportParameter("ZZBRIEF", Herst)
            S.AP.SetImportParameter("ZZKENN", strKennzeichen)
            S.AP.SetImportParameter("ZZREFNR", Ref)
            S.AP.SetImportParameter("ZZFAHRG", Vin)
            S.AP.SetImportParameter("VDATU", strDatUeberf)
            S.AP.SetImportParameter("ZULGE", strZugelassen)
            S.AP.SetImportParameter("WASCHEN", strWaesche)
            S.AP.SetImportParameter("TANKE", strTanken)
            S.AP.SetImportParameter("EINW", strEinweisung)
            S.AP.SetImportParameter("SOWIHIN", strSoWi)
            S.AP.SetImportParameter("ROTKENN", strRotKenn)
            S.AP.SetImportParameter("ZZBEZEI", ReHerst)
            S.AP.SetImportParameter("ZZKENNRUCK", strReKennzeichen)
            S.AP.SetImportParameter("ZZFAHRGRUCK", ReVin)
            S.AP.SetImportParameter("ZZREFNRRUCK", ReRef)
            S.AP.SetImportParameter("ZULGRUCK", strReZugelassen)
            S.AP.SetImportParameter("SOWIRUCK", strReSoWi)
            S.AP.SetImportParameter("BEMERKUNG", "")
            S.AP.SetImportParameter("AUGRU", SelFahrzeugwert)
            S.AP.SetImportParameter("EQUNR", Equipment)
            S.AP.SetImportParameter("I_VBELN", Auftragsnummer)
            S.AP.SetImportParameter("USER", m_objUser.UserName)
            S.AP.SetImportParameter("LFSPERR", ZulAuftrag)
            S.AP.SetImportParameter("BEMERKUNG02", Bemerkung)
            S.AP.SetImportParameter("BEMERKUNG03", "")
            S.AP.SetImportParameter("WINTER", "")
            S.AP.SetImportParameter("FIX", "")
            S.AP.SetImportParameter("UNBKRUCK", "")
            S.AP.SetImportParameter("ZZFAHRZGTYP", Herst)
            S.AP.SetImportParameter("KFZ_KL", Fahrzeugklasse)
            S.AP.SetImportParameter("KFZ_KLR", ReFahrzeugklasse)
            S.AP.SetImportParameter("EXPRESS_VERSAND", Express)
            S.AP.SetImportParameter("HIN_ZUL_KCL", strHinKCLZul)
            S.AP.SetImportParameter("ANSCHL_ZUL_KCL", strReKCLZul)
            S.AP.SetImportParameter("I_WMENG", "")

            'myProxy.callBapi()
            S.AP.Execute()

            Vbeln = S.AP.GetExportParameter("VBELN")
            Vbeln1510 = S.AP.GetExportParameter("VBELN_1510")
            Qmnum = S.AP.GetExportParameter("QMNUM")

            'SAPTable = myProxy.getExportTable("PARTNER")
            SAPTableRet = S.AP.GetExportTable("RETURN")

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case "NO_PARAMETER"
                    m_strMessage = "Eingabedaten nicht ausreichend."
                Case "NO_ASL"
                    m_strMessage = "Falsche Kundennr."
                Case "NO_LANGTEXT"
                    m_strMessage = ""
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        End Try

        Return SAPTableRet

    End Function

    Public Function Update(Optional ByVal sAktion As String = "B") As DataTable

        ' Parameter füllen
        ' -------------------------------------------

        Dim strEinweisung As String = ""
        Dim strTanken As String = ""
        Dim strZugelassen As String = ""
        Dim strWaesche As String = ""
        Dim strMatnr As String = ""
        Dim strKennzeichen As String = Kenn1 & "-" & Kenn2
        Dim strAbKunnr As String = ""
        Dim strAnKunnr As String = ""
        Dim strRegKunnr As String = ""
        Dim strRechKunnr As String = ""
        Dim strReKCLZul As String = ""
        Dim strHinKCLZul As String = ""
        Dim strSoWi As String = ""
        Dim strReSoWi As String = ""
        Dim strReZugelassen As String = ""
        Dim strReKennzeichen As String = ""
        Dim strDatUeberf As Date = Nothing
        Dim strRotKenn As String = ""

        Dim strKunnr As String = Right("0000000000" & m_objUser.KUNNR, 10)
        Dim Saprow As DataRow
        Dim Saprows() As DataRow
        Dim tblPartnerReturn As DataTable = Nothing

        strVkorg = "1510"

        If booFzgEinweisung = True Then
            strEinweisung = "x"
        End If

        If booTanken = True Then
            strTanken = "x"
        End If

        If booWaesche = True Then
            strWaesche = "x"
        End If

        If booRotKenn = True Then
            strRotKenn = "x"
        End If
        If Express = "Ja" Then
            strExpress = "x"
        Else
            strExpress = "n"
        End If

        If FzgZugelassen = "Nein" Then
            strZugelassen = "n"
        Else
            strZugelassen = "x"
        End If

        If ReFzgZugelassen = "Nein" Then
            strReZugelassen = "n"
        Else
            strReZugelassen = "x"
        End If

        If SomWin = "Winter" Then
            strSoWi = "X"
        ElseIf SomWin = "Ganzjahresreifen" Then
            strSoWi = "G"
        ElseIf SomWin = "Sommer" Then
            strSoWi = "S"
        End If

        If ReSomWin = "Winter" Then
            strReSoWi = "X"
        ElseIf ReSomWin = "Ganzjahresreifen" Then
            strReSoWi = "G"
        ElseIf ReSomWin = "Sommer" Then
            strReSoWi = "S"
        End If

        If Anschluss = True Then
            strMatnr = "000000000000002340"
            strReKennzeichen = ReKenn1 & "-" & ReKenn2
        Else
            strMatnr = "000000000000001911"
        End If

        If DatumUeberf <> "" Then
            strDatUeberf = CDate(DatumUeberf)
        End If
        If Hin_KCL_Zulassen = "Ja" Then
            strHinKCLZul = "X"
        Else
            strHinKCLZul = "N"
        End If

        strReKCLZul = ""

        If SelRegulierer = "" Then
            strRegKunnr = strKunnr
        Else
            strRegKunnr = SelRegulierer
        End If

        If SelRechnungsempf = "" Then
            strRechKunnr = strKunnr
        Else
            strRechKunnr = SelRechnungsempf
        End If


        If SelAbholung = "" Then
            strAbKunnr = strKunnr
        Else
            strAbKunnr = SelAbholung
        End If

        If SelAnlieferung = "" Then
            strAnKunnr = strKunnr
        Else
            strAnKunnr = SelAnlieferung
        End If

        ' -------------------------------------------

        ' Tabellen füllen
        ' -------------------------------------------
        Try

            'Auftraggeber
            Saprows = TabPartnerSel.Select("Partn_Role='AG'")

            For Each Saprow In Saprows
                Saprow("Partn_Role") = "AG"
                Saprow("Itm_Number") = "000000"
                Saprow("Partn_Numb") = strKunnr
            Next

            TabPartnerSel.AcceptChanges()

            'Regulierer
            Saprows = TabPartnerSel.Select("Partn_Role='RG'")

            For Each Saprow In Saprows
                Saprow("Itm_Number") = "000000"
                Saprow("Partn_Numb") = strRegKunnr
            Next

            TabPartnerSel.AcceptChanges()

            'Rechnungsempfänger

            Saprows = TabPartnerSel.Select("Partn_Role='RE'")
            For Each Saprow In Saprows
                Saprow("Itm_Number") = "000000"
                Saprow("Partn_Numb") = strRechKunnr
            Next

            TabPartnerSel.AcceptChanges()

            'Warenempfänger
            Saprows = TabPartnerSel.Select("Partn_Role='WE'")

            For Each Saprow In Saprows
                Saprow("Itm_Number") = "000000"
                Saprow("Partn_Numb") = strAnKunnr
                Saprow("Name") = AnName
                Saprow("Name_2") = AnAnsprechpartner
                Saprow("STREET") = AnStrasse & " " & AnNr
                Saprow("Postl_Code") = AnPlz
                Saprow("City") = AnOrt
                Saprow("Telephone") = AnTelefon
                Saprow("Telephone2") = AnTelefon2
            Next
            TabPartnerSel.AcceptChanges()


            Saprows = TabPartnerSel.Select("Partn_Role='ZB'")

            For Each Saprow In Saprows
                Saprow("Itm_Number") = "000000"
                Saprow("Partn_Numb") = strAbKunnr
                Saprow("Name") = AbName
                Saprow("Name_2") = AbAnsprechpartner
                Saprow("STREET") = AbStrasse & " " & AbNr
                Saprow("Postl_Code") = AbPlz
                Saprow("City") = AbOrt
                Saprow("Telephone") = AbTelefon
                Saprow("Telephone2") = AbTelefon2
            Next
            TabPartnerSel.AcceptChanges()


            If Anschluss = True Then
                'Retour
                Saprows = TabPartnerSel.Select("Partn_Role='ZR'")

                For Each Saprow In Saprows
                    Saprow("Itm_Number") = "000000"
                    Saprow("Partn_Numb") = strKunnr
                    Saprow("Name") = ReName
                    Saprow("Name_2") = ReAnsprechpartner
                    Saprow("STREET") = ReStrasse & " " & ReNr
                    Saprow("Postl_Code") = RePlz
                    Saprow("City") = ReOrt
                    Saprow("Telephone") = ReTelefon
                    Saprow("Telephone2") = ReTelefon2
                Next
                TabPartnerSel.AcceptChanges()
            End If


            Saprows = TabUeberfSel.Select("AUF_ID='" & AufIDSel & "'")

            For Each Saprow In Saprows
                Saprow("VKORG") = strVkorg
                Saprow("KUNRG") = strRegKunnr
                Saprow("KUNRE") = strRechKunnr
                Saprow("MATNR") = strMatnr
                Saprow("ZZBRIEF") = ""
                Saprow("ZZKENN") = strKennzeichen
                Saprow("ZZREFNR") = Ref
                Saprow("ZZFAHRG") = Vin
                Saprow("VDATU") = strDatUeberf
                Saprow("ZULGE") = strZugelassen
                Saprow("WASCHEN") = strWaesche
                Saprow("TANKE") = strTanken
                Saprow("EINW") = strEinweisung
                Saprow("SOWIHIN") = strSoWi
                Saprow("RotKenn") = strRotKenn
                Saprow("Bemerkung") = " "
                Saprow("AUGRU") = SelFahrzeugwert
                Saprow("EQUNR") = Equipment
                Saprow("BEMERKUNG02") = strBemerkung
                Saprow("BEMERKUNG03") = " "
                Saprow("ZZFAHRZGTYP") = strHerst
                Saprow("KFZ_KL") = Fahrzeugklasse
                Saprow("EXPRESS_VERSAND") = strExpress
                Saprow("HIN_ZUL_KCL") = strHinKCLZul

                Saprow("ZZKENNRUCK") = strReKennzeichen
                Saprow("ZZREFNRRUCK") = ReRef
                Saprow("ZZFAHRGRUCK") = ReVin
                Saprow("ZZFAHRZGTYPRUCK") = strReHerst
                Saprow("ZULGRUCK") = strZugelassen
                Saprow("ANSCHL_ZUL_KCL") = strReKCLZul
                Saprow("SOWIRUCK") = strReSoWi
                Saprow("KFZ_KLR") = ReFahrzeugklasse
                Saprow("STATUS") = statusauftr
                If boonewdataset = True Then
                    Saprow("ERDAT") = Today 'HelpProcedures.MakeDateSAP(Today)
                    Saprow("ERNAM") = m_objUser.UserName
                Else : Saprow("AEDAT") = Today 'HelpProcedures.MakeDateSAP(Today)
                    Saprow("AENAM") = m_objUser.UserName
                End If
            Next

            TabUeberfSel.AcceptChanges()

            ' -------------------------------------------

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_KCL_CHANGE_AUFTR_001", m_objApp, m_objUser, page)

            S.AP.Init("Z_V_KCL_CHANGE_AUFTR_001", "I_AKTION", sAktion)

            'myProxy.setImportParameter("I_AKTION", sAktion)

            Dim tmpPartner As DataTable = S.AP.GetImportTable("GT_PARTNER")
            tmpPartner.Merge(TabPartnerSel)
            tmpPartner.AcceptChanges()

            Dim tmpUeberf As DataTable = S.AP.GetImportTable("I_AUFTRAG")
            tmpUeberf.Merge(TabUeberfSel)
            tmpUeberf.AcceptChanges()

            'myProxy.callBapi()
            S.AP.Execute()

            Dim strError As String = S.AP.GetExportParameter("E_FEHLER")

            If strError IsNot Nothing AndAlso strError <> String.Empty Then
                Throw New Exception(strError)
            End If

            tblPartnerReturn = S.AP.GetExportTable("GT_PARTNER")

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        Catch ex As Exception
            m_intStatus = -5555

            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case "NO_PARAMETER"
                    m_strMessage = "Eingabedaten nicht ausreichend."
                Case "NO_ASL"
                    m_strMessage = "Falsche Kundennr."
                Case "NO_LANGTEXT"
                    m_strMessage = ""
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        End Try

        Return tblPartnerReturn
    End Function

    Public Function getPartner(ByVal KunNr As String) As DataSets.AddressDataSet.ADDRESSEDataTable

        Dim SAPTable As DataTable

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Partner_Aus_Knvp_Lesen", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("KUNNR", Right("0000000000" & KunNr, 10))

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Partner_Aus_Knvp_Lesen", "KUNNR", Right("0000000000" & KunNr, 10))

            SAPTable = S.AP.GetExportTable("AUSGABE")

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case "NO_PARAMETER"
                    m_strMessage = "Eingabedaten nicht ausreichend."
                Case "NO_ASL"
                    m_strMessage = "Falsche Kundennr."
                Case "NO_LANGTEXT"
                    m_strMessage = ""
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        End Try

        Dim res As New DataSets.AddressDataSet.ADDRESSEDataTable()

        For Each row As DataRow In SAPTable.Rows
            res.AddADDRESSERow(row("Name1").ToString(), row("Post_Code1").ToString(), row("City1").ToString(), row("Street").ToString(), _
                               row("Name1").ToString() + "_" + row("Post_Code1").ToString() + "_" + row("City1").ToString(), _
                               row("House_Num1").ToString(), row("Tel_Number").ToString(), "", row("Name2").ToString(), row("Parvw").ToString(), row("Kunnr").ToString(), "")
        Next

        Return res

    End Function

    Public Function getPartnerStandard(ByVal KunNr As String) As DataTable

        Dim sapTable As DataTable = Nothing

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Partner_Aus_Knvp_Lesen", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("KUNNR", Right("0000000000" & KunNr, 10))

            'myProxy.callBapi()
            S.AP.InitExecute("Z_M_PARTNER_AUS_KNVP_LESEN", "KUNNR", Right("0000000000" & KunNr, 10))

            sapTable = S.AP.GetExportTable("AUSGABE") 'myProxy.getExportTable("AUSGABE")


            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message.Replace("Execution failed", "")
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case "NO_PARAMETER"
                    m_strMessage = "Eingabedaten nicht ausreichend."
                Case "NO_ASL"
                    m_strMessage = "Falsche Kundennr."
                Case "NO_LANGTEXT"
                    m_strMessage = ""
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        End Try

        Return sapTable

    End Function

    Public Function InsertUeberfuehrung() As Boolean

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ZULAUFTR_INSERT", m_objApp, m_objUser, page)

            Dim SAPTable As DataTable = S.AP.GetImportTableWithInit("Z_M_ZULAUFTR_INSERT.GT_WEB")
            Dim DtRow As DataRow = SAPTable.NewRow()

            DtRow("KUNNR") = Right("0000000000" & m_objUser.KUNNR, 10)
            DtRow("CHASSIS_NUM") = Vin
            DtRow("EQUNR") = Equipment
            DtRow("ZZREFERENZ") = Ref
            DtRow("VBELN") = Auftragsnummer

            SAPTable.Rows.Add(DtRow)
            SAPTable.AcceptChanges()


            S.AP.SetImportParameter("I_BAPI", "Z_M_ZULAUFTR_INSERT")
            S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            S.AP.SetImportParameter("I_WEB_REPORT", "Ueberf01.aspx")

            'myProxy.callBapi()
            S.AP.Execute()

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

            Return True
        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case "NO_PARAMETER"
                    m_strMessage = "Eingabedaten nicht ausreichend."
                Case "NO_ASL"
                    m_strMessage = "Falsche Kundennr."
                Case "NO_LANGTEXT"
                    m_strMessage = ""
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

            Return False

        End Try

    End Function

    Public Sub SetVehicleData(ByVal KunNr As String, ByVal Kennzeichen As String, ByVal ref As String)

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Fahrzeugdaten", m_objApp, m_objUser, page)
            S.AP.Init("Z_M_Fahrzeugdaten")

            S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & KunNr, 10))
            S.AP.SetImportParameter("I_LICENSE_NUM", Kennzeichen)
            S.AP.SetImportParameter("I_LIZNR", ref)

            'myProxy.callBapi()
            S.AP.Execute()

            Dim SAPStruc As DataTable = S.AP.GetExportTable("GF_WEB")

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)


            Dim StrucRow As DataRow = SAPStruc.Rows(0)

            If Trim(StrucRow("License_Num").ToString) <> String.Empty Then
                Dim arKenn As String() = SAPStruc.Rows(0)("License_Num").ToString.Split("-"c)
                If arKenn.GetLength(0) = 2 Then
                    Kenn1 = arKenn(0)
                    Kenn2 = arKenn(1)
                End If
            End If

            Herst = StrucRow("ZZHERST_TEXT").ToString & " " & StrucRow("ZZKLARTEXT_TYP").ToString
            ref = StrucRow("LIZNR").ToString
            Vin = StrucRow("CHASSIS_NUM").ToString
            Equipment = StrucRow("EQUNR").ToString

            FahrzeugVorhanden = True

        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case "NO_PARAMETER"
                    m_strMessage = "Eingabedaten nicht ausreichend."
                Case "NO_ASL"
                    m_strMessage = "Falsche Kundennr."
                Case "NO_LANGTEXT"
                    m_strMessage = ""
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        End Try

    End Sub

    Public Function GetUeberfuehrungsbuffer(ByVal KunNr As String) As DataTable

        Dim sapTable As DataTable = Nothing

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ZULAUFTR_LIST", m_objApp, m_objUser, Page)
            S.AP.Init("Z_M_ZULAUFTR_LIST")

            S.AP.SetImportParameter("I_BAPI", "Z_M_ZULAUFTR_LIST")
            S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & KunNr, 10))
            S.AP.SetImportParameter("I_WEB_REPORT", "Ueberf01.aspx")

            'myProxy.callBapi()
            S.AP.Execute()

            sapTable = S.AP.GetExportTable("GT_WEB")

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case "NO_PARAMETER"
                    m_strMessage = "Eingabedaten nicht ausreichend."
                Case "NO_ASL"
                    m_strMessage = "Falsche Kundennr."
                Case "NO_LANGTEXT"
                    m_strMessage = ""
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        End Try

        Return sapTable

    End Function

    Public Sub getSTVA(ByVal strPLZ As String)

        Dim SAPTableSTVA As DataTable = Nothing
        Dim SAPTablePLZ As DataTable = Nothing

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_Get_Zulst_By_Plz", m_objApp, m_objUser, page)

            S.AP.InitExecute("Z_Get_Zulst_By_Plz", "I_PLZ,I_ORT", strPLZ, "")

            'myProxy.setImportParameter()
            'myProxy.setImportParameter("I_ORT", "")

            'myProxy.callBapi()

            SAPTablePLZ = S.AP.GetExportTable("T_ORTE")
            SAPTableSTVA = S.AP.GetExportTable("T_ZULST")

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case "NO_PARAMETER"
                    m_strMessage = "Eingabedaten nicht ausreichend."
                Case "NO_ASL"
                    m_strMessage = "Falsche Kundennr."
                Case "NO_LANGTEXT"
                    m_strMessage = ""
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        End Try

        tblKreis = SAPTableSTVA

    End Sub

    Public Sub getVKBuero(ByVal KunNr As String)

        Dim sapTable As DataTable = Nothing

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Adresse_Vk_Buero", m_objApp, m_objUser, page)
            S.AP.Init("Z_M_Adresse_Vk_Buero")

            S.AP.SetImportParameter("I_BAPI", "Z_M_ADRESSE_VK_BUERO")
            S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & KunNr, 10))
            S.AP.SetImportParameter("I_KUNNR", "ZUL01.aspx")
            S.AP.SetImportParameter("I_AKTION", "")

            S.AP.Execute()
            'myProxy.callBapi()

            VB3100 = S.AP.GetExportParameter("NE_3100")
            sapTable = S.AP.GetExportTable("E_ADRESSE")

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

            If Not sapTable Is Nothing Then
                With sapTable.Rows(0)
                    VBName1 = ("Name1")
                    VBName2 = ("Name2")
                    VBStrasse = ("Street") & " " & ("House_Num1")
                    VBPlzOrt = ("Post_Code1") & " " & ("City1")
                    VBTel = ("Tel_Number")
                    VBMail = ("Smtp_Addr")
                End With
            End If

        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case "NO_PARAMETER"
                    m_strMessage = "Eingabedaten nicht ausreichend."
                Case "NO_ASL"
                    m_strMessage = "Falsche Kundennr."
                Case "NO_LANGTEXT"
                    m_strMessage = ""
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        End Try

    End Sub

    Public Sub getVKBueroFooter(ByVal KunNr As String)

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Adresse_Vk_Buero", m_objApp, m_objUser, page)
            S.AP.Init("Z_M_Adresse_Vk_Buero")

            S.AP.SetImportParameter("I_BAPI", "Z_M_ADRESSE_VK_BUERO")
            S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & KunNr, 10))
            S.AP.SetImportParameter("I_WEB_REPORT", "ZUL01.aspx")
            S.AP.SetImportParameter("I_AKTION", "A")

            S.AP.Execute()
            'myProxy.callBapi()

            Dim sapTable As DataTable = S.AP.GetExportTable("E_ADRESSE")

            If Not sapTable Is Nothing Then

                Dim dr As DataRow = sapTable.Rows(0)

                FooVBName1 = dr("Name1").ToString
                FooVBName2 = dr("Name2").ToString
                FooVBStrasse = dr("Street").ToString & " " & dr("House_Num1").ToString
                FooVBPlzOrt = dr("Post_Code1").ToString & " " & dr("City1").ToString
                FooVBTel = "Telefon " & dr("Tel_Number").ToString
                FooVBFax = "Fax " & dr("Fax_Number").ToString
                FooVBMail = dr("Smtp_Addr").ToString
            End If

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case "NO_PARAMETER"
                    m_strMessage = "Eingabedaten nicht ausreichend."
                Case "NO_ASL"
                    m_strMessage = "Falsche Kundennr."
                Case "NO_LANGTEXT"
                    m_strMessage = ""
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        End Try

    End Sub

    Public Overloads Sub FillData()

        Dim strKunnr As String = Right("0000000000" & m_objUser.KUNNR, 10)

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_KCL_READ_AUFTR_001", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR_AG", strKunnr)

            'myProxy.callBapi()
            S.AP.InitExecute("Z_V_KCL_READ_AUFTR_001", "I_KUNNR_AG", strKunnr)

            Dim sapTableAuf As DataTable = S.AP.GetExportTable("GT_AUFTRAG")
            Dim sapTablePartn As DataTable = S.AP.GetExportTable("GT_PARTNER")

            tblUpload = New DataTable
            tblUpload.Columns.Add("Problem", Type.GetType("System.String"))
            tblUpload.Columns.Add("Auf_ID", Type.GetType("System.String"))
            tblUpload.Columns.Add("Datum", Type.GetType("System.String"))
            tblUpload.Columns.Add("UeberfDatum", Type.GetType("System.String"))
            tblUpload.Columns.Add("Referenz", Type.GetType("System.String"))
            tblUpload.Columns.Add("Kennzeichen", Type.GetType("System.String"))
            tblUpload.Columns.Add("Fahrzeugtyp", Type.GetType("System.String"))
            tblUpload.Columns.Add("Startort", Type.GetType("System.String"))
            tblUpload.Columns.Add("Zielort", Type.GetType("System.String"))
            tblUpload.Columns.Add("Rueckort", Type.GetType("System.String"))
            tblUpload.Columns.Add("WEZielort", Type.GetType("System.String"))
            tblUpload.Columns.Add("Url", Type.GetType("System.String"))
            tblUpload.Columns.Add("Vbeln", Type.GetType("System.String"))
            tblUpload.Columns.Add("Ok", Type.GetType("System.Boolean"))
            tblUpload.Columns.Add("Del", Type.GetType("System.Boolean"))
            tblUpload.Columns.Add("NoSel", Type.GetType("System.Boolean"))
            tblUpload.Columns.Add("User", Type.GetType("System.String"))

            'Aufträge
            For Each datarow As DataRow In sapTableAuf.Rows
                Dim newrow As DataRow = tblUpload.NewRow

                Dim aufID As String = datarow("Auf_ID").ToString
                Dim anschluss As Boolean = False

                newrow("Problem") = "O"
                newrow("Auf_ID") = aufID
                newrow("Ok") = False
                newrow("Del") = False
                newrow("NoSel") = True

                Dim rowspartner() As DataRow = sapTablePartn.Select("AUF_ID='" & aufID & "'")

                If aufID = "524" Then
                    newrow("Kennzeichen") = datarow("ZZKENN").ToString
                End If

                For Each datarow2 As DataRow In rowspartner
                    Select Case datarow2("PARTN_ROLE").ToString
                        Case "WE"
                            newrow("Zielort") = datarow2("CITY").ToString
                            newrow("WEZielort") = datarow2("NAME").ToString

                            If datarow2("CITY").ToString.Trim.Length = 0 Then
                                newrow("Problem") = "X"
                            ElseIf datarow2("NAME").ToString.Trim.Length = 0 Then
                                newrow("Problem") = "X"
                            ElseIf datarow2("STREET").ToString.Trim.Length = 0 Then
                                newrow("Problem") = "X"
                            ElseIf datarow2("POSTL_CODE").ToString.Trim.Length = 0 Then
                                newrow("Problem") = "X"
                            ElseIf datarow2("NAME_2").ToString.Trim.Length = 0 Then
                                newrow("Problem") = "X"
                            ElseIf datarow2("TELEPHONE").ToString.Trim.Length = 0 Then
                                newrow("Problem") = "X"
                            End If
                        Case "ZB"
                            newrow("Startort") = datarow2("CITY").ToString

                            If datarow2("CITY").ToString.Trim.Length = 0 Then
                                newrow("Problem") = "X"
                            ElseIf datarow2("NAME").ToString.Trim.Length = 0 Then
                                newrow("Problem") = "X"
                            ElseIf datarow2("STREET").ToString.Trim.Length = 0 Then
                                newrow("Problem") = "X"
                            ElseIf datarow2("POSTL_CODE").ToString.Trim.Length = 0 Then
                                newrow("Problem") = "X"
                            ElseIf datarow2("NAME_2").ToString.Trim.Length = 0 Then
                                newrow("Problem") = "X"
                            ElseIf datarow2("TELEPHONE").ToString.Trim.Length = 0 Then
                                newrow("Problem") = "X"
                            End If
                        Case "ZR"
                            anschluss = True
                            newrow("Rueckort") = datarow2("CITY").ToString

                            If datarow2("CITY").ToString.Trim.Length = 0 Then
                                newrow("Problem") = "X"
                            ElseIf datarow2("NAME").ToString.Trim.Length = 0 Then
                                newrow("Problem") = "X"
                            ElseIf datarow2("STREET").ToString.Trim.Length = 0 Then
                                newrow("Problem") = "X"
                            ElseIf datarow2("POSTL_CODE").ToString.Trim.Length = 0 Then
                                newrow("Problem") = "X"
                            ElseIf datarow2("NAME_2").ToString.Trim.Length = 0 Then
                                newrow("Problem") = "X"
                            ElseIf datarow2("TELEPHONE").ToString.Trim.Length = 0 Then
                                newrow("Problem") = "X"
                            End If
                    End Select

                Next

                If Not IsDBNull(datarow("ERDAT")) AndAlso IsDate(datarow("ERDAT")) AndAlso CType(datarow("ERDAT"), DateTime).Year > 1900 Then
                    newrow("Datum") = CType(datarow("ERDAT"), DateTime).ToShortDateString()
                Else
                    newrow("Datum") = ""
                End If

                If Not IsDBNull(datarow("VDATU")) AndAlso IsDate(datarow("VDATU")) AndAlso CType(datarow("VDATU"), DateTime).Year > 1900 Then
                    newrow("UeberfDatum") = CType(datarow("VDATU"), DateTime).ToShortDateString()
                Else
                    newrow("UeberfDatum") = ""
                End If

                newrow("Referenz") = datarow("ZZREFNR").ToString
                newrow("Kennzeichen") = datarow("ZZKENN").ToString
                newrow("User") = datarow("ERNAM").ToString

                If newrow("Kennzeichen").ToString.Trim.Length = 0 Then
                    newrow("Problem") = "X"
                End If

                newrow("Fahrzeugtyp") = datarow("ZZFAHRZGTYP").ToString

                If datarow("ZZFAHRZGTYP").ToString.Trim.Length = 0 Then
                    newrow("Problem") = "X"
                End If

                If datarow("KFZ_KL").ToString.Trim.Length = 0 Then
                    newrow("Problem") = "X"
                End If

                If datarow("ZULGE").ToString.Trim.Length = 0 Then
                    newrow("Problem") = "X"
                End If

                If datarow("SOWIHIN").ToString.Trim.Length = 0 Then
                    newrow("Problem") = "X"
                End If

                If datarow("AUGRU").ToString.Trim.Length = 0 Then
                    newrow("Problem") = "X"
                End If

                If datarow("HIN_ZUL_KCL").ToString.Trim.Length = 0 AndAlso (IsDBNull(datarow("VDATU")) OrElse Not IsDate(datarow("VDATU")) OrElse CType(datarow("VDATU"), DateTime).Year < 1901) Then
                    newrow("Problem") = "X"
                End If

                If datarow("EXPRESS_VERSAND").ToString.Trim.Length = 0 Then
                    newrow("Problem") = "X"
                End If

                If anschluss = True Then
                    If datarow("ZZKENNRUCK").ToString.Trim.Length = 0 Then
                        newrow("Problem") = "X"
                    End If
                    If datarow("ZZFAHRZGTYPRUCK").ToString.Trim.Length = 0 Then
                        newrow("Problem") = "X"
                    End If
                    If datarow("KFZ_KLR").ToString.Trim.Length = 0 Then
                        newrow("Problem") = "X"
                    End If
                    If datarow("ZULGRUCK").ToString.Trim.Length = 0 Then
                        newrow("Problem") = "X"
                    End If
                    If datarow("SOWIRUCK").ToString.Trim.Length = 0 Then
                        newrow("Problem") = "X"
                    End If
                End If

                tblUpload.Rows.Add(newrow)
            Next

            selectedUser = "X"
            tblPartner = sapTablePartn
            tblUeberf = sapTableAuf

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case "NO_PARAMETER"
                    m_strMessage = "Eingabedaten nicht ausreichend."
                Case "NO_ASL"
                    m_strMessage = "Falsche Kundennr."
                Case "NO_LANGTEXT"
                    m_strMessage = ""
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        End Try

    End Sub

    Public Sub NewAuftrag()
        Dim newRow As DataRow

        With Me

            .TabPartnerSel = .TabPartner.Clone
            newRow = .TabPartnerSel.NewRow
            newRow("MANDT") = "010"
            newRow("KUNNR_AG") = Right("0000000000" & m_objUser.KUNNR, 10)
            newRow("AUF_ID") = "0000000000"
            newRow("Partn_Role") = "AG"
            .TabPartnerSel.Rows.Add(newRow)

            newRow = .TabPartnerSel.NewRow
            newRow("MANDT") = "010"
            newRow("KUNNR_AG") = Right("0000000000" & m_objUser.KUNNR, 10)
            newRow("AUF_ID") = "0000000000"
            newRow("Partn_Role") = "RE"
            .TabPartnerSel.Rows.Add(newRow)

            newRow = .TabPartnerSel.NewRow
            newRow("MANDT") = "010"
            newRow("KUNNR_AG") = Right("0000000000" & m_objUser.KUNNR, 10)
            newRow("AUF_ID") = "0000000000"
            newRow("Partn_Role") = "RG"
            .TabPartnerSel.Rows.Add(newRow)

            newRow = .TabPartnerSel.NewRow
            newRow("MANDT") = "010"
            newRow("KUNNR_AG") = Right("0000000000" & m_objUser.KUNNR, 10)
            newRow("AUF_ID") = "0000000000"
            newRow("Partn_Role") = "WE"
            .TabPartnerSel.Rows.Add(newRow)

            newRow = .TabPartnerSel.NewRow
            newRow("MANDT") = "010"
            newRow("KUNNR_AG") = Right("0000000000" & m_objUser.KUNNR, 10)
            newRow("AUF_ID") = "0000000000"
            newRow("Partn_Role") = "ZB"
            .TabPartnerSel.Rows.Add(newRow)

            If .Anschluss = True Then
                newRow = .TabPartnerSel.NewRow
                newRow("MANDT") = "010"
                newRow("KUNNR_AG") = Right("0000000000" & m_objUser.KUNNR, 10)
                newRow("AUF_ID") = "0000000000"
                newRow("Partn_Role") = "ZR"
                .TabPartnerSel.Rows.Add(newRow)
                .TabPartnerSel.AcceptChanges()
            End If


            .TabUeberfSel = .TabUeberf.Clone
            newRow = .TabUeberfSel.NewRow
            newRow("MANDT") = "010"
            newRow("KUNNR_AG") = Right("0000000000" & m_objUser.KUNNR, 10)
            newRow("AUF_ID") = "0000000000"
            .TabUeberfSel.Rows.Add(newRow)
            .TabUeberfSel.AcceptChanges()
        End With
    End Sub

    Public Sub CleanClass()

        With Me
            .KunnrNL = Nothing
            .Auftragsnummer = String.Empty
            .Bemerkung = String.Empty
            .DatumUeberf = String.Empty
            .Equipment = String.Empty
            .Fahrzeugklasse = String.Empty
            .FahrzeugklasseTxt = String.Empty
            .FahrzeugVorhanden = Nothing
            .FahrzeugwertTxt = String.Empty
            .Filiale = String.Empty
            .FzgEinweisung = Nothing
            .FzgZugelassen = String.Empty
            .Herst = String.Empty
            .Kenn1 = String.Empty
            .Kenn2 = String.Empty
            .Qmnum = String.Empty
            .Ref = String.Empty
            .ReAnsprechpartner = String.Empty
            .ReFahrzeugklasse = String.Empty
            .ReFahrzeugklasseTxt = String.Empty
            .ReFax = String.Empty
            .ReFzgZugelassen = String.Empty
            .ReHerst = String.Empty
            .ReKenn1 = String.Empty
            .ReKenn2 = String.Empty
            .ReName = String.Empty
            .ReOrt = String.Empty
            .RePlz = String.Empty
            .ReRef = String.Empty
            .ReSomWin = String.Empty
            .ReStrasse = String.Empty
            .ReNr = String.Empty
            .ReTelefon = String.Empty
            .ReTelefon2 = String.Empty
            .ReVin = String.Empty
            .SelRetour = Nothing

            .RotKenn = Nothing
            .SelFahrzeugwert = Nothing
            .SomWin = Nothing
            .Tanken = Nothing
            .VB3100 = String.Empty
            .Vbeln = String.Empty
            .Vbeln1510 = String.Empty
            .Vin = String.Empty
            .Waesche = Nothing
            .Zulassungsdatum = String.Empty
            .ZulAuftrag = String.Empty
            .ZulHaltername = String.Empty
            .ZulPLZ = String.Empty
            .Express = String.Empty
            .VBMail = String.Empty
            .VBName1 = String.Empty
            .VBName2 = String.Empty
            .VBPlzOrt = String.Empty
            .VBStrasse = String.Empty
            .VBTel = String.Empty

            .FooVBMail = String.Empty
            .FooVBName1 = String.Empty
            .FooVBName2 = String.Empty
            .FooVBPlzOrt = String.Empty
            .FooVBStrasse = String.Empty
            .FooVBTel = String.Empty
            .FooVBFax = String.Empty

            .Wunschkennzeichen1 = String.Empty
            .Wunschkennzeichen2 = String.Empty
            .ZP0 = String.Empty
            .ZP1 = String.Empty
            .ZP2 = String.Empty
            .ZP3 = String.Empty
            .ZP4 = String.Empty
            .ZP5 = String.Empty
            .ZP6 = String.Empty
            .ZP7 = String.Empty
            .ZP8 = String.Empty
            .ZP9 = String.Empty
            .Z0 = String.Empty
            .Z1 = String.Empty
            .Z2 = String.Empty
            .Z3 = String.Empty
            .Z4 = String.Empty
            .Z5 = String.Empty
            .Z6 = String.Empty
            .Z7 = String.Empty
            .Z8 = String.Empty
            .Z9 = String.Empty
            .UnterlagenTxt = String.Empty
            .ZulassungsdienstTxt = String.Empty
            .ShowTables = False
            .NewDataSet = False

        End With

    End Sub

    Public Function getStdTexte(ByRef tblHgruppe As DataTable, ByRef tblUgruppe As DataTable) As DataTable
        Dim strKunnr As String = Right("0000000000" & m_objUser.KUNNR, 10)
        
        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_Kcl_Web_Text_Read", m_objApp, m_objUser, page)

            S.AP.Init("Z_Kcl_Web_Text_Read")

            S.AP.SetImportParameter("I_KUNNR", strKunnr)
            S.AP.SetImportParameter("I_VKORG", "1510")
            S.AP.SetImportParameter("I_VTWEG", "01")
            S.AP.SetImportParameter("I_SPART", "01")

            tblHgruppe = S.AP.GetExportTable("WEB_HAUPTGRUPPE")
            tblUgruppe = S.AP.GetExportTable("WEB_UNTERGRUPPE")
            tblStdTexte = S.AP.GetExportTable("WEB_TEXTE")

            'myProxy.callBapi()
            S.AP.Execute()

            tblHgruppe = S.AP.GetExportTable("WEB_HAUPTGRUPPE")
            tblUgruppe = S.AP.GetExportTable("WEB_UNTERGRUPPE")
            tblStdTexte = S.AP.GetExportTable("WEB_TEXTE")

          
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case "NO_PARAMETER"
                    m_strMessage = "Eingabedaten nicht ausreichend."
                Case "NO_ASL"
                    m_strMessage = "Falsche Kundennr."
                Case "NO_LANGTEXT"
                    m_strMessage = ""
                Case "KUNDE_NOT_FOUND"
                    m_strMessage = "Kunde nicht gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
     
        End Try

        Return tblStdTexte
    End Function

    Public Sub saveStdTexte(ByVal dTable As DataTable, ByVal dTableHaupt As DataTable, ByVal dTableUnter As DataTable)
        Dim strKunnr As String = Right("0000000000" & m_objUser.KUNNR, 10)
        
        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_Kcl_Web_Text_Save", m_objApp, m_objUser, page)

            S.AP.Init("Z_Kcl_Web_Text_Save")

            S.AP.SetImportParameter("I_KUNNR", strKunnr)
            S.AP.SetImportParameter("I_VKORG", "1510")
            S.AP.SetImportParameter("I_VTWEG", "01")
            S.AP.SetImportParameter("I_SPART", "01")

            Dim SAPTableText As DataTable = S.AP.GetImportTable("WEB_TEXTE")
            SAPTableText.Merge(dTable)
            SAPTableText.AcceptChanges()

            Dim SAPTableHaupt As DataTable = S.AP.GetImportTable("WEB_HAUPTGRUPPE")
            SAPTableHaupt.Merge(dTableHaupt)
            SAPTableHaupt.AcceptChanges()

            Dim SAPTableUnter As DataTable = S.AP.GetImportTable("WEB_UNTERGRUPPE")
            SAPTableUnter.Merge(dTableUnter)
            SAPTableUnter.AcceptChanges()

            'myProxy.callBapi()
            S.AP.Execute()

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case "NO_PARAMETER"
                    m_strMessage = "Eingabedaten nicht ausreichend."
                Case "NO_ASL"
                    m_strMessage = "Falsche Kundennr."
                Case "NO_LANGTEXT"
                    m_strMessage = ""
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
       
        End Try

    End Sub

    Public Sub MakePartnerTable()
        tblPartner = New DataTable
        With tblPartner.Columns
            .Add("MANDT", Type.GetType("System.String"))
            .Add("KUNNR_AG", Type.GetType("System.String"))
            .Add("AUF_ID", Type.GetType("System.String"))
            .Add("PARTN_ROLE", Type.GetType("System.String"))
            .Add("PARTN_NUMB", Type.GetType("System.String"))
            .Add("ITM_NUMBER", Type.GetType("System.String"))
            .Add("TITLE", Type.GetType("System.String"))
            .Add("NAME", Type.GetType("System.String"))
            .Add("NAME_2", Type.GetType("System.String"))
            .Add("NAME_3", Type.GetType("System.String"))
            .Add("NAME_4", Type.GetType("System.String"))
            .Add("STREET", Type.GetType("System.String"))
            .Add("COUNTRY", Type.GetType("System.String"))
            .Add("POSTL_CODE", Type.GetType("System.String"))
            .Add("CITY", Type.GetType("System.String"))
            .Add("DISTRICT", Type.GetType("System.String"))
            .Add("REGION", Type.GetType("System.String"))
            .Add("TELEPHONE", Type.GetType("System.String"))
            .Add("TELEPHONE2", Type.GetType("System.String"))
            .Add("FAX_NUMBER", Type.GetType("System.String"))
            .Add("LANGU", Type.GetType("System.String"))
        End With
        tblPartner.AcceptChanges()

    End Sub

    Public Sub MakeAuftragsTable()
        tblUeberf = New DataTable
        With tblUeberf.Columns
            .Add("MANDT", Type.GetType("System.String"))
            .Add("KUNNR_AG", Type.GetType("System.String"))
            .Add("AUF_ID", Type.GetType("System.String"))
            .Add("VKORG", Type.GetType("System.String"))
            .Add("KUNRG", Type.GetType("System.String"))
            .Add("KUNRE", Type.GetType("System.String"))
            .Add("MATNR", Type.GetType("System.String"))
            .Add("ZZBRIEF", Type.GetType("System.String"))
            .Add("ZZKENN", Type.GetType("System.String"))
            .Add("ZZREFNR", Type.GetType("System.String"))
            .Add("ZZFAHRG", Type.GetType("System.String"))
            .Add("VDATU", Type.GetType("System.DateTime"))
            .Add("ZULGE", Type.GetType("System.String"))
            .Add("WASCHEN", Type.GetType("System.String"))
            .Add("TANKE", Type.GetType("System.String"))
            .Add("EINW", Type.GetType("System.String"))
            .Add("SOWIHIN", Type.GetType("System.String"))
            .Add("ROTKENN", Type.GetType("System.String"))
            .Add("ZZBEZEI", Type.GetType("System.String"))
            .Add("ZZKENNRUCK", Type.GetType("System.String"))
            .Add("ZZFAHRGRUCK", Type.GetType("System.String"))
            .Add("ZZREFNRRUCK", Type.GetType("System.String"))
            .Add("ZULGRUCK", Type.GetType("System.String"))
            .Add("SOWIRUCK", Type.GetType("System.String"))
            .Add("BEMERKUNG", Type.GetType("System.String"))
            .Add("AUGRU", Type.GetType("System.String"))
            .Add("EQUNR", Type.GetType("System.String"))
            .Add("ZUSER", Type.GetType("System.String"))
            .Add("LFSPERR", Type.GetType("System.String"))
            .Add("BEMERKUNG02", Type.GetType("System.String"))
            .Add("BEMERKUNG03", Type.GetType("System.String"))
            .Add("WINTER", Type.GetType("System.String"))
            .Add("FIX", Type.GetType("System.String"))
            .Add("UNBKRUCK", Type.GetType("System.String"))
            .Add("ZZFAHRZGTYP", Type.GetType("System.String"))
            .Add("KFZ_KL", Type.GetType("System.String"))
            .Add("KFZ_KLR", Type.GetType("System.String"))
            .Add("EXPRESS_VERSAND", Type.GetType("System.String"))
            .Add("HIN_ZUL_KCL", Type.GetType("System.String"))
            .Add("ANSCHL_ZUL_KCL", Type.GetType("System.String"))
            .Add("ZZFAHRZGTYPRUCK", Type.GetType("System.String"))
            .Add("WINTERABWERK", Type.GetType("System.String"))
            .Add("GRREIFEN", Type.GetType("System.String"))
            .Add("REIFGR", Type.GetType("System.String"))
            .Add("FELGEN", Type.GetType("System.String"))
            .Add("RADKAP", Type.GetType("System.String"))
            .Add("ZWEIRADSATZ", Type.GetType("System.String"))
            .Add("WINTERBEM", Type.GetType("System.String"))
            .Add("WINTERRUCK", Type.GetType("System.String"))
            .Add("WINTERABWERKRUCK", Type.GetType("System.String"))
            .Add("GRREIFENRUCK", Type.GetType("System.String"))
            .Add("REIFGRRUCK", Type.GetType("System.String"))
            .Add("FELGENRUCK", Type.GetType("System.String"))
            .Add("RADKAPRUCK", Type.GetType("System.String"))
            .Add("ZWEIRADSATZRUCK", Type.GetType("System.String"))
            .Add("WINTERBEMRUCK", Type.GetType("System.String"))
            .Add("STATUS", Type.GetType("System.String"))
            .Add("ERDAT", Type.GetType("System.DateTime"))
            .Add("ERNAM", Type.GetType("System.String"))
            .Add("AEDAT", Type.GetType("System.DateTime"))
            .Add("AENAM", Type.GetType("System.String"))
        End With

        tblUeberf.AcceptChanges()

    End Sub
#End Region

End Class
