using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Application.Interfaces
{
    public interface IContractRepository
    {
        Task Add(Contract.Domain.ContractModel entity, CancellationToken ct);
        Task<bool> ExistsForProposal(Guid proposalId, CancellationToken ct);
    }
}
