using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AkulaDisk.Controllers
{
    public class CommonController : Controller
    {
        private IUserRepository _userRepo;
        private ISharedFolderRepository _sharedRepository;

        public CommonController(IUserRepository userRepo,ISharedFolderRepository sharedRepo)
        {
            _userRepo = userRepo;
            _sharedRepository = sharedRepo;

        }
        public IActionResult Index(string path="\\")
        {
            var folders=_userRepo.GetShared(User.Identity.Name);
            return View(folders);
        }
        public IActionResult Other(string path = "\\")
        {
            var user = _userRepo.GetUserWithOtherSharedFolders(User.Identity.Name);
            return View();
        }
    }
}
