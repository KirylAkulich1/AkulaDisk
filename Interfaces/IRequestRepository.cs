using Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IRequestRepository
    {
        AddRequest GetRequestById(int id);
        void DeleteRequest(AddRequest req);
        void SaveChanges();
    }
}
