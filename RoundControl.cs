using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace GameMotor
{
    public class RoundControl
    {
        private int MinutesPerRound { get; set; }
        private string ExecutionKey { get; set; }
        private string CacheKey { get; set; }
        private XmlLogger xmlLogger;
        public bool IsRunning { 
            get {
                var cache = HttpRuntime.Cache;
                if (cache.Get(CacheKey + ".IsRunning") != null)
                    return bool.Parse(cache[CacheKey + ".IsRunning"].ToString());
                else
                    return false;
            } 
            
            set {
                var cache = HttpRuntime.Cache;
                cache[CacheKey + ".IsRunning"] = value;
            } 
        }

        public RoundControl(string executionKey, int roundSpan)
        {
            MinutesPerRound = roundSpan <= 0 ? 60 : roundSpan;
            this.ExecutionKey = executionKey;
            this.CacheKey = string.Concat("RoundStatus.", this.ExecutionKey);
            xmlLogger = new XmlLogger(this.ExecutionKey);
        }

        public RoundControl(int roundSpan) : this("default", roundSpan){}

        public RoundControl() : this("default", 0){ }

        public List<string> roundLog = null;        

        public delegate void RoundExecutionDelegate();
        public event RoundExecutionDelegate OnRoundExecution;

        public void StartRoundControl()
        {
            xmlLogger.Initialize();
            roundLog = new List<string>();
            DateTime now = DateTime.Now;
            try { 
                CheckRoundAction();
                IsRunning = true;
            }
            catch (Exception error) { 
                xmlLogger.Write(now, DateTime.Now, error.Message); 
            }
        }

        public void StopRoundControl()
        {
            IsRunning = false;
            xmlLogger.Write(DateTime.Now, DateTime.Now, "Stopped by user");
            HttpRuntime.Cache.Remove(this.CacheKey);
        }

        public void CheckRoundAction()
        {
            DateTime now = DateTime.Now;            
            //var cache = HttpRuntime.Cache;
            AspnetCache cache = new AspnetCache();
            bool hasRun = CurrentRoundHasRun(now);
            string comment = string.Empty;
            if (now.Minute % MinutesPerRound == 0 && !hasRun)
            {
                //TODO create logic to allow rollback
                //TODO block actions during roung calculation

                //Execute round
                if (OnRoundExecution != null)
                    try{OnRoundExecution();}catch (Exception error){comment = error.Message;}

                //Save now on lastExecutionDate
                DateTime lastExecutionDate = now;

                now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, 0);
                xmlLogger.Write(lastExecutionDate, DateTime.Now, comment);
             
                //Sleep minutesPerRound minutes                
                //cache.Insert(CacheKey, "1", null, now.AddMinutes(MinutesPerRound), TimeSpan.Zero, CacheItemPriority.Normal, RoundCallback);		
                cache.Insert(CacheKey, "1", now.AddMinutes(MinutesPerRound), TimeSpan.Zero, CacheItemPriority.Normal, RoundCallback);		
            }
            else
            {
                //xmlLogger.Write(now, DateTime.Now, "Not executed");
                //cache.Insert(CacheKey, "1", null, now.AddMinutes(now.Minute % MinutesPerRound).AddSeconds(-1 * now.Second), TimeSpan.Zero, CacheItemPriority.Normal, RoundCallback);
                cache.Insert(CacheKey, "1", now.AddMinutes(now.Minute % MinutesPerRound).AddSeconds(-1 * now.Second), TimeSpan.Zero, CacheItemPriority.Normal, RoundCallback);
            }
        }

        public void RoundCallback(String k, Object v, CacheItemRemovedReason r)
        {
            if (IsRunning)
                CheckRoundAction();
        }

        private bool CurrentRoundHasRun(DateTime now)
        {
            DateTime? date = xmlLogger.GetLastExecutionDate();

            if (date == null) return false;

            DateTime lastDate = date.Value;

            return (now.Year == lastDate.Year && now.Month == lastDate.Month && now.Day == lastDate.Day && now.Hour == lastDate.Hour && now.Minute == lastDate.Minute);
        }       
    }
}
