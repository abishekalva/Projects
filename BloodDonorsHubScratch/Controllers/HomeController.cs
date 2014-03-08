using BloodDonorsHubScratch.Models;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace BloodDonorsHubScratch.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        [HttpGet]
        [ActionName("Index")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult Index(LoginUserModel users)
        {

            //Check if Admin Credentials,validate & redirect to admin page
            string username = WebConfigurationManager.AppSettings["AdminEmail"].ToString();
            string password = WebConfigurationManager.AppSettings["AdminPassword"].ToString();
            if (username == users.Email && password == users.Password)
            {

                TempData["Message"] = "Admin";
                return RedirectToAction("AdminLogin", "Account");
            }
            else
            {
                //If User Credentials, validate & redirect to users page
                bool isValidLogin = Users.ValidateLoginUser(users.Email, users.Password);

                if (isValidLogin)
                {
                    string[] name = users.Email.Split('@');
                    TempData["Message"] = name[0].ToUpper().ToString();
                    return RedirectToAction("UserLogin", "Account");
                }
            }
            ViewData["message_color"] = "red";
            ViewData["Error"] = "Login Failed, Please enter valid credentials";
            return View();

        }
    }
}
