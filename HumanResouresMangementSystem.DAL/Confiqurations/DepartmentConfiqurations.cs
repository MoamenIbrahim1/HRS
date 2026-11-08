using HumanResouresMangementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.DAL.Confiqurations
{
   public class DepartmentConfiquration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.Id);

            
            builder.HasMany(d => d.Employees)
                   .WithOne(e => e.Department)
                   .HasForeignKey(e => e.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict); // يفضل منع الحذف التلقائي للموظفين عند حذف القسم
        }
    }
}
