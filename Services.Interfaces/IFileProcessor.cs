
using Domain.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IFileProcessor
    {
        void DeleteFile(string path);
        void Save(IFormFile file, string userName, string webRootPath, string relativePath);
        void CreateFolder(string FoldePath);
        void DeleteFolder(string FolderPath);
        void AddFolder(string FolderPath);
        void Save(IFormFile file, string path);
        //void DownLoad(string userName, string webRootPath, string relativePath);
    }
}
