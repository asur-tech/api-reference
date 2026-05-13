using SharedKernel;

namespace Domain.Payrolls;

public static class PayrollErrors
{
    public static Error NotFound(Guid payrollId) => Error.NotFound(
        "Payrolls.NotFound",
        $"The payroll with the Id = '{payrollId}' was not found");

    public static readonly Error NotFoundByEmail = Error.NotFound(
        "Payrolls.NotFoundByEmail",
        "The payroll with the specified email was not found");

    public static readonly Error EmailNotUnique = Error.Conflict(
        "Payrolls.EmailNotUnique",
        "The provided email is not unique");
}
