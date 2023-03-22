using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using reddit_miniProjekt.Shared.Models;

namespace reddit_miniProjekt.Server.Context
{
    public class RedditContext : DbContext
    {
        public DbSet<RedditThread> Threads { get; set; }
        public DbSet<User> Users { get; set; }

        public RedditContext(DbContextOptions<RedditContext> options) : base(options)
        {
            // Den her er tom. Men ": base(options)" sikre at constructor
            // på DbContext super-klassen bliver kaldt.
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RedditThread>().ToTable("Threads");
            modelBuilder.Entity<User>().ToTable("Users");
        }


    }
}



