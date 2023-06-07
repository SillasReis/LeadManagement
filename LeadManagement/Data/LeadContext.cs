using LeadManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LeadManagement.Data;

public class LeadContext : DbContext
{
    public LeadContext(DbContextOptions<LeadContext> opts): base(opts) { }

    public DbSet<Lead> Leads { get; set; }
}
