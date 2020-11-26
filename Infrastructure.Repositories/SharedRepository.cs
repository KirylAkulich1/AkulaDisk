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
        private ApplicationDbContext db;
        public SharedRepository(ApplicationDbContext _db)
        {
            db = _db;
        }

        public void AddRequest(string SharedId,AddRequest req)
        {
            var folder = GetFolderById(SharedId);
            folder.Requests.Add(req);
        }

        public void AddUser(ApplicationUser user, SharedFolder folder)
        {
            var sharedUser = new SharedUser { User = user,Folder=folder };
            folder.Users.Add(sharedUser);
        }

        public SharedFolder GetFolderById(string SharedId)
        {
            return db.SharedFolders
                .Include(p=>p.Requests)
                .ThenInclude(p=>p.ToUser)
                .Include(p=>p.File)
                .First(p=>p.Id == SharedId);
        }

        public IEnumerable<AddRequest> GetRelatedRequests(string FolderId)
        {
            var folder = GetFolderById(FolderId);
            return folder.Requests;
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}
