using Core.Utilities.Messages;
using Couchbase.Configuration.Server.Serialization;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using static Core.Extensions.ErrorDetails;

namespace Core.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var watch = Stopwatch.StartNew();
            try
            {
                string message = "[Request] HTTP " + httpContext.Request.Method + " - " + httpContext.Request.Path;
                Console.WriteLine(message);

                await _next(httpContext);
                watch.Stop();

                message = "[Response] HTTP " + httpContext.Request.Method + " - " + httpContext.Request.Path + " responded " + httpContext.Response.StatusCode + " in " + watch.Elapsed.TotalMilliseconds + "ms";
                Console.WriteLine(message);
            }
            catch (Exception E)
            {
                watch.Stop();
                await HandleExceptionAsync(httpContext, E, watch);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception e, Stopwatch watch)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            _ = e.Message;
            string message = "[Error] HTTP " + httpContext.Request.Method + " - " + httpContext.Response.StatusCode + " - Error Message " + e.Message + " in " + watch.Elapsed.TotalMilliseconds + "ms";
            Console.WriteLine(message);

            var result = JsonConvert.SerializeObject(new { error = e.Message }, Formatting.None);


            IEnumerable<ValidationFailure> errors;
            if (e.GetType() == typeof(ValidationException))
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                message = e.Message;
                errors = ((ValidationException)e).Errors;

                return httpContext.Response.WriteAsync(new ValidationErrorDetails
                {
                    StatusCode = 400,
                    Message = message,
                    Errors = errors
                }.ToString());
            }
            else if (e.GetType() == typeof(BootstrapException))
            {
                message = "Bağlantı Problemi. "+e.Message;
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (e.GetType() == typeof(ApplicationException))
            {
                message = e.Message;
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (e.GetType() == typeof(UnauthorizedAccessException))
            {
                message = e.Message;
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else if (e.GetType() == typeof(SecurityException))
            {
                message = e.Message;
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else
            {
                message = ExceptionMessage.InternalServerError + e.Message +" \n "+ e.StackTrace;
            }

            return httpContext.Response.WriteAsync(message);
        }
    }
}
