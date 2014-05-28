using System.Data;

namespace DocumentTools.Services
{
	public class DataTableMailMergeSource : Aspose.Words.Reporting.IMailMergeDataSource
	{
		private readonly DataTable _dt;
		private readonly bool _onlyFirstRow;
		private int _index = -1;

		public DataTableMailMergeSource(DataTable dt, bool onlyFirstRow)
		{
			this._dt = dt;
			this._onlyFirstRow = onlyFirstRow;
		}

		public bool GetValue(string fieldName, out object fieldValue)
		{
			if (this._dt.Columns.Contains(fieldName)) {
				fieldValue = this._dt.Rows[this._index][fieldName];
				return true;
			}

		    fieldValue = null;
		    return false;
		}

		public bool MoveNext()
		{
			if ((this._onlyFirstRow && this._index > 0) || this._dt.Rows.Count <= _index + 1) {
				return false;
			}

		    _index += 1;
		    return true;
		}

		public string TableName {
			get { return this._dt.TableName; }
		}
	}
}
