using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Nt.Helpers.AspNetCore.Filters.ETag
{
    public class ETagFilter : ActionFilterAttribute
    {
        private readonly IEtagProvider _etagSvc;
        private readonly string _resourceName;

        const string EtagHeaderName = "ETag";
        const string MatchHeaderName = "If-None-Match";

        public ETagFilter(IEtagProvider etagSvc, string resourseName)
        {
            _etagSvc = etagSvc;
            _resourceName = resourseName;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var challengeSuccess = false;
            var resourceEtag = string.Empty;

            // проверим - корректный актуальность ETag от клитента
            if (context.HttpContext.Request.Headers.Keys.Contains(MatchHeaderName))
            {
                var requestEtag = context.HttpContext.Request.Headers[MatchHeaderName];
                resourceEtag = await _etagSvc.GetResourceEtag(_resourceName);

                if (!string.IsNullOrEmpty(resourceEtag) && string.Equals(resourceEtag, requestEtag))
                {
                    context.Result = new StatusCodeResult((int)HttpStatusCode.NotModified);
                    challengeSuccess = true;
                }
            }

            // продолжим выполнение, только если пришел неактуальный ETag
            if (!challengeSuccess) await next();

            // отдадим клиенту актуальный ETag
            if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.OK
                && context.HttpContext.Request.Method == "GET")
            {
                resourceEtag = string.IsNullOrEmpty(resourceEtag)
                    ? await _etagSvc.GetResourceEtag(_resourceName)
                    : resourceEtag;

                if (!string.IsNullOrEmpty(resourceEtag))
                {
                    context.HttpContext.Response.Headers.Add(EtagHeaderName, new[] { resourceEtag });
                }

            }
        }
    }
}
