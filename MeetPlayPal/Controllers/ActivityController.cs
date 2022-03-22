using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MeetPlayPal.Controllers
{
    public class ActivityController : Controller
    {
        // GET: Dashboard
        public ActionResult Interaction()
        {
            return View();
        }

        public ActionResult MessageList()
        {
            return View();
        }

        public ActionResult PartnerList()
        {
            return View();
        }

        public ActionResult PostMessage()
        {
            return View();
        }
    }
}