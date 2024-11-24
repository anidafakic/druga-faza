using HangfireCleanup.Models;
using Microsoft.EntityFrameworkCore;

namespace HangfireCleanup.Data;

public class AppDbContext : DbContext
{
    public DbSet<Osoba> Osobe { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}

