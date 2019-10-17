using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Mvc.Models;

namespace ToDo.Mvc.ViewModels
{
    public class ItemTodoViewModel
    {
        public IEnumerable<ItemToDoModel> ItemToDos { get; set; }
    }
}
