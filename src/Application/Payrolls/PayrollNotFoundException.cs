namespace Application.Payrolls;

public sealed class PayrollNotFoundException : Exception
{
    public PayrollNotFoundException(Guid payrollId)
        : base($"The payroll with the identifier {payrollId} was not found")
    {
        PayrollId = payrollId;
    }

    public Guid PayrollId { get; }
}
