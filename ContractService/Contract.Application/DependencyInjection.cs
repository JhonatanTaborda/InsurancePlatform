using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Contract.Application.Handlers;

namespace Contract.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddContractApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(HireProposalHandler).Assembly));
            return services;
        }
    }
}
