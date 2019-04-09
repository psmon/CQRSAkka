using DDDSample.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDDSample.Repository
{
    public class UserRepository : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserRepository(DbContextOptions<UserRepository> options)
:           base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //제약설정( Fluent API를 통해 확장 설정이 가능합니다. for POCO )
            //modelBuilder.Entity<TokenHistory>()
            //    .HasIndex(p => new { p.AuthToken })
            //    .IsUnique(true);
        }

        
    }
}
