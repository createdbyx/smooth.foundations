using Smooth.Delegates;
using Smooth.Foundations.PatternMatching;

namespace Smooth.Foundations.Foundations.PatternMatching.ValueOrErrorStructure.Function
{
    public class ValueOrErrorResultMatcher<T1, TResult>
    {
        private readonly ValueOrErrorMatchFunctionSelector<T1, TResult> _functionSelector;
        private readonly ValueOrError<T1> _item;

        internal ValueOrErrorResultMatcher(ValueOrError<T1> item)
        {
            _item = item;
            _functionSelector = new ValueOrErrorMatchFunctionSelector<T1, TResult>(
                x => { throw new NoMatchException(string.Format("No match action exists for value of {0}", _item)); }, item.IsError);
        }

        public WithForValueOrErrorHandler<ValueOrErrorResultMatcher<T1, TResult>, T1, TResult> With(T1 value)
        {
            return new WithForValueOrErrorHandler<ValueOrErrorResultMatcher<T1, TResult>, T1, TResult>(value,
                this.RecordAction,
                this);
        }

        public WhereForValueOrError<ValueOrErrorResultMatcher<T1, TResult>, T1, TResult> Where(
            DelegateFunc<T1, bool> expression)
        {
            return new WhereForValueOrError<ValueOrErrorResultMatcher<T1, TResult>, T1, TResult>(expression,
                this.RecordAction,
                this);
        }

        public ValueOrErrorResultMatcherWithElse<T1, TResult> Else(DelegateFunc<T1, TResult> action)
        {
            return new ValueOrErrorResultMatcherWithElse<T1, TResult>(this._functionSelector, action, this._item);
        }

        public ValueOrErrorResultMatcherWithElse<T1, TResult> Else(TResult result)
        {
            return new ValueOrErrorResultMatcherWithElse<T1, TResult>(this._functionSelector, x => result, this._item);
        }

        public ValueOrError<TResult> Result()
        {
            return this._functionSelector.DetermineResultUsingDefaultIfRequired(this._item);
        }

        private void RecordAction(DelegateFunc<T1, bool> test, DelegateFunc<T1, TResult> action)
        {
            this._functionSelector.AddPredicateAndAction(test, action);
        }
    }
}