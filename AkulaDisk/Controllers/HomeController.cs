using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AkulaDisk.Models;
using Microsoft.AspNetCore.Authorization;
using Interfaces;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Services.Interfaces;
using Microsoft.AspNetCore.Hosting;

using Domain.Core;
using Services.Implementations;
using System.IO;
using Microsoft.AspNetCore.Identity;

namespace AkulaDisk.Controllers
{
   [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger _fileLogger;
        private readonly ILogger _errorFileLogger;
        private readonly ILogger _stdoutLogger;
        private readonly IUserRepository _userRepo;
        private readonly IFileRepository _fileRepo;
        private readonly IFileProcessor _fileProc;
        private readonly IWebHostEnvironment _appEnviroment;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(IWebHostEnvironment appEnviroment, IFileProcessor fileProc,ILoggerFactory loggerFactory, 
            IUserRepository userRepo,IFileRepository fileRepo)
        {
            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
            loggerFactory.AddErrorFile(Path.Combine(Directory.GetCurrentDirectory(), "elogger.txt"));
            _fileLogger = loggerFactory.CreateLogger("FileLogger");
            _errorFileLogger = loggerFactory.CreateLogger("ErrorFileLogger");
            _stdoutLogger = loggerFactory.CreateLogger("StdoutFileLogger");
            _userRepo = userRepo;
            _fileRepo = fileRepo;
            _fileProc = fileProc;
            _appEnviroment = appEnviroment;

        }

        public IActionResult Index(string path="\\")
        {
         
            ViewBag.CurrentPath = path;
            var user = _userRepo.GetUserByName(User.Identity.Name);
            var fileList = _userRepo.GetFiles(User.Identity.Name,path);
            return View(fileList);
        }
        public IActionResult AddFile(IFormFile uploadedFile, string path)
        {
            if(uploadedFile==null)
            {
                return RedirectToAction("Index", new { path = path });
            }
            _fileProc.Save(uploadedFile,User.Identity.Name,_appEnviroment.WebRootPath,path);
          //  var user = _userRepo.GetUserByName(User.Identity.Name);
            FileModel file = new FileModel
            {
                Name=uploadedFile.FileName,
                Path=path,
                Type=FileType.File,

            };
            _userRepo.AddFile(User.Identity.Name, file);
           // _fileRepo.Create(file);
           // _fileRepo.Save();
            return RedirectToAction("Index",new  {path=path });
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Download(string filename,string path)
        {
            string filePath = _appEnviroment.WebRootPath+"\\Files\\" + User.Identity.Name + path+filename;
            return PhysicalFile(filePath, "application/force-download", filename);
        }
        public IActionResult Delete(string filename,string path,string fileid)
        {
            var file = _fileRepo.GetFile(fileid);
            _userRepo.RemoveFile(User.Identity.Name, file);
            string filePath = _appEnviroment.WebRootPath + "\\Files\\" + User.Identity.Name + path + filename;
            try
            {
                _fileProc.DeleteFile(filePath);

            }catch(Exception ex)
            {
                _errorFileLogger.LogError(ex.Message);
                _fileLogger.LogError(ex.Message);
                _stdoutLogger.LogError(ex.Message);
            }
            _userRepo.SaveChanges();
            return RedirectToAction("Index", new { path = path });
        }
        [HttpPost]
        public IActionResult CreateFolder(string path,string foldername)
        {
            string filePath = _appEnviroment.WebRootPath + "\\Files\\" + User.Identity.Name + path + foldername+"\\";
            FileModel file = new FileModel
            {
                Name = foldername,
                Path = path,
                Type = FileType.Folder,

            };
            _fileProc.CreateFolder(filePath);
            _userRepo.AddFile(User.Identity.Name,file);
            return RedirectToAction("Index",new { path = path });
        }
        public IActionResult MakeShared(string path,string folderId)
        {
            FileModel folder = _fileRepo.GetFile(folderId);
            folder.isShared = true;
            ApplicationUser user = _userRepo.GetUserByName(User.Identity.Name);
            SharedFolder sharedFolder = new SharedFolder();
            _fileRepo.MakeShared(folderId,sharedFolder);
            _userRepo.AddShared(User.Identity.Name, sharedFolder);
            return RedirectToAction("Index",new  {path=path });
        }
        /*
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return View();
            }
        }*/
    }
}
