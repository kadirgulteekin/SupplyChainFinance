using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class ResponseDto<T>
    {
        public T? Data { get; set; }
        [JsonIgnore]
        public int StatusCode { get; private set; }
        [JsonIgnore]
        public bool IsSuccessful { get; private set; }
        public List<string>? Errors { get; set; }
        public static ResponseDto<T> Success(T data, int statusCode)
        {
            return new ResponseDto<T> { Data = data, StatusCode = statusCode };
        }
        public static ResponseDto<T> Success(int statusCode)
        {
            return new ResponseDto<T> { StatusCode = statusCode, IsSuccessful = true, Data = default(T) };
        }

        public static ResponseDto<T> Failed(List<string> errors, int statusCode)
        {
            return new ResponseDto<T>
            {
                StatusCode = statusCode,
                IsSuccessful = false,
                Errors = errors

            };
        }

        public static ResponseDto<T> Failed(string error, int statusCode)
        {
            return new ResponseDto<T>
            {
                StatusCode = statusCode,
                IsSuccessful = false,
                Errors = new List<string>() { error }
            };
        }

    }
}
