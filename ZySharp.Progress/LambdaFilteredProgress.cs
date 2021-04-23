using System;

using ZySharp.Validation;

namespace ZySharp.Progress
{
    /// <summary>
    /// A progress handler that reports only certain progress values based on a filter lambda.
    /// </summary>
    /// <typeparam name="T">The progress value type.</typeparam>
    public sealed class LambdaFilteredProgress<T> :
        FilteredProgressBase<T>
    {
        private readonly Func<T, bool> _filter;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="nextHandler">The next progress handler in the chain.</param>
        /// <param name="filter">The filter lambda.</param>
        public LambdaFilteredProgress(IProgress<T> nextHandler, Func<T, bool> filter) : base(nextHandler)
        {
            ValidateArgument.For(filter, nameof(filter))
                .NotNull();

            _filter = filter;
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="action">The action to execute when a progress value is reported.</param>
        /// <param name="filter">The filter lambda.</param>
        public LambdaFilteredProgress(Action<T> action, Func<T, bool> filter) : base(action)
        {
            ValidateArgument.For(filter, nameof(filter))
                .NotNull();

            _filter = filter;
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="filter">The filter lambda.</param>
        public LambdaFilteredProgress(Func<T, bool> filter)
        {
            ValidateArgument.For(filter, nameof(filter))
                .NotNull();

            _filter = filter;
        }

        protected override bool ShouldReport(T value)
        {
            return _filter(value);
        }
    }
}