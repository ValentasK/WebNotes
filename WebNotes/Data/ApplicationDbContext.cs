using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebNotes.Models;

namespace WebNotes.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<WebNotes.Models.Note> Note { get; set; }
        public DbSet<WebNotes.Models.Admin> Admins { get; set; }
        public DbSet<WebNotes.Models.SharedNote> SharedNotes { get; set; }
        public DbSet<WebNotes.Models.PublicNote> PublicNotes { get; set; }

    }
}
