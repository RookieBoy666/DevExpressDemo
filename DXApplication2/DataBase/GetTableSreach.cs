
using DXApplication.Page;
using DXApplication2.DataBase;
using System.Data;

namespace DXApplication.DataBase
{
    public class GetTableSreach
    {
        public static GetTableSreach tableSreach = new GetTableSreach();
        public DataTable table(string sqlSreach)
        {
            CommonData fNData = new CommonData();
            string sql = Conf.getXMLSelectSql(SQLPath.MainSQL, "capacityList");
            sql = string.Format(sql, sqlSreach);
            using (DataTable dt = DataFactory.openCon().Ado.GetDataTable(sql))
            {
                dt.Columns.Add("check", System.Type.GetType("System.Boolean"));
                fNData.dtData = dt;
                return dt;
            }
        }
    }
}
