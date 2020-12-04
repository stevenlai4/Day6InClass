using Day4InClass.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Day4InClass.Data
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options) { }
        public DbSet<ToDo> ToDos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDo>().HasData(
                new { Id = 1, Description = "Bake Cookies", IsComplete = false, Priority = 1, CreatedOn = DateTime.Now },
                new { Id = 2, Description = "Eat the cookies", IsComplete = false, Priority = 1, CreatedOn = DateTime.Now },
                new { Id = 3, Description = "Run around the block", IsComplete = false, Priority = 1, CreatedOn = DateTime.Now });
        }
    }
}
