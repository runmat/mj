Option Explicit On 
Option Strict On

Imports System

Namespace Business
    Public Class ABE2FHZG
        REM § Unterobjekt zur Speicherung der ABE-Daten, Kunde: Sixt, BAPI: -.

#Region " Declarations"
        Private m_strZZCHASSIS_NUM As String  'Fahrgestellnummer (Ziffer 4)
        Private m_strZZPRFZ As String  'Fahrgestellnummer-Prüfziffer (Ziffer 4)
        Private m_strZZREPLA_DATE As String  'Datum der Erstzulassung (Ziffer 32)
        Private m_strZZFLEET_HGT As String 'Höhe des Fahrzeugs (Ziffer 13)
        Private m_strZZFLEET_WID As String 'Breite des Fahrzeugs (Ziffer 13)
        Private m_strZZFLEET_LEN As String 'Länge des Fahrzeugs (Ziffer 13)
        Private m_strZZNUM_AXLE As String 'Anzahl der Achsen des Fahrzeugs (Ziffer 18)
        Private m_strZZMAX_OCCUPANTS As String 'Maximal zulässige Anzahl von beförderten Personen (Ziffer 12)
        Private m_strZZENGINE_TYPE As String 'Antriebsart (Ziffer 5)
        Private m_strZZENGINE_POWER As String 'Leistung bei gegebener Umdrehungszahl (Ziffer 7)
        Private m_strZZREVOLUTIONS As String 'Umdrehungszahl pro Minute (Ziffer 7)
        Private m_strZZENGINE_CAP As String 'Hubraum (Ziffer 8)
        Private m_strZZGROSS_WGT As String 'Zulässiges Gesamtgewicht (Ziffer 15)
        Private m_strZZFLEET_CAT As String 'Fahrzeugart (Ziffer 1)
        Private m_strZZFLEET_VIN As String 'Identifikationsnummer des Herstellers für das Fahrzeug (Ziffer 2)
        Private m_strZZUNIT_POWER As String 'Maßeinheit für die Leistung (Ziffer 7)
        Private m_strZZSPEED_MAX As String 'Höchstgeschwindigkeit des Fahrzeugs (Ziffer 6)
        Private m_strZZLGEW As String 'Leergewicht (Ziffer 14)
        Private m_strZZALVO As String 'Achslast vorn (Ziffer 16)
        Private m_strZZALHI As String 'Achslast hinten (Ziffer 16)
        Private m_strZZALMI As String 'Achslast mitte (Ziffer 16)
        Private m_strZZANGE As String 'davon angetriebene Achsen (Ziffer 19)
        Private m_strZZREVO As String 'Bereifunsgrösse vorn (Ziffer 20)
        Private m_strZZREHI As String 'Bereifunsgrösse mitte/hinten (Ziffer 21)
        Private m_strZZORVO As String 'oder Bereifunsgrösse vorn (Ziffer 22)
        Private m_strZZORHI As String 'oder Bereifunsgrösse mitte/hinten (Ziffer 23)
        Private m_strZZAKPZ As String 'Anhänger-Kupplung Prüfzeichen (Ziffer 27)
        Private m_strZZALBR As String 'Anhängerlast gebremst (Ziffer 28)
        Private m_strZZALUB As String 'Anhängerlast ungebremst (Ziffer 29)
        Private m_strZZSDB9 As String 'Stand-Geräusch Ziffern (Ziffer 30)
        Private m_strZZFDB9 As String 'Fahrgeräusch Ziffern (Ziffer 31)
        Private m_strZZBDR1 As String 'Bremsdruck Einleitung Bar * 10 (Ziffer 24)
        Private m_strZZBDR2 As String 'Bremsdruck Zweileitung Bar * 10 (Ziffer 25)
        Private m_strZZAKUP As String 'Anhängerkupplung DIN 740 (Ziffer 26)
        Private m_strZZAUSF As String 'Ausführung (Ziffer 3)
        Private m_strZZNULA As String 'Nutz-Aufliegelast (Ziffer 9) 
        Private m_strZZTYP As String 'Typ Schlüssel (Ziffer 3)  
        Private m_strZZKLTXT As String 'Hersteller Klartext (Ziffer 2)  
        Private m_strZZTYPA As String 'Typ und Ausführungsart (Ziffer 3)
        Private m_strZZABEKZ As String 'Prüfziffer (Ziffer 3)  
        Private m_strZZTANK As String 'Tankinhalt in 100 Liter (Ziffer 10)
        Private m_strZZSTPL As String 'Stehplätze (Ziffer 11)
        Private m_strZZAUFB As String 'Schlüssel Aufbauart Ergänzung (Ziffer 1)
        Private m_strZZFARBE As String 'Farbziffer des Fahrzeugs (Ziffer 32)
        Private m_strZZBEXX As String 'Text der Länge 22x27 (Ziffer 33)
        Private m_strZZAUFTXT As String 'Text für Aufbauart (Ziffer 1)
        Private m_strZZEARTX As String 'Text für Fahrzeugart (Ziffer 1)
        Private m_strZZTYPE_TEXT As String 'Kurztext zur Antriebsart (Ziffer 5)
        '---------------------------------------------------------
        Private m_ZZABGASRICHTL_TG As String ' Abgasrichtlinie TG
        Private m_ZZACHSL_A1_STA As String ' max. Achslast Achse 1im Mitgl.Staat
        Private m_ZZACHSL_A2_STA As String ' max. Achslast Achse 2 im Mitgl
        Private m_ZZACHSL_A3_STA As String ' max. Achslast Achse 3im Mitgl
        Private m_ZZACHSLST_ACHSE1 As String ' max. Achslast Achse 1
        Private m_ZZACHSLST_ACHSE2 As String ' max. Achslast Achse 2
        Private m_ZZACHSLST_ACHSE3 As String ' max. Achslast Achse 3
        Private m_ZZANHLAST_GEBR As String ' Anhängerlast gebremst
        Private m_ZZANHLAST_UNGEBR As String ' Anängerlast ungebremst
        Private m_ZZANTRIEBSACHS As String ' Anzahl der Antriebsachsen
        Private m_ZZANZACHS As String ' Anzahl der Achsen
        Private m_ZZANZSITZE As String ' Anzahl Sitze
        Private m_ZZANZSTEHPLAETZE As String ' Anzahl Stehplätze
        Private m_ZZBEIUMDREH As String ' Umdrehungen zur Nennleistung
        Private m_ZZBEMER1 As String ' Bemerkungen
        Private m_ZZBEMER10 As String ' Bemerkungen
        Private m_ZZBEMER11 As String ' Bemerkungen
        Private m_ZZBEMER12 As String ' Bemerkungen
        Private m_ZZBEMER13 As String ' Bemerkungen
        Private m_ZZBEMER14 As String ' Bemerkungen
        Private m_ZZBEMER2 As String ' Bemerkungen
        Private m_ZZBEMER3 As String ' Bemerkungen
        Private m_ZZBEMER4 As String ' Bemerkungen
        Private m_ZZBEMER5 As String ' Bemerkungen
        Private m_ZZBEMER6 As String ' Bemerkungen
        Private m_ZZBEMER7 As String ' Bemerkungen
        Private m_ZZBEMER8 As String ' Bemerkungen
        Private m_ZZBEMER9 As String ' Bemerkungen
        Private m_ZZBEREIFACHSE1 As String ' Bereifung Achse 1
        Private m_ZZBEREIFACHSE2 As String ' Bereifung Achse 2
        Private m_ZZBEREIFACHSE3 As String ' Bereifung Achse 3
        Private m_ZZBREITEMAX As String ' Breite Max
        Private m_ZZBREITEMIN As String ' Breite Min
        Private m_ZZCO2KOMBI As String ' Co2 Gehalt in g/km
        Private m_ZZCODE_AUFBAU As String ' Code Aufbau
        Private m_ZZCODE_KRAFTSTOF As String ' Kraftstoff Code
        Private m_ZZDREHZSTANDGER As String ' Drehzahl zu Standgeräusch
        Private m_ZZFABRIKNAME As String ' Fabrikname
        Private m_ZZFAHRGERAEUSCH As String ' Fahrgeräusch
        Private m_ZZFAHRZEUGKLASSE As String ' Fahrzeugklasse
        Private m_ZZFASSVERMOEGEN As String ' Fassungsvermögen bei Tankfahrzeugen
        Private m_ZZFHRZKLASSE_TXT As String ' Fahrzeugklasse Text
        Private m_ZZGENEHMIGDAT As String ' Genehmigungs Datum
        Private m_ZZGENEHMIGNR As String ' Genehmigungsnummer
        Private m_ZZHANDELSNAME As String ' Handelsname
        Private m_ZZHERST_TEXT As String ' Hersteller Kurzbezeichnung
        Private m_ZZHERSTELLER_SCH As String ' Hersteller Schlüssel
        Private m_ZZHOECHSTGESCHW As String ' Höchstgeschwindigkeit
        Private m_ZZHOEHEMAX As String ' Höhe Max
        Private m_ZZHOEHEMIN As String ' Höhe Min
        Private m_ZZHUBRAUM As String ' Hubraum
        Private m_ZZKLARTEXT_TYP As String ' Klartext Typ
        Private m_ZZKRAFTSTOFF_TXT As String ' Kraftstoffart Text
        Private m_ZZLAENGEMAX As String ' Länge Max
        Private m_ZZLAENGEMIN As String ' Länge Min
        Private m_ZZLEISTUNGSGEW As String ' Leistungsgewicht
        Private m_ZZMASSEFAHRBMAX As String ' Masse fahbereit Max
        Private m_ZZMASSEFAHRBMIN As String ' Masse fahrbereit Min
        Private m_ZZNATIONALE_EMIK As String ' Nationale Emisionsklasse
        Private m_ZZNENNLEISTUNG As String ' Nennleistung in KW
        Private m_ZZSLD As String ' Code nat. Emiklasse
        Private m_ZZSTANDGERAEUSCH As String ' Standgeräusch
        Private m_ZZSTUETZLAST As String ' Stützlast
        Private m_ZZTEXT_AUFBAU As String ' Text Aufbau
        Private m_ZZTYP_SCHL As String ' Typ Schlüssel
        Private m_ZZTYP_VVS_PRUEF As String ' VVS Prüfziffer
        Private m_ZZVARIANTE As String ' Variante
        Private m_ZZVERSION As String ' Version
        Private m_ZZVVS_SCHLUESSEL As String ' VVS Schlüssel
        Private m_ZZZULGESGEW As String ' zulässiges Gesamtgewicht
        Private m_ZZZULGESGEWSTAAT As String ' zulässiges Gesamtgewicht im Mitgl.Staat
        Private m_ZZFARBE_KLAR As String 'Fahrzeugfarbe in Klartext
#End Region

#Region " Properties"
        Public Property Fahrgestellnummer() As String
            Get
                Return m_strZZCHASSIS_NUM
            End Get
            Set(ByVal Value As String)
                m_strZZCHASSIS_NUM = Value
            End Set
        End Property

        Public Property FahrgestellnummerPruefziffer() As String
            Get
                Return m_strZZPRFZ
            End Get
            Set(ByVal Value As String)
                m_strZZPRFZ = Value
            End Set
        End Property

        Public Property TagDerErstenZulassung() As String
            Get
                Return m_strZZREPLA_DATE
            End Get
            Set(ByVal Value As String)
                m_strZZREPLA_DATE = Value
            End Set
        End Property

        Public Property HoeheDesFahrzeugs() As String 'Höhe des Fahrzeugs (Ziffer 13)
            Get
                Return m_strZZFLEET_HGT
            End Get
            Set(ByVal Value As String)
                m_strZZFLEET_HGT = Value
            End Set
        End Property

        Public Property BreiteDesFahrzeugs() As String 'ZZFLEET_WID (Ziffer 13)
            Get
                Return m_strZZFLEET_WID
            End Get
            Set(ByVal Value As String)
                m_strZZFLEET_WID = Value
            End Set
        End Property

        Public Property LaengeDesFahrzeugs() As String 'ZZFLEET_LEN (Ziffer 13)
            Get
                Return m_strZZFLEET_LEN
            End Get
            Set(ByVal Value As String)
                m_strZZFLEET_LEN = Value
            End Set
        End Property

        Public Property AnzahlDerAchsenDesFahrzeugs() As String 'ZZNUM_AXLE (Ziffer 18)
            Get
                Return m_strZZNUM_AXLE
            End Get
            Set(ByVal Value As String)
                m_strZZNUM_AXLE = Value
            End Set
        End Property

        Public Property MaximalAnzahlPersonen() As String 'ZZMAX_OCCUPANTS (Ziffer 12)
            Get
                Return m_strZZMAX_OCCUPANTS
            End Get
            Set(ByVal Value As String)
                m_strZZMAX_OCCUPANTS = Value
            End Set
        End Property

        Public Property Antriebsart() As String 'ZZENGINE_TYPE (Ziffer 5)
            Get
                Return m_strZZENGINE_TYPE
            End Get
            Set(ByVal Value As String)
                m_strZZENGINE_TYPE = Value
            End Set
        End Property

        Public Property Leistung() As String
            'ZZENGINE_POWER (Ziffer 7)
            'ZZREVOLUTIONS (Ziffer 7)
            'ZZUNIT_POWER (Ziffer 7)
            Get
                Return m_strZZENGINE_POWER
            End Get
            Set(ByVal Value As String)
                m_strZZENGINE_POWER = Value
            End Set
        End Property

        Public Property Hubraum() As String 'ZZENGINE_CAP (Ziffer 8)
            Get
                Return m_strZZENGINE_CAP
            End Get
            Set(ByVal Value As String)
                m_strZZENGINE_CAP = Value
            End Set
        End Property

        Public Property ZulaessigesGesamtgewicht() As String 'ZZGROSS_WGT (Ziffer 15)
            Get
                Return m_strZZGROSS_WGT
            End Get
            Set(ByVal Value As String)
                m_strZZGROSS_WGT = Value
            End Set
        End Property

        Public Property Fahrzeugart() As String 'ZZFLEET_CAT (Ziffer 1)
            Get
                Return m_strZZFLEET_CAT
            End Get
            Set(ByVal Value As String)
                m_strZZFLEET_CAT = Value
            End Set
        End Property

        Public Property IdentifikationsnummerFuerFahrzeug() As String 'ZZFLEET_VIN (Ziffer 2)
            Get
                Return m_strZZFLEET_VIN
            End Get
            Set(ByVal Value As String)
                m_strZZFLEET_VIN = Value
            End Set
        End Property

        Public Property Hoechstgeschwindigkeit() As String 'ZZSPEED_MAX (Ziffer 6)
            Get
                Return m_strZZSPEED_MAX
            End Get
            Set(ByVal Value As String)
                m_strZZSPEED_MAX = Value
            End Set
        End Property

        Public Property Leergewicht() As String 'ZZLGEW (Ziffer 14)
            Get
                Return m_strZZLGEW
            End Get
            Set(ByVal Value As String)
                m_strZZLGEW = Value
            End Set
        End Property

        Public Property AchslastVorn() As String 'ZZALVO (Ziffer 16)
            Get
                Return m_strZZALVO
            End Get
            Set(ByVal Value As String)
                m_strZZALVO = Value
            End Set
        End Property

        Public Property AchslastHinten() As String 'ZZALHI (Ziffer 16)
            Get
                Return m_strZZALHI
            End Get
            Set(ByVal Value As String)
                m_strZZALHI = Value
            End Set
        End Property

        Public Property AchslastMitte() As String 'ZZALMI (Ziffer 16)
            Get
                Return m_strZZALMI
            End Get
            Set(ByVal Value As String)
                m_strZZALMI = Value
            End Set
        End Property

        Public Property DavonAngetriebeneAchsen() As String 'ZZANGE (Ziffer 19)
            Get
                Return m_strZZANGE
            End Get
            Set(ByVal Value As String)
                m_strZZANGE = Value
            End Set
        End Property

        Public Property BereifunsgroesseVorn() As String 'ZZREVO (Ziffer 20)
            Get
                Return m_strZZREVO
            End Get
            Set(ByVal Value As String)
                m_strZZREVO = Value
            End Set
        End Property

        Public Property BereifunsgroesseMitte_Hinten() As String 'ZZREHI (Ziffer 21)
            Get
                Return m_strZZREHI
            End Get
            Set(ByVal Value As String)
                m_strZZREHI = Value
            End Set
        End Property

        Public Property OderBereifunsgroesseVorn() As String 'ZZORVO (Ziffer 22)
            Get
                Return m_strZZORVO
            End Get
            Set(ByVal Value As String)
                m_strZZORVO = Value
            End Set
        End Property

        Public Property OderBereifunsgroesseMitte_Hinten() As String 'ZZORHI (Ziffer 23)
            Get
                Return m_strZZORHI
            End Get
            Set(ByVal Value As String)
                m_strZZORHI = Value
            End Set
        End Property

        Public Property AnhaengerKupplungPruefzeichen() As String 'ZZAKPZ (Ziffer 27)
            Get
                Return m_strZZAKPZ
            End Get
            Set(ByVal Value As String)
                m_strZZAKPZ = Value
            End Set
        End Property

        Public Property AnhaengerlastGebremst() As String 'ZZALBR (Ziffer 28)
            Get
                Return m_strZZALBR
            End Get
            Set(ByVal Value As String)
                m_strZZALBR = Value
            End Set
        End Property

        Public Property AnhaengerlastUngebremst() As String 'ZZALUB (Ziffer 29)
            Get
                Return m_strZZALUB
            End Get
            Set(ByVal Value As String)
                m_strZZALUB = Value
            End Set
        End Property

        Public Property StandGeraeusch() As String 'ZZSDB9 (Ziffer 30)
            Get
                Return m_strZZSDB9
            End Get
            Set(ByVal Value As String)
                m_strZZSDB9 = Value
            End Set
        End Property

        Public Property Fahrgeraeusch() As String 'ZZFDB9 (Ziffer 31)
            Get
                Return m_strZZFDB9
            End Get
            Set(ByVal Value As String)
                m_strZZFDB9 = Value
            End Set
        End Property

        Public Property BremsdruckEinleitung() As String 'ZZBDR1 (Ziffer 24)
            Get
                Return m_strZZBDR1
            End Get
            Set(ByVal Value As String)
                m_strZZBDR1 = Value
            End Set
        End Property

        Public Property BremsdruckZweileitung() As String 'ZZBDR2 (Ziffer 25)
            Get
                Return m_strZZBDR2
            End Get
            Set(ByVal Value As String)
                m_strZZBDR2 = Value
            End Set
        End Property

        Public Property Anhaengerkupplung() As String 'ZZAKUP (Ziffer 26)
            Get
                Return m_strZZAKUP
            End Get
            Set(ByVal Value As String)
                m_strZZAKUP = Value
            End Set
        End Property

        Public Property Ausfuehrung() As String 'ZZAUSF (Ziffer 3)
            Get
                Return m_strZZAUSF
            End Get
            Set(ByVal Value As String)
                m_strZZAUSF = Value
            End Set
        End Property

        Public Property NutzAufliegelast() As String 'ZZNULA (Ziffer 9) 
            Get
                Return m_strZZNULA
            End Get
            Set(ByVal Value As String)
                m_strZZNULA = Value
            End Set
        End Property

        Public Property TypSchluessel() As String 'ZZTYP (Ziffer 3)  
            Get
                Return m_strZZTYP
            End Get
            Set(ByVal Value As String)
                m_strZZTYP = Value
            End Set
        End Property

        Public Property HerstellerKlartext() As String 'ZZKLTXT (Ziffer 2)  
            Get
                Return m_strZZKLTXT
            End Get
            Set(ByVal Value As String)
                m_strZZKLTXT = Value
            End Set
        End Property

        Public Property TypUndAusfuehrungsart() As String 'ZZTYPA (Ziffer 3)
            Get
                Return m_strZZTYPA
            End Get
            Set(ByVal Value As String)
                m_strZZTYPA = Value
            End Set
        End Property

        Public Property TypUndAusfuehrungsPruefziffer() As String 'ZZABEKZ (Ziffer 3)  
            Get
                Return m_strZZABEKZ
            End Get
            Set(ByVal Value As String)
                m_strZZABEKZ = Value
            End Set
        End Property

        Public Property Tankinhalt() As String 'ZZTANK (Ziffer 10)
            Get
                Return m_strZZTANK
            End Get
            Set(ByVal Value As String)
                m_strZZTANK = Value
            End Set
        End Property

        Public Property Stehplaetze() As String 'ZZSTPL (Ziffer 11)
            Get
                Return m_strZZSTPL
            End Get
            Set(ByVal Value As String)
                m_strZZSTPL = Value
            End Set
        End Property

        Public Property AufbauartSchluessel() As String 'ZZAUFB (Ziffer 1)
            Get
                Return m_strZZAUFB
            End Get
            Set(ByVal Value As String)
                m_strZZAUFB = Value
            End Set
        End Property

        Public Property Farbziffer() As String 'ZZFARBE (Ziffer 32)
            Get
                Return m_strZZFARBE
            End Get
            Set(ByVal Value As String)
                m_strZZFARBE = Value
            End Set
        End Property

        Public Property Bemerkung() As String 'ZZBEXX - Text der Länge 22x27 (Ziffer 33)
            Get
                Return m_strZZBEXX
            End Get
            Set(ByVal Value As String)
                m_strZZBEXX = Value
            End Set
        End Property

        Public Property AufbauartText() As String 'ZZAUFTXT (Ziffer 1)
            Get
                Return m_strZZAUFTXT
            End Get
            Set(ByVal Value As String)
                m_strZZAUFTXT = Value
            End Set
        End Property

        Public Property FahrzeugartText() As String 'ZZEARTX (Ziffer 1)
            Get
                Return m_strZZEARTX
            End Get
            Set(ByVal Value As String)
                m_strZZEARTX = Value
            End Set
        End Property

        Public Property AntriebsartKurztext() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_strZZTYPE_TEXT
            End Get
            Set(ByVal Value As String)
                m_strZZTYPE_TEXT = Value
            End Set
        End Property
        '--------------------------------------------------------------------------
        Public Property ZZABGASRICHTL_TG() As String
            Get
                Return m_ZZABGASRICHTL_TG
            End Get
            Set(ByVal Value As String)
                m_ZZABGASRICHTL_TG = Value
            End Set
        End Property

        Public Property ZZACHSL_A1_STA() As String
            Get
                Return m_ZZACHSL_A1_STA
            End Get
            Set(ByVal Value As String)
                m_ZZACHSL_A1_STA = Value
            End Set
        End Property

        Public Property ZZACHSL_A2_STA() As String
            Get
                Return m_ZZACHSL_A2_STA
            End Get
            Set(ByVal Value As String)
                m_ZZACHSL_A2_STA = Value
            End Set
        End Property

        Public Property ZZACHSL_A3_STA() As String 'Höhe des Fahrzeugs (Ziffer 13)
            Get
                Return m_ZZACHSL_A3_STA
            End Get
            Set(ByVal Value As String)
                m_ZZACHSL_A3_STA = Value
            End Set
        End Property

        Public Property ZZACHSLST_ACHSE1() As String 'ZZFLEET_WID (Ziffer 13)
            Get
                Return m_ZZACHSLST_ACHSE1
            End Get
            Set(ByVal Value As String)
                m_ZZACHSLST_ACHSE1 = Value
            End Set
        End Property

        Public Property ZZACHSLST_ACHSE2() As String 'ZZFLEET_LEN (Ziffer 13)
            Get
                Return m_ZZACHSLST_ACHSE2
            End Get
            Set(ByVal Value As String)
                m_ZZACHSLST_ACHSE2 = Value
            End Set
        End Property

        Public Property ZZACHSLST_ACHSE3() As String 'ZZNUM_AXLE (Ziffer 18)
            Get
                Return m_ZZACHSLST_ACHSE3
            End Get
            Set(ByVal Value As String)
                m_ZZACHSLST_ACHSE3 = Value
            End Set
        End Property

        Public Property ZZANHLAST_GEBR() As String 'ZZMAX_OCCUPANTS (Ziffer 12)
            Get
                Return m_ZZANHLAST_GEBR
            End Get
            Set(ByVal Value As String)
                m_ZZANHLAST_GEBR = Value
            End Set
        End Property

        Public Property ZZANHLAST_UNGEBR() As String 'ZZENGINE_TYPE (Ziffer 5)
            Get
                Return m_ZZANHLAST_UNGEBR
            End Get
            Set(ByVal Value As String)
                m_ZZANHLAST_UNGEBR = Value
            End Set
        End Property

        Public Property ZZANTRIEBSACHS() As String
            'ZZENGINE_POWER (Ziffer 7)
            'ZZREVOLUTIONS (Ziffer 7)
            'ZZUNIT_POWER (Ziffer 7)
            Get
                Return m_ZZANTRIEBSACHS
            End Get
            Set(ByVal Value As String)
                m_ZZANTRIEBSACHS = Value
            End Set
        End Property

        Public Property ZZANZACHS() As String 'ZZENGINE_CAP (Ziffer 8)
            Get
                Return m_ZZANZACHS
            End Get
            Set(ByVal Value As String)
                m_ZZANZACHS = Value
            End Set
        End Property

        Public Property ZZANZSITZE() As String 'ZZGROSS_WGT (Ziffer 15)
            Get
                Return m_ZZANZSITZE
            End Get
            Set(ByVal Value As String)
                m_ZZANZSITZE = Value
            End Set
        End Property

        Public Property ZZANZSTEHPLAETZE() As String 'ZZFLEET_CAT (Ziffer 1)
            Get
                Return m_ZZANZSTEHPLAETZE
            End Get
            Set(ByVal Value As String)
                m_ZZANZSTEHPLAETZE = Value
            End Set
        End Property

        Public Property ZZBEIUMDREH() As String 'ZZFLEET_VIN (Ziffer 2)
            Get
                Return m_ZZBEIUMDREH
            End Get
            Set(ByVal Value As String)
                m_ZZBEIUMDREH = Value
            End Set
        End Property

        Public Property ZZBEMER1() As String 'ZZSPEED_MAX (Ziffer 6)
            Get
                Return m_ZZBEMER1
            End Get
            Set(ByVal Value As String)
                m_ZZBEMER1 = Value
            End Set
        End Property

        Public Property ZZBEMER2() As String 'ZZLGEW (Ziffer 14)
            Get
                Return m_ZZBEMER2
            End Get
            Set(ByVal Value As String)
                m_ZZBEMER2 = Value
            End Set
        End Property

        Public Property ZZBEMER3() As String 'ZZALVO (Ziffer 16)
            Get
                Return m_ZZBEMER3
            End Get
            Set(ByVal Value As String)
                m_ZZBEMER3 = Value
            End Set
        End Property

        Public Property ZZBEMER4() As String 'ZZALHI (Ziffer 16)
            Get
                Return m_ZZBEMER4
            End Get
            Set(ByVal Value As String)
                m_ZZBEMER4 = Value
            End Set
        End Property

        Public Property ZZBEMER5() As String 'ZZALMI (Ziffer 16)
            Get
                Return m_ZZBEMER5
            End Get
            Set(ByVal Value As String)
                m_ZZBEMER5 = Value
            End Set
        End Property

        Public Property ZZBEMER6() As String 'ZZANGE (Ziffer 19)
            Get
                Return m_ZZBEMER6
            End Get
            Set(ByVal Value As String)
                m_ZZBEMER6 = Value
            End Set
        End Property

        Public Property ZZBEMER7() As String 'ZZREVO (Ziffer 20)
            Get
                Return m_ZZBEMER7
            End Get
            Set(ByVal Value As String)
                m_ZZBEMER7 = Value
            End Set
        End Property

        Public Property ZZBEMER8() As String 'ZZREHI (Ziffer 21)
            Get
                Return m_ZZBEMER8
            End Get
            Set(ByVal Value As String)
                m_ZZBEMER8 = Value
            End Set
        End Property

        Public Property ZZBEMER9() As String 'ZZORVO (Ziffer 22)
            Get
                Return m_ZZBEMER9
            End Get
            Set(ByVal Value As String)
                m_ZZBEMER9 = Value
            End Set
        End Property

        Public Property ZZBEMER10() As String 'ZZORHI (Ziffer 23)
            Get
                Return m_ZZBEMER10
            End Get
            Set(ByVal Value As String)
                m_ZZBEMER10 = Value
            End Set
        End Property

        Public Property ZZBEMER11() As String 'ZZAKPZ (Ziffer 27)
            Get
                Return m_ZZBEMER11
            End Get
            Set(ByVal Value As String)
                m_ZZBEMER11 = Value
            End Set
        End Property

        Public Property ZZBEMER12() As String 'ZZALBR (Ziffer 28)
            Get
                Return m_ZZBEMER12
            End Get
            Set(ByVal Value As String)
                m_ZZBEMER12 = Value
            End Set
        End Property

        Public Property ZZBEMER13() As String 'ZZALUB (Ziffer 29)
            Get
                Return m_ZZBEMER13
            End Get
            Set(ByVal Value As String)
                m_ZZBEMER13 = Value
            End Set
        End Property

        Public Property ZZBEMER14() As String 'ZZSDB9 (Ziffer 30)
            Get
                Return m_ZZBEMER14
            End Get
            Set(ByVal Value As String)
                m_ZZBEMER14 = Value
            End Set
        End Property

        Public Property ZZBEREIFACHSE1() As String 'ZZFDB9 (Ziffer 31)
            Get
                Return m_ZZBEREIFACHSE1
            End Get
            Set(ByVal Value As String)
                m_ZZBEREIFACHSE1 = Value
            End Set
        End Property

        Public Property ZZBEREIFACHSE2() As String 'ZZBDR1 (Ziffer 24)
            Get
                Return m_ZZBEREIFACHSE2
            End Get
            Set(ByVal Value As String)
                m_ZZBEREIFACHSE2 = Value
            End Set
        End Property

        Public Property ZZBEREIFACHSE3() As String 'ZZBDR2 (Ziffer 25)
            Get
                Return m_ZZBEREIFACHSE3
            End Get
            Set(ByVal Value As String)
                m_ZZBEREIFACHSE3 = Value
            End Set
        End Property

        Public Property ZZBREITEMAX() As String 'ZZAKUP (Ziffer 26)
            Get
                Return m_ZZBREITEMAX
            End Get
            Set(ByVal Value As String)
                m_ZZBREITEMAX = Value
            End Set
        End Property

        Public Property ZZBREITEMIN() As String 'ZZAUSF (Ziffer 3)
            Get
                Return m_ZZBREITEMIN
            End Get
            Set(ByVal Value As String)
                m_ZZBREITEMIN = Value
            End Set
        End Property

        Public Property ZZCO2KOMBI() As String 'ZZNULA (Ziffer 9) 
            Get
                Return m_ZZCO2KOMBI
            End Get
            Set(ByVal Value As String)
                m_ZZCO2KOMBI = Value
            End Set
        End Property

        Public Property ZZCODE_AUFBAU() As String 'ZZTYP (Ziffer 3)  
            Get
                Return m_ZZCODE_AUFBAU
            End Get
            Set(ByVal Value As String)
                m_ZZCODE_AUFBAU = Value
            End Set
        End Property

        Public Property ZZCODE_KRAFTSTOF() As String 'ZZKLTXT (Ziffer 2)  
            Get
                Return m_ZZCODE_KRAFTSTOF
            End Get
            Set(ByVal Value As String)
                m_ZZCODE_KRAFTSTOF = Value
            End Set
        End Property

        Public Property ZZDREHZSTANDGER() As String 'ZZTYPA (Ziffer 3)
            Get
                Return m_ZZDREHZSTANDGER
            End Get
            Set(ByVal Value As String)
                m_ZZDREHZSTANDGER = Value
            End Set
        End Property

        Public Property ZZFABRIKNAME() As String 'ZZABEKZ (Ziffer 3)  
            Get
                Return m_ZZFABRIKNAME
            End Get
            Set(ByVal Value As String)
                m_ZZFABRIKNAME = Value
            End Set
        End Property

        Public Property ZZFAHRGERAEUSCH() As String 'ZZTANK (Ziffer 10)
            Get
                Return m_ZZFAHRGERAEUSCH
            End Get
            Set(ByVal Value As String)
                m_ZZFAHRGERAEUSCH = Value
            End Set
        End Property

        Public Property ZZFAHRZEUGKLASSE() As String 'ZZSTPL (Ziffer 11)
            Get
                Return m_ZZFAHRZEUGKLASSE
            End Get
            Set(ByVal Value As String)
                m_ZZFAHRZEUGKLASSE = Value
            End Set
        End Property

        Public Property ZZFASSVERMOEGEN() As String 'ZZAUFB (Ziffer 1)
            Get
                Return m_ZZFASSVERMOEGEN
            End Get
            Set(ByVal Value As String)
                m_ZZFASSVERMOEGEN = Value
            End Set
        End Property

        Public Property ZZFHRZKLASSE_TXT() As String 'ZZFARBE (Ziffer 32)
            Get
                Return m_ZZFHRZKLASSE_TXT
            End Get
            Set(ByVal Value As String)
                m_ZZFHRZKLASSE_TXT = Value
            End Set
        End Property

        Public Property ZZGENEHMIGDAT() As String 'ZZBEXX - Text der Länge 22x27 (Ziffer 33)
            Get
                Return m_ZZGENEHMIGDAT
            End Get
            Set(ByVal Value As String)
                m_ZZGENEHMIGDAT = Value
            End Set
        End Property

        Public Property ZZGENEHMIGNR() As String 'ZZAUFTXT (Ziffer 1)
            Get
                Return m_ZZGENEHMIGNR
            End Get
            Set(ByVal Value As String)
                m_ZZGENEHMIGNR = Value
            End Set
        End Property

        Public Property ZZHANDELSNAME() As String 'ZZEARTX (Ziffer 1)
            Get
                Return m_ZZHANDELSNAME
            End Get
            Set(ByVal Value As String)
                m_ZZHANDELSNAME = Value
            End Set
        End Property

        Public Property ZZHERST_TEXT() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZHERST_TEXT
            End Get
            Set(ByVal Value As String)
                m_ZZHERST_TEXT = Value
            End Set
        End Property

        Public Property ZZHERSTELLER_SCH() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZHERSTELLER_SCH
            End Get
            Set(ByVal Value As String)
                m_ZZHERSTELLER_SCH = Value
            End Set
        End Property

        Public Property ZZHOECHSTGESCHW() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZHOECHSTGESCHW
            End Get
            Set(ByVal Value As String)
                m_ZZHOECHSTGESCHW = Value
            End Set
        End Property

        Public Property ZZHOEHEMAX() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZHOEHEMAX
            End Get
            Set(ByVal Value As String)
                m_ZZHOEHEMAX = Value
            End Set
        End Property

        Public Property ZZHOEHEMIN() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZHOEHEMIN
            End Get
            Set(ByVal Value As String)
                m_ZZHOEHEMIN = Value
            End Set
        End Property

        Public Property ZZHUBRAUM() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZHUBRAUM
            End Get
            Set(ByVal Value As String)
                m_ZZHUBRAUM = Value
            End Set
        End Property

        Public Property ZZKLARTEXT_TYP() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZKLARTEXT_TYP
            End Get
            Set(ByVal Value As String)
                m_ZZKLARTEXT_TYP = Value
            End Set
        End Property
        Public Property ZZKRAFTSTOFF_TXT() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZKRAFTSTOFF_TXT
            End Get
            Set(ByVal Value As String)
                m_ZZKRAFTSTOFF_TXT = Value
            End Set
        End Property
        Public Property ZZLAENGEMAX() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZLAENGEMAX
            End Get
            Set(ByVal Value As String)
                m_ZZLAENGEMAX = Value
            End Set
        End Property
        Public Property ZZLAENGEMIN() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZLAENGEMIN
            End Get
            Set(ByVal Value As String)
                m_ZZLAENGEMIN = Value
            End Set
        End Property
        Public Property ZZLEISTUNGSGEW() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZLEISTUNGSGEW
            End Get
            Set(ByVal Value As String)
                m_ZZLEISTUNGSGEW = Value
            End Set
        End Property
        Public Property ZZMASSEFAHRBMAX() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZMASSEFAHRBMAX
            End Get
            Set(ByVal Value As String)
                m_ZZMASSEFAHRBMAX = Value
            End Set
        End Property

        Public Property ZZMASSEFAHRBMIN() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZMASSEFAHRBMIN
            End Get
            Set(ByVal Value As String)
                m_ZZMASSEFAHRBMIN = Value
            End Set
        End Property

        Public Property ZZNATIONALE_EMIK() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZNATIONALE_EMIK
            End Get
            Set(ByVal Value As String)
                m_ZZNATIONALE_EMIK = Value
            End Set
        End Property
        Public Property ZZNENNLEISTUNG() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZNENNLEISTUNG
            End Get
            Set(ByVal Value As String)
                m_ZZNENNLEISTUNG = Value
            End Set
        End Property
        Public Property ZZSLD() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZSLD
            End Get
            Set(ByVal Value As String)
                m_ZZSLD = Value
            End Set
        End Property
        Public Property ZZSTANDGERAEUSCH() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZSTANDGERAEUSCH
            End Get
            Set(ByVal Value As String)
                m_ZZSTANDGERAEUSCH = Value
            End Set
        End Property
        Public Property ZZSTUETZLAST() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZSTUETZLAST
            End Get
            Set(ByVal Value As String)
                m_ZZSTUETZLAST = Value
            End Set
        End Property
        Public Property ZZTEXT_AUFBAU() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZTEXT_AUFBAU
            End Get
            Set(ByVal Value As String)
                m_ZZTEXT_AUFBAU = Value
            End Set
        End Property

        Public Property ZZTYP_SCHL() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZTYP_SCHL
            End Get
            Set(ByVal Value As String)
                m_ZZTYP_SCHL = Value
            End Set
        End Property
        Public Property ZZTYP_VVS_PRUEF() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZTYP_VVS_PRUEF
            End Get
            Set(ByVal Value As String)
                m_ZZTYP_VVS_PRUEF = Value
            End Set
        End Property
        Public Property ZZVARIANTE() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZVARIANTE
            End Get
            Set(ByVal Value As String)
                m_ZZVARIANTE = Value
            End Set
        End Property
        Public Property ZZVERSION() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZVERSION
            End Get
            Set(ByVal Value As String)
                m_ZZVERSION = Value
            End Set
        End Property
        Public Property ZZVVS_SCHLUESSEL() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZVVS_SCHLUESSEL
            End Get
            Set(ByVal Value As String)
                m_ZZVVS_SCHLUESSEL = Value
            End Set
        End Property
        Public Property ZZZULGESGEW() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZZULGESGEW
            End Get
            Set(ByVal Value As String)
                m_ZZZULGESGEW = Value
            End Set
        End Property
        Public Property ZZZULGESGEWSTAAT() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZZULGESGEWSTAAT
            End Get
            Set(ByVal Value As String)
                m_ZZZULGESGEWSTAAT = Value
            End Set
        End Property
        'm_ZZFARBE_KLAR
        Public Property ZZFARBE() As String 'ZZTYPE_TEXT (Ziffer 5)
            Get
                Return m_ZZFARBE_KLAR
            End Get
            Set(ByVal Value As String)
                m_ZZFARBE_KLAR = Value
            End Set
        End Property
#End Region

#Region " Methods"
        Public Sub New()
            MyBase.New()

            m_strZZCHASSIS_NUM = ""
            m_strZZPRFZ = ""
            m_strZZREPLA_DATE = ""
            m_strZZFLEET_HGT = ""
            m_strZZFLEET_WID = ""
            m_strZZFLEET_LEN = ""
            m_strZZNUM_AXLE = ""
            m_strZZMAX_OCCUPANTS = ""
            m_strZZENGINE_TYPE = ""
            m_strZZENGINE_POWER = ""
            m_strZZREVOLUTIONS = ""
            m_strZZENGINE_CAP = ""
            m_strZZGROSS_WGT = ""
            m_strZZFLEET_CAT = ""
            m_strZZFLEET_VIN = ""
            m_strZZUNIT_POWER = ""
            m_strZZSPEED_MAX = ""
            m_strZZLGEW = ""
            m_strZZALVO = ""
            m_strZZALHI = ""
            m_strZZALMI = ""
            m_strZZANGE = ""
            m_strZZREVO = ""
            m_strZZREHI = ""
            m_strZZORVO = ""
            m_strZZORHI = ""
            m_strZZAKPZ = ""
            m_strZZALBR = ""
            m_strZZALUB = ""
            m_strZZSDB9 = ""
            m_strZZFDB9 = ""
            m_strZZBDR1 = ""
            m_strZZBDR2 = ""
            m_strZZAKUP = ""
            m_strZZAUSF = ""
            m_strZZNULA = ""
            m_strZZTYP = ""
            m_strZZKLTXT = ""
            m_strZZTYPA = ""
            m_strZZABEKZ = ""
            m_strZZTANK = ""
            m_strZZSTPL = ""
            m_strZZAUFB = ""
            m_strZZFARBE = ""
            m_strZZBEXX = ""
            m_strZZAUFTXT = ""
            m_strZZEARTX = ""
            m_strZZTYPE_TEXT = ""
            '-------------- NEUE ABE-Daten ab Okt. 2005 ------------------------------------
            m_ZZABGASRICHTL_TG = ""
            m_ZZACHSL_A1_STA = ""
            m_ZZACHSL_A2_STA = ""
            m_ZZACHSL_A3_STA = ""
            m_ZZACHSLST_ACHSE1 = ""
            m_ZZACHSLST_ACHSE2 = ""
            m_ZZACHSLST_ACHSE3 = ""
            m_ZZANHLAST_GEBR = ""
            m_ZZANHLAST_UNGEBR = ""
            m_ZZANTRIEBSACHS = ""
            m_ZZANZACHS = ""
            m_ZZANZSITZE = ""
            m_ZZANZSTEHPLAETZE = ""
            m_ZZBEIUMDREH = ""
            m_ZZBEMER1 = ""
            m_ZZBEMER10 = ""
            m_ZZBEMER11 = ""
            m_ZZBEMER12 = ""
            m_ZZBEMER13 = ""
            m_ZZBEMER14 = ""
            m_ZZBEMER2 = ""
            m_ZZBEMER3 = ""
            m_ZZBEMER4 = ""
            m_ZZBEMER5 = ""
            m_ZZBEMER6 = ""
            m_ZZBEMER7 = ""
            m_ZZBEMER8 = ""
            m_ZZBEMER9 = ""
            m_ZZBEREIFACHSE1 = ""
            m_ZZBEREIFACHSE2 = ""
            m_ZZBEREIFACHSE3 = ""
            m_ZZBREITEMAX = ""
            m_ZZBREITEMIN = ""
            m_ZZCO2KOMBI = ""
            m_ZZCODE_AUFBAU = ""
            m_ZZCODE_KRAFTSTOF = ""
            m_ZZDREHZSTANDGER = ""
            m_ZZFABRIKNAME = ""
            m_ZZFAHRGERAEUSCH = ""
            m_ZZFAHRZEUGKLASSE = ""
            m_ZZFASSVERMOEGEN = ""
            m_ZZFHRZKLASSE_TXT = ""
            m_ZZGENEHMIGDAT = ""
            m_ZZGENEHMIGNR = ""
            m_ZZHANDELSNAME = ""
            m_ZZHERST_TEXT = ""
            m_ZZHERSTELLER_SCH = ""
            m_ZZHOECHSTGESCHW = ""
            m_ZZHOEHEMAX = ""
            m_ZZHOEHEMIN = ""
            m_ZZHUBRAUM = ""
            m_ZZKLARTEXT_TYP = ""
            m_ZZKRAFTSTOFF_TXT = ""
            m_ZZLAENGEMAX = ""
            m_ZZLAENGEMIN = ""
            m_ZZLEISTUNGSGEW = ""
            m_ZZMASSEFAHRBMAX = ""
            m_ZZMASSEFAHRBMIN = ""
            m_ZZNATIONALE_EMIK = ""
            m_ZZNENNLEISTUNG = ""
            m_ZZSLD = ""
            m_ZZSTANDGERAEUSCH = ""
            m_ZZSTUETZLAST = ""
            m_ZZTEXT_AUFBAU = ""
            m_ZZTYP_SCHL = ""
            m_ZZTYP_VVS_PRUEF = ""
            m_ZZVARIANTE = ""
            m_ZZVERSION = ""
            m_ZZVVS_SCHLUESSEL = ""
            m_ZZZULGESGEW = ""
            m_ZZZULGESGEWSTAAT = ""
            m_ZZFARBE_KLAR = ""
        End Sub
#End Region
    End Class
End Namespace

' ************************************************
' $History: ABE2FZG.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Business
' 
' *****************  Version 4  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Business
' 
' ************************************************