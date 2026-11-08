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
    public class EmployeeOvertimeConfiguration : IEntityTypeConfiguration<EmployeeOvertime>
    {
        public void Configure(EntityTypeBuilder<EmployeeOvertime> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Hours)
                   .HasPrecision(18, 2);

            builder.Property(x => x.HourRate)
                   .HasPrecision(18, 2);

            builder.Property(x => x.Amount)
                   .HasPrecision(18, 2);

            // العلاقة مع Employee
            builder.HasOne(x => x.Employee)
                   .WithMany(e => e.EmployeeOvertimes) // إن لم توجد Collection property في Employee استخدم .WithMany()
                   .HasForeignKey(x => x.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            // العلاقة مع OvertimeSetting
            builder.HasOne(x => x.OvertimeSetting)
                   .WithMany(s => s.EmployeeOvertimes) // إن لم توجد Collection property في OvertimeSetting استخدم .WithMany()
                   .HasForeignKey(x => x.OvertimeSettingId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
