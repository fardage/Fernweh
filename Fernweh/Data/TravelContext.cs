using System;
using System.IO;
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
                    ;
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
    }
}