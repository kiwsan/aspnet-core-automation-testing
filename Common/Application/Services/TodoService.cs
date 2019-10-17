using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Application.Services
{

    public interface ITodoService
    {

        bool IsToDo(int id);

    }

    public class TodoService : ITodoService
    {
        public bool IsToDo(int id)
        {
            if (id == 1)
            {
                return true;
            }

            throw new NotImplementedException("Please create a test first.");
        }
    }
}
