using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core
{
    public class SharedFolder
    {
        public string Id { get; set; }
        public string FileModelId { get; set; }
        public FileModel File { get ; set ; }

        public ApplicationUser User { get; set; }
        public ICollection<SharedUser> Users { get; set; } 

        public ICollection<AddRequest> Requests { get; set; }
 
    }
}
