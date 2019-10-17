using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Common.Data.Entities;
using Common.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using ToDo.Mvc.Models;
using ToDo.Mvc.ViewModels;

namespace ToDo.Mvc.Controllers
{
    public class HomeController : Controller
    {

        private readonly IItemToDoRepository _itemToDoRepository;
        public HomeController(IItemToDoRepository itemToDoRepository)
        {
            _itemToDoRepository = itemToDoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _itemToDoRepository.BrowsAsync();

            return View(new ItemTodoViewModel
            {
                ItemToDos = model.Select(x => new ItemToDoModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
            });
        }

        [HttpPost]
        public async Task<IActionResult> Index(ItemToDoModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _itemToDoRepository.AddAsync(new ItemTodo
            {
                Id = Guid.NewGuid(),
                Name = model.Name
            });

            return RedirectToAction(actionName: nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (!id.HasValue)
            {
                return BadRequest(ModelState);
            }

            await _itemToDoRepository.DeleteAsync(id.Value);

            return RedirectToAction(actionName: nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
