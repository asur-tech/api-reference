using Application.Payrolls.Create;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Payrolls;

public class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("payrolls", async (
            CreatePayrollRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new CreatePayrollCommand(
                request.Email,
                request.Name,
                request.CompanyName,
                request.StartDate,
                request.EndDate);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Payrolls);
    }
}
