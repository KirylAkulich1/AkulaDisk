using Microsoft.AspNetCore.Http;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkulaDisk.Tests
{
    public class FakeFileProcessor : IFileProcessor
    {
        public void AddFolder(string FolderPath)
        {
            throw new NotImplementedException();
        }

        public void CreateFolder(string FoldePath)
        {
            
        }

        public void DeleteFile(string path)
        {
            
        }

        public void DeleteFolder(string FolderPath)
        {
            
        }

        public void Save(IFormFile file, string userName, string webRootPath, string relativePath)
        {
            
        }

        public void Save(IFormFile file, string path)
        {
            throw new NotImplementedException();
        }
    }
}
