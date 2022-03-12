﻿using MeetPlayPal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MeetPlayPal.Controllers
{
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private Users user = new Users();

        public ActionResult Login()
        {
            user = (Users)Session["User"];
            if (user!=null && !string.IsNullOrEmpty(user.Email))
            {
                GetUserRefType(user);
                return View("../Account/ViewUser", user);
            }
            return View();
        }

        public ActionResult QuickLogin()
        {
            user = (Users)Session["User"];
            if (user != null && !string.IsNullOrEmpty(user.Email))
            {
                GetUserRefType(user);
                return View("../Account/ViewUser", user);
            }
            return View();
        }

        public ActionResult ExtLogin()
        {
            return View();
        }


        public ActionResult ProcessLogin(string txtEMailId, string txtPassword)
        {
            if (Request.Form["hfUserEMail"] != null)
            {
                txtEMailId = Request.Form["hfUserEMail"];
                ConnManager con = new ConnManager();
                DataTable dtUserActivation = con.GetDataTable("select * from UserActivation where  Emailid = '" + txtEMailId + "'");
                if (dtUserActivation.Rows.Count > 0)
                {
                    SendActivationEMail(txtEMailId, dtUserActivation.Rows[0]["ActivationCode"].ToString());
                    ViewBag.Activation = "Activation Code Sent. Please check your inbox and click on the activation link.";
                    ViewBag.UserActEMail = txtEMailId;
                }
                return View("../Account/Login");

            }
            else
            {
                return CheckUserLogin(txtEMailId, txtPassword);
            }
        } 

        private ActionResult CheckUserLogin(string txtEMailId, string txtPassword)
        {
            ConnManager connManager = new ConnManager();
            DataTable DtUserList = new DataTable();
            DataTable dtUserActivation = new DataTable();

            dtUserActivation = connManager.GetDataTable("select * from UserActivation where Emailid = '" + txtEMailId + "'");
            if (dtUserActivation.Rows.Count > 0)
            {
                //ViewBag.Ack = "User activation pending";
                ViewBag.Activation = "User activation pending. Resend Activation Code?";
                ViewBag.UserActEMail = txtEMailId;
                return View("../Account/Login");
            }

           DtUserList = connManager.GetDataTable("select * from VwDomains where email = '" + txtEMailId + "' and Password = '" + txtPassword + "'");

            if (DtUserList.Rows.Count == 0)
            {
                ViewBag.Ack = "Invalid login credentials, please try again";
                return View("../Account/Login");
            }
            else
            {
                Users user = new Users();
                user.UserId = double.Parse(DtUserList.Rows[0]["UserId"].ToString());
                user.FirstName = DtUserList.Rows[0]["FirstName"].ToString();
                user.LastName = DtUserList.Rows[0]["LastName"].ToString();
                user.Email = DtUserList.Rows[0]["EMail"].ToString();
                user.IsPublisher = bool.Parse(DtUserList.Rows[0]["IsPublisher"].ToString());
                if (!string.IsNullOrEmpty(DtUserList.Rows[0]["IsOwner"].ToString()))
                {
                    user.IsOwner = bool.Parse(DtUserList.Rows[0]["IsOwner"].ToString());
                }

                //user.Details = DtUserList.Rows[0]["Details"].ToString();
                Session["User"] = user;
                
                HttpCookie userCookie = new HttpCookie("BooqmarqsLogin");
                userCookie.Values["EMail"] = user.Email;
                userCookie.Expires = System.DateTime.Now.AddDays(180);
                Response.Cookies.Add(userCookie);

                Utilities.LogMessage("I", "Login", user.Email);

                if (Request.UrlReferrer.ToString().ToLower().Contains("account/quicklogin"))
                {
                    //return View("../MeetPlayPal/ExtMeetPlayPals");
                    return RedirectToAction("ExtMeetPlayPals", "MeetPlayPal");
                }

                if (Session["MeetPlayPal"] != null && Request.Form["btnQuickLogin"] != null)
                {
                    AddMeetPlayPal(Session["MeetPlayPal"], user.UserId.ToString());
                    Session["MeetPlayPal"] = null;
                    return View("../MeetPlayPal/BMAdded");
                }
                else if (user.IsOwner)
                {
                    Session["SiteOwner"] = null;
                    ViewBag.IsOwner = true;
                    return RedirectToAction("Reports", "MeetPlayPal");
                }
                else
                {
                    return RedirectToAction("MyMeetPlayPals", "MeetPlayPal");
                }
            }
        }

        //private void AddMeetPlayPal(object bmrk, string userId)
        //{
        //    string url = string.Empty;
        //    if (bmrk != null)
        //    {
        //        url = bmrk.ToString();
        //        MeetPlayPalCls objBmrk = new MeetPlayPal.MeetPlayPalCls();
        //        string strCity = string.Empty;
        //        string strState = string.Empty;
        //        string strCountry = string.Empty;
        //        string ipAddr = Utilities.GetIPAddress();
        //        Utilities.GetLocation(ipAddr, ref strCity, ref strState, ref strCountry);

        //        objBmrk.URL = url;
        //        objBmrk.FolderId = 27;
        //        objBmrk.CreatedDate = DateTime.Now.ToString();
        //        objBmrk.CreatedUserId = userId;
        //        objBmrk.Name = url;
        //        objBmrk.IsFolder = false;
        //        objBmrk.IpAddr = ipAddr;
        //        objBmrk.City = strCity;
        //        objBmrk.Country = strCountry;
        //    }

        //}

        public ActionResult WebUserReg()
        {
            //ViewData["RegisType"] = "WebUser";
            return Register(null, null, null, null);
        }

        public ActionResult SiteOwnerReg()
        {
            Session["SiteOwner"] = "SiteOwner";
            return Register(null, null, null, null);
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            Session["User"] = null;
            Session["Facebook"] = null;
            Session["MeetPlayPal"] = null;
            Session.RemoveAll();
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            if (Request.Cookies["BooqmarqsLogin"] != null)
            {
                HttpCookie myCookie = Request.Cookies["BooqmarqsLogin"];
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                myCookie.Values["UserId"] = null;
                myCookie.Values["FirstName"] = null;
                myCookie.Values["LastName"] = null;
                Response.Cookies.Set(myCookie);
            }

            //Response.Redirect("../Home/Index");　
            return View("../Home/Index");
        }

        [AllowAnonymous]
        public ActionResult Choice()
        {
            user = (Users)Session["User"];
            if(user!=null && !string.IsNullOrEmpty(user.Email))
            {
                GetUserRefType(user);
                return View("../Account/ViewUser", user);
            }
            return View();
        }

        //[ReCaptcha]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Register(string txtFirstName, string txtLastName, string txtEMailId, string txtNewPassword)
        {
            string activationCode = Guid.NewGuid().ToString();
            //AddEdit user
            if (!string.IsNullOrEmpty(txtEMailId))
            {
                var response = Request["g-recaptcha-response"];
                string secretKey = ConfigurationManager.AppSettings["ReCaptcha.PrivateKey"];
                var client = new WebClient();
                var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
                var obj = JObject.Parse(result);
                var status = (bool)obj.SelectToken("success");
                //ViewBag.Message = status ? "Google reCaptcha validation success" : "Google reCaptcha validation failed";

                if (status)
                {

                    if(!IsActivated(txtEMailId))
                    {
                        return View();
                    }

                    if (!IsNewUser(txtEMailId))
                    {
                        ViewBag.Ack = "EMail id already exists. If you have forgotten password, please click forgot password link on the Sign In page.";
                        return View();
                    }

                    double dblUserID = 0;
                    user.OptionID = 1;
                    user.CreatedDateTime = DateTime.Now;
                    user.Password = txtNewPassword;
                    user.Email = txtEMailId;
                    user.FirstName = txtFirstName;
                    user.LastName = txtLastName;
                    //user.IsWebUser = true;
                    //if (Session["SiteOwner"] != null)
                    //    user.IsPublisher = true;
                    //else
                    //    user.IsPublisher = false;
                    user.CreateUsers(ref dblUserID);
                    user.CreateUserActivation(user, activationCode, dblUserID);
                    ViewBag.Ack = "User Info Saved Successfully. An activation link has been sent to your email address, please check your inbox and activate your account";
                    SendActivationEMail(user.Email, activationCode);
                    SendEMail(txtEMailId, txtFirstName, txtLastName);
                    user = new Users();
                    Session["SiteOwner"] = null;
                    return View();
                }
                else
                {
                    ViewBag.Ack = "reCaptcha validation failed";
                    return View();
                }
            }
            else
            {
                return View("../Account/Register");
            }
        }

        public ActionResult ViewUser(Users user)
        {
            string email = string.Empty;
            if (Session["User"] != null)
            {
                user = (Users)Session["User"];
                email = user.Email;

                GetUserRefType(user);
                return View("../Account/ViewUser", user);
            }
            else
            {
                return View("../Account/Login");
            }
        }

        private void GetUserRefType(Users user)
        {
            if (string.IsNullOrEmpty(user.GetPassword(user.Email)))
            {
                ViewBag.RefType = "Social";
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult EditUser(Users user, string chkIsPublisher)
        {
            double dblUserID = 0;
            Users tempUser = new Users();
            ViewData["chkIsPublisher"] = false;
                
            tempUser = (Users)Session["User"];
            //if (Request.Form["Edit"] != null)
            //{
            //    ViewData["chkIsPublisher"] = tempUser.IsPublisher;
            //    return View("../Account/EditUser", tempUser);
            //}
            //else if (Request.Form["ChangePassword"] != null)
            //{
            //    ViewData["chkIsPublisher"] = tempUser.IsPublisher;
            //    return View("../Account/ChangePassword", tempUser);
            //}

            if (Request.Form["EditSubmit"] != null)
            {
                if (ModelState.IsValid)
                {
                    ConnManager con = new ConnManager();
                    DataTable dtUser = con.GetDataTable("Select * from Users where Email = '" + tempUser.Email + "'");
                    if (dtUser.Rows.Count > 0)
                    {
                        user.UserId = double.Parse(dtUser.Rows[0]["UserId"].ToString());
                        user.OptionID = 2;
                        user.FirstName = user.FirstName;
                        user.LastName = user.LastName;
                        //user.Email = user.Email;
                       // user.IsPublisher = bool.Parse(chkIsPublisher);
                        user.ModifiedDateTime = DateTime.Now;
                        dblUserID = user.UserId;
                        user.CreateUsers(ref dblUserID);
                        if (bool.Parse(chkIsPublisher))
                            ViewData["chkIsPublisher"] = true;
                        else
                            ViewData["chkIsPublisher"] = false;
                        ViewBag.Ack = "User Updated Successfully.";
                        Session["User"] = user;
                    }
                    return RedirectToAction("ViewUser", "Account");     //return Redirect("../Account/ViewUser");
                }
                else
                {
                    user = (Users)Session["User"];
                    return RedirectToAction("ViewUser", "Account");     //return View("../Account/ViewUser", user);
                }
            }
            //else if (Request.Form["UpdateAsSiteOwner"] != null)
            //{
            //    return RedirectToAction("ScriptCode", "MeetPlayPal");               
            //}
            else
            {
                user = (Users)Session["User"];
                GetUserRefType(user);
                return View("../Account/ViewUser", user);
            }
        }

        public ActionResult ChangePassword(string txtPassword, string txtNewPassword)
        {
            if (Request.Form["Cancel"] == null)
            {
                if (Session["User"] != null && !string.IsNullOrEmpty(txtPassword))
                {
                    user = (Users)Session["User"];

                    if (user.GetPassword(user.Email) == txtPassword)
                    {
                        double dblUserID = 0;
                        user.OptionID = 5;
                        user.Password = txtNewPassword;
                        user.ModifiedDateTime = DateTime.Now;
                        dblUserID = user.UserId;
                        user.CreateUsers(ref dblUserID);
                        ViewBag.Ack = "Password changed successfully";
                    }
                    else
                    {
                        ViewBag.Ack = "Current password does not match with existing password";
                    }
                }
                return View(user);
            }
            else
            {
                if (Session["User"] != null)
                {
                    user = (Users)Session["User"];
                    ViewBag.UserEMail = user.Email;
                }
                GetUserRefType(user);
                return View("../Account/ViewUser", user);
            }
        }

        public ActionResult ForgotPassword(string txtEMailId)
        {
            user = (Users)Session["User"];
            if (user!=null && !string.IsNullOrEmpty(user.Email))
            {
                GetUserRefType(user);
                return View("../Account/ViewUser", user);
            }

            if (!string.IsNullOrEmpty(txtEMailId))
            {
                ConnManager con = new ConnManager();

                DataTable dtUserActivation = con.GetDataTable("select * from UserActivation where EmailId = '" + txtEMailId + "'");
                if (dtUserActivation.Rows.Count > 0)
                {
                    //ViewBag.Ack = "User activation pending";
                    ViewBag.Activation = "User activation pending. Resend Activation Code?";
                    ViewBag.UserActEMail = txtEMailId;
                    return View();
                }

                DataTable dtUser = con.GetDataTable("Select * from Users where Email = '" + txtEMailId + "'");
                if (dtUser.Rows.Count <= 0)
                {
                    ViewBag.Ack = "No such email exists";
                }

                else if (!string.IsNullOrEmpty(dtUser.Rows[0]["Password"].ToString()))
                {
                    Mail mail = new Mail();
                    mail.IsBodyHtml = true;
                    string EMailBody = System.IO.File.ReadAllText(Server.MapPath("../EMailBody.txt"));
                    mail.Body = string.Format(EMailBody, "Forgot Password", "Your Booqmarqs account password is " + dtUser.Rows[0]["Password"].ToString());
                    mail.FromAdd = "admin@booqmarqs.com";
                    mail.Subject = "Booqmarqs account password";
                    mail.ToAdd = dtUser.Rows[0]["EMail"].ToString();
                    mail.SendMail();

                    ViewBag.Ack = "Password has been emailed to you, please check your inbox.";
                }
                else
                {

                    ViewBag.Ack = "You might have created your profile by using one of the popular social logins. Please use the same channel to login.";
                }
            }
            return View();
        }

        public ActionResult Activate()
        {
            string ActivationCode = Request.QueryString["ActivationCode"];
            Users usr = new global::MeetPlayPal.Users();
            ViewBag.Ack = usr.ActivateUser(ActivationCode);
            return View("../Account/Login");
        }

        public ActionResult ProcessActivationCode()
        {
            string txtEMailId = Request.Form["hfUserEMail"];
            ConnManager con = new ConnManager();
            DataTable dtUserActivation = con.GetDataTable("select * from UserActivation where  Emailid = '" + txtEMailId + "'");
            if (dtUserActivation.Rows.Count > 0)
            {
                SendActivationEMail(txtEMailId, dtUserActivation.Rows[0]["ActivationCode"].ToString());
                ViewBag.Activation = "Activation Code Sent. Please check your inbox and click on the activation link.";
                ViewBag.UserActEMail = txtEMailId;
            }
           return View("../Account/Login");
        }

        private void SendEMail(string Email_address, string firstName, string LastName)
        {
            try
            {
                Mail mail = new Mail();
                string strBody = string.Empty;
                strBody = "new user email id " + Email_address + " name " + firstName;

                try
                {
                    strBody += "<br /><br /> IP - " + Utilities.GetUserIP() + "<br /><br />";
                }
                catch
                {

                }

                mail.Body = strBody;
                mail.FromAdd = "admin@booqmarqs.com";
                mail.Subject = "New User registered";

                mail.ToAdd = "admin@booqmarqs.com";
                mail.IsBodyHtml = true;
                mail.SendMail();
            }
            catch
            {


            }
        }

        private void SendNewUserRegEMail(string email)
        {
            try
            {
                Mail mail = new Mail();
                string EMailBody = System.IO.File.ReadAllText(Server.MapPath("../EMailBody.txt"));
                string strCA = "<a id=HyperLink1 style=font-size: medium; font-weight: bold; color:White href=http://Booqmarqs.com>Booqmarqs</a>";
                mail.Body = string.Format(EMailBody, "New User Register", "Welcome to " + strCA + ". Start using the new way of Booqmarqs and feel the difference.");
                mail.FromAdd = "admin@booqmarqs.com";
                mail.Subject = "Welcome to Booqmarqs";
                mail.ToAdd = email;
                mail.IsBodyHtml = true;
                mail.SendMail();
            }
            catch
            {


            }
        }

        private void SendActivationEMail(string email, string ActivationCode)
        {
            try
            {
                Mail mail = new Mail();
                string EMailBody = System.IO.File.ReadAllText(Server.MapPath("../EMailBody.txt"));
                string strActLink = "http://booqmarqs.com/Account/Activate/?ActivationCode=" + ActivationCode;
                string strCA = "<a id=HyperLink1 style=font-size: medium; font-weight: bold; color:White href=http://booqmarqs.com>Booqmarqs</a>";
                mail.Body = string.Format(EMailBody, "User Activation", "Welcome to " + strCA + ". Activate your account and start using the new way of Booqmarqs and feel the difference.<br/> <br/>Please click <a id=actHere href=http://booqmarqs.com/Account/Activate/?ActivationCode=" + ActivationCode + ">" + strActLink + "</a> to activate your account");
                mail.FromAdd = "admin@booqmarqs.com";
                mail.Subject = "Welcome to Booqmarqs";
                mail.ToAdd = email;
                mail.IsBodyHtml = true;
                mail.SendMail();
            }
            catch
            {


            }
        }


        /// <summary>
        /// Social Login Section
        /// </summary>

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {            
            if(Request.Form.Keys.Count > 0)
            {
                if(Request.Form.Keys[1].Contains("Facebook"))
                {
                    provider = "Facebook";
                }
                else if (Request.Form.Keys[1].Contains("Google"))
                {
                    provider = "Google";

                }
                else if (Request.Form.Keys[1].Contains("Twitter"))
                {
                    provider = "Twitter";
                }
            }
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            bool _isNewUser = false;
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                Users user1 = new Users();

                if (loginInfo.Login.LoginProvider.ToLower() == "twitter")
                {
                    string access_token = loginInfo.ExternalIdentity.Claims.Where(x => x.Type == "urn:twitter:access_token").Select(x => x.Value).FirstOrDefault();
                    string access_secret = loginInfo.ExternalIdentity.Claims.Where(x => x.Type == "urn:twitter:access_secret").Select(x => x.Value).FirstOrDefault();
                    TwitterDto response = TwitterLogin(access_token, access_secret, ConfigurationManager.AppSettings["twitter_consumer_key"], ConfigurationManager.AppSettings["twitter_consumer_secret"]);
                    user1.Email = response.email;
                    user1.FirstName = response.name;
                    Utilities.LogMessage("I", "ExternalLoginCallback - Twitter", user1.Email);
                }
                else
                {
                    ClaimsIdentity claimsIdentities = loginInfo.ExternalIdentity;
                    if (loginInfo.Login.LoginProvider.ToLower() == "facebook")
                    {
                        user1.FirstName = (from c in claimsIdentities.Claims
                                           where c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
                                           select c.Value).Single();
                        user1.Email = (from c in claimsIdentities.Claims
                                       where c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
                                       select c.Value).Single();
                        Utilities.LogMessage("I", "ExternalLoginCallback - Facebook", user1.Email);
                    }
                    else if (loginInfo.Login.LoginProvider.ToLower() == "google")
                    {
                        user1.FirstName = (from c in claimsIdentities.Claims
                                           where c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"
                                           select c.Value).Single();
                        user1.LastName = (from c in claimsIdentities.Claims
                                          where c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname"
                                          select c.Value).Single();
                        user1.Email = (from c in claimsIdentities.Claims
                                       where c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
                                       select c.Value).Single();
                        Utilities.LogMessage("I", "ExternalLoginCallback - Google", user1.Email);
                    }

                }


                if (!IsActivated(user1.Email))
                {
                    return View("../Account/Login");
                }


                if (IsNewUser(ref user1))
                {
                    _isNewUser = true;
                    if (Session["SiteOwner"] != null && Session["SiteOwner"].ToString() == "SiteOwner")
                    {
                        user1.IsPublisher = true;
                    }
                    RegisterUser(ref user1);
                }


                Session["User"] = user1;
                HttpCookie mycookie = new HttpCookie("BooqmarqsLogin");
                mycookie.Values["EMail"] = user1.Email;
                mycookie.Expires = System.DateTime.Now.AddDays(180);
                Response.Cookies.Add(mycookie);

                Utilities.LogMessage("I", "ExternalLogin", user1.Email);


                if (Session["MeetPlayPal"] != null && Request.Form["btnQuickLogin"] != null)
                {
                    AddMeetPlayPal(Session["MeetPlayPal"], user1.UserId.ToString());
                    Session["MeetPlayPal"] = null;
                    return View("../MeetPlayPal/BMAdded");
                }
                if (_isNewUser)
                {
                    if (Session["SiteOwner"] != null && Session["SiteOwner"].ToString() == "SiteOwner")
                    {

                        return RedirectToAction("ScriptCode", "MeetPlayPal");
                    }
                    else
                    {
                        MeetPlayPalCls bmrk = new MeetPlayPalCls();
                        if (bmrk.GetMeetPlayPalsCountForUser(user1.UserId) > 0)
                        {
                            return RedirectToAction("MyMeetPlayPals", "MeetPlayPal");
                        }
                        else
                        {
                            return RedirectToAction("Import", "MeetPlayPal");
                        }
                    }
                }
                else
                {
                    if (user1.IsOwner)
                    {
                        ViewBag.IsOwner = true;
                        return RedirectToAction("Reports", "MeetPlayPal");
                    }
                    else
                    {
                        MeetPlayPalCls bmrk = new MeetPlayPalCls();
                        if (bmrk.GetMeetPlayPalsCountForUser(user1.UserId) > 0)
                        {
                            return RedirectToAction("MyMeetPlayPals", "MeetPlayPal");
                        }
                        else
                        {
                            return RedirectToAction("Import", "MeetPlayPal");
                        }
                    }
                }
            }
        }

        public ActionResult FacebookWelcome()
        {
            string returnUrl = string.Empty;
            return new ChallengeResult("Facebook", Url.Action("FacebookCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> FacebookCallback(string returnUrl)
        {
            bool _isNewUser = false;
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                if (Request.Browser.Type.ToLower().Contains("chrome"))
                {
                    return Redirect("https://chrome.google.com/webstore/detail/booqmarqs/nabhjndfpicfhnminejhekphlfdaojla");
                }
                else if (Request.Browser.Type.ToLower().Contains("firefox"))
                {
                    return Redirect("https://addons.mozilla.org/en-US/firefox/addon/booqmarqs/");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            else
            {
                Users user1 = new Users();
              
                ClaimsIdentity claimsIdentities = loginInfo.ExternalIdentity;
                    user1.FirstName = (from c in claimsIdentities.Claims
                                        where c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
                                        select c.Value).Single();
                    user1.Email = (from c in claimsIdentities.Claims
                                    where c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
                                    select c.Value).Single();


                if (IsNewUser(ref user1))
                {
                    _isNewUser = true;
                    if (Session["SiteOwner"] != null && Session["SiteOwner"].ToString() == "SiteOwner")
                    {
                        user1.IsPublisher = true;
                    }
                    RegisterUser(ref user1);
                }


                Session["User"] = user1;
                HttpCookie mycookie = new HttpCookie("BooqmarqsLogin");
                mycookie.Values["EMail"] = user1.Email;
                mycookie.Expires = System.DateTime.Now.AddDays(180);
                Response.Cookies.Add(mycookie);
   
                if (_isNewUser)
                {  
                    if(Request.Browser.Type.ToLower().Contains("chrome"))
                    {                        
                        return Redirect("https://chrome.google.com/webstore/detail/booqmarqs/nabhjndfpicfhnminejhekphlfdaojla");
                    }
                    else if (Request.Browser.Type.ToLower().Contains("firefox"))
                    {
                        return Redirect("https://addons.mozilla.org/en-US/firefox/addon/booqmarqs/");
                    }
                    else
                    {
                        return RedirectToAction("Login");
                    }
                }
                else
                {                   
                    MeetPlayPalCls bmrk = new MeetPlayPalCls();
                    if (bmrk.GetMeetPlayPalsCountForUser(user1.UserId) > 0)
                    {
                        return RedirectToAction("MyMeetPlayPals", "MeetPlayPal");
                    }
                    else
                    {
                        return RedirectToAction("Import", "MeetPlayPal");
                    }                    
                }
            }
            return null;
        }

        public ActionResult GoogleWelcome()
        {
            string returnUrl = string.Empty;
            return new ChallengeResult("Google", Url.Action("GoogleCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> GoogleCallback(string returnUrl)
        {
            bool _isNewUser = false;
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                if (Request.Browser.Type.ToLower().Contains("chrome"))
                {
                    return Redirect("https://chrome.google.com/webstore/detail/booqmarqs/nabhjndfpicfhnminejhekphlfdaojla");
                }
                else if (Request.Browser.Type.ToLower().Contains("firefox"))
                {
                    return Redirect("https://addons.mozilla.org/en-US/firefox/addon/booqmarqs/");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            else
            {
                Users user1 = new Users();

                ClaimsIdentity claimsIdentities = loginInfo.ExternalIdentity;

                    user1.FirstName = (from c in claimsIdentities.Claims
                                       where c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"
                                       select c.Value).Single();
                    user1.LastName = (from c in claimsIdentities.Claims
                                      where c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname"
                                      select c.Value).Single();
                    user1.Email = (from c in claimsIdentities.Claims
                                   where c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
                                   select c.Value).Single();


                if (IsNewUser(ref user1))
                {
                    _isNewUser = true;
                    if (Session["SiteOwner"] != null && Session["SiteOwner"].ToString() == "SiteOwner")
                    {
                        user1.IsPublisher = true;
                    }
                    RegisterUser(ref user1);
                }


                Session["User"] = user1;
                HttpCookie mycookie = new HttpCookie("BooqmarqsLogin");
                mycookie.Values["EMail"] = user1.Email;
                mycookie.Expires = System.DateTime.Now.AddDays(180);
                Response.Cookies.Add(mycookie);

                if (_isNewUser)
                {
                    if (Request.Browser.Type.ToLower().Contains("chrome"))
                    {
                        return Redirect("https://chrome.google.com/webstore/detail/booqmarqs/nabhjndfpicfhnminejhekphlfdaojla");
                    }
                    else if (Request.Browser.Type.ToLower().Contains("firefox"))
                    {
                        return Redirect("https://addons.mozilla.org/en-US/firefox/addon/booqmarqs/");
                    }
                    else
                    {
                        return RedirectToAction("Login");
                    }
                }
                else
                {
                    MeetPlayPalCls bmrk = new MeetPlayPalCls();
                    if (bmrk.GetMeetPlayPalsCountForUser(user1.UserId) > 0)
                    {
                        return RedirectToAction("MyMeetPlayPals", "MeetPlayPal");
                    }
                    else
                    {
                        return RedirectToAction("Import", "MeetPlayPal");
                    }
                }
            }
            return null;
        }


        private bool IsNewUser(string email)
        {
            bool userExists = false;
            DataTable dtUser = new DataTable();
            ConnManager con = new ConnManager();
            dtUser = con.GetDataTable("Select * from Users where Email = '" + email + "'");

            if (dtUser.Rows.Count > 0)
            {
                userExists = false;
            }
            else
            {
                userExists = true;
            }
            return userExists;
        }


        private bool IsNewUser(ref Users user1)
        {
            bool userExists = false;
            DataTable dtUser = new DataTable();
            ConnManager con = new ConnManager();            
            dtUser = con.GetDataTable("Select * from VwDomains where Email = '" + user1.Email + "'");           

            if (dtUser.Rows.Count > 0)
            {
                userExists = false;
                user1.UserId = double.Parse(dtUser.Rows[0]["UserId"].ToString());
                user1.FirstName = dtUser.Rows[0]["FirstName"].ToString();
                user1.LastName = dtUser.Rows[0]["LastName"].ToString();
                user1.Email = dtUser.Rows[0]["EMail"].ToString();
              //  user1.IsPublisher = bool.Parse(dtUser.Rows[0]["IsPublisher"].ToString());
                //if (!string.IsNullOrEmpty(dtUser.Rows[0]["IsOwner"].ToString()))
                //{
                //    user1.IsOwner = bool.Parse(dtUser.Rows[0]["IsOwner"].ToString());
                //}
            }
            else
            {
                userExists = true;
            }
            return userExists;
        }


        private void RegisterUser(ref Users user1)
        {
            user1 = user1.CreateUser(user1.Email, user1.FirstName, user1.LastName, user1.IsPublisher);
            SendEMail(user1.Email, user1.FirstName, user1.LastName);
        }

        private bool IsActivated(string email)
        {
            ConnManager con = new ConnManager();
            DataTable dtUserActivation = con.GetDataTable("select * from UserActivation where  Emailid = '" + email + "'");
            if (dtUserActivation.Rows.Count > 0)
            {
                //ViewBag.Ack = "User activation pending";
                ViewBag.Activation = "User activation pending. Resend Activation Code?";
                ViewBag.UserActEMail = email;
                return false;
            }
            else
                return true;
        }


        public static TwitterDto TwitterLogin(string oauth_token, string oauth_token_secret, string oauth_consumer_key, string oauth_consumer_secret)
        {
            // oauth implementation details
            var oauth_version = "1.0";
            var oauth_signature_method = "HMAC-SHA1";

            // unique request details
            var oauth_nonce = Convert.ToBase64String(
                new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            var timeSpan = DateTime.UtcNow
                - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

            var resource_url = "https://api.twitter.com/1.1/account/verify_credentials.json";
            var request_query = "include_email=true";
            // create oauth signature
            var baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                            "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}";

            var baseString = string.Format(baseFormat,
                                        oauth_consumer_key,
                                        oauth_nonce,
                                        oauth_signature_method,
                                        oauth_timestamp,
                                        oauth_token,
                                        oauth_version
                                        );

            baseString = string.Concat("GET&", Uri.EscapeDataString(resource_url) + "&" + Uri.EscapeDataString(request_query), "%26", Uri.EscapeDataString(baseString));

            var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
                                    "&", Uri.EscapeDataString(oauth_token_secret));

            string oauth_signature;
            using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
            {
                oauth_signature = Convert.ToBase64String(
                    hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            }

            // create the request header
            var headerFormat = "OAuth oauth_consumer_key=\"{0}\", oauth_nonce=\"{1}\", oauth_signature=\"{2}\", oauth_signature_method=\"{3}\", oauth_timestamp=\"{4}\", oauth_token=\"{5}\", oauth_version=\"{6}\"";

            var authHeader = string.Format(headerFormat,
                                    Uri.EscapeDataString(oauth_consumer_key),
                                    Uri.EscapeDataString(oauth_nonce),
                                    Uri.EscapeDataString(oauth_signature),
                                    Uri.EscapeDataString(oauth_signature_method),
                                    Uri.EscapeDataString(oauth_timestamp),
                                    Uri.EscapeDataString(oauth_token),
                                    Uri.EscapeDataString(oauth_version)
                            );


            // make the request

            ServicePointManager.Expect100Continue = false;
            resource_url += "?include_email=true";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resource_url);
            request.Headers.Add("Authorization", authHeader);
            request.Method = "GET";

            WebResponse response = request.GetResponse();
            return JsonConvert.DeserializeObject<TwitterDto>(new StreamReader(response.GetResponseStream()).ReadToEnd());
        }
    

    public class TwitterDto
    {
        public string name { get; set; }
        public string email { get; set; }
    }

    #region Helpers


    // Sign in the user with this external login provider if the user already has a login
    //var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
    //switch (result)
    //{
    //    case SignInStatus.Success:
    //        return RedirectToLocal(returnUrl);
    //    //case SignInStatus.LockedOut:
    //        //return View("Lockout");
    //    //case SignInStatus.RequiresVerification:
    //        //return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
    //    case SignInStatus.Failure:
    //    default:
    //        // If the user does not have an account, then prompt the user to create an account
    //        ViewBag.ReturnUrl = returnUrl;
    //        ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
    //        return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
    //}


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult LogOff()
        //{
        //    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        //    return RedirectToAction("Index", "Home");
        //}


        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }




        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }



}
