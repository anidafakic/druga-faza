using HangfireCleanup.Data;
using System;
using System.Linq;

namespace HangfireCleanup.Services;

public class CleanupService
{
    private readonly AppDbContext _context;

    public CleanupService(AppDbContext context)
    {
        _context = context;
    }

    public void ObrisiStareKorisnike()
    {
        var granicniDatum = DateTime.Now.AddMonths(-6);
        var stareOsobe = _context.Osobe
                                 .Where(o => o.DatumVremePoslednjegLogovanja < granicniDatum)
                                 .ToList();

        _context.Osobe.RemoveRange(stareOsobe);
        _context.SaveChanges();

        Console.WriteLine($"Obrisano {stareOsobe.Count} starih korisnika.");
    }
}

