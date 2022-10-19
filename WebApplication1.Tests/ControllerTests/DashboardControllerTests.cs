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
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Configuration;
using System.Collections;

namespace WebApplication1.Tests.ControllerTests
{
    public class DashboardControllerTests
    {

       
        public ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated(); ;

            databaseContext.UserAnime.Add(new UserAnime
            {
                AnimeId = 49776,
                AnimeDateAdded = new DateTime(),
                UserName = "Test"

            });
            databaseContext.SaveChangesAsync();

            return databaseContext;

        }

        public IConfiguration GetUserSecrets()
        {
            var configuration = new ConfigurationBuilder().AddUserSecrets("82c10d77-d87f-45c5-9848-4b43a0b809ac").Build();
            return configuration;
        }
        

        [Fact]
        public async void GetAnimeHistory()
        {
            //Assert
            var _db = GetDbContext();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Test"),
                new Claim(ClaimTypes.Role,"user")
            };
            IConfiguration _config = GetUserSecrets();
            IMyAnimeClient _myAnimeClient = new MyAnimeClient(new HttpClient(), _config);
            IJikanApiClient _jikanApiClient = new JikanApiClient();


            var userIdentity = new ClaimsIdentity(claims, "TestCookie");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(userIdentity);


            var controller = new DashboardController(_db, _jikanApiClient, _myAnimeClient)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
                }
            };
            //Action
            var result = await controller.AnimeHistory();
            PartialViewResult result2 = (PartialViewResult)result;
            var animeInfoTest = result2.Model;
            
            //Assert
            animeInfoTest.Should().NotBeNull();
            Assert.Equal("_AnimeHistoryPartial",result2.ViewName);
            animeInfoTest.Should().BeOfType<List<AnimeHistoryModel>>();
            
        }

    }
}
