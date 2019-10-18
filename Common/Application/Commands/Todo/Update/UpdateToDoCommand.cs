using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.Application.Commands.Todo.Update
{
    public class UpdateToDoCommand
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }

        [JsonConstructor]
        public UpdateToDoCommand(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
