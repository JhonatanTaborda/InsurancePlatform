using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Proposal.Application;
using Proposal.Application.Handlers;
using Proposal.Application.Interfaces;
using Proposal.Infrastructure.Entity;
using Proposal.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
var cfg = builder.Configuration;

builder.Services.AddProposalApplication();   
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ProposalDbContext>(o =>
o.UseSqlServer(cfg.GetConnectionString("Sql"),
        sql => sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)));

builder.Services.AddScoped<IProposalRepository, ProposalRepository>();
builder.Services.AddScoped<IUnitOfWork, EfUow>();
builder.Services.AddScoped<CreateProposalHandler>();
builder.Services.AddScoped<ListProposalsHandler>();
builder.Services.AddScoped<ChangeStatusHandler>();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.UsingRabbitMq((context, bus) =>
    {
        var rabbit = cfg.GetConnectionString("Rabbit") ?? "amqp://guest:guest@rabbitmq:5672";
        bus.Host(new Uri(rabbit));
    });
});

builder.Services.AddOptions<MassTransitHostOptions>().Configure(x =>
{
    x.WaitUntilStarted = true;
    x.StartTimeout = TimeSpan.FromSeconds(30);
    x.StopTimeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Use(async (ctx, next) =>
{
    try { await next(); }
    catch (Exception ex)
    {
        Console.Error.WriteLine(ex);
        ctx.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
        await ctx.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
});

app.Run();

public sealed class EfUow : IUnitOfWork
{
    private readonly ProposalDbContext _db;
    public EfUow(ProposalDbContext db) => _db = db;
    public Task Commit(CancellationToken ct) => _db.SaveChangesAsync(ct);
}