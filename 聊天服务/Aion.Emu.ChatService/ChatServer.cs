using System;
using System.Diagnostics;
using Aion.Emu.Common;
using log4net;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.ChatService
{
    [StandardModule]
    internal sealed class ChatServer
    {
        private static string title = "Aion Emu - ChatServer";

        private static ILog log = LogManager.GetLogger(typeof(ChatServer));

        [STAThread]
        public static void Main()
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo("log4net.config"));

                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                AppDomain.CurrentDomain.UnhandledException += UnhandledException;
                Console.Title = title;
                Util.SystemInfo();
                Stopwatch stopwatch = Stopwatch.StartNew();
                CronService.GetInstance().Initialize();
                Util.Section("核心配置");
                Configs.LoadConfig();
                Util.Section("数据连接");
                DAOManager.Initialize();
                Util.Section("注册服务");
                GameService.Load();
                Util.Section("网络服务");
                NewLateBinding.LateCall(SlbService.GetInstance(), null, "Start", new object[0], null, null, null, IgnoreReturn: true);
                ChatPacketFactory.GetInstance();
                AionListener.GetInstance().Start();
                GamePacketFactory.GetInstance();
                GameListener.GetInstance().Start();
                Util.Section("");
                stopwatch.Stop();
                string text = string.Format("聊天服务器启动完成,耗时{0}秒!", ((double)stopwatch.ElapsedMilliseconds / 1000.0).ToString("0.00"));
                Console.WriteLine(new string(' ', Conversions.ToInteger(Util.StringLength(text))) + text);
                Util.Section("");
                if (ChatConfig.WORKING_MEMORY_SET_ENABLE)
                {
                    WorkingMemorySet.GetInstance().Initialize(ChatConfig.WORKING_MEMORY_SET_CRON);
                }
            }
            catch (Exception err)
            {
                log.Error(err.Message, err);
            }
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            if (e.IsTerminating)
            {
                log.Fatal((object)(ex.TargetSite.ReflectedType.FullName + " : " + ex.Message + "\r\n" + ex.StackTrace));
                Console.WriteLine();
                Console.Write("请按任意键退出...");
                Console.ReadKey(intercept: true);
                Environment.Exit(0);
            }
            else
            {
                log.Error((object)(ex.TargetSite.ReflectedType.FullName + " : " + ex.Message + "\r\n" + ex.StackTrace));
            }
        }
    }
}
