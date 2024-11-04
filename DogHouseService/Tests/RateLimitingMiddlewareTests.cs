using DogHouseService.Middleware;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
namespace DogHouseService.Tests
{
    public class RateLimitingMiddlewareTests
    {
        private readonly Mock<ILogger<RateLimitingMiddleware>> _mockLogger;
        private readonly RequestDelegate _next;

        public RateLimitingMiddlewareTests()
        {
            _mockLogger = new Mock<ILogger<RateLimitingMiddleware>>();
            _next = (context) => Task.CompletedTask;
        }

        [Fact]
        public async Task ShouldReturn429WhenRateLimitExceeded()
        {
            
            var middleware = new RateLimitingMiddleware(_next, _mockLogger.Object);
            var context = new DefaultHttpContext();

            
            await middleware.InvokeAsync(context);

            
            Assert.Equal(429, context.Response.StatusCode);
        }

        private static HttpContext CreateHttpContext()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new System.IO.MemoryStream();
            return context;
        }

       
    }
}
