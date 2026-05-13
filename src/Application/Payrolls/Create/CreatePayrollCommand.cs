using Application.Abstractions.Messaging;

namespace Application.Payrolls.Create;

public sealed record CreatePayrollCommand(
    string Email,
    string Name,
    string CompanyName,
    DateOnly StartDate,
    DateOnly EndDate) : ICommand<Guid>;
