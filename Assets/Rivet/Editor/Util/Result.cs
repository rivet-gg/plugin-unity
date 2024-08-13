using System;
using System.Collections.Generic;

namespace Rivet.Editor
{
    public abstract class Result
    {
        public bool Ok { get; protected set; }
        public bool Err => !Ok;
    }

    public abstract class Result<T> : Result
    {
        private T _data;

        protected Result(T data)
        {
            Data = data;
        }

        public T Data
        {
            get => Ok ? _data : throw new Exception($"You can't access .{nameof(Data)} when .{nameof(Ok)} is false");
            set => _data = value;
        }
    }

    public class ResultOk : Result
    {
        public ResultOk()
        {
            Ok = true;
        }
    }

    public class ResultOk<T> : Result<T>
    {
        public ResultOk(T data) : base(data)
        {
            Ok = true;
        }
    }

    public class ResultErr : Result, IErrorResult
    {
        public ResultErr(string message) : this(message, Array.Empty<Error>())
        {
        }

        public ResultErr(string message, IReadOnlyCollection<Error> errors)
        {
            Message = message;
            Ok = false;
            Errors = errors ?? Array.Empty<Error>();
        }

        public string Message { get; }
        public IReadOnlyCollection<Error> Errors { get; }
    }

    public class ResultErr<T> : Result<T>, IErrorResult
    {
        public ResultErr(string message) : this(message, Array.Empty<Error>())
        {
        }

        public ResultErr(string message, IReadOnlyCollection<Error> errors) : base(default)
        {
            Message = message;
            Ok = false;
            Errors = errors ?? Array.Empty<Error>();
        }

        public string Message { get; set; }
        public IReadOnlyCollection<Error> Errors { get; }
    }

    public class Error
    {
        public Error(string details) : this(null, details)
        {

        }

        public Error(string code, string details)
        {
            Code = code;
            Details = details;
        }

        public string Code { get; }
        public string Details { get; }
    }

    internal interface IErrorResult
    {
        string Message { get; }
        IReadOnlyCollection<Error> Errors { get; }
    }
}