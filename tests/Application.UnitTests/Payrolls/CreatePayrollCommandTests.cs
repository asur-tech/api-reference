using Application.Abstractions.Data;
using Application.Payrolls.Create;
using Domain.Payrolls;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Payrolls;

public class CreatePayrollCommandTests
{
    private static readonly CreatePayrollCommand Command = new(
        "test@test.com",
        "FullName",
        "Acme Corp",
        new DateOnly(2026, 1, 1),
        new DateOnly(2026, 1, 15));

    private readonly CreatePayrollCommandHandler _handler;
    private readonly IPayrollRepository _payrollRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public CreatePayrollCommandTests()
    {
        _payrollRepositoryMock = Substitute.For<IPayrollRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _handler = new CreatePayrollCommandHandler(
            _payrollRepositoryMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenEmailIsInvalid()
    {
        // Arrange
        CreatePayrollCommand invalidCommand = Command with { Email = "invalid_email" };

        // Act
        Result<Guid> result = await _handler.Handle(invalidCommand, default);

        // Assert
        result.Error.Should().Be(EmailErrors.InvalidFormat);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenEmailIsNotUnique()
    {
        // Arrange
        _payrollRepositoryMock.IsEmailUniqueAsync(Arg.Is<Email>(e => e.Value == Command.Email))
            .Returns(false);

        // Act
        Result<Guid> result = await _handler.Handle(Command, default);

        // Assert
        result.Error.Should().Be(PayrollErrors.EmailNotUnique);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenCreateSucceeds()
    {
        // Arrange
        _payrollRepositoryMock.IsEmailUniqueAsync(Arg.Is<Email>(e => e.Value == Command.Email))
            .Returns(true);

        // Act
        Result<Guid> result = await _handler.Handle(Command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_CallRepository_WhenCreateSucceeds()
    {
        // Arrange
        _payrollRepositoryMock.IsEmailUniqueAsync(Arg.Is<Email>(e => e.Value == Command.Email))
            .Returns(true);

        // Act
        Result<Guid> result = await _handler.Handle(Command, default);

        // Assert
        _payrollRepositoryMock.Received(1).Insert(Arg.Is<Payroll>(p => p.Id == result.Value));
    }

    [Fact]
    public async Task Handle_Should_CallUnitOfWork_WhenCreateSucceeds()
    {
        // Arrange
        _payrollRepositoryMock.IsEmailUniqueAsync(Arg.Is<Email>(e => e.Value == Command.Email))
            .Returns(true);

        // Act
        await _handler.Handle(Command, default);

        // Assert
        await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
