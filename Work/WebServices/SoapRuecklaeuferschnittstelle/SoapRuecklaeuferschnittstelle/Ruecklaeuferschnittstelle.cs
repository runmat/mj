using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoapRuecklaeuferschnittstelle
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Ruecklaeuferschnittstelle
    {

        private System.DateTime zuletzt_aktualisiertField;

        private string fehlercodeField;

        private RuecklaeuferschnittstelleStammdaten stammdatenField;

        private RuecklaeuferschnittstelleAbholung abholungField;

        private RuecklaeuferschnittstelleRueckgabeprotokoll rueckgabeprotokollField;

        private RuecklaeuferschnittstelleStilllegung_erfolgt stilllegung_erfolgtField;

        private RuecklaeuferschnittstelleAufbereitung aufbereitungField;

        private RuecklaeuferschnittstelleVerwertung verwertungField;

        private RuecklaeuferschnittstelleTransport[] transportField;

        private RuecklaeuferschnittstelleProzessende prozessendeField;

        private long vorgangsidField;

        /// <remarks/>
        public System.DateTime zuletzt_aktualisiert
        {
            get
            {
                return this.zuletzt_aktualisiertField;
            }
            set
            {
                this.zuletzt_aktualisiertField = value;
            }
        }

        /// <remarks/>
        public string fehlercode
        {
            get
            {
                return this.fehlercodeField;
            }
            set
            {
                this.fehlercodeField = value;
            }
        }

        /// <remarks/>
        public RuecklaeuferschnittstelleStammdaten Stammdaten
        {
            get
            {
                return this.stammdatenField;
            }
            set
            {
                this.stammdatenField = value;
            }
        }

        /// <remarks/>
        public RuecklaeuferschnittstelleAbholung Abholung
        {
            get
            {
                return this.abholungField;
            }
            set
            {
                this.abholungField = value;
            }
        }

        /// <remarks/>
        public RuecklaeuferschnittstelleRueckgabeprotokoll Rueckgabeprotokoll
        {
            get
            {
                return this.rueckgabeprotokollField;
            }
            set
            {
                this.rueckgabeprotokollField = value;
            }
        }

        /// <remarks/>
        public RuecklaeuferschnittstelleStilllegung_erfolgt Stilllegung_erfolgt
        {
            get
            {
                return this.stilllegung_erfolgtField;
            }
            set
            {
                this.stilllegung_erfolgtField = value;
            }
        }

        /// <remarks/>
        public RuecklaeuferschnittstelleAufbereitung Aufbereitung
        {
            get
            {
                return this.aufbereitungField;
            }
            set
            {
                this.aufbereitungField = value;
            }
        }

        /// <remarks/>
        public RuecklaeuferschnittstelleVerwertung Verwertung
        {
            get
            {
                return this.verwertungField;
            }
            set
            {
                this.verwertungField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Transport")]
        public RuecklaeuferschnittstelleTransport[] Transport
        {
            get
            {
                return this.transportField;
            }
            set
            {
                this.transportField = value;
            }
        }

        /// <remarks/>
        public RuecklaeuferschnittstelleProzessende Prozessende
        {
            get
            {
                return this.prozessendeField;
            }
            set
            {
                this.prozessendeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public long vorgangsid
        {
            get
            {
                return this.vorgangsidField;
            }
            set
            {
                this.vorgangsidField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleStammdaten
    {

        private string rueckgabeoptionField;

        private string kundennummerField;

        private string kundennameField;

        private string nutzerField;

        private string vertragsnummerField;

        private string kennzeichenField;

        private string fahrgestellnummerField;

        private System.DateTime erstzulassungField;

        private string herstellerField;

        private string serieField;

        private string treibstoffartField;

        private string aufbauartField;

        private string leistungField;

        private string winterreifenField;

        /// <remarks/>
        public string Rueckgabeoption
        {
            get
            {
                return this.rueckgabeoptionField;
            }
            set
            {
                this.rueckgabeoptionField = value;
            }
        }

        /// <remarks/>
        public string Kundennummer
        {
            get
            {
                return this.kundennummerField;
            }
            set
            {
                this.kundennummerField = value;
            }
        }

        /// <remarks/>
        public string Kundenname
        {
            get
            {
                return this.kundennameField;
            }
            set
            {
                this.kundennameField = value;
            }
        }

        /// <remarks/>
        public string Nutzer
        {
            get
            {
                return this.nutzerField;
            }
            set
            {
                this.nutzerField = value;
            }
        }

        /// <remarks/>
        public string Vertragsnummer
        {
            get
            {
                return this.vertragsnummerField;
            }
            set
            {
                this.vertragsnummerField = value;
            }
        }

        /// <remarks/>
        public string Kennzeichen
        {
            get
            {
                return this.kennzeichenField;
            }
            set
            {
                this.kennzeichenField = value;
            }
        }

        /// <remarks/>
        public string Fahrgestellnummer
        {
            get
            {
                return this.fahrgestellnummerField;
            }
            set
            {
                this.fahrgestellnummerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime Erstzulassung
        {
            get
            {
                return this.erstzulassungField;
            }
            set
            {
                this.erstzulassungField = value;
            }
        }

        /// <remarks/>
        public string Hersteller
        {
            get
            {
                return this.herstellerField;
            }
            set
            {
                this.herstellerField = value;
            }
        }

        /// <remarks/>
        public string Serie
        {
            get
            {
                return this.serieField;
            }
            set
            {
                this.serieField = value;
            }
        }

        /// <remarks/>
        public string Treibstoffart
        {
            get
            {
                return this.treibstoffartField;
            }
            set
            {
                this.treibstoffartField = value;
            }
        }

        /// <remarks/>
        public string Aufbauart
        {
            get
            {
                return this.aufbauartField;
            }
            set
            {
                this.aufbauartField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "positiveInteger")]
        public string Leistung
        {
            get
            {
                return this.leistungField;
            }
            set
            {
                this.leistungField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string Winterreifen
        {
            get
            {
                return this.winterreifenField;
            }
            set
            {
                this.winterreifenField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleAbholung
    {

        private RuecklaeuferschnittstelleAbholungAbholauftrag abholauftragField;

        private RuecklaeuferschnittstelleAbholungAbholtermin_bestaetigt abholtermin_bestaetigtField;

        private RuecklaeuferschnittstelleAbholungEingang_Zielort eingang_ZielortField;

        /// <remarks/>
        public RuecklaeuferschnittstelleAbholungAbholauftrag Abholauftrag
        {
            get
            {
                return this.abholauftragField;
            }
            set
            {
                this.abholauftragField = value;
            }
        }

        /// <remarks/>
        public RuecklaeuferschnittstelleAbholungAbholtermin_bestaetigt Abholtermin_bestaetigt
        {
            get
            {
                return this.abholtermin_bestaetigtField;
            }
            set
            {
                this.abholtermin_bestaetigtField = value;
            }
        }

        /// <remarks/>
        public RuecklaeuferschnittstelleAbholungEingang_Zielort Eingang_Zielort
        {
            get
            {
                return this.eingang_ZielortField;
            }
            set
            {
                this.eingang_ZielortField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleAbholungAbholauftrag
    {

        private System.DateTime zuletzt_aktualisiertField;

        private string transportartField;

        private System.DateTime datum_vonField;

        private System.DateTime datum_bisField;

        private string bemerkungField;

        private bool eigenanlieferungField;

        private bool? gutachten_erstellenField = true;

        private RuecklaeuferschnittstelleAbholungAbholauftragAdressen adressenField;

        /// <remarks/>
        public System.DateTime zuletzt_aktualisiert
        {
            get
            {
                return this.zuletzt_aktualisiertField;
            }
            set
            {
                this.zuletzt_aktualisiertField = value;
            }
        }

        /// <remarks/>
        public string Transportart
        {
            get
            {
                return this.transportartField;
            }
            set
            {
                this.transportartField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime Datum_von
        {
            get
            {
                return this.datum_vonField;
            }
            set
            {
                this.datum_vonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime Datum_bis
        {
            get
            {
                return this.datum_bisField;
            }
            set
            {
                this.datum_bisField = value;
            }
        }

        /// <remarks/>
        public string Bemerkung
        {
            get
            {
                return this.bemerkungField;
            }
            set
            {
                this.bemerkungField = value;
            }
        }

        /// <remarks/>
        public bool Eigenanlieferung
        {
            get
            {
                return this.eigenanlieferungField;
            }
            set
            {
                this.eigenanlieferungField = value;
            }
        }

        /// <remarks/>
        public bool? Gutachten_erstellen
        {
            get
            {
                return this.gutachten_erstellenField;
            }
            set
            {
                this.gutachten_erstellenField = value;
            }
        }



        /// <remarks/>
        public RuecklaeuferschnittstelleAbholungAbholauftragAdressen Adressen
        {
            get
            {
                return this.adressenField;
            }
            set
            {
                this.adressenField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleAbholungAbholauftragAdressen
    {

        private RuecklaeuferschnittstelleAbholungAbholauftragAdressenAbholort abholortField;

        private RuecklaeuferschnittstelleAbholungAbholauftragAdressenZielort zielortField;

        /// <remarks/>
        public RuecklaeuferschnittstelleAbholungAbholauftragAdressenAbholort Abholort
        {
            get
            {
                return this.abholortField;
            }
            set
            {
                this.abholortField = value;
            }
        }

        /// <remarks/>
        public RuecklaeuferschnittstelleAbholungAbholauftragAdressenZielort Zielort
        {
            get
            {
                return this.zielortField;
            }
            set
            {
                this.zielortField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleAbholungAbholauftragAdressenAbholort
    {

        private string firmaField;

        private string strasseField;

        private string postleitzahlField;

        private string ortField;

        private string ansprechpartnerField;

        private string telefonField;

        private string emailField;

        /// <remarks/>
        public string Firma
        {
            get
            {
                return this.firmaField;
            }
            set
            {
                this.firmaField = value;
            }
        }

        /// <remarks/>
        public string Strasse
        {
            get
            {
                return this.strasseField;
            }
            set
            {
                this.strasseField = value;
            }
        }

        /// <remarks/>
        public string Postleitzahl
        {
            get
            {
                return this.postleitzahlField;
            }
            set
            {
                this.postleitzahlField = value;
            }
        }

        /// <remarks/>
        public string Ort
        {
            get
            {
                return this.ortField;
            }
            set
            {
                this.ortField = value;
            }
        }

        /// <remarks/>
        public string Ansprechpartner
        {
            get
            {
                return this.ansprechpartnerField;
            }
            set
            {
                this.ansprechpartnerField = value;
            }
        }

        /// <remarks/>
        public string Telefon
        {
            get
            {
                return this.telefonField;
            }
            set
            {
                this.telefonField = value;
            }
        }

        /// <remarks/>
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleAbholungAbholauftragAdressenZielort
    {

        private string firmaField;

        private string strasseField;

        private string postleitzahlField;

        private string ortField;

        private string ansprechpartnerField;

        private string telefonField;

        private string emailField;

        /// <remarks/>
        public string Firma
        {
            get
            {
                return this.firmaField;
            }
            set
            {
                this.firmaField = value;
            }
        }

        /// <remarks/>
        public string Strasse
        {
            get
            {
                return this.strasseField;
            }
            set
            {
                this.strasseField = value;
            }
        }

        /// <remarks/>
        public string Postleitzahl
        {
            get
            {
                return this.postleitzahlField;
            }
            set
            {
                this.postleitzahlField = value;
            }
        }

        /// <remarks/>
        public string Ort
        {
            get
            {
                return this.ortField;
            }
            set
            {
                this.ortField = value;
            }
        }

        /// <remarks/>
        public string Ansprechpartner
        {
            get
            {
                return this.ansprechpartnerField;
            }
            set
            {
                this.ansprechpartnerField = value;
            }
        }

        /// <remarks/>
        public string Telefon
        {
            get
            {
                return this.telefonField;
            }
            set
            {
                this.telefonField = value;
            }
        }

        /// <remarks/>
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleAbholungAbholtermin_bestaetigt
    {

        private System.DateTime zuletzt_aktualisiertField;

        private System.DateTime bestaetigter_AbholterminField;

        /// <remarks/>
        public System.DateTime zuletzt_aktualisiert
        {
            get
            {
                return this.zuletzt_aktualisiertField;
            }
            set
            {
                this.zuletzt_aktualisiertField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime Bestaetigter_Abholtermin
        {
            get
            {
                return this.bestaetigter_AbholterminField;
            }
            set
            {
                this.bestaetigter_AbholterminField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleAbholungEingang_Zielort
    {

        private System.DateTime zuletzt_aktualisiertField;

        private System.DateTime datumField;

        private string firmaField;

        private string strasseField;

        private string postleitzahlField;

        private string ortField;

        private string ansprechpartnerField;

        private string telefonField;

        private string emailField;

        private string protokoll_AbholungField;

        private string protokoll_ZielortField;

        /// <remarks/>
        public System.DateTime zuletzt_aktualisiert
        {
            get
            {
                return this.zuletzt_aktualisiertField;
            }
            set
            {
                this.zuletzt_aktualisiertField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime Datum
        {
            get
            {
                return this.datumField;
            }
            set
            {
                this.datumField = value;
            }
        }

        /// <remarks/>
        public string Firma
        {
            get
            {
                return this.firmaField;
            }
            set
            {
                this.firmaField = value;
            }
        }

        /// <remarks/>
        public string Strasse
        {
            get
            {
                return this.strasseField;
            }
            set
            {
                this.strasseField = value;
            }
        }

        /// <remarks/>
        public string Postleitzahl
        {
            get
            {
                return this.postleitzahlField;
            }
            set
            {
                this.postleitzahlField = value;
            }
        }

        /// <remarks/>
        public string Ort
        {
            get
            {
                return this.ortField;
            }
            set
            {
                this.ortField = value;
            }
        }

        /// <remarks/>
        public string Ansprechpartner
        {
            get
            {
                return this.ansprechpartnerField;
            }
            set
            {
                this.ansprechpartnerField = value;
            }
        }

        /// <remarks/>
        public string Telefon
        {
            get
            {
                return this.telefonField;
            }
            set
            {
                this.telefonField = value;
            }
        }

        /// <remarks/>
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }

        /// <remarks/>
        public string Protokoll_Abholung
        {
            get
            {
                return this.protokoll_AbholungField;
            }
            set
            {
                this.protokoll_AbholungField = value;
            }
        }

        /// <remarks/>
        public string Protokoll_Zielort
        {
            get
            {
                return this.protokoll_ZielortField;
            }
            set
            {
                this.protokoll_ZielortField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleRueckgabeprotokoll
    {

        private System.DateTime zuletzt_aktualisiertField;

        private System.DateTime eingangsdatumField;

        private System.DateTime ruecknahmedatumField;

        private string ruecknahmezeitField;

        private string ruecknahmeortField;

        private string standortField;

        private string kMstand_RuecknahmeField;

        private string kMstand_nachTransferField;

        private bool zB1Field;

        private bool serviceheftField;

        private bool gueltigeHUField;

        private bool radioCodeKarteField;

        private bool naviCD_DVDField;

        private bool bordwerkzeugField;

        private bool reserveradField;

        private bool reifenReparaturSetField;

        private bool laderaumabdeckungField;

        private string ruecknahme_unter_Vorbehalt_GrundField;

        private bool ruecknahmeschaeden_innenField;

        private bool ruecknahmeschaeden_aussenField;

        private string bemerkungField;

        private string protokoll_AbholungField;

        /// <remarks/>
        public System.DateTime zuletzt_aktualisiert
        {
            get
            {
                return this.zuletzt_aktualisiertField;
            }
            set
            {
                this.zuletzt_aktualisiertField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime Eingangsdatum
        {
            get
            {
                return this.eingangsdatumField;
            }
            set
            {
                this.eingangsdatumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime Ruecknahmedatum
        {
            get
            {
                return this.ruecknahmedatumField;
            }
            set
            {
                this.ruecknahmedatumField = value;
            }
        }

        /// <remarks/>
        public string Ruecknahmezeit
        {
            get
            {
                return this.ruecknahmezeitField;
            }
            set
            {
                this.ruecknahmezeitField = value;
            }
        }

        /// <remarks/>
        public string Ruecknahmeort
        {
            get
            {
                return this.ruecknahmeortField;
            }
            set
            {
                this.ruecknahmeortField = value;
            }
        }

        /// <remarks/>
        public string Standort
        {
            get
            {
                return this.standortField;
            }
            set
            {
                this.standortField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("KM-stand_Ruecknahme", DataType = "positiveInteger")]
        public string KMstand_Ruecknahme
        {
            get
            {
                return this.kMstand_RuecknahmeField;
            }
            set
            {
                this.kMstand_RuecknahmeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("KM-stand_nachTransfer", DataType = "positiveInteger")]
        public string KMstand_nachTransfer
        {
            get
            {
                return this.kMstand_nachTransferField;
            }
            set
            {
                this.kMstand_nachTransferField = value;
            }
        }

        /// <remarks/>
        public bool ZB1
        {
            get
            {
                return this.zB1Field;
            }
            set
            {
                this.zB1Field = value;
            }
        }

        /// <remarks/>
        public bool Serviceheft
        {
            get
            {
                return this.serviceheftField;
            }
            set
            {
                this.serviceheftField = value;
            }
        }

        /// <remarks/>
        public bool GueltigeHU
        {
            get
            {
                return this.gueltigeHUField;
            }
            set
            {
                this.gueltigeHUField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Radio-Code-Karte")]
        public bool RadioCodeKarte
        {
            get
            {
                return this.radioCodeKarteField;
            }
            set
            {
                this.radioCodeKarteField = value;
            }
        }

        /// <remarks/>
        public bool NaviCD_DVD
        {
            get
            {
                return this.naviCD_DVDField;
            }
            set
            {
                this.naviCD_DVDField = value;
            }
        }

        /// <remarks/>
        public bool Bordwerkzeug
        {
            get
            {
                return this.bordwerkzeugField;
            }
            set
            {
                this.bordwerkzeugField = value;
            }
        }

        /// <remarks/>
        public bool Reserverad
        {
            get
            {
                return this.reserveradField;
            }
            set
            {
                this.reserveradField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Reifen-Reparatur-Set")]
        public bool ReifenReparaturSet
        {
            get
            {
                return this.reifenReparaturSetField;
            }
            set
            {
                this.reifenReparaturSetField = value;
            }
        }

        /// <remarks/>
        public bool Laderaumabdeckung
        {
            get
            {
                return this.laderaumabdeckungField;
            }
            set
            {
                this.laderaumabdeckungField = value;
            }
        }

        /// <remarks/>
        public string Ruecknahme_unter_Vorbehalt_Grund
        {
            get
            {
                return this.ruecknahme_unter_Vorbehalt_GrundField;
            }
            set
            {
                this.ruecknahme_unter_Vorbehalt_GrundField = value;
            }
        }

        /// <remarks/>
        public bool Ruecknahmeschaeden_innen
        {
            get
            {
                return this.ruecknahmeschaeden_innenField;
            }
            set
            {
                this.ruecknahmeschaeden_innenField = value;
            }
        }

        /// <remarks/>
        public bool Ruecknahmeschaeden_aussen
        {
            get
            {
                return this.ruecknahmeschaeden_aussenField;
            }
            set
            {
                this.ruecknahmeschaeden_aussenField = value;
            }
        }

        /// <remarks/>
        public string Bemerkung
        {
            get
            {
                return this.bemerkungField;
            }
            set
            {
                this.bemerkungField = value;
            }
        }

        /// <remarks/>
        public string Protokoll_Abholung
        {
            get
            {
                return this.protokoll_AbholungField;
            }
            set
            {
                this.protokoll_AbholungField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleStilllegung_erfolgt
    {

        private System.DateTime zuletzt_aktualisiertField;

        private System.DateTime stilllegungsterminField;

        /// <remarks/>
        public System.DateTime zuletzt_aktualisiert
        {
            get
            {
                return this.zuletzt_aktualisiertField;
            }
            set
            {
                this.zuletzt_aktualisiertField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime Stilllegungstermin
        {
            get
            {
                return this.stilllegungsterminField;
            }
            set
            {
                this.stilllegungsterminField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleAufbereitung
    {

        private RuecklaeuferschnittstelleAufbereitungAufbereitungsauftrag aufbereitungsauftragField;

        private RuecklaeuferschnittstelleAufbereitungAufbereitung_erfolgt aufbereitung_erfolgtField;

        /// <remarks/>
        public RuecklaeuferschnittstelleAufbereitungAufbereitungsauftrag Aufbereitungsauftrag
        {
            get
            {
                return this.aufbereitungsauftragField;
            }
            set
            {
                this.aufbereitungsauftragField = value;
            }
        }

        /// <remarks/>
        public RuecklaeuferschnittstelleAufbereitungAufbereitung_erfolgt Aufbereitung_erfolgt
        {
            get
            {
                return this.aufbereitung_erfolgtField;
            }
            set
            {
                this.aufbereitung_erfolgtField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleAufbereitungAufbereitungsauftrag
    {

        private System.DateTime zuletzt_aktualisiertField;

        private System.DateTime datumField;

        private string bemerkungField;

        private RuecklaeuferschnittstelleAufbereitungAufbereitungsauftragPosition[] reparaturenField;

        /// <remarks/>
        public System.DateTime zuletzt_aktualisiert
        {
            get
            {
                return this.zuletzt_aktualisiertField;
            }
            set
            {
                this.zuletzt_aktualisiertField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime Datum
        {
            get
            {
                return this.datumField;
            }
            set
            {
                this.datumField = value;
            }
        }

        /// <remarks/>
        public string Bemerkung
        {
            get
            {
                return this.bemerkungField;
            }
            set
            {
                this.bemerkungField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Position", IsNullable = false)]
        public RuecklaeuferschnittstelleAufbereitungAufbereitungsauftragPosition[] Reparaturen
        {
            get
            {
                return this.reparaturenField;
            }
            set
            {
                this.reparaturenField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleAufbereitungAufbereitungsauftragPosition
    {

        private string codeField;

        private string bezeichnungField;

        private string massnahmeField;

        private decimal reparaturkostenField;

        private string typField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string Bezeichnung
        {
            get
            {
                return this.bezeichnungField;
            }
            set
            {
                this.bezeichnungField = value;
            }
        }

        /// <remarks/>
        public string Massnahme
        {
            get
            {
                return this.massnahmeField;
            }
            set
            {
                this.massnahmeField = value;
            }
        }

        /// <remarks/>
        public decimal Reparaturkosten
        {
            get
            {
                return this.reparaturkostenField;
            }
            set
            {
                this.reparaturkostenField = value;
            }
        }

        /// <remarks/>
        public string Typ
        {
            get
            {
                return this.typField;
            }
            set
            {
                this.typField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleAufbereitungAufbereitung_erfolgt
    {

        private System.DateTime zuletzt_aktualisiertField;

        private System.DateTime aufbereitet_amField;

        /// <remarks/>
        public System.DateTime zuletzt_aktualisiert
        {
            get
            {
                return this.zuletzt_aktualisiertField;
            }
            set
            {
                this.zuletzt_aktualisiertField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime Aufbereitet_am
        {
            get
            {
                return this.aufbereitet_amField;
            }
            set
            {
                this.aufbereitet_amField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleVerwertung
    {

        private RuecklaeuferschnittstelleVerwertungVerwertungsentscheidung verwertungsentscheidungField;

        private RuecklaeuferschnittstelleVerwertungFahrzeug_eingestellt fahrzeug_eingestelltField;

        /// <remarks/>
        public RuecklaeuferschnittstelleVerwertungVerwertungsentscheidung Verwertungsentscheidung
        {
            get
            {
                return this.verwertungsentscheidungField;
            }
            set
            {
                this.verwertungsentscheidungField = value;
            }
        }

        /// <remarks/>
        public RuecklaeuferschnittstelleVerwertungFahrzeug_eingestellt Fahrzeug_eingestellt
        {
            get
            {
                return this.fahrzeug_eingestelltField;
            }
            set
            {
                this.fahrzeug_eingestelltField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleVerwertungVerwertungsentscheidung
    {

        private System.DateTime zuletzt_aktualisiertField;

        private string entscheidungField;

        private string verkaufskanalField;

        /// <remarks/>
        public System.DateTime zuletzt_aktualisiert
        {
            get
            {
                return this.zuletzt_aktualisiertField;
            }
            set
            {
                this.zuletzt_aktualisiertField = value;
            }
        }

        /// <remarks/>
        public string Entscheidung
        {
            get
            {
                return this.entscheidungField;
            }
            set
            {
                this.entscheidungField = value;
            }
        }

        /// <remarks/>
        public string Verkaufskanal
        {
            get
            {
                return this.verkaufskanalField;
            }
            set
            {
                this.verkaufskanalField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleVerwertungFahrzeug_eingestellt
    {

        private System.DateTime zuletzt_aktualisiertField;

        private string angebotsnummerField;

        private string linkField;

        /// <remarks/>
        public System.DateTime zuletzt_aktualisiert
        {
            get
            {
                return this.zuletzt_aktualisiertField;
            }
            set
            {
                this.zuletzt_aktualisiertField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "positiveInteger")]
        public string Angebotsnummer
        {
            get
            {
                return this.angebotsnummerField;
            }
            set
            {
                this.angebotsnummerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI")]
        public string link
        {
            get
            {
                return this.linkField;
            }
            set
            {
                this.linkField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleTransport
    {

        private RuecklaeuferschnittstelleTransportTransportauftrag transportauftragField;

        private RuecklaeuferschnittstelleTransportAbholtermin_bestaetigt abholtermin_bestaetigtField;

        private RuecklaeuferschnittstelleTransportEingang_Zielort eingang_ZielortField;

        /// <remarks/>
        public RuecklaeuferschnittstelleTransportTransportauftrag Transportauftrag
        {
            get
            {
                return this.transportauftragField;
            }
            set
            {
                this.transportauftragField = value;
            }
        }

        /// <remarks/>
        public RuecklaeuferschnittstelleTransportAbholtermin_bestaetigt Abholtermin_bestaetigt
        {
            get
            {
                return this.abholtermin_bestaetigtField;
            }
            set
            {
                this.abholtermin_bestaetigtField = value;
            }
        }

        /// <remarks/>
        public RuecklaeuferschnittstelleTransportEingang_Zielort Eingang_Zielort
        {
            get
            {
                return this.eingang_ZielortField;
            }
            set
            {
                this.eingang_ZielortField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleTransportTransportauftrag
    {

        private System.DateTime zuletzt_aktualisiertField;

        private string transportartField;

        private System.DateTime datum_vonField;

        private System.DateTime datum_bisField;

        private string bemerkungField;

        private long nummerField;

        private RuecklaeuferschnittstelleTransportTransportauftragAdressen adressenField;

        /// <remarks/>
        public System.DateTime zuletzt_aktualisiert
        {
            get
            {
                return this.zuletzt_aktualisiertField;
            }
            set
            {
                this.zuletzt_aktualisiertField = value;
            }
        }

        /// <remarks/>
        public string Transportart
        {
            get
            {
                return this.transportartField;
            }
            set
            {
                this.transportartField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime Datum_von
        {
            get
            {
                return this.datum_vonField;
            }
            set
            {
                this.datum_vonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime Datum_bis
        {
            get
            {
                return this.datum_bisField;
            }
            set
            {
                this.datum_bisField = value;
            }
        }

        /// <remarks/>
        public string Bemerkung
        {
            get
            {
                return this.bemerkungField;
            }
            set
            {
                this.bemerkungField = value;
            }
        }

        /// <remarks/>
        public long Nummer
        {
            get
            {
                return this.nummerField;
            }
            set
            {
                this.nummerField = value;
            }
        }

        /// <remarks/>
        public RuecklaeuferschnittstelleTransportTransportauftragAdressen Adressen
        {
            get
            {
                return this.adressenField;
            }
            set
            {
                this.adressenField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleTransportTransportauftragAdressen
    {

        private RuecklaeuferschnittstelleTransportTransportauftragAdressenAbholort abholortField;

        private RuecklaeuferschnittstelleTransportTransportauftragAdressenZielort zielortField;

        /// <remarks/>
        public RuecklaeuferschnittstelleTransportTransportauftragAdressenAbholort Abholort
        {
            get
            {
                return this.abholortField;
            }
            set
            {
                this.abholortField = value;
            }
        }

        /// <remarks/>
        public RuecklaeuferschnittstelleTransportTransportauftragAdressenZielort Zielort
        {
            get
            {
                return this.zielortField;
            }
            set
            {
                this.zielortField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleTransportTransportauftragAdressenAbholort
    {

        private string firmaField;

        private string strasseField;

        private string postleitzahlField;

        private string ortField;

        private string ansprechpartnerField;

        private string telefonField;

        private string emailField;

        /// <remarks/>
        public string Firma
        {
            get
            {
                return this.firmaField;
            }
            set
            {
                this.firmaField = value;
            }
        }

        /// <remarks/>
        public string Strasse
        {
            get
            {
                return this.strasseField;
            }
            set
            {
                this.strasseField = value;
            }
        }

        /// <remarks/>
        public string Postleitzahl
        {
            get
            {
                return this.postleitzahlField;
            }
            set
            {
                this.postleitzahlField = value;
            }
        }

        /// <remarks/>
        public string Ort
        {
            get
            {
                return this.ortField;
            }
            set
            {
                this.ortField = value;
            }
        }

        /// <remarks/>
        public string Ansprechpartner
        {
            get
            {
                return this.ansprechpartnerField;
            }
            set
            {
                this.ansprechpartnerField = value;
            }
        }

        /// <remarks/>
        public string Telefon
        {
            get
            {
                return this.telefonField;
            }
            set
            {
                this.telefonField = value;
            }
        }

        /// <remarks/>
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleTransportTransportauftragAdressenZielort
    {

        private string firmaField;

        private string strasseField;

        private string postleitzahlField;

        private string ortField;

        private string ansprechpartnerField;

        private string telefonField;

        private string emailField;

        /// <remarks/>
        public string Firma
        {
            get
            {
                return this.firmaField;
            }
            set
            {
                this.firmaField = value;
            }
        }

        /// <remarks/>
        public string Strasse
        {
            get
            {
                return this.strasseField;
            }
            set
            {
                this.strasseField = value;
            }
        }

        /// <remarks/>
        public string Postleitzahl
        {
            get
            {
                return this.postleitzahlField;
            }
            set
            {
                this.postleitzahlField = value;
            }
        }

        /// <remarks/>
        public string Ort
        {
            get
            {
                return this.ortField;
            }
            set
            {
                this.ortField = value;
            }
        }

        /// <remarks/>
        public string Ansprechpartner
        {
            get
            {
                return this.ansprechpartnerField;
            }
            set
            {
                this.ansprechpartnerField = value;
            }
        }

        /// <remarks/>
        public string Telefon
        {
            get
            {
                return this.telefonField;
            }
            set
            {
                this.telefonField = value;
            }
        }

        /// <remarks/>
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleTransportAbholtermin_bestaetigt
    {

        private System.DateTime zuletzt_aktualisiertField;

        private System.DateTime bestaetigter_AbholterminField;

        /// <remarks/>
        public System.DateTime zuletzt_aktualisiert
        {
            get
            {
                return this.zuletzt_aktualisiertField;
            }
            set
            {
                this.zuletzt_aktualisiertField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime Bestaetigter_Abholtermin
        {
            get
            {
                return this.bestaetigter_AbholterminField;
            }
            set
            {
                this.bestaetigter_AbholterminField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleTransportEingang_Zielort
    {

        private System.DateTime zuletzt_aktualisiertField;

        private System.DateTime datumField;

        private string firmaField;

        private string strasseField;

        private string postleitzahlField;

        private string ortField;

        private string ansprechpartnerField;

        private string telefonField;

        private string emailField;

        private string protokoll_AbholungField;

        private string protokoll_ZielortField;

        /// <remarks/>
        public System.DateTime zuletzt_aktualisiert
        {
            get
            {
                return this.zuletzt_aktualisiertField;
            }
            set
            {
                this.zuletzt_aktualisiertField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime Datum
        {
            get
            {
                return this.datumField;
            }
            set
            {
                this.datumField = value;
            }
        }

        /// <remarks/>
        public string Firma
        {
            get
            {
                return this.firmaField;
            }
            set
            {
                this.firmaField = value;
            }
        }

        /// <remarks/>
        public string Strasse
        {
            get
            {
                return this.strasseField;
            }
            set
            {
                this.strasseField = value;
            }
        }

        /// <remarks/>
        public string Postleitzahl
        {
            get
            {
                return this.postleitzahlField;
            }
            set
            {
                this.postleitzahlField = value;
            }
        }

        /// <remarks/>
        public string Ort
        {
            get
            {
                return this.ortField;
            }
            set
            {
                this.ortField = value;
            }
        }

        /// <remarks/>
        public string Ansprechpartner
        {
            get
            {
                return this.ansprechpartnerField;
            }
            set
            {
                this.ansprechpartnerField = value;
            }
        }

        /// <remarks/>
        public string Telefon
        {
            get
            {
                return this.telefonField;
            }
            set
            {
                this.telefonField = value;
            }
        }

        /// <remarks/>
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }

        /// <remarks/>
        public string Protokoll_Abholung
        {
            get
            {
                return this.protokoll_AbholungField;
            }
            set
            {
                this.protokoll_AbholungField = value;
            }
        }

        /// <remarks/>
        public string Protokoll_Zielort
        {
            get
            {
                return this.protokoll_ZielortField;
            }
            set
            {
                this.protokoll_ZielortField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RuecklaeuferschnittstelleProzessende
    {

        private System.DateTime zuletzt_aktualisiertField;

        private string vorgang_abgeschlossenField;

        public RuecklaeuferschnittstelleProzessende()
        {
            this.vorgang_abgeschlossenField = "0";
        }

        /// <remarks/>
        public System.DateTime zuletzt_aktualisiert
        {
            get
            {
                return this.zuletzt_aktualisiertField;
            }
            set
            {
                this.zuletzt_aktualisiertField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string vorgang_abgeschlossen
        {
            get
            {
                return this.vorgang_abgeschlossenField;
            }
            set
            {
                this.vorgang_abgeschlossenField = value;
            }
        }
    }
}