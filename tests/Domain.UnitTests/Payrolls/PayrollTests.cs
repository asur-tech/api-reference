using Domain.Payrolls;
using FluentAssertions;

namespace Domain.UnitTests.Payrolls;

public class PayrollTests
{
    private static readonly Email ValidEmail = Email.Create("test@test.com").Value;
    private static readonly Name ValidName = new("Full Name");
    private static readonly CompanyName ValidCompanyName = new("Acme Corp");
    private static readonly DateOnly StartDate = new(2026, 1, 1);
    private static readonly DateOnly EndDate = new(2026, 1, 15);

    [Fact]
    public void Create_Should_CreatePayroll_WhenInputsAreValid()
    {
        // Act
        var payroll = Payroll.Create(ValidEmail, ValidName, ValidCompanyName, StartDate, EndDate);

        // Assert
        payroll.Should().NotBeNull();
        payroll.CompanyName.Should().Be(ValidCompanyName);
        payroll.StartDate.Should().Be(StartDate);
        payroll.EndDate.Should().Be(EndDate);
    }

    [Fact]
    public void Create_Should_RaiseDomainEvent_WhenInputsAreValid()
    {
        // Act
        var payroll = Payroll.Create(ValidEmail, ValidName, ValidCompanyName, StartDate, EndDate);

        // Assert
        payroll.DomainEvents
            .Should().ContainSingle()
            .Which
            .Should().BeOfType<PayrollCreatedDomainEvent>();
    }
}
