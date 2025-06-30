using System;
using System.Collections.Generic;
using System.Linq;

namespace SilverLeaf.Screener.Admin.Objects
{
    public class Response
    {
        public bool Succeed { get; set; }
        public bool Canceled { get; set; }
        public List<string> Errors { get; set; }

        public static Response Success()
        {
            return new Response { Succeed = true };
        }

        public static T Failure<T>(params string[] errors)
            where T : Response, new()
        {
            return Failure<T>((IEnumerable<string>)errors);
        }

        public static T Failure<T>(IEnumerable<string> errors)
            where T : Response, new()
        {
            if (errors == null)
            {
                throw new ArgumentNullException(nameof(errors));
            }

            return new T
            {
                Errors = errors.ToList(),
                Succeed = false
            };
        }

        public static T Cancellation<T>()
            where T : Response, new()
        {
            return new T
            {
                Canceled = true
            };
        }

        public string GetFormattedErrors(string separator = " ")
        {
            if (Errors == null)
            {
                return null;
            }

            var errors = new List<string>();
            foreach (var error in Errors)
            {
                if (!string.IsNullOrWhiteSpace(error))
                {
                    var e = error.Trim();
                    if (!e.EndsWith("."))
                    {
                        e = e + ".";
                    }

                    errors.Add(e);
                }
            }

            return string.Join(separator, errors);
        }
    }

    public class Response<T> : Response
    {
        public T Data { get; set; }

        public static Response<T> Success(T data)
        {
            return new Response<T>
            {
                Data = data,
                Succeed = true
            };
        }
    }
}
