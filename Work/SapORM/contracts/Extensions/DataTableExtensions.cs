using System.Data;

namespace SapORM.Contracts
{
    public static class DataTableExtensions
    {
        public static void Copy(this DataTable dtSrc, DataTable dtDst)
        {
            foreach (DataRow rowSrc in dtSrc.Rows)
            {
                var rowDst = dtDst.NewRow();
                foreach (DataColumn column in dtDst.Columns)
                {
                    rowDst[column.ColumnName] = rowSrc[column.ColumnName];
                }
                dtDst.Rows.Add(rowDst);
            }
        }
    }
}
