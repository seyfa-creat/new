//--- The contract--
public interface IEnrollmentService
{
Task<EnrollmentRecord> EnrollAsync(string studentId, string courseCode);
Task<EnrollmentRecord?> GetByIdAsync(string id);
Task<IReadOnlyList<EnrollmentRecord>> GetAllAsync();
Task<bool> DeleteAsync(string id);
}
//--- The in-memory implementation--
public class EnrollmentService : IEnrollmentService
{
private readonly Dictionary<string, EnrollmentRecord> _store = new();
private readonly ILogger<EnrollmentService> _logger;
public EnrollmentService(ILogger<EnrollmentService> logger)
{
_logger = logger;
}
public Task<EnrollmentRecord> EnrollAsync(string studentId, string courseCode)
{
var id = Guid.NewGuid().ToString("N")[..8];
var record = new EnrollmentRecord(id, studentId, courseCode, DateTime.UtcNow);
_store[id] = record;
_logger.LogInformation(
"Enrolled {StudentId} in {CourseCode} record {EnrollmentId}",
studentId, courseCode, id);
return Task.FromResult(record);
}
public Task<EnrollmentRecord?> GetByIdAsync(string id)
{
_store.TryGetValue(id, out var record);
return Task.FromResult(record);
}
public Task<IReadOnlyList<EnrollmentRecord>> GetAllAsync()
{
IReadOnlyList<EnrollmentRecord> all = _store.Values.ToList();
return Task.FromResult(all);
}
public Task<bool> DeleteAsync(string id)
{
var removed = _store.Remove(id);
return Task.FromResult(removed);
}
}
//--- The data shape--
public record EnrollmentRecord(
string Id,
string StudentId,
string CourseCode,
DateTime EnrolledAt);
builder.Services.AddSingleton<EnrollmentWorker>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
//Addhost validation so the container catches illegal lifetime wiring early:
builder.Host.UseDefaultServiceProvider(options =>
{
options.ValidateScopes = true;
options.ValidateOnBuild = true;
});
app.MapGet("/api/enrollments/worker-smoke", (EnrollmentWorker worker) =>
{
worker.ProcessBatch();
return Results.Ok("processed");
});
//$base = "http://localhost:5000" # or https://localhost:7xxx match your terminal
//1..15 | ForEach-Object-Parallel {
//Invoke-WebRequest-Uri "$using:base/api/enrollments/worker-smoke"-UseBasicParsing | Out-Null
};//-ThrottleLimit 15
// These registrations are given do NOT change them:
builder.Services.AddSingleton<EnrollmentWorker>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
// Inside EnrollmentWorker.cs...
public class EnrollmentWorker(IServiceScopeFactory scopeFactory)
{
public void ProcessBatch()
{
// TODO2:Create a short-lived scope using the injected factory.
// Stuck? using var scope = factory.CreateScope();
// TODO3:Resolve the scoped service from the new scope's provider.
// Stuck? var svc = scope.ServiceProvider.GetRequiredService<IEnrollmentService>();
// TODO4:Usetheservice, then let the 'using' block dispose the scope
//
//and its scoped services automatically.
}
}
// TODO1:Create a class called PaymentOptions with two properties:
//- GatewayUrl (string, required use [Required] attribute)
//- MaxDepositBirr (decimal, range 100-100000 use [Range] attribute)
// Stuck? public class PaymentOptions { [Required] public required string GatewayUrl { get; init; }
... }
and enable startup validation.
// TODO2:In Program.cs, bind PaymentOptions to the "Payments" section of appsettings.json
//
// Stuck? builder.Services.AddOptions<PaymentOptions>()
//
.BindConfiguration("Payments")
//
//
.ValidateDataAnnotations()
.ValidateOnStart();
// TODO3:Test it delete the "Payments" section from appsettings.json and run the app.
//
// BAD oneconcatenated blob, not queryable
logger.LogInformation("Enrolling student " + studentId + " in course " + course);
// GOOD StudentId and Course become queryable properties in any log aggregator
logger.LogInformation("Enrolling student {StudentId} in course {Course}", studentId, course);
public Task<EnrollmentRecord> EnrollAsync(string studentId, string courseCode)
{
// Check for duplicate enrollment
var existing = _store.Values
.FirstOrDefault(e => e.StudentId == studentId && e.CourseCode == courseCode);
if (existing is not null)
{
_logger.LogWarning(
"Duplicate enrollment attempt {StudentId} already in {CourseCode} (record {EnrollmentI
d})",
}
studentId, courseCode, existing.Id);
return Task.FromResult(existing);
var id = Guid.NewGuid().ToString("N")[..8];
var record = new EnrollmentRecord(id, studentId, courseCode, DateTime.UtcNow);
_store[id] = record;
_logger.LogInformation(
"Enrolled {StudentId} in {CourseCode} record {EnrollmentId}",
studentId, courseCode, id);
return Task.FromResult(record);
}
2. AddaLogWarning to GetByIdAsync when the requested record does not exist:
public Task<EnrollmentRecord?> GetByIdAsync(string id)
{
_store.TryGetValue(id, out var record);
if (record is null)
{
_logger.LogWarning("Enrollment {EnrollmentId} not found", id);
}
return Task.FromResult(record);
}
3. AddaLogInformation to DeleteAsync:
public Task<bool> DeleteAsync(string id)
{
var removed = _store.Remove(id);
if (removed)
_logger.LogInformation("Deleted enrollment {EnrollmentId}", id);
else
_logger.LogWarning("Delete failed enrollment {EnrollmentId} not found", id);
return Task.FromResult(removed);
}

