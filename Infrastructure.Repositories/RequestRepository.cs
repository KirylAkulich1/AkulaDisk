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
    public class RequestRepository : IRequestRepository
    {
        private ApplicationDbContext _context;
        public RequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void DeleteRequest(AddRequest req)
        {
            var request = _context.AddRequests
                .Include(p => p.ToUser)
                .Include(p => p.Folder)
                .Single(p => p.Id == req.Id);
            if (request.Accepted)
            {
                var sharedUser = _context.SharedUsers.Single(p => p.FolderId == req.FolderId && p.UserId == req.ToId);
                _context.SharedUsers.Remove(sharedUser);
            }
            _context.AddRequests.Remove(req);
            
            
        }

        public AddRequest GetRequestById(int id)
        {
            return _context.AddRequests.Include(p => p.ToUser).Include(p => p.Folder).ThenInclude(p=>p.Users).First(p => p.Id == id);
        }
       
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
