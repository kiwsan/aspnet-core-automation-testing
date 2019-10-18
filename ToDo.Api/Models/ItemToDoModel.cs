using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo.Api.Models
{
    public class ItemToDoModel
    {
        public Guid Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(200, ErrorMessage = "There's a 200 character limit on messages. Please shorten your message.")]
        public string Name { get; set; }
    }
}
