using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPP
{
    public class InMemoryDbContext : DbContext
    {
        public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options)
                : base(options) { }
        public DbSet<DbModels.UserDbModel> Users { get; set; }

        public DbSet<DbModels.DepartmentDbModel> Departments{ get; set; }
    }
}
