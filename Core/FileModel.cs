using Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core
{
    public enum FileType
    {
        Folder,
        Image,
        File
    }
    public class FileModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public FileType Type { get; set; }
        public SharedFolder Shared { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser Owner { get; set; }
        public bool isShared { get; set; }
    }
}
