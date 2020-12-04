using AkulaDisk.Models;
using Domain.Core;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AkulaDisk.SignalR
{
    [Authorize]
    public class RequestHub:Hub
    {
        private ILogger<RequestHub> _logger;
        private ISharedFolderRepository _sharedRepo;
        private IUserRepository _userRepo;
        public RequestHub(ILogger<RequestHub> logger,ISharedFolderRepository sharedRepository,IUserRepository userRepo)
        {
            _logger = logger;
            _sharedRepo = sharedRepository;
            _userRepo = userRepo;
        }

        public async Task Send(RequestViewModel message)
        {
           
            var userName = Context.User.Identity.Name;
            if (message.To == userName)
            {
          //      return;
            }
            AddRequest request = new AddRequest();
            request.Date = DateTime.Now;
            _userRepo.AddToOutComeRequest(message.From, request);
            _userRepo.AddToIncomeRequuest(message.To, request);
            _sharedRepo.AddRequest(message.Folder, request);
            _userRepo.SaveChanges();
            _logger.LogError(message.To);
            TableRow tr = new TableRow { From = message.From, Request = request.Id,Data=request.Date.ToString(),Name=request.Name };
           // message.Request = request.Id;
           // message.Data = request.Date.ToString();
            await Clients.User(message.To).SendAsync("Recieve", tr);
            await Clients.Caller.SendAsync("Sended", message);
        }
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Notify", "");
            
        }
        public class User
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
        public class RequestViewModel
        {
            public string From { get; set; }
            public string To { get; set; }
            public string Folder { get; set; }
            public string Name { get; set; }

        }
        public class TableRow {
            public string From { get; set; }
            public string To { get; set; }
            public string Folder { get; set; }
            public string Name { get; set; }
            public int Request { get; set; }
            public string Data { get; set; }
        }

    }
}
