using Common.Data;
using Common.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Mvc.Controllers;
using ToDo.Mvc.Models;
using ToDo.Mvc.ViewModels;

namespace ToDo.UnitTests.Controllers
{
    [TestFixture]
    public class HomeController_Tests : DbOptionBase
    {
        private Mock<IItemToDoRepository> _itemToDoRepository;

        [SetUp]
        public void Init()
        {
            //arrange
            _itemToDoRepository = new Mock<IItemToDoRepository>();

            _items = new[] { _firstItem, _secondItem };
        }

        [Test]
        public async Task ShouldHavePassWhenGetIndexReturnAViewResultAsync()
        {
            //arrange
            _itemToDoRepository.Setup(repo => repo.BrowsAsync())
                .ReturnsAsync(_items);

            var controller = new HomeController(_itemToDoRepository.Object);

            //actual
            var result = await controller.Index();

            //assert
            Assert.IsInstanceOf<ViewResult>(result);

            var view = result as ViewResult;

            Assert.IsAssignableFrom<ItemTodoViewModel>(view.Model);

            var model = view.Model as ItemTodoViewModel;

            Assert.AreEqual(2, model.ItemToDos.Count());
        }

        [Test]
        public async Task ShouldHaveNotPassWhenPostIndexIsInValidModelStateReturnBadRequestResultAsync()
        {
            //arrange
            _itemToDoRepository.Setup(repo => repo.BrowsAsync())
                .ReturnsAsync(_items);

            var controller = new HomeController(_itemToDoRepository.Object);

            controller.ModelState.AddModelError("model.Name", "error happened");

            var command = new ItemToDoModel();

            //actual
            var result = await controller.Index(command);

            //assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequest = result as BadRequestObjectResult;

            Assert.IsInstanceOf<SerializableError>(badRequest.Value);
        }

    }
}
