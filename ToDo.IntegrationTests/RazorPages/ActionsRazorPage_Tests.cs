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

namespace ToDo.IntegrationTests.RazorPages
{
    [TestFixture]
    public class ActionsRazorPage_Tests : DbOptionBase
    {
        private RazorPageAppFactory _factory;
        [OneTimeSetUp]
        public void SetUp()
        {
            _factory = new RazorPageAppFactory();
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
        [TestCase("/Index")]
        [TestCase("/Privacy")]
        public async Task ShouldHavePassWhenEnpointReturnSuccessAndCurrectContentTypeAsync(string url)
        {
            //arrange
            var client = _factory.CreateClient();

            //actual
            var response = await client.GetAsync(url);

            //assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
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
            var homePage = await client.GetAsync("/");

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
            var homePage = await client.GetAsync("/");
            var html = await HtmlHelpers.GetDocumentAsync(homePage);

            var response = await client.SendAsync((IHtmlFormElement)html.QuerySelector("form[id='addToDo']"),
                (IHtmlButtonElement)html.QuerySelector("button[id='Add']"),
                new Dictionary<string, string>
                {
                    ["ItemToDo.Name"] = "Hello world!"
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
            var homePage = await client.GetAsync("/");
            var html = await HtmlHelpers.GetDocumentAsync(homePage);

            var response = await client.SendAsync((IHtmlFormElement)html.QuerySelector("form[id='todos']"),
                (IHtmlButtonElement)html.QuerySelector("form[id='todos']")
                .QuerySelector("div[class='panel-body']")
                .QuerySelector("button"));

            //assert
            Assert.AreEqual(HttpStatusCode.OK, homePage.StatusCode);
            Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode);
            Assert.AreEqual("/", response.Headers.Location.OriginalString);
        }

    }
}
