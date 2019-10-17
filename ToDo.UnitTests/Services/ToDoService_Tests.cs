using Common.Application.Services;
using NUnit.Framework;
using System;

namespace ToDo.UnitTests.Services
{
    public class ToDoService_Tests
    {

        private ITodoService _todoService;

        [SetUp]
        public void Setup()
        {
            _todoService = new TodoService();
        }

        [TestCase(1)]
        public void ShouldHavePassIsToDoWhenInputIs1ReturnTrue(int value)
        {
            var result = _todoService.IsToDo(value);

            Assert.True(result, $"{value} Should be ToDo");
        }
    }
}