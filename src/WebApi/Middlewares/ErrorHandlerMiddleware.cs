using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Dtos;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace Api.Middlewares
{
    public  class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var errorResponseModel = new ErrorResponse<string>
                {
                    Message = error?.Message
                };

                switch (error)
                {
                    case ValidationException e:
                        // custom application error
                        response.StatusCode = (int) HttpStatusCode.BadRequest;
                        errorResponseModel.Code = e.Errors.First().ErrorCode;
                        errorResponseModel.Message = e.Errors.First().ErrorMessage;
                        break;

                    default:
                        // unhandled error
                        response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        errorResponseModel.Code = "unhandled_exception";
                        errorResponseModel.Message = "some thing went wrong!";
                        break;
                }

                var result = JsonSerializer.Serialize(errorResponseModel);
                _logger.LogError(result);
                await response.WriteAsync(result);
            }
        }
    }
}