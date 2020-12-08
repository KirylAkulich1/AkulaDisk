using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AkulaDisk.Models
{

    public class FileApiModel 
    {
        public string UserName { get; set; }
        public string Path { get; set; }
        public IFormFile FormFile{get;set;}
    }
}
