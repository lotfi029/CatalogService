using Application.Features.Auth.Commands;
using Application.Features.Auth.Contracts;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints;

public static class AuthenticationEndpoint
{
    public static void MapAuthenticationEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Authentication");

        group.MapPost("/register", Register)
             .WithName("Register");
    }

    private static async Task<IResult> Register(
        [FromBody] RegisterRequest request,
        IValidator<RegisterRequest> validator,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var problemDetails = new HttpValidationProblemDetails(validationResult.ToDictionary())
            {
                Title = "validate failed",
                Detail = "one or more validation errors occured",
                Instance = "/api/register",
                Status = StatusCodes.Status400BadRequest

            };

            return TypedResults.Problem(problemDetails);
        }
        var command = new RegisterCommandRequest(request);
        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok()
            : TypedResults.BadRequest(string.Join(", ", result.Error));
    }
}