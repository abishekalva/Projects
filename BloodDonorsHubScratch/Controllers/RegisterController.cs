using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;


namespace BloodDonorsHubScratch.Controllers
{
    public class RegisterController : Controller
    {
        //
        // GET: /Registration/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ActionName("Registration")]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Registration")]
        public ActionResult Registration(Users users)
        {

            bool isSuccessful = Users.Register(users);
            if (!isSuccessful)
            {
                ViewData["Error"] = "Registration unsuccessful. This record already exists in our database";
            }
            else
            {
                ViewData["Success"] = "You have been successfully registered. An email has been sent to your mail id with your login credentials.";
            }
            return View();
        }

    }
}
