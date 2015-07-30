using System.Data;

namespace AppRemarketing.lib
{
    public class HistorieVorschaden
    {
        public DataView VorschadenView { get; set; }
  
        public HistorieVorschaden(DataTable vorschadenTable)
        {
            BuildVorschadenView(vorschadenTable);
        }

        void BuildVorschadenView(DataTable vorschadenDataTable)
        {

            foreach (DataRow dRow in vorschadenDataTable.Rows)
            {

                dRow["ERDAT"] = dRow["ERDAT"];
                dRow["PREIS"] = dRow["PREIS"];
                dRow["SCHAD_DAT"] = dRow["SCHAD_DAT"];
                dRow["DAT_UPD_VORSCH"] = dRow["DAT_UPD_VORSCH"];
                dRow["WRTMBETR"] = dRow["WRTMBETR"];
            }

            VorschadenView = vorschadenDataTable.DefaultView;
        }

    }
 
}