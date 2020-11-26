using Domain.Core;
using Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkulaDisk.Tests
{
    class FakeRepository : IUserRepository
    {
        Dictionary<String, ApplicationUser> _users = new Dictionary<string, ApplicationUser>
        {
            { "test1@gmail.com",new ApplicationUser{ Email="test1@gmail.com",Id="test1",UserName="test1",Files=new List<FileModel>(),OwnShared=new List<SharedFolder>(),Income=new List<AddRequest>(),Sended=new List<AddRequest>()} },
            {"test2@gmail.com",new ApplicationUser{ Email="test2@gmail.com",Id="test2",UserName="test2",Files=new List<FileModel>(),OwnShared=new List<SharedFolder>(),Income=new List<AddRequest>(),Sended=new List<AddRequest>()} }
        };
        Dictionary<String, FileModel> _files = new Dictionary<string, FileModel>
        {
            {"file1",new FileModel{ Id="1",Name="file1",Path="\\",Type=FileType.File,ApplicationUserId="test1"} },
            {"file2",new FileModel{ Id="2",Name="file2",Path="\\",Type=FileType.Folder,ApplicationUserId="test2"} }
        };
        Dictionary<String, AddRequest> _addRequests = new Dictionary<string, AddRequest>
        {
            { "request1",new AddRequest{ Id=1,FromId="test1",ToId="test2"} },
            { "request2",new AddRequest{ Id=2,FromId="test2",ToId="test1"} }
        };
        Dictionary<String, SharedFolder> _sharedFolders = new Dictionary<string, SharedFolder>
        {

        };
        public void AddFile(string UserName, FileModel fm)
        {
            _users[UserName].Files.Add(fm);
        }

        public void AddShared(string UserName, SharedFolder fm)
        {
            _users[UserName].OwnShared.Add(fm);
        }

        public void AddToIncomeRequuest(string userName, AddRequest req)
        {
            _users[userName].Income.Add(req);
        }

        public void AddToOutComeRequest(string userName, AddRequest req)
        {
            _users[userName].Sended.Add(req);
        }

        public IEnumerable<FileModel> GetFiles(string UsrName, string path)
        {
            return _files.Values;
        }

        public IEnumerable<AddRequest> GetInputRequests(string UserName)
        {
            return _addRequests.Values;
        }

        public IEnumerable<AddRequest> GetSendedRequests(string UserName)
        {
            return _addRequests.Values;
        }

        public IEnumerable<SharedFolder> GetShared(string Username)
        {
            return _sharedFolders.Values;
        }

        public ApplicationUser GetUserByName(string UserName)
        {
            return _users[UserName];
        }

        public ApplicationUser GetUserByNameWithShared(string UserNme)
        {
            var user = _users[UserNme];
            user.OwnShared.Add(new Mock<SharedFolder>().Object);
            return user;
        }

        public ApplicationUser GetUserWithOtherSharedFolders(string UserName)
        {
            throw new NotImplementedException();
        }

        public void RemoveFile(string UserName, FileModel fm)
        {
           
        }

        public void SaveChanges()
        {
            
        }
    }
}
