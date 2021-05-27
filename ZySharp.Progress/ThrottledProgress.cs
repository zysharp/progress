using System;

namespace ZySharp.Progress
{
    /// <summary>
    /// A progress handler that throttles the progress report frequency.
    /// </summary>
    /// <typeparam name="T">The type of the progress value.</typeparam>
    public sealed class ThrottledProgress<T> :
        ChainedProgressBase<T, T>
    {
        private DateTime _lastReportTime = DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(1));

        /// <summary>
        /// The minimum interval to wait between two progress value updates.
        /// </summary>
        public TimeSpan MinReportInterval { get; set; } = TimeSpan.FromSeconds(1);

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="nextHandler">The next progress handler in the chain.</param>
        public ThrottledProgress(IProgress<T> nextHandler) : base(nextHandler)
        {
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="action">The action to execute when a progress value is reported.</param>
        public ThrottledProgress(Action<T> action) : base(action)
        {
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        public ThrottledProgress()
        {
        }

        /// <inheritdoc cref="ChainedProgressBase{TInput,TOutput}.Report"/>
        public override void Report(T value)
        {
            var shouldReport = (DateTime.UtcNow - _lastReportTime) >= MinReportInterval;
            if (!shouldReport)
            {
                return;
            }

            ReportNext(value);

            _lastReportTime = DateTime.UtcNow;
        }
    }
}