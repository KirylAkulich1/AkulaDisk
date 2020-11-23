using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Interfaces;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AkulaDisk.Controllers
{
    public class RequestController : Controller
    {
        private IFileRepository _fileRepo;
        private IUserRepository _userRepo;
        private IRequestRepository _reqRepo;
        private ISharedFolderRepository _sharedrepo;

        public RequestController(IFileRepository fileRepo,IUserRepository userRepo,
            IRequestRepository reqRepo,ISharedFolderRepository sharedRepo)
        {
            _fileRepo = fileRepo;
            _userRepo = userRepo;
            _reqRepo = reqRepo;
            _sharedrepo = sharedRepo;
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
        public IActionResult AddRequest(string folderId,string userName)
        {
            AddRequest request = new AddRequest();
            request.Date = DateTime.Now;
            _userRepo.AddToOutComeRequest(User.Identity.Name,request);
            _userRepo.AddToIncomeRequuest(userName, request);
            _sharedrepo.AddRequest(folderId, request);
            _userRepo.SaveChanges();
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
    }
}
