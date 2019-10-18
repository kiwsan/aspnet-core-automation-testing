using Common.Application.Commands.Todo.Create;
using Common.Application.Commands.Todo.Update;
using Common.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Api.Controllers;
using ToDo.Api.Models;

namespace ToDo.UnitTests.ApiControllers
{
    [TestFixture]
    public class ToDoApiController_Tests : DbOptionBase
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
        public async Task ShouldHaveNotPassWhenPostModelStateIsInValidAndReturnBadRequestAsync()
        {
            //arrange
            var controller = new ToDosController(_itemToDoRepository.Object);

            controller.ModelState.AddModelError("error", "some error");

            //actual
            var result = await controller.PostAsync(null);

            //assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task ShouldHavePassWhenPostModelStateIsValidAndReturnOkAsync()
        {
            //arrange
            var controller = new ToDosController(_itemToDoRepository.Object);

            var command = new CreateToDoCommand("Hello world!");

            //actual
            var result = await controller.PostAsync(command);

            //assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var response = result as OkObjectResult;
            Assert.IsInstanceOf<CreateToDoCommand>(response.Value);

            var model = response.Value as CreateToDoCommand;
            Assert.AreEqual(command.Name, model.Name);
        }

        [Test]
        public async Task ShouldHavePassWhenBrowsIsValidAndReturnOkAsync()
        {
            //arrange
            _itemToDoRepository.Setup(repo => repo.BrowsAsync())
                .ReturnsAsync(_items);

            var controller = new ToDosController(_itemToDoRepository.Object);

            //actual
            var result = await controller.GetAsync();

            //assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var response = result as OkObjectResult;
            Assert.IsInstanceOf<IEnumerable<ItemToDoModel>>(response.Value);

            var model = response.Value as IEnumerable<ItemToDoModel>;
            Assert.IsTrue(model.Any(x => _items.Select(m => m.Id).Contains(x.Id)));
        }

        [Test]
        public async Task ShouldHavePassWhenGetIsValidAndReturnOkAsync()
        {
            //arrange
            _itemToDoRepository.Setup(repo => repo.GetAsync(_firstItem.Id))
                .ReturnsAsync(_items.FirstOrDefault(x => x.Id == _firstItem.Id));

            var controller = new ToDosController(_itemToDoRepository.Object);

            //actual
            var result = await controller.GetAsync(_firstItem.Id);

            //assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var response = result as OkObjectResult;
            Assert.IsInstanceOf<ItemToDoModel>(response.Value);

            var model = response.Value as ItemToDoModel;
            Assert.AreEqual(_firstItem.Id, model.Id);
        }

        [Test]
        public async Task ShouldHaveNotPassWhenGetIsInValidAndReturnNotFoundAsync()
        {
            //arrange
            var controller = new ToDosController(_itemToDoRepository.Object);

            //actual
            var result = await controller.GetAsync(Guid.Parse("74a1eedc-0719-408c-8c07-1bee8c2dd0b1"));

            //assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task ShouldHaveNotPassWhenPutModelStateIsInValidAndReturnBadRequestAsync()
        {
            //arrange
            var controller = new ToDosController(_itemToDoRepository.Object);

            controller.ModelState.AddModelError("error", "some error");

            //actual
            var result = await controller.PostAsync(null);

            //assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task ShouldHavePassWhenPutModelStateIsValidAndReturnOkAsync()
        {
            //arrange
            var controller = new ToDosController(_itemToDoRepository.Object);

            var command = new UpdateToDoCommand(_firstItem.Id, "Hello world!");

            //actual
            var result = await controller.PutAsync(command);

            //assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var response = result as OkObjectResult;
            Assert.IsInstanceOf<UpdateToDoCommand>(response.Value);

            var model = response.Value as UpdateToDoCommand;
            Assert.AreEqual(command.Name, model.Name);
        }

        [Test]
        public async Task ShouldHavePassWhenDeleteModelStateIsValidAndReturnOkAsync()
        {
            //arrange
            var controller = new ToDosController(_itemToDoRepository.Object);

            //actual
            var result = await controller.DeleteAsync(_firstItem.Id);

            //assert
            Assert.IsInstanceOf<OkResult>(result);
        }

    }
}
