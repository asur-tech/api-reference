namespace Application.Payrolls;

public static class PayrollErrorCodes
{
    public static class CreatePayroll
    {
        public const string MissingName = nameof(MissingName);
        public const string MissingEmail = nameof(MissingEmail);
        public const string InvalidEmail = nameof(InvalidEmail);
        public const string MissingCompanyName = nameof(MissingCompanyName);
        public const string EndDateBeforeStartDate = nameof(EndDateBeforeStartDate);
    }
}
