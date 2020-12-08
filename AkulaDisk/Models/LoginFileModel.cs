using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AkulaDisk.Models
{
    public class LoginFileModel
    {
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Path { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
