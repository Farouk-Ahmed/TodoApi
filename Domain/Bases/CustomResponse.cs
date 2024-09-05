using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Bases
{
    public class CustomResponse<Tdynamic>
    {
        public CustomResponse()
        {

        }
        public CustomResponse(dynamic data, string message = null)
        {
            IsCompleted = true;
            Message = message;
            Data = data;
        }
        public CustomResponse(string message)
        {
            IsCompleted = false;
            Message = message;
        }
        public CustomResponse(string message, bool IsCompleted)
        {
            IsCompleted = IsCompleted;
            Message = message;
        }

        public object Meta { get; set; }

        public bool IsCompleted { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }
}
