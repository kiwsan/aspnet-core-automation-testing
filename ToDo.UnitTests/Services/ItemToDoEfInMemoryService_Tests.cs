using Common.Application.Services;
using Common.Data;
using Common.Data.Entities;
using Common.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.UnitTests.Services
{
    [TestFixture]
    public class ItemToDoEfInMemoryService_Tests : DbOptionBase
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
        public async Task ShouldHavePassWhenGetAsyncReturnCurrectItem()
        {
            //actual
            var actualItem = await _itemToDoService.GetAsync(_firstItem.Id);

            //assert
            Assert.AreEqual(_firstItem.Id, actualItem.Id);
        }

        [Test]
        public async Task ShouldHavePassWhenBrowsAsyncReturnCurrectItem()
        {
            //actual
            var actualItems = await _itemToDoService.BrowsAsync();

            //assert
            Assert.That(actualItems.Any(x => x.Id == _firstItem.Id));
            Assert.That(actualItems.Any(x => x.Id == _secondItem.Id));
            Assert.AreEqual(_items.Count(), actualItems.Count());
        }

        [Test]
        public async Task ShouldHavePassWhenAddAsyncItem()
        {
            //arrange
            var thirdItem = new ItemTodo() { Id = Guid.Parse("74a1eedc-0719-408c-8c07-1bee8c2dd0b1"), Name = "Third Item" };
            await _itemToDoService.AddAsync(thirdItem);

            //actual
            var actualItems = await _itemToDoService.BrowsAsync();

            //assert
            Assert.That(actualItems.Any(x => x.Id == thirdItem.Id));
            Assert.AreEqual(3, actualItems.Count());
        }
        
        [TestCase("testname")]
        [TestCase("TESTNAME")]
        [TestCase("Ein.Test")]
        [TestCase("$%)/)(&")]
        public async Task ShouldHavePassWhenUpdateAsyncItemChange(string expectedName)
        {
            //arrange
            var entity = await _itemToDoService.GetAsync(_firstItem.Id);

            entity.Name = expectedName;

            await _itemToDoService.UpdateAsync(_firstItem);

            //actual
            var actualItem = await _itemToDoService.GetAsync(_firstItem.Id);

            //assert
            Assert.AreEqual(expectedName, actualItem.Name);
        }
        
        [Test]
        public async Task ShouldHavePassWhenDeleteAsyncItem()
        {
            //arrange
            await _itemToDoService.DeleteAsync(_firstItem.Id);

            //actual
            var actualItems = await _itemToDoService.BrowsAsync();

            //assert
            Assert.That(!actualItems.Any(x => x.Id == _firstItem.Id));
        }

    }
}
