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
    public class OvertimeSettingConfiguration : IEntityTypeConfiguration<OvertimeSetting>
    {
        public void Configure(EntityTypeBuilder<OvertimeSetting> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(x => x.Rate)
                   .HasPrecision(18, 2);
        }
    }
}
