using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Data;
using FakeItEasy;
using WebApplication1.Clients;
using WebApplication1.Controllers;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using WebApplication1.Models;
using Microsoft.AspNetCore.Http;
using Moq;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using FluentAssertions.Common;
using WebApplication1.Models.AnimeApi;
using Microsoft.Extensions.Configuration;

namespace WebApplication1.Tests.ControllerTests
{
    public class UserManagementControllerTests
    {


       
        public ApplicationDbContext GetDbContext()
        {
            IConfiguration _config = new ConfigurationBuilder().Build();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated(); ;
            
            databaseContext.Users.Add(new User
            {
                UserName = "test",
                Password = _config["User_Test_Password"],
                Salt = _config["User_Test_Salt"]
            });
            databaseContext.SaveChangesAsync();
            
            return databaseContext;


        }

        public Mock<IServiceProvider> GetIServiceProvider()
        {
            var authServiceMock = new Mock<IAuthenticationService>();
            //HttpContext.SignInAsync format context SignInAsync(context, scheme, principal, properties);
            authServiceMock.Setup(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(),
                It.IsAny<AuthenticationProperties>()));
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(_ => _.GetService(typeof(IAuthenticationService))).Returns(authServiceMock.Object);
            return serviceProviderMock;

        }


        [Fact]
        public void UserManagement_LoginAsync_ReturnsIActionResult()
        {
            // Setup
            var _db = GetDbContext();
            var serviceProviderMock = GetIServiceProvider();
            var controller = new UserManagementController(_db)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        RequestServices = serviceProviderMock.Object,
                    }
                }
            };
            //What I'm Testing
            var result = controller.Login(new User()
            {
                UserName = "test",
                Password = "test"
            });
            
            //Assert - Object check actions && check view model
            result.Should().BeOfType<Task<IActionResult>>();

        }

        [Fact]
        public void UserManagement_CreateAccount_ReturnIActionResult()
        {
            var _db = GetDbContext();
            var controller = new UserManagementController(_db);

            var result = controller.Index();

            result.Should().BeOfType<ViewResult>();

        }




   

    }
}



