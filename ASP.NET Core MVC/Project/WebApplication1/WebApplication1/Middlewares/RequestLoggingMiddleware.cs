using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace WebApplication1.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Collect request info
            var path = context.Request.Path;
            var user = context.User?.Identity?.IsAuthenticated == true ? context.User.Identity.Name : "Anonymous";
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Format log line
            var logLine = $"[{timestamp}] Path: {path}, User: {user}";

            // Option 1: log to console
            Console.WriteLine(logLine);

            // Option 2: log to file
            var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "requests.log");
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath)!);
            await File.AppendAllTextAsync(logFilePath, logLine + Environment.NewLine);

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
}