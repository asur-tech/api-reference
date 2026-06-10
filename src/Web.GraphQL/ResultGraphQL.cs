using HotChocolate;

namespace Web.GraphQL;

// Bridges the application's Result/Error pattern into GraphQL. A failed Result is surfaced
// as a GraphQLException carrying the domain error code, which Hot Chocolate renders in the
// response's "errors" array (the GraphQL equivalent of the REST ProblemDetails mapping).
public static class ResultGraphQL
{
    public static T Throw<T>(SharedKernel.Error error) =>
        throw new GraphQLException(
            ErrorBuilder.New()
                .SetMessage(error.Description)
                .SetCode(error.Code)
                .SetExtension("errorType", error.Type.ToString())
                .Build());
}
