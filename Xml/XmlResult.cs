using System;
using System.Web.Mvc;
using System.IO;
using System.Xml.Serialization;
using System.Web;

namespace GameMotor
{
    public class XmlActionResult<T> : ActionResult
    {
        public T Data { private get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            HttpContextBase httpContextBase = context.HttpContext;
            httpContextBase.Response.Buffer = true;
            httpContextBase.Response.Clear();

            string fileName = DateTime.Now.ToString("ddmmyyyyhhss") + ".xml";
            //httpContextBase.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            httpContextBase.Response.ContentType = "text/xml";

            using (StringWriter writer = new StringWriter())
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                xml.Serialize(writer, Data);
                httpContextBase.Response.Write(writer);
            }
        }
    }
}
