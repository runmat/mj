Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Common

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

                'm_strCustomer = Right("0000000000" & strCustomer, 10)
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

                    'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ABEZUFZG", m_objApp, m_objUser, page)

                    'myProxy.setImportParameter("ZZKUNNR", m_strKUNNR)
                    'myProxy.setImportParameter("ZZEQUNR", strEquNr)

                    'myProxy.callBapi()

                    S.AP.InitExecute("Z_M_ABEZUFZG", "ZZKUNNR, ZZEQUNR", KUNNR, strEquNr)

                    'strZZCHASSIS_NUM = myProxy.getExportParameter("ZZCHASSIS_NUM")
                    'strZZFARBE = myProxy.getExportParameter("ZZFARBE")
                    'm_ZZHERSTELLER_SCH = myProxy.getExportParameter("ZZHERSTELLER_SCH")
                    'm_ZZTYP_SCHL = myProxy.getExportParameter("ZZTYP_SCHL")
                    'm_ZZVVS_SCHLUESSEL = myProxy.getExportParameter("ZZVVS_SCHLUESSEL")
                    'm_ZZTYP_VVS_PRUEF = myProxy.getExportParameter("ZZTYP_VVS_PRUEF")
                    'm_ZZFAHRZEUGKLASSE = myProxy.getExportParameter("ZZFAHRZEUGKLASSE")
                    'm_ZZCODE_AUFBAU = myProxy.getExportParameter("ZZCODE_AUFBAU")
                    'm_ZZFABRIKNAME = myProxy.getExportParameter("ZZFABRIKNAME")
                    'm_ZZKLARTEXT_TYP = myProxy.getExportParameter("ZZKLARTEXT_TYP")
                    'm_ZZVARIANTE = myProxy.getExportParameter("ZZVARIANTE")
                    'm_ZZVERSION = myProxy.getExportParameter("ZZVERSION")
                    'm_ZZHANDELSNAME = myProxy.getExportParameter("ZZHANDELSNAME")
                    'm_ZZHERST_TEXT = myProxy.getExportParameter("ZZHERST_TEXT")
                    'm_ZZTEXT_AUFBAU = myProxy.getExportParameter("ZZFHRZKLASSE_TXT")
                    'm_ZZTEXT_AUFBAU = myProxy.getExportParameter("ZZTEXT_AUFBAU")
                    'm_ZZABGASRICHTL_TG = myProxy.getExportParameter("ZZABGASRICHTL_TG")
                    'm_ZZNATIONALE_EMIK = myProxy.getExportParameter("ZZNATIONALE_EMIK")
                    'm_ZZKRAFTSTOFF_TXT = myProxy.getExportParameter("ZZKRAFTSTOFF_TXT")
                    'm_ZZCODE_KRAFTSTOF = myProxy.getExportParameter("ZZCODE_KRAFTSTOF")
                    'm_ZZSLD = myProxy.getExportParameter("ZZSLD")
                    'm_ZZHUBRAUM = myProxy.getExportParameter("ZZHUBRAUM")
                    'm_ZZANZACHS = myProxy.getExportParameter("ZZANZACHS")
                    'm_ZZANTRIEBSACHS = myProxy.getExportParameter("ZZANTRIEBSACHS")
                    'm_ZZNENNLEISTUNG = myProxy.getExportParameter("ZZNENNLEISTUNG")
                    'm_ZZBEIUMDREH = myProxy.getExportParameter("ZZBEIUMDREH")
                    'm_ZZHOECHSTGESCHW = myProxy.getExportParameter("ZZHOECHSTGESCHW")
                    'm_ZZFASSVERMOEGEN = myProxy.getExportParameter("ZZFASSVERMOEGEN")
                    'm_ZZANZSITZE = myProxy.getExportParameter("ZZANZSITZE")
                    'm_ZZANZSTEHPLAETZE = myProxy.getExportParameter("ZZANZSTEHPLAETZE")
                    'm_ZZMASSEFAHRBMIN = myProxy.getExportParameter("ZZMASSEFAHRBMIN")
                    'm_ZZMASSEFAHRBMAX = myProxy.getExportParameter("ZZMASSEFAHRBMAX")
                    'm_ZZZULGESGEW = myProxy.getExportParameter("ZZZULGESGEW")
                    'm_ZZZULGESGEWSTAAT = myProxy.getExportParameter("ZZZULGESGEWSTAAT")

                    'm_ZZACHSLST_ACHSE1 = myProxy.getExportParameter("ZZACHSLST_ACHSE1")
                    'm_ZZACHSLST_ACHSE2 = myProxy.getExportParameter("ZZACHSLST_ACHSE2")
                    'm_ZZACHSLST_ACHSE3 = myProxy.getExportParameter("ZZACHSLST_ACHSE3")
                    'm_ZZACHSL_A1_STA = myProxy.getExportParameter("ZZACHSL_A1_STA")
                    'm_ZZACHSL_A2_STA = myProxy.getExportParameter("ZZACHSL_A2_STA")
                    'm_ZZACHSL_A3_STA = myProxy.getExportParameter("ZZACHSL_A3_STA")
                    'm_ZZCO2KOMBI = myProxy.getExportParameter("ZZCO2KOMBI")
                    'm_ZZSTANDGERAEUSCH = myProxy.getExportParameter("ZZSTANDGERAEUSCH")
                    'm_ZZDREHZSTANDGER = myProxy.getExportParameter("ZZDREHZSTANDGER")
                    'm_ZZFAHRGERAEUSCH = myProxy.getExportParameter("ZZFAHRGERAEUSCH")
                    'm_ZZANHLAST_GEBR = myProxy.getExportParameter("ZZANHLAST_GEBR")
                    'm_ZZLEISTUNGSGEW = myProxy.getExportParameter("ZZANHLAST_UNGEBR")
                    'm_ZZLEISTUNGSGEW = myProxy.getExportParameter("ZZLEISTUNGSGEW")
                    'm_ZZLAENGEMIN = myProxy.getExportParameter("ZZLAENGEMIN")
                    'm_ZZLAENGEMAX = myProxy.getExportParameter("ZZLAENGEMAX")
                    'm_ZZBREITEMIN = myProxy.getExportParameter("ZZBREITEMIN")
                    'm_ZZBREITEMAX = myProxy.getExportParameter("ZZBREITEMAX")
                    'm_ZZHOEHEMIN = myProxy.getExportParameter("ZZHOEHEMIN")
                    'm_ZZHOEHEMAX = myProxy.getExportParameter("ZZHOEHEMAX")
                    'm_ZZSTUETZLAST = myProxy.getExportParameter("ZZSTUETZLAST")
                    'm_ZZBEREIFACHSE1 = myProxy.getExportParameter("ZZBEREIFACHSE1")
                    'm_ZZBEREIFACHSE2 = myProxy.getExportParameter("ZZBEREIFACHSE2")
                    'm_ZZBEREIFACHSE3 = myProxy.getExportParameter("ZZBEREIFACHSE3")
                    'm_ZZGENEHMIGNR = myProxy.getExportParameter("ZZGENEHMIGNR")
                    'm_ZZGENEHMIGDAT = myProxy.getExportParameter("ZZGENEHMIGDAT")
                    'm_ZFARBE_KLAR = myProxy.getExportParameter("ZFARBE_KLAR")
                    'm_ZZFHRZKLASSE_TXT = myProxy.getExportParameter("ZZFHRZKLASSE_TXT")

                    strZZCHASSIS_NUM = S.AP.GetExportParameter("ZZCHASSIS_NUM")
                    strZZFARBE = S.AP.GetExportParameter("ZZFARBE")
                    m_ZZHERSTELLER_SCH = S.AP.GetExportParameter("ZZHERSTELLER_SCH")
                    m_ZZTYP_SCHL = S.AP.GetExportParameter("ZZTYP_SCHL")
                    m_ZZVVS_SCHLUESSEL = S.AP.GetExportParameter("ZZVVS_SCHLUESSEL")
                    m_ZZTYP_VVS_PRUEF = S.AP.GetExportParameter("ZZTYP_VVS_PRUEF")
                    m_ZZFAHRZEUGKLASSE = S.AP.GetExportParameter("ZZFAHRZEUGKLASSE")
                    m_ZZCODE_AUFBAU = S.AP.GetExportParameter("ZZCODE_AUFBAU")
                    m_ZZFABRIKNAME = S.AP.GetExportParameter("ZZFABRIKNAME")
                    m_ZZKLARTEXT_TYP = S.AP.GetExportParameter("ZZKLARTEXT_TYP")
                    m_ZZVARIANTE = S.AP.GetExportParameter("ZZVARIANTE")
                    m_ZZVERSION = S.AP.GetExportParameter("ZZVERSION")
                    m_ZZHANDELSNAME = S.AP.GetExportParameter("ZZHANDELSNAME")
                    m_ZZHERST_TEXT = S.AP.GetExportParameter("ZZHERST_TEXT")
                    m_ZZTEXT_AUFBAU = S.AP.GetExportParameter("ZZTEXT_AUFBAU")
                    m_ZZABGASRICHTL_TG = S.AP.GetExportParameter("ZZABGASRICHTL_TG")
                    m_ZZNATIONALE_EMIK = S.AP.GetExportParameter("ZZNATIONALE_EMIK")
                    m_ZZKRAFTSTOFF_TXT = S.AP.GetExportParameter("ZZKRAFTSTOFF_TXT")
                    m_ZZCODE_KRAFTSTOF = S.AP.GetExportParameter("ZZCODE_KRAFTSTOF")
                    m_ZZSLD = S.AP.GetExportParameter("ZZSLD")
                    m_ZZHUBRAUM = S.AP.GetExportParameter("ZZHUBRAUM")
                    m_ZZANZACHS = S.AP.GetExportParameter("ZZANZACHS")
                    m_ZZANTRIEBSACHS = S.AP.GetExportParameter("ZZANTRIEBSACHS")
                    m_ZZNENNLEISTUNG = S.AP.GetExportParameter("ZZNENNLEISTUNG")
                    m_ZZBEIUMDREH = S.AP.GetExportParameter("ZZBEIUMDREH")
                    m_ZZHOECHSTGESCHW = S.AP.GetExportParameter("ZZHOECHSTGESCHW")
                    m_ZZFASSVERMOEGEN = S.AP.GetExportParameter("ZZFASSVERMOEGEN")
                    m_ZZANZSITZE = S.AP.GetExportParameter("ZZANZSITZE")
                    m_ZZANZSTEHPLAETZE = S.AP.GetExportParameter("ZZANZSTEHPLAETZE")
                    m_ZZMASSEFAHRBMIN = S.AP.GetExportParameter("ZZMASSEFAHRBMIN")
                    m_ZZMASSEFAHRBMAX = S.AP.GetExportParameter("ZZMASSEFAHRBMAX")
                    m_ZZZULGESGEW = S.AP.GetExportParameter("ZZZULGESGEW")
                    m_ZZZULGESGEWSTAAT = S.AP.GetExportParameter("ZZZULGESGEWSTAAT")

                    m_ZZACHSLST_ACHSE1 = S.AP.GetExportParameter("ZZACHSLST_ACHSE1")
                    m_ZZACHSLST_ACHSE2 = S.AP.GetExportParameter("ZZACHSLST_ACHSE2")
                    m_ZZACHSLST_ACHSE3 = S.AP.GetExportParameter("ZZACHSLST_ACHSE3")
                    m_ZZACHSL_A1_STA = S.AP.GetExportParameter("ZZACHSL_A1_STA")
                    m_ZZACHSL_A2_STA = S.AP.GetExportParameter("ZZACHSL_A2_STA")
                    m_ZZACHSL_A3_STA = S.AP.GetExportParameter("ZZACHSL_A3_STA")
                    m_ZZCO2KOMBI = S.AP.GetExportParameter("ZZCO2KOMBI")
                    m_ZZSTANDGERAEUSCH = S.AP.GetExportParameter("ZZSTANDGERAEUSCH")
                    m_ZZDREHZSTANDGER = S.AP.GetExportParameter("ZZDREHZSTANDGER")
                    m_ZZFAHRGERAEUSCH = S.AP.GetExportParameter("ZZFAHRGERAEUSCH")
                    m_ZZANHLAST_GEBR = S.AP.GetExportParameter("ZZANHLAST_GEBR")
                    m_ZZANHLAST_UNGEBR = S.AP.GetExportParameter("ZZANHLAST_UNGEBR")
                    m_ZZLEISTUNGSGEW = S.AP.GetExportParameter("ZZLEISTUNGSGEW")
                    m_ZZLAENGEMIN = S.AP.GetExportParameter("ZZLAENGEMIN")
                    m_ZZLAENGEMAX = S.AP.GetExportParameter("ZZLAENGEMAX")
                    m_ZZBREITEMIN = S.AP.GetExportParameter("ZZBREITEMIN")
                    m_ZZBREITEMAX = S.AP.GetExportParameter("ZZBREITEMAX")
                    m_ZZHOEHEMIN = S.AP.GetExportParameter("ZZHOEHEMIN")
                    m_ZZHOEHEMAX = S.AP.GetExportParameter("ZZHOEHEMAX")
                    m_ZZSTUETZLAST = S.AP.GetExportParameter("ZZSTUETZLAST")
                    m_ZZBEREIFACHSE1 = S.AP.GetExportParameter("ZZBEREIFACHSE1")
                    m_ZZBEREIFACHSE2 = S.AP.GetExportParameter("ZZBEREIFACHSE2")
                    m_ZZBEREIFACHSE3 = S.AP.GetExportParameter("ZZBEREIFACHSE3")
                    m_ZZGENEHMIGNR = S.AP.GetExportParameter("ZZGENEHMIGNR")
                    m_ZZGENEHMIGDAT = S.AP.GetExportParameter("ZZGENEHMIGDAT")
                    m_ZFARBE_KLAR = S.AP.GetExportParameter("ZFARBE_KLAR")
                    m_ZZFHRZKLASSE_TXT = S.AP.GetExportParameter("ZZFHRZKLASSE_TXT")

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

                    WriteLogEntry(True, "ZZKUNNR=" & m_objUser.KUNNR & ", ZZEQUNR=" & strEquNr, m_tblResult)

                Catch ex As Exception
                    Select Case ex.Message.Replace("Execution failed", "").Trim()
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
                    WriteLogEntry(False, "ZZKUNNR=" & m_objUser.KUNNR & ", ZZEQUNR=" & strEquNr & " , " & Replace(m_strMessage, "<br>", " "), m_tblResult)
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