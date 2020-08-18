using DevExpress.XtraEditors;
using DXApplication.DataBase;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace DXApplication2
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        public static string connStr  = "data source=.;user id=zjp;password=123456;initial catalog=FN_TPCS;Persist Security Info=true";



        DataTable wholeData = new DataTable();//数据集总表
        public int pagesize = 20;//页行数
        public int pageIndex = 1;//当前页  
        public int pageCount;    //总页数    
        public string TableNmae = "order_plan_control";
        string OrderBy = "id";
        string StrWhere = "";
        int row;
        public MainForm()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            bindingNavigatorMoveLastItem.Enabled = true;
            DataSet dataset = GetListByPage(TableNmae, StrWhere, OrderBy, pageIndex, pagesize);
            gridControl1.DataSource = dataset.Tables[0];
            BindPageGridList(TableNmae, StrWhere);
            bindingNavigator();
            //LookUp
            using (DataTable dt = CatchServer.getPbProvider())
            {
                searchLookUpEdit1.Properties.DataSource = dt;
            }
            //sreachList("1=1");
        }
        private void sreachList(string sqlSreach)
        {
            gridControl1.DataSource = GetTableSreach.tableSreach.table(sqlSreach);
            gridView1.BestFitColumns();
            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;
        }
        private void tileBar_SelectedItemChanged(object sender, TileItemEventArgs e)
        {
            navigationFrame.SelectedPageIndex = tileBarGroupTables.Items.IndexOf(e.Item);
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            setWholeData();
        }
        private void setWholeData()
        {
            StringBuilder @string = new StringBuilder();

            //订单号
            if (!string.IsNullOrEmpty(this.sOrderNo.Text))
            {
                @string.Append($" and  order_no like '%{this.sOrderNo.Text}%'");
            }
            //款号
            if (!string.IsNullOrEmpty(this.txtOrder.Text))
            {
                @string.Append($"  and sample_material_no like '%{this.txtOrder.Text}%'");
            }
            //供应商
            if (!string.IsNullOrEmpty(this.searchLookUpEdit1.Text))
            {
                @string.Append($"  and pb_provider like '%{this.searchLookUpEdit1.Text}%'");
            }

            StrWhere = @string.ToString();
            DataSet dataSet = new DataSet();
            dataSet = GetListByPage(TableNmae, StrWhere, OrderBy, pageIndex, pagesize);
            gridControl1.DataSource = dataSet.Tables[0];

            row = GetRecordCount(TableNmae, StrWhere);
            if (row % pagesize > 0)
            {
                pageCount = row / pagesize + 1;
            }
            else
            {
                pageCount = row / pagesize;
            }
            pageIndex = 1;

            bindingNavigator();
            BindPageGridList(TableNmae, StrWhere);
        }


        #region

        /// <summary>
        /// 绑定分页控件和GridControl数据
        /// </summary>
        /// <author></author>
        /// <time></time>
        /// <param name="strWhere">查询条件</param>
        public void BindPageGridList(string TableName, string strWhere)
        {
            bindingNavigatorMovePreviousItem.Enabled = true;
            bindingNavigatorMoveNextItem.Enabled = true;
            bindingNavigatorPositionItem.Enabled = true;
            //记录获取开始数
            int startIndex = (pageIndex - 1) * pagesize + 1;
            //结束数
            int endIndex = pageIndex * pagesize;
            //总行数
            row = GetRecordCount(TableName, strWhere);
            //int row = 40;
            //获取总页数  
            if (row % pagesize > 0)
            {
                pageCount = row / pagesize + 1;
            }
            else
            {
                pageCount = row / pagesize;
            }

            if (pageIndex == 1)
            {
                bindingNavigatorMovePreviousItem.Enabled = false;
                //nvgtDataPager.Buttons.CustomButtons[1].Enabled = false; ;
            }
            //最后页时获取真实记录数
            if (pageCount == pageIndex)
            {
                endIndex = row;
                bindingNavigatorMoveNextItem.Enabled = false;
            }
            //分页获取数据列表
            DataTable dt = GetListByPage(TableNmae, strWhere, "", startIndex, endIndex).Tables[0];
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        /// 
        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string TableName, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) FROM  " + TableName);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where 1=1 " + strWhere);
            }
            object obj = GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 返回数据的行数
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns></returns>
        private object GetSingle(string strSQL)
        {
            Object obj = new Object();
            SqlConnection Sqlconn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand(strSQL.ToString(), Sqlconn);
            Sqlconn.Open();
            SqlDataAdapter da = new SqlDataAdapter(strSQL.ToString(), Sqlconn);
            DataSet dataSet = new DataSet();
            da.Fill(dataSet);
            obj = dataSet.Tables[0].Rows[0][0].ToString();     //obj可能为0
            return obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="startIndex">起始页</param>
        /// <param name="endIndex">结束页</param>
        /// <returns></returns>
        public DataSet GetListByPage(string TableName, string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Rowssss as 行号,order_no,id , saleman_name, sample_material_no, first_dlvdate, last_dlvdate, status, update_time, create_time, update_user_id, create_user_id, sunit ,pb_provider FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.ID desc");
            }
            strSql.Append(")AS Rowssss, T.*  from  " + TableName + " T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE  1=1  " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Rowssss between {0} and {1}", startIndex, endIndex);
            return Query(strSql.ToString());
        }
        /// <summary>
        /// 查询返回DataSet
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private DataSet Query(string v)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection Sqlconn = new SqlConnection(connStr);
                SqlCommand cmd = new SqlCommand(v.ToString(), Sqlconn);
                Sqlconn.Open();

                SqlDataAdapter da = new SqlDataAdapter(v.ToString(), Sqlconn);
                da.Fill(ds);
                Sqlconn.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "查找失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return ds;

        }
        /// <summary>
        /// 前一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            pageIndex--;
            bindingNavigator();
            //绑定分页控件和GridControl数据
            DataSet dataset = GetListByPage(TableNmae, StrWhere, OrderBy, (pageIndex - 1) * pagesize + 1, pageIndex * pagesize);
            gridControl1.DataSource = dataset.Tables[0];
            BindPageGridList(TableNmae, StrWhere);
            bindingNavigatorMoveLastItem.Enabled = true;
        }
        /// <summary>
        /// 后一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            pageIndex++;
            bindingNavigator();
            //绑定分页控件和GridControl数据
            DataSet dataset = GetListByPage(TableNmae, StrWhere, OrderBy, (pageIndex - 1) * pagesize + 1, pageIndex * pagesize);
            gridControl1.DataSource = dataset.Tables[0];
            BindPageGridList(TableNmae, StrWhere);
            bindingNavigatorMoveFirstItem.Enabled = true;
        }
        /// <summary>
        /// Enter键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EveryRow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pagesize = int.Parse(EveryRow.Text);
                //绑定分页控件和GridControl数据
                DataSet dataset = GetListByPage(TableNmae, StrWhere, OrderBy, (pageIndex - 1) * pagesize + 1, pageIndex * pagesize);
                gridControl1.DataSource = dataset.Tables[0];
                BindPageGridList(TableNmae, StrWhere);
                bindingNavigator();
            }
        }

        /// <summary>
        /// Enter键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bindingNavigatorPositionItem_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pagesize = int.Parse(EveryRow.Text);
                pageIndex = int.Parse(bindingNavigatorPositionItem.Text);
                //绑定分页控件和GridControl数据
                DataSet dataset = GetListByPage(TableNmae, StrWhere, OrderBy, (pageIndex - 1) * pagesize + 1, pageIndex * pagesize);
                gridControl1.DataSource = dataset.Tables[0];
                BindPageGridList(TableNmae, StrWhere);
                bindingNavigator();
            }
        }
        /// <summary>
        /// 绑定当前页/总页
        /// </summary>
        public void bindingNavigator()
        {
            bindingNavigatorPositionItem.Text = pageIndex.ToString();
            bindingNavigatorCountItem.Text = "/" + pageCount.ToString();
            toolStripLabel2.Text = string.Format("当前第 {1} 页   共 {0} 页   共 {2} 条数据", pageCount.ToString(), pageIndex.ToString(), row);
        }
        /// <summary>
        /// 跳到首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            pageIndex = 1;
            //绑定分页控件和GridControl数据
            DataSet dataset = GetListByPage(TableNmae, StrWhere, OrderBy, (pageIndex - 1) * pagesize + 1, pageIndex * pagesize);
            gridControl1.DataSource = dataset.Tables[0];
            BindPageGridList(TableNmae, StrWhere);
            bindingNavigator();
            bindingNavigatorMoveFirstItem.Enabled = false;
            bindingNavigatorMoveLastItem.Enabled = true;
        }
        /// <summary>
        /// 跳到尾页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            pageIndex = pageCount;
            //绑定分页控件和GridControl数据
            DataSet dataset = GetListByPage(TableNmae, StrWhere, OrderBy, (pageIndex - 1) * pagesize + 1, pageIndex * pagesize);
            gridControl1.DataSource = dataset.Tables[0];
            BindPageGridList(TableNmae, StrWhere);
            bindingNavigator();
            bindingNavigatorMoveFirstItem.Enabled = true;
            bindingNavigatorMoveLastItem.Enabled = false;
        }
        /// <summary>
        /// 导出为Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportData_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "导出Excel";
            saveFileDialog.Filter = "Excel文件(*.xls)|(*.xlsx)";
            DialogResult dialogResult = saveFileDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                gridControl1.ExportToXls(saveFileDialog.FileName, options);
                DevExpress.XtraEditors.XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion
    }
}