using System;
using System.IO;
using System.Text;
using LostKingdom.GameContent.Localization;

namespace LostKingdom.Utils
{
    public class Logger
    {
        public static readonly string SenderGameName = GameLocalization.GetText("Logger.Sender.GameName");
        public static readonly string WarningSender = GameLocalization.GetText("Logger.Sender.Warning");
        public static readonly string ErrorSender = GameLocalization.GetText("Logger.Sender.Error");
        public string Path { get; private set; }
        public string Name { get; set; }

        public Logger(string path = null, string name = "Logger.log")
        {
            this.Path = System.IO.Path.GetFullPath(path ?? Environment.CurrentDirectory);
            this.Name = name;
            FileStream stream = new FileStream(System.IO.Path.Combine(this.Path, this.Name), FileMode.Create);
            stream.Close();
        }

        public void Write(string log, string who)
        {
            StreamWriter writer = new StreamWriter(System.IO.Path.Combine(this.Path, this.Name), true, Encoding.UTF8);
            writer.WriteLine(string.Format("[{0}] [{1}]: {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), who, log));
            writer.Close();
        }

        public void WrittenByWarningSender(string log, Action solution = null)
        {
            this.Write(log, Logger.WarningSender);
            solution?.Invoke();
        }

        public void WrittenByErrorSender(string log, Action solution = null)
        {
            this.Write(log, Logger.ErrorSender);
            solution?.Invoke();
        }
    }
}
