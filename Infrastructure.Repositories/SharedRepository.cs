using Domain.Core;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Repositories
{
    public class SharedRepository: ISharedFolderRepository
    {
        private ApplicatopnDbContext db;
        public SharedRepository(ApplicatopnDbContext _db)
        {
            db = _db;
        }

        public void AddRequest(string SharedId,AddRequest req)
        {
            var folder = GetFolderById(SharedId);
            folder.Requests.Add(req);
            db.SaveChanges();
        }

        public SharedFolder GetFolderById(string SharedId)
        {
            return db.SharedFolders
                .Include(p=>p.Requests)
                .Include(p=>p.File)
                .First(p=>p.Id == SharedId);
        }

        public IEnumerable<AddRequest> GetRelatedRequests(string FolderId)
        {
            var folder = GetFolderById(FolderId);
            return folder.Requests;
        }
    }
}
