using Application.Abstractions.Caching;

namespace Application.Payrolls.GetById;

public sealed record GetPayrollByIdQuery(Guid PayrollId) : ICachedQuery<PayrollResponse>
{
    public string CacheKey => $"payroll-by-id-{PayrollId}";

    public TimeSpan? Expiration => null;
}
