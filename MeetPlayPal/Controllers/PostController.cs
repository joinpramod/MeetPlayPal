using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MeetPlayPal.Controllers
{
    public class PostMessageController : Controller
    {
        // GET: Dashboard
        //private Users user = new Users();
        //[Route("")]
        //public ActionResult UnAns(string ddType)
        //{
        //    List<QuestionModel> questions = new List<QuestionModel>();
        //    if (ModelState.IsValid)
        //    {
        //    string strSQL = string.Empty;
        //    user = (Users)Session["User"];
        //    long val = 0;

        //    if (ddType == null && RouteData.Values["id"] != null)
        //    {
        //        ddType = RouteData.Values["id"].ToString();
        //    }

        //    if (!string.IsNullOrEmpty(ddType))
        //    {
        //        try
        //        {
        //            val = Convert.ToInt64(ddType);
        //        }
        //        catch
        //        {
        //            return View("../Que/UnAns");
        //        }
        //        if (user != null && user.UserId == 1)
        //            strSQL = "Select * from Question Where QuestionId > 37861 and QuestionTypeId = " + ddType + " order by questionid desc";
        //        else
        //            strSQL = "Select top 75  * from Question Where QuestionId > 37861 and QuestionTypeId = " + ddType + " order by questionid desc";
        //    }
        //    else
        //    {
        //        if (user != null && user.UserId == 1)
        //            strSQL = "Select * from Question Where QuestionId > 37861 order by questionid desc";
        //        else
        //            strSQL = "Select top 75 * from Question Where QuestionId > 37861 order by questionid desc";
        //    }
        //    ConnManager connManager = new ConnManager();
        //    questions = connManager.GetQuestions(strSQL);
        //    }

        //    ConnManager conn = new ConnManager();
        //    List<QuestionType> items = conn.GetQuestionType();
        //    ViewBag.DDItems = items;
        //    return View(questions);
        //}

        //public ActionResult Post()
        //{
        //    ConnManager conn = new ConnManager();
        //    List<QuestionType> items = conn.GetQuestionType();
        //    QuestionType types = new QuestionType();
        //    types.Types = items;
        //    return View(types);
        //}

        //[Route("{Id}/{Title}")]
        //public ActionResult Ans(string SolutionEditor)
        //{
        //    if (Request.Form["Submit"] != null)
        //    {
        //        return InsertAns(SolutionEditor);
        //    }
        //    if (Request.Form.GetValues("DeletePost") != null)
        //    {
        //        string Id = Request.Form.Keys[0].ToString();
        //        RouteData.Values["id"] = Id;
        //        string RId = Session["DeleteReplyId"].ToString(); // RouteData.Values["RId"].ToString();
        //        string Title = RouteData.Values["Title"].ToString();                
        //        return DeleteReply(Id);
        //    }
        //    else if (Request.Form.GetValues("UP") != null || Request.Form.GetValues("DOWN") != null)
        //    {
        //        string Id = Request.Form.Keys[0].ToString();
        //        string RId = Request.Form.Keys[1].ToString();
        //        string VoteType = Request.Form.Keys[2].ToString();

        //        if (Request.Form.GetValues("UP") != null)
        //        {
        //            ProcessVotes("UP", RId, Id);
        //        }
        //        else //if (VoteType.Equals("DOWN"))
        //        {
        //            ProcessVotes("DOWN", RId, Id);
        //        }
        //        RouteData.Values["id"] = Id;
        //        VwSolutionsModel model = SetDefaults();
        //        return View("../Que/Ans", model);
        //    }
        //    else
        //    {
        //        VwSolutionsModel model = SetDefaults();
        //        return View(model);
        //    }
        //}

        //public ActionResult InsertQuestion(string txtTitle, string ddType, string EditorAskQuestion)
        //{
        //    string strTemp = "";
        //    if (Session["User"] != null)
        //    {
        //        txtTitle = txtTitle.Replace("``", "<");
        //        txtTitle = txtTitle.Replace("~~", "&#");

        //        user = (Users)Session["User"];
        //        double dblQuestionID = 0;
        //        Question question = new Question();
        //        SqlConnection LclConn = new SqlConnection();
        //        SqlTransaction SetTransaction = null;
        //        bool IsinTransaction = false;
        //        if (LclConn.State != ConnectionState.Open)
        //        {
        //            question.SetConnection = question.OpenConnection(LclConn);
        //            SetTransaction = LclConn.BeginTransaction(IsolationLevel.ReadCommitted);
        //            IsinTransaction = true;
        //        }
        //        else
        //        {
        //            question.SetConnection = LclConn;
        //        }
        //        question.QuestionTitle = txtTitle;
        //        question.QuestionTypeId = int.Parse(ddType);
        //        question.OptionID = 1;

        //        CleanBeforeInsert(ref EditorAskQuestion, ref strTemp);

        //        question.QuestionDetails = EditorAskQuestion;
        //        question.AskedDateTime = DateTime.Now;

        //        if (user.UserId == 1)
        //        {
        //            int[] myy = new int[38] { 16, 17, 18, 19, 23, 24, 25, 26, 32, 34, 35, 37, 39, 40, 41, 42, 44, 45, 46, 47, 48, 51, 52, 54, 55, 56, 57, 58, 59, 63, 69, 70, 71, 72, 73, 82, 104, 106 };
        //            Random ran = new Random();
        //            int mynum = myy[ran.Next(0, myy.Length)];
        //            question.AskedUser = mynum;
        //        }
        //        else
        //        {
        //            question.AskedUser = user.UserId;
        //        }

        //        bool result = question.CreateQuestion(ref dblQuestionID, SetTransaction);

        //        if (IsinTransaction && result)
        //        {
        //            SetTransaction.Commit();
        //            Mail mail = new Mail();
        //            mail.Body = "<a>www.codeanalyze.com/Soln.aspx?QId=" + dblQuestionID.ToString() + "&QT=" + txtTitle + "</a>";
        //            mail.FromAdd = "admin@codeanalyze.com";
        //            mail.Subject = txtTitle;
        //            mail.ToAdd = "admin@codeanalyze.com";
        //            mail.IsBodyHtml = true;

        //            if (user.Email != "admin@codeanalyze.com")
        //            {
        //                mail.SendMail();
        //            }
        //        }
        //        else
        //        {
        //            SetTransaction.Rollback();
        //        }
        //        question.CloseConnection(LclConn);


        //        string title = txtTitle;
        //        System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9 -]");
        //        txtTitle = rgx.Replace(txtTitle, "");


        //        string strAck = "Question posted successfully, we will email you when users post answers.<br /> View your posted question ";
        //        if (Request.Url.ToString().Contains("localhost"))
        //            strAck += "<a style=\"color:blue;text-decoration:underline\" href=\"/CodeAnalyzeMVC2015/Que/Ans/" + dblQuestionID.ToString() + "/" + txtTitle.ToString().Replace(" ", "-") + "\">here</a>";
        //        else
        //            strAck += "<a style=\"color:blue;text-decoration:underline\" href=\"http://codeanalyze.com/Que/Ans/" + dblQuestionID.ToString() + "/" + txtTitle.ToString().Replace(" ", "-") + "\">here</a>";
        //        strAck += "<br />";

        //        ViewBag.Ack = strAck;
        //    }

        //    ConnManager conn = new ConnManager();
        //    List<QuestionType> items = conn.GetQuestionType();
        //    QuestionType types = new QuestionType();
        //    types.Types = items;
        //    return View("../Que/Post", types);
        //}


        //public ActionResult InsertAns(string SolutionEditor)
        //{
        //    VwSolutionsModel model = new VwSolutionsModel();
        //    string strContent = SolutionEditor;
        //    string strTemp = "";

        //    if (Session["User"] != null)
        //    {
        //        user = (Users)Session["User"];
        //        string quesID = RouteData.Values["id"].ToString();

        //        ConnManager connManager = new ConnManager();
        //        connManager.OpenConnection();

        //        double dblReplyID = 0;
        //        Replies replies = new Replies();
        //        SqlConnection LclConn = new SqlConnection();
        //        SqlTransaction SetTransaction = null;
        //        bool IsinTransaction = false;
        //        if (LclConn.State != ConnectionState.Open)
        //        {
        //            replies.SetConnection = replies.OpenConnection(LclConn);
        //            SetTransaction = LclConn.BeginTransaction(IsolationLevel.ReadCommitted);
        //            IsinTransaction = true;
        //        }
        //        else
        //        {
        //            replies.SetConnection = LclConn;
        //        }

        //        replies.OptionID = 1;
        //        replies.QuestionId = double.Parse(quesID.ToString());

        //        CleanBeforeInsert(ref SolutionEditor, ref strTemp);

        //        replies.Reply = SolutionEditor;


        //        replies.RepliedDate = DateTime.Now;

        //        if (user.UserId == 1)
        //        {
        //            int[] myy = new int[38] { 16, 17, 18, 19, 23, 24, 25, 26, 32, 34, 35, 37, 39, 40, 41, 42, 44, 45, 46, 47, 48, 51, 52, 54, 55, 56, 57, 58, 59, 63, 69, 70, 71, 72, 73, 82, 104, 106 };
        //            Random ran = new Random();
        //            int mynum = myy[ran.Next(0, myy.Length)];
        //            replies.RepliedUser = mynum;

        //            int[] myvotes = new int[12] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        //            Random ran2 = new Random();
        //            int mynum2 = myvotes[ran2.Next(0, myvotes.Length)];
        //            replies.UpVotes = mynum2;

        //        }
        //        else
        //        {
        //            replies.RepliedUser = user.UserId;
        //        }

        //        bool result = replies.CreateReplies(ref dblReplyID, SetTransaction);


        //        if (IsinTransaction && result)
        //        {
        //            SetTransaction.Commit();
        //        }
        //        else
        //        {
        //            SetTransaction.Rollback();
        //        }

        //        replies.CloseConnection(LclConn);
        //        ViewBag.ReplyId = dblReplyID;
        //        model = SetDefaults();

        //        try
        //        {
        //            if (!Session["AskedUserEMail"].ToString().Contains("codeanalyze.com"))
        //            {
        //                Mail mail = new Mail();

        //                string EMailBody = System.IO.File.ReadAllText(Server.MapPath("../../../EMailBody.txt"));

        //                System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9 -]");

        //                if (model.QuestionTitle != null)
        //                {
        //                    model.QuestionTitle = rgx.Replace(model.QuestionTitle, "").Replace(" ", "-");
        //                }

        //                string strLink = "www.codeanalyze.com/Que/Ans/" + quesID.ToString() + "/" + model.QuestionTitle + "";

        //                string strBody = "Your question on CodeAnalyse has been answered by one of the users. Check now <a href=" + strLink + "\\>here</a>";

        //                mail.Body = string.Format(EMailBody, strBody);


        //                mail.FromAdd = "admin@codeanalyze.com";
        //                mail.Subject = "Code Analyze - Received response for " + model.QuestionTitle;
        //                mail.ToAdd = Session["AskedUserEMail"].ToString();
        //                mail.CCAdds = "admin@codeanalyze.com";
        //                mail.IsBodyHtml = true;

        //                if (!mail.ToAdd.ToString().ToLower().Equals("pramodh.suresh@yahoo.com"))
        //                {
        //                    mail.SendMail();
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {


        //        }
        //        GetQuestionData(quesID.ToString(), ref model);
        //        BindSolution("Select * from VwSolutions where QuestionId = " + quesID.ToString(), null);                
        //        ViewBag.lblAck = string.Empty;
        //    }
        //    else
        //    {
        //        ViewBag.lblAck = "Please sign in to post your question.";
        //    }
        //    return View("../Que/Ans", model);
        //}

        //public ActionResult DeleteReply(string Id)
        //{
        //    VwSolutionsModel model = new VwSolutionsModel();
        //    if (Session["DeleteReplyId"] != null)
        //    {
        //        if (Id != null)
        //        {
        //            ConnManager conn = new ConnManager();
        //            conn.DeleteReply(Session["DeleteReplyId"].ToString());
        //            model = SetDefaults();
        //        }
        //        Session["DeleteReplyId"] = null;
        //    }
        //    return View("../Que/Ans", model);
        //}



        //private VwSolutionsModel SetDefaults()
        //{
        //    if (Session["User"] != null)
        //    {
        //        user = (Users)Session["User"];
        //        ViewBag.UserEMail = user.Email;
        //    }

        //    string quesID;
        //    string questionTitle = string.Empty;

        //    quesID = RouteData.Values["id"].ToString();

        //    VwSolutionsModel model = new VwSolutionsModel();
        //    GetQuestionData(quesID.ToString(), ref model);

        //    questionTitle = model.QuestionTitle;

        //    if (string.IsNullOrEmpty(questionTitle))
        //    {
        //        questionTitle = RouteData.Values["title"].ToString();
        //    }

        //    ViewBag.Description = questionTitle.Replace("-", " ");
        //    ViewBag.keywords = questionTitle.Replace("-", " ");

        //    if (quesID != null)
        //    {
        //        BindSolution("Select * from VwSolutions where QuestionId =" + quesID.ToString(), ref model);
        //    }

        //    if (user.Email != null)
        //    {
        //        ViewBag.lblAck = string.Empty;
        //        ViewBag.hfUserEMail = user.Email;
        //    }
        //    else
        //    {
        //          ViewBag.lblAck = "Please sign in to post your answer.";
        //        ViewBag.hfUserEMail = string.Empty;
        //    }
        //    return model;
        //}

        //private void GetQuestionData(string strQuestionId, ref VwSolutionsModel model)
        //{
        //    ConnManager connManager = new ConnManager();
        //    connManager.OpenConnection();
        //    DataTable dsQuestion = connManager.GetQuestion(strQuestionId);
        //    connManager.DisposeConn();
        //    long quesID;

        //    string strQuestionDetails = String.Empty;

        //    if (dsQuestion != null)
        //    {
        //        if (dsQuestion.Rows.Count > 0)
        //        {
        //            quesID = long.Parse(dsQuestion.Rows[0]["QuestionId"].ToString());
        //            model.QuestionID = quesID.ToString();
        //            if (!dsQuestion.Rows[0]["EMail"].ToString().Contains("codeanalyze.com"))
        //            {
        //                model.AskedUser = dsQuestion.Rows[0]["FirstName"].ToString() + " ";
        //                if (!string.IsNullOrEmpty(dsQuestion.Rows[0]["LastName"].ToString()))
        //                {
        //                    model.AskedUser = model.AskedUser + "" + dsQuestion.Rows[0]["LastName"].ToString() + "";
        //                }

        //                if (!string.IsNullOrEmpty(dsQuestion.Rows[0]["ImageURL"].ToString()))
        //                    model.ImageURL = dsQuestion.Rows[0]["ImageURL"].ToString();
        //                else
        //                    model.ImageURL = "~/Images/Person.JPG";
        //            }
        //            else
        //            {
        //                model.ImageURL = "~/Images/Person.JPG";
        //            }


        //            Session["AskedUserEMail"] = dsQuestion.Rows[0]["EMail"].ToString();
        //            model.QuestionTitle = dsQuestion.Rows[0]["QuestionTitle"].ToString();

        //            strQuestionDetails = dsQuestion.Rows[0]["Question"].ToString().Replace("font-size: x-small", "font-size: medium");
        //            strQuestionDetails = StringClean(strQuestionDetails);

        //            model.QuestionDetails = "<table style=\"width:100%\"><tr><td>" + strQuestionDetails + "</td></tr></table>";
        //            ViewBag.QuestionDetails = model.QuestionDetails;

        //            model.QuestionViews = "<b>" + dsQuestion.Rows[0]["Views"].ToString() + "<b>";

        //        }
        //    }
        //}

        //private void BindSolution(string strQuery, ref VwSolutionsModel model)
        //{
        //    ConnManager connManager = new ConnManager();
        //    connManager.OpenConnection();
        //    DataTable dsSolution = connManager.GetDataTable(strQuery);
        //    string quesID = RouteData.Values["id"].ToString();
        //    string strReplyId = "";
        //    string lblUp, lblDown = "0";
        //    string tblReplies = "<table style=\"word-wrap:break-word; width:98%; \">";
        //    string strDeleteRow = string.Empty;
        //    string strTitle = string.Empty;

        //    if (RouteData.Values["Title"] != null)
        //    {
        //        strTitle = RouteData.Values["Title"].ToString();
        //    }
        //    else
        //    {
        //        strTitle = model.QuestionTitle;
        //    }

        //    if (dsSolution != null && dsSolution.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dsSolution.Rows.Count; i++)
        //        {

        //            lblUp = dsSolution.Rows[i]["ThumbsUp"].ToString();
        //            lblDown = dsSolution.Rows[i]["ThumbsDown"].ToString();

        //            strReplyId = dsSolution.Rows[i]["ReplyID"].ToString();
        //            Response no user details
        //            string htrResponseNoByDetailsOuterRow = "<tr>";
        //            string htcResponseNoByDetailsOuterCell = "<td style=\"background-color:lightgrey; border-radius:10px;\">";

        //            #region table
        //            string htmlTblResponseNoByDetails = "<table style=\"width:100%\">";

        //            string htmlRowResponseNoByDetails = "<tr style=\"width:100%;\">";

        //            string htcUserImage = "<td align=\"right\"> ";
        //            if (!string.IsNullOrEmpty(dsSolution.Rows[i]["ImageURL"].ToString()))
        //            {
        //                if (Request.Url.ToString().Contains("localhost"))
        //                    htcUserImage += "<img title=\"User Avatar\" src=\"/CodeAnalyzeMVC2015" + dsSolution.Rows[i]["ImageURL"].ToString().Replace("~", "") + "\" style=\"height:30px;width:30px\" />";
        //                else
        //                    htcUserImage += "<img  title=\"User Avatar\" src='" + dsSolution.Rows[i]["ImageURL"].ToString().Replace("~", "").Replace("/CodeAnalyzeMVC2015", "") + "' style=\"height:30px;width:30px\" />";
        //                htcUserImage += dsSolution.Rows[i]["ImageURL"].ToString();
        //            }
        //            else
        //            {
        //                if (Request.Url.ToString().Contains("localhost"))
        //                    htcUserImage += "<img title=\"User Avatar\" src=\"/CodeAnalyzeMVC2015/Images/Person.JPG\" style=\"height:25px;width:25px\" />";
        //                else
        //                    htcUserImage += "<img title=\"User Avatar\" src=\"/Images/Person.JPG\" style=\"height:25px;width:25px\" />";
        //            }
        //            htcUserImage += "</td>";


        //            string htcResponseNoByDetails = "<td valign=\"middle\">";


        //            #region responseNoBy
        //            string strFirstName = "";
        //            string strLastName = "";
        //            string strAnswers = "";
        //            string strRepliedDate = "";

        //            string strUserId = dsSolution.Rows[i]["UserId"].ToString();
        //            if (!string.IsNullOrEmpty(dsSolution.Rows[i]["FirstName"].ToString()))
        //            {
        //                strFirstName = dsSolution.Rows[i]["FirstName"].ToString();
        //                strLastName = dsSolution.Rows[i]["LastName"].ToString();
        //            }
        //            else
        //                strFirstName = dsSolution.Rows[i]["EMail"].ToString().Split('@')[0];
        //            strRepliedDate = dsSolution.Rows[i]["RepliedDate"].ToString().Split('@')[0];

        //            strRepliedDate = DateTime.Parse(strRepliedDate).ToShortDateString();

        //            DataTable dsCount = new DataTable();
        //            dsCount = connManager.GetDataTable("SELECT COUNT(*) FROM VwSolutions WHERE (RepliedUser = " + strUserId + ") AND (AskedUser <> " + strUserId + ")");

        //            if (dsCount != null && dsCount.Rows.Count > 0)
        //            {
        //                if (dsCount.Rows[0][0].ToString() != "0")
        //                {
        //                    if (dsCount != null && dsCount.Rows.Count > 0)
        //                        strAnswers = dsCount.Rows[0][0].ToString();
        //                    else
        //                        strAnswers = "<b>none</b>";


        //                    if (!dsSolution.Rows[i]["EMail"].ToString().Contains("codeanalyze.com"))
        //                        htcResponseNoByDetails += "Response No <b>" + (i + 1).ToString() + "</b> by <b>" + strFirstName + " " + strLastName + "</b>  ";// + strRepliedDate + "";   // Total replies by user: " + strAnswers + ".";
        //                        htcResponseNoByDetails += "<b>" + strFirstName + " " + strLastName + "</b>  " + strRepliedDate + "";   // Total replies by user: " + strAnswers + ".";

        //                    else
        //                        htcResponseNoByDetails += "Response No <b>" + (i + 1).ToString() + "</b> by <b>" + strFirstName + "</b>  ";// + strRepliedDate + "";
        //                        htcResponseNoByDetails += "<b>" + strFirstName + "</b>  " + strRepliedDate + "";
        //                    htc4.InnerHtml = "Comment No <b>" + (i + 1).ToString();
        //                }
        //                else
        //                    if (!strFirstName.ToLower().Equals("admin"))
        //                        htcResponseNoByDetails += "Response No <b>" + (i + 1).ToString() + "</b> by <b>" + strFirstName + " " + strLastName + "</b>  ";// + strRepliedDate + "";
        //                    htcResponseNoByDetails += "<b>" + strFirstName + " " + strLastName + "</b>  " + strRepliedDate + "";
        //                else
        //                    htcResponseNoByDetails += "Response No <b>" + (i + 1).ToString() + "</b> " + strRepliedDate + "";
        //                    htcResponseNoByDetails += " " + strRepliedDate;
        //            }
        //            else
        //                if (!strFirstName.ToLower().Equals("admin"))
        //                htcResponseNoByDetails += "Response No <b>" + (i + 1).ToString() + "</b> by <b>" + strFirstName + " " + strLastName + "</b>  ";// + strRepliedDate + "";
        //                htcResponseNoByDetails += "<b>" + strFirstName + " " + strLastName + "</b>  " + strRepliedDate + "";

        //            else
        //                htcResponseNoByDetails += "Response No <b>" + (i + 1).ToString() + "</b> " + strRepliedDate + " ";
        //                htcResponseNoByDetails += " " + strRepliedDate + " ";
        //            #endregion
        //            htcResponseNoByDetails += "</td>";

        //            htmlRowResponseNoByDetails += htcUserImage;
        //            htmlRowResponseNoByDetails += htcResponseNoByDetails;
        //            htmlRowResponseNoByDetails += AddThumbsUpDown(i, quesID, strReplyId, lblUp, lblDown);


        //            htmlRowResponseNoByDetails += "</tr>";
        //            htmlTblResponseNoByDetails += htmlRowResponseNoByDetails + "</table>";
        //            #endregion

        //            htcResponseNoByDetailsOuterCell += htmlTblResponseNoByDetails + "</td>";
        //            htrResponseNoByDetailsOuterRow += htcResponseNoByDetailsOuterCell + "</tr>";


        //            Solution Row
        //            string htmlRowSolutionContent = "<tr>";
        //            string htcReplyContent = "<td style=\"font-family:Calibri\" >";
        //            strReplyId = dsSolution.Rows[i]["ReplyId"].ToString();
        //            #region Reply
        //            string strReply = dsSolution.Rows[i]["Reply"].ToString().Replace("font-size: x-small", "font-size: 16px");

        //            strReply = StringClean(strReply);

        //            #endregion
        //            htcReplyContent += strReply + "</td>";
        //            htmlRowSolutionContent += htcReplyContent + "</tr>";



        //            if (ViewBag.ReplyId != null && strReplyId == Convert.ToString(ViewBag.ReplyId))
        //            {
        //                strDeleteRow += "<tr><td align=\"right\" style=\"color:red;font-weight:bold;font-family:Calibri;font-size:18px;\">";
        //                strDeleteRow += "<input type=\"submit\" name=\"Delete\" value=\"Delete\"; onClick=\"DeletePost('" + quesID + "')\" style=\"color:red;font-weight:bold;font-family:Calibri;font-size:18px;border:solid;border-width:1px;border-color:black\">";
        //                strDeleteRow += "</td></tr>";
        //                Session["DeleteReplyId"] = strReplyId;
        //            }

        //            tblReplies += htrResponseNoByDetailsOuterRow + strDeleteRow + htmlRowSolutionContent;

        //            tblReplies += "<tr><td><br /></td></tr>";

        //        }
        //        tblReplies += "</table>";
        //        model.AnswerDetails = tblReplies;
        //        ViewBag.AnswerDetails = tblReplies;
        //    }
        //    else
        //    {
        //        ViewBag.AnswerDetails = null;
        //    }

        //    connManager.DisposeConn();

        //}







        //private void ProcessVotes(string LikeType, string ReplyId, string quesID)
        //{
        //    List<string> lstReplies = (List<string>)Session["Replies"];
        //    string strQuery = "";
        //    int votes = 0;
        //    if (lstReplies == null)
        //    {
        //        lstReplies = new List<string>();
        //    }

        //    if (!lstReplies.Contains(ReplyId))
        //    {

        //        ConnManager connManager = new ConnManager();
        //        connManager.OpenConnection();

        //        DataTable dsVotes = connManager.GetDataTable("Select ThumbsUp, ThumbsDown from Replies where ReplyId = " + ReplyId);

        //        if (dsVotes != null && dsVotes.Rows.Count > 0)
        //        {
        //            if (LikeType.Equals("UP"))
        //            {
        //                if (string.IsNullOrEmpty(dsVotes.Rows[0]["ThumbsUp"].ToString()))
        //                    votes = votes + 1;
        //                else
        //                    votes = int.Parse(dsVotes.Rows[0]["ThumbsUp"].ToString()) + 1;

        //                strQuery = "Update Replies set ThumbsUp = " + votes + " where ReplyId = " + ReplyId;
        //            }
        //            else
        //            {
        //                if (string.IsNullOrEmpty(dsVotes.Rows[0]["ThumbsDown"].ToString()))
        //                    votes = votes - 1;
        //                else
        //                    votes = int.Parse(dsVotes.Rows[0]["ThumbsDown"].ToString()) + 1;

        //                strQuery = "Update Replies set ThumbsDown = " + votes + " where ReplyId = " + ReplyId;
        //            }
        //        }

        //        SqlCommand command = new SqlCommand(strQuery, connManager.DataCon);
        //        command.CommandText = strQuery;
        //        command.ExecuteNonQuery();
        //        connManager.DisposeConn();

        //        lstReplies.Add(ReplyId);
        //        Session["Replies"] = lstReplies;
        //    }
        //    BindQuestionAskedUserData("Select * from VwQuestions where QuestionId = " + quesID.ToString() + "");
        //}
    }
}