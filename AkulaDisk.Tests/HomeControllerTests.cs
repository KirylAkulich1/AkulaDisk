using AkulaDisk.Controllers;
using Domain.Interfaces;
using Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace AkulaDisk.Tests
{
    public class HomeControllerTests
    {
        ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, "test1"),
                                        new Claim(ClaimTypes.Name, "test1@gmail.com")
                                        // other required and custom claims
                                   }, "TestAuthentication"));
       
        HomeController controller;
        public HomeControllerTests()
        {


            var logger = new NullLogger<HomeController>();
            var controller = new HomeController(new Mock<IWebHostEnvironment>().Object,
                new FakeFileProcessor(), logger,
                new FakeRepository(),
                new Mock<IFileRepository>().Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
        }
        [Fact]
        public void Index_should_return_private_view_for_authenticated_user()
        {
            var logger = new NullLogger<HomeController>();
            var controller = new HomeController(new Mock<IWebHostEnvironment>().Object,
               new FakeFileProcessor(), logger,
               new FakeRepository(),
               new Mock<IFileRepository>().Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            var result = controller.Index() as ViewResult;
            var viewName = result.ViewName;

            Assert.True(string.IsNullOrEmpty(viewName) || viewName == "Index");
        }
        [Fact]
        public void AddFile_should_redirect_to_Index()
        {
            var logger = new NullLogger<HomeController>();
            var controller = new HomeController(new Mock<IWebHostEnvironment>().Object,
               new FakeFileProcessor(), logger,
               new FakeRepository(),
               new Mock<IFileRepository>().Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            var result = controller.AddFile(new Mock<IFormFile>().Object,"\\") as RedirectToActionResult;
            Assert.True(result.ActionName == "Index");
        }
        [Fact]
        public void Download_should_return_file()
        {
            var logger = new NullLogger<HomeController>();
            var controller = new HomeController(new Mock<IWebHostEnvironment>().Object,
               new FakeFileProcessor(), logger,
               new FakeRepository(),
               new Mock<IFileRepository>().Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            var result = controller.Download("E:\\abc\\C\\C0019_test.jpg", "\\") as IActionResult;
            Assert.True(result!=null);
        }
        [Fact]
        public void Delete_should_redirect_to_index()
        {
            var logger = new NullLogger<HomeController>();
            var controller = new HomeController(new Mock<IWebHostEnvironment>().Object,
               new FakeFileProcessor(), logger,
               new FakeRepository(),
               new Mock<IFileRepository>().Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            var result = controller.Delete("","","") as RedirectToActionResult;
            Assert.True(result.ActionName == "Index");
        }
        [Fact]
        public void CreateFolder_should_redirect_to_index()
        {
            var logger = new NullLogger<HomeController>();
            var controller = new HomeController(new Mock<IWebHostEnvironment>().Object,
               new FakeFileProcessor(), logger,
               new FakeRepository(),
               new Mock<IFileRepository>().Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            var result = controller.CreateFolder("", "") as RedirectToActionResult;
            Assert.True(result.ActionName == "Index");
        }
        [Fact]
        public void MakeShared_should_redirect_to_index()
        {
            var logger = new NullLogger<HomeController>();
            var controller = new HomeController(new Mock<IWebHostEnvironment>().Object,
               new FakeFileProcessor(), logger,
               new FakeRepository(),
               new Mock<IFileRepository>().Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            var result = controller.MakeShared("", "file1") as RedirectToActionResult;
            Assert.True(result.ActionName == "Index");
        }
    }
}
