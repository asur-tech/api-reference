using Application.IntegrationTests.Infrastructure;
using Application.Payrolls.Create;
using Domain.Payrolls;
using FluentAssertions;
using SharedKernel;

namespace Application.IntegrationTests.Payrolls;

public class CreatePayrollTests : BaseIntegrationTest
{
    public CreatePayrollTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Handle_Should_CreatePayroll_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreatePayrollCommand(
            Faker.Internet.Email(),
            Faker.Name.FullName(),
            Faker.Company.CompanyName(),
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 1, 15));

        // Act
        Result<Guid> result = await Sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_AddPayrollToDatabase_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreatePayrollCommand(
            Faker.Internet.Email(),
            Faker.Name.FullName(),
            Faker.Company.CompanyName(),
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 1, 15));

        // Act
        Result<Guid> result = await Sender.Send(command);

        // Assert
        Payroll? payroll = await DbContext.Payrolls.FindAsync(result.Value);

        payroll.Should().NotBeNull();
    }
}
