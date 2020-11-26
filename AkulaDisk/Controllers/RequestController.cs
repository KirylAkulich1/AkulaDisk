using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Interfaces;
using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AkulaDisk.Controllers
{
    public class RequestController : Controller
    {
        private IFileRepository _fileRepo;
        private IUserRepository _userRepo;
        private IRequestRepository _reqRepo;
        private ISharedFolderRepository _sharedrepo;
        private IMailService _mailservice;
        public RequestController(IFileRepository fileRepo,IUserRepository userRepo,
            IRequestRepository reqRepo,ISharedFolderRepository sharedRepo,IMailService mailservice)
        {
            _fileRepo = fileRepo;
            _userRepo = userRepo;
            _reqRepo = reqRepo;
            _sharedrepo = sharedRepo;
            _mailservice = mailservice;
        }
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult FolderRequest(string fileId)
        {
            var requests = _sharedrepo.GetRelatedRequests(fileId);
            ViewBag.FolderId = fileId;
            return View(requests);
        }
        [HttpPost]
        public async Task<IActionResult> AddRequest(string folderId,string userName)
        {
            AddRequest request = new AddRequest();
            request.Date = DateTime.Now;
            _userRepo.AddToOutComeRequest(User.Identity.Name,request);
            _userRepo.AddToIncomeRequuest(userName, request);
            _sharedrepo.AddRequest(folderId, request);
            _userRepo.SaveChanges();
            MailRequest mailRequest = new MailRequest
            {
                Subject="Subject",
                Attachments = new List<IFormFile>(),
                ToEmail = "akuladisksender@gmail.com",
                Body = "Hello message"
            };
            await _mailservice.SendEmailAsync(mailRequest);
            return RedirectToAction("FolderRequest",new { fileId=folderId});
        }
        public IActionResult Income()
        {

            var requests=_userRepo.GetInputRequests(User.Identity.Name);
            return View(requests);
        }
        public IActionResult Sended()
        {
            var requests = _userRepo.GetSendedRequests(User.Identity.Name);
            return View(requests);
        }
        public IActionResult Accept(int requestid)
        {
            var request = _reqRepo.GetRequestById(requestid);
            _sharedrepo.AddUser(request.ToUser, request.Folder);
            _sharedrepo.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Deny(int requestid)
        {
            var request = _reqRepo.GetRequestById(requestid);
            _reqRepo.DeleteRequest(request);
            return RedirectToAction("Index");
        }
      
    }
}
