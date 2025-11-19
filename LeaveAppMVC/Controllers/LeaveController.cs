using LeaveAppMVC.Data;
using LeaveAppMVC.Models;
using LeaveAppMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeaveAppMVC.Controllers
{
    public class LeaveController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly EmailService _email;

        public LeaveController(ApplicationDbContext dbContext, EmailService email)
        {
            _dbContext = dbContext;
            _email = email;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(LeaveApplicationModel leaveModel)
        {
            leaveModel.Status = "Pending";

            if (!ModelState.IsValid)
            {
                return View(leaveModel);
            }

            _dbContext.LeaveApplications.Add(leaveModel);
            _dbContext.SaveChanges();

            _email.SendEmail(
                to: "najwa@cmasolutions.my",
                subject: "New Leave Request Needs Approval",
                message: $"Employee <b>{leaveModel.EmployeeName}</b> submitted a leave request.<br>" +
                         $"Start: {leaveModel.StartDate}<br>" +
                         $"End: {leaveModel.EndDate}<br><br>" +
                         $"Please log in to approve or reject."
            );

            _email.SendEmail(
                to: "najwa@cmasolutions.my",
                subject: "Your Leave Request Was Submitted",
                message: $"Hi {leaveModel.EmployeeName},<br>" +
                         $"Your leave request is now pending manager approval."
            );

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Review()
        {
            var pending = _dbContext.LeaveApplications
                .Where(x => x.Status == "Pending")
                .ToList();

            return View(pending);
        }

        public IActionResult Approve(int id)
        {
            var record = _dbContext.LeaveApplications.Find(id);
            record.Status = "Approved";
            _dbContext.SaveChanges();

            // notify applicant
            _email.SendEmail(
                to: "najwa@cmasolutions.my",
                subject: "Leave Approved",
                message: "Your leave request has been <b>APPROVED</b>."
            );

            return RedirectToAction("Review");
        }

        public IActionResult Reject(int id)
        {
            var record = _dbContext.LeaveApplications.Find(id);
            record.Status = "Rejected";
            _dbContext.SaveChanges();

            // notify applicant
            _email.SendEmail(
                to: "najwa@cmasolutions.my",
                subject: "Leave Rejected",
                message: "Your leave request has been <b>REJECTED</b>."
            );

            return RedirectToAction("Review");
        }
    }
}
