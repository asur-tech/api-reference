using Application.Payrolls.GetByEmail;
using Application.Payrolls.GetById;
using MediatR;
using Web.GraphQL.Types;

namespace Web.GraphQL;

// GraphQL root query. Each resolver is thin: it delegates to the existing MediatR query
// (which keeps validation, logging and Redis caching intact) and maps the Result onto the
// GraphQL type — the same job the REST GET endpoints used to do.
public sealed class Query
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
