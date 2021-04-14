
using PdfiumViewer;
using System;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using web_print_agent.Utils;

namespace web_print_agent.Service
{
    public class PrintService
    {
    
        private PdfDocument pd;
        private dynamic printParams;

        public bool SetPrint(string printParams)
        {
            try
            {
               this.printParams = DynamicJson.Parse( printParams);
                new Utils.Notification().Open("打印通知", "已接到打印指令，正在打印");
                return true;
            }
            catch (Exception ex)
            {
                MyLogService.Error("反序列化打印指令出错", ex);
                new Utils.Notification().Open("打印通知", "反序列化打印指令出错");
                return false;
            }
        }

        public bool Print()
        {
            try
            {
                TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(printParams.url, Path.Combine(System.Environment.CurrentDirectory, Convert.ToInt64(ts.TotalSeconds).ToString()+ "temp.pdf"));
                }
                pd = PdfDocument.Load(Path.Combine(System.Environment.CurrentDirectory, Convert.ToInt64(ts.TotalSeconds).ToString() + "temp.pdf"));
                var print=  pd.CreatePrintDocument();
                print.PrintController = new StandardPrintController(); //去掉打印弹出框
                print.Print();
                new Utils.Notification().Open("打印通知", "打印完成");
               
            }
            catch (Exception ex)
            {
                MyLogService.Error("解析打印指令出错", ex);
                new Utils.Notification().Open("打印通知", "解析打印指令出错");
                return false;
            }

            return true;
        }
    }
}
