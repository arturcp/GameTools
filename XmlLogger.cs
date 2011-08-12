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
        public static void Create()
        {
            DataSet ds = new DataSet();
            ds.Tables[0].Columns.Add(new DataColumn("ExecutionPlan", typeof(DateTime)));
            ds.Tables[0].Columns.Add(new DataColumn("StartedOn", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("FinishedOn", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Comment", typeof(string)));

            using (FileStream xml = new FileStream(GameSettings.Xml, System.IO.FileMode.Open))
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

        public static void Write(DateTime executionPlan, DateTime? startedOn, DateTime? finishedOn, string comment)
        {
            DataSet ds = new DataSet();
            using (FileStream xml = new FileStream(GameSettings.Xml, System.IO.FileMode.Append))
            {
                try
                {
                    ds.ReadXml(xml);
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["ExecutionPlan"] = executionPlan;
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
        
    }
}
