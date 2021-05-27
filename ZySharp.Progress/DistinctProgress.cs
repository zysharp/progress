using System;
using System.Collections.Generic;

using ZySharp.Validation;

namespace ZySharp.Progress
{
    /// <summary>
    /// A progress handler that reports only distinct progress values.
    /// </summary>
    /// <typeparam name="T">The progress value type.</typeparam>
    public sealed class DistinctProgress<T> :
        DistinctProgressBase<T>
    {
        private readonly IEqualityComparer<T> _equalityComparer;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="nextHandler">The next progress handler in the chain.</param>
        /// <param name="equalityComparer">The equality comparer instance to use.</param>
        public DistinctProgress(IProgress<T> nextHandler, IEqualityComparer<T> equalityComparer = null) : base(nextHandler)
        {
            ValidateArgument.For(equalityComparer, nameof(equalityComparer))
                .NotNull();

            _equalityComparer = equalityComparer;
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="action">The action to execute when a progress value is reported.</param>
        /// <param name="equalityComparer">The equality comparer instance to use.</param>
        public DistinctProgress(Action<T> action, IEqualityComparer<T> equalityComparer = null) : base(action)
        {
            ValidateArgument.For(equalityComparer, nameof(equalityComparer))
                .NotNull();

            _equalityComparer = equalityComparer;
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="equalityComparer">The equality comparer instance to use.</param>
        public DistinctProgress(IEqualityComparer<T> equalityComparer = null)
        {
            ValidateArgument.For(equalityComparer, nameof(equalityComparer))
                .NotNull();

            _equalityComparer = equalityComparer;
        }

        /// <inheritdoc cref="DistinctProgressBase{T}.ShouldReport"/>
        protected override bool ShouldReport(T lastValue, T currentValue)
        {
            return !_equalityComparer.Equals(currentValue, lastValue);
        }
    }
}