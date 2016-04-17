using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Delegates;
using Smooth.Slinq;

namespace Smooth.Foundations.Foundations.PatternMatching.ValueOrErrorStructure.Function
{
    public class ValueOrErrorMatchFunctionSelector<T1, TResult>
    {
        private readonly DelegateFunc<T1, TResult> _defaultFunction;

        private readonly List<Tuple<DelegateFunc<T1, bool>, DelegateFunc<T1, TResult>>> _predicatesAndFuncs =
            new List<Tuple<DelegateFunc<T1, bool>, DelegateFunc<T1, TResult>>>();

        private readonly bool _isError;

        public ValueOrErrorMatchFunctionSelector(DelegateFunc<T1, TResult> defaultFunction, bool isError)
        {
            _defaultFunction = defaultFunction;
            _isError = isError;
        }

        public void AddPredicateAndAction(DelegateFunc<T1, bool> test, DelegateFunc<T1, TResult> action)
        {
            this._predicatesAndFuncs.Add(new Tuple<DelegateFunc<T1, bool>, DelegateFunc<T1, TResult>>(test, action));
        }

        public Option<ValueOrError<TResult>> DetermineResult(ValueOrError<T1> value)
        {
            return _predicatesAndFuncs
                .Slinq()
                .FirstOrNone((tuple, p) => tuple.Item1(p.Value), value)
                .Select((pair, p) => ValueOrError<TResult>.FromValue(pair.Item2(p.Value)), value);
        }

        public ValueOrError<TResult> DetermineResultUsingDefaultIfRequired(ValueOrError<T1> value)
        {
            if (_isError) return ValueOrError<TResult>.FromError(value.Error);

            var funcAndArgsPair =
                _predicatesAndFuncs
                    .Slinq()
                    .FirstOrNone((predicateAndTransformationTuple, concreteValue)
                        => predicateAndTransformationTuple.Item1(concreteValue.Value), value);

            return funcAndArgsPair.isSome
                ? ValueOrError<TResult>.FromValue(funcAndArgsPair.value.Item2(value.Value))
                : ValueOrError<TResult>.FromValue(_defaultFunction(value.Value));
        }
    }
}