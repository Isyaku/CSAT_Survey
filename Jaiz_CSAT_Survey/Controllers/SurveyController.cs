using Jaiz_CSAT_Survey.Data;
using Jaiz_CSAT_Survey.Models;
using Jaiz_CSAT_Survey.Services;
using Jaiz_CSAT_Survey.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Jaiz_CSAT_Survey.Controllers
{
    public class SurveyController : Controller
    {
        private readonly AppDbContext _context;

        public SurveyController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> QRImage(string channel, string UserID, string TraxID, string Rating, string Email, string Account, string Phone)
        {
            //string channel = "DivFOkaVQ7VK3g6r7J0Yig==", string UserID = "", string TraxID = "4Oe14I13JYjgtdxjsPRiEg==", string Rating = "", string Email = "", string Account = "", string Phone = ""

            try
            {
                channel = channel?.Replace(' ', '+');
                UserID = UserID?.Replace(' ', '+');
                TraxID = TraxID?.Replace(' ', '+');
                Rating = Rating?.Replace(' ', '+');
                Email = Email?.Replace(' ', '+');
                Account = Account?.Replace(' ', '+');
                Phone = Phone?.Replace(' ', '+');
                channel = channel?.Replace('@', '+');
                UserID = UserID?.Replace('@', '+');
                TraxID = TraxID?.Replace('@', '+');
                Rating = Rating?.Replace('@', '+');
                Email = Email?.Replace('@', '+');
                Account = Account?.Replace('@', '+');
                Phone = Phone?.Replace('@', '+');
            }
            catch (Exception)
            {
            }

            string decryptedChannel = "";
            string decryptedUserID = "";
            string decryptedTraxID = "";
            string decryptedRating = "";
            string decryptedEmail = "";
            string decryptedAccount = "";
            string decryptedPhone = "";


            decryptedChannel = Crypto.DecryptText(channel);

            if (decryptedChannel.Contains("Invalid encryption"))
            {
                decryptedChannel = "";
            }

            decryptedUserID = Crypto.DecryptText(UserID);

            if (decryptedUserID.Contains("Invalid encryption"))
            {
                decryptedUserID = "";
            }

            decryptedTraxID = Crypto.DecryptText(TraxID);

            if (decryptedTraxID.Contains("Invalid encryption"))
            {
                decryptedTraxID = "";
            }

            decryptedRating = Crypto.DecryptText(Rating);

            if (decryptedRating.Contains("Invalid encryption"))
            {
                decryptedRating = "";
            }

            decryptedEmail = Crypto.DecryptText(Email);

            if (decryptedEmail.Contains("Invalid encryption"))
            {
                decryptedEmail = "";
            }

            decryptedAccount = Crypto.DecryptText(Account);

            if (decryptedAccount.Contains("Invalid encryption"))
            {
                decryptedAccount = "";
            }

            decryptedPhone = Crypto.DecryptText(Phone);

            if (decryptedPhone.Contains("Invalid encryption"))
            {
                decryptedPhone = "";
            }

            if (string.IsNullOrEmpty(decryptedTraxID))
            {
                Random rs = new Random();
                decryptedTraxID = rs.Next(100000001, 999999999).ToString();
            }

            var recommendRatings = Enumerable.Range(0, 11).Select(x => new { Value = x, Text = x <= 6 ? "Detractor" : x <= 8 ? "Passive" : "Promoter" });

            ViewBag.Branch = decryptedChannel;
            ViewBag.RecommendRatings = recommendRatings;
            ViewBag.SurveyType = "BranchQR";
            return View("BranchQR");
        }


        [HttpGet]
        public async Task<IActionResult> Index(string channel, string UserID, string TraxID, string Rating, string Email, string Account, string Phone)
        {
            //string channel = "rI4QVtzH1J4=", string UserID = "YarUP2QSHuc=", string TraxID = "xJUWHALi9CQmZhZ3Aq2KfA==", string Rating = "Ad1NSrYD06k=", string Email = "mcecu6n34WSdZ/yqcxD6eo3wJfDYJ5Wd&Account=J9xlTH/we8cFG9xaYz0tLQ==", string Account = "2rpVeHDcFdP2LmxplS1HegVv0NP9oPl4", string Phone = "KpvYYTtZVdMXSLjRn/boNA=="

            if (
                string.IsNullOrEmpty(channel) && string.IsNullOrEmpty(UserID) && string.IsNullOrEmpty(TraxID) &&
                string.IsNullOrEmpty(Rating) && string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Account) &&
                string.IsNullOrEmpty(Phone)
                )
            {
                return RedirectToAction(nameof(ThankYou));
            }

            try
            {
                channel = channel?.Replace(' ', '+');
                UserID = UserID?.Replace(' ', '+');
                TraxID = TraxID?.Replace(' ', '+');
                Rating = Rating?.Replace(' ', '+');
                Email = Email?.Replace(' ', '+');
                Account = Account?.Replace(' ', '+');
                Phone = Phone?.Replace(' ', '+');
                channel = channel?.Replace('@', '+');
                UserID = UserID?.Replace('@', '+');
                TraxID = TraxID?.Replace('@', '+');
                Rating = Rating?.Replace('@', '+');
                Email = Email?.Replace('@', '+');
                Account = Account?.Replace('@', '+');
                Phone = Phone?.Replace('@', '+');
            }
            catch (Exception)
            {
            }

            string decryptedChannel = "";
            string decryptedUserID = "";
            string decryptedTraxID = "";
            string decryptedRating = "";
            string decryptedEmail = "";
            string decryptedAccount = "";
            string decryptedPhone = "";


            decryptedChannel = Crypto.DecryptText(channel);
            if (decryptedChannel.Contains("Invalid encryption"))
            {
                decryptedChannel = "";
            }

            decryptedUserID = Crypto.DecryptText(UserID);
            if (decryptedUserID.Contains("Invalid encryption"))
            {
                decryptedUserID = "";
            }

            decryptedTraxID = Crypto.DecryptText(TraxID);
            if (decryptedTraxID.Contains("Invalid encryption"))
            {
                decryptedTraxID = "";
            }

            decryptedRating = Crypto.DecryptText(Rating);
            if (decryptedRating.Contains("Invalid encryption"))
            {
                decryptedRating = "";
            }

            decryptedEmail = Crypto.DecryptText(Email);
            if (decryptedEmail.Contains("Invalid encryption"))
            {
                decryptedEmail = "";
            }

            decryptedAccount = Crypto.DecryptText(Account);
            if (decryptedAccount.Contains("Invalid encryption"))
            {
                decryptedAccount = "";
            }

            decryptedPhone = Crypto.DecryptText(Phone);
            if (decryptedPhone.Contains("Invalid encryption"))
            {
                decryptedPhone = "";
            }

            if ((decryptedChannel.Trim() == "193" || decryptedChannel.Trim() == "194" || decryptedChannel.Trim() == "74" || decryptedChannel.Trim() == "184" || decryptedChannel.Trim() == "188" || decryptedChannel.Trim() == "170" || decryptedChannel.Trim() == "2"))
            {
                decryptedChannel = "otherChannels";
            }
            else
            {
                decryptedChannel = "emailalert";
            }

            var recommendRatings = Enumerable.Range(0, 11).Select(x => new { Value = x, Text = x <= 6 ? "Detractor" : x <= 8 ? "Passive" : "Promoter" });

            switch (decryptedChannel?.ToLower())
            {
                case "emailalert":
                    ViewBag.Branch = "";
                    ViewBag.SurveyType = "EmailAlert";
                    ViewBag.TransactionId = decryptedTraxID;
                    ViewBag.CustomerName = decryptedAccount;
                    ViewBag.CustomerPhone = decryptedPhone;
                    ViewBag.NPSRating = decryptedRating;
                    ViewBag.UserId = decryptedUserID;
                    ViewBag.Email = decryptedEmail;
                    return View("EmailAlert");

                default:
                    ViewBag.Branch = "";
                    ViewBag.SurveyType = "OtherChannels";
                    ViewBag.TransactionId = decryptedTraxID;
                    ViewBag.RecommendRatings = recommendRatings;
                    return View("Index");

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(SurveyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var emailService = new Helper();

            var survey = new SurveyResponse
            {
                SurveyType = model.SurveyType ?? "",
                Branch = model.Branch ?? "",
                Staff = model.Staff ?? "",
                ServiceSatisfaction = model.ServiceSatisfaction,
                Feedback = model.Comments ?? "",

                StaffProfessionalism = model.BranchQRViewModel?.StaffProfessionalism ?? 0,
                BranchAmbience = model.BranchQRViewModel?.BranchAmbience ?? 0,

                RecommendationLikelihood = model.RecommendationLikelihood,

                TransactionEase = model.EmailAlertViewModel?.TransactionEase ?? 0,
                ProductRating = model.EmailAlertViewModel?.ProductRating ?? 0,

                IssueResolution = model.OtherSurveyViewModel?.IssueResolution ?? 0,
                WebRating = model.OtherSurveyViewModel?.WebRating ?? 0,

                CustomerName = model.CustomerName ?? "",
                CustomerPhone = model.CustomerPhone ?? "",
                Email = model.Email ?? "",
                TransactionId = model.TransactionId ?? "",

                SubmittedAt = DateTime.Now
            };

            _context.SurveyResponses.Add(survey);

            await _context.SaveChangesAsync();

            bool lowRating =
                model.ServiceSatisfaction is 1 or 2 || model.BranchQRViewModel?.StaffProfessionalism is 1 or 2 ||
                model.BranchQRViewModel?.BranchAmbience is 1 or 2 || model.RecommendationLikelihood is 1 or 2 ||
                model.EmailAlertViewModel?.TransactionEase is 1 or 2 || model.EmailAlertViewModel?.ProductRating is 1 or 2 ||
                model.OtherSurveyViewModel?.IssueResolution is 1 or 2 || model.OtherSurveyViewModel?.WebRating is 1 or 2;

            if (lowRating)
            {

                //Send mail to support team and save low response   contactcentre@jaizbankplc.com feedbackalerts@jaizbankplc.com
                var surveyAlert = new SurveyAlert
                {
                    Branch = "",
                    SurveyId = survey.SurveyId,
                    AlertType = "Low Rating",
                    CreatedDate = DateTime.Now
                };

                _context.SurveyAlerts.Add(surveyAlert);

                await _context.SaveChangesAsync();


                emailService.SendNotificationEmail("feedbackalerts@jaizbankplc.com");
            }

            return RedirectToAction(nameof(ThankYou));
        }

        public IActionResult ThankYou()
        {
            return View();
        }
    }
}
