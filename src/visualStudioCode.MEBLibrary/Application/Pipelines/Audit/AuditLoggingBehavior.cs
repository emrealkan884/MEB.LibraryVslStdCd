using System.Text.Json;
using System.Security.Claims;
using Application.Services.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Pipelines.Audit;

public class AuditLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AuditLoggingBehavior(
        IHttpContextAccessor httpContextAccessor,
        IServiceScopeFactory serviceScopeFactory
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        bool shouldAudit = typeof(TRequest).Name.EndsWith("Command", StringComparison.Ordinal);
        if (!shouldAudit)
            return await next();

        try
        {
            TResponse response = await next();
            await tryWriteAuditAsync(request, exception: null);
            return response;
        }
        catch (Exception ex)
        {
            await tryWriteAuditAsync(request, ex);
            throw;
        }
    }

    private async Task tryWriteAuditAsync(TRequest request, Exception? exception)
    {
        try
        {
            AuditLog auditLog = BuildAuditLog(request, exception);
            await using AsyncServiceScope scope = _serviceScopeFactory.CreateAsyncScope();
            IAuditLogRepository scopedRepository = scope.ServiceProvider.GetRequiredService<IAuditLogRepository>();
            await scopedRepository.AddAsync(auditLog);
        }
        catch
        {
            // Audit log failures should not block business flow.
        }
    }

    private AuditLog BuildAuditLog(TRequest request, Exception? exception)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        ClaimsPrincipal? user = httpContext?.User;

        string? userId = user?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user?.FindFirstValue("sub")
            ?? user?.FindFirstValue("id")
            ?? user?.FindFirstValue(ClaimTypes.Sid);

        string? userName = user?.Identity?.Name
            ?? user?.FindFirstValue(ClaimTypes.Email)
            ?? user?.FindFirstValue(ClaimTypes.Name)
            ?? buildFullName(user);

        string? ip = httpContext?.Connection.RemoteIpAddress?.ToString();
        string? userAgent = httpContext?.Request.Headers["User-Agent"].ToString();

        string payload = string.Empty;
        try
        {
            object payloadObject = new
            {
                success = exception is null,
                request,
                exception = exception is null
                    ? null
                    : new
                    {
                        type = exception.GetType().FullName,
                        message = exception.Message,
                        stackTrace = exception.StackTrace,
                        innerMessage = exception.InnerException?.Message
                    }
            };
            payload = JsonSerializer.Serialize(payloadObject);
        }
        catch
        {
            payload = exception is null
                ? request.ToString() ?? typeof(TRequest).Name
                : $"{exception.GetType().FullName}: {exception.Message}";
        }

        string actionName = typeof(TRequest).FullName ?? typeof(TRequest).Name;
        if (exception is not null)
            actionName += ".Failed";

        return new AuditLog
        {
            ActionName = actionName,
            UserId = Guid.TryParse(userId, out Guid parsedUserId) ? parsedUserId : null,
            UserName = userName,
            Payload = payload,
            ClientIp = ip,
            UserAgent = userAgent,
            OccurredOn = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow
        };
    }

    private static string? buildFullName(ClaimsPrincipal? user)
    {
        if (user is null)
            return null;

        string? firstName = user.FindFirstValue(ClaimTypes.GivenName);
        string? lastName = user.FindFirstValue(ClaimTypes.Surname);

        if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
            return null;

        return string.Join(' ', new[] { firstName, lastName }.Where(s => !string.IsNullOrWhiteSpace(s)));
    }
}
