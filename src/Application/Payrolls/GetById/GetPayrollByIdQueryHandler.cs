using System.Data;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using Domain.Payrolls;
using SharedKernel;

namespace Application.Payrolls.GetById;

internal sealed class GetPayrollByIdQueryHandler(IDbConnectionFactory factory)
    : IQueryHandler<GetPayrollByIdQuery, PayrollResponse>
{
    public async Task<Result<PayrollResponse>> Handle(GetPayrollByIdQuery query, CancellationToken cancellationToken)
    {
        const string sql =
            """
            SELECT
                p.id AS Id,
                p.email AS Email,
                p.name AS Name,
                p.company_name AS CompanyName,
                p.start_date AS StartDate,
                p.end_date AS EndDate
            FROM payrolls p
            WHERE p.id = @PayrollId
            """;

        using IDbConnection connection = factory.GetOpenConnection();

        PayrollResponse? payroll = await connection.QueryFirstOrDefaultAsync<PayrollResponse>(
            sql,
            query);

        if (payroll is null)
        {
            return Result.Failure<PayrollResponse>(PayrollErrors.NotFound(query.PayrollId));
        }

        return payroll;
    }
}
