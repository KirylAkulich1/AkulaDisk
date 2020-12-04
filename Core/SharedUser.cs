using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core
{
    public class SharedUser
    {
       public string UserId { get; set; }
        public string FolderId { get; set; }
        public ApplicationUser User { get; set; }
        public SharedFolder Folder { get; set; }
    }
}
