using GymSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Configurations
{
    public class PlanConfigurations : IEntityTypeConfiguration<Plan>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Plan> builder)
        {
            builder.Property(P => P.Name).HasColumnType("varchar(100)").HasMaxLength(50);
            builder.Property(P => P.Description).HasMaxLength(200);
            builder.Property(P => P.Price).HasPrecision(10, 2);
            builder.Property(P => P.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("DurationCheckValue", "Duration Between 1 and 365");
            });


        }
    }
}
