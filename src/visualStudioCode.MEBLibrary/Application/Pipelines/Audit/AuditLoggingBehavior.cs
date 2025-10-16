using System.Text.Json;
using Application.Services.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Pipelines.Audit;

public class AuditLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditLoggingBehavior(IAuditLogRepository auditLogRepository, IHttpContextAccessor httpContextAccessor)
    {
        _auditLogRepository = auditLogRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        TResponse response = await next();

        if (!typeof(TRequest).Name.EndsWith("Command", StringComparison.Ordinal))
            return response;

        try
        {
            AuditLog auditLog = BuildAuditLog(request);
            await _auditLogRepository.AddAsync(auditLog);
        }
        catch
        {
            // Audit log failures should not block business flow.
        }

        return response;
    }

    private AuditLog BuildAuditLog(TRequest request)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        string? userId = httpContext?.User?.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "id")?.Value;
        string? userName = httpContext?.User?.Identity?.Name;
        string? ip = httpContext?.Connection.RemoteIpAddress?.ToString();
        string? userAgent = httpContext?.Request.Headers["User-Agent"].ToString();

        string payload = string.Empty;
        try
        {
            payload = JsonSerializer.Serialize(request);
        }
        catch
        {
            payload = request.ToString() ?? typeof(TRequest).Name;
        }

        return new AuditLog
        {
            ActionName = typeof(TRequest).FullName ?? typeof(TRequest).Name,
            UserId = Guid.TryParse(userId, out Guid parsedUserId) ? parsedUserId : null,
            UserName = userName,
            Payload = payload,
            ClientIp = ip,
            UserAgent = userAgent,
            OccurredOn = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow
        };
    }
}
