using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Common
{
    public record result(bool IsSuccess, string? Error= null,ResultKind Kind=ResultKind.Ok)
    {
        public static result Ok() => new result(true);
        public static result Fail(string ErrorMessage,ResultKind Kind= ResultKind.Conflict) => new result(false, ErrorMessage,Kind);
        public static result NotFound(string ErrorMessage = "NotFound") => new result(false, ErrorMessage, ResultKind.NotFound);
        public static result Validation(string ErrorMessage = "NotFound") => new result(false, ErrorMessage, ResultKind.Validationfailed );


    }
    public record result<T>(bool IsSuccess, T? Value = default, string? Error = null, ResultKind Kind = ResultKind.Ok)
    {
        public static result<T> Ok(T Value) => new(true, Value);
        public static result<T> Fail(string ErrorMessage, ResultKind Kind = ResultKind.Conflict) => new (false, default, ErrorMessage, Kind);
        public static result<T> NotFound(string ErrorMessage = "NotFound") => new(false, default, ErrorMessage, ResultKind.NotFound);
        public static result<T> Validation(string ErrorMessage = "Validation failed") => new(false, default, ErrorMessage, ResultKind.Validationfailed);
    }
    public enum ResultKind
    {
        Ok,
        NotFound,
        Conflict,
        Validationfailed,
        Forbidden,
    }
}
