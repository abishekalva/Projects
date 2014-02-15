using BloodDonorsHubScratch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;

namespace BloodDonorsHubScratch.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ActionName("AdminLogin")]
        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        [ActionName("AdminLogin")]
        public ActionResult AdminLogin(RequestModel model)
        {
            return View();
        }

        [HttpGet]
        [ActionName("UserLogin")]
        public ActionResult UserLogin()
        {
            return View();
        }
       
        [HttpPost]
        [ActionName("UserLogin")]
        public ActionResult UserLogin(RequestModel model)
        {
           UserActions.PostRequest(model.EmployeeID, model.BloodGroupRequested, model.City);
           
              return View();
        }

    }
}
