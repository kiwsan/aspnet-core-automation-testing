using Common.Application.Services;
using Common.Data;
using Common.Data.Entities;
using Common.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDo.RazorPages.Pages;
using ToDo.RazorPages.ViewModels;

namespace ToDo.UnitTests.RazorPages
{
    [TestFixture]
    public class IndexRazorPage_Tests : DbOptionBase
    {

        private AppDbContext _dbContext;
        private IItemToDoInMemoryService _itemToDoService;

        [SetUp]
        public void Init()
        {
            //arrange
            _dbContext = new AppDbContext(InMemoryWithEfDatabase());
            _dbContext.Database.EnsureCreated();

            var repository = new Mock<ItemToDoRepository>(_dbContext);

            _itemToDoService = new ItemToDoInMemoryService(repository.Object);

            _items = new[] { _firstItem, _secondItem };

            foreach (var item in _items)
            {
                _itemToDoService.AddAsync(item);
            }
        }

        [Test]
        public async Task ShouldHavePassWhenPopulatesThePageModelWithAListAsync()
        {

            //arrange
            var model = new IndexModel(_itemToDoService);

            //actual
            await model.OnGetAsync();

            //assert
            Assert.IsAssignableFrom<List<ItemToDoViewModel>>(model.ItemToDos);
        }

        [Test]
        public async Task ShouldHavePassWhenAddItemAndModelStateIsInValidReturnAPageResultAsync()
        {
            //arrange
            var httpContext = new DefaultHttpContext();
            var modelState = new ModelStateDictionary();
            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            var modelMetadataProvider = new EmptyModelMetadataProvider();
            var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            var ctx = new PageContext(actionContext)
            {
                ViewData = viewData
            };

            var model = new IndexModel(_itemToDoService)
            {
                PageContext = ctx,
                TempData = tempData,
                Url = new UrlHelper(actionContext)
            };

            model.ModelState.AddModelError("ItemToDo.Name", "The Text field is required."); //set invalid model

            //actual
            var result = await model.OnPostAddAsync();

            //assert
            Assert.IsInstanceOf<PageResult>(result);
        }

        [Test]
        public async Task ShouldHavePassWhenAddItemAndModelStateIsValidReturnARedirectToPageResultAsync()
        {
            //arrange
            var httpContext = new DefaultHttpContext();
            var modelState = new ModelStateDictionary();
            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            var modelMetadataProvider = new EmptyModelMetadataProvider();
            var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            var ctx = new PageContext(actionContext)
            {
                ViewData = viewData
            };

            var model = new IndexModel(_itemToDoService)
            {
                PageContext = ctx,
                TempData = tempData,
                Url = new UrlHelper(actionContext)
            };

            //actual
            model.ItemToDo = new ItemToDoViewModel
            {
                Name = "Hello world!"
            };

            var result = await model.OnPostAddAsync();

            //assert
            Assert.IsInstanceOf<RedirectToPageResult>(result);
        }

        [Test]
        public async Task ShouldHavePassWhenDeleteAsyncReturnARedirectToResultAsync()
        {
            //arrange
            var model = new IndexModel(_itemToDoService);

            //actual
            var result = await model.OnPostDeleteAsync(_firstItem.Id);

            //assert
            Assert.IsInstanceOf<RedirectToPageResult>(result);
        }

    }
}
