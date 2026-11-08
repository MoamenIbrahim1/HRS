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
    public class WorkDataConfiguration : IEntityTypeConfiguration<WorkData>
    {
        public void Configure(EntityTypeBuilder<WorkData> builder)
        {
            builder.HasKey(w => w.Id);

          

        
        }
    }
}
