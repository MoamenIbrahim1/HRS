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
    internal class GroupPremissionConfiquration : IEntityTypeConfiguration<GroupPermission>
    {
        
        public void Configure(EntityTypeBuilder<GroupPermission> builder)
        {
            builder.HasKey(x => new
            {
                x.GroupId,
                x.PermissionId
            });

            builder.HasOne(x => x.Group)
                .WithMany(x => x.GroupPermissions)
                .HasForeignKey(x => x.GroupId);

            builder.HasOne(x => x.Permission)
                .WithMany(x => x.GroupPermissions)
                .HasForeignKey(x => x.PermissionId);
        }
    }
    
}
