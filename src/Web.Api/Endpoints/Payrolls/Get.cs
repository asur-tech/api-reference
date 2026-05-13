using Application.Payrolls.GetById;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Payrolls;

public class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("payrolls/{payrollId}", async (
            Guid payrollId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetPayrollByIdQuery(payrollId);

            Result<PayrollResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Payrolls);
    }
}
