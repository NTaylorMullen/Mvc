#if NET45
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace MvcSample.Web
{
    /// <summary>
    /// Summary description for MonitoringMiddlware
    /// </summary>
    public class MonitoringMiddlware
    {
        private RequestDelegate _next;
        private IServiceProvider _services;

        public MonitoringMiddlware(RequestDelegate next, IServiceProvider services)
        {
            _next = next;
            _services = services;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string url = httpContext.Request.Path.Value;

            if (url.Equals("/Monitoring/Clear", StringComparison.OrdinalIgnoreCase))
            {
                var controller = new MonitoringController();
                controller.Clear();
                httpContext.Response.ContentType = "text/plain";
                var buffer = Encoding.ASCII.GetBytes("Cleared");
                httpContext.Response.Body.Write(buffer, 0, buffer.Length);
            }
            else if (url.Equals("/Monitoring/ActivatedTypes"))
            {
                var controller = new MonitoringController();
                var data = controller.ActivatedTypes();
                httpContext.Response.ContentType = "text/plain charset=utf-8";
                var buffer = Encoding.UTF8.GetBytes(data);

                httpContext.Response.Body.Write(buffer, 0, buffer.Length);
            }
            else
            {
                await _next(httpContext);
            }
        }
    }
}
#endif