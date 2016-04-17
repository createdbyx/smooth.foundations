using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Delegates;
using Smooth.Slinq;

namespace Smooth.Foundations.Foundations.PatternMatching.ValueOrErrorStructure.Action
{
    public class ValueOrErrorMatchActionSelector<T1>
    {
        private readonly DelegateAction _matchNotFoundAction;

        private Option<DelegateAction<T1>> _onValueDefaultAction = Option<DelegateAction<T1>>.None;

        private readonly List<Tuple<DelegateFunc<T1, bool>, DelegateAction<T1>>> _testsAndActions =
            new List<Tuple<DelegateFunc<T1, bool>, DelegateAction<T1>>>();

        private readonly List<DelegateAction<string>> _errorActions =
            new List<DelegateAction<string>>();

        public ValueOrErrorMatchActionSelector(DelegateAction matchNotFoundAction)
        {
            _matchNotFoundAction = matchNotFoundAction;
        }


        public void SetDefaultOnValueAction(DelegateAction<T1> action)
        {
            this._onValueDefaultAction = new Option<DelegateAction<T1>>(action);
        }


        public void AddPredicateAndAction(DelegateFunc<T1, bool> test, DelegateAction<T1> action)
        {
            this._testsAndActions.Add(new Tuple<DelegateFunc<T1, bool>, DelegateAction<T1>>(test, action));
        }

        public void AddErrorAction(DelegateAction<string> action)
        {
            this._errorActions.Add(action);
        }


        public void InvokeMatchedOrDefaultAction(ValueOrError<T1> inputArgument)
        {
            if (inputArgument.IsError)
            {
                if (_errorActions.Count != 0)
                {
                    _errorActions[0](inputArgument.Error);
                }
                else
                {
                    _matchNotFoundAction();
                }
            }
            else
            {
                var action =
                    _testsAndActions
                        .Slinq()
                        .FirstOrNone((matcher, param) => matcher.Item1(param.Value), inputArgument);

                if (action.isSome)
                {
                    action.value.Item2(inputArgument.Value);
                }
                else if (_onValueDefaultAction.isSome)
                {
                    _onValueDefaultAction.value(inputArgument.Value);
                }
                else
                {
                    _matchNotFoundAction();
                }
            }
        }
    }
}