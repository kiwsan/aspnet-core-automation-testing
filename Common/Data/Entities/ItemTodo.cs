using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Data.Entities
{
    public class ItemTodo : EntityBase
    {
        public string Name { get; set; }

        public ICollection<Item> TableItems { get; set; }
    }
}
