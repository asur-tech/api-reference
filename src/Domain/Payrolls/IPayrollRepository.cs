namespace Domain.Payrolls;

public interface IPayrollRepository
{
    Task<Payroll?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> IsEmailUniqueAsync(Email email);

    void Insert(Payroll payroll);
}
