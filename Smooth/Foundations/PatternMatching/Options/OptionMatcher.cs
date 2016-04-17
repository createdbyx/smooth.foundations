using System;
using Smooth.Algebraics;

namespace Smooth.Foundations.PatternMatching.Options
{
    public sealed class OptionMatcher<T>
    {
        private readonly ActionSelectorForOption<T> _actionSelector;
        private readonly Option<T> _item;

        internal OptionMatcher(Option<T> item)
        {
            _item = item;
            _actionSelector = new ActionSelectorForOption<T>(x =>
            { throw new NoMatchException(string.Format("No match action exists for value of {0}", x)); });
        }


        public SomeMatcher<T> Some()
        {
            return new SomeMatcher<T>(this, this._actionSelector.AddPredicateAndAction);
        }

        public NoneMatcher<T> None()
        {
            return new NoneMatcher<T>(this, this._actionSelector.AddPredicateAndAction);
        }

        public OptionMatcherAfterElse<T> Else(Action<Option<T>> elseAction)
        {
            return new OptionMatcherAfterElse<T>(this._actionSelector, elseAction, this._item);
        }

        public OptionMatcherAfterElse<T> IgnoreElse()
        {
            return new OptionMatcherAfterElse<T>(this._actionSelector, _ => { }, this._item);
        }

        public void Exec()
        {
            this._actionSelector.InvokeMatchedOrDefaultAction(this._item);
        }
    }
}