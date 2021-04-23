using System;

namespace ZySharp.Progress
{
    /// <summary>
    /// The base interface for all chained progress-handler implementations.
    /// </summary>
    /// <typeparam name="TInput">The input progress value type.</typeparam>
    /// <typeparam name="TOutput">The output progress value type.</typeparam>
    public interface IChainedProgress<in TInput, TOutput> :
        IProgress<TInput>
    {
        public IProgress<TOutput> NextHandler { get; set; }
    }
}