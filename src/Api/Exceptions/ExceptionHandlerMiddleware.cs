using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Products.Domain.Exceptions;

namespace Products.Api.Exceptions
{
    public class ExceptionHandlerMiddleware {
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (Exception ex)
            {
                await this.HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            if (context.Response.HasStarted) {
                return Task.CompletedTask;
            }

            var code = HttpStatusCode.InternalServerError;
            if (ex is BaseNotFoundException) {
                code = HttpStatusCode.NotFound;
            } else if (ex is BaseException) {
                code = HttpStatusCode.BadRequest;
            }
            var result = JsonConvert.SerializeObject(new { Message = ex.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;
            return context.Response.WriteAsync(result);
        }
    }
}