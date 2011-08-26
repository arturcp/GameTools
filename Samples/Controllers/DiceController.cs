using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameTools.Dice;

namespace Samples.Controllers
{
    public class DiceController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Show(FormCollection form)
        {
            int faces = 0;
            try
            {
                faces = int.Parse(form["faces"]);
            }
            catch
            {
                faces = 6;
            }

            ViewBag.DiceResult = DiceRoller.Throw(faces);
            return View("Index");
        }
    }
}
