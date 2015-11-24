using System.Data;

namespace AppRemarketing.lib
{
    public class HistorieAusstattung
    {
        public DataTable ModelTable { get; set; }
        public DataTable AussenFarbeTable { get; set; }
        public DataTable InnenFarbeTable { get; set; }
        public DataTable AusstattungTable { get; set; }

        public HistorieAusstattung(DataTable vorschadenTable)
        {
            BuildAusstattungView(vorschadenTable);
        }

        void BuildAusstattungView(DataTable dataTable)
        {
            dataTable.DefaultView.RowFilter = "PRNR_TYP='M'";
            ModelTable = dataTable.DefaultView.ToTable();

            dataTable.DefaultView.RowFilter = "PRNR_TYP='A'";
            AussenFarbeTable = dataTable.DefaultView.ToTable();

            dataTable.DefaultView.RowFilter = "PRNR_TYP='I'";
            InnenFarbeTable = dataTable.DefaultView.ToTable();

            dataTable.DefaultView.RowFilter = "PRNR_TYP='P' OR PRNR_TYP='E'";
            AusstattungTable = dataTable.DefaultView.ToTable();
            AusstattungTable.Columns.Add(new DataColumn("Pos", typeof(System.Int32)));
            for (int i = 0; i < AusstattungTable.Rows.Count; i++)
                AusstattungTable.Rows[i]["Pos"] = i + 1;
        }
    }
}