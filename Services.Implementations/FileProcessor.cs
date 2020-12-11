
using Domain.Core;
using Microsoft.AspNetCore.Http;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Services.Implementations
{
   public  class FileProcessor : IFileProcessor
    {
        public void AddFolder(string FolderPath)
        {
            
        }

        public void CreateFolder(string FoldePath)
        {
            try
            {
                Directory.CreateDirectory(FoldePath);
            }
            catch(Exception)
            {

            }
        }

        public void DeleteFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        public void DeleteFolder(string FolderPath)
        {
            throw new NotImplementedException();
        }

        public void DownLoad(FileModel file)
        {
            throw new NotImplementedException();
        }

      

        public async void Save(IFormFile file, string userName, string webRootPath, string relativePath)
        {
            bool folder = Directory.Exists(webRootPath + @"\Files\" + userName + relativePath);
            string folderPath = webRootPath + @"\Files\" + userName + relativePath;
            if (!folder)
                Directory.CreateDirectory(webRootPath + @"\Files\" + userName + relativePath);
           
            using (var fileStrem = new FileStream(folderPath + file.FileName, FileMode.OpenOrCreate))
            {
                await file.CopyToAsync(fileStrem);
            }
        }

        public async void Save(IFormFile file, string path)
        {
                using (var fileStrem = new FileStream(path, FileMode.OpenOrCreate))
                {
                    await file.CopyToAsync(fileStrem);
                }

        }
    }
}
