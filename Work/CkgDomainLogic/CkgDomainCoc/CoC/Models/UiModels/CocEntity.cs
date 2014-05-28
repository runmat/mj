using System;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.CoC.Models
{
    public class CocEntity 
    {
        private string _coc52Text;

        // ReSharper disable InconsistentNaming
        // ReSharper disable LocalizableElement

        [ModelMappingCompareIgnore]
        [GridHidden]
        public string PrimaryKeyCocTyp { get { return string.Format("{0}_{1}_{2}", COC_0_2_TYP.NotNullOrEmpty().ToUpper(), COC_0_2_VAR.NotNullOrEmpty().ToUpper(), COC_0_2_VERS.NotNullOrEmpty().ToUpper()); } }

        [GridHidden]
        public int ID { get; set; }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get; set; }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool InsertModeTmp { get; set; }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool NoSaveButUiRefreshOnly { get; set; }

        /// <summary>
        /// Usability enhancement: Optionally hide default "COC TYPEN" properties so the user will only be involved with COC ORDER relevant properties
        /// </summary>
        [ModelMappingCompareIgnore]
        [GridHidden]
        [LocalizedDisplay(LocalizeConstants._NurAenderbareFelderAnzeigen)]
        public bool CocOrderHideCocTypenProperties { get; set; }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsCocOrder { get; set; }

        [ModelMappingCompareIgnore]
        public string VORG_NR { get; set; }

        [LocalizedDisplay(LocalizeConstants._Auslieferung)]
        public DateTime? AUSLIEFER_DATUM { get; set; }

        [LocalizedDisplay(LocalizeConstants._Auftrag)]
        public DateTime? AUFTRAG_DAT { get; set; }
        
        public string KUNNR { get; set; }

        public string VORLAGE { get; set; }

        [LocalizedDisplay(LocalizeConstants.RecordingDate)]
        [ModelMappingCompareIgnore]
        public DateTime? ERDAT { get; set; }


        [CocLayout(Group = "0.1", GroupLabel = "Fabrikmarke", Measure = "", Label = "", MaxLen = 25)]
        public string COC_0_1 { get; set; }


        [CocLayout(Group = "0.2", GroupLabel = "Typ / Variante / Version", Measure = "Typ", Label = "", MaxLen = 25)]
        [Required]
        [LocalizedDisplay(LocalizeConstants._Typ)]
        public string COC_0_2_TYP { get; set; }

        [CocLayout(Group = "0.2", GroupLabel = "", Measure = "Variante", Label = "", MaxLen = 25)]
        [Required]
        [LocalizedDisplay(LocalizeConstants._Variante)]
        public string COC_0_2_VAR { get; set; }

        [CocLayout(Group = "0.2", GroupLabel = "", Measure = "Version", Label = "", MaxLen = 25)]
        [Required]
        [LocalizedDisplay(LocalizeConstants._Version)]
        public string COC_0_2_VERS { get; set; }


        [CocLayout(Group = "0.4", GroupLabel = "Fahrzeugklasse", Measure = "", Label = "", MaxLen = 4)]
        public string COC_0_4 { get; set; }

        [CocLayout(Group = "0.5", GroupLabel = "Name + Anschr. Hersteller", Measure = "", Label = "", MaxLen = 150, MultiLine = true)]
        public string COC_0_5 { get; set; }

        [CocLayout(Group = "0.6", GroupLabel = "Anbringung VIN", Measure = "", Label = "", MaxLen = 100, MultiLine = true)]
        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string COC_0_6_VIN { get; set; }

        [CocLayout(Group = "0.6 ", GroupLabel = "Anbringung Schilder", Measure = "", Label = "", MaxLen = 100, MultiLine = true)]
        public string COC_0_6_SCHILD { get; set; }

        [CocLayout(Group = "0.9", GroupLabel = "Name bevollm. Hersteller", Measure = "", Label = "", MaxLen = 150)]
        public string COC_0_9 { get; set; }

        string _vin;
        [CocLayout(Group = "~VIN", GroupLabel = "VIN", Measure = "", Label = "", MaxLen = 17, IsCocOrderEditable = true)] 
        [LocalizedDisplay(LocalizeConstants.VIN)] 
        [VIN]
        public string VIN { get { return _vin; } set { _vin = value.NotNullOrEmpty().ToUpper(); } }

        [CocLayout(Group = "~AUFTR_KD", GroupLabel = "Auftrag-Nr. Kunde", Measure = "", Label = "", MaxLen = 25, IsCocOrderEditable = true)]
        [LocalizedDisplay(LocalizeConstants.OrderID)]
        public string AUFTR_NR_KD { get; set; }

        [CocLayout(Group = "~LAND", GroupLabel = "Land", Measure = "", Label = "", MaxLen = 2, IsCocOrderEditable = true)]
        [LocalizedDisplay(LocalizeConstants.Country)]
        [LowerCase]
        public string LAND { get; set; }

        [CocLayout(Group = "1", GroupLabel = "Anzahl Achsen", Measure = "", Label = "", MaxLen = 2)]
        public int? COC_1_ANZ_ACHS { get; set; }

        [CocLayout(Group = "1 ", GroupLabel = "Anzahl Räder", Measure = "", Label = "", MaxLen = 2)]
        public int? COC_1_ANZ_RAED { get; set; }

        [CocLayout(Group = "1.1", GroupLabel = "Anzahl / Lage Achsen", Measure = "(Anzahl)", Label = "", MaxLen = 2)]
        public int? COC_1_1 { get; set; }

        [CocLayout(Group = "2", GroupLabel = "Gelenkte Achsen", Measure = "(Anzahl)", Label = "", MaxLen = 2)]
        public int? COC_2_ANZ_GEL_ACHS { get; set; }

        [CocLayout(Group = "2", GroupLabel = "", Measure = "(Lage)", Label = "", MaxLen = 10)]
        public string COC_2_LAG_GEL_ACHS { get; set; }

        [CocLayout(Group = "4", GroupLabel = "Radstand min/max", Measure = "mm", Label = "Min")]
        public int? COC_4_MIN { get; set; }

        [CocLayout(Group = "4", GroupLabel = "", Measure = "mm", Label = "Max")]
        public int? COC_4_MAX { get; set; }

        [CocLayout(Group = "4.1", GroupLabel = "Achsabstände", Measure = "mm", Label = "1.")]
        public int? COC_4_1_1 { get; set; }

        [CocLayout(Group = "4.1", GroupLabel = "", Measure = "mm", Label = "2.")]
        public int? COC_4_1_2 { get; set; }

        [CocLayout(Group = "4.1", GroupLabel = "", Measure = "mm", Label = "3.")]
        public int? COC_4_1_3 { get; set; }

        [CocLayout(Group = "5", GroupLabel = "Länge min/max", Measure = "mm", Label = "Min")]
        public int? COC_5_MIN { get; set; }

        [CocLayout(Group = "5", GroupLabel = "", Measure = "mm", Label = "Max")]
        public int? COC_5_MAX { get; set; }

        [CocLayout(Group = "6", GroupLabel = "Breite", Measure = "mm", Label = "")]
        public int? COC_6 { get; set; }

        [CocLayout(Group = "7", GroupLabel = "Höhe", Measure = "mm", Label = "")]
        public int? COC_7 { get; set; }

        [CocLayout(Group = "10", GroupLabel = "Abstand Mittelp. Anh. / Heck", Measure = "mm", Label = "Min")]
        public int? COC_10_MIN { get; set; }

        [CocLayout(Group = "10", GroupLabel = "", Measure = "mm", Label = "Max")]
        public int? COC_10_MAX { get; set; }

        [CocLayout(Group = "11", GroupLabel = "Länge Ladefläche", Measure = "mm", Label = "Min")]
        public int? COC_11_MIN { get; set; }

        [CocLayout(Group = "11", GroupLabel = "", Measure = "mm", Label = "Max")]
        public int? COC_11_MAX { get; set; }

        [CocLayout(Group = "12", GroupLabel = "Hinterer Überhang", Measure = "mm", Label = "Min")]
        public int? COC_12_MIN { get; set; }

        [CocLayout(Group = "12", GroupLabel = "", Measure = "mm", Label = "Max")]
        public int? COC_12_MAX { get; set; }

        [CocLayout(Group = "13", GroupLabel = "Masse fahrber. Fzg.", Measure = "kg", Label = "", MaxLen = 4)]
        public int? COC_13 { get; set; }

        [CocLayout(Group = "13.1", GroupLabel = "Verteil. Masse / Achsen", Measure = "kg", Label = "1.", MaxLen = 4)]
        public int? COC_13_1_1 { get; set; }

        [CocLayout(Group = "13.1", GroupLabel = "", Measure = "kg", Label = "2.", MaxLen = 4)]
        public int? COC_13_1_2 { get; set; }

        [CocLayout(Group = "13.1", GroupLabel = "", Measure = "kg", Label = "3.", MaxLen = 4)]
        public int? COC_13_1_3 { get; set; }

        [CocLayout(Group = "13.1", GroupLabel = "", Measure = "kg", Label = "4.", MaxLen = 4)]
        public int? COC_13_1_4 { get; set; }

        [CocLayout(Group = "16.1", GroupLabel = "Techn. zul. Gesamtmasse", Measure = "kg", Label = "", MaxLen = 4)]
        public int? COC_16_1 { get; set; }

        [CocLayout(Group = "16.2", GroupLabel = "Gesamtmasse / Achse", Measure = "kg", Label = "1.", MaxLen = 4)]
        public int? COC_16_2_1 { get; set; }

        [CocLayout(Group = "16.2", GroupLabel = "", Measure = "kg", Label = "2.", MaxLen = 4)]
        public int? COC_16_2_2 { get; set; }

        [CocLayout(Group = "16.2", GroupLabel = "", Measure = "kg", Label = "3.", MaxLen = 4)]
        public int? COC_16_2_3 { get; set; }

        [CocLayout(Group = "16.2", GroupLabel = "", Measure = "kg", Label = "4.", MaxLen = 4)]
        public int? COC_16_2_4 { get; set; }

        [CocLayout(Group = "16.3", GroupLabel = "Techn. zul. Masse / Achsgruppe", Measure = "kg", Label = "1.", MaxLen = 4)]
        public int? COC_16_3_1 { get; set; }

        [CocLayout(Group = "16.3", GroupLabel = "", Measure = "kg", Label = "2.", MaxLen = 4)]
        public int? COC_16_3_2 { get; set; }

        [CocLayout(Group = "17.1", GroupLabel = "Höchstzul. Gesamtmasse", Measure = "kg", Label = "", MaxLen = 4)]
        public int? COC_17_1 { get; set; }

        [CocLayout(Group = "17.2", GroupLabel = "Gesamtmasse / Achse", Measure = "kg", Label = "1.", MaxLen = 4)]
        public int? COC_17_2_1 { get; set; }

        [CocLayout(Group = "17.2", GroupLabel = "", Measure = "kg", Label = "2.", MaxLen = 4)]
        public int? COC_17_2_2 { get; set; }

        [CocLayout(Group = "17.2", GroupLabel = "", Measure = "kg", Label = "3.", MaxLen = 4)]
        public int? COC_17_2_3 { get; set; }

        [CocLayout(Group = "17.2", GroupLabel = "", Measure = "kg", Label = "4.", MaxLen = 4)]
        public int? COC_17_2_4 { get; set; }

        [CocLayout(Group = "17.3", GroupLabel = "Gesamtmasse / Achsgruppe", Measure = "kg", Label = "1.", MaxLen = 4)]
        public int? COC_17_3_1 { get; set; }

        [CocLayout(Group = "17.3", GroupLabel = "", Measure = "kg", Label = "2.")]
        public int? COC_17_3_2 { get; set; }

        [CocLayout(Group = "19", GroupLabel = "Stützlast am Kuppl.punkt", Measure = "kg", Label = "", MaxLen = 4)]
        public int? COC_19 { get; set; }

        [CocLayout(Group = "29", GroupLabel = "Höchstgeschwindigk.", Measure = "km/h", Label = "")]
        public int? COC_29 { get; set; }

        [CocLayout(Group = "31", GroupLabel = "Lage anhebbare Achsen", Measure = "1.", Label = "")]
        public bool COC_31_1 { get; set; }

        [CocLayout(Group = "31", GroupLabel = "", Measure = "2.", Label = "")]
        public bool COC_31_2 { get; set; }

        [CocLayout(Group = "31", GroupLabel = "", Measure = "3.", Label = "")]
        public bool COC_31_3 { get; set; }

        [CocLayout(Group = "31", GroupLabel = "", Measure = "4.", Label = "")]
        public bool COC_31_4 { get; set; }

        [CocLayout(Group = "32", GroupLabel = "Lage belastbare Achsen", Measure = "1.", Label = "")]
        public bool COC_32_1 { get; set; }

        [CocLayout(Group = "32", GroupLabel = "", Measure = "2.", Label = "")]
        public bool COC_32_2 { get; set; }

        [CocLayout(Group = "32", GroupLabel = "", Measure = "3.", Label = "")]
        public bool COC_32_3 { get; set; }

        [CocLayout(Group = "32", GroupLabel = "", Measure = "4.", Label = "")]
        public bool COC_32_4 { get; set; }

        [CocLayout(Group = "34", GroupLabel = "Achsen mit Luftfederung", Label = "", SelectOptions = "X,Ja; ,Nein")]
        public string COC_34_JA { get; set; }

        [CocLayout(Group = "35", GroupLabel = "Reifen- / Radkombination", Measure = "", Label = "A1,R", MaxLen = 25)]
        public string COC_35_ACHSE1_R { get; set; }

        [CocLayout(Group = "35", GroupLabel = "", Measure = "", Label = "A1,F", MaxLen = 25)]
        public string COC_35_ACHSE1_F { get; set; }

        [CocLayout(Group = "35", GroupLabel = "", Measure = "", Label = "A2,R", MaxLen = 25)]
        public string COC_35_ACHSE2_R { get; set; }

        [CocLayout(Group = "35", GroupLabel = "", Measure = "", Label = "A2,F", MaxLen = 25)]
        public string COC_35_ACHSE2_F { get; set; }

        [CocLayout(Group = "35", GroupLabel = "", Measure = "", Label = "A3,R", MaxLen = 25)]
        public string COC_35_ACHSE3_R { get; set; }

        [CocLayout(Group = "35", GroupLabel = "", Measure = "", Label = "A3,F", MaxLen = 25)]
        public string COC_35_ACHSE3_F { get; set; }

        [CocLayout(Group = "36", GroupLabel = "Anhänger-Bremsanschlüsse", Measure = "mechanisch", Label = "")]
        public bool COC_36_MECHANISCH { get; set; }

        [CocLayout(Group = "36", GroupLabel = "", Measure = "elektrisch", Label = "")]
        public bool COC_36_ELEKTRISCH { get; set; }

        [CocLayout(Group = "36", GroupLabel = "", Measure = "pneumatisch", Label = "")]
        public bool COC_36_PNEUMATISCH { get; set; }

        [CocLayout(Group = "36", GroupLabel = "", Measure = "hydraulisch", Label = "")]
        public bool COC_36_HYDRAULISCH { get; set; }

        [CocLayout(Group = "38", GroupLabel = "Code des Aufbaus", Measure = "", Label = "", MaxLen = 4)]
        public string COC_38 { get; set; }

        [CocLayout(Group = "44", GroupLabel = "Genehmigungsnummer", Measure = "", Label = "", MaxLen = 25)]
        public string COC_44 { get; set; }

        [CocLayout(Group = "45.1", GroupLabel = "Kennwerte", Measure = "", Label = "D", MaxLen = 4)]
        public string COC_45_1_D { get; set; }

        [CocLayout(Group = "45.1", GroupLabel = "", Measure = "", Label = "V", MaxLen = 4)]
        public string COC_45_1_V { get; set; }

        [CocLayout(Group = "45.1", GroupLabel = "", Measure = "", Label = "S", MaxLen = 4)]
        public string COC_45_1_S { get; set; }

        [CocLayout(Group = "45.1", GroupLabel = "", Measure = "", Label = "U", MaxLen = 4)]
        public string COC_45_1_U { get; set; }

        [CocLayout(Group = "50", GroupLabel = "Typ genehmigt", Label = "", SelectOptions = "X,Ja; ,Nein")]
        public string COC_50_JA { get; set; }

        [CocLayout(Group = "50", GroupLabel = "", Measure = "EXII", Label = "")]
        public bool COC_50_EXII { get; set; }

        [CocLayout(Group = "50", GroupLabel = "", Measure = "EXIII", Label = "")]
        public bool COC_50_EXIII { get; set; }

        [CocLayout(Group = "50", GroupLabel = "", Measure = "FL", Label = "")]
        public bool COC_50_FL { get; set; }

        [CocLayout(Group = "50", GroupLabel = "", Measure = "OX", Label = "")]
        public bool COC_50_OX { get; set; }

        [CocLayout(Group = "50", GroupLabel = "", Measure = "AT", Label = "")]
        public bool COC_50_AT { get; set; }

        [CocLayout(Group = "51", GroupLabel = "Bez. gem. Anhang II", Measure = "", Label = "", MaxLen = 50)]
        public string COC_51 { get; set; }

        [CocLayout(Group = "~REIFEN", GroupLabel = "Reifen", Measure = "", Label = "", MaxLen = 60)]
        public string COC_52_REIFEN_ALT { get; set; }

        [CocLayout(Group = "~REG_ITALIEN", GroupLabel = "Reg-Nr. (Italien)", Measure = "", Label = "", MaxLen = 8)]
        public string COC_52_REG_ITALIEN { get; set; }

        [CocLayout(Group = "~REG_TEXT", GroupLabel = "Text", Measure = "", Label = "", MaxLen = 1000, MultiLine = true, MultiLineRows = 10)]
        public string COC_52_TEXT
        {
            get { return _coc52Text; }
            set { _coc52Text = value.NotNullOrEmpty().Trim(); }
        }


        [CocLayout(Group = "~COC_EG", GroupLabel = "EG-Typengenehmigung", Measure = "", Label = "", MaxLen = 25)]
        public string COC_EG_TYP_GEN { get; set; }

        [CocLayout(Group = "~COC_EG_DAT", GroupLabel = "Datum (EG-Typengenehm.)", Measure = "", Label = "", MaxLen = 10)]
        [DisplayFormatDate]
        public DateTime? COC_EG_TYP_GEN_DAT { get; set; }

        [CocLayout(Group = "~ZBII", GroupLabel = "ZBII", Measure = "", Label = "2", MaxLen = 25)]
        public string ZBII_2 { get; set; }

        [CocLayout(Group = "~ZBII", GroupLabel = "", Measure = "", Label = "2.1", MaxLen = 4)]
        public string ZBII_2_1 { get; set; }

        [CocLayout(Group = "~ZBII", GroupLabel = "", Measure = "", Label = "2.2 Typ", MaxLen = 3)]
        [LocalizedDisplay(LocalizeConstants.Code)]
        public string ZBII_2_2_TYP { get; set; }

        [CocLayout(Group = "~ZBII", GroupLabel = "", Measure = "", Label = "2.2 VVS", MaxLen = 5)]
        [LocalizedDisplay(LocalizeConstants._Typ)]
        public string ZBII_2_2_VVS { get; set; }

        [CocLayout(Group = "~ZBII", GroupLabel = "", Measure = "", Label = "2.2 PZ", MaxLen = 1)]
        [LocalizedDisplay(LocalizeConstants._Version)]
        public string ZBII_2_2_PZ { get; set; }

        [CocLayout(Group = "~ZBII", GroupLabel = "", Measure = "", Label = "D 3", MaxLen = 25)]
        public string ZBII_D_3 { get; set; }

        [CocLayout(Group = "~ZBII", GroupLabel = "", Measure = "", Label = "5 Klasse", MaxLen = 25)]
        public string ZBII_5_KLASSE { get; set; }

        [CocLayout(Group = "~ZBII", GroupLabel = "", Measure = "", Label = "5 Aufbau", MaxLen = 25)]
        public string ZBII_5_AUFBAU { get; set; }


        public string ZBII_D_1 { get; set; }

        public string ZBII_D_2_TYP { get; set; }

        public string ZBII_D_2_VARIANTE { get; set; }

        public string ZBII_D_2_VERSION { get; set; }

        public string ZBII_J { get; set; }

        public string ZBII_4 { get; set; }

        public string ZBII_K { get; set; }

        public string ZBII_6 { get; set; }


        [ModelMappingCompareIgnore]
        public string ZBII_R { get; set; }

        [LocalizedDisplay(LocalizeConstants.Color)]
        [ModelMappingCompareIgnore]
        public string Color { get { return ZBII_R; } }

        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        [ModelMappingCompareIgnore]
        public string Hersteller { get { return ZBII_2_1; } }

        [ModelMappingCompareIgnore]
        public bool IsSelected { get; set; }

        [ModelMappingCompareIgnore]
        [LocalizedDisplay(LocalizeConstants.CodeTypeVersion)]
        public string CodeTypeVersion { get { return string.Format("{0}{1}{2}", ZBII_2_2_TYP, ZBII_2_2_VVS, ZBII_2_2_PZ); } }


        [ModelMappingCompareIgnore]
        public bool DuplicateVinOccured { get; set; }
        [ModelMappingCompareIgnore]
        public bool DuplicateVinIgnoreOnSaving { get; set; }


        [ModelMappingCompareIgnore]
        public string ZBII_DRUCK { get; set; }

        [ModelMappingCompareIgnore]
        public DateTime? ZBII_DRUCK_DATUM { get; set; }

        [ModelMappingCompareIgnore]
        public string ZBII_DRUCK_ZEIT { get; set; }

        [ModelMappingCompareIgnore]
        public string ZBII_KBA_MELD { get; set; }
        
        [ModelMappingCompareIgnore]
        public string VERSAND { get; set; }
        
        [ModelMappingCompareIgnore]
        public string ZUL_DEZ { get; set; }
        
        [ModelMappingCompareIgnore]
        public string ZUL_AUSLAND { get; set; }

        [ModelMappingCompareIgnore]
        public string ZUL_EXPORT { get; set; }

        [ModelMappingCompareIgnore]
        public string COC_KD_ORIG { get; set; }

        [ModelMappingCompareIgnore]
        public string COC_KD_KOPIE { get; set; }

        [ModelMappingCompareIgnore]
        public string COC_DRUCK_ORIG { get; set; }

        [ModelMappingCompareIgnore]
        public string COC_DRUCK_KOPIE { get; set; }

        [ModelMappingCompareIgnore]
        public DateTime? COC_DRUCK_DATUM { get; set; }

        [ModelMappingCompareIgnore]
        public string COC_DRUCK_ZEIT { get; set; }

        // ReSharper restore LocalizableElement
        // ReSharper restore InconsistentNaming


        public bool IsReadonly(CocMetaProperty metaProperty)
        {
            return !metaProperty.IsCocOrderEditable && IsCocOrder;
        }

        public string GetControlGroupCssClass(CocMetaProperty metaProperty)
        {
            if (IsCocOrder)
                return metaProperty.IsCocOrderEditable ? "cocOrderHighlighted" : CocOrderHideCocTypenProperties ? "hide" : "";

            return metaProperty.IsCocOrderEditable ? "hide" : "";
        } 

        public CocEntity SetInsertMode(bool insertMode)
        {
            InsertModeTmp = insertMode;
            return this;
        }

        public CocEntity SetOrderStatus(bool isOrder)
        {
            IsCocOrder = isOrder;
            return this;
        }
    }
}
