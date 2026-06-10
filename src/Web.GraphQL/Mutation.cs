using Application.Payrolls.Create;
using MediatR;
using Web.GraphQL.Types;

namespace Web.GraphQL;

// GraphQL root mutation. Delegates to the existing CreatePayrollCommand (validation runs in
// the MediatR pipeline) and returns the new id, or surfaces the failed Result as a GraphQL error.
public sealed class Mutation
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
