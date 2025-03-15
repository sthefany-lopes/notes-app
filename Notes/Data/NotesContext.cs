using Microsoft.EntityFrameworkCore;
using Notes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Data
{
    public class NotesContext : DbContext
    {
        public DbSet<Category> Category { get; set; } = default!;
        public DbSet<Note> Note { get; set; } = default!;

        public NotesContext (DbContextOptions<NotesContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Note>().HasOne(note => note.Category).WithMany(category => category.Notes).HasForeignKey(note => note.CategoryId).OnDelete(DeleteBehavior.SetNull);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            foreach (var entry in ChangeTracker.Entries<Note>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = entry.Entity.CreatedAt;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property("CreatedAt").IsModified = false;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}