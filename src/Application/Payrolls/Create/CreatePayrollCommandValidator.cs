using FluentValidation;

namespace Application.Payrolls.Create;

internal sealed class CreatePayrollCommandValidator : AbstractValidator<CreatePayrollCommand>
{
    public CreatePayrollCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithErrorCode(PayrollErrorCodes.CreatePayroll.MissingName);

        RuleFor(c => c.Email)
            .NotEmpty().WithErrorCode(PayrollErrorCodes.CreatePayroll.MissingEmail)
            .EmailAddress().WithErrorCode(PayrollErrorCodes.CreatePayroll.InvalidEmail);

        RuleFor(c => c.CompanyName)
            .NotEmpty().WithErrorCode(PayrollErrorCodes.CreatePayroll.MissingCompanyName);

        RuleFor(c => c.EndDate)
            .GreaterThanOrEqualTo(c => c.StartDate)
            .WithErrorCode(PayrollErrorCodes.CreatePayroll.EndDateBeforeStartDate);
    }
}
