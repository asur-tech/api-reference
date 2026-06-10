using HotChocolate;

namespace Web.GraphQL.Types;

// Presentation-layer GraphQL shape for a payroll. Kept independent of the Application
// read models (which differ per query) so the schema has a single canonical "Payroll" type.
[GraphQLName("Payroll")]
public sealed record PayrollDto(
    Guid Id,
    string Email,
    string Name,
    string CompanyName,
    DateOnly StartDate,
    DateOnly EndDate);
