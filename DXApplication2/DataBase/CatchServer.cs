using DXApplication2.DataBase;
using System.Data;

namespace DXApplication.DataBase
{
    public class CatchServer
    {
        public static DataTable getPbProvider(int type = 0)
        {
            object o = Cache.GetCache("pbProvider");
            if (o == null)
            {
                string sql = "";
                if (type == 0)
                {
                    sql = Conf.getXMLSelectSql(SQLPath.MainSQL, "pbProvider");

                }
                DataTable dt = DataFactory.openCon().Ado.GetDataTable(sql);
                Cache.SetCache("pbProvider", dt);

                return dt;
            }
            else
            {
                return (DataTable)o;
            }
        }
    }

}
