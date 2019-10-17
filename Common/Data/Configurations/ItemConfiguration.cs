using Common.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Data.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Description);
            builder.Property(x => x.HasReminder);
            builder.Property(x => x.IsCompleted);

            builder.HasOne(x => x.TableItemToDo)
                   .WithMany(x => x.TableItems)
                   .HasForeignKey(s => s.ItemTodoId);
        }
    }
}
