using System;
using Smooth.Foundations.Foundations.PatternMatching.ValueOrErrorStructure.Function;

namespace Smooth.Foundations.Foundations.PatternMatching.ValueOrErrorStructure
{
    public struct ValueOrError<T>
    {
        public T Value
        {
            get
            {
                if (IsError)
                {
                    throw new InvalidOperationException();
                }
                return _value;
            }
        }

        public bool IsError { get; private set; }

        public string Error { get; private set; }

        private readonly T _value;

        public static ValueOrError<T> FromValue(T value)
        {
            return new ValueOrError<T>(value, string.Empty, false);
        }

        public static ValueOrError<T> FromError(string error)
        {
            return new ValueOrError<T>(default(T), error, true);
        }

        private ValueOrError(T value, string error, bool isError)
        {
            _value = value;
            this.Error = error;
            this.IsError = isError;
        }
      
        public ValueOrErrorMatcher<T> Match()
        {
            return new ValueOrErrorMatcher<T>(this);
        }

        public ValueOrErrorResultMatcher<T, TResult> MatchTo<TResult>()
        {
           return new ValueOrErrorResultMatcher<T, TResult>(this);
        }


        public ValueOrError<TResult> ContinueWith<TResult>(Func<ValueOrError<TResult>> func)
        {
            return IsError
                ? ValueOrError<TResult>.FromError(Error)
                : func();
        }

        public ValueOrError<TResult> ContinueWith<TResult>(Func<ValueOrError<T>, ValueOrError<TResult>> func)
        {
            return IsError
                ? ValueOrError<TResult>.FromError(Error)
                : func(this);
        }

        public ValueOrError<TResult> ContinueWith<TResult>(Func<T, ValueOrError<TResult>> func)
        {
            return IsError
                ? ValueOrError<TResult>.FromError(Error)
                : func(Value);
        }

        public ValueOrError<TResult> ContinueWith<TResult>(Func<T, TResult> func)
        {
            if (IsError)
            {
                return ValueOrError<TResult>.FromError(Error);
            }
            try
            {
                return ValueOrError<TResult>.FromValue(func(Value));
            }
            catch (Exception e)
            {
                return ValueOrError<TResult>.FromError(e.Message);
            }
        }

        public ValueOrError<TResult> ContinueWith<TResult, TU>(Func<T, TU, TResult> func, TU arg)
        {
            if (IsError)
            {
                return ValueOrError<TResult>.FromError(Error);
            }
            try
            {
                return ValueOrError<TResult>.FromValue(func(Value, arg));
            }
            catch (Exception e)
            {
                return ValueOrError<TResult>.FromError(e.Message);
            }
        }
    }
}