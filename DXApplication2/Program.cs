using DXApplication;
using System;
using System.Threading;
using System.Windows.Forms;

namespace DXApplication2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //WaitDialogForm wdf = new WaitDialogForm("提示", "正在加载配置......");
            //int i = 1;
            //for (int j = 1; j < i; j++)
            //{
            //    Thread.Sleep(3000);
            //    string id = i.ToString() + "%";
            //    string jd = j.ToString() + "%";
            //    wdf.SetCaption("执行进度（" + jd + "/" + id + "）");
            //}


            //WaitDialogForm waitDialogForm = new WaitDialogForm() ;
            //new Thread((ThreadStart)delegate
            //{
            //    waitDialogForm = new WaitDialogForm("请稍后...", "正在加载系统应用", new Size(300, 40));
            //    Application.Run(waitDialogForm);
            //}).Start();
            ////TODO:
            //waitDialogForm.Invoke((EventHandler)delegate { waitDialogForm.Close(); });




            int i = 6;
            ShowDialogForm sdf = new ShowDialogForm("Point", "Loading...", "Please be patient for dozens of seconds ", i);
            for (int j = 1; j < i; j++)
            {
                Thread.Sleep(300);
                string id = j.ToString() + "%";
                string jd = i.ToString() + "%";
                sdf.SetCaption("Execution Progress（" + id + "/" + jd + "）");
            }
            sdf.Close();



            Application.Run(new MainForm());
        }
    }
}
