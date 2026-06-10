namespace Web.GraphQL.Types;

// Input for the createPayroll mutation. Hot Chocolate exposes this as "CreatePayrollInput".
public sealed record CreatePayrollInput(
    string Email,
    string Name,
    string CompanyName,
    DateOnly StartDate,
    DateOnly EndDate);
