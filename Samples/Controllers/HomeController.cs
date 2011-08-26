using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameTools;

namespace Samples.Controllers
{
    public class HomeController : Controller
    {
        RoundControl round = new RoundControl(1);

        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to Game Motor Samples!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create()
        {
            round.OnRoundExecution += new RoundControl.RoundExecutionDelegate(round_OnRoundExecution);
            round.StartRoundControl();            

            ViewBag.Message = "Processo iniciado";
            return View("Index");
        }

        [HttpPost]
        public ActionResult Schedule()
        {
            DateTime date = DateTime.Parse(Request["scheduleDate"]);
            round.OnRoundExecution += new RoundControl.RoundExecutionDelegate(round_OnRoundExecution);
            round.ScheduleStart(date);

            ViewBag.Message = "Processo agendado";
            string hour = date.Hour < 12? date.ToShortTimeString() + " AM" : string.Format("{0}:{1}:{2} PM", date.Hour - 12, date.Minute, date.Second);
            string formattedDate = date.Month.ToString().PadLeft(2, '0') + "/" + date.Day.ToString().PadLeft(2, '0') + "/" + date.Year.ToString().PadLeft(4, '0');
            ViewBag.IsSchedule = string.Format("{0} {1}", formattedDate, hour);

            return View("Index");
        }

        [HttpPost]
        public ActionResult Delete()
        {
            round.StopRoundControl();
            ViewBag.Message = "Processo interrompido";
            return View("Index");
        }

        private void round_OnRoundExecution()
        {
            //Thread.Sleep(200);
        }
    }
}
