using System;
using Destinataire.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Destinataire.Data
{
    public class DestinaireContext : DbContext
    {
        private readonly string _connectionString = "Data Source=Destinaire.db";

        public DestinaireContext():base()
        {
        }

        public DestinaireContext(string connectionString) : base()
        {
            _connectionString = connectionString;
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Message> Messages { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlite(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}