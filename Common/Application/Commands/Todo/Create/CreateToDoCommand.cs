using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.Application.Commands.Todo.Create
{
    public class CreateToDoCommand
    {

        [Required]
        public string Name { get; set; }

        [JsonConstructor]
        public CreateToDoCommand(string name)
        {
            Name = name;
        }

    }
}
