using System;
using Smooth.Algebraics;
using Smooth.Delegates;

namespace Smooth.Foundations.PatternMatching.Options
{
    public sealed class ResultOptionMatcher<T, TResult>
    {
        private readonly FuncSelectorForOption<T, TResult> _funcSelector;
        private readonly Option<T> _item;

        public ResultOptionMatcher(Option<T> item)
        {
            _item = item;
            _funcSelector = new FuncSelectorForOption<T, TResult>(x =>
            {
                throw new NoMatchException(string.Format("No match exist for value of {0}",x));
            });
        }
        public SomeMatcherResult<T, TResult> Some()
        {
            return new SomeMatcherResult<T, TResult>(this, this._funcSelector);
        }

        public NoneMatcherResult<T, TResult> None()
        {
            return new NoneMatcherResult<T, TResult>(this, this._funcSelector);
        }

        public ResultOptionMatcherAfterElse<T, TResult> Else(DelegateFunc<Option<T>, TResult> elseResult)
        {
            return new ResultOptionMatcherAfterElse<T, TResult>(this._funcSelector, elseResult, this._item);
        }

        public ResultOptionMatcherAfterElse<T, TResult> Else(TResult elseResult)
        {
            return new ResultOptionMatcherAfterElse<T, TResult>(this._funcSelector, elseResult, this._item);
        }

        public TResult Result()
        {
            return this._funcSelector.GetMatchedOrDefaultResult(this._item);
        }
    }
}