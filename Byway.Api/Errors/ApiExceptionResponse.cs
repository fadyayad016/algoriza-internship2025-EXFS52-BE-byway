﻿namespace Byway.Api.Errors
{
    public class ApiExceptionResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string? Details { get; set; }
    }
}
