using System.Diagnostics;
using ClosedXML.Excel;
using Jaiz_CSAT_Survey.Models;
using Jaiz_CSAT_Survey.Services;
using Jaiz_CSAT_Survey.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Jaiz_CSAT_Survey.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Helper util = new Helper();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var userName = util.DecryptTextWithPrivateKey(model.Username);
            var userPassword = util.DecryptTextWithPrivateKey(model.Password);


            ////FOR VAPT TESTER
            //if (userName.ToLower() == "tester" && userPassword.ToLower() == "tester1@csat") {

            //    HttpContext.Session.SetString("user", "User");

            //    return RedirectToAction("Index", "Home");
            //}

            var isValidationSuccessful = ValidateUser(userName, userPassword);

            if (isValidationSuccessful)
            {
                return RedirectToAction("Index", "Home");
            }
            else {

                ModelState.AddModelError("InvalidUsernameOrPassword", "The user name or password provided is incorrect.");
            }

            return View(model);

        }

        public IActionResult Index()
        {
            if (!SetSessionData())
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        private bool SetSessionData()
        {
            try
            {
                var user = HttpContext.Session.GetString("user");
                if (user == null)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        private bool ValidateUser(string username, string password)
        {
            var userValidation = new JaizAuthService.JaizRoleManagerServiceClient(0);
            var logModel = new JaizAuthService.LogonModel()
            {
                username = username,
                password = password,
                appID = 72,
            };

            var result = new JaizAuthService.LoginResult();

            try
            {
                result = userValidation.ValidateADUser2FA(logModel);

                if (result.loggedIn)
                {
                    HttpContext.Session.SetString("user", "User");

                    return true;
                   
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
            }
            return result.loggedIn;
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("user");
            return RedirectToAction("Login", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
