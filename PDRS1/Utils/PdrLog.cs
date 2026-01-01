using MelonLoader;
using System;

namespace PDRS1
{
    public static class PDRLog
    {
        private const string Prefix = "[PDRS1] ";

        public static void Info(string msg)
        {
            MelonLogger.Msg(ConsoleColor.Cyan, Prefix + msg);
        }

        public static void Success(string msg)
        {
            MelonLogger.Msg(ConsoleColor.Green, Prefix + msg);
        }

        public static void Warn(string msg)
        {
            MelonLogger.Warning(Prefix + msg);
        }

        public static void Error(string msg)
        {
            MelonLogger.Error(Prefix + msg);
        }

        public static void Debug(string msg)
        {
            MelonLogger.Msg(ConsoleColor.DarkCyan, Prefix + msg);
        }
    }
}