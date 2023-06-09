using LeadManagement.Data;
using LeadManagement.Interfaces;
using LeadManagement.Repository;
using LeadManagement.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Get connection strings
var leadDbConnectionStrings = builder.Configuration.GetConnectionString("LeadDbConnection");


// Add services to the container.
builder.Services.AddDbContext<LeadContext>(opts => opts.UseSqlServer(leadDbConnectionStrings));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ILeadRepository, LeadRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
