using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using LeadManagement.Interfaces;
using FakeItEasy;
using Xunit;
using LeadManagement.Data.Dtos;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using LeadManagement.Models;

namespace LeadManagemet.Controllers.Tests;

[TestClass()]
public class LeadControllerTests
{
    private readonly ILeadRepository _leadRepository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;

    public LeadControllerTests()
    {
        _leadRepository = A.Fake<ILeadRepository>();
        _mapper = A.Fake<IMapper>();
        _emailService = A.Fake<IEmailService>();
    }

    [Fact]
    public void LeadController_CreateLead_ReturnOK()
    {
        var lead = A.Fake<Lead>();
        var leadDto = A.Fake<CreateLeadDto>();

        A.CallTo(() => _mapper.Map<Lead>(leadDto)).Returns(lead);

        var controller = new LeadController(_leadRepository, _mapper, _emailService);

        var result = controller.CreateLead(leadDto);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(CreatedAtActionResult));
        A.CallTo(() => _mapper.Map<Lead>(leadDto)).MustHaveHappened();
        A.CallTo(() => _leadRepository.CreateLead(lead)).MustHaveHappened();
    }

    [Fact]
    public void LeadController_CreateLead_ReturnBadRequest()
    {
        var lead = A.Fake<Lead>();
        var leadDto = A.Fake<CreateLeadDto>();

        A.CallTo(() => _mapper.Map<Lead>(leadDto)).Returns(lead);
        A.CallTo(() => _leadRepository.CreateLead(lead)).Throws(() => new Exception("Fake exception"));

        var controller = new LeadController(_leadRepository, _mapper, _emailService);

        var result = controller.CreateLead(leadDto);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(BadRequestObjectResult));
        A.CallTo(() => _mapper.Map<Lead>(leadDto)).MustHaveHappened();
        A.CallTo(() => _leadRepository.CreateLead(lead)).MustHaveHappened();
    }

    [Fact]
    public void LeadController_AcceptLead_ReturnsNoContentAndApplyDiscount()
    {
        var lead = A.Fake<Lead>();
        lead.Status = LeadStatus.INVITED;
        lead.Price = 500.10m;

        A.CallTo(() => _leadRepository.GetLead(lead.Id)).Returns(lead);

        var controller = new LeadController(_leadRepository, _mapper, _emailService);

        var result = controller.AcceptLead(lead.Id);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(NoContentResult));
        lead.Status.Should().Be(LeadStatus.ACCEPTED);
        lead.Discount.Should().Be(50.01m);
        A.CallTo(() => _leadRepository.GetLead(lead.Id)).MustHaveHappened();
        A.CallTo(() => _leadRepository.UpdateLead(lead)).MustHaveHappened();
        A.CallTo(() => _emailService.SendEmail(
            "vendas@test.com",
            "Accepted Lead",
            $"Lead {lead.Id} from {lead.ContactFullName} was accepted. | Description: {lead.Description}"
        )).MustHaveHappened();
    }
    
    [Fact]
    public void LeadController_AcceptLead_ReturnsNoContentAndDontApplyDiscount()
    {
        var lead = A.Fake<Lead>();
        lead.Status = LeadStatus.INVITED;
        lead.Price = 450.98m;

        A.CallTo(() => _leadRepository.GetLead(lead.Id)).Returns(lead);

        var controller = new LeadController(_leadRepository, _mapper, _emailService);

        var result = controller.AcceptLead(lead.Id);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(NoContentResult));
        lead.Status.Should().Be(LeadStatus.ACCEPTED);
        lead.Discount.Should().Be(0m);
        A.CallTo(() => _leadRepository.GetLead(lead.Id)).MustHaveHappened();
        A.CallTo(() => _leadRepository.UpdateLead(lead)).MustHaveHappened();
        A.CallTo(() => _emailService.SendEmail(
            "vendas@test.com",
            "Accepted Lead",
            $"Lead {lead.Id} from {lead.ContactFullName} was accepted. | Description: {lead.Description}"
        )).MustHaveHappened();
    }
    
    [Fact]
    public void LeadController_AcceptLead_ReturnsNoContentAndDontUpdate()
    {
        var lead = A.Fake<Lead>();
        lead.Status = LeadStatus.ACCEPTED;
        lead.Price = 450.98m;

        A.CallTo(() => _leadRepository.GetLead(lead.Id)).Returns(lead);

        var controller = new LeadController(_leadRepository, _mapper, _emailService);

        var result = controller.AcceptLead(lead.Id);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(NoContentResult));
        A.CallTo(() => _leadRepository.GetLead(lead.Id)).MustHaveHappened();
        A.CallTo(() => _leadRepository.UpdateLead(A<Lead>.Ignored)).MustNotHaveHappened();
        A.CallTo(() => _emailService.SendEmail(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored)).MustNotHaveHappened();
    }

    [Fact]
    public void LeadController_AcceptLead_ReturnsNotFound()
    {
        A.CallTo(() => _leadRepository.GetLead(1)).Returns(null);

        var controller = new LeadController(_leadRepository, _mapper, _emailService);

        var result = controller.AcceptLead(1);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(NotFoundResult));
        A.CallTo(() => _leadRepository.GetLead(1)).MustHaveHappened();
        A.CallTo(() => _leadRepository.UpdateLead(A<Lead>.Ignored)).MustNotHaveHappened();
        A.CallTo(() => _emailService.SendEmail(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored)).MustNotHaveHappened();
    }
    
    [Fact]
    public void LeadController_AcceptLead_ReturnsBadRequest()
    {
        var lead = A.Fake<Lead>();
        lead.Status = LeadStatus.INVITED;
        lead.Price = 450.98m;

        A.CallTo(() => _leadRepository.GetLead(lead.Id)).Returns(lead);
        A.CallTo(() => _leadRepository.UpdateLead(lead)).Throws(() => new Exception("Fake exception"));

        var controller = new LeadController(_leadRepository, _mapper, _emailService);

        var result = controller.AcceptLead(lead.Id);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(BadRequestObjectResult));
        A.CallTo(() => _leadRepository.GetLead(lead.Id)).MustHaveHappened();
        A.CallTo(() => _leadRepository.UpdateLead(A<Lead>.Ignored)).MustHaveHappened();
        A.CallTo(() => _emailService.SendEmail(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored)).MustNotHaveHappened();
    }

    [Fact]
    public void LeadController_DeclineLead_ReturnsNoContentAndUpdateStatus()
    {
        var lead = A.Fake<Lead>();
        lead.Status = LeadStatus.INVITED;

        A.CallTo(() => _leadRepository.GetLead(lead.Id)).Returns(lead);

        var controller = new LeadController(_leadRepository, _mapper, _emailService);

        var result = controller.DeclineLead(lead.Id);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(NoContentResult));
        lead.Status.Should().Be(LeadStatus.DECLINED);
        A.CallTo(() => _leadRepository.UpdateLead(lead)).MustHaveHappened();
    }

    [Fact]
    public void LeadController_DeclineLead_ReturnsNoContentAndDontUpdate()
    {
        var lead = A.Fake<Lead>();
        lead.Status = LeadStatus.DECLINED;

        A.CallTo(() => _leadRepository.GetLead(lead.Id)).Returns(lead);

        var controller = new LeadController(_leadRepository, _mapper, _emailService);

        var result = controller.DeclineLead(lead.Id);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(NoContentResult));
        A.CallTo(() => _leadRepository.UpdateLead(A<Lead>.Ignored)).MustNotHaveHappened();
    }

    [Fact]
    public void LeadController_DeclineLead_ReturnsNotFound()
    {
        A.CallTo(() => _leadRepository.GetLead(1)).Returns(null);

        var controller = new LeadController(_leadRepository, _mapper, _emailService);

        var result = controller.DeclineLead(1);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(NotFoundResult));
        A.CallTo(() => _leadRepository.UpdateLead(A<Lead>.Ignored)).MustNotHaveHappened();
    }

    [Fact]
    public void LeadController_DeclineLead_ReturnsBadRequest()
    {
        var lead = A.Fake<Lead>();
        lead.Status = LeadStatus.INVITED;

        A.CallTo(() => _leadRepository.GetLead(lead.Id)).Returns(lead);
        A.CallTo(() => _leadRepository.UpdateLead(lead)).Throws(new Exception("Fake exception"));

        var controller = new LeadController(_leadRepository, _mapper, _emailService);

        var result = controller.DeclineLead(lead.Id);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(BadRequestObjectResult));
        A.CallTo(() => _leadRepository.UpdateLead(lead)).MustHaveHappened();
    }

    [Fact]
    public void LeadController_ListLeads_ReturnOK()
    {
        var leads = A.Fake<List<Lead>>();
        var leadsDto = A.Fake<List<ReadLeadDto>>();

        A.CallTo(() => _leadRepository.ListLeads(A<int>.Ignored, A<int>.Ignored)).Returns(leads);
        A.CallTo(() => _mapper.Map<List<ReadLeadDto>>(leads)).Returns(leadsDto);

        var controller = new LeadController(_leadRepository, _mapper, _emailService);

        var result = controller.ListLeads();
        var okResult = result as OkObjectResult;

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(OkObjectResult));
        okResult?.Value.Should().Be(leadsDto);
    }

    [Fact]
    public void LeadController_ListLeads_ReturnBadRequest()
    {
        var leads = A.Fake<List<Lead>>();
        var leadsDto = A.Fake<List<ReadLeadDto>>();

        A.CallTo(() => _leadRepository.ListLeads(A<int>.Ignored, A<int>.Ignored)).Throws(() => new Exception("Fake exception"));

        var controller = new LeadController(_leadRepository, _mapper, _emailService);

        var result = controller.ListLeads();

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(BadRequestObjectResult));
    }

    [Fact]
    public void LeadController_ListAcceptedLeads_ReturnOK()
    {
        var leads = A.Fake<List<Lead>>();
        var leadsDto = A.Fake<List<ReadAcceptedLeadDto>>();

        A.CallTo(() => _leadRepository.ListLeadsByStatus(A<LeadStatus>.Ignored, A<int>.Ignored, A<int>.Ignored)).Returns(leads);
        A.CallTo(() => _mapper.Map<List<ReadAcceptedLeadDto>>(leads)).Returns(leadsDto);

        var controller = new LeadController(_leadRepository, _mapper, _emailService);

        var result = controller.ListAcceptedLeads();
        var okResult = result as OkObjectResult;

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(OkObjectResult));
        okResult?.Value.Should().Be(leadsDto);
    }

    [Fact]
    public void LeadController_ListAcceptedLeads_ReturnBadRequest()
    {
        var leads = A.Fake<List<Lead>>();
        var leadsDto = A.Fake<List<ReadAcceptedLeadDto>>();

        A.CallTo(() => _leadRepository.ListLeadsByStatus(A<LeadStatus>.Ignored, A<int>.Ignored, A<int>.Ignored)).Throws(() => new Exception("Fake exception"));

        var controller = new LeadController(_leadRepository, _mapper, _emailService);

        var result = controller.ListAcceptedLeads();

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(BadRequestObjectResult));
    }

    [Fact]
    public void LeadController_GetLead_ReturnOK()
    {
        var lead = A.Fake<Lead>();
        var leadDto = A.Fake<ReadLeadDto>();

        A.CallTo(() => _leadRepository.GetLead(lead.Id)).Returns(lead);
        A.CallTo(() => _mapper.Map<ReadLeadDto>(lead)).Returns(leadDto);

        var controller = new LeadController(_leadRepository, _mapper, _emailService);

        var result = controller.GetLead(lead.Id);
        var okResult = result as OkObjectResult;

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(OkObjectResult));
        okResult?.Value.Should().Be(leadDto);
    }

    [Fact]
    public void LeadController_GetLead_ReturnNotFound()
    {
        A.CallTo(() => _leadRepository.GetLead(1)).Returns(null);

        var controller = new LeadController(_leadRepository, _mapper, _emailService);

        var result = controller.GetLead(1);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(NotFoundResult));
    }

    [Fact]
    public void LeadController_GetLead_ReturnBadRequest()
    {
        var lead = A.Fake<Lead>();
        var leadDto = A.Fake<ReadLeadDto>();

        A.CallTo(() => _leadRepository.GetLead(lead.Id)).Throws(() => new Exception("Fake exception"));

        var controller = new LeadController(_leadRepository, _mapper, _emailService);

        var result = controller.GetLead(lead.Id);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(BadRequestObjectResult));
    }

    [Fact]
    public void LeadController_UpdateLead_ReturnNoContent()
    {
        var lead = A.Fake<Lead>();
        var leadDto = A.Fake<UpdateLeadDto>();

        A.CallTo(() => _leadRepository.GetLead(lead.Id)).Returns(lead);

        var controller = new LeadController(_leadRepository, _mapper, _emailService);

        var result = controller.UpdateLead(lead.Id, leadDto);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(NoContentResult));
        A.CallTo(() => _mapper.Map(leadDto, lead)).MustHaveHappened();
        A.CallTo(() => _leadRepository.UpdateLead(lead)).MustHaveHappened();
    }

    [Fact]
    public void LeadController_UpdateLead_ReturnNotFound()
    {
        A.CallTo(() => _leadRepository.GetLead(1)).Returns(null);

        var controller = new LeadController(_leadRepository, _mapper, _emailService);

        var result = controller.UpdateLead(1, A.Fake<UpdateLeadDto>());

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(NotFoundResult));
        A.CallTo(() => _mapper.Map(A<UpdateLeadDto>.Ignored, A<Lead>.Ignored)).MustNotHaveHappened();
        A.CallTo(() => _leadRepository.UpdateLead(A<Lead>.Ignored)).MustNotHaveHappened();
    }

    [Fact]
    public void LeadController_UpdateLead_ReturnBadRequest()
    {
        var lead = A.Fake<Lead>();
        var leadDto = A.Fake<UpdateLeadDto>();

        A.CallTo(() => _leadRepository.GetLead(lead.Id)).Returns(lead);
        A.CallTo(() => _leadRepository.UpdateLead(lead)).Throws(() => new Exception("Fake exception"));

        var controller = new LeadController(_leadRepository, _mapper, _emailService);

        var result = controller.UpdateLead(lead.Id, leadDto);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(BadRequestObjectResult));
        A.CallTo(() => _mapper.Map(leadDto, lead)).MustHaveHappened();
        A.CallTo(() => _leadRepository.UpdateLead(lead)).MustHaveHappened();
    }

    [Fact]
    public void LeadController_DeleteLead_ReturnNoContent()
    {
        var lead = A.Fake<Lead>();

        A.CallTo(() => _leadRepository.GetLead(lead.Id)).Returns(lead);

        var controller = new LeadController(_leadRepository, _mapper, _emailService);

        var result = controller.DeleteLead(lead.Id);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(NoContentResult));
        A.CallTo(() => _leadRepository.DeleteLead(lead)).MustHaveHappened();
    }

    [Fact]
    public void LeadController_DeleteLead_ReturnNotFound()
    {
        A.CallTo(() => _leadRepository.GetLead(1)).Returns(null);

        var controller = new LeadController(_leadRepository, _mapper, _emailService);

        var result = controller.DeleteLead(1);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(NotFoundResult));
        A.CallTo(() => _leadRepository.DeleteLead(A<Lead>.Ignored)).MustNotHaveHappened();
    }

    [Fact]
    public void LeadController_DeleteLead_ReturnBadRequest()
    {
        var lead = A.Fake<Lead>();

        A.CallTo(() => _leadRepository.GetLead(lead.Id)).Returns(lead);
        A.CallTo(() => _leadRepository.DeleteLead(lead)).Throws(() => new Exception("Fake exception"));

        var controller = new LeadController(_leadRepository, _mapper, _emailService);

        var result = controller.DeleteLead(lead.Id);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(BadRequestObjectResult));
        A.CallTo(() => _leadRepository.DeleteLead(lead)).MustHaveHappened();
    }
}