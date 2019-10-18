using Common.Application.Commands.Todo.Create;
using Common.Application.Commands.Todo.Update;
using Common.Data.Entities;
using Common.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Api.Models;

namespace ToDo.Api.Controllers
{
    [Route("api/todos")]
    [ApiController]
    public class ToDosController : ControllerBase
    {
        private readonly IItemToDoRepository _itemToDoRepository;
        public ToDosController(IItemToDoRepository itemToDoRepository)
        {
            _itemToDoRepository = itemToDoRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAsync()
        {
            var model = await _itemToDoRepository.BrowsAsync();

            return Ok(model.Select(x => new ItemToDoModel
            {
                Id = x.Id,
                Name = x.Name
            }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var model = await _itemToDoRepository.GetAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return Ok(new ItemToDoModel
            {
                Id = model.Id,
                Name = model.Name
            });
        }

        [HttpPost()]
        public async Task<IActionResult> PostAsync(CreateToDoCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _itemToDoRepository.AddAsync(new ItemTodo
            {
                Id = Guid.NewGuid(),
                Name = command.Name
            });

            return Ok(command);
        }

        [HttpPut()]
        public async Task<IActionResult> PutAsync(UpdateToDoCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _itemToDoRepository.UpdateAsync(new ItemTodo
            {
                Id = command.Id,
                Name = command.Name
            });

            return Ok(command);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _itemToDoRepository.DeleteAsync(id);

            return Ok();
        }

    }
}
