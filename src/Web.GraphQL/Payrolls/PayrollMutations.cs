using Application.Payrolls.Create;
using HotChocolate.Types;
using MediatR;

namespace Web.GraphQL.Payrolls;

// ── Vertical slice: the Payroll feature's MUTATION fields ───────────────────────────────────
// [ExtendObjectType("Mutation")] grafts these fields onto the root Mutation type. Same idea as
// PayrollQueries — the feature owns its write operations alongside its reads and DTOs.
[ExtendObjectType("Mutation")]
public sealed class PayrollMutations
{
    public async Task<Guid> CreatePayroll(
        CreatePayrollInput input,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new CreatePayrollCommand(
            input.Email,
            input.Name,
            input.CompanyName,
            input.StartDate,
            input.EndDate);

        SharedKernel.Result<Guid> result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? result.Value : ResultGraphQL.Throw<Guid>(result.Error);
    }
}
