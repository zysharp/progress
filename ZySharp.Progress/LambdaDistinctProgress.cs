using System;

using ZySharp.Validation;

namespace ZySharp.Progress
{
    /// <summary>
    /// A progress handler that reports only distinct progress values based on a comparator lambda.
    /// </summary>
    /// <typeparam name="T">The progress value type.</typeparam>
    public sealed class LambdaDistinctProgress<T> :
        DistinctProgressBase<T>
    {
        private readonly Func<T, T, bool> _isEqualValue;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="nextHandler">The next progress handler in the chain.</param>
        /// <param name="isEqualValue">The equality comparer lambda.</param>
        public LambdaDistinctProgress(IProgress<T> nextHandler, Func<T, T, bool> isEqualValue) : base(nextHandler)
        {
            ValidateArgument.For(isEqualValue, nameof(isEqualValue))
                .NotNull();

            _isEqualValue = isEqualValue;
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="action">The action to execute when a progress value is reported.</param>
        /// <param name="isEqualValue">The equality comparer lambda.</param>
        public LambdaDistinctProgress(Action<T> action, Func<T, T, bool> isEqualValue) : base(action)
        {
            ValidateArgument.For(isEqualValue, nameof(isEqualValue))
                .NotNull();

            _isEqualValue = isEqualValue;
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="isEqualValue">The equality comparer lambda.</param>
        public LambdaDistinctProgress(Func<T, T, bool> isEqualValue)
        {
            ValidateArgument.For(isEqualValue, nameof(isEqualValue))
                .NotNull();

            _isEqualValue = isEqualValue;
        }

        /// <inheritdoc cref="DistinctProgressBase{T}.ShouldReport"/>
        protected override bool ShouldReport(T lastValue, T currentValue)
        {
            return !_isEqualValue(lastValue, currentValue);
        }
    }
}