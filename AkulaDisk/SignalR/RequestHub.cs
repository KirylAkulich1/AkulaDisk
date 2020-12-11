using AkulaDisk.Models;
using Domain.Core;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
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
        private IMailService _mailService;
        public RequestHub(ILogger<RequestHub> logger,ISharedFolderRepository sharedRepository,
            IUserRepository userRepo,IMailService mailService)
        {
            _logger = logger;
            _sharedRepo = sharedRepository;
            _userRepo = userRepo;
            _mailService = mailService;
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
            TableRow tr = new TableRow { From = message.From, Request = request.Id,Data = DateTime.Now.ToString(),Name=request.Name,To=message.To };
            // message.Request = request.Id;
            // message.Data = request.Date.ToString();
            _userRepo.SaveChanges();
            MailRequest mailRequest = new MailRequest
            {
                Subject = "Subject",
                Attachments = new List<IFormFile>(),
                ToEmail = message.To,
                Body = $"Click: <a href='https://akuladisk20201119113753.azurewebsites.net/Request/Accept/?requestid={request.Id}'>Accept</a>.<p>" +
               $"<a href = 'https://akuladisk20201119113753.azurewebsites.net/Request/Deny/?requestid={request.Id}' >Deny</a>. " //String.Format(
                // "User {0} offer you request on {1}:\n" +
                //"To accept follow link: {2}/Request/AddRequest/?fileid={3} username={4}} \n" +
                //"To deny follow link:  {2}/Request/AddRequest/?fileid={3} username={4}}")
            };
            await _mailService.SendEmailAsync(mailRequest);
            await Clients.User(message.To).SendAsync("Recieve", tr);
            await Clients.Caller.SendAsync("Sended", tr);
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
