using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace GISA.MIC.Application.Helper
{
    public class Response<T>
    {
        public Response()
        {
        }

        public Response(T data, string message = default, bool isSuccess = default, HttpStatusCode statusCode = default)
        {
            StatusCode = statusCode;
            Succeeded = isSuccess;
            Message = message;
            Data = data;
        }

        public HttpStatusCode StatusCode { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }
    }
}
