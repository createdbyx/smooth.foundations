using Smooth.Delegates;

namespace Smooth.Foundations.Foundations.PatternMatching.ValueOrErrorStructure.Action
{
    public sealed class ValueMatcher<T>
    {
        private readonly DelegateAction<DelegateFunc<T, bool>, DelegateAction<T>> _addPredicateAndAction;
        private readonly DelegateAction<DelegateAction<T>> _addDefaultValueAction;
        private readonly ValueOrErrorMatcher<T> _matcher;
        private readonly bool _isError;

        public ValueMatcher(ValueOrErrorMatcher<T> matcher,
            DelegateAction<DelegateFunc<T, bool>, DelegateAction<T>> addPredicateAndAction,
            DelegateAction<DelegateAction<T>> addAddDefaultValueAction,
            bool isError)
        {
            _addDefaultValueAction = addAddDefaultValueAction;
            _matcher = matcher;
            _addPredicateAndAction = addPredicateAndAction;
            _isError = isError;
        }

        public WhereForValue<T> Where(DelegateFunc<T, bool> predicate)
        {
            return this._isError
                ? WhereForValue<T>.Useless(this._matcher)
                : new WhereForValue<T>(predicate, this._addPredicateAndAction, this._matcher);
        }

        public ValueOrErrorMatcher<T> Do(DelegateAction<T> action)
        {
            _addDefaultValueAction(action);
            return _matcher;
        }
    }
}