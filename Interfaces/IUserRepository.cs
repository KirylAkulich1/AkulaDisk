
using Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        ApplicationUser GetUserByName(string UserName);
        ApplicationUser GetUserByNameWithShared(string UserNme);
        IEnumerable<FileModel> GetFiles(string UsrName,string path);
        void AddFile(string UserName, FileModel fm);
        void RemoveFile(string UserName, FileModel fm);
        void AddShared(string UserName, SharedFolder fm);
        IEnumerable<SharedFolder> GetShared(string Username);
        IEnumerable<AddRequest> GetSendedRequests(string UserName);
        IEnumerable<AddRequest> GetInputRequests(string UserName);
        void SaveChanges();
        void AddToIncomeRequuest(string userName,AddRequest req);
        void AddToOutComeRequest(string userName,AddRequest req);
        ApplicationUser GetUserWithOtherSharedFolders(string UserName);

        
    }
}
