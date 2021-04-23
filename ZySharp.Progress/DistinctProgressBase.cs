using System;

namespace ZySharp.Progress
{
    /// <summary>
    /// A progress handler that reports only distinct progress values.
    /// </summary>
    /// <typeparam name="T">The progress value type.</typeparam>
    public abstract class DistinctProgressBase<T> :
        ChainedProgressBase<T, T>
    {
        private bool _isFirstReport = true;
        private T _lastValue;
        private DateTime _lastReportTime = DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(1));

        /// <summary>
        /// An optional interval after which the progress report is enforced even if the value did not change.
        /// </summary>
        public TimeSpan? ForceReportInterval { get; set; }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="nextHandler">The next progress handler in the chain.</param>
        protected DistinctProgressBase(IProgress<T> nextHandler) : base(nextHandler)
        {
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="action">The action to execute when a progress value is reported.</param>
        protected DistinctProgressBase(Action<T> action) : base(action)
        {
        }

        protected DistinctProgressBase()
        {
        }

        public override void Report(T value)
        {
            var forceUpdate = ForceReportInterval.HasValue && ((DateTime.UtcNow - _lastReportTime) >= ForceReportInterval);
            var shouldReport = _isFirstReport || forceUpdate || ShouldReport(_lastValue, value);

            if (!shouldReport)
            {
                return;
            }

            _lastReportTime = DateTime.UtcNow;
            _isFirstReport = false;
            _lastValue = value;

            ReportNext(value);
        }

        protected abstract bool ShouldReport(T lastValue, T currentValue);
    }
}