namespace bbv.Common.EventBroker.Internals
{
    using System;
    using System.Collections.Generic;

    public class InlineExpressionProvider<TQuestion, TAnswer, TParameter, TExpressionResult> : IExpressionProvider<TQuestion, TAnswer, TParameter, TExpressionResult>
            where TQuestion : IQuestion<TAnswer, TParameter>
    {
        private Func<TQuestion, TParameter, TExpressionResult> expression;

        public InlineExpressionProvider(Func<TQuestion, TParameter, TExpressionResult> expression)
        {
            this.expression = expression;
        }

        public IEnumerable<IExpression<TExpressionResult, TParameter>> GetExpressions(TQuestion question)
        {
            return new[] { new InlineExpression<TQuestion, TParameter, TExpressionResult>(question, this.expression) };
        }
    }
}