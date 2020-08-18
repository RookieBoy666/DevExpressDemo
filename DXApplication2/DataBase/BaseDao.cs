using SqlSugar;

namespace DXApplication2.DataBase
{
    public class BaseDao
    {
        private static ConnectionConfig conf = new ConnectionConfig();
        public BaseDao()
        {
            //  string conStr = ConfigurationManager.ConnectionStrings["FN_TPCS_Connection"].ConnectionString;
            string conStr = "data source=fuen.hsip.net,8808;user id=wangqk;password=wangqk;initial catalog=FN_TPCS;Persist Security Info=true";
            //string providerName = ConfigurationManager.ConnectionStrings["ConnStr"].ProviderName;
            //string conStr = "data source=192.168.88.6;user id=wangqk;password=wangqk;initial catalog=FN_TPCS;Persist Security Info=true";
            string providerName = null;
            conf.DbType = getDbType(providerName);
            conf.IsAutoCloseConnection = true;
            conf.ConnectionString = conStr;
        }
        public SqlSugarClient getCon()
        {
            return new SqlSugarClient(conf);
        }
        private DbType getDbType(string name)
        {

            return DbType.SqlServer;
        }
    }
}
