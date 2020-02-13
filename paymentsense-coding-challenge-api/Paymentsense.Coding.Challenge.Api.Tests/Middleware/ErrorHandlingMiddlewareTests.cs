using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Paymentsense.Coding.Challenge.Api.Extensions;
using Paymentsense.Coding.Challenge.Api.Dtos;
using Xunit;
using FluentAssertions;

namespace Paymentsense.Coding.Challenge.Api.Tests.Middleware
{
    public class ErrorHandlingMiddlewareTests
    {
        [Fact]
        public async Task Invoke__When__An_Unexpected_Exception_Occurs__Then__Returns_500_And_UnhandleError()
        {
            var exceptionMessage = "Danger Will Robinson";
            var middleware = new ErrorHandlingMiddleware((innerHttpContext) =>
            {
                throw new Exception(exceptionMessage);
            });
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            await middleware.Invoke(context);

            context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var unhandledError = await context.Response.Body.ReadAsAsync<UnhandledError>();
            unhandledError.ErrorMessage.Should().Be(exceptionMessage);
        }
    }
}