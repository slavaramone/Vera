using Api.Controllers;
using Domain.Enums;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Api
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (BusinessErrorException exc)
            {
                await HandleExceptionAsync(HttpStatusCode.NotFound, context, exc);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex, $"Исключение ErrorHandlingMiddleware");
                await HandleExceptionAsync(HttpStatusCode.InternalServerError, context, new BusinessErrorException(ErrorCode.Unknown, ex.Message));
            }
        }

        private static Task HandleExceptionAsync(HttpStatusCode code, HttpContext context, BusinessErrorException ex)
        {
            var errorModel = new ErrorResponse
            {
                Code = (int)ex.Code,
                Description = ex.Message
            };
            var result = JsonConvert.SerializeObject(errorModel);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
