﻿using System;
using Smooth.Algebraics;
using Smooth.Delegates;

namespace Smooth.Foundations.PatternMatching.Options
{
    public sealed class SomeMatcher<T>
    {
        private readonly OptionMatcher<T> _matcher;
        private readonly Action<DelegateFunc<Option<T>, bool>, Action<T>> _addPredicateAndAction;

        public SomeMatcher(OptionMatcher<T> matcher, Action<DelegateFunc<Option<T>, bool>, Action<T>> addPredicateAndAction)
        {
            _matcher = matcher;
            _addPredicateAndAction = addPredicateAndAction;
        }

        public OfMatcher<T> Of(T value)
        {
            return new OfMatcher<T>(value, this._matcher, this._addPredicateAndAction);
        }

        public WhereForOption<T> Where(DelegateFunc<T, bool> predicate)
        {
            return new WhereForOption<T>(predicate, this._addPredicateAndAction, this._matcher);
        }

        public OptionMatcher<T> Do(Action<T> action)
        {
            _addPredicateAndAction(o => o.isSome, action);
            return _matcher;
        } 
    }
}