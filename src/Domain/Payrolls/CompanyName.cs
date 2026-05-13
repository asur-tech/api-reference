using SharedKernel;

namespace Domain.Payrolls;

public sealed record CompanyName
{
    public CompanyName(string? value)
    {
        Ensure.NotNullOrEmpty(value);

        Value = value;
    }

    public string Value { get; }
}
