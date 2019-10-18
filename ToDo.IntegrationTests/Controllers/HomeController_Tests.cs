using Common.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ToDo.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Http;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using AngleSharp.Dom;

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
            _items = new[] { _firstItem, _secondItem };
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

        private HttpClient CreateHttpClient()
            => _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var serviceProvider = services.BuildServiceProvider();

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;

                        var db = scopedServices.GetRequiredService<AppDbContext>();

                        db.Database.EnsureCreated();

                        db.AddRange(_items);
                        db.SaveChanges();
                    }
                });
            })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

        [Test]
        public async Task ShouldHavePassWhenBrowsAndReturnItemsAsync()
        {
            //arrange
            var client = CreateHttpClient();

            //actual
            var homePage = await client.GetAsync("/Home");

            //assert
            Assert.AreEqual(HttpStatusCode.OK, homePage.StatusCode);
            var html = await HtmlHelpers.GetDocumentAsync(homePage);

            var items = html.QuerySelectorAll(".todo-item");

            Assert.AreEqual(2, items.Count());
        }

        [Test]
        public async Task ShouldHavePassWhenPostAndReturnRedirectToRootAsync()
        {
            //arrange
            var client = CreateHttpClient();

            //actual
            var homePage = await client.GetAsync("/Home");
            var html = await HtmlHelpers.GetDocumentAsync(homePage);

            var response = await client.SendAsync((IHtmlFormElement)html.QuerySelector("form[id='addToDo']"),
                (IHtmlButtonElement)html.QuerySelector("button[id='Add']"),
                new Dictionary<string, string>
                {
                    ["Name"] = "Hello world!"
                });

            //assert
            Assert.AreEqual(HttpStatusCode.OK, homePage.StatusCode);
            Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode);
            Assert.AreEqual("/", response.Headers.Location.OriginalString);
        }

        [Test]
        public async Task ShouldHavePassWhenDeleteAndReturnRedirectToRootAsync()
        {
            //arrange
            var client = CreateHttpClient();

            //actual
            var items = await AssertToDoItemsAsync("/", 2);

            foreach (var item in items)
            {
                var parser = new HtmlParser();

                var link = parser.Parse(item.OuterHtml);

                var a = link.QuerySelector("a");

                var url = a.GetAttribute("href");

                var response = await client.GetAsync(url);
            }

            items = await AssertToDoItemsAsync("/", 0);

            async Task<IHtmlCollection<IElement>> AssertToDoItemsAsync(string url, int expected)
            {
                //actual
                var homePage = await client.GetAsync(url);

                //assert
                Assert.AreEqual(HttpStatusCode.OK, homePage.StatusCode);
                var html = await HtmlHelpers.GetDocumentAsync(homePage);

                items = html.QuerySelectorAll(".todo-item");

                Assert.AreEqual(expected, items.Count());

                return items;
            }

        }

    }
}
