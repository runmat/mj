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


		public static void SetImportParameter_ZZEQUNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("ZZEQUNR", value);
		}

		public static void SetImportParameter_ZZKUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("ZZKUNNR", value);
		}

		public static string GetExportParameter_ZFARBE_KLAR(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZFARBE_KLAR").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZABEKZ(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZABEKZ").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZABGASRICHTL_TG(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZABGASRICHTL_TG").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZACHSLST_ACHSE1(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZACHSLST_ACHSE1").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZACHSLST_ACHSE2(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZACHSLST_ACHSE2").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZACHSLST_ACHSE3(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZACHSLST_ACHSE3").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZACHSL_A1_STA(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZACHSL_A1_STA").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZACHSL_A2_STA(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZACHSL_A2_STA").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZACHSL_A3_STA(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZACHSL_A3_STA").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZAKPZ(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZAKPZ").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZAKUP(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZAKUP").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZALBR(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZALBR").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZALHI(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZALHI").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZALMI(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZALMI").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZALUB(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZALUB").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZALVO(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZALVO").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZANGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZANGE").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZANHLAST_GEBR(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZANHLAST_GEBR").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZANHLAST_UNGEBR(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZANHLAST_UNGEBR").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZANTRIEBSACHS(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZANTRIEBSACHS").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZANZACHS(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZANZACHS").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZANZSITZE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZANZSITZE").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZANZSTEHPLAETZE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZANZSTEHPLAETZE").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZAUFB(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZAUFB").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZAUFTXT(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZAUFTXT").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZAUSF(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZAUSF").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBDR1(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBDR1").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBDR2(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBDR2").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBE01(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE01").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBE02(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE02").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBE03(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE03").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBE04(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE04").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBE05(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE05").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBE06(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE06").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBE07(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE07").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBE08(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE08").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBE09(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE09").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBE10(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE10").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBE11(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE11").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBE12(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE12").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBE13(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE13").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBE14(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE14").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBE15(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE15").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBE16(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE16").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBE17(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE17").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBE18(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE18").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBE19(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE19").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBE20(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE20").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBE21(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE21").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBE22(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBE22").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBEIUMDREH(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEIUMDREH").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBEMER1(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER1").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBEMER10(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER10").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBEMER11(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER11").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBEMER12(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER12").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBEMER13(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER13").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBEMER14(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER14").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBEMER2(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER2").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBEMER3(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER3").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBEMER4(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER4").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBEMER5(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER5").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBEMER6(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER6").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBEMER7(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER7").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBEMER8(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER8").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBEMER9(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEMER9").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBEREIFACHSE1(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEREIFACHSE1").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBEREIFACHSE2(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEREIFACHSE2").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBEREIFACHSE3(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBEREIFACHSE3").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBREITEMAX(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBREITEMAX").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZBREITEMIN(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZBREITEMIN").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZCHASSIS_NUM(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZCHASSIS_NUM").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZCO2KOMBI(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZCO2KOMBI").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZCODE_AUFBAU(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZCODE_AUFBAU").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZCODE_KRAFTSTOF(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZCODE_KRAFTSTOF").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZDREHZSTANDGER(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZDREHZSTANDGER").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZEARTX(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZEARTX").NotNullOrEmpty().Trim();
		}

		public static decimal? GetExportParameter_ZZENGINE_CAP(ISapDataService sap)
		{
			return sap.GetExportParameter<decimal?>("ZZENGINE_CAP");
		}

		public static decimal? GetExportParameter_ZZENGINE_POWER(ISapDataService sap)
		{
			return sap.GetExportParameter<decimal?>("ZZENGINE_POWER");
		}

		public static string GetExportParameter_ZZENGINE_TYPE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZENGINE_TYPE").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZFABRIKNAME(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZFABRIKNAME").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZFAHRGERAEUSCH(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZFAHRGERAEUSCH").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZFAHRZEUGKLASSE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZFAHRZEUGKLASSE").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZFARBE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZFARBE").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZFASSVERMOEGEN(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZFASSVERMOEGEN").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZFDB9(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZFDB9").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZFHRZKLASSE_TXT(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZFHRZKLASSE_TXT").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZFLEET_CAT(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZFLEET_CAT").NotNullOrEmpty().Trim();
		}

		public static decimal? GetExportParameter_ZZFLEET_HGT(ISapDataService sap)
		{
			return sap.GetExportParameter<decimal?>("ZZFLEET_HGT");
		}

		public static decimal? GetExportParameter_ZZFLEET_LEN(ISapDataService sap)
		{
			return sap.GetExportParameter<decimal?>("ZZFLEET_LEN");
		}

		public static string GetExportParameter_ZZFLEET_VIN(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZFLEET_VIN").NotNullOrEmpty().Trim();
		}

		public static decimal? GetExportParameter_ZZFLEET_WID(ISapDataService sap)
		{
			return sap.GetExportParameter<decimal?>("ZZFLEET_WID");
		}

		public static string GetExportParameter_ZZGENEHMIGDAT(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZGENEHMIGDAT").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZGENEHMIGNR(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZGENEHMIGNR").NotNullOrEmpty().Trim();
		}

		public static decimal? GetExportParameter_ZZGROSS_WGT(ISapDataService sap)
		{
			return sap.GetExportParameter<decimal?>("ZZGROSS_WGT");
		}

		public static string GetExportParameter_ZZHANDELSNAME(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZHANDELSNAME").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZHERSTELLER_SCH(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZHERSTELLER_SCH").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZHERST_TEXT(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZHERST_TEXT").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZHOECHSTGESCHW(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZHOECHSTGESCHW").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZHOEHEMAX(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZHOEHEMAX").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZHOEHEMIN(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZHOEHEMIN").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZHUBRAUM(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZHUBRAUM").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZKLARTEXT_TYP(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZKLARTEXT_TYP").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZKLTXT(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZKLTXT").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZKRAFTSTOFF_TXT(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZKRAFTSTOFF_TXT").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZLAENGEMAX(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZLAENGEMAX").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZLAENGEMIN(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZLAENGEMIN").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZLEISTUNGSGEW(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZLEISTUNGSGEW").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZLGEW(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZLGEW").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZMASSEFAHRBMAX(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZMASSEFAHRBMAX").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZMASSEFAHRBMIN(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZMASSEFAHRBMIN").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZMAX_OCCUPANTS(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZMAX_OCCUPANTS").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZNATIONALE_EMIK(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZNATIONALE_EMIK").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZNENNLEISTUNG(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZNENNLEISTUNG").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZNULA(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZNULA").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZNUM_AXLE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZNUM_AXLE").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZORHI(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZORHI").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZORVO(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZORVO").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZPRFZ(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZPRFZ").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZREHI(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZREHI").NotNullOrEmpty().Trim();
		}

		public static DateTime? GetExportParameter_ZZREPLA_DATE(ISapDataService sap)
		{
			return sap.GetExportParameter<DateTime?>("ZZREPLA_DATE");
		}

		public static string GetExportParameter_ZZREVO(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZREVO").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZREVOLUTIONS(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZREVOLUTIONS").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZSDB9(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZSDB9").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZSLD(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZSLD").NotNullOrEmpty().Trim();
		}

		public static decimal? GetExportParameter_ZZSPEED_MAX(ISapDataService sap)
		{
			return sap.GetExportParameter<decimal?>("ZZSPEED_MAX");
		}

		public static string GetExportParameter_ZZSTANDGERAEUSCH(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZSTANDGERAEUSCH").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZSTPL(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZSTPL").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZSTUETZLAST(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZSTUETZLAST").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZTANK(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZTANK").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZTEXT_AUFBAU(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZTEXT_AUFBAU").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZTYP(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZTYP").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZTYPA(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZTYPA").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZTYPE_TEXT(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZTYPE_TEXT").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZTYP_SCHL(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZTYP_SCHL").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZTYP_VVS_PRUEF(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZTYP_VVS_PRUEF").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZUNIT_POWER(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZUNIT_POWER").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZVARIANTE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZVARIANTE").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZVERSION(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZVERSION").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZVVS_SCHLUESSEL(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZVVS_SCHLUESSEL").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZZULGESGEW(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZZULGESGEW").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_ZZZULGESGEWSTAAT(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("ZZZULGESGEWSTAAT").NotNullOrEmpty().Trim();
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
