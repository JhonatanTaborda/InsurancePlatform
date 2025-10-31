using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Proposal.Application.Handlers;

namespace Proposal.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProposalApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProposalHandler).Assembly));
            return services;
        }
    }
}
