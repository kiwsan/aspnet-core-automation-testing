using Common.Data;
using Common.Data.Entities;
using Common.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Application.Services
{

    public interface IItemToDoInMemoryService
    {
        Task<ItemTodo> GetAsync(Guid id);
        Task<IEnumerable<ItemTodo>> BrowsAsync();
        Task AddAsync(ItemTodo item);
        Task UpdateAsync(ItemTodo item);
        Task DeleteAsync(Guid id);
    }

    //Data tranform object & business logic
    public class ItemToDoInMemoryService : IItemToDoInMemoryService
    {
        private readonly IItemToDoRepository _itemToDoRepository;
        public ItemToDoInMemoryService(IItemToDoRepository itemToDoRepository)
        {
            this._itemToDoRepository = itemToDoRepository ?? throw new ArgumentNullException(nameof(itemToDoRepository));
        }

        public async Task AddAsync(ItemTodo item)
            => await _itemToDoRepository.AddAsync(item);

        public async Task<IEnumerable<ItemTodo>> BrowsAsync()
            => await _itemToDoRepository.BrowsAsync();

        public async Task DeleteAsync(Guid id)
            => await _itemToDoRepository.DeleteAsync(id);

        public async Task<ItemTodo> GetAsync(Guid id)
            => await _itemToDoRepository.GetAsync(id);

        public async Task UpdateAsync(ItemTodo item)
            => await _itemToDoRepository.UpdateAsync(item);

    }
}
