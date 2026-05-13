using System.Data;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using Domain.Payrolls;
using SharedKernel;

namespace Application.Payrolls.GetByEmail;

internal sealed class GetPayrollByEmailQueryHandler(IDbConnectionFactory factory)
    : IQueryHandler<GetPayrollByEmailQuery, PayrollResponse>
{
    public async Task<Result<PayrollResponse>> Handle(GetPayrollByEmailQuery query, CancellationToken cancellationToken)
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
            WHERE p.email = @Email
            """;

        using IDbConnection connection = factory.GetOpenConnection();

        PayrollResponse? payroll = await connection.QueryFirstOrDefaultAsync<PayrollResponse>(
            sql,
            query);

        if (payroll is null)
        {
            return Result.Failure<PayrollResponse>(PayrollErrors.NotFoundByEmail);
        }

        return payroll;
    }
}
