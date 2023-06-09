using AutoMapper;
using LeadManagement.Data.Dtos;
using LeadManagement.Interfaces;
using LeadManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeadManagemet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeadController : ControllerBase
{
    private readonly ILeadRepository _leadRepository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;

    public LeadController(ILeadRepository leadRepository, IMapper mapper, IEmailService emailService)
    {
        _leadRepository = leadRepository;
        _mapper = mapper;
        _emailService = emailService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateLeadResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateLead([FromBody] CreateLeadDto leadDto)
    {
        try
        {
            Lead lead = _mapper.Map<Lead>(leadDto);
            _leadRepository.CreateLead(lead);
            return CreatedAtAction(nameof(GetLead), new { id = lead.Id }, lead);
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/accept")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult AcceptLead(int id)
    {
        try
        {
            Lead lead = _leadRepository.GetLead(id)!;

            if (lead == null) return NotFound();

            if (lead.Status != LeadStatus.ACCEPTED)
            {
                lead.Status = LeadStatus.ACCEPTED;
                lead.Discount = lead.Price > 500m ? 0.1m * lead.Price : 0m;

                _leadRepository.UpdateLead(lead);
                
                _emailService.SendEmail(
                    "vendas@test.com",
                    "Accepted Lead",
                    $"Lead {lead.Id} from {lead.ContactFullName} was accepted. | Description: {lead.Description}"
                );

                
            }

            return NoContent();
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/decline")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeclineLead(int id)
    {
        try
        {
            Lead lead = _leadRepository.GetLead(id)!;

            if (lead == null) return NotFound();

            if (lead.Status != LeadStatus.DECLINED)
            {
                lead.Status = LeadStatus.DECLINED;
                _leadRepository.UpdateLead(lead);
            }
            
            return NoContent();
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReadLeadDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult ListLeads([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        try
        {
            return Ok(_mapper.Map<List<ReadLeadDto>>(_leadRepository.ListLeads(skip, take)));
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("accepted")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReadAcceptedLeadDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult ListAcceptedLeads([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        try
        {
            var leads = _leadRepository.ListLeadsByStatus(LeadStatus.ACCEPTED, skip, take);

            var leadDto = _mapper.Map<List<ReadAcceptedLeadDto>>(leads);

            return Ok(leadDto);
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReadLeadDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetLead(int id)
    {
        try
        {
            Lead lead = _leadRepository.GetLead(id)!;

            if (lead == null) return NotFound();

            var leadDto = _mapper.Map<ReadLeadDto>(lead);

            return Ok(leadDto);
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UpdateLead(int id, [FromBody] UpdateLeadDto leadDto)
    {
        try
        {
            Lead lead = _leadRepository.GetLead(id)!;
        
            if (lead == null) return NotFound();

            _mapper.Map(leadDto, lead);
            
            _leadRepository.UpdateLead(lead);

            return NoContent();
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteLead(int id)
    {
        try
        {
            Lead lead = _leadRepository.GetLead(id)!;

            if (lead == null) return NotFound();

            _leadRepository.DeleteLead(lead);

            return NoContent();
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
