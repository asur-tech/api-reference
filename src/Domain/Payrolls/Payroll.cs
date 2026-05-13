using SharedKernel;

namespace Domain.Payrolls;

public sealed class Payroll : Entity
{
    private Payroll(
        Guid id,
        Email email,
        Name name,
        CompanyName companyName,
        DateOnly startDate,
        DateOnly endDate)
        : base(id)
    {
        Email = email;
        Name = name;
        CompanyName = companyName;
        StartDate = startDate;
        EndDate = endDate;
    }

    private Payroll()
    {
    }

    public Email Email { get; private set; }

    public Name Name { get; private set; }

    public CompanyName CompanyName { get; private set; }

    public DateOnly StartDate { get; private set; }

    public DateOnly EndDate { get; private set; }

    public static Payroll Create(
        Email email,
        Name name,
        CompanyName companyName,
        DateOnly startDate,
        DateOnly endDate)
    {
        var payroll = new Payroll(Guid.NewGuid(), email, name, companyName, startDate, endDate);

        payroll.Raise(new PayrollCreatedDomainEvent(payroll.Id));

        return payroll;
    }
}
