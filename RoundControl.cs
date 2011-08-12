using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.IO;
using System.Xml;


namespace GameMotor
{
    public class RoundControl
    {
        public DateTime lastExecutionDate = DateTime.Now.Subtract(new TimeSpan(0, 1, 0, 0, 0));

        public int MinutesPerRound { get; set; }

        public RoundControl()
        {
            MinutesPerRound = string.IsNullOrEmpty(GameSettings.RoundSpan) ? 60 : int.Parse(GameSettings.RoundSpan);
            if (!Directory.Exists(GameSettings.Xml))
                Directory.CreateDirectory(GameSettings.Xml);
        }

        public Cache cache = new Cache();

        public List<string> roundLog = null;

        public string CACHE_KEY = "RoundStatus";

        public void StartRoundControl()
        {
            roundLog = new List<string>();
            CheckRoundAction();
        }

        public void ExecuteRoundCalculation()
        {
            roundLog.Add(DateTime.Now.ToString());
        }

        public void CheckRoundAction()
        {
            DateTime now = DateTime.Now;

            //roundLog.Add(CurrentRoundHasRun(now).ToString());
            if (now.Minute % MinutesPerRound == 0 && !CurrentRoundHasRun(now))
            {
                //TODO create logic to allow rollback
                //TODO block actions during roung calculation

                //Execute round calculation
                ExecuteRoundCalculation();

                //Save now on lastExecutionDate
                lastExecutionDate = now;

                now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, 0);
                //Sleep minutesPerRound minutes
                cache.Insert(CACHE_KEY, "1", null, now.AddMinutes(MinutesPerRound), TimeSpan.Zero, CacheItemPriority.Normal, RoundCallback);
            }
            else
            {
                cache.Insert(CACHE_KEY, "1", null, now.AddMinutes(now.Minute % MinutesPerRound).AddSeconds(-1 * now.Second), TimeSpan.Zero, CacheItemPriority.Normal, RoundCallback);
            }
        }


        public void RoundCallback(String k, Object v, CacheItemRemovedReason r)
        {
            CheckRoundAction();
        }

        private bool CurrentRoundHasRun(DateTime now)
        {
            return (now.Year == lastExecutionDate.Year && now.Month == lastExecutionDate.Month && now.Day == lastExecutionDate.Day && now.Hour == lastExecutionDate.Hour && now.Minute == lastExecutionDate.Minute);
        }

        private void LogRoundExecution()
        {
            using (XmlTextWriter writer = new XmlTextWriter(GameSettings.Xml, Encoding.Default))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Executions");
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }            

        }
    }
}
