using System;
using System.Diagnostics.Contracts;

using ZySharp.Validation;

namespace ZySharp.Progress.Builder
{
    /// <summary>
    /// Represents a progress-builder context that is used to provide fluent-api functionality.
    /// </summary>
    /// <typeparam name="TResult">The type of the resulting handlers progress value.</typeparam>
    /// <typeparam name="TPrevious">The type of the previous progress value.</typeparam>
    /// <typeparam name="TCurrent">The type of the current progress value.</typeparam>
    public sealed class ProgressBuilderContext<TResult, TPrevious, TCurrent>
    {
        private IProgress<TResult> _result;

        private readonly IChainedProgress<TPrevious, TCurrent> _nextHandler;

        internal ProgressBuilderContext()
        {
            // Hide default constructor
        }

        internal ProgressBuilderContext(IProgress<TResult> result, IChainedProgress<TPrevious, TCurrent> nextHandler)
        {
            Contract.Assert(result != null);
            Contract.Assert(nextHandler != null);

            _result = result;
            _nextHandler = nextHandler;
        }

        /// <summary>
        /// Appends a new handler to the chain.
        /// </summary>
        /// <typeparam name="TNext">The output progress value type of the new handler.</typeparam>
        /// <param name="nextHandler">The progress handler to append.</param>
        /// <returns>A new <see cref="ProgressBuilderContext{TResult,TPrevious,TCurrent}"/> instance.</returns>
        public ProgressBuilderContext<TResult, TCurrent, TNext> Append<TNext>(IChainedProgress<TCurrent, TNext> nextHandler)
        {
            ValidateArgument.For(nextHandler, nameof(nextHandler), v => v.NotNull());

            // As the type of `TResult` is guaranteed to be the type of `TCurrent` for the first builder instance, this
            // cast will always succeed.
            _result ??= nextHandler as IProgress<TResult>;

            if (_nextHandler != null)
            {
                _nextHandler.NextHandler = nextHandler;
            }

            return new ProgressBuilderContext<TResult, TCurrent, TNext>(_result, nextHandler);
        }

        /// <summary>
        /// Adds a final progress-handler and returns the first handler in the chain.
        /// </summary>
        /// <param name="finalHandler">The final progress handler.</param>
        /// <returns>The first progress-handler in the chain.</returns>
        public IProgress<TResult> Build(IProgress<TCurrent> finalHandler)
        {
            ValidateArgument.For(finalHandler, nameof(finalHandler), v => v.NotNull());

            // As the type of `TResult` is guaranteed to be the type of `TCurrent` for the first builder instance, this
            // cast will always succeed.
            _result ??= finalHandler as IProgress<TResult>;

            if (_nextHandler != null)
            {
                _nextHandler.NextHandler = finalHandler;
            }

            return _result;
        }
    }
}