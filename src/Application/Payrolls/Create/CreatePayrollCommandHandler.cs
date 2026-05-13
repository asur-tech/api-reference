using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Payrolls;
using SharedKernel;

namespace Application.Payrolls.Create;

internal sealed class CreatePayrollCommandHandler(
    IPayrollRepository payrollRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreatePayrollCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreatePayrollCommand command,
        CancellationToken cancellationToken)
    {
        Result<Email> emailResult = Email.Create(command.Email);
        if (emailResult.IsFailure)
        {
            return Result.Failure<Guid>(emailResult.Error);
        }

        Email email = emailResult.Value;
        if (!await payrollRepository.IsEmailUniqueAsync(email))
        {
            return Result.Failure<Guid>(PayrollErrors.EmailNotUnique);
        }

        var name = new Name(command.Name);
        var companyName = new CompanyName(command.CompanyName);
        var payroll = Payroll.Create(email, name, companyName, command.StartDate, command.EndDate);

        payrollRepository.Insert(payroll);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return payroll.Id;
    }
}
