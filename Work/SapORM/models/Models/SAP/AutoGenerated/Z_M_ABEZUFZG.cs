using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using GeneralTools.Models;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_M_ABEZUFZG
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_ABEZUFZG).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_ABEZUFZG).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_ZZEQUNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("ZZEQUNR", value);
		}

		public void SetImportParameter_ZZKUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("ZZKUNNR", value);
		}

		public string GetExportParameter_ZFARBE_KLAR(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZFARBE_KLAR");
		}

		public string GetExportParameter_ZZABEKZ(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZABEKZ");
		}

		public string GetExportParameter_ZZABGASRICHTL_TG(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZABGASRICHTL_TG");
		}

		public string GetExportParameter_ZZACHSLST_ACHSE1(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZACHSLST_ACHSE1");
		}

		public string GetExportParameter_ZZACHSLST_ACHSE2(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZACHSLST_ACHSE2");
		}

		public string GetExportParameter_ZZACHSLST_ACHSE3(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZACHSLST_ACHSE3");
		}

		public string GetExportParameter_ZZACHSL_A1_STA(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZACHSL_A1_STA");
		}

		public string GetExportParameter_ZZACHSL_A2_STA(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZACHSL_A2_STA");
		}

		public string GetExportParameter_ZZACHSL_A3_STA(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZACHSL_A3_STA");
		}

		public string GetExportParameter_ZZAKPZ(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZAKPZ");
		}

		public string GetExportParameter_ZZAKUP(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZAKUP");
		}

		public string GetExportParameter_ZZALBR(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZALBR");
		}

		public string GetExportParameter_ZZALHI(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZALHI");
		}

		public string GetExportParameter_ZZALMI(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZALMI");
		}

		public string GetExportParameter_ZZALUB(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZALUB");
		}

		public string GetExportParameter_ZZALVO(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZALVO");
		}

		public string GetExportParameter_ZZANGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZANGE");
		}

		public string GetExportParameter_ZZANHLAST_GEBR(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZANHLAST_GEBR");
		}

		public string GetExportParameter_ZZANHLAST_UNGEBR(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZANHLAST_UNGEBR");
		}

		public string GetExportParameter_ZZANTRIEBSACHS(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZANTRIEBSACHS");
		}

		public string GetExportParameter_ZZANZACHS(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZANZACHS");
		}

		public string GetExportParameter_ZZANZSITZE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZANZSITZE");
		}

		public string GetExportParameter_ZZANZSTEHPLAETZE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZANZSTEHPLAETZE");
		}

		public string GetExportParameter_ZZAUFB(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZAUFB");
		}

		public string GetExportParameter_ZZAUFTXT(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZAUFTXT");
		}

		public string GetExportParameter_ZZAUSF(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZAUSF");
		}

		public string GetExportParameter_ZZBDR1(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBDR1");
		}

		public string GetExportParameter_ZZBDR2(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBDR2");
		}

		public string GetExportParameter_ZZBE01(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE01");
		}

		public string GetExportParameter_ZZBE02(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE02");
		}

		public string GetExportParameter_ZZBE03(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE03");
		}

		public string GetExportParameter_ZZBE04(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE04");
		}

		public string GetExportParameter_ZZBE05(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE05");
		}

		public string GetExportParameter_ZZBE06(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE06");
		}

		public string GetExportParameter_ZZBE07(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE07");
		}

		public string GetExportParameter_ZZBE08(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE08");
		}

		public string GetExportParameter_ZZBE09(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE09");
		}

		public string GetExportParameter_ZZBE10(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE10");
		}

		public string GetExportParameter_ZZBE11(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE11");
		}

		public string GetExportParameter_ZZBE12(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE12");
		}

		public string GetExportParameter_ZZBE13(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE13");
		}

		public string GetExportParameter_ZZBE14(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE14");
		}

		public string GetExportParameter_ZZBE15(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE15");
		}

		public string GetExportParameter_ZZBE16(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE16");
		}

		public string GetExportParameter_ZZBE17(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE17");
		}

		public string GetExportParameter_ZZBE18(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE18");
		}

		public string GetExportParameter_ZZBE19(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE19");
		}

		public string GetExportParameter_ZZBE20(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE20");
		}

		public string GetExportParameter_ZZBE21(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE21");
		}

		public string GetExportParameter_ZZBE22(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE22");
		}

		public string GetExportParameter_ZZBEIUMDREH(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEIUMDREH");
		}

		public string GetExportParameter_ZZBEMER1(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER1");
		}

		public string GetExportParameter_ZZBEMER10(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER10");
		}

		public string GetExportParameter_ZZBEMER11(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER11");
		}

		public string GetExportParameter_ZZBEMER12(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER12");
		}

		public string GetExportParameter_ZZBEMER13(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER13");
		}

		public string GetExportParameter_ZZBEMER14(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER14");
		}

		public string GetExportParameter_ZZBEMER2(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER2");
		}

		public string GetExportParameter_ZZBEMER3(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER3");
		}

		public string GetExportParameter_ZZBEMER4(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER4");
		}

		public string GetExportParameter_ZZBEMER5(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER5");
		}

		public string GetExportParameter_ZZBEMER6(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER6");
		}

		public string GetExportParameter_ZZBEMER7(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER7");
		}

		public string GetExportParameter_ZZBEMER8(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER8");
		}

		public string GetExportParameter_ZZBEMER9(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER9");
		}

		public string GetExportParameter_ZZBEREIFACHSE1(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEREIFACHSE1");
		}

		public string GetExportParameter_ZZBEREIFACHSE2(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEREIFACHSE2");
		}

		public string GetExportParameter_ZZBEREIFACHSE3(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEREIFACHSE3");
		}

		public string GetExportParameter_ZZBREITEMAX(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBREITEMAX");
		}

		public string GetExportParameter_ZZBREITEMIN(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBREITEMIN");
		}

		public string GetExportParameter_ZZCHASSIS_NUM(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZCHASSIS_NUM");
		}

		public string GetExportParameter_ZZCO2KOMBI(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZCO2KOMBI");
		}

		public string GetExportParameter_ZZCODE_AUFBAU(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZCODE_AUFBAU");
		}

		public string GetExportParameter_ZZCODE_KRAFTSTOF(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZCODE_KRAFTSTOF");
		}

		public string GetExportParameter_ZZDREHZSTANDGER(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZDREHZSTANDGER");
		}

		public string GetExportParameter_ZZEARTX(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZEARTX");
		}

		public decimal? GetExportParameter_ZZENGINE_CAP(ISapDataService sap)
		{
			return sap.GetExportParameter<decimal?>("ZZENGINE_CAP");
		}

		public decimal? GetExportParameter_ZZENGINE_POWER(ISapDataService sap)
		{
			return sap.GetExportParameter<decimal?>("ZZENGINE_POWER");
		}

		public string GetExportParameter_ZZENGINE_TYPE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZENGINE_TYPE");
		}

		public string GetExportParameter_ZZFABRIKNAME(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZFABRIKNAME");
		}

		public string GetExportParameter_ZZFAHRGERAEUSCH(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZFAHRGERAEUSCH");
		}

		public string GetExportParameter_ZZFAHRZEUGKLASSE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZFAHRZEUGKLASSE");
		}

		public string GetExportParameter_ZZFARBE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZFARBE");
		}

		public string GetExportParameter_ZZFASSVERMOEGEN(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZFASSVERMOEGEN");
		}

		public string GetExportParameter_ZZFDB9(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZFDB9");
		}

		public string GetExportParameter_ZZFHRZKLASSE_TXT(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZFHRZKLASSE_TXT");
		}

		public string GetExportParameter_ZZFLEET_CAT(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZFLEET_CAT");
		}

		public decimal? GetExportParameter_ZZFLEET_HGT(ISapDataService sap)
		{
			return sap.GetExportParameter<decimal?>("ZZFLEET_HGT");
		}

		public decimal? GetExportParameter_ZZFLEET_LEN(ISapDataService sap)
		{
			return sap.GetExportParameter<decimal?>("ZZFLEET_LEN");
		}

		public string GetExportParameter_ZZFLEET_VIN(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZFLEET_VIN");
		}

		public decimal? GetExportParameter_ZZFLEET_WID(ISapDataService sap)
		{
			return sap.GetExportParameter<decimal?>("ZZFLEET_WID");
		}

		public string GetExportParameter_ZZGENEHMIGDAT(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZGENEHMIGDAT");
		}

		public string GetExportParameter_ZZGENEHMIGNR(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZGENEHMIGNR");
		}

		public decimal? GetExportParameter_ZZGROSS_WGT(ISapDataService sap)
		{
			return sap.GetExportParameter<decimal?>("ZZGROSS_WGT");
		}

		public string GetExportParameter_ZZHANDELSNAME(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZHANDELSNAME");
		}

		public string GetExportParameter_ZZHERSTELLER_SCH(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZHERSTELLER_SCH");
		}

		public string GetExportParameter_ZZHERST_TEXT(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZHERST_TEXT");
		}

		public string GetExportParameter_ZZHOECHSTGESCHW(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZHOECHSTGESCHW");
		}

		public string GetExportParameter_ZZHOEHEMAX(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZHOEHEMAX");
		}

		public string GetExportParameter_ZZHOEHEMIN(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZHOEHEMIN");
		}

		public string GetExportParameter_ZZHUBRAUM(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZHUBRAUM");
		}

		public string GetExportParameter_ZZKLARTEXT_TYP(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZKLARTEXT_TYP");
		}

		public string GetExportParameter_ZZKLTXT(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZKLTXT");
		}

		public string GetExportParameter_ZZKRAFTSTOFF_TXT(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZKRAFTSTOFF_TXT");
		}

		public string GetExportParameter_ZZLAENGEMAX(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZLAENGEMAX");
		}

		public string GetExportParameter_ZZLAENGEMIN(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZLAENGEMIN");
		}

		public string GetExportParameter_ZZLEISTUNGSGEW(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZLEISTUNGSGEW");
		}

		public string GetExportParameter_ZZLGEW(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZLGEW");
		}

		public string GetExportParameter_ZZMASSEFAHRBMAX(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZMASSEFAHRBMAX");
		}

		public string GetExportParameter_ZZMASSEFAHRBMIN(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZMASSEFAHRBMIN");
		}

		public string GetExportParameter_ZZMAX_OCCUPANTS(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZMAX_OCCUPANTS");
		}

		public string GetExportParameter_ZZNATIONALE_EMIK(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZNATIONALE_EMIK");
		}

		public string GetExportParameter_ZZNENNLEISTUNG(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZNENNLEISTUNG");
		}

		public string GetExportParameter_ZZNULA(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZNULA");
		}

		public string GetExportParameter_ZZNUM_AXLE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZNUM_AXLE");
		}

		public string GetExportParameter_ZZORHI(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZORHI");
		}

		public string GetExportParameter_ZZORVO(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZORVO");
		}

		public string GetExportParameter_ZZPRFZ(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZPRFZ");
		}

		public string GetExportParameter_ZZREHI(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZREHI");
		}

		public DateTime? GetExportParameter_ZZREPLA_DATE(ISapDataService sap)
		{
			return sap.GetExportParameter<DateTime?>("ZZREPLA_DATE");
		}

		public string GetExportParameter_ZZREVO(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZREVO");
		}

		public string GetExportParameter_ZZREVOLUTIONS(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZREVOLUTIONS");
		}

		public string GetExportParameter_ZZSDB9(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZSDB9");
		}

		public string GetExportParameter_ZZSLD(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZSLD");
		}

		public decimal? GetExportParameter_ZZSPEED_MAX(ISapDataService sap)
		{
			return sap.GetExportParameter<decimal?>("ZZSPEED_MAX");
		}

		public string GetExportParameter_ZZSTANDGERAEUSCH(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZSTANDGERAEUSCH");
		}

		public string GetExportParameter_ZZSTPL(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZSTPL");
		}

		public string GetExportParameter_ZZSTUETZLAST(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZSTUETZLAST");
		}

		public string GetExportParameter_ZZTANK(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZTANK");
		}

		public string GetExportParameter_ZZTEXT_AUFBAU(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZTEXT_AUFBAU");
		}

		public string GetExportParameter_ZZTYP(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZTYP");
		}

		public string GetExportParameter_ZZTYPA(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZTYPA");
		}

		public string GetExportParameter_ZZTYPE_TEXT(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZTYPE_TEXT");
		}

		public string GetExportParameter_ZZTYP_SCHL(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZTYP_SCHL");
		}

		public string GetExportParameter_ZZTYP_VVS_PRUEF(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZTYP_VVS_PRUEF");
		}

		public string GetExportParameter_ZZUNIT_POWER(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZUNIT_POWER");
		}

		public string GetExportParameter_ZZVARIANTE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZVARIANTE");
		}

		public string GetExportParameter_ZZVERSION(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZVERSION");
		}

		public string GetExportParameter_ZZVVS_SCHLUESSEL(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZVVS_SCHLUESSEL");
		}

		public string GetExportParameter_ZZZULGESGEW(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZZULGESGEW");
		}

		public string GetExportParameter_ZZZULGESGEWSTAAT(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZZULGESGEWSTAAT");
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
