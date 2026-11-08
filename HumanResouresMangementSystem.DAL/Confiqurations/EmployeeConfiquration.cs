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
    public class EmployeeConfiquration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasOne(e => e.WorkData).
                 WithOne(e => e.Employee)
                 .HasForeignKey<WorkData>(w => w.EmployeeId);
  //          builder.ToTable(t => t.HasCheckConstraint(
  //    "CK_Employee_Age_Min20",
  //    "DATEDIFF(YEAR, [BirthDate], GETDATE()) >= 20"
  //));

        }
    }
    
}
