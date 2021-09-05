using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static Flipsider.NPC;

namespace Flipsider.GUI
{
    public static class Logger
    {
        private static readonly int LogCount = 100;
        internal static List<string> Logs = new List<string>();

        public static int TimeWithoutLog = 0;
        public static void NewText(string LogMessage)
        {
            TimeWithoutLog = 0;
            Logs.Insert(0, LogMessage);
            if(Logs.Count > LogCount)
            {
                Logs.RemoveAt(LogCount);
            }
        }

        public static void NewText(object LogMessage)
        {
            string? LM = LogMessage.ToString();
            if (LM != null)
            {
                TimeWithoutLog = 0;
                Logs.Insert(0, LM);
                if (Logs.Count > LogCount)
                {
                    Logs.RemoveAt(LogCount);
                }
            }
        }
    }
    internal class LoggerUI : UIScreen
    {
        public float LogAlpha;
        protected override void OnLoad()
        {

        }
        protected override void OnUpdate()
        {
            Logger.TimeWithoutLog++;

            if (Logger.TimeWithoutLog > 180) LogAlpha = LogAlpha.ReciprocateTo(0);
            else LogAlpha = LogAlpha.ReciprocateTo(1,3f);
        }
        protected override void OnDraw()
        {
            int MaxOnscreenLogs = 10;
            var logger = Logger.Logs;

            Vector2 ASS = Utils.ActualScreenSize;
            int Count = MathHelper.Min(logger.Count , MaxOnscreenLogs);

            for (int i = 0; i < Count; i++)
            {
                float alpha = 1 - i / (float)MaxOnscreenLogs;
                Utils.DrawTextToLeft(logger[i], Color.Yellow * alpha * LogAlpha, new Vector2(30, ASS.Y - 30 - 20*i));
            }
        }
    }

}
