
using Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public interface IFileRepository:IDisposable
    {
        IEnumerable<FileModel> GetFileList();
        FileModel GetFile(string id);
        void Create(FileModel file);
        void Update(FileModel file);
        void Delete(string id);
        void Save();
        void MakeShared(string id, SharedFolder folder);
        IEnumerable<FileModel> GetFilesbyPath(string path);
    }
}
