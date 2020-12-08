using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AkulaDisk.Controllers
{
    [Authorize]
    public class CommonController : Controller
    {
        private IUserRepository _userRepo;
        private ISharedFolderRepository _sharedRepository;
        private IFileRepository _fileRepository;
        private readonly IFileRepository _fileRepo;
        private readonly IFileProcessor _fileProc;
        private readonly IWebHostEnvironment _appEnviroment;
        private string folderId;
        public CommonController(IUserRepository userRepo,ISharedFolderRepository sharedRepo,
            IFileRepository fileRepo, IWebHostEnvironment appEnviroment, IFileProcessor fileProc)
        {
            _userRepo = userRepo;
            _sharedRepository = sharedRepo;
            _fileRepository = fileRepo;
            _fileProc = fileProc;
            _appEnviroment = appEnviroment;

        }
        public IActionResult Index(string path="\\")
        {
            var folders=_userRepo.GetShared(User.Identity.Name);
            return View(folders);
        }
        public IActionResult DeleteShared(string id,string fileId)
        {
            _sharedRepository.DeleteShared(id, fileId);
            return RedirectToAction("Index");
        }
        public IActionResult Other(string path = "\\")
        {
            var user = _userRepo.GetUserWithOtherSharedFolders(User.Identity.Name);
            return View(user.SharedFiles);
        }
        public IActionResult CommonFolder(string id,string ownername,string path)
        {
            var userName = User.Identity.Name;
            bool hasAccess=_sharedRepository.IsUserHasAccess(userName, id);
            var files =_userRepo.GetFiles(ownername,path);
            ViewBag.CurrentPath = path;
            ViewBag.CurrentId = id;
            ViewBag.OwnerName = ownername;
            if (hasAccess)
            {

                 return View(files);
            }
            else
            {
                return View(new List<FileModel>());
            }
        }
        public IActionResult Load(string id,string ownername,string path, IFormFile uploadedFile)
        {
            var userName = User.Identity.Name;
            bool hasAccess = _sharedRepository.IsUserHasAccess(userName, id);
            if (uploadedFile == null)
            {
                return RedirectToAction("CommonFolder", new { path = path });
            }
            _fileProc.Save(uploadedFile, ownername, _appEnviroment.WebRootPath, path);
            //  var user = _userRepo.GetUserByName(User.Identity.Name);
            FileModel file = new FileModel
            {
                Name = uploadedFile.FileName,
                Path = path,
                Type = FileType.File,

            };
            _userRepo.AddFile(ownername, file);
            _userRepo.SaveChanges();
            return RedirectToAction("CommonFolder", new { id=id,ownername=ownername, path = path });
        }
        public IActionResult DeleteFile(string id,string fileId,string ownername,string path)
        {
            var userName = User.Identity.Name;
            bool hasAccess = _sharedRepository.IsUserHasAccess(userName, id);
            var file = _fileRepo.GetFile(fileId);
            _userRepo.RemoveFile(User.Identity.Name, file);
            string filePath = _appEnviroment.WebRootPath + "\\Files\\" + User.Identity.Name + path + file.Name;
            _fileProc.DeleteFile(filePath);
            _userRepo.SaveChanges();
            return RedirectToAction("Index", new {id=id,ownername=ownername, path = path });
        }
    
        public IActionResult Download(string filename, string path)
        {
            var userName = User.Identity.Name;
            bool hasAccess = _sharedRepository.IsUserHasAccess(userName, folderId);
            string filePath = _appEnviroment.WebRootPath + "\\Files\\" + User.Identity.Name + path + filename;
            return PhysicalFile(filePath, "application/force-download", filename);
        }
        public IActionResult Delete(string filename, string path, string fileid)
        {
            var userName = User.Identity.Name;
            bool hasAccess = _sharedRepository.IsUserHasAccess(userName, folderId);
            var file = _fileRepo.GetFile(fileid);
            _userRepo.RemoveFile(User.Identity.Name, file);
            string filePath = _appEnviroment.WebRootPath + "\\Files\\" + User.Identity.Name + path + filename;
            _fileProc.DeleteFile(filePath);
            return RedirectToAction("Index", new { path = path });
        }
    }
}
