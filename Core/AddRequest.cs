using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core
{
    public class AddRequest
    {
        public int Id { get; set; }
        public string FromId { get; set; }
        public ApplicationUser FromUser { get; set; }
        public string ToId { get; set; }
        public ApplicationUser ToUser { get; set; }
        public string FolderId { get; set; }
        public SharedFolder Folder { get; set; }

        public bool Accepted;
        public string Name { get; set; } 
        public DateTime Date { get; set; }

    }
}
