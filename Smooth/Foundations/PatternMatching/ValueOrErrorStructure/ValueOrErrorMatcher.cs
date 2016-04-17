using System;
using Smooth.Foundations.Foundations.PatternMatching.ValueOrErrorStructure.Action;
using Smooth.Foundations.Foundations.PatternMatching.ValueOrErrorStructure.Function;
using Smooth.Foundations.PatternMatching;

namespace Smooth.Foundations.Foundations.PatternMatching.ValueOrErrorStructure
{
    public class ValueOrErrorMatcher<T1>
    {
        private readonly ValueOrErrorMatchActionSelector<T1> _actionSelector;
        private readonly ValueOrError<T1> _item;

        internal ValueOrErrorMatcher(ValueOrError<T1> item)
        {
            _item = item;
            _actionSelector = new ValueOrErrorMatchActionSelector<T1>(
                () => { throw new NoMatchException(string.Format("No match action exists for value of {0}", _item)); });
        }

        public ValueMatcher<T1> Value()
        {
            return new ValueMatcher<T1>(this,
                this._actionSelector.AddPredicateAndAction,
                this._actionSelector.SetDefaultOnValueAction,
                this._item.IsError);
        }

        public ErrorMatcher<T1> Error()
        {
            return new ErrorMatcher<T1>(this, this._actionSelector.AddErrorAction, this._item.IsError);
        }

        public void Exec()
        {
            this._actionSelector.InvokeMatchedOrDefaultAction(this._item);
        }
    }
}