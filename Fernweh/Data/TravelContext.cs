using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fernweh.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Xamarin.Forms;

namespace Fernweh.Data
{
    public sealed class TravelContext : DbContext
    {
        private const string DatabaseName = "database.db";

        public TravelContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<ItemCategory> Checklists { get; set; }
        public DbSet<Item> ChecklistItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string databasePath;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    Batteries_V2.Init();
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..",
                        "Library", DatabaseName);
                    break;
                case Device.Android:
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                        DatabaseName);
                    break;
                default:
                    throw new NotImplementedException("Platform not supported");
            }

            optionsBuilder.UseSqlite($"Filename={databasePath}");
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var now = DateTime.UtcNow;

            foreach (var changedEntity in ChangeTracker.Entries())
            {
                if (changedEntity.Entity is IEntityDate entity)
                {
                    switch (changedEntity.State)
                    {
                        case EntityState.Added:
                            entity.CreatedAt = now;
                            entity.UpdatedAt = now;
                            break;

                        case EntityState.Modified:
                            Entry(entity).Property(x => x.CreatedAt).IsModified = false;
                            entity.UpdatedAt = now;
                            break;
                    }
                }
            }

            return (await SaveChangesAsync(true, cancellationToken));
        }

    }
}