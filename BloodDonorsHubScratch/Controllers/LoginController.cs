﻿using BloodDonorsHubScratch.Models;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace BloodDonorsHubScratch.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ActionName("Login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Login")]
        public ActionResult Login(LoginUserModel users)
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
                    //string[] name=users.Email.Split('@');
                    //TempData["Message"] = name[0].ToUpper().ToString();
                     string EmployeeName=Users.getNameByEmail(users.Email);
                     TempData["Message"] = EmployeeName;    
                    return RedirectToAction("UserLogin", "Account");
                }
            }
            ViewData["Error"] = "Login Failed, Please enter valid credentials";
            return View();
            
        }
    }
}
