
using Domain.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public DbSet<FileModel> Files { get; set; }
        public DbSet<SharedFolder> SharedFolders { get; set; }
        public DbSet<AddRequest> AddRequests { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
           
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<FileModel>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<SharedFolder>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<ApplicationUser>()
                .HasMany(p => p.Files)
                .WithOne(p => p.Owner)
                .HasForeignKey(p => p.ApplicationUserId);
            builder.Entity<SharedFolder>()
                .HasOne(p => p.File)
                .WithOne(s => s.Shared)
                .HasForeignKey<SharedFolder>(p=>p.FileModelId);
                
            builder.Entity<SharedUser>().HasKey(p => new { p.UserId, p.FolderId });
            builder.Entity<SharedUser>().
                HasOne(p => p.Folder).
                WithMany(p => p.Users).
                HasForeignKey(p => p.UserId);
            builder.Entity<SharedUser>()
                .HasOne(p => p.User)
                .WithMany(p => p.SharedFiles)
                .HasForeignKey(p => p.UserId);
            builder.Entity<ApplicationUser>()
                .HasMany(p => p.OwnShared)
                .WithOne(p => p.User);
            builder.Entity<ApplicationUser>()
                .HasMany(p => p.Income)
                .WithOne(p => p.ToUser)
                .HasForeignKey(p => p.ToId);
            builder.Entity<ApplicationUser>()
                .HasMany(p => p.Sended)
                .WithOne(p => p.FromUser)
                .HasForeignKey(p => p.FromId);
            builder.Entity<SharedFolder>()
                .HasMany(p => p.Requests)
                .WithOne(p => p.Folder)
                .HasForeignKey(p => p.FolderId);
                

        }
    }
}
