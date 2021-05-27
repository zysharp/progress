using System;
using System.Globalization;

namespace ZySharp.Progress
{
    /// <summary>
    /// A progress handler that converts from an input progress value type to an output progress value type.
    /// </summary>
    /// <typeparam name="TInput">The input progress value type.</typeparam>
    /// <typeparam name="TOutput">The output progress value type.</typeparam>
    public sealed class ConversionProgress<TInput, TOutput> :
        ProjectionProgressBase<TInput, TOutput>
        where TInput : IConvertible
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="nextHandler">The next progress handler in the chain.</param>
        public ConversionProgress(IProgress<TOutput> nextHandler) : base(nextHandler)
        {
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="action">The action to execute when a progress value is reported.</param>
        public ConversionProgress(Action<TOutput> action) : base(action)
        {
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        public ConversionProgress()
        {
        }

        /// <inheritdoc cref="ProjectionProgressBase{TInput,TOutput}.Transform"/>
        protected override TOutput Transform(TInput value)
        {
            return (TOutput)Convert.ChangeType(value, typeof(TOutput), CultureInfo.InvariantCulture);
        }
    }
}