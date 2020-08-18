using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DXApplication2.DataBase
{
    public class Conf
    {
        public static string filePath = "Config/FNServerConf.json";
        public static string sqlFilePath = "Assets/SQL";
        /// <summary>
        /// 获取文件物理路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string getRootPath(string path)
        {
            string root1 = System.Environment.CurrentDirectory;
            root1 = root1.Replace("bin\\Debug", "\\");
            path = path.Replace("/", "\\");
            //  writeLog(root1 + "\\" + path);
            return root1 + "\\" + path;
        }
        public static string getXMLSelectSql(string filePath, string id)
        {
            filePath = sqlFilePath + filePath;
            string result = "";
            //  string path = HttpContext.Current.Server.MapPath(filePath);
            string path = getRootPath(filePath);
            if (File.Exists(path))
            {
                XDocument doc = XDocument.Load(path);
                var data = from d in doc.Descendants("select")
                           where d.Attribute("id").Value.Equals(id)
                           select new { sql = d.Value };

                result = data.Select(a => a.sql).FirstOrDefault();
                result = result.Replace("#", "");
            }
            return Regex.Replace(result, "[\f\n\r\t\v]", " ");

        }
        //public static string getNode(string[] keys)
        //{
        //    string result = "";
        //    string path = FNFun.getRootPath(filePath);
        //    if (File.Exists(path))
        //    {
        //        using (StreamReader file = new StreamReader(path, Encoding.UTF8))
        //        {
        //            using (JsonTextReader reader = new JsonTextReader(file))
        //            {
        //                JObject ja = (JObject)JToken.ReadFrom(reader);
        //                result = getStr(keys, 0, ja);
        //            }
        //        }

        //    }
        //    return result;
        //}

        //public static string getStr(string[] keys, int i, XObject job)
        //{
        //    string result = "";
        //    switch (keys.Length)
        //    {
        //        case 1:
        //            result = job[keys[0]].ToString();
        //            break;
        //        case 2:
        //            result = job[keys[0]][keys[1]].ToString();
        //            break;
        //        case 3:
        //            result = job[keys[0]][keys[1]][keys[2]].ToString();
        //            break;
        //        case 4:
        //            result = job[keys[0]][keys[1]][keys[2]][keys[3]].ToString();
        //            break;
        //    }
        //    return result;
        //}
        //public static void setNode(string[] keys, string value)
        //{
        //    string path = FNFun.getRootPath(filePath);

        //    if (File.Exists(path))
        //    {
        //        JObject obj = null;
        //        using (StreamReader file = new StreamReader(path, Encoding.UTF8))
        //        {
        //            using (JsonTextReader reader = new JsonTextReader(file))
        //            {

        //                obj = (JObject)JToken.ReadFrom(reader);
        //                setValue(keys, obj, value);
        //            }

        //        }
        //        using (Stream s = new FileStream(path, FileMode.Create))
        //        {
        //            using (StreamWriter wtyeu = new StreamWriter(s, Encoding.UTF8))
        //            {
        //                wtyeu.Write(obj.ToString());
        //            }
        //        }


        //    }
        //}

        ///// <summary>
        ///// 设置json值
        ///// </summary>
        ///// <param name="keys"></param>
        ///// <param name="i"></param>
        ///// <param name="jArray"></param>
        ///// <returns></returns>
        //public static void setValue(string[] keys, IObject jo, string value)
        //{
        //    int len = keys.Length;
        //    switch (len)
        //    {
        //        case 1:
        //            jo[keys[0]] = value;
        //            break;
        //        case 2:
        //            jo[keys[0]][keys[1]] = value;
        //            break;
        //        case 3:
        //            jo[keys[0]][keys[1]][keys[2]] = value;
        //            break;
        //        case 4:
        //            jo[keys[0]][keys[1]][keys[2]][keys[3]] = value;
        //            break;
        //    }
        //}




    }

}
