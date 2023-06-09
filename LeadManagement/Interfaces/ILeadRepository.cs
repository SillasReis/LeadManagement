using LeadManagement.Models;

namespace LeadManagement.Interfaces;

public interface ILeadRepository
{
    public void CreateLead(Lead lead);
    public Lead? GetLead(int id);
    public void UpdateLead(Lead lead);
    public List<Lead> ListLeads(int skip, int take);
    public List<Lead> ListLeadsByStatus(LeadStatus leadStatus, int skip, int take);
    public void DeleteLead(Lead lead);
}
