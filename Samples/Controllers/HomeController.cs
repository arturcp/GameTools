using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameMotor;

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
