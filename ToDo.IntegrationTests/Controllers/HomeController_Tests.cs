using Common.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

//https://www.automatetheplanet.com/nunit-cheat-sheet/
namespace ToDo.IntegrationTests.Controllers
{
    [TestFixture]
    public class HomeController_Tests : DbOptionBase
    {

        private MvcAppFactory _factory;
        [OneTimeSetUp]
        public void SetUp()
        {
            _factory = new MvcAppFactory();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _factory.Dispose();
        }

        [SetUp]
        public void Init()
        {

        }

        [TestCase("/")]
        [TestCase("/Home")]
        [TestCase("/Home/Privacy")]
        public async Task ShouldHavePassWhenEnpointReturnSuccessAndCurrectContentTypeAsync(string url)
        {
            //arrange
            var client = _factory.CreateClient();

            //actual
            var response = await client.GetAsync(url);

            //assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Test]
        public void ShouldHaveRedirectToAuthWhenGetSecurePage()
        {
            //arrange
            var client = _factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            //actual
            //var response = await client.GetAsync("Home/Secure");

            //assert
            //Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode);
            //Assert.That(response.Headers.Location.OriginalString.StartsWith("http://localhost/Identity/Account/Login"));
        }

    }
}
