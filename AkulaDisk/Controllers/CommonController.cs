using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AkulaDisk.Controllers
{
    public class CommonController : Controller
    {
        private IUserRepository _userRepo;
        private ISharedFolderRepository _sharedRepository;
        private IFileRepository _fileRepository;

        public CommonController(IUserRepository userRepo,ISharedFolderRepository sharedRepo,
            IFileRepository fileRepo)
        {
            _userRepo = userRepo;
            _sharedRepository = sharedRepo;
            _fileRepository = fileRepo;
        }
        public IActionResult Index(string path="\\")
        {
            var folders=_userRepo.GetShared(User.Identity.Name);
            return View(folders);
        }
        public IActionResult Other(string path = "\\")
        {
            var user = _userRepo.GetUserWithOtherSharedFolders(User.Identity.Name);
            return View(user.SharedFiles);
        }
        public IActionResult CommonFolder(string id,string path)
        {
            var userName = User.Identity.Name;
            bool hasAccess=_sharedRepository.IsUserHasAccess(userName, id);
            var files = _fileRepository.GetFilesbyPath(path);
            if(hasAccess)
            {

                return View(files);
            }
            else
            {
                return View(new List<FileModel>());
            }
        }
        public IActionResult Load()
        {
            return View();
        }
        public IActionResult DeleteFile()
        {
            return View();
        }
    }
}
