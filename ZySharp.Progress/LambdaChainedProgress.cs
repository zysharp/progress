using System;

using ZySharp.Validation;

namespace ZySharp.Progress
{
    /// <summary>
    /// A progress handler that reports progress updates to another progress handler of the same type
    /// after invoking a lambda callback.
    /// </summary>
    /// <typeparam name="T">The type of the progress value.</typeparam>
    public sealed class LambdaChainedProgress<T> :
        ChainedProgressBase<T, T>
    {
        private readonly Action<T> _callback;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="nextHandler">The next progress handler in the chain.</param>
        /// <param name="callback">The callback to execute when a progress value is reported.</param>
        public LambdaChainedProgress(IProgress<T> nextHandler, Action<T> callback) : base(nextHandler)
        {
            ValidateArgument.For(callback, nameof(callback), v => v.NotNull());

            _callback = callback;
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="action">The action to execute when a progress value is reported.</param>
        /// <param name="callback">The callback to execute when a progress value is reported.</param>
        public LambdaChainedProgress(Action<T> action, Action<T> callback) : base(action)
        {
            ValidateArgument.For(callback, nameof(callback), v => v.NotNull());

            _callback = callback;
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="callback">The callback to execute when a progress value is reported.</param>
        public LambdaChainedProgress(Action<T> callback)
        {
            ValidateArgument.For(callback, nameof(callback), v => v.NotNull());

            _callback = callback;
        }

        /// <inheritdoc cref="ChainedProgressBase{TInput,TOutput}.Report"/>
        public override void Report(T value)
        {
            _callback(value);
            ReportNext(value);
        }
    }
}