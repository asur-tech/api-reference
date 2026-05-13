using System.Net;
using System.Net.Http.Json;
using Api.FunctionalTests.Contracts;
using Api.FunctionalTests.Extensions;
using Api.FunctionalTests.Infrastructure;
using Application.Payrolls;
using Application.Payrolls.Create;
using FluentAssertions;

namespace Api.FunctionalTests.Payrolls;

public class CreatePayrollTests : BaseFunctionalTest
{
    private static readonly DateOnly StartDate = new(2026, 1, 1);
    private static readonly DateOnly EndDate = new(2026, 1, 15);

    public CreatePayrollTests(FunctionalTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenEmailIsMissing()
    {
        // Arrange
        var request = new CreatePayrollRequest("", "name", "Acme Corp", StartDate, EndDate);

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/payrolls", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        CustomProblemDetails problemDetails = await response.GetProblemDetails();

        problemDetails.Errors.Select(e => e.Code)
            .Should()
            .Contain([
                PayrollErrorCodes.CreatePayroll.MissingEmail,
                PayrollErrorCodes.CreatePayroll.InvalidEmail
            ]);
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenEmailIsInvalid()
    {
        // Arrange
        var request = new CreatePayrollRequest("test", "name", "Acme Corp", StartDate, EndDate);

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/payrolls", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        CustomProblemDetails problemDetails = await response.GetProblemDetails();

        problemDetails.Errors.Select(e => e.Code)
            .Should()
            .Contain([PayrollErrorCodes.CreatePayroll.InvalidEmail]);
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenNameIsMissing()
    {
        // Arrange
        var request = new CreatePayrollRequest("test@test.com", "", "Acme Corp", StartDate, EndDate);

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/payrolls", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        CustomProblemDetails problemDetails = await response.GetProblemDetails();

        problemDetails.Errors.Select(e => e.Code)
            .Should()
            .Contain([PayrollErrorCodes.CreatePayroll.MissingName]);
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenCompanyNameIsMissing()
    {
        // Arrange
        var request = new CreatePayrollRequest("test@test.com", "name", "", StartDate, EndDate);

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/payrolls", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        CustomProblemDetails problemDetails = await response.GetProblemDetails();

        problemDetails.Errors.Select(e => e.Code)
            .Should()
            .Contain([PayrollErrorCodes.CreatePayroll.MissingCompanyName]);
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenEndDateBeforeStartDate()
    {
        // Arrange
        var laterDate = new DateOnly(2026, 2, 1);
        var earlierDate = new DateOnly(2026, 1, 1);
        var request = new CreatePayrollRequest("test@test.com", "name", "Acme Corp", laterDate, earlierDate);

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/payrolls", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        CustomProblemDetails problemDetails = await response.GetProblemDetails();

        problemDetails.Errors.Select(e => e.Code)
            .Should()
            .Contain([PayrollErrorCodes.CreatePayroll.EndDateBeforeStartDate]);
    }

    [Fact]
    public async Task Should_ReturnOk_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreatePayrollRequest("test@test.com", "name", "Acme Corp", StartDate, EndDate);

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/payrolls", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_ReturnConflict_WhenPayrollExists()
    {
        // Arrange
        var request = new CreatePayrollRequest("test@test.com", "name", "Acme Corp", StartDate, EndDate);

        // Act
        await HttpClient.PostAsJsonAsync("api/v1/payrolls", request);

        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/payrolls", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
