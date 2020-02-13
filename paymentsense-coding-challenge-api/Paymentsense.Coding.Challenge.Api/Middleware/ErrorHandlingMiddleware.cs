using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Paymentsense.Coding.Challenge.Api.Dtos;
using Paymentsense.Coding.Challenge.Api.Extensions;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate next;
    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {   
        var result = new UnhandledError() 
        { 
            ErrorMessage = ex.Message 
        };
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsync(result.Serialize());
    }
}