// Starter pipeline (do not assume this order is correct)
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseRouting();
app.MapGet("/api/assessments/results", () => Results.Ok(new
{
courseCode = "CS-101",
studentId = "S-001",
letterGrade = "A"
}));
app.UseAuthentication();
app.UseAuthorization();
app.Run();
var builder = WebApplication.CreateBuilder(args);
// Services: add authentication / authorization services
var app = builder.Build();
// TODO1:Register routing in the pipeline where it belongs for your app.
// TODO2:Register authentication and authorization in the pipeline where your template and fa
cilitator expect them for a protected minimal API route.
// TODO3:MapGET/api/assessments/results with the same response body as the starter, but r
equire authorization for that route.
app.Run();
// Read the request path
var path = context.Request.Path;
// Read the HTTP method
var method = context.Request.Method;
// Add a response header
context.Response.Headers["X-Correlation-Id"] = correlationId;
// Read the response status code
var statusCode = context.Response.StatusCode;
// Measure elapsed time
var stopwatch = Stopwatch.StartNew();
// work happens here
stopwatch.Stop();
var elapsedMs = stopwatch.ElapsedMilliseconds;
// Write a structured log entry
_logger.LogInformation(
"Request {Method} {Path}",
context.Request.Method,
context.Request.Path);
// Pass control to the next middleware
await _next(context);
Typical middleware flow
// Before next
//- Read request information
//- Add response headers
//- Start timing
await _next(context);
// After next
//- Read response information
//- Stop timing
//- Write completion logs
Create a file named TrainingAuthHandler.cs.
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
public class TrainingAuthHandler
: AuthenticationHandler<AuthenticationSchemeOptions>
{
public TrainingAuthHandler(
IOptionsMonitor<AuthenticationSchemeOptions> options,
ILoggerFactory logger,
UrlEncoder encoder)
: base(options, logger, encoder)
{
}
protected override Task<AuthenticateResult> HandleAuthenticat
eAsync()
{
if (!Request.Headers.ContainsKey("X-Training-User"))
{
return Task.FromResult(
AuthenticateResult.Fail("Missing training user he
ader."));
}
var claims = new[]
{
g-User"]!)
};
new Claim(ClaimTypes.Name, Request.Headers["X-Trainin
var identity = new ClaimsIdentity(claims, Scheme.Name);
var principal = new ClaimsPrincipal(identity);
var ticket = new AuthenticationTicket(principal, Scheme.N
ame);
}
}
return Task.FromResult(
AuthenticateResult.Success(ticket));
builder.Services
.AddAuthentication("Training")
.AddScheme<AuthenticationSchemeOptions,
TrainingAuthHandler>("Training", null);
builder.Services.AddAuthorization();
Configure the middleware pipeline:
app.UseAuthentication();
app.UseAuthorization();
Protect the endpoint:
app.MapGet(...)
.RequireAuthorization();