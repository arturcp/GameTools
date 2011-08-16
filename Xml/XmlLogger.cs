using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace GameMotor
{
    public class XmlLogger
    {
        public string ExecutionKey { get; set; }
        public XmlLogger(string executionKey)
        {
            this.ExecutionKey = executionKey;
        }

        private string FILENAME = "RoundControl.xml";

        public string FilePath { get { return string.Format("{0}\\{1}-{2}", GameSettings.Xml, this.ExecutionKey, FILENAME); } }

        public void Initialize()
        {
            if (!Directory.Exists(GameSettings.Xml))
                Directory.CreateDirectory(GameSettings.Xml);

            if (!File.Exists(FilePath))
                Create();
        }

        private void Create()
        {
            XmlDocument document = new XmlDocument();
            XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "UTF-8", null);
            
            document.AppendChild(declaration);

            XmlElement element = document.CreateElement("rounds");            
            document.AppendChild(element);
            document.Save(FilePath);            
        }

        public void Write(DateTime? startedOn, DateTime? finishedOn, string comments) //DateTime executionPlan, 
        {
            XmlDocument document = new XmlDocument();
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

        public string ShowIndex()
        {
            XmlDocument document = new XmlDocument();
            document.Load(FilePath);
            return document.OuterXml;
        }

        public DateTime? GetLastExecutionDate()
        {
            DateTime? result = new DateTime();
            //http://www.codeproject.com/KB/cpp/myXPath.aspx
            XPathDocument doc = new XPathDocument(FilePath);
            XPathNavigator nav = doc.CreateNavigator();

            // Compile a standard XPath expression

            XPathExpression expr;
            expr = nav.Compile("/rounds/round[comments='']/startedOn[last()]");
            XPathNodeIterator iterator = nav.Select(expr);

            // Iterate on the node set
            try
            {
                while (iterator.MoveNext())
                {
                    XPathNavigator nav2 = iterator.Current.Clone();
                    result = DateTime.Parse(nav2.Value);
                }
            }
            catch
            {
                result = null;
            }

            return result;
        }
    }
}
