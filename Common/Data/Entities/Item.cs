using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Data.Entities
{
    public class Item : EntityBase
    {
        public Guid ItemTodoId { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public bool HasReminder { get; set; }

        public ItemTodo TableItemToDo { get; set; }
    }
}
