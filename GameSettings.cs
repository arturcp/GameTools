using System.Configuration;

namespace GameMotor
{
    public static class GameSettings
    {
        public static string RelativeXml { get {  return ConfigurationManager.AppSettings["Game.Round.Index"]; } }
        public static string Xml = string.Empty;
    }
}
