
using Domain.Core;
using Infrastructure.Data;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class FileRepository : IFileRepository
    {
        private ApplicationDbContext db;
        private bool disposed = false;
        public FileRepository(ApplicationDbContext ctx)
        {
           db = ctx;
        }
        public void Create(FileModel file)
        {
            db.Files.Add(file);
        }

        public void Delete(string id)
        {
            FileModel file = db.Files.Find(id);
            if (file != null)
                db.Files.Remove(file);
        }

        public void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                  //  db.Dispose();
                }
            }
            this.disposed = true;
        }

        public FileModel GetFile(string id)
        {
            return db.Files.Include(p=>p.Shared).ThenInclude(p=>p.Requests).Include(p=>p.Owner).First(p=>p.Id==id);
        }

        public IEnumerable<FileModel> GetFileList()
        {
            return db.Files;
         }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(FileModel file)
        {
            db.Entry(file).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        IEnumerable<FileModel> IFileRepository.GetFileList()
        {
            throw new NotImplementedException();
        }

        public void MakeShared(string id,SharedFolder folder)
        {
            var file = db.Files.Find(id);
            file.Shared = folder;
           

        }

        public IEnumerable<FileModel> GetFilesbyPath(string path)
        {
            return db.Files.Where(p=>p.Path==path);
        }
    }
}
