using DreamDay.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.DAL.Context
{
    public class DreamDayDbContext : DbContext
    {
        public DreamDayDbContext(DbContextOptions<DreamDayDbContext> options) : base(options) { }

         public DbSet<User> Users { get; set; }
    }
}
