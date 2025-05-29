using DreamDay.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DreamDay.DAL.Context
{
    public class DreamDayDbContext : DbContext
    {
        public DreamDayDbContext() { }
        public DreamDayDbContext(DbContextOptions<DreamDayDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Wedding> Weddings { get; set; }
        public DbSet<WeddingTask> WeddingTasks { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Agenda> Agendas { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }

        // New entities for advanced wedding planning
        public DbSet<GuestCategory> GuestCategories { get; set; }
        public DbSet<WeddingService> WeddingServices { get; set; }
        public DbSet<AgendaItem> AgendaItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // GuestCategory: many-to-one with Wedding
            modelBuilder.Entity<GuestCategory>()
                .HasOne(gc => gc.Wedding)
                .WithMany(w => w.GuestCategories)
                .HasForeignKey(gc => gc.WeddingId)
                .OnDelete(DeleteBehavior.Cascade);

            // WeddingService: many-to-one with Wedding
            modelBuilder.Entity<WeddingService>()
                .HasOne(ws => ws.Wedding)
                .WithMany(w => w.SelectedServices)
                .HasForeignKey(ws => ws.WeddingId)
                .OnDelete(DeleteBehavior.Cascade);

            // AgendaItem: many-to-one with Wedding
            modelBuilder.Entity<AgendaItem>()
                .HasOne(ai => ai.Wedding)
                .WithMany(w => w.Agenda)
                .HasForeignKey(ai => ai.WeddingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Booking: optional many-to-one with Wedding
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Wedding)
                .WithMany()
                .HasForeignKey(b => b.WeddingId)
                .OnDelete(DeleteBehavior.SetNull);

            // Agenda: many-to-one with Wedding
            modelBuilder.Entity<Agenda>()
                .HasOne(a => a.Wedding)
                .WithMany()
                .HasForeignKey(a => a.WeddingId)
                .OnDelete(DeleteBehavior.Cascade);

            // WeddingTask: many-to-one with Wedding
            modelBuilder.Entity<WeddingTask>()
                .HasOne(t => t.Wedding)
                .WithMany(w => w.Tasks)
                .HasForeignKey(t => t.WeddingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
