using System;
using Smooth.Algebraics;
using Smooth.Delegates;

namespace Smooth.Foundations.PatternMatching
{
    /// <summary>
    /// An instance of this class will start a fluent function chain for defining and evaluating a pattern match against
    /// a value of T. It either ends in Exec(), or if To{TResult}() is used, switches to a ResultMatcher{T}.
    /// </summary>
    public sealed class ExecMatcher<T1>
    {
        private readonly MatchActionSelector<T1> _actionSelector;
        private readonly T1 _item;

        internal ExecMatcher(T1 item)
        {
            _item = item;
            _actionSelector = new MatchActionSelector<T1>(
                x => { throw new NoMatchException(string.Format("No match action exists for value of {0}", _item)); });
        }

        public ResultMatcher<T1, TResult> To<TResult>()
        {
            return new ResultMatcher<T1, TResult>(this._item);
        }

        public WithForActionHandler<ExecMatcher<T1>, T1> With(T1 value)
        {
            return new WithForActionHandler<ExecMatcher<T1>, T1>(value, this.RecordAction, this);
        }

        public WhereForActionHandler<ExecMatcher<T1>, T1> Where(DelegateFunc<T1, bool> expression)
        {
            return new WhereForActionHandler<ExecMatcher<T1>, T1>(expression, this.RecordAction, this);
        }

        public ExecMatcherAfterElse<T1> Else(Action<T1> action)
        {
            return new ExecMatcherAfterElse<T1>(this._actionSelector, action, this._item);
        }

        public ExecMatcherAfterElse<T1> IgnoreElse()
        {
            return new ExecMatcherAfterElse<T1>(this._actionSelector, x => { }, this._item);
        }

        public void Exec()
        {
            this._actionSelector.InvokeMatchedActionUsingDefaultIfRequired(this._item);
        }

        private void RecordAction(DelegateFunc<T1, bool> test, Action<T1> action)
        {
            this._actionSelector.AddPredicateAndAction(test, action);
        }
    }

    public sealed class ExecMatcher<T1, T2>
    {
        private readonly MatchActionSelector<T1, T2> _actionSelector;
        private readonly Tuple<T1, T2> _item;

        internal ExecMatcher(T1 item1, T2 item2)
        {
            _item = Tuple.Create(item1, item2);
            _actionSelector = new MatchActionSelector<T1, T2>((x, y) =>
            {
                throw new NoMatchException(string.Format("No match action exists for value of ({0},{1})", _item.Item1, _item.Item2));
            });
        }

        public ResultMatcher<T1, T2, TResult> To<TResult>()
        {
            return new ResultMatcher<T1, T2, TResult>(this._item);
        }

        public WithForActionHandler<ExecMatcher<T1, T2>, T1, T2> With(T1 value1, T2 value2)
        {
            return new WithForActionHandler<ExecMatcher<T1, T2>, T1, T2>(Tuple.Create(value1, value2), this.RecordAction,
                this);
        }

        public WhereForActionHandler<ExecMatcher<T1, T2>, T1, T2> Where(DelegateFunc<T1, T2, bool> expression)
        {
            return new WhereForActionHandler<ExecMatcher<T1, T2>, T1, T2>(expression, this.RecordAction, this);
        }

        private void RecordAction(DelegateFunc<T1, T2, bool> test, Action<T1, T2> action)
        {
            this._actionSelector.AddPredicateAndAction(test, action);
        }

        public ExecMatcherAfterElse<T1, T2> Else(Action<T1, T2> action)
        {
            return new ExecMatcherAfterElse<T1, T2>(this._actionSelector, action, this._item);
        }

        public ExecMatcherAfterElse<T1, T2> IgnoreElse()
        {
            return new ExecMatcherAfterElse<T1, T2>(this._actionSelector, (x, y) => { }, this._item);
        }

        public void Exec()
        {
            this._actionSelector.InvokeMatchedActionUsingDefaultIfRequired(this._item.Item1, this._item.Item2);
        }
    }

    public sealed class ExecMatcher<T1, T2, T3>
    {
        private readonly MatchActionSelector<T1, T2, T3> _actionSelector;
        private readonly Tuple<T1, T2, T3> _item;

        internal ExecMatcher(T1 item1, T2 item2, T3 item3)
        {
            _item = Tuple.Create(item1, item2, item3);
            _actionSelector = new MatchActionSelector<T1, T2, T3>((x, y, z) =>
            {
                throw new NoMatchException(
                    string.Format("No match action exists for value of ({0}, {1}, {2})", _item.Item1, _item.Item2, _item.Item3));
            });
        }

        public ResultMatcher<T1, T2, T3, TResult> To<TResult>()
        {
            return new ResultMatcher<T1, T2, T3, TResult>(this._item);
        }

        public WithForActionHandler<ExecMatcher<T1, T2, T3>, T1, T2, T3> With(T1 value1, T2 value2, T3 value3)
        {
            return new WithForActionHandler<ExecMatcher<T1, T2, T3>, T1, T2, T3>(Tuple.Create(value1, value2, value3),
                this.RecordAction,
                this);
        }

        public WhereForActionHandler<ExecMatcher<T1, T2, T3>, T1, T2, T3> Where(DelegateFunc<T1, T2, T3, bool> expression)
        {
            return new WhereForActionHandler<ExecMatcher<T1, T2, T3>, T1, T2, T3>(expression, this.RecordAction, this);
        }

        private void RecordAction(DelegateFunc<T1, T2, T3, bool> test, Action<T1, T2, T3> action)
        {
            this._actionSelector.AddPredicateAndAction(test, action);
        }

        public ExecMatcherAfterElse<T1, T2, T3> Else(Action<T1, T2, T3> action)
        {
            return new ExecMatcherAfterElse<T1, T2, T3>(this._actionSelector, action, this._item);
        }

        public ExecMatcherAfterElse<T1, T2, T3> IgnoreElse()
        {
            return new ExecMatcherAfterElse<T1, T2, T3>(this._actionSelector, (x, y, z) => { }, this._item);
        }

        public void Exec()
        {
            this._actionSelector.InvokeMatchedActionUsingDefaultIfRequired(this._item.Item1, this._item.Item2,
                this._item.Item3);
        }
    }

    public sealed class ExecMatcher<T1, T2, T3, T4>
    {
        private readonly MatchActionSelector<T1, T2, T3, T4> _actionSelector;
        private readonly Tuple<T1, T2, T3, T4> _item;

        internal ExecMatcher(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            _item = Tuple.Create(item1, item2, item3, item4);
            _actionSelector = new MatchActionSelector<T1, T2, T3, T4>(
                (w, x, y, z) =>
                {
                    throw new NoMatchException(
                        string.Format("No match action exists for value of ({0}, {1}, {2})",_item.Item1, _item.Item2, _item.Item3));
                });
        }

        public ResultMatcher<T1, T2, T3, T4, TResult> To<TResult>()
        {
            return new ResultMatcher<T1, T2, T3, T4, TResult>(this._item);
        }

        public WithForActionHandler<ExecMatcher<T1, T2, T3, T4>, T1, T2, T3, T4> With(T1 value1,
                                                                                      T2 value2,
                                                                                      T3 value3,
                                                                                      T4 value4)
        {
            return new WithForActionHandler<ExecMatcher<T1, T2, T3, T4>, T1, T2, T3, T4>(Tuple.Create(value1,
                                                                                                      value2,
                                                                                                      value3,
                                                                                                      value4),
                                                                                         RecordAction,
                                                                                         this);
        }

        public WhereForActionHandler<ExecMatcher<T1, T2, T3, T4>, T1, T2, T3, T4>
            Where(DelegateFunc<T1, T2, T3, T4, bool> expression)
        {
            return new WhereForActionHandler<ExecMatcher<T1, T2, T3, T4>, T1, T2, T3, T4>(expression, this.RecordAction,
                this);
        }

        private void RecordAction(DelegateFunc<T1, T2, T3, T4, bool> test, Action<T1, T2, T3, T4> action)
        {
            this._actionSelector.AddPredicateAndAction(test, action);
        }

        public ExecMatcherAfterElse<T1, T2, T3, T4> Else(Action<T1, T2, T3, T4> action)
        {
            return new ExecMatcherAfterElse<T1, T2, T3, T4>(this._actionSelector, action, this._item);
        }

        public ExecMatcherAfterElse<T1, T2, T3, T4> IgnoreElse()
        {
            return new ExecMatcherAfterElse<T1, T2, T3, T4>(this._actionSelector, (w, x, y, z) => { }, this._item);
        }

        public void Exec()
        {
            this._actionSelector.InvokeMatchedActionUsingDefaultIfRequired(this._item.Item1,
                this._item.Item2,
                this._item.Item3,
                this._item.Item4);
        }
    }

}

