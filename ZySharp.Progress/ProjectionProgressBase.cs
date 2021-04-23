using System;

namespace ZySharp.Progress
{
    /// <summary>
    /// A progress handler that projects an input progress value to an output progress value.
    /// </summary>
    /// <typeparam name="TInput">The input progress value type.</typeparam>
    /// <typeparam name="TOutput">The output progress value type.</typeparam>
    public abstract class ProjectionProgressBase<TInput, TOutput> :
        ChainedProgressBase<TInput, TOutput>
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="nextHandler">The next progress handler in the chain.</param>
        protected ProjectionProgressBase(IProgress<TOutput> nextHandler) : base(nextHandler)
        {
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="action">The action to execute when a progress value is reported.</param>
        protected ProjectionProgressBase(Action<TOutput> action) : base(action)
        {
        }

        protected ProjectionProgressBase()
        {
        }

        public override void Report(TInput value)
        {
            ReportNext(Transform(value));
        }

        protected abstract TOutput Transform(TInput value);
    }
}