using System.Net;
using System.Net.Http.Json;
using Api.FunctionalTests.Infrastructure;
using Application.Payrolls.Create;
using Application.Payrolls.GetById;
using FluentAssertions;

namespace Api.FunctionalTests.Payrolls;

public class GetPayrollTests : BaseFunctionalTest
{
    private static readonly DateOnly StartDate = new(2026, 1, 1);
    private static readonly DateOnly EndDate = new(2026, 1, 15);

    public GetPayrollTests(FunctionalTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnNotFound_WhenPayrollDoesNotExist()
    {
        // Arrange
        var payrollId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await HttpClient.GetAsync($"api/v1/payrolls/{payrollId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_ReturnPayroll_WhenPayrollExists()
    {
        // Arrange
        Guid payrollId = await CreatePayrollAsync();

        // Act
        PayrollResponse? payroll = await HttpClient.GetFromJsonAsync<PayrollResponse>($"api/v1/payrolls/{payrollId}");

        // Assert
        payroll.Should().NotBeNull();
    }

    private async Task<Guid> CreatePayrollAsync()
    {
        var request = new CreatePayrollRequest("test@test.com", "name", "Acme Corp", StartDate, EndDate);

        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/payrolls", request);

        return await response.Content.ReadFromJsonAsync<Guid>();
    }
}
