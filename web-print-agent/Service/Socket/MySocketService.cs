using Fleck;
using System;

namespace web_print_agent.Service.Socket
{
    public class MySocketService : SocketService
    {
        private PrintService printService;

        public MySocketService(PrintService printService)
        {
            this.printService = printService;
        }

        public override void OnError(Exception ex)
        {
            MyLogService.Error("socket调用发生错误：" + ex.Message, ex);
        }

        public override void OnMessage(string msg, IWebSocketConnection client)
        {
            if (printService.SetPrint(msg))
            {
                printService.Print();
                client.Send("打印完成");
                base.OnMessage(msg, client);
            }
            else
            {
                client.Send("打印失败");
            }
        }
    }
}
