using Contract.Application;
using Contract.Application.Handlers;
using Contract.Application.Interfaces;
using Contract.Infrastructure.Consumers;
using Contract.Infrastructure.Entity;
using Contract.Infrastructure.Repositories;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var cfg = builder.Configuration;

builder.Services.AddContractApplication();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ContractDbContext>(o =>
o.UseSqlServer(cfg.GetConnectionString("Sql"),
        sql => sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)));


builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IUnitOfWork, EntityUow>();
builder.Services.AddScoped<IApprovedProposalReadModel, EfReadModel>();
builder.Services.AddScoped<HireProposalHandler>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ProposalApprovedConsumer>();
    x.SetKebabCaseEndpointNameFormatter();
    x.UsingRabbitMq((context, bus) =>
    {
        var rabbit = cfg.GetConnectionString("Rabbit") ?? "amqp://guest:guest@rabbitmq:5672";
        bus.Host(new Uri(rabbit));
        bus.ReceiveEndpoint("contract-proposal-approved", e =>
        {
            e.ConfigureConsumer<ProposalApprovedConsumer>(context);
        });
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

public sealed class EntityUow : IUnitOfWork
{
    private readonly ContractDbContext _db;
    public EntityUow(ContractDbContext db) => _db = db;         
    public Task Commit(CancellationToken ct) => _db.SaveChangesAsync(ct);
}

public sealed class EfReadModel : IApprovedProposalReadModel
{
    private readonly ContractDbContext _db;
    public EfReadModel(ContractDbContext db) => _db = db;
    public Task<bool> IsApproved(Guid proposalId, CancellationToken ct) => _db.ApprovedProposals.AnyAsync(x => x.ProposalId == proposalId, ct);
}