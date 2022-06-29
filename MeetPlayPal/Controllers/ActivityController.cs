using MeetPlayPal.Models;
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

            //List<VwSolutionsModel> solns = new List<VwSolutionsModel>();
            //solns = GetMyAns(solns);
            PagingInfo info = new PagingInfo();
            info.SortField = " ";
            info.SortDirection = " ";
            info.PageSize = 20;
            info.PageCount = 10; //Convert.ToInt32(Math.Ceiling((double)(solns.Count() / info.PageSize)));
            info.CurrentPageIndex = 0;
            // var query = solns.OrderBy(c => c.QuestionID).Take(info.PageSize);
            ViewBag.PagingInfo = info;
            // return View(query.ToList());

            List<Message> lstMsg = new List<Message>();
            Message msg = new Message();
            msg.MessageText = "testing 123 testing 123 testing 123 testing 123 testing 123 testing 123 testing 123 ";
            msg.ModifiedDateTime = DateTime.Now;
            msg.Status_Code = "Ping Pong";
            msg.MsgFrom = 1;
            msg.MessageId = 234;
            lstMsg.Add(msg);

            msg = new Message();
            msg.MessageText = "testing 123 testing 123 testing 123 testing 123 testing 123 testing 123 testing 123 ";
            msg.ModifiedDateTime = DateTime.Now;
            msg.Status_Code = "Ping Pong";
            msg.MsgFrom = 2;
            msg.MessageId = 4354;
            lstMsg.Add(msg);

            return View(lstMsg);

            
        }

        public ActionResult PartnerList()
        {
            return View();
        }

        public ActionResult PostMessage()
        {
            return View();
        }
        public ActionResult Search()
        {
            return View();
        }
    }
}