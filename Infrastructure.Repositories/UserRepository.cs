
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
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext db;
        public UserRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void AddFile(string UserName, FileModel fm)
        {
            var user = GetUserByName(UserName);
            user.Files.Add(fm);
            db.SaveChanges();
        }

        public void AddShared(string UserName, SharedFolder fm)
        {
            var user = GetUserByName(UserName);
            user.OwnShared.Add(fm);
            db.SaveChanges();
        }

        public IEnumerable<FileModel> GetFiles(string UsrName,string path)
        {
            return db.Users.Include(p=>p.Files).First(p => p.UserName == UsrName).Files.Where(file=>file.Path==path);
        }

        public ApplicationUser GetUserByName(string UserName)
        {
            return db.Users.Include(p => p.Files).Include(p=>p.OwnShared).First(p=>p.UserName==UserName);
        }
       

        public void RemoveFile(string UserName, FileModel fm)
        {
            var user = GetUserByName(UserName);
            user.Files.Remove(fm);
            db.SaveChanges();
        }
        public IEnumerable<SharedFolder> GetShared(string UserName)
        {
            var user = GetUserByNameWithShared(UserName);
            return user.OwnShared;
        }

        public ApplicationUser GetUserByNameWithShared(string UserName)
        {
            return db.Users.Include(p => p.OwnShared).ThenInclude(p=>p.File).First(p => p.UserName == UserName);
        }
        public ApplicationUser GetUserByNameWithSended(string UserName)
        {
            return db.Users.Include(p => p.Sended).ThenInclude(p => p.ToUser).First(p => p.UserName == UserName);
        }
        public ApplicationUser GetUserByNameWithIncome(string UserName)
        {
            return db.Users.Include(p => p.Income).ThenInclude(p => p.FromUser)
                .First(p => p.UserName == UserName);
        }

        public IEnumerable<AddRequest> GetSendedRequests(string UserName)
        {
            var User = GetUserByNameWithSended(UserName);
            return User.Sended;
        }

        public IEnumerable<AddRequest> GetInputRequests(string UserName)
        {
            var User = GetUserByNameWithIncome(UserName);
            return User.Income;
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public void AddToIncomeRequuest(string UserName,AddRequest req)
        {
            var user = GetUserByNameWithIncome(UserName);
            user.Income.Add(req);
        }

        public void AddToOutComeRequest(string UserName,AddRequest req)
        {
            var user = GetUserByNameWithSended(UserName);
            user.Sended.Add(req);
        }

        public ApplicationUser GetUserWithOtherSharedFolders(string UserName)
        {
            var user = db.Users.Include(p => p.SharedFiles).ThenInclude(p => p.Folder).ThenInclude(p=>p.File).First(p => p.UserName == UserName);
            return user;
        }

    }
}
