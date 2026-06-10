using Application.Payrolls.GetByEmail;
using Application.Payrolls.GetById;
using HotChocolate.Types;
using MediatR;

namespace Web.GraphQL.Payrolls;

// ── Vertical slice: the Payroll feature's QUERY fields ──────────────────────────────────────
// [ExtendObjectType("Query")] grafts these fields onto the root Query type (created bare in Program.cs
// via AddQueryType()). So instead of one giant Query class, every feature owns its query fields in its
// own folder — the presentation-layer mirror of the Application/Payrolls vertical slices.
//
// ResultGraphQL (in the parent Web.GraphQL namespace) is reachable here without a using, because name
// lookup walks outward from Web.GraphQL.Payrolls to Web.GraphQL.
[ExtendObjectType("Query")]
public sealed class PayrollQueries
{
    public async Task<PayrollDto> GetPayrollById(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken)
    {
        SharedKernel.Result<Application.Payrolls.GetById.PayrollResponse> result =
            await sender.Send(new GetPayrollByIdQuery(id), cancellationToken);

        return result.IsSuccess
            ? ToDto(result.Value)
            : ResultGraphQL.Throw<PayrollDto>(result.Error);
    }

    public async Task<PayrollDto> GetPayrollByEmail(
        string email,
        ISender sender,
        CancellationToken cancellationToken)
    {
        SharedKernel.Result<Application.Payrolls.GetByEmail.PayrollResponse> result =
            await sender.Send(new GetPayrollByEmailQuery(email), cancellationToken);

        return result.IsSuccess
            ? ToDto(result.Value)
            : ResultGraphQL.Throw<PayrollDto>(result.Error);
    }

    private static PayrollDto ToDto(Application.Payrolls.GetById.PayrollResponse p) =>
        new(p.Id, p.Email, p.Name, p.CompanyName, p.StartDate, p.EndDate);

    private static PayrollDto ToDto(Application.Payrolls.GetByEmail.PayrollResponse p) =>
        new(p.Id, p.Email, p.Name, p.CompanyName, p.StartDate, p.EndDate);
}
