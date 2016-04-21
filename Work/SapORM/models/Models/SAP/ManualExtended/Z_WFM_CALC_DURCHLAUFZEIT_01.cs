using System;
using System.Data;
using GeneralTools.Models;

namespace SapORM.Models
{
    public partial class Z_WFM_CALC_DURCHLAUFZEIT_01
    {
        public partial class ES_STATISTIK 
        {
            partial void OnMappingError(Exception e, DataRow row, bool isExport)
            {
                DURCHSCHNITT_DAUER = (decimal?)row["DURCHSCHNITT_DAUER"].ToString().ToDouble(0);
                ANZ_GES = row["ANZ_GES"].ToString().ToInt(0);

                ANZ_STD_LE_03 = row["ANZ_STD_LE_03"].ToString().ToInt(0);
                ANZ_STD_04_10 = row["ANZ_STD_04_10"].ToString().ToInt(0);
                ANZ_STD_11_20 = row["ANZ_STD_11_20"].ToString().ToInt(0);
                ANZ_STD_21_30 = row["ANZ_STD_21_30"].ToString().ToInt(0);
                ANZ_STD_31_40 = row["ANZ_STD_31_40"].ToString().ToInt(0);
                ANZ_STD_GT_40 = row["ANZ_STD_GT_40"].ToString().ToInt(0);

                ANZ_KLAER_LE_03 = row["ANZ_KLAER_LE_03"].ToString().ToInt(0);
                ANZ_KLAER_04_10 = row["ANZ_KLAER_04_10"].ToString().ToInt(0);
                ANZ_KLAER_11_20 = row["ANZ_KLAER_11_20"].ToString().ToInt(0);
                ANZ_KLAER_21_30 = row["ANZ_KLAER_21_30"].ToString().ToInt(0);
                ANZ_KLAER_31_40 = row["ANZ_KLAER_31_40"].ToString().ToInt(0);
                ANZ_KLAER_GT_40 = row["ANZ_KLAER_GT_40"].ToString().ToInt(0);

                ANZ_ALLE_LE_03 = row["ANZ_ALLE_LE_03"].ToString().ToInt(0);
                ANZ_ALLE_04_10 = row["ANZ_ALLE_04_10"].ToString().ToInt(0);
                ANZ_ALLE_11_20 = row["ANZ_ALLE_11_20"].ToString().ToInt(0);
                ANZ_ALLE_21_30 = row["ANZ_ALLE_21_30"].ToString().ToInt(0);
                ANZ_ALLE_31_40 = row["ANZ_ALLE_31_40"].ToString().ToInt(0);
                ANZ_ALLE_GT_40 = row["ANZ_ALLE_GT_40"].ToString().ToInt(0);

                MappingErrorProcessed = true;
            }
        }
    }
}
