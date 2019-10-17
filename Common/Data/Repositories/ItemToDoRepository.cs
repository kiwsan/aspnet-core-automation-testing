using Common.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.Repositories
{
    public interface IItemToDoRepository
    {
        Task<ItemTodo> GetAsync(Guid id);
        Task<IEnumerable<ItemTodo>> BrowsAsync();
        Task AddAsync(ItemTodo item);
        Task UpdateAsync(ItemTodo item);
        Task DeleteAsync(Guid id);
    }

    public class ItemToDoRepository : IItemToDoRepository
    {
        private readonly AppDbContext _dbContext;
        public ItemToDoRepository(AppDbContext dbContext)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task AddAsync(ItemTodo item)
        {
            await _dbContext.ItemTodos.AddAsync(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ItemTodo>> BrowsAsync()
            => await _dbContext.ItemTodos.AsNoTracking().Select(s => s).ToListAsync();

        public async Task DeleteAsync(Guid id)
        {
            var item = await _dbContext.ItemTodos.FindAsync(id);
            _dbContext.ItemTodos.Remove(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ItemTodo> GetAsync(Guid id)
            => await _dbContext.ItemTodos.FindAsync(id);

        public async Task UpdateAsync(ItemTodo item)
        {
            var entity = await _dbContext.ItemTodos.FindAsync(item.Id);
            entity.Name = item.Name;
            await _dbContext.SaveChangesAsync();
        }
    }
}
