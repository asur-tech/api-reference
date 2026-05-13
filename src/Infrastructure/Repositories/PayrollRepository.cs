using Domain.Payrolls;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class PayrollRepository(ApplicationDbContext context) : IPayrollRepository
{
    public Task<Payroll?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Payrolls.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<bool> IsEmailUniqueAsync(Email email)
    {
        return !await context.Payrolls.AnyAsync(p => p.Email == email);
    }

    public void Insert(Payroll payroll)
    {
        context.Payrolls.Add(payroll);
    }
}
