using LawFirmDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LawFirmDatabaseImplement
{
    class LawFirmDatabase: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LawFirmDatabaseRez;Integrated Security=True;MultipleActiveResultSets=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }
        public virtual DbSet<Blank> Blanks { set; get; }
        public virtual DbSet<Document> Documents { set; get; }
        public virtual DbSet<DocumentBlank> DocumentBlanks { set; get; }
        public virtual DbSet<Order> Orders { set; get; }
        public virtual DbSet<Storage> Storages { set; get; }
        public virtual DbSet<StorageBlank> StorageBlanks { set; get; }
    }
}
