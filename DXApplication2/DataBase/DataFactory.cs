using SqlSugar;
using System;

namespace DXApplication2.DataBase
{
    public class DataFactory
    {

        public static SqlSugarClient openCon()
        {
            //Type type = typeof(BaseDao);
            //BaseDao bd = Activator.CreateInstance(type) as BaseDao;
            BaseDao bd = new BaseDao();
            return bd.getCon();
        }

    }
    public class DataFactory<T>
    {
        public static T getInstance()
        {
            Type type = typeof(T);
            T t = (T)Activator.CreateInstance(type);
            return t;
        }
    }
}
