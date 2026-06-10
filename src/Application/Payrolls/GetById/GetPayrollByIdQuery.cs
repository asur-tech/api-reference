using Application.Abstractions.Caching;

namespace Application.Payrolls.GetById;

public sealed record GetPayrollByIdQuery(Guid PayrollId) : ICachedQuery<PayrollResponse>
{
    public string CacheKey => $"payroll-by-id-{PayrollId}";

    // Interim TTL backstop (Phase 1.7): bounds staleness until write-driven cache
    // invalidation lands in Phase 3. Replaces the previous `null` (cache-forever).
    public TimeSpan? Expiration => TimeSpan.FromMinutes(2);
}
