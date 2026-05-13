namespace Application.Payrolls.GetByEmail;

public sealed record PayrollResponse
{
    public Guid Id { get; init; }

    public string Email { get; init; }

    public string Name { get; init; }

    public string CompanyName { get; init; }

    public DateOnly StartDate { get; init; }

    public DateOnly EndDate { get; init; }
}
