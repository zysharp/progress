using System;

using ZySharp.Validation;

namespace ZySharp.Progress
{
    /// <summary>
    /// A progress handler that projects an input progress value to an output progress value based
    /// on a selector lambda.
    /// </summary>
    /// <typeparam name="TInput">The input progress value type.</typeparam>
    /// <typeparam name="TOutput">The output progress value type.</typeparam>
    public sealed class LambdaProjectionProgress<TInput, TOutput> :
        ProjectionProgressBase<TInput, TOutput>
    {
        private readonly Func<TInput, TOutput> _selector;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="nextHandler">The next progress handler in the chain.</param>
        /// <param name="selector">The selector lambda.</param>
        public LambdaProjectionProgress(IProgress<TOutput> nextHandler, Func<TInput, TOutput> selector) : base(nextHandler)
        {
            ValidateArgument.For(selector, nameof(selector), v => v.NotNull());

            _selector = selector;
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="action">The action to execute when a progress value is reported.</param>
        /// <param name="selector">The selector lambda.</param>
        public LambdaProjectionProgress(Action<TOutput> action, Func<TInput, TOutput> selector) : base(action)
        {
            ValidateArgument.For(selector, nameof(selector), v => v.NotNull());

            _selector = selector;
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="selector">The selector lambda.</param>
        public LambdaProjectionProgress(Func<TInput, TOutput> selector)
        {
            ValidateArgument.For(selector, nameof(selector), v => v.NotNull());

            _selector = selector;
        }

        /// <inheritdoc cref="ProjectionProgressBase{TInput,TOutput}.Transform"/>
        protected override TOutput Transform(TInput value)
        {
            return _selector(value);
        }
    }
}