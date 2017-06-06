using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBuilder.Models;

namespace TeamBuilder.Data.Configurations
{
    class TeamConfiguration : EntityTypeConfiguration<Team>
    {
        public TeamConfiguration()
        {
            this.Property(u => u.Name).HasMaxLength(25).IsRequired();
            this.Property(u => u.Description).HasMaxLength(32);
            this.Property(u => u.Acronym).IsFixedLength().HasMaxLength(3).IsRequired();          
        }
    }
}
