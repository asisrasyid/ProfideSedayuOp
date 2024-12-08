using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProfideSedayuOp.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult pages_misc_comingsoon() { return View(); }
        public ActionResult pages_misc_error() { return View(); }
        public ActionResult pages_misc_not_authorized() { return View(); }
        public ActionResult pages_misc_server_error() { return View(); }
        public ActionResult pages_misc_under_maintenance() { return View(); }
    }
}