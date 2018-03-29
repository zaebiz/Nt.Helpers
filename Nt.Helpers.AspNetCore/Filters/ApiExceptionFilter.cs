using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Nt.Helpers.AspNetCore.Models;

namespace Nt.Helpers.AspNetCore.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ApiExceptionFilter> _log;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> log)
        {
            _log = log;
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var apiError = new RtApiErrorResponse(context.Exception);
            _log.LogError(context.Exception, "Internal server error");
            context.Result = new JsonResult(apiError);

            return base.OnExceptionAsync(context);
        }

    }
}