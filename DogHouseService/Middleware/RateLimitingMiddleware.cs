using System.Collections.Concurrent;

namespace DogHouseService.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private static int _requestCount = 0;
        private static DateTime _resetTime = DateTime.UtcNow.AddSeconds(1);
        private const int MaxRequestsPerSecond = 10;
        private readonly ILogger<RateLimitingMiddleware> _logger;

        public RateLimitingMiddleware(RequestDelegate next, ILogger<RateLimitingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (DateTime.UtcNow >= _resetTime)
            {
                Interlocked.Exchange(ref _requestCount, 0);
                _resetTime = DateTime.UtcNow.AddSeconds(1);
            }

            if (Interlocked.Increment(ref _requestCount) > MaxRequestsPerSecond)
            {
                _logger.LogWarning("Too many requests from IP: {IP}", context.Connection.RemoteIpAddress);
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Too many requests. Please try again later.");
                return;
            }

            await _next(context);
        }
    }
}
    
    
