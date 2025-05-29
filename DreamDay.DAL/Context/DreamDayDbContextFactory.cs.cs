using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DreamDay.DAL.Context
{
    public class DreamDayDbContextFactory : IDesignTimeDbContextFactory<DreamDayDbContext>
    {
        public DreamDayDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DreamDayDbContext>();
            // Replace with your actual connection string
            optionsBuilder.UseSqlServer("Data Source=SHARADA\\SQLEXPRESS01;Initial Catalog=WeddingManagementSystem;Integrated Security=True;Trust Server Certificate=True");
            return new DreamDayDbContext(optionsBuilder.Options);
        }
        //Data Source = SHARADA\SQLEXPRESS01;Initial Catalog = WeddingManagementSystem; Integrated Security = True; Trust Server Certificate=True
    }
}
