using System;

using ZySharp.Validation;

namespace ZySharp.Progress
{
    /// <summary>
    /// A progress handler implementation that executes a simple lambda action.
    /// </summary>
    /// <typeparam name="T">The progress value type.</typeparam>
    public sealed class LambdaProgress<T> :
        IProgress<T>
    {
        private readonly Action<T> _action;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="action">The action to execute when a progress value is reported.</param>
        public LambdaProgress(Action<T> action)
        {
            ValidateArgument.For(action, nameof(action), v => v.NotNull());

            _action = action;
        }

        /// <inheritdoc cref="IProgress{T}.Report"/>
        public void Report(T value)
        {
            _action(value);
        }
    }
}