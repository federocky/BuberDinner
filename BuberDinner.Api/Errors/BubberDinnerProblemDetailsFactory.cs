using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace BuberDinner.Api.Errors;

public class BubberDinnerProblemDetailsFactory : ProblemDetailsFactory
{
    private readonly ApiBehaviorOptions _options;

    public BubberDinnerProblemDetailsFactory(IOptions<ApiBehaviorOptions> options)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    
    public override ProblemDetails CreateProblemDetails(
        HttpContext httpContext, 
        int? statusCode = null, 
        string? title = null, 
        string? type = null, 
        string? instance = null, 
        string? detail = null)
    {
        statusCode ??= 500;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title ,
            Type = type ,
            Detail = detail,
            Instance = instance,
        };
        
        ApplyPRoblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);
        
        return problemDetails;
    }

    public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext httpContext, ModelStateDictionary modelStateDictionary, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
    {
        throw new NotImplementedException();
    }

    private void ApplyPRoblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
    {
        problemDetails.Status ??= statusCode;

        if(_options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
        {
            problemDetails.Title ??= clientErrorData.Title;
            problemDetails.Type ??= clientErrorData.Link;
        }

        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
        if(traceId != null)
        {
            problemDetails.Extensions["traceId"] = traceId;
        }

        problemDetails.Extensions.Add("customProperty", "customValue");
    }


}