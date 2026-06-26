using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace PhoneBook.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            // Handled separately because it returns a structured 400 response
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var problem = new ValidationProblemDetails(
                ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()))
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation failed"
            };

            var json = JsonSerializer.Serialize(problem);
            await context.Response.WriteAsync(json);
        }
        catch (DomainException ex)
        {
            // All custom domain exceptions carry their own HTTP status code
            context.Response.StatusCode = ex.HttpStatusCode;
            context.Response.ContentType = "application/json";

            var problem = new ProblemDetails
            {
                Status = ex.HttpStatusCode,
                Title = ex.GetType().Name.Replace("Exception", string.Empty),
                Detail = ex.Message
            };

            var json = JsonSerializer.Serialize(problem);
            await context.Response.WriteAsync(json);
        }
        catch (Exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            // Optional logging
        }
    }
}