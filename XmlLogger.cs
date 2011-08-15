using System;
using System.IO;
using System.Xml;

namespace GameMotor
{
    public static class XmlLogger
    {
        private static string FILENAME = "rounds.xml";

        public static string FilePath { get { return string.Format("{0}\\{1}_{2}", GameSettings.Xml, GameSettings.ApplicationKey, FILENAME); } }

        public static void Initialize()
        {
            if (!Directory.Exists(GameSettings.Xml))
                Directory.CreateDirectory(GameSettings.Xml);

            if (!File.Exists(string.Format(GameSettings.Xml, FILENAME)))
                XmlLogger.Create();
        }

        private static void Create()
        {
            XmlDocument document = new XmlDocument();
            XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "UTF-8", null);
            
            document.AppendChild(declaration);

            XmlElement element = document.CreateElement("rounds");            
            document.AppendChild(element);
            document.Save(FilePath);            
        }

        public static void Write(DateTime? startedOn, DateTime? finishedOn, string comments) //DateTime executionPlan, 
        {
            XmlDocument document = new XmlDocument();
            //document.LoadXml(FilePath);
            document.Load(FilePath);

            XmlElement round = document.CreateElement("round");
            XmlElement startedTime = document.CreateElement("startedOn");
            startedTime.InnerText = startedOn.HasValue ? startedOn.ToString() : string.Empty;
            XmlElement finishedTime = document.CreateElement("finishedOn");
            finishedTime.InnerText = finishedOn.HasValue ? finishedOn.ToString() : string.Empty;
            XmlElement comment = document.CreateElement("comments");
            comment.InnerText = comments;

            round.AppendChild(startedTime);
            round.AppendChild(finishedTime);
            round.AppendChild(comment);            

            document.DocumentElement.AppendChild(round);
            document.Save(FilePath);
        }

        public static string ShowIndex()
        {
            XmlDocument document = new XmlDocument();
            document.Load(FilePath);
            return document.OuterXml;
        }        
    }
}
