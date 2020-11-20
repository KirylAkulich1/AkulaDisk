
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Core
{
    public class ApplicationUser: IdentityUser
    {
        public ICollection<FileModel> Files { get; set; }
        public ICollection<SharedUser> SharedFiles { get; set; }
        public ICollection<SharedFolder> OwnShared { get; set; }
       
        public ICollection<AddRequest> Sended { get; set; }
      
        public ICollection<AddRequest> Income { get; set; }
    }
}
