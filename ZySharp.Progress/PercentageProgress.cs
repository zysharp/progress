using System;
using System.Globalization;

using ZySharp.Progress.Internal;
using ZySharp.Validation;

namespace ZySharp.Progress
{
    /// <summary>
    /// A progress handler that converts numeric progress values into percentage progress.
    /// </summary>
    /// <typeparam name="T">
    ///     The input progress value type (numeric only). The input values for the <see cref="IProgress{T}.Report"/>
    ///     method are expected to be in a range between 'min..max'.
    /// </typeparam>
    public sealed class PercentageProgress<T> :
        ChainedProgressBase<T, double>
        where T : struct, IConvertible, IComparable, IComparable<T>, IEquatable<T>
    {
        private readonly double _min;
        private readonly double _max;
        private readonly double _total;

        /// <summary>
        /// The minimum progress value. Used to calculate the percentage progress.
        /// </summary>
        public T MinProgressValue { get; }

        /// <summary>
        /// The maximum progress value. Used to calculate the percentage progress.
        /// </summary>
        public T MaxProgressValue { get; }

        /// <summary>
        /// Set `true` to automatically adjust input values below the minimum- or above the maximum-value prior to the
        /// progress calculation.
        /// </summary>
        public bool SanitizeInputValues { get; set; } = true;

        static PercentageProgress()
        {
            var input = typeof(T);
            if (!input.IsNumeric())
            {
                throw new NotSupportedException(Resources.InputTypeMustBeNumeric);
            }
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="nextHandler">The next progress handler in the chain.</param>
        /// <param name="minProgressValue">The minimum progress value. Used to calculate the percentage progress.</param>
        /// <param name="maxProgressValue">The maximum progress value. Used to calculate the percentage progress.</param>
        public PercentageProgress(IProgress<double> nextHandler, T minProgressValue, T maxProgressValue) : base(nextHandler)
        {
            ValidateArgument.For(maxProgressValue, nameof(maxProgressValue))
                .GreaterThan(minProgressValue);

            MinProgressValue = minProgressValue;
            MaxProgressValue = maxProgressValue;

            _min = Convert.ToDouble(minProgressValue, CultureInfo.InvariantCulture);
            _max = Convert.ToDouble(maxProgressValue, CultureInfo.InvariantCulture);
            _total = (_max - _min);
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="action">The action to execute when a progress value is reported.</param>
        /// <param name="minProgressValue">The minimum progress value. Used to calculate the percentage progress.</param>
        /// <param name="maxProgressValue">The maximum progress value. Used to calculate the percentage progress.</param>
        public PercentageProgress(Action<double> action, T minProgressValue, T maxProgressValue) : base(action)
        {
            ValidateArgument.For(maxProgressValue, nameof(maxProgressValue))
                .GreaterThan(minProgressValue);

            MinProgressValue = minProgressValue;
            MaxProgressValue = maxProgressValue;

            _min = Convert.ToDouble(minProgressValue, CultureInfo.InvariantCulture);
            _max = Convert.ToDouble(maxProgressValue, CultureInfo.InvariantCulture);
            _total = (_max - _min);
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="minProgressValue">The minimum progress value. Used to calculate the percentage progress.</param>
        /// <param name="maxProgressValue">The maximum progress value. Used to calculate the percentage progress.</param>
        public PercentageProgress(T minProgressValue, T maxProgressValue)
        {
            ValidateArgument.For(maxProgressValue, nameof(maxProgressValue))
                .GreaterThan(minProgressValue);

            MinProgressValue = minProgressValue;
            MaxProgressValue = maxProgressValue;

            _min = Convert.ToDouble(minProgressValue, CultureInfo.InvariantCulture);
            _max = Convert.ToDouble(maxProgressValue, CultureInfo.InvariantCulture);
            _total = (_max - _min);
        }

        /// <inheritdoc cref="ChainedProgressBase{TInput,TOutput}.Report"/>
        public override void Report(T value)
        {
            var v = Convert.ToDouble(value, CultureInfo.InvariantCulture);

            if (SanitizeInputValues)
            {
                if (v < _min)
                {
                    v = _min;
                }

                if (v > _max)
                {
                    v = _max;
                }
            }

            var progress = ((v - _min) * 100.0d) / _total;

            ReportNext(progress);
        }
    }
}