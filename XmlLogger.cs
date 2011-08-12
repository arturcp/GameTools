using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.IO;

namespace GameMotor
{
    public static class XmlLogger
    {
        private static string FILENAME = "rounds.xml";

        public static void Initialize()
        {
            if (!Directory.Exists(GameSettings.Xml))
                Directory.CreateDirectory(GameSettings.Xml);

            if (!File.Exists(string.Format(GameSettings.Xml, FILENAME)))
                XmlLogger.Create();
        }

        private static void Create()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            //ds.Tables[0].Columns.Add(new DataColumn("ExecutionPlan", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("StartedOn", typeof(string)));
            dt.Columns.Add(new DataColumn("FinishedOn", typeof(string)));
            dt.Columns.Add(new DataColumn("Comment", typeof(string)));

            ds.Tables.Add(dt);

            using (FileStream xml = new FileStream(string.Format("{0}/{1}", GameSettings.Xml, FILENAME), System.IO.FileMode.Open))
            {
                try
                {
                    ds.WriteXml(xml);
                }
                catch
                {
                    //TODO Log error
                }
            }
        }

        public static void Write(DateTime? startedOn, DateTime? finishedOn, string comment) //DateTime executionPlan, 
        {
            DataSet ds = new DataSet();
            using (FileStream xml = new FileStream(string.Format("{0}/{1}", GameSettings.Xml, FILENAME), System.IO.FileMode.Append))
            {
                try
                {
                    ds.ReadXml(xml);
                    DataRow dr = ds.Tables[0].NewRow();
                    //dr["ExecutionPlan"] = executionPlan;
                    dr["StartedOn"] = startedOn.HasValue ? startedOn.ToString() : string.Empty;
                    dr["FinishedOn"] = finishedOn.HasValue ? finishedOn.ToString() : string.Empty;
                    dr["Comment"] = comment;
                    ds.Tables[0].Rows.Add(dr);
                    ds.WriteXml(xml);
                }
                catch
                {
                    //TODO Log error
                }
            }
            
        }

        public static string ShowIndex()
        {
             DataSet ds = new DataSet();
             using (FileStream xml = new FileStream(string.Format("{0}/{1}", GameSettings.Xml, FILENAME), System.IO.FileMode.Append))
             {
                 try
                 {
                     ds.ReadXml(xml);
                 }
                 catch { }
             }

             return ds.GetXml();
        }
        
    }
}
