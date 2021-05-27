using System;

namespace ZySharp.Progress
{
    /// <summary>
    /// A progress handler that reports only certain progress values based on a filter condition.
    /// </summary>
    /// <typeparam name="T">The progress value type.</typeparam>
    public abstract class FilteredProgressBase<T> :
        ChainedProgressBase<T, T>
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="nextHandler">The next progress handler in the chain.</param>
        protected FilteredProgressBase(IProgress<T> nextHandler) : base(nextHandler)
        {
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="action">The action to execute when a progress value is reported.</param>
        protected FilteredProgressBase(Action<T> action) : base(action)
        {
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        protected FilteredProgressBase()
        {
        }

        /// <inheritdoc cref="ChainedProgressBase{TInput,TOutput}.Report"/>
        public override void Report(T value)
        {
            if (!ShouldReport(value))
            {
                return;
            }

            ReportNext(value);
        }

        /// <summary>
        /// Checks if the given value should be reported to the next progress-handler.
        /// </summary>
        /// <param name="value">The current value.</param>
        /// <returns>`True`, if the given value should be reported or false, if not.</returns>
        protected abstract bool ShouldReport(T value);
    }
}