using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Application.Services;
using Common.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToDo.RazorPages.ViewModels;

namespace ToDo.RazorPages.Pages
{
    public class IndexModel : PageModel
    {

        private readonly IItemToDoInMemoryService _itemToDoInMemoryService;
        public IndexModel(IItemToDoInMemoryService itemToDoInMemoryService)
        {
            _itemToDoInMemoryService = itemToDoInMemoryService;
        }

        [BindProperty]
        public ItemToDoViewModel ItemToDo { get; set; }

        public IList<ItemToDoViewModel> ItemToDos { get; set; }

        [TempData]
        public string Result { get; set; }

        public async Task OnGetAsync()
        {
            var items = await _itemToDoInMemoryService.BrowsAsync();

            ItemToDos = items.Select(x => new ItemToDoViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            if (!ModelState.IsValid)
            {
                var items = await _itemToDoInMemoryService.BrowsAsync();

                ItemToDos = items.Select(x => new ItemToDoViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();

                return Page();
            }

            await _itemToDoInMemoryService.AddAsync(new ItemTodo
            {
                Id = Guid.NewGuid(),
                Name = ItemToDo.Name
            });

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!ModelState.IsValid)
            {
                var items = await _itemToDoInMemoryService.BrowsAsync();

                ItemToDos = items.Select(x => new ItemToDoViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();

                return Page();
            }

            await _itemToDoInMemoryService.UpdateAsync(new ItemTodo
            {
                Id = ItemToDo.Id,
                Name = ItemToDo.Name
            });

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            await _itemToDoInMemoryService.DeleteAsync(id);

            return RedirectToPage();
        }

    }
}
