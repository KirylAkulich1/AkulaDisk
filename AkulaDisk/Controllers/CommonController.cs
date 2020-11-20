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
        public CommonController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        public IActionResult Index(string path="\\")
        {
            var folders=_userRepo.GetShared(User.Identity.Name);
            return View(folders);
        }
    }
}
