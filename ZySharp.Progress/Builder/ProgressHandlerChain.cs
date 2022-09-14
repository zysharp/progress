using System;
using System.Collections.Generic;

using ZySharp.Validation;

namespace ZySharp.Progress.Builder
{
    /// <summary>
    /// Provides a fluent API for the creation of progress-handler chains.
    /// </summary>
    public static class ProgressHandlerChain
    {
        /// <summary>
        /// Creates a new progress-handler chain.
        /// </summary>
        /// <typeparam name="TR">The type of the resulting handlers progress value.</typeparam>
        /// <returns>A new <see cref="ProgressBuilderContext{TResult,TPrevious,TCurrent}"/> instance.</returns>
        public static ProgressBuilderContext<TR, TR, TR> Create<TR>()
        {
            return new();
        }

        /// <summary>
        /// Returns the first progress handler in the resulting chain.
        /// <para>
        ///     This method adds a <see cref="LambdaProgress{T}"/> handler to the chain.
        /// </para>
        /// </summary>
        /// <typeparam name="TR">The type of the resulting handlers progress value.</typeparam>
        /// <typeparam name="TP">The type of the previous progress value.</typeparam>
        /// <typeparam name="TC">The type of the current progress value.</typeparam>
        /// <param name="context">The current <see cref="ProgressBuilderContext{TResult,TPrevious,TCurrent}"/> instance.</param>
        /// <param name="action">The final action of the progress handler chain.</param>
        /// <returns>The first progress handler in the resulting chain.</returns>
        public static IProgress<TR> Build<TR, TP, TC>(this ProgressBuilderContext<TR, TP, TC> context,
            Action<TC> action)
        {
            ValidateArgument.For(context, nameof(context), v => v.NotNull());
            ValidateArgument.For(action, nameof(action), v => v.NotNull());

            return context.Build(new LambdaProgress<TC>(action));
        }

        /// <summary>
        /// Executes a callback when a progress values is reported.
        /// <para>
        ///     This method adds a <see cref="LambdaChainedProgress{T}"/> handler to the chain.
        /// </para>
        /// </summary>
        /// <typeparam name="TR">The type of the resulting handlers progress value.</typeparam>
        /// <typeparam name="TP">The type of the previous progress value.</typeparam>
        /// <typeparam name="TC">The type of the current progress value.</typeparam>
        /// <param name="context">The current <see cref="ProgressBuilderContext{TResult,TPrevious,TCurrent}"/> instance.</param>
        /// <param name="callback">The callback to execute when a progress value is reported.</param>
        /// <returns>A <see cref="ProgressBuilderContext{TResult,TPrevious,TCurrent}"/> instance.</returns>
        public static ProgressBuilderContext<TR, TC, TC> Call<TR, TP, TC>(this ProgressBuilderContext<TR, TP, TC> context,
            Action<TC> callback)
        {
            ValidateArgument.For(context, nameof(context), v => v.NotNull());

            return context.Append(new LambdaChainedProgress<TC>(callback));
        }

        /// <summary>
        /// Throttles the progress reporting to a specified minimum report interval.
        /// <para>
        ///     This method adds a <see cref="ThrottledProgress{T}"/> handler to the chain.
        /// </para>
        /// </summary>
        /// <param name="context">The current <see cref="ProgressBuilderContext{TResult,TPrevious,TCurrent}"/> instance.</param>
        /// <param name="minReportInterval">The minimum report interval.</param>
        /// <returns>A <see cref="ProgressBuilderContext{TResult,TPrevious,TCurrent}"/> instance.</returns>
        public static ProgressBuilderContext<TR, TC, TC> Throttle<TR, TP, TC>(this ProgressBuilderContext<TR, TP, TC> context,
            TimeSpan minReportInterval)
        {
            ValidateArgument.For(context, nameof(context), v => v.NotNull());

            return context.Append(new ThrottledProgress<TC>
            {
                MinReportInterval = minReportInterval
            });
        }

        /// <summary>
        /// Prevents the same progress value from being reported multiple times.
        /// <para>
        ///     This methods adds a <see cref="DistinctProgress{T}"/> handler to the chain.
        /// </para>
        /// </summary>
        /// <typeparam name="TR">The type of the resulting handlers progress value.</typeparam>
        /// <typeparam name="TP">The type of the previous progress value.</typeparam>
        /// <typeparam name="TC">The type of the current progress value.</typeparam>
        /// <param name="context">The current <see cref="ProgressBuilderContext{TResult,TPrevious,TCurrent}"/> instance.</param>
        /// <param name="forceReportInterval">
        ///     An optional interval after which the progress report is enforced even if the value did not change.
        /// </param>
        /// <returns>A <see cref="ProgressBuilderContext{TResult,TPrevious,TCurrent}"/> instance.</returns>
        public static ProgressBuilderContext<TR, TC, TC> Distinct<TR, TP, TC>(this ProgressBuilderContext<TR, TP, TC> context,
            TimeSpan? forceReportInterval = null)
            where TC : IEquatable<TC>
        {
            ValidateArgument.For(context, nameof(context), v => v.NotNull());

            return context.Append(new DistinctProgress<TC>(EqualityComparer<TC>.Default)
            {
                ForceReportInterval = forceReportInterval
            });
        }

        /// <summary>
        /// Prevents the same progress value from being reported multiple times.
        /// <para>
        ///     This methods adds a <see cref="DistinctProgress{T}"/> handler to the chain.
        /// </para>
        /// </summary>
        /// <typeparam name="TR">The type of the resulting handlers progress value.</typeparam>
        /// <typeparam name="TP">The type of the previous progress value.</typeparam>
        /// <typeparam name="TC">The type of the current progress value.</typeparam>
        /// <param name="context">The current <see cref="ProgressBuilderContext{TResult,TPrevious,TCurrent}"/> instance.</param>
        /// <param name="equalityComparer">The equality comparer instance to use.</param>
        /// <param name="forceReportInterval">
        ///     An optional interval after which the progress report is enforced even if the value did not change.
        /// </param>
        /// <returns>A <see cref="ProgressBuilderContext{TResult,TPrevious,TCurrent}"/> instance.</returns>
        public static ProgressBuilderContext<TR, TC, TC> Distinct<TR, TP, TC>(this ProgressBuilderContext<TR, TP, TC> context,
            IEqualityComparer<TC> equalityComparer, TimeSpan? forceReportInterval = null)
        {
            ValidateArgument.For(context, nameof(context), v => v.NotNull());

            return context.Append(new DistinctProgress<TC>(equalityComparer)
            {
                ForceReportInterval = forceReportInterval
            });
        }

        /// <summary>
        /// Prevents the same progress value from being reported multiple times.
        /// <para>
        ///     This methods adds a <see cref="LambdaDistinctProgress{T}"/> handler to the chain.
        /// </para>
        /// </summary>
        /// <typeparam name="TR">The type of the resulting handlers progress value.</typeparam>
        /// <typeparam name="TP">The type of the previous progress value.</typeparam>
        /// <typeparam name="TC">The type of the current progress value.</typeparam>
        /// <param name="context">The current <see cref="ProgressBuilderContext{TResult,TPrevious,TCurrent}"/> instance.</param>
        /// <param name="isEqualValue">The equality comparer lambda.</param>
        /// <param name="forceReportInterval">
        ///     An optional interval after which the progress report is enforced even if the value did not change.
        /// </param>
        /// <returns>A <see cref="ProgressBuilderContext{TResult,TPrevious,TCurrent}"/> instance.</returns>
        public static ProgressBuilderContext<TR, TC, TC> Distinct<TR, TP, TC>(this ProgressBuilderContext<TR, TP, TC> context,
            Func<TC, TC, bool> isEqualValue, TimeSpan? forceReportInterval = null)
        {
            ValidateArgument.For(context, nameof(context), v => v.NotNull());

            return context.Append(new LambdaDistinctProgress<TC>(isEqualValue)
            {
                ForceReportInterval = forceReportInterval
            });
        }

        /// <summary>
        /// Filters progress values using a lambda function.
        /// <para>
        ///     The method adds a <see cref="LambdaFilteredProgress{T}"/> handler to the chain.
        /// </para>
        /// </summary>
        /// <typeparam name="TR">The type of the resulting handlers progress value.</typeparam>
        /// <typeparam name="TP">The type of the previous progress value.</typeparam>
        /// <typeparam name="TC">The type of the current progress value.</typeparam>
        /// <param name="context">The current <see cref="ProgressBuilderContext{TResult,TPrevious,TCurrent}"/> instance.</param>
        /// <param name="filter">The filter lambda.</param>
        /// <returns>A <see cref="ProgressBuilderContext{TResult,TPrevious,TCurrent}"/> instance.</returns>
        public static ProgressBuilderContext<TR, TC, TC> Where<TR, TP, TC>(this ProgressBuilderContext<TR, TP, TC> context,
            Func<TC, bool> filter)
        {
            ValidateArgument.For(context, nameof(context), v => v.NotNull());

            return context.Append(new LambdaFilteredProgress<TC>(filter));
        }

        /// <summary>
        /// Projects progress values using a lambda function.
        /// <para>
        ///     This method adds a <see cref="LambdaProjectionProgress{TInput,TOutput}"/> handler to the chain.
        /// </para>
        /// </summary>
        /// <typeparam name="TR">The type of the resulting handlers progress value.</typeparam>
        /// <typeparam name="TP">The type of the previous progress value.</typeparam>
        /// <typeparam name="TC">The type of the current progress value.</typeparam>
        /// <typeparam name="TN">The type of the next progress value.</typeparam>
        /// <param name="context">The current <see cref="ProgressBuilderContext{TResult,TPrevious,TCurrent}"/> instance.</param>
        /// <param name="selector">The selector lambda.</param>
        /// <returns>A <see cref="ProgressBuilderContext{TResult,TPrevious,TCurrent}"/> instance.</returns>
        public static ProgressBuilderContext<TR, TC, TN> Select<TR, TP, TC, TN>(this ProgressBuilderContext<TR, TP, TC> context,
            Func<TC, TN> selector)
        {
            ValidateArgument.For(context, nameof(context), v => v.NotNull());

            return context.Append(new LambdaProjectionProgress<TC, TN>(selector));
        }

        /// <summary>
        /// Converts numeric progress values to percentage progress.
        /// <para>
        ///     This method adds a <see cref="PercentageProgress{T}"/> handler to the chain.
        /// </para>
        /// </summary>
        /// <typeparam name="TR">The type of the resulting handlers progress value.</typeparam>
        /// <typeparam name="TP">The type of the previous progress value.</typeparam>
        /// <typeparam name="TC">The type of the current progress value.</typeparam>
        /// <param name="context">The current <see cref="ProgressBuilderContext{TResult,TPrevious,TCurrent}"/> instance.</param>
        /// <param name="minProgressValue">The minimum progress value.</param>
        /// <param name="maxProgressValue">The maximum progress value.</param>
        /// <returns>A <see cref="ProgressBuilderContext{TResult,TPrevious,TCurrent}"/> instance.</returns>
        public static ProgressBuilderContext<TR, TC, double> ToPercent<TR, TP, TC>(this ProgressBuilderContext<TR, TP, TC> context,
            TC minProgressValue, TC maxProgressValue)
            where TC : struct, IConvertible, IComparable, IComparable<TC>, IEquatable<TC>
        {
            ValidateArgument.For(context, nameof(context), v => v.NotNull());

            return context.Append(new PercentageProgress<TC>(minProgressValue, maxProgressValue));
        }

        /// <summary>
        /// Converts multi-step progress values to percentage progress.
        /// <para>
        ///     This method adds a <see cref="LambdaProjectionProgress{TInput,TOutput}"/> handler to the chain.
        /// </para>
        /// </summary>
        /// <typeparam name="TR">The type of the resulting handlers progress value.</typeparam>
        /// <typeparam name="TP">The type of the previous progress value.</typeparam>
        /// <typeparam name="TC">The type of the current progress value.</typeparam>
        /// <param name="context">The current <see cref="ProgressBuilderContext{TResult,TPrevious,TCurrent}"/> instance.</param>
        /// <returns>A <see cref="ProgressBuilderContext{TResult,TPrevious,TCurrent}"/> instance.</returns>
        public static ProgressBuilderContext<TR, TC, double> ToPercent<TR, TP, TC>(this ProgressBuilderContext<TR, TP, TC> context)
            where TC : IMultiStepProgressValue
        {
            ValidateArgument.For(context, nameof(context), v => v.NotNull());

            return context.Append(new LambdaProjectionProgress<TC, double>(x => x.TotalProgress));
        }
    }
}