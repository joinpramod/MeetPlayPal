using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MeetPlayPal.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                if (filterContext.HttpContext.Session["User"] != null)
                {
                    Users user = (Users)filterContext.HttpContext.Session["User"];

                    ViewBag.lblFirstName = user.FirstName;
                    ViewBag.IsUserLoggedIn = true;
                    //ViewBag.IsOwner = user.IsOwner;
                    RedirectToProfile(filterContext);
                }
                else if (Request.Cookies["MeetPlayPalLogin"] != null && Request.Cookies["MeetPlayPalLogin"].HasKeys)
                {
                    string uname = Request.Cookies["MeetPlayPalLogin"].Values["EMail"];
                    Users user1 = new Users();
                    user1 = user1.GetUser(uname);
                    Session["User"] = user1;
                    ViewBag.lblFirstName = user1.FirstName;

                }
                else
                {
                    if (Request.Url.AbsoluteUri.ToLower().Contains("account/changepassword")
                        || Request.Url.AbsoluteUri.ToLower().Contains("account/edituser")
                        || Request.Url.AbsoluteUri.ToLower().Contains("account/viewuser"))
                    {
                        //filterContext.Result = new RedirectResult("Account/Login");
                        filterContext.Result = new RedirectToRouteResult(
                                                   new RouteValueDictionary
                                                       {{"controller", "Account"}, {"action", "Login"}});
                        return;

                    }

                    //if (Request.Url.AbsoluteUri.ToLower().Contains("bookmark/extbookmarks"))
                    //{
                    //    filterContext.Result = new RedirectToRouteResult(
                    //                               new RouteValueDictionary
                    //                                   {{"controller", "Account"}, {"action", "ExtLogin"}});
                    //    return;

                    //}
                }
            }
            catch (Exception ex)
            {

                Session["User"] = null;
                Session["Facebook"] = null;
                Session["bookmark"] = null;
                Session.RemoveAll();

                if (Request.Cookies["MeetPlayPalLogin"] != null)
                {
                    HttpCookie myCookie = Request.Cookies["MeetPlayPalLogin"];
                    myCookie.Expires = DateTime.Now.AddDays(-1d);
                    myCookie.Values["UserId"] = null;
                    myCookie.Values["FirstName"] = null;
                    myCookie.Values["LastName"] = null;
                    Response.Cookies.Set(myCookie);
                }


                filterContext.Result = new RedirectToRouteResult(
                                                   new RouteValueDictionary
                                                       {{"controller", "Home"}, {"action", "Index"}});
                return;
            }
        }

        private void RedirectToProfile(ActionExecutingContext filterContext)
        {
            if (Request.Url.AbsoluteUri.Contains("account/register")
                   || Request.Url.AbsoluteUri.ToLower().Contains("account/login")
                   || Request.Url.AbsoluteUri.ToLower().Contains("account/register"))
                {

                filterContext.Result = new RedirectToRouteResult(
                                              new RouteValueDictionary
                                                  {{"controller", "Account"}, {"action", "ViewUser"}});
            }
        }
    }
}