using Common.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Data.Configurations
{
    public class ItemTodoConfiguration : IEntityTypeConfiguration<ItemTodo>
    {
        public void Configure(EntityTypeBuilder<ItemTodo> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired();
        }
    }
}
