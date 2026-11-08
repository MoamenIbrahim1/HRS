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
    public class EmployeeDeductionConfiguration : IEntityTypeConfiguration<EmployeeDeduction>
    {
        public void Configure(EntityTypeBuilder<EmployeeDeduction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Amount)
                   .HasPrecision(18, 2);

            // العلاقة مع Employee
            builder.HasOne(x => x.Employee)
                   .WithMany(e => e.EmployeeDeductions) // إن لم توجد Collection property في Employee استخدم .WithMany()
                   .HasForeignKey(x => x.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            // العلاقة مع DeductionSetting
            builder.HasOne(x => x.DeductionSetting)
                   .WithMany(s => s.employeeDeductions) // إن لم توجد Collection property في DeductionSetting استخدم .WithMany()
                   .HasForeignKey(x => x.DeductionSettingId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
