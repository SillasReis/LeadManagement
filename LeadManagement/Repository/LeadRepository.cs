using LeadManagement.Data;
using LeadManagement.Interfaces;
using LeadManagement.Models;

namespace LeadManagement.Repository;

public class LeadRepository : ILeadRepository
{
    private readonly LeadContext _context;

    public LeadRepository(LeadContext context)
    {
        _context = context;
    }

    public void CreateLead(Lead lead)
    {
        _context.Leads.Add(lead);
        _context.SaveChanges();
    }

    public Lead? GetLead(int id)
    {
        return _context.Leads.Where(lead => lead.Id == id).FirstOrDefault()!;
    }

    public void UpdateLead(Lead lead)
    {
        _context.Update(lead);
        _context.SaveChanges();
    }

    public List<Lead> ListLeads(int skip, int take)
    {

        return _context.Leads.Skip(skip).Take(take).ToList();
    }

    public List<Lead> ListLeadsByStatus(LeadStatus leadStatus, int skip, int take)
    {
        return _context.Leads.Where(lead => lead.Status == leadStatus).Skip(skip).Take(take).ToList();
    }

    public void DeleteLead(Lead lead)
    {
        _context.Remove(lead);
        _context.SaveChanges();
    }
}
