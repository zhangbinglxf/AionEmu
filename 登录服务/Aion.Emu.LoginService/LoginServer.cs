using System;
using System.Diagnostics;
using Aion.Emu.Common;
using log4net;
using Microsoft.VisualBasic.CompilerServices;

namespace Aion.Emu.LoginService
{
    [StandardModule]
    internal sealed class LoginServer
    {
        private static ILog log = LogManager.GetLogger(typeof(LoginServer));

        private static string title = "Aion Emu - LoginServer";

        private static Util.HandlerRoutine handler = OnClosing;

        private static bool onSave = false;

        [STAThread]
        public static void Main()
        {
            try
            {

                log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo("log4net.config"));
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                //try
                //{
                //    Test();
                //}
                //catch (Exception err)
                //{

                //}

                AppDomain.CurrentDomain.UnhandledException += UnhandledException;
                Util.RemoveMenu();
                Util.RegeditCloseEvent(ref handler, add: true);
                Console.Title = title;
                Util.SystemInfo();
                Stopwatch stopwatch = Stopwatch.StartNew();
                CronService.GetInstance().Initialize();
                Util.Section("服务配置");
                Configs.LoadConfig();
                Util.Section("核心服务");
                DAOManager.Initialize();
                KeyGen.Initial();
                GameService.LoadGameservers();
                BannedIpController.Initialize();
                LunaController.GetInstance().Initial();
                Util.Section("网络服务");
                AionPacketFactory.GetInstance();
                LoginListener.GetInstance().Start();
                GamePacketFactory.GetInstance();
                GameListener.GetInstance().Start();
                Util.Section("充值奖励");
                PayRewardService.GetInstance();
                Util.Section("");
                stopwatch.Stop();
                string text = string.Format("登陆服务器启动完成,耗时{0}秒!", ((double)stopwatch.ElapsedMilliseconds / 1000.0).ToString("0.00"));
                string text2 = "使用快捷键 Ctrl + C 关闭服务并保存数据!";
                Console.WriteLine(new string(' ', Conversions.ToInteger(Util.StringLength(text))) + text);
                Console.WriteLine(new string(' ', Conversions.ToInteger(Util.StringLength(text2))) + text2);
                Util.Section("");
                if (LoginConfig.WORKING_MEMORY_SET_ENABLE)
                {
                    WorkingMemorySet.GetInstance().Initialize(LoginConfig.WORKING_MEMORY_SET_CRON);
                }
            }
            catch (Exception err)
            {
                log.Error(err.Message, err);
            }
        }

        private static void Test()
        {
            string[] files = System.IO.Directory.GetFiles(@"E:\Baidu\AION Server大全工具\【版本】2018.6.20-aion5.8添加南北部需配合6.18一键端使用\服务端补丁\AL-Game\data", "*.xml", System.IO.SearchOption.AllDirectories);

            foreach (var item in files)
            {
                try
                {
                    string txt = string.Empty;

                    using (System.IO.StreamReader sr = new System.IO.StreamReader(item, System.Text.Encoding.UTF8))
                    {
                        txt = sr.ReadToEnd();
                    }

                    var math = System.Text.RegularExpressions.Regex.Matches(txt, "[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD\u10000-\u10FFF]+");

                    if (math != null && math.Count > 0)
                    {
                        using (System.IO.StreamReader sr2 = new System.IO.StreamReader(item, System.Text.Encoding.GetEncoding("GB2312")))
                        {
                            txt = sr2.ReadToEnd();
                        }
                    }

                    txt = txt.Replace("\r", string.Empty);
                    txt = txt.Replace("\"gb2312\"", "\"utf-8\"");
                    txt = txt.Trim();
                    using (System.IO.FileStream fs = new System.IO.FileStream(item, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
                    {
                        fs.Write(System.Text.Encoding.UTF8.GetBytes(txt));
                    }

                }
                catch (Exception err)
                {

                }
            }
        }

        private static bool OnClosing(int dw)
        {
            if (onSave)
            {
                return true;
            }
            onSave = true;
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("开始关闭所有服务!");
            Util.RegeditCloseEvent(ref handler, add: false);
            onSave = false;
            Console.ResetColor();
            Console.WriteLine();
            Console.Write("请按任意键退出...");
            Console.ReadKey(intercept: true);
            return false;
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
