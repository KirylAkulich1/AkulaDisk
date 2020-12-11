using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using AkulaDisk.Models;
using Domain.Core;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace AkulaDisk.Controllers
{
    [Route("api/[controller]")]
    [Consumes("application/json","application/octet-stream", "multipart/form-data")]
    [ApiController]
    public class ApiFileController : ControllerBase
    {
        IUserRepository _userRepo;
        IFileProcessor _fileService;
        UserManager<ApplicationUser> _userManager;
        public ApiFileController(IUserRepository userRepo,IFileProcessor fileProc, UserManager<ApplicationUser> userManager)
        {
            _userRepo = userRepo;
            _fileService = fileProc;
            _userManager = userManager;
            
        }
        [HttpPost("Upload/{username}/{password}")]
        public async Task<ActionResult> Upload(string username,string password,IFormFile file)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                return Unauthorized();
            }
                string userName = username;
            
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",userName,file.FileName);
                _fileService.Save(file, path);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [Authorize]
        [HttpPost("UploadWithToken")]
        public ActionResult UploadWithToken(FileApiModel file)
        {
            string userName = file.UserName;
            var user = _userRepo.GetUserByName(userName);
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", userName, file.Path, file.FormFile.Name);
                _fileService.Save(file.FormFile, path);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("Download")]
        public ActionResult Download([FromBody] LoginDownload model)
        {
            if (model.Path.Contains("."))
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(),model.UserName, model.Path);
                return PhysicalFile(path, "application/force-download");
            }
            else {
                string path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot","Files", model.UserName, model.Path);
                if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\Zips\\" + model.UserName))
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Zips\\" + model.UserName);
                
                ZipFile.CreateFromDirectory(path, Directory.GetCurrentDirectory()+"\\Zips\\"+model.UserName+"\\zip123.zip");
                return PhysicalFile(Directory.GetCurrentDirectory() + "\\Zips\\" + model.UserName + "\\zip.zip", "application/force-download", "zip.zip");
            }
        }
        [Authorize]
        [HttpPost("DownloadWithToken")]
        public ActionResult DownloadWithToken()
        {

            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "city.sql"), "application/force-download", "tests");
        }

    }
}
