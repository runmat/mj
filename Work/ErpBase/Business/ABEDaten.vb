Option Explicit On
Option Infer On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Common
Imports CKG.Base.Kernel.Common

Namespace Business
    Public Class ABEDaten
        REM § Lesefunktion, Kunde: Verschiedene, 
        REM § ShowDatenABE - BAPI: Z_M_Abezufzg,

        Inherits BankBase

#Region " Declarations"
        Private m_intErrorCount As Int32
        Private m_objABE_Daten As ABE2FHZG
        Private m_dsPDI_Data As DataSet
        Private m_dsPDI_Data_Selected As DataSet
        Private m_tblPDIs As DataTable
        Private m_tblAllPDIs As DataTable
        Private m_tblModelle As DataTable
        Private m_tblFahrzeuge As DataTable
        Private m_strPDINummer As String
        Private m_strPDINummerSuche As String
        Private m_strModellCode As String
        Private m_strFahrgestellnummer As String
        Private m_intLastID As Int32
        Private m_strTask As String
        Private m_intSelectedCars As Int32
        Private m_blnShowBelegnummer As Boolean
        Private m_intFahrzeugeGesamtZulassungsf As Int32
        Private m_intFahrzeugeGesamtGesperrt As Int32
        Private m_tblErledigt As DataTable
        Private data As ArrayList
        Private returnList As ArrayList
#End Region

#Region " Properties"
        Public ReadOnly Property ErrorCount() As Int32
            Get
                Return m_intErrorCount
            End Get
        End Property

        Public ReadOnly Property Erledigt() As DataTable
            Get
                Return m_tblErledigt
            End Get
        End Property

        Public ReadOnly Property FahrzeugeGesamtZulassungsf() As Int32
            Get
                Return m_intFahrzeugeGesamtZulassungsf
            End Get
        End Property

        Public ReadOnly Property FahrzeugeGesamtGesperrt() As Int32
            Get
                Return m_intFahrzeugeGesamtGesperrt
            End Get
        End Property

        Public Property ABE_Daten() As ABE2FHZG
            Get
                Return m_objABE_Daten
            End Get
            Set(ByVal Value As ABE2FHZG)
                m_objABE_Daten = Value
            End Set
        End Property

        Public ReadOnly Property AllPDIs() As DataTable
            Get
                Return m_tblAllPDIs
            End Get
        End Property

        Public ReadOnly Property PDI_Data_Selected() As DataSet
            Get
                Return m_dsPDI_Data_Selected
            End Get
        End Property

        Public ReadOnly Property ShowBelegnummer() As Boolean
            Get
                Return m_blnShowBelegnummer
            End Get
        End Property

        Public ReadOnly Property PDI_Data() As DataSet
            Get
                Return m_dsPDI_Data
            End Get
        End Property

        Public ReadOnly Property Task() As String
            Get
                Return m_strTask
            End Get
        End Property

        Public Property PDINummer() As String
            Get
                Return m_strPDINummer
            End Get
            Set(ByVal Value As String)
                m_strPDINummer = Value
            End Set
        End Property

        Public Property PDINummerSuche() As String
            Get
                Return m_strPDINummerSuche
            End Get
            Set(ByVal Value As String)
                m_strPDINummerSuche = Value
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

        Public Property ModellCode() As String
            Get
                Return m_strModellCode
            End Get
            Set(ByVal Value As String)
                m_strModellCode = Value
            End Set
        End Property

        Public ReadOnly Property SelectedCars() As Int32
            Get
                Return m_intSelectedCars
            End Get
        End Property

        Public ReadOnly Property confirmList() As ArrayList
            Get
                Return returnList
            End Get
        End Property
#End Region

#Region " Methods"
        Public Sub New(ByRef objUser As Kernel.Security.User, ByRef objApp As Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String, ByRef strCustomer As String, ByRef strCreditControlArea As String, ByRef strPDINummerSuche As String, ByRef strTask As String)
            MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
            If Not m_blnGestartet Then
                m_blnGestartet = True

                m_intErrorCount = 0
                m_intLastID = -1

                m_strModellCode = ""
                m_strFahrgestellnummer = ""
                m_strPDINummer = ""
                m_intSelectedCars = 0
                m_blnShowBelegnummer = False

                m_strCustomer = Right("0000000000" & strCustomer, 10)
                m_strCreditControlArea = strCreditControlArea
                m_strPDINummerSuche = strPDINummerSuche
                m_strTask = strTask

                m_objABE_Daten = New ABE2FHZG()

                m_tblAllPDIs = New DataTable("AllPDIs")
                m_tblAllPDIs.Columns.Add("PDI_Nummer", System.Type.GetType("System.String"))
                m_tblAllPDIs.Columns.Add("Kunden_PDI_Nummer", System.Type.GetType("System.String"))
                m_tblAllPDIs.Columns.Add("PDI_Name", System.Type.GetType("System.String"))
                m_tblAllPDIs.Columns.Add("Zulassungsf_Fahrzeuge", System.Type.GetType("System.Int32"))
                m_tblAllPDIs.Columns.Add("Gesperrte_Fahrzeuge", System.Type.GetType("System.Int32"))
                m_tblAllPDIs.Columns.Add("Details", System.Type.GetType("System.Boolean"))
                m_tblAllPDIs.Columns.Add("Loaded", System.Type.GetType("System.Boolean"))

                m_dsPDI_Data = New DataSet()

                m_tblPDIs = New DataTable("PDIs")
                m_tblPDIs.Columns.Add("PDI_Nummer", System.Type.GetType("System.String"))
                m_tblPDIs.Columns.Add("Kunden_PDI_Nummer", System.Type.GetType("System.String"))
                m_tblPDIs.Columns.Add("PDI_Name", System.Type.GetType("System.String"))
                m_tblPDIs.Columns.Add("Zulassungsf_Fahrzeuge", System.Type.GetType("System.Int32"))
                m_tblPDIs.Columns.Add("Gesperrte_Fahrzeuge", System.Type.GetType("System.Int32"))
                m_tblPDIs.Columns.Add("Details", System.Type.GetType("System.Boolean"))
                m_tblPDIs.Columns.Add("Loaded", System.Type.GetType("System.Boolean"))

                m_tblModelle = New DataTable("Modelle")
                m_tblModelle.Columns.Add("ID", System.Type.GetType("System.Int32"))
                m_tblModelle.Columns.Add("PDI_Nummer", System.Type.GetType("System.String"))
                m_tblModelle.Columns.Add("Kunden_PDI_Nummer", System.Type.GetType("System.String"))
                m_tblModelle.Columns.Add("PDI_Name", System.Type.GetType("System.String"))
                m_tblModelle.Columns.Add("Hersteller", System.Type.GetType("System.String"))
                m_tblModelle.Columns.Add("Modellbezeichnung", System.Type.GetType("System.String"))
                m_tblModelle.Columns.Add("Schaltung", System.Type.GetType("System.String"))
                m_tblModelle.Columns.Add("Ausfuehrung", System.Type.GetType("System.String"))
                m_tblModelle.Columns.Add("Antrieb", System.Type.GetType("System.String"))
                m_tblModelle.Columns.Add("Bereifung", System.Type.GetType("System.String"))
                m_tblModelle.Columns.Add("Navigation", System.Type.GetType("System.String"))
                m_tblModelle.Columns.Add("Beklebung", System.Type.GetType("System.Boolean"))
                m_tblModelle.Columns.Add("BeklebungAlsText", System.Type.GetType("System.String"))
                m_tblModelle.Columns.Add("Sperre", System.Type.GetType("System.Boolean"))
                m_tblModelle.Columns.Add("SperreAlsText", System.Type.GetType("System.String"))
                m_tblModelle.Columns.Add("VM", System.Type.GetType("System.Boolean"))
                m_tblModelle.Columns.Add("Limo", System.Type.GetType("System.Boolean"))
                m_tblModelle.Columns.Add("Anzahl_alt", System.Type.GetType("System.Int32"))
                m_tblModelle.Columns.Add("Anzahl_neu", System.Type.GetType("System.Int32"))
                m_tblModelle.Columns.Add("Bemerkung", System.Type.GetType("System.String"))
                m_tblModelle.Columns.Add("DatumErstzulassung", System.Type.GetType("System.DateTime"))
                m_tblModelle.Columns.Add("ZielPDI", System.Type.GetType("System.String"))
                m_tblModelle.Columns.Add("OrtsKennz", System.Type.GetType("System.String"))

                m_tblModelle.Columns.Add("WK1", System.Type.GetType("System.String"))
                m_tblModelle.Columns.Add("WK2", System.Type.GetType("System.String"))
                m_tblModelle.Columns.Add("WK3", System.Type.GetType("System.String"))

                m_tblModelle.Columns.Add("Task", System.Type.GetType("System.String"))

                m_tblFahrzeuge = New DataTable("Fahrzeuge")
                m_tblFahrzeuge.Columns.Add("Modell_ID", System.Type.GetType("System.Int32"))
                m_tblFahrzeuge.Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                m_tblFahrzeuge.Columns.Add("Meldungsnummer", System.Type.GetType("System.String"))
                m_tblFahrzeuge.Columns.Add("Equipmentnummer", System.Type.GetType("System.String"))
                m_tblFahrzeuge.Columns.Add("Eingangsdatum", System.Type.GetType("System.DateTime"))
                m_tblFahrzeuge.Columns.Add("Ausgewaehlt", System.Type.GetType("System.Boolean"))
                m_tblFahrzeuge.Columns.Add("VM", System.Type.GetType("System.Boolean"))
                m_tblFahrzeuge.Columns.Add("Limo", System.Type.GetType("System.Boolean"))
                m_tblFahrzeuge.Columns.Add("Kennzeichen2zeilig", System.Type.GetType("System.Boolean"))
                m_tblFahrzeuge.Columns.Add("Bemerkung", System.Type.GetType("System.String"))
                m_tblFahrzeuge.Columns.Add("DatumErstzulassung", System.Type.GetType("System.DateTime"))
                m_tblFahrzeuge.Columns.Add("ZielPDI", System.Type.GetType("System.String"))
                m_tblFahrzeuge.Columns.Add("OrtsKennz", System.Type.GetType("System.String"))

                m_tblFahrzeuge.Columns.Add("WK1", System.Type.GetType("System.String"))
                m_tblFahrzeuge.Columns.Add("WK2", System.Type.GetType("System.String"))
                m_tblFahrzeuge.Columns.Add("WK3", System.Type.GetType("System.String"))

                m_tblFahrzeuge.Columns.Add("Belegnummer", System.Type.GetType("System.String"))
                m_dsPDI_Data.Tables.Add(m_tblPDIs)

                m_dsPDI_Data.Tables.Add(m_tblModelle)
                m_dsPDI_Data.Tables.Add(m_tblFahrzeuge)

                Dim dc1 As DataColumn
                Dim dc2 As DataColumn
                'Relation Author => Title
                dc1 = m_dsPDI_Data.Tables("PDIs").Columns("PDI_Nummer")
                dc2 = m_dsPDI_Data.Tables("Modelle").Columns("PDI_Nummer")
                Dim dr As DataRelation = New DataRelation("PDI_Modell", dc1, dc2, False)
                m_dsPDI_Data.Relations.Add(dr)

                'Relation Title => Sales
                dc1 = m_dsPDI_Data.Tables("Modelle").Columns("ID")
                dc2 = m_dsPDI_Data.Tables("Fahrzeuge").Columns("Modell_ID")
                dr = New DataRelation("Modell_Fahrzeug", dc1, dc2, False)
                m_dsPDI_Data.Relations.Add(dr)

                m_blnGestartet = False
            End If
        End Sub

        Public Sub ShowDatenABE(ByVal strEquNr As String)
            m_strClassAndMethod = "ABEDaten.ShowDatenABE"
            If Not m_blnGestartet Then
                m_blnGestartet = True

                Dim strZZCHASSIS_NUM As String = "" 'Fahrgestellnummer (Ziffer 4)
                Dim strZZPRFZ As String = ""  'Fahrgestellnummer-Prüfziffer (Ziffer 4)
                Dim strZZREPLA_DATE As String = ""  'Datum der Erstzulassung (Ziffer 32)
                Dim decZZFLEET_HGT As Decimal  'Höhe des Fahrzeugs (Ziffer 13)
                Dim decZZFLEET_WID As Decimal 'Breite des Fahrzeugs (Ziffer 13)
                Dim decZZFLEET_LEN As Decimal 'Länge des Fahrzeugs (Ziffer 13)
                Dim strZZNUM_AXLE As String = ""  'Anzahl der Achsen des Fahrzeugs (Ziffer 18)
                Dim strZZMAX_OCCUPANTS As String = "" 'Maximal zulässige Anzahl von beförderten Personen (Ziffer 12)
                Dim strZZENGINE_TYPE As String = "" 'Antriebsart (Ziffer 5)
                Dim decZZENGINE_POWER As Decimal 'Leistung bei gegebener Umdrehungszahl (Ziffer 7)
                Dim strZZREVOLUTIONS As String = "" 'Umdrehungszahl pro Minute (Ziffer 7)
                Dim decZZENGINE_CAP As Decimal 'Hubraum (Ziffer 8)
                Dim decZZGROSS_WGT As Decimal 'Zulässiges Gesamtgewicht (Ziffer 15)
                Dim strZZFLEET_CAT As String = "" 'Fahrzeugart (Ziffer 1)
                Dim strZZFLEET_VIN As String = "" 'Identifikationsnummer des Herstellers für das Fahrzeug (Ziffer 2)
                Dim strZZUNIT_POWER As String = "" 'Maßeinheit für die Leistung (Ziffer 7)
                Dim decZZSPEED_MAX As Decimal 'Höchstgeschwindigkeit des Fahrzeugs (Ziffer 6)
                Dim strZZLGEW As String = ""  'Leergewicht (Ziffer 14)
                Dim strZZALVO As String = ""  'Achslast vorn (Ziffer 16)
                Dim strZZALHI As String = ""  'Achslast hinten (Ziffer 16)
                Dim strZZALMI As String = ""  'Achslast mitte (Ziffer 16)
                Dim strZZANGE As String = ""  'davon angetriebene Achsen (Ziffer 19)
                Dim strZZREVO As String = ""  'Bereifunsgrösse vorn (Ziffer 20)
                Dim strZZREHI As String = ""  'Bereifunsgrösse mitte/hinten (Ziffer 21)
                Dim strZZORVO As String = "" 'oder Bereifunsgrösse vorn (Ziffer 22)
                Dim strZZORHI As String = ""  'oder Bereifunsgrösse mitte/hinten (Ziffer 23)
                Dim strZZAKPZ As String = "" 'Anhänger-Kupplung Prüfzeichen (Ziffer 27)
                Dim strZZALBR As String = "" 'Anhängerlast gebremst (Ziffer 28)
                Dim strZZALUB As String = "" 'Anhängerlast ungebremst (Ziffer 29)
                Dim strZZSDB9 As String = "" 'Stand-Geräusch Ziffern (Ziffer 30)
                Dim strZZFDB9 As String = "" 'Fahrgeräusch Ziffern (Ziffer 31)
                Dim strZZBDR1 As String = "" 'Bremsdruck Einleitung Bar * 10 (Ziffer 24)
                Dim strZZBDR2 As String = "" 'Bremsdruck Zweileitung Bar * 10 (Ziffer 25)
                Dim strZZAKUP As String = "" 'Anhängerkupplung DIN 740 (Ziffer 26)
                Dim strZZAUSF As String = ""  'Ausführung (Ziffer 3)
                Dim strZZNULA As String = "" 'Nutz-Aufliegelast (Ziffer 9) 
                Dim strZZTYP As String = "" 'Typ Schlüssel (Ziffer 3)  
                Dim strZZKLTXT As String = "" 'Hersteller Klartext (Ziffer 2)  
                Dim strZZTYPA As String = "" 'Typ und Ausführungsart (Ziffer 3)
                Dim strZZABEKZ As String = "" 'Prüfziffer (Ziffer 3)  
                Dim strZZTANK As String = "" 'Tankinhalt in 100 Liter (Ziffer 10)
                Dim strZZSTPL As String = "" 'Stehplätze (Ziffer 11)
                Dim strZZAUFB As String = "" 'Schlüssel Aufbauart Ergänzung (Ziffer 1)
                Dim strZZFARBE As String = "" 'Farbziffer des Fahrzeugs (Ziffer 32)
                Dim strZZBE01 As String = ""  'Text der Länge 27 (Ziffer 33)   
                Dim strZZBE02 As String = "" 'Text der Länge 27 (Ziffer 33)   
                Dim strZZBE03 As String = "" 'Text der Länge 27 (Ziffer 33)   
                Dim strZZBE04 As String = "" 'Text der Länge 27 (Ziffer 33)   
                Dim strZZBE05 As String = "" 'Text der Länge 27 (Ziffer 33)   
                Dim strZZBE06 As String = "" 'Text der Länge 27 (Ziffer 33)   
                Dim strZZBE07 As String = "" 'Text der Länge 27 (Ziffer 33)   
                Dim strZZBE08 As String = "" 'Text der Länge 27 (Ziffer 33)   
                Dim strZZBE09 As String = "" 'Text der Länge 27 (Ziffer 33)   
                Dim strZZBE10 As String = "" 'Text der Länge 27 (Ziffer 33)   
                Dim strZZBE11 As String = "" 'Text der Länge 27 (Ziffer 33)   
                Dim strZZBE12 As String = "" 'Text der Länge 27 (Ziffer 33)   
                Dim strZZBE13 As String = "" 'Text der Länge 27 (Ziffer 33)   
                Dim strZZBE14 As String = "" 'Text der Länge 27 (Ziffer 33)   
                Dim strZZBE15 As String = ""  'Text der Länge 27 (Ziffer 33)   
                Dim strZZBE16 As String = "" 'Text der Länge 27 (Ziffer 33)   
                Dim strZZBE17 As String = "" 'Text der Länge 27 (Ziffer 33)   
                Dim strZZBE18 As String = "" 'Text der Länge 27 (Ziffer 33)   
                Dim strZZBE19 As String = "" 'Text der Länge 27 (Ziffer 33)   
                Dim strZZBE20 As String = "" 'Text der Länge 27 (Ziffer 33)   
                Dim strZZBE21 As String = "" 'Text der Länge 27 (Ziffer 33)   
                Dim strZZBE22 As String = "" 'Text der Länge 27 (Ziffer 33)
                Dim strZZAUFTXT As String = "" 'Text für Aufbauart (Ziffer 1)
                Dim strZZEARTX As String = "" 'Text für Fahrzeugart (Ziffer 1)
                Dim strZZTYPE_TEXT As String = "" 'Kurztext zur Antriebsart (Ziffer 5)
                '-------- NEUE ABE-Daten ab Okt. 2005 --------------------------------------
                Dim m_ZZABGASRICHTL_TG As String = "" ' Abgasrichtlinie TG
                Dim m_ZZACHSL_A1_STA As String = "" ' max. Achslast Achse 1im Mitgl.Staat
                Dim m_ZZACHSL_A2_STA As String = ""  ' max. Achslast Achse 2 im Mitgl
                Dim m_ZZACHSL_A3_STA As String = "" ' max. Achslast Achse 3im Mitgl
                Dim m_ZZACHSLST_ACHSE1 As String = "" ' max. Achslast Achse 1
                Dim m_ZZACHSLST_ACHSE2 As String = ""  ' max. Achslast Achse 2
                Dim m_ZZACHSLST_ACHSE3 As String = "" ' max. Achslast Achse 3
                Dim m_ZZANHLAST_GEBR As String = "" ' Anhängerlast gebremst
                Dim m_ZZANHLAST_UNGEBR As String = "" ' Anängerlast ungebremst
                Dim m_ZZANTRIEBSACHS As String = ""  ' Anzahl der Antriebsachsen
                Dim m_ZZANZACHS As String = ""  ' Anzahl der Achsen
                Dim m_ZZANZSITZE As String = "" ' Anzahl Sitze
                Dim m_ZZANZSTEHPLAETZE As String = "" ' Anzahl Stehplätze
                Dim m_ZZBEIUMDREH As String = "" ' Umdrehungen zur Nennleistung
                Dim m_ZZBEMER1 As String = "" ' Bemerkungen
                Dim m_ZZBEMER10 As String = "" ' Bemerkungen
                Dim m_ZZBEMER11 As String = "" ' Bemerkungen
                Dim m_ZZBEMER12 As String = "" ' Bemerkungen
                Dim m_ZZBEMER13 As String = "" ' Bemerkungen
                Dim m_ZZBEMER14 As String = "" ' Bemerkungen
                Dim m_ZZBEMER2 As String = "" ' Bemerkungen
                Dim m_ZZBEMER3 As String = "" ' Bemerkungen
                Dim m_ZZBEMER4 As String = "" ' Bemerkungen
                Dim m_ZZBEMER5 As String = "" ' Bemerkungen
                Dim m_ZZBEMER6 As String = "" ' Bemerkungen
                Dim m_ZZBEMER7 As String = "" ' Bemerkungen
                Dim m_ZZBEMER8 As String = "" ' Bemerkungen
                Dim m_ZZBEMER9 As String = "" ' Bemerkungen
                Dim m_ZZBEREIFACHSE1 As String = "" ' Bereifung Achse 1
                Dim m_ZZBEREIFACHSE2 As String = "" ' Bereifung Achse 2
                Dim m_ZZBEREIFACHSE3 As String = "" ' Bereifung Achse 3
                Dim m_ZZBREITEMAX As String = "" ' Breite Max
                Dim m_ZZBREITEMIN As String = "" ' Breite Min
                Dim m_ZZCO2KOMBI As String = "" ' Co2 Gehalt in g/km
                Dim m_ZZCODE_AUFBAU As String = "" ' Code Aufbau
                Dim m_ZZCODE_KRAFTSTOF As String = "" ' Kraftstoff Code
                Dim m_ZZDREHZSTANDGER As String = "" ' Drehzahl zu Standgeräusch
                Dim m_ZZFABRIKNAME As String = "" ' Fabrikname
                Dim m_ZZFAHRGERAEUSCH As String = "" ' Fahrgeräusch
                Dim m_ZZFAHRZEUGKLASSE As String = "" ' Fahrzeugklasse
                Dim m_ZZFASSVERMOEGEN As String = "" ' Fassungsvermögen bei Tankfahrzeugen
                Dim m_ZZFHRZKLASSE_TXT As String = "" ' Fahrzeugklasse Text
                Dim m_ZZGENEHMIGDAT As String = "" ' Genehmigungs Datum
                Dim m_ZZGENEHMIGNR As String = "" ' Genehmigungsnummer
                Dim m_ZZHANDELSNAME As String = "" ' Handelsname
                Dim m_ZZHERST_TEXT As String = "" ' Hersteller Kurzbezeichnung
                Dim m_ZZHERSTELLER_SCH As String = "" ' Hersteller Schlüssel
                Dim m_ZZHOECHSTGESCHW As String = "" ' Höchstgeschwindigkeit
                Dim m_ZZHOEHEMAX As String = "" ' Höhe Max
                Dim m_ZZHOEHEMIN As String = "" ' Höhe Min
                Dim m_ZZHUBRAUM As String = "" ' Hubraum
                Dim m_ZZKLARTEXT_TYP As String = "" ' Klartext Typ
                Dim m_ZZKRAFTSTOFF_TXT As String = "" ' Kraftstoffart Text
                Dim m_ZZLAENGEMAX As String = "" ' Länge Max
                Dim m_ZZLAENGEMIN As String = "" ' Länge Min
                Dim m_ZZLEISTUNGSGEW As String = "" ' Leistungsgewicht
                Dim m_ZZMASSEFAHRBMAX As String = "" ' Masse fahbereit Max
                Dim m_ZZMASSEFAHRBMIN As String = "" ' Masse fahrbereit Min
                Dim m_ZZNATIONALE_EMIK As String = "" ' Nationale Emisionsklasse
                Dim m_ZZNENNLEISTUNG As String = "" ' Nennleistung in KW
                Dim m_ZZSLD As String = "" ' Code nat. Emiklasse
                Dim m_ZZSTANDGERAEUSCH As String = "" ' Standgeräusch
                Dim m_ZZSTUETZLAST As String = "" ' Stützlast
                Dim m_ZZTEXT_AUFBAU As String = "" ' Text Aufbau
                Dim m_ZZTYP_SCHL As String = "" ' Typ Schlüssel
                Dim m_ZZTYP_VVS_PRUEF As String = "" ' VVS Prüfziffer
                Dim m_ZZVARIANTE As String = "" ' Variante
                Dim m_ZZVERSION As String = "" ' Version
                Dim m_ZZVVS_SCHLUESSEL As String = "" ' VVS Schlüssel
                Dim m_ZZZULGESGEW As String = "" ' zulässiges Gesamtgewicht
                Dim m_ZZZULGESGEWSTAAT As String = "" ' zulässiges Gesamtgewicht im Mitgl.Staat
                Dim m_ZFARBE_KLAR As String = ""

                'Dim objSAP As New SAPProxy_Base.SAPProxy_Base() ' SAPProxy_SIXT.SAPProxy_SIXT()

                'MakeDestination()
                'objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
                'objSAP.Connection.Open()

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                m_intIDSAP = -1

                Try
                    m_intStatus = 0
                    m_strMessage = ""

                    If CheckCustomerData() Then
                        m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Abezufzg", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                        Dim proxy = DynSapProxy.getProxy("Z_M_Abezufzg", m_objApp, m_objUser, PageHelper.GetCurrentPage())

                        proxy.setImportParameter("ZZEQUNR", strEquNr)
                        proxy.setImportParameter("ZZKUNNR", m_strCustomer)

                        proxy.callBapi()

                        If m_intIDSAP > -1 Then
                            m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                        End If

                        m_ZFARBE_KLAR = proxy.getExportParameter("ZFARBE_KLAR")
                        strZZABEKZ = proxy.getExportParameter("ZZABEKZ")
                        m_ZZABGASRICHTL_TG = proxy.getExportParameter("ZZABGASRICHTL_TG")
                        m_ZZACHSL_A1_STA = proxy.getExportParameter("ZZACHSL_A1_STA")
                        m_ZZACHSL_A2_STA = proxy.getExportParameter("ZZACHSL_A2_STA")
                        m_ZZACHSL_A3_STA = proxy.getExportParameter("ZZACHSL_A3_STA")
                        m_ZZACHSLST_ACHSE1 = proxy.getExportParameter("ZZACHSLST_ACHSE1")
                        m_ZZACHSLST_ACHSE2 = proxy.getExportParameter("ZZACHSLST_ACHSE2")
                        m_ZZACHSLST_ACHSE3 = proxy.getExportParameter("ZZACHSLST_ACHSE3")
                        strZZAKPZ = proxy.getExportParameter("ZZAKPZ")
                        strZZAKUP = proxy.getExportParameter("ZZAKUP")
                        strZZALBR = proxy.getExportParameter("ZZALBR")
                        strZZALHI = proxy.getExportParameter("ZZALHI")
                        strZZALMI = proxy.getExportParameter("ZZALMI")
                        strZZALUB = proxy.getExportParameter("ZZALUB")
                        strZZALVO = proxy.getExportParameter("ZZALVO")
                        strZZANGE = proxy.getExportParameter("ZZANGE")
                        m_ZZANHLAST_GEBR = proxy.getExportParameter("ZZANHLAST_GEBR")
                        m_ZZANHLAST_UNGEBR = proxy.getExportParameter("ZZANHLAST_UNGEBR")
                        m_ZZANTRIEBSACHS = proxy.getExportParameter("ZZANTRIEBSACHS")
                        m_ZZANZACHS = proxy.getExportParameter("ZZANZACHS")
                        m_ZZANZSITZE = proxy.getExportParameter("ZZANZSITZE")
                        m_ZZANZSTEHPLAETZE = proxy.getExportParameter("ZZANZSTEHPLAETZE")
                        strZZAUFB = proxy.getExportParameter("ZZAUFB")
                        strZZAUFTXT = proxy.getExportParameter("ZZAUFTXT")
                        strZZAUSF = proxy.getExportParameter("ZZAUSF")
                        strZZBDR1 = proxy.getExportParameter("ZZBDR1")
                        strZZBDR2 = proxy.getExportParameter("ZZBDR2")
                        strZZBE01 = proxy.getExportParameter("ZZBE01")
                        strZZBE02 = proxy.getExportParameter("ZZBE02")
                        strZZBE03 = proxy.getExportParameter("ZZBE03")
                        strZZBE04 = proxy.getExportParameter("ZZBE04")
                        strZZBE05 = proxy.getExportParameter("ZZBE05")
                        strZZBE06 = proxy.getExportParameter("ZZBE06")
                        strZZBE07 = proxy.getExportParameter("ZZBE07")
                        strZZBE08 = proxy.getExportParameter("ZZBE08")
                        strZZBE09 = proxy.getExportParameter("ZZBE09")
                        strZZBE10 = proxy.getExportParameter("ZZBE10")
                        strZZBE11 = proxy.getExportParameter("ZZBE11")
                        strZZBE12 = proxy.getExportParameter("ZZBE12")
                        strZZBE13 = proxy.getExportParameter("ZZBE13")
                        strZZBE14 = proxy.getExportParameter("ZZBE14")
                        strZZBE15 = proxy.getExportParameter("ZZBE15")
                        strZZBE16 = proxy.getExportParameter("ZZBE16")
                        strZZBE17 = proxy.getExportParameter("ZZBE17")
                        strZZBE18 = proxy.getExportParameter("ZZBE18")
                        strZZBE19 = proxy.getExportParameter("ZZBE19")
                        strZZBE20 = proxy.getExportParameter("ZZBE20")
                        strZZBE21 = proxy.getExportParameter("ZZBE21")
                        strZZBE22 = proxy.getExportParameter("ZZBE22")
                        m_ZZBEIUMDREH = proxy.getExportParameter("ZZBEIUMDREH")
                        m_ZZBEMER1 = proxy.getExportParameter("ZZBEMER1")
                        m_ZZBEMER2 = proxy.getExportParameter("ZZBEMER2")
                        m_ZZBEMER3 = proxy.getExportParameter("ZZBEMER3")
                        m_ZZBEMER4 = proxy.getExportParameter("ZZBEMER4")
                        m_ZZBEMER5 = proxy.getExportParameter("ZZBEMER5")
                        m_ZZBEMER6 = proxy.getExportParameter("ZZBEMER6")
                        m_ZZBEMER7 = proxy.getExportParameter("ZZBEMER7")
                        m_ZZBEMER8 = proxy.getExportParameter("ZZBEMER8")
                        m_ZZBEMER9 = proxy.getExportParameter("ZZBEMER9")
                        m_ZZBEMER10 = proxy.getExportParameter("ZZBEMER10")
                        m_ZZBEMER11 = proxy.getExportParameter("ZZBEMER11")
                        m_ZZBEMER12 = proxy.getExportParameter("ZZBEMER12")
                        m_ZZBEMER13 = proxy.getExportParameter("ZZBEMER13")
                        m_ZZBEMER14 = proxy.getExportParameter("ZZBEMER14")
                        m_ZZBEREIFACHSE1 = proxy.getExportParameter("ZZBEREIFACHSE1")
                        m_ZZBEREIFACHSE2 = proxy.getExportParameter("ZZBEREIFACHSE2")
                        m_ZZBEREIFACHSE3 = proxy.getExportParameter("ZZBEREIFACHSE3")
                        m_ZZBREITEMAX = proxy.getExportParameter("ZZBREITEMAX")
                        m_ZZBREITEMIN = proxy.getExportParameter("ZZBREITEMIN")
                        strZZCHASSIS_NUM = proxy.getExportParameter("ZZCHASSIS_NUM")
                        m_ZZCO2KOMBI = proxy.getExportParameter("ZZCO2KOMBI")
                        m_ZZCODE_AUFBAU = proxy.getExportParameter("ZZCODE_AUFBAU")
                        m_ZZCODE_KRAFTSTOF = proxy.getExportParameter("ZZCODE_KRAFTSTOFF")
                        m_ZZDREHZSTANDGER = proxy.getExportParameter("ZZDREHZSTANDGER")
                        strZZEARTX = proxy.getExportParameter("ZZEARTX")
                        Decimal.TryParse(proxy.getExportParameter("ZZENGINE_CAP"), decZZENGINE_CAP)
                        Decimal.TryParse(proxy.getExportParameter("ZZENGINE_POWER"), decZZENGINE_POWER)
                        strZZENGINE_TYPE = proxy.getExportParameter("ZZENGINE_TYPE")
                        m_ZZFABRIKNAME = proxy.getExportParameter("ZZFABRIKNAME")
                        m_ZZFAHRGERAEUSCH = proxy.getExportParameter("ZZFAHRGERAEUSCH")
                        m_ZZFAHRZEUGKLASSE = proxy.getExportParameter("ZZFAHRZEUGKLASSE")
                        strZZFARBE = proxy.getExportParameter("ZZFARBE")
                        m_ZZFASSVERMOEGEN = proxy.getExportParameter("ZZFASSVERMOEGEN")
                        strZZFDB9 = proxy.getExportParameter("ZZFDB9")
                        m_ZZFHRZKLASSE_TXT = proxy.getExportParameter("ZZFHRZKLASSE_TXT")
                        strZZFLEET_CAT = proxy.getExportParameter("ZZFLEET_CAT")
                        Decimal.TryParse(proxy.getExportParameter("ZZFLEET_HGT"), decZZFLEET_HGT)
                        Decimal.TryParse(proxy.getExportParameter("ZZFLEET_LEN"), decZZFLEET_LEN)
                        strZZFLEET_VIN = proxy.getExportParameter("ZZFLEET_VIN")
                        Decimal.TryParse(proxy.getExportParameter("ZZFLEET_WID"), decZZFLEET_WID)
                        m_ZZGENEHMIGDAT = proxy.getExportParameter("ZZGENEHMIGDAT")
                        m_ZZGENEHMIGNR = proxy.getExportParameter("ZZGENEHMIGNR")
                        Decimal.TryParse(proxy.getExportParameter("ZZGROSS_WGT"), decZZGROSS_WGT)
                        m_ZZHANDELSNAME = proxy.getExportParameter("ZZHANDELSNAME")
                        m_ZZHERST_TEXT = proxy.getExportParameter("ZZHERST_TEXT")
                        m_ZZHERSTELLER_SCH = proxy.getExportParameter("ZZHERSTELLER_SCH")
                        m_ZZHOECHSTGESCHW = proxy.getExportParameter("ZZHOECHSTGESCHW")
                        m_ZZHOEHEMAX = proxy.getExportParameter("ZZHOEHEMAX")
                        m_ZZHOEHEMIN = proxy.getExportParameter("ZZHOEHEMIN")
                        m_ZZHUBRAUM = proxy.getExportParameter("ZZHUBRAUM")
                        m_ZZKLARTEXT_TYP = proxy.getExportParameter("ZZKLARTEXT_TYP")
                        strZZKLTXT = proxy.getExportParameter("ZZKLTXT")
                        m_ZZKRAFTSTOFF_TXT = proxy.getExportParameter("ZZKRAFTSTOFF_TXT")
                        m_ZZLAENGEMAX = proxy.getExportParameter("ZZLAENGEMAX")
                        m_ZZLAENGEMIN = proxy.getExportParameter("ZZLAENGEMIN")
                        m_ZZLEISTUNGSGEW = proxy.getExportParameter("ZZLEISTUNGSGEW")
                        strZZLGEW = proxy.getExportParameter("ZZLGEW")
                        m_ZZMASSEFAHRBMAX = proxy.getExportParameter("ZZMASSEFAHRBMAX")
                        m_ZZMASSEFAHRBMIN = proxy.getExportParameter("ZZMASSEFAHRBMIN")
                        strZZMAX_OCCUPANTS = proxy.getExportParameter("ZZMAX_OCCUPANTS")
                        m_ZZNATIONALE_EMIK = proxy.getExportParameter("ZZNATIONALE_EMIK")
                        m_ZZNENNLEISTUNG = proxy.getExportParameter("ZZNENNLEISTUNG")
                        strZZNULA = proxy.getExportParameter("ZZNULA")
                        strZZNUM_AXLE = proxy.getExportParameter("ZZNUM_AXLE")
                        strZZORHI = proxy.getExportParameter("ZZORHI")
                        strZZORVO = proxy.getExportParameter("ZZORVO")
                        strZZPRFZ = proxy.getExportParameter("ZZPRFZ")
                        strZZREHI = proxy.getExportParameter("ZZREHI")
                        strZZREPLA_DATE = proxy.getExportParameter("ZZREPLA_DATE")
                        strZZREVO = proxy.getExportParameter("ZZREVO")
                        strZZREVOLUTIONS = proxy.getExportParameter("ZZREVOLUTIONS")
                        strZZSDB9 = proxy.getExportParameter("ZZSDB9")
                        m_ZZSLD = proxy.getExportParameter("ZZSLD")
                        Decimal.TryParse(proxy.getExportParameter("ZZSPEED_MAX"), decZZSPEED_MAX)
                        m_ZZSTANDGERAEUSCH = proxy.getExportParameter("ZZSTANDGERAEUSCH")
                        strZZSTPL = proxy.getExportParameter("ZZSTPL")
                        m_ZZSTUETZLAST = proxy.getExportParameter("ZZSTUETZLAST")
                        strZZTANK = proxy.getExportParameter("ZZTANK")
                        m_ZZTEXT_AUFBAU = proxy.getExportParameter("ZZTEXT_AUFBAU")
                        strZZTYP = proxy.getExportParameter("ZZTYP")
                        m_ZZTYP_SCHL = proxy.getExportParameter("ZZTYP_SCHL")
                        m_ZZTYP_VVS_PRUEF = proxy.getExportParameter("ZZTYP_VVS_PRUEF")
                        strZZTYPA = proxy.getExportParameter("ZZTYPA")
                        strZZTYPE_TEXT = proxy.getExportParameter("ZZTYPE_TEXT")
                        strZZUNIT_POWER = proxy.getExportParameter("ZZUNIT_POWER")
                        m_ZZVARIANTE = proxy.getExportParameter("ZZVARIANTE")
                        m_ZZVERSION = proxy.getExportParameter("ZZVERSION")
                        m_ZZVVS_SCHLUESSEL = proxy.getExportParameter("ZZVVS_SCHLUESSEL")
                        m_ZZZULGESGEW = proxy.getExportParameter("ZZZULGESGEW")
                        m_ZZZULGESGEWSTAAT = proxy.getExportParameter("ZZZULGESGEWSTAAT")

                        If Not strZZCHASSIS_NUM.Length = 0 Then
                            m_objABE_Daten.Fahrgestellnummer = strZZCHASSIS_NUM
                        Else
                            m_objABE_Daten.Fahrgestellnummer = "-"
                        End If
                        If Not strZZPRFZ.Length = 0 Then
                            m_objABE_Daten.FahrgestellnummerPruefziffer = strZZPRFZ
                        Else
                            m_objABE_Daten.FahrgestellnummerPruefziffer = "-"
                        End If
                        If Not strZZREPLA_DATE.Length = 0 Then
                            m_objABE_Daten.TagDerErstenZulassung = MakeDateStandard(strZZREPLA_DATE).ToShortDateString
                            If m_objABE_Daten.TagDerErstenZulassung = "01.01.1900" Then
                                m_objABE_Daten.TagDerErstenZulassung = "-"
                            End If
                        Else
                            m_objABE_Daten.TagDerErstenZulassung = "-"
                        End If
                        If IsNumeric(decZZFLEET_HGT) Then
                            m_objABE_Daten.HoeheDesFahrzeugs = CStr(CInt(decZZFLEET_HGT))
                        End If
                        If IsNumeric(decZZFLEET_WID) Then
                            m_objABE_Daten.BreiteDesFahrzeugs = CStr(CInt(decZZFLEET_WID))
                        End If
                        If IsNumeric(decZZFLEET_LEN) Then
                            m_objABE_Daten.LaengeDesFahrzeugs = CStr(CInt(decZZFLEET_LEN))
                        End If
                        If IsNumeric(strZZNUM_AXLE) Then
                            m_objABE_Daten.AnzahlDerAchsenDesFahrzeugs = CStr(CInt(strZZNUM_AXLE))
                        Else
                            m_objABE_Daten.AnzahlDerAchsenDesFahrzeugs = "-"
                        End If
                        If IsNumeric(strZZMAX_OCCUPANTS) Then
                            m_objABE_Daten.MaximalAnzahlPersonen = CStr(CInt(strZZMAX_OCCUPANTS))
                        Else
                            m_objABE_Daten.MaximalAnzahlPersonen = "-"
                        End If
                        If Not strZZENGINE_TYPE.Length = 0 Then
                            m_objABE_Daten.Antriebsart = strZZENGINE_TYPE
                        Else
                            m_objABE_Daten.Antriebsart = "-"
                        End If
                        If IsNumeric(decZZENGINE_POWER) And IsNumeric(strZZREVOLUTIONS) Then
                            If decZZENGINE_POWER > 1000 Then
                                m_objABE_Daten.Leistung = "K" & CStr(CInt(decZZENGINE_POWER / 1000)) & "/" & CStr(CInt(strZZREVOLUTIONS))
                            Else
                                m_objABE_Daten.Leistung = "K" & CStr(CInt(decZZENGINE_POWER)) & "/" & CStr(CInt(strZZREVOLUTIONS))
                            End If
                        Else
                            m_objABE_Daten.Leistung = "-"
                        End If
                        If IsNumeric(decZZENGINE_CAP) Then
                            m_objABE_Daten.Hubraum = CStr(CInt(decZZENGINE_CAP))
                        Else
                            m_objABE_Daten.Hubraum = "-"
                        End If
                        If IsNumeric(decZZGROSS_WGT) Then
                            m_objABE_Daten.ZulaessigesGesamtgewicht = CStr(CInt(decZZGROSS_WGT))
                        Else
                            m_objABE_Daten.ZulaessigesGesamtgewicht = ""
                        End If
                        If Not strZZFLEET_CAT.Length = 0 Then
                            m_objABE_Daten.Fahrzeugart = strZZFLEET_CAT
                        Else
                            m_objABE_Daten.Fahrzeugart = "-"
                        End If
                        If Not strZZFLEET_VIN.Length = 0 Then
                            m_objABE_Daten.IdentifikationsnummerFuerFahrzeug = strZZFLEET_VIN
                        Else
                            m_objABE_Daten.IdentifikationsnummerFuerFahrzeug = "-"
                        End If
                        If IsNumeric(decZZSPEED_MAX) Then
                            m_objABE_Daten.Hoechstgeschwindigkeit = CStr(CInt(decZZSPEED_MAX))
                        Else
                            m_objABE_Daten.Hoechstgeschwindigkeit = "-"
                        End If
                        If IsNumeric(strZZLGEW) Then
                            m_objABE_Daten.Leergewicht = CStr(CInt(strZZLGEW))
                        Else
                            m_objABE_Daten.Leergewicht = "-"
                        End If
                        If IsNumeric(strZZALVO) Then
                            m_objABE_Daten.AchslastVorn = CStr(CInt(strZZALVO))
                        Else
                            m_objABE_Daten.AchslastVorn = "-"
                        End If
                        If IsNumeric(strZZALHI) Then
                            m_objABE_Daten.AchslastHinten = CStr(CInt(strZZALHI))
                        Else
                            m_objABE_Daten.AchslastHinten = "-"
                        End If
                        If IsNumeric(strZZALMI) Then
                            m_objABE_Daten.AchslastMitte = CStr(CInt(strZZALMI))
                        Else
                            m_objABE_Daten.AchslastMitte = "-"
                        End If
                        If IsNumeric(strZZANGE) Then
                            m_objABE_Daten.DavonAngetriebeneAchsen = CStr(CInt(strZZANGE))
                        Else
                            m_objABE_Daten.DavonAngetriebeneAchsen = "-"
                        End If
                        If Not strZZREVO.Length = 0 Then
                            m_objABE_Daten.BereifunsgroesseVorn = strZZREVO
                        Else
                            m_objABE_Daten.BereifunsgroesseVorn = "-"
                        End If
                        If Not strZZREHI.Length = 0 Then
                            m_objABE_Daten.BereifunsgroesseMitte_Hinten = strZZREHI
                        Else
                            m_objABE_Daten.BereifunsgroesseMitte_Hinten = "-"
                        End If
                        If Not strZZORVO.Length = 0 Then
                            m_objABE_Daten.OderBereifunsgroesseVorn = strZZORVO
                        Else
                            m_objABE_Daten.OderBereifunsgroesseVorn = "-"
                        End If
                        If Not strZZORHI.Length = 0 Then
                            m_objABE_Daten.OderBereifunsgroesseMitte_Hinten = strZZORHI
                        Else
                            m_objABE_Daten.OderBereifunsgroesseMitte_Hinten = "-"
                        End If
                        If Not strZZAKPZ.Length = 0 Then
                            m_objABE_Daten.AnhaengerKupplungPruefzeichen = strZZAKPZ
                        Else
                            m_objABE_Daten.AnhaengerKupplungPruefzeichen = "-"
                        End If
                        If IsNumeric(strZZALBR) Then
                            m_objABE_Daten.AnhaengerlastGebremst = CStr(CInt(strZZALBR))
                        Else
                            m_objABE_Daten.AnhaengerlastGebremst = "-"
                        End If
                        If IsNumeric(strZZALUB) Then
                            m_objABE_Daten.AnhaengerlastUngebremst = CStr(CInt(strZZALUB))
                        Else
                            m_objABE_Daten.AnhaengerlastUngebremst = "-"
                        End If
                        If IsNumeric(strZZSDB9) Then
                            m_objABE_Daten.StandGeraeusch = CStr(CInt(strZZSDB9))
                        Else
                            m_objABE_Daten.StandGeraeusch = "-"
                        End If
                        If IsNumeric(strZZFDB9) Then
                            m_objABE_Daten.Fahrgeraeusch = CStr(CInt(strZZFDB9))
                        Else
                            m_objABE_Daten.Fahrgeraeusch = "-"
                        End If
                        If IsNumeric(strZZBDR1) Then
                            m_objABE_Daten.BremsdruckEinleitung = CStr(CInt(strZZBDR1))
                        Else
                            m_objABE_Daten.BremsdruckEinleitung = "-"
                        End If
                        If IsNumeric(strZZBDR2) Then
                            m_objABE_Daten.BremsdruckZweileitung = CStr(CInt(strZZBDR2))
                        Else
                            m_objABE_Daten.BremsdruckZweileitung = "-"
                        End If
                        If IsNumeric(strZZNULA) Then
                            m_objABE_Daten.NutzAufliegelast = CStr(CInt(strZZNULA))
                        Else
                            m_objABE_Daten.NutzAufliegelast = "-"
                        End If
                        If IsNumeric(strZZTANK) Then
                            m_objABE_Daten.Tankinhalt = CStr(CInt(strZZTANK))
                        Else
                            m_objABE_Daten.Tankinhalt = "-"
                        End If
                        If IsNumeric(strZZSTPL) Then
                            m_objABE_Daten.Stehplaetze = CStr(CInt(strZZSTPL))
                        Else
                            m_objABE_Daten.Stehplaetze = "-"
                        End If

                        m_objABE_Daten.Bemerkung = ""
                        If Not strZZBE01.Length = 0 Then
                            m_objABE_Daten.Bemerkung &= strZZBE01
                        End If
                        If Not strZZBE02.Length = 0 Then
                            m_objABE_Daten.Bemerkung &= strZZBE02 & "<br>"
                        End If
                        If Not strZZBE03.Length = 0 Then
                            m_objABE_Daten.Bemerkung &= strZZBE03
                        End If
                        If Not strZZBE04.Length = 0 Then
                            m_objABE_Daten.Bemerkung &= strZZBE04 & "<br>"
                        End If
                        If Not strZZBE05.Length = 0 Then
                            m_objABE_Daten.Bemerkung &= strZZBE05
                        End If
                        If Not strZZBE06.Length = 0 Then
                            m_objABE_Daten.Bemerkung &= strZZBE06 & "<br>"
                        End If
                        If Not strZZBE07.Length = 0 Then
                            m_objABE_Daten.Bemerkung &= strZZBE07
                        End If
                        If Not strZZBE08.Length = 0 Then
                            m_objABE_Daten.Bemerkung &= strZZBE08 & "<br>"
                        End If
                        If Not strZZBE09.Length = 0 Then
                            m_objABE_Daten.Bemerkung &= strZZBE09
                        End If
                        If Not strZZBE10.Length = 0 Then
                            m_objABE_Daten.Bemerkung &= strZZBE10 & "<br>"
                        End If
                        If Not strZZBE11.Length = 0 Then
                            m_objABE_Daten.Bemerkung &= strZZBE11
                        End If
                        If Not strZZBE12.Length = 0 Then
                            m_objABE_Daten.Bemerkung &= strZZBE12 & "<br>"
                        End If
                        If Not strZZBE13.Length = 0 Then
                            m_objABE_Daten.Bemerkung &= strZZBE13
                        End If
                        If Not strZZBE14.Length = 0 Then
                            m_objABE_Daten.Bemerkung &= strZZBE14 & "<br>"
                        End If
                        If Not strZZBE15.Length = 0 Then
                            m_objABE_Daten.Bemerkung &= strZZBE15
                        End If
                        If Not strZZBE16.Length = 0 Then
                            m_objABE_Daten.Bemerkung &= strZZBE16 & "<br>"
                        End If
                        If Not strZZBE17.Length = 0 Then
                            m_objABE_Daten.Bemerkung &= strZZBE17
                        End If
                        If Not strZZBE18.Length = 0 Then
                            m_objABE_Daten.Bemerkung &= strZZBE18 & "<br>"
                        End If
                        If Not strZZBE19.Length = 0 Then
                            m_objABE_Daten.Bemerkung &= strZZBE19
                        End If
                        If Not strZZBE20.Length = 0 Then
                            m_objABE_Daten.Bemerkung &= strZZBE20 & "<br>"
                        End If
                        If Not strZZBE21.Length = 0 Then
                            m_objABE_Daten.Bemerkung &= strZZBE21
                        End If
                        If Not strZZBE22.Length = 0 Then
                            m_objABE_Daten.Bemerkung &= strZZBE22
                        End If
                        If m_objABE_Daten.Bemerkung.Length = 0 Then
                            m_objABE_Daten.Bemerkung = "-"
                        End If

                        If Not strZZAKUP.Length = 0 Then
                            m_objABE_Daten.Anhaengerkupplung = strZZAKUP
                        Else
                            m_objABE_Daten.Anhaengerkupplung = "-"
                        End If
                        If Not strZZAUSF.Length = 0 Then
                            m_objABE_Daten.Ausfuehrung = strZZAUSF
                        Else
                            m_objABE_Daten.Ausfuehrung = "-"
                        End If
                        If Not strZZTYP.Length = 0 Then
                            m_objABE_Daten.TypSchluessel = strZZTYP
                        Else
                            m_objABE_Daten.TypSchluessel = "-"
                        End If
                        If Not strZZKLTXT.Length = 0 Then
                            m_objABE_Daten.HerstellerKlartext = strZZKLTXT
                        Else
                            m_objABE_Daten.HerstellerKlartext = "-"
                        End If
                        If Not strZZTYPA.Length = 0 Then
                            m_objABE_Daten.TypUndAusfuehrungsart = strZZTYPA
                        Else
                            m_objABE_Daten.TypUndAusfuehrungsart = "-"
                        End If
                        If Not strZZABEKZ.Length = 0 Then
                            m_objABE_Daten.TypUndAusfuehrungsPruefziffer = strZZABEKZ
                        Else
                            m_objABE_Daten.TypUndAusfuehrungsPruefziffer = "-"
                        End If
                        If Not strZZAUFB.Length = 0 Then
                            m_objABE_Daten.AufbauartSchluessel = strZZAUFB
                        Else
                            m_objABE_Daten.AufbauartSchluessel = "-"
                        End If
                        If Not strZZFARBE.Length = 0 Then
                            m_objABE_Daten.Farbziffer = strZZFARBE
                        Else
                            m_objABE_Daten.Farbziffer = "-"
                        End If
                        If Not strZZAUFTXT.Length = 0 Then
                            m_objABE_Daten.AufbauartText = strZZAUFTXT
                        Else
                            m_objABE_Daten.AufbauartText = "-"
                        End If
                        If Not strZZEARTX.Length = 0 Then
                            m_objABE_Daten.FahrzeugartText = strZZEARTX
                        Else
                            m_objABE_Daten.FahrzeugartText = "-"
                        End If

                        If Not strZZTYPE_TEXT.Length = 0 Then
                            m_objABE_Daten.AntriebsartKurztext = strZZTYPE_TEXT
                        Else
                            m_objABE_Daten.AntriebsartKurztext = "-"
                        End If
                        '---------------------------------------------------------------
                        With m_objABE_Daten
                            .ZZABGASRICHTL_TG = m_ZZABGASRICHTL_TG
                            .ZZACHSL_A1_STA = m_ZZACHSL_A1_STA
                            .ZZACHSL_A2_STA = m_ZZACHSL_A2_STA
                            .ZZACHSL_A3_STA = m_ZZACHSL_A3_STA
                            .ZZACHSLST_ACHSE1 = m_ZZACHSLST_ACHSE1
                            .ZZACHSLST_ACHSE2 = m_ZZACHSLST_ACHSE2
                            .ZZACHSLST_ACHSE3 = m_ZZACHSLST_ACHSE3
                            .ZZANHLAST_GEBR = m_ZZANHLAST_GEBR
                            .ZZANHLAST_UNGEBR = m_ZZANHLAST_UNGEBR
                            .ZZANTRIEBSACHS = m_ZZANTRIEBSACHS
                            .ZZANZACHS = m_ZZANZACHS
                            .ZZANZSITZE = m_ZZANZSITZE
                            .ZZANZSTEHPLAETZE = m_ZZANZSTEHPLAETZE
                            .ZZBEIUMDREH = m_ZZBEIUMDREH
                            .ZZBEMER1 = m_ZZBEMER1
                            .ZZBEMER10 = m_ZZBEMER10
                            .ZZBEMER11 = m_ZZBEMER11
                            .ZZBEMER12 = m_ZZBEMER12
                            .ZZBEMER13 = m_ZZBEMER13
                            .ZZBEMER14 = m_ZZBEMER14
                            .ZZBEMER2 = m_ZZBEMER2
                            .ZZBEMER3 = m_ZZBEMER3
                            .ZZBEMER4 = m_ZZBEMER4
                            .ZZBEMER5 = m_ZZBEMER5
                            .ZZBEMER6 = m_ZZBEMER6
                            .ZZBEMER7 = m_ZZBEMER7
                            .ZZBEMER8 = m_ZZBEMER8
                            .ZZBEMER9 = m_ZZBEMER9
                            .ZZBEREIFACHSE1 = m_ZZBEREIFACHSE1
                            .ZZBEREIFACHSE2 = m_ZZBEREIFACHSE2
                            .ZZBEREIFACHSE3 = m_ZZBEREIFACHSE3
                            .ZZBREITEMAX = m_ZZBREITEMAX
                            .ZZBREITEMIN = m_ZZBREITEMIN
                            .ZZCO2KOMBI = m_ZZCO2KOMBI
                            .ZZCODE_AUFBAU = m_ZZCODE_AUFBAU
                            .ZZCODE_KRAFTSTOF = m_ZZCODE_KRAFTSTOF
                            .ZZDREHZSTANDGER = m_ZZDREHZSTANDGER
                            .ZZFABRIKNAME = m_ZZFABRIKNAME
                            .ZZFAHRGERAEUSCH = m_ZZFAHRGERAEUSCH
                            .ZZFAHRZEUGKLASSE = m_ZZFAHRZEUGKLASSE
                            .ZZFASSVERMOEGEN = m_ZZFASSVERMOEGEN
                            .ZZFHRZKLASSE_TXT = m_ZZFHRZKLASSE_TXT
                            .ZZGENEHMIGDAT = m_ZZGENEHMIGDAT
                            .ZZGENEHMIGNR = m_ZZGENEHMIGNR
                            .ZZHANDELSNAME = m_ZZHANDELSNAME
                            .ZZHERST_TEXT = m_ZZHERST_TEXT
                            .ZZHERSTELLER_SCH = m_ZZHERSTELLER_SCH
                            .ZZHOECHSTGESCHW = m_ZZHOECHSTGESCHW
                            .ZZHOEHEMAX = m_ZZHOEHEMAX
                            .ZZHOEHEMIN = m_ZZHOEHEMIN
                            .ZZHUBRAUM = m_ZZHUBRAUM
                            .ZZKLARTEXT_TYP = m_ZZKLARTEXT_TYP
                            .ZZKRAFTSTOFF_TXT = m_ZZKRAFTSTOFF_TXT
                            .ZZLAENGEMAX = m_ZZLAENGEMAX
                            .ZZLAENGEMIN = m_ZZLAENGEMIN
                            .ZZLEISTUNGSGEW = m_ZZLEISTUNGSGEW
                            .ZZMASSEFAHRBMAX = m_ZZMASSEFAHRBMAX
                            .ZZMASSEFAHRBMIN = m_ZZMASSEFAHRBMIN
                            .ZZNATIONALE_EMIK = m_ZZNATIONALE_EMIK
                            .ZZNENNLEISTUNG = m_ZZNENNLEISTUNG
                            .ZZSLD = m_ZZSLD
                            .ZZSTANDGERAEUSCH = m_ZZSTANDGERAEUSCH
                            .ZZSTUETZLAST = m_ZZSTUETZLAST
                            .ZZTEXT_AUFBAU = m_ZZTEXT_AUFBAU
                            .ZZTYP_SCHL = m_ZZTYP_SCHL
                            .ZZTYP_VVS_PRUEF = m_ZZTYP_VVS_PRUEF
                            .ZZVARIANTE = m_ZZVARIANTE
                            .ZZVERSION = m_ZZVERSION
                            .ZZVVS_SCHLUESSEL = m_ZZVVS_SCHLUESSEL
                            .ZZZULGESGEW = m_ZZZULGESGEW
                            .ZZZULGESGEWSTAAT = m_ZZZULGESGEWSTAAT
                            .ZZFARBE = m_ZFARBE_KLAR
                        End With

                        WriteLogEntry(True, "ZZKUNNR=" & m_strCustomer.TrimStart("0"c) & ", ZZEQUNR=" & strEquNr, m_tblResult)
                    End If
                Catch ex As Exception
                    Select Case ex.Message
                        Case "ERR_INV_KUNNR"
                            'ungültige Kundennummer
                            m_intStatus = -1
                            m_strMessage = "Ungültige Kundennummer"
                        Case "ERR_INV_EQUNR"
                            'ungültige Equipmentnummer
                            m_intStatus = -1
                            m_strMessage = "Ungültige Fahrzeugnummer"
                        Case "ERR_INV_KOMBI"
                            'ungültige Kombination Kundennummer und Equipmentnummer
                            m_intStatus = -1
                            m_strMessage = "Ungültige Kombination von Kundennummer und Fahrzeugnummer"
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = ex.Message
                    End Select
                    If m_intIDSAP > -1 Then
                        m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                    End If
                    WriteLogEntry(False, "ZZKUNNR=" & m_strCustomer.TrimStart("0"c) & ", ZZEQUNR=" & strEquNr & " , " & Replace(m_strMessage, "<br>", " "), m_tblResult)
                Finally
                    If m_intIDSAP > -1 Then
                        m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                    End If


                    m_blnGestartet = False
                End Try
            End If
        End Sub
        Public Sub FillDatenABE(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page, ByVal strEquNr As String) ' Dynproxy

            m_strClassAndMethod = "ABEDaten.FillDatenABE"
            If Not m_blnGestartet Then
                m_blnGestartet = True

                Dim strZZCHASSIS_NUM As String = "" 'Fahrgestellnummer (Ziffer 4)
                Dim strZZFARBE As String = "" 'Farbziffer des Fahrzeugs (Ziffer 32)
                '-------- NEUE ABE-Daten ab Okt. 2005 --------------------------------------
                Dim m_ZZABGASRICHTL_TG As String = "" ' Abgasrichtlinie TG
                Dim m_ZZACHSL_A1_STA As String = "" ' max. Achslast Achse 1im Mitgl.Staat
                Dim m_ZZACHSL_A2_STA As String = ""  ' max. Achslast Achse 2 im Mitgl
                Dim m_ZZACHSL_A3_STA As String = "" ' max. Achslast Achse 3im Mitgl
                Dim m_ZZACHSLST_ACHSE1 As String = "" ' max. Achslast Achse 1
                Dim m_ZZACHSLST_ACHSE2 As String = ""  ' max. Achslast Achse 2
                Dim m_ZZACHSLST_ACHSE3 As String = "" ' max. Achslast Achse 3
                Dim m_ZZANHLAST_GEBR As String = "" ' Anhängerlast gebremst
                Dim m_ZZANHLAST_UNGEBR As String = "" ' Anängerlast ungebremst
                Dim m_ZZANTRIEBSACHS As String = ""  ' Anzahl der Antriebsachsen
                Dim m_ZZANZACHS As String = ""  ' Anzahl der Achsen
                Dim m_ZZANZSITZE As String = "" ' Anzahl Sitze
                Dim m_ZZANZSTEHPLAETZE As String = "" ' Anzahl Stehplätze
                Dim m_ZZBEIUMDREH As String = "" ' Umdrehungen zur Nennleistung

                Dim m_ZZBEREIFACHSE1 As String = "" ' Bereifung Achse 1
                Dim m_ZZBEREIFACHSE2 As String = "" ' Bereifung Achse 2
                Dim m_ZZBEREIFACHSE3 As String = "" ' Bereifung Achse 3
                Dim m_ZZBREITEMAX As String = "" ' Breite Max
                Dim m_ZZBREITEMIN As String = "" ' Breite Min
                Dim m_ZZCO2KOMBI As String = "" ' Co2 Gehalt in g/km
                Dim m_ZZCODE_AUFBAU As String = "" ' Code Aufbau
                Dim m_ZZCODE_KRAFTSTOF As String = "" ' Kraftstoff Code
                Dim m_ZZDREHZSTANDGER As String = "" ' Drehzahl zu Standgeräusch
                Dim m_ZZFABRIKNAME As String = "" ' Fabrikname
                Dim m_ZZFAHRGERAEUSCH As String = "" ' Fahrgeräusch
                Dim m_ZZFAHRZEUGKLASSE As String = "" ' Fahrzeugklasse
                Dim m_ZZFASSVERMOEGEN As String = "" ' Fassungsvermögen bei Tankfahrzeugen
                Dim m_ZZFHRZKLASSE_TXT As String = "" ' Fahrzeugklasse Text
                Dim m_ZZGENEHMIGDAT As String = "" ' Genehmigungs Datum
                Dim m_ZZGENEHMIGNR As String = "" ' Genehmigungsnummer
                Dim m_ZZHANDELSNAME As String = "" ' Handelsname
                Dim m_ZZHERST_TEXT As String = "" ' Hersteller Kurzbezeichnung
                Dim m_ZZHERSTELLER_SCH As String = "" ' Hersteller Schlüssel
                Dim m_ZZHOECHSTGESCHW As String = "" ' Höchstgeschwindigkeit
                Dim m_ZZHOEHEMAX As String = "" ' Höhe Max
                Dim m_ZZHOEHEMIN As String = "" ' Höhe Min
                Dim m_ZZHUBRAUM As String = "" ' Hubraum
                Dim m_ZZKLARTEXT_TYP As String = "" ' Klartext Typ
                Dim m_ZZKRAFTSTOFF_TXT As String = "" ' Kraftstoffart Text
                Dim m_ZZLAENGEMAX As String = "" ' Länge Max
                Dim m_ZZLAENGEMIN As String = "" ' Länge Min
                Dim m_ZZLEISTUNGSGEW As String = "" ' Leistungsgewicht
                Dim m_ZZMASSEFAHRBMAX As String = "" ' Masse fahbereit Max
                Dim m_ZZMASSEFAHRBMIN As String = "" ' Masse fahrbereit Min
                Dim m_ZZNATIONALE_EMIK As String = "" ' Nationale Emisionsklasse
                Dim m_ZZNENNLEISTUNG As String = "" ' Nennleistung in KW
                Dim m_ZZSLD As String = "" ' Code nat. Emiklasse
                Dim m_ZZSTANDGERAEUSCH As String = "" ' Standgeräusch
                Dim m_ZZSTUETZLAST As String = "" ' Stützlast
                Dim m_ZZTEXT_AUFBAU As String = "" ' Text Aufbau
                Dim m_ZZTYP_SCHL As String = "" ' Typ Schlüssel
                Dim m_ZZTYP_VVS_PRUEF As String = "" ' VVS Prüfziffer
                Dim m_ZZVARIANTE As String = "" ' Variante
                Dim m_ZZVERSION As String = "" ' Version
                Dim m_ZZVVS_SCHLUESSEL As String = "" ' VVS Schlüssel
                Dim m_ZZZULGESGEW As String = "" ' zulässiges Gesamtgewicht
                Dim m_ZZZULGESGEWSTAAT As String = "" ' zulässiges Gesamtgewicht im Mitgl.Staat
                Dim m_ZFARBE_KLAR As String = ""

                m_intIDSAP = -1

                Try
                    m_intStatus = 0
                    m_strMessage = ""

                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ABEZUFZG", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("ZZKUNNR", m_strCustomer)
                    myProxy.setImportParameter("ZZEQUNR", strEquNr)

                    myProxy.callBapi()

                    strZZCHASSIS_NUM = myProxy.getExportParameter("ZZCHASSIS_NUM")
                    strZZFARBE = myProxy.getExportParameter("ZZFARBE")
                    m_ZZHERSTELLER_SCH = myProxy.getExportParameter("ZZHERSTELLER_SCH")
                    m_ZZTYP_SCHL = myProxy.getExportParameter("ZZTYP_SCHL")
                    m_ZZVVS_SCHLUESSEL = myProxy.getExportParameter("ZZVVS_SCHLUESSEL")
                    m_ZZTYP_VVS_PRUEF = myProxy.getExportParameter("ZZTYP_VVS_PRUEF")
                    m_ZZFAHRZEUGKLASSE = myProxy.getExportParameter("ZZFAHRZEUGKLASSE")
                    m_ZZCODE_AUFBAU = myProxy.getExportParameter("ZZCODE_AUFBAU")
                    m_ZZFABRIKNAME = myProxy.getExportParameter("ZZFABRIKNAME")
                    m_ZZKLARTEXT_TYP = myProxy.getExportParameter("ZZKLARTEXT_TYP")
                    m_ZZVARIANTE = myProxy.getExportParameter("ZZVARIANTE")
                    m_ZZVERSION = myProxy.getExportParameter("ZZVERSION")
                    m_ZZHANDELSNAME = myProxy.getExportParameter("ZZHANDELSNAME")
                    m_ZZHERST_TEXT = myProxy.getExportParameter("ZZHERST_TEXT")
                    m_ZZTEXT_AUFBAU = myProxy.getExportParameter("ZZFHRZKLASSE_TXT")
                    m_ZZTEXT_AUFBAU = myProxy.getExportParameter("ZZTEXT_AUFBAU")
                    m_ZZABGASRICHTL_TG = myProxy.getExportParameter("ZZABGASRICHTL_TG")
                    m_ZZNATIONALE_EMIK = myProxy.getExportParameter("ZZNATIONALE_EMIK")
                    m_ZZKRAFTSTOFF_TXT = myProxy.getExportParameter("ZZKRAFTSTOFF_TXT")
                    m_ZZCODE_KRAFTSTOF = myProxy.getExportParameter("ZZCODE_KRAFTSTOF")
                    m_ZZSLD = myProxy.getExportParameter("ZZSLD")
                    m_ZZHUBRAUM = myProxy.getExportParameter("ZZHUBRAUM")
                    m_ZZANZACHS = myProxy.getExportParameter("ZZANZACHS")
                    m_ZZANTRIEBSACHS = myProxy.getExportParameter("ZZANTRIEBSACHS")
                    m_ZZNENNLEISTUNG = myProxy.getExportParameter("ZZNENNLEISTUNG")
                    m_ZZBEIUMDREH = myProxy.getExportParameter("ZZBEIUMDREH")
                    m_ZZHOECHSTGESCHW = myProxy.getExportParameter("ZZHOECHSTGESCHW")
                    m_ZZFASSVERMOEGEN = myProxy.getExportParameter("ZZFASSVERMOEGEN")
                    m_ZZANZSITZE = myProxy.getExportParameter("ZZANZSITZE")
                    m_ZZANZSTEHPLAETZE = myProxy.getExportParameter("ZZANZSTEHPLAETZE")
                    m_ZZMASSEFAHRBMIN = myProxy.getExportParameter("ZZMASSEFAHRBMIN")
                    m_ZZMASSEFAHRBMAX = myProxy.getExportParameter("ZZMASSEFAHRBMAX")
                    m_ZZZULGESGEW = myProxy.getExportParameter("ZZZULGESGEW")
                    m_ZZZULGESGEWSTAAT = myProxy.getExportParameter("ZZZULGESGEWSTAAT")

                    m_ZZACHSLST_ACHSE1 = myProxy.getExportParameter("ZZACHSLST_ACHSE1")
                    m_ZZACHSLST_ACHSE2 = myProxy.getExportParameter("ZZACHSLST_ACHSE2")
                    m_ZZACHSLST_ACHSE3 = myProxy.getExportParameter("ZZACHSLST_ACHSE3")
                    m_ZZACHSL_A1_STA = myProxy.getExportParameter("ZZACHSL_A1_STA")
                    m_ZZACHSL_A2_STA = myProxy.getExportParameter("ZZACHSL_A2_STA")
                    m_ZZACHSL_A3_STA = myProxy.getExportParameter("ZZACHSL_A3_STA")
                    m_ZZCO2KOMBI = myProxy.getExportParameter("ZZCO2KOMBI")
                    m_ZZSTANDGERAEUSCH = myProxy.getExportParameter("ZZSTANDGERAEUSCH")
                    m_ZZDREHZSTANDGER = myProxy.getExportParameter("ZZDREHZSTANDGER")
                    m_ZZFAHRGERAEUSCH = myProxy.getExportParameter("ZZFAHRGERAEUSCH")
                    m_ZZANHLAST_GEBR = myProxy.getExportParameter("ZZANHLAST_GEBR")
                    m_ZZLEISTUNGSGEW = myProxy.getExportParameter("ZZANHLAST_UNGEBR")
                    m_ZZLEISTUNGSGEW = myProxy.getExportParameter("ZZLEISTUNGSGEW")
                    m_ZZLAENGEMIN = myProxy.getExportParameter("ZZLAENGEMIN")
                    m_ZZLAENGEMAX = myProxy.getExportParameter("ZZLAENGEMAX")
                    m_ZZBREITEMIN = myProxy.getExportParameter("ZZBREITEMIN")
                    m_ZZBREITEMAX = myProxy.getExportParameter("ZZBREITEMAX")
                    m_ZZHOEHEMIN = myProxy.getExportParameter("ZZHOEHEMIN")
                    m_ZZHOEHEMAX = myProxy.getExportParameter("ZZHOEHEMAX")
                    m_ZZSTUETZLAST = myProxy.getExportParameter("ZZSTUETZLAST")
                    m_ZZBEREIFACHSE1 = myProxy.getExportParameter("ZZBEREIFACHSE1")
                    m_ZZBEREIFACHSE2 = myProxy.getExportParameter("ZZBEREIFACHSE2")
                    m_ZZBEREIFACHSE3 = myProxy.getExportParameter("ZZBEREIFACHSE3")
                    m_ZZGENEHMIGNR = myProxy.getExportParameter("ZZGENEHMIGNR")
                    m_ZZGENEHMIGDAT = myProxy.getExportParameter("ZZGENEHMIGDAT")
                    m_ZFARBE_KLAR = myProxy.getExportParameter("ZFARBE_KLAR")
                    m_ZZFHRZKLASSE_TXT = myProxy.getExportParameter("ZZFHRZKLASSE_TXT")

                    If Not strZZCHASSIS_NUM.Length = 0 Then
                        m_objABE_Daten.Fahrgestellnummer = strZZCHASSIS_NUM
                    Else
                        m_objABE_Daten.Fahrgestellnummer = "-"
                    End If

                    m_objABE_Daten.Bemerkung = ""

                    If m_objABE_Daten.Bemerkung.Length = 0 Then
                        m_objABE_Daten.Bemerkung = "-"
                    End If

                    If Not strZZFARBE.Length = 0 Then
                        m_objABE_Daten.Farbziffer = strZZFARBE
                    Else
                        m_objABE_Daten.Farbziffer = "-"
                    End If
                    '---------------------------------------------------------------
                    With m_objABE_Daten
                        .ZZABGASRICHTL_TG = m_ZZABGASRICHTL_TG
                        .ZZACHSL_A1_STA = m_ZZACHSL_A1_STA
                        .ZZACHSL_A2_STA = m_ZZACHSL_A2_STA
                        .ZZACHSL_A3_STA = m_ZZACHSL_A3_STA
                        .ZZACHSLST_ACHSE1 = m_ZZACHSLST_ACHSE1
                        .ZZACHSLST_ACHSE2 = m_ZZACHSLST_ACHSE2
                        .ZZACHSLST_ACHSE3 = m_ZZACHSLST_ACHSE3
                        .ZZANHLAST_GEBR = m_ZZANHLAST_GEBR
                        .ZZANHLAST_UNGEBR = m_ZZANHLAST_UNGEBR
                        .ZZANTRIEBSACHS = m_ZZANTRIEBSACHS
                        .ZZANZACHS = m_ZZANZACHS
                        .ZZANZSITZE = m_ZZANZSITZE
                        .ZZANZSTEHPLAETZE = m_ZZANZSTEHPLAETZE
                        .ZZBEIUMDREH = m_ZZBEIUMDREH
                        .ZZBEREIFACHSE1 = m_ZZBEREIFACHSE1
                        .ZZBEREIFACHSE2 = m_ZZBEREIFACHSE2
                        .ZZBEREIFACHSE3 = m_ZZBEREIFACHSE3
                        .ZZBREITEMAX = m_ZZBREITEMAX
                        .ZZBREITEMIN = m_ZZBREITEMIN
                        .ZZCO2KOMBI = m_ZZCO2KOMBI
                        .ZZCODE_AUFBAU = m_ZZCODE_AUFBAU
                        .ZZCODE_KRAFTSTOF = m_ZZCODE_KRAFTSTOF
                        .ZZDREHZSTANDGER = m_ZZDREHZSTANDGER
                        .ZZFABRIKNAME = m_ZZFABRIKNAME
                        .ZZFAHRGERAEUSCH = m_ZZFAHRGERAEUSCH
                        .ZZFAHRZEUGKLASSE = m_ZZFAHRZEUGKLASSE
                        .ZZFASSVERMOEGEN = m_ZZFASSVERMOEGEN
                        .ZZFHRZKLASSE_TXT = m_ZZFHRZKLASSE_TXT
                        .ZZGENEHMIGDAT = m_ZZGENEHMIGDAT
                        .ZZGENEHMIGNR = m_ZZGENEHMIGNR
                        .ZZHANDELSNAME = m_ZZHANDELSNAME
                        .ZZHERST_TEXT = m_ZZHERST_TEXT
                        .ZZHERSTELLER_SCH = m_ZZHERSTELLER_SCH
                        .ZZHOECHSTGESCHW = m_ZZHOECHSTGESCHW
                        .ZZHOEHEMAX = m_ZZHOEHEMAX
                        .ZZHOEHEMIN = m_ZZHOEHEMIN
                        .ZZHUBRAUM = m_ZZHUBRAUM
                        .ZZKLARTEXT_TYP = m_ZZKLARTEXT_TYP
                        .ZZKRAFTSTOFF_TXT = m_ZZKRAFTSTOFF_TXT
                        .ZZLAENGEMAX = m_ZZLAENGEMAX
                        .ZZLAENGEMIN = m_ZZLAENGEMIN
                        .ZZLEISTUNGSGEW = m_ZZLEISTUNGSGEW
                        .ZZMASSEFAHRBMAX = m_ZZMASSEFAHRBMAX
                        .ZZMASSEFAHRBMIN = m_ZZMASSEFAHRBMIN
                        .ZZNATIONALE_EMIK = m_ZZNATIONALE_EMIK
                        .ZZNENNLEISTUNG = m_ZZNENNLEISTUNG
                        .ZZSLD = m_ZZSLD
                        .ZZSTANDGERAEUSCH = m_ZZSTANDGERAEUSCH
                        .ZZSTUETZLAST = m_ZZSTUETZLAST
                        .ZZTEXT_AUFBAU = m_ZZTEXT_AUFBAU
                        .ZZTYP_SCHL = m_ZZTYP_SCHL
                        .ZZTYP_VVS_PRUEF = m_ZZTYP_VVS_PRUEF
                        .ZZVARIANTE = m_ZZVARIANTE
                        .ZZVERSION = m_ZZVERSION
                        .ZZVVS_SCHLUESSEL = m_ZZVVS_SCHLUESSEL
                        .ZZZULGESGEW = m_ZZZULGESGEW
                        .ZZZULGESGEWSTAAT = m_ZZZULGESGEWSTAAT
                        .ZZFARBE = m_ZFARBE_KLAR
                    End With

                    WriteLogEntry(True, "ZZKUNNR=" & m_strCustomer.TrimStart("0"c) & ", ZZEQUNR=" & strEquNr, m_tblResult)

                Catch ex As Exception
                    Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                        Case "ERR_INV_KUNNR"
                            'ungültige Kundennummer
                            m_intStatus = -1
                            m_strMessage = "Ungültige Kundennummer"
                        Case "ERR_INV_EQUNR"
                            'ungültige Equipmentnummer
                            m_intStatus = -1
                            m_strMessage = "Ungültige Fahrzeugnummer"
                        Case "ERR_INV_KOMBI"
                            'ungültige Kombination Kundennummer und Equipmentnummer
                            m_intStatus = -1
                            m_strMessage = "Ungültige Kombination von Kundennummer und Fahrzeugnummer"
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = ex.Message
                    End Select
                    WriteLogEntry(False, "ZZKUNNR=" & m_strCustomer.TrimStart("0"c) & ", ZZEQUNR=" & strEquNr & " , " & Replace(m_strMessage, "<br>", " "), m_tblResult)
                Finally
                    m_blnGestartet = False
                End Try
            End If
        End Sub
        Public Overrides Sub Show()

        End Sub

        Public Overrides Sub Change()

        End Sub
#End Region
    End Class
End Namespace

' ************************************************
' $History: ABEDaten.vb $
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 18.12.09   Time: 15:37
' Updated in $/CKAG/Base/Business
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 18.12.09   Time: 13:47
' Updated in $/CKAG/Base/Business
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 18.12.09   Time: 8:56
' Updated in $/CKAG/Base/Business
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 15.12.09   Time: 17:23
' Updated in $/CKAG/Base/Business
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Base/Business
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Business
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 15:39
' Updated in $/CKG/Base/Base/Business
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 6  *****************
' User: Uha          Date: 22.05.07   Time: 9:31
' Updated in $/CKG/Base/Base/Business
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 5  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Business
' 
' ************************************************