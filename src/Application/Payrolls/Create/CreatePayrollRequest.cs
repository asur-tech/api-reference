namespace Application.Payrolls.Create;

public sealed record CreatePayrollRequest(
    string Email,
    string Name,
    string CompanyName,
    DateOnly StartDate,
    DateOnly EndDate);
