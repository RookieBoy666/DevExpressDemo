using System.Data;

namespace DXApplication.Page
{
    public class CommonData
    {
        private PageData pds;
        private DataTable dtdata;

        /// <summary>
        /// 分页信息
        /// </summary>
        public PageData pd { set { this.pds = value; } get { if (this.pds == null) this.pds = new PageData(); return this.pds; } }
        /// <summary>
        /// 列表
        /// </summary>
        public DataTable dtData { set { this.dtdata = value; } get { if (this.dtdata == null) this.dtdata = new DataTable(); return dtdata; } }
        /// <summary>
        /// 返回状态信息
        /// </summary>
        public StateCode stateCode { set; get; }
        /// <summary>
        /// sql语句的条件
        /// </summary>
        public string condition { set; get; }
        /// <summary>
        /// 排序
        /// </summary>
        public string orderOrGroup { set; get; }
    }
}
