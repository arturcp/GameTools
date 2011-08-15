using System.Configuration;

namespace GameMotor
{
    public static class GameSettings
    {
        public static string Xml { get { return ConfigurationManager.AppSettings["Game.Round.Index"]; } }
        public static string RoundSpan { get { return ConfigurationManager.AppSettings["Game.Round.Span"]; } }
        public static string ApplicationKey { get { return ConfigurationManager.AppSettings["Game.Round.ApplicationKey"]; } }
    }
}
