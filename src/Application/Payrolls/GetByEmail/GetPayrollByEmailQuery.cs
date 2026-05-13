using Application.Abstractions.Messaging;

namespace Application.Payrolls.GetByEmail;

public sealed record GetPayrollByEmailQuery(string Email) : IQuery<PayrollResponse>;
