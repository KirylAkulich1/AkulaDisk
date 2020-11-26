using Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
   public  interface ISharedFolderRepository
    {
        void AddRequest(string SharedId,AddRequest req);
        SharedFolder GetFolderById(string SharedId);
        IEnumerable<AddRequest> GetRelatedRequests(string SharedId);
        void AddUser(ApplicationUser user,SharedFolder folder);
        void SaveChanges();
        
    }
}
