using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.IO;
using System.Xml;
using System.Web;


namespace GameMotor
{
    public class RoundControl
    {
        public DateTime lastExecutionDate = DateTime.Now.Subtract(new TimeSpan(0, 1, 0, 0, 0));

        public int MinutesPerRound { get; set; }

        public RoundControl()
        {
            MinutesPerRound = string.IsNullOrEmpty(GameSettings.RoundSpan) ? 60 : int.Parse(GameSettings.RoundSpan);            
        }

        public List<string> roundLog = null;

        public string CACHE_KEY = "RoundStatus";

        public delegate void RoundExecutionDelegate();
        public event RoundExecutionDelegate OnRoundExecution;

        public void StartRoundControl()
        {
            roundLog = new List<string>();
            DateTime now = DateTime.Now;            
            try { CheckRoundAction(); } catch (Exception error) { XmlLogger.Write(now, DateTime.Now, error.Message); }
        }

       /* public void ExecuteRoundCalculation()
        {
            roundLog.Add(DateTime.Now.ToString());
        }*/

        public void CheckRoundAction()
        {
            DateTime now = DateTime.Now;            
            Cache cache = new Cache();

            if (now.Minute % MinutesPerRound == 0 && !CurrentRoundHasRun(now))
            {
                //TODO create logic to allow rollback
                //TODO block actions during roung calculation

                //Execute round
                if (OnRoundExecution != null)
                    OnRoundExecution();
                //ExecuteRoundCalculation();

                //Save now on lastExecutionDate
                lastExecutionDate = now;

                now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, 0);
                XmlLogger.Write(lastExecutionDate, DateTime.Now, "");
                //Sleep minutesPerRound minutes
                
                //HttpContext.Current.Cache.Insert(CACHE_KEY, "1", null, now.AddMinutes(MinutesPerRound), TimeSpan.Zero, CacheItemPriority.Normal, RoundCallback);
                //cache.Insert(CACHE_KEY, "1", null, now.AddMinutes(MinutesPerRound), TimeSpan.Zero, CacheItemPriority.Normal, RoundCallback);		
                cache.Insert(CACHE_KEY, "1", null, now.AddSeconds(10), TimeSpan.Zero, CacheItemPriority.Normal, RoundCallback);		
            }
            else
            {
                XmlLogger.Write(now, DateTime.Now, "Not executed");
                //HttpContext.Current.Cache.Insert(CACHE_KEY, "1", null, now.AddMinutes(now.Minute % MinutesPerRound).AddSeconds(-1 * now.Second), TimeSpan.Zero, CacheItemPriority.Normal, RoundCallback);
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
