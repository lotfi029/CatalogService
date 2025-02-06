using Application.Features.Auth.Commands;
using Application.Features.Auth.Contracts;
using System.ComponentModel.DataAnnotations;

namespace API.Endpoints;

public class AuthenticationEndpoint : ICarterModule
{
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth")
            .WithTags("Authentication");

        group.MapPost("/register", Register);
        group.MapPost("/login", Login);
        group.MapPost("/refresh-token", GetRefreshToken);
        group.MapPost("/revoke-refresh-token", RevokeRefreshToken);
        group.MapPost("/confirm-email", ConfirmEmail);
        group.MapPost("/re-confirm-email", ReConfirmEmail);
        group.MapPost("/forget-password", ForgetPassword);
        group.MapPost("/reset-password", ResetPassword);
    }
    private static async Task<IResult> Register(
        [FromBody] RegisterRequest request,
        IValidator<RegisterRequest> validator,
        [FromServices] ISender sender,                    
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
        var command = new RegisterCommand(request);
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok()
            : result.ToProblemDetails();
    }
    private static async Task<IResult> Login(
        [FromBody] LoginRequest request,
        IValidator<LoginRequest> validator,
        [FromServices] ISender sender,
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
        var command = new LoginCommand (request);
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblemDetails();
    }
    private static async Task<IResult> GetRefreshToken(
        [FromBody] RefreshTokenRequest request,
        IValidator<RefreshTokenRequest> _validator,
        [FromServices] ISender _sender,
        CancellationToken cancellationToken
        )
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

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

        var command = new RefreshTokenCommand (request);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblemDetails();
    }

    private async Task<IResult> RevokeRefreshToken(
        [FromBody] RefreshTokenRequest request,
        IValidator<RefreshTokenRequest> _validator,
        [FromServices] ISender _sender,
        CancellationToken cancellationToken
        )
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

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

        var command = new RevokeRefreshTokenCommand (request);

        var result = await _sender.Send(command, cancellationToken);


        return result.IsSuccess
            ? TypedResults.Ok()
            : result.ToProblemDetails();
    }

    private async Task<IResult> ConfirmEmail(
        [FromBody] ConfirmEmailRequest request,
        IValidator<ConfirmEmailRequest> _validator,
        [FromServices] ISender _sender,
        CancellationToken cancellationToken
        )
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

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

        var command = new ConfirmEmailCommand(request);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok()
            : result.ToProblemDetails();
    }
    private async Task<IResult> ReConfirmEmail(
        [FromBody] ResendConfirmEmailRequest request,
        IValidator<ResendConfirmEmailRequest> _validator,
        [FromServices] ISender _sender,
        CancellationToken cancellationToken
        )
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

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

        var command = new ReConfirmEmailCommand(request);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok()
            : result.ToProblemDetails();
    }
    private async Task<IResult> ForgetPassword(
        [FromBody] ForgetPasswordRequest request,
        IValidator<ForgetPasswordRequest> _validator,
        [FromServices] ISender _sender,
        CancellationToken cancellationToken
        )
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

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

        var command = new ForgetPasswordCommand(request);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok()
            : result.ToProblemDetails();
    }
    private async Task<IResult> ResetPassword(
        [FromBody] ResetPasswordRequest request,
        IValidator<ResetPasswordRequest> _validator,
        [FromServices] ISender _sender,
        CancellationToken cancellationToken
        )
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

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

        var command = new ResetPasswordCommand(request);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok()
            : result.ToProblemDetails();
    }
    

}