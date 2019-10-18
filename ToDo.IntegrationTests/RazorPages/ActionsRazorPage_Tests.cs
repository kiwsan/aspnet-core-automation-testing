using Common.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        }

        [SetUp]
        public void Init()
        {

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
    }
}
