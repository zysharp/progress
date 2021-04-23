using System;
using System.Diagnostics.Contracts;
using System.Globalization;

using ZySharp.Progress.Internal;
using ZySharp.Validation;

namespace ZySharp.Progress
{
    /// <summary>
    /// A progress handler that handles progress for multi-step operations where each individual step is represented
    /// by a simple integer value.
    /// <para>
    ///     The total number of steps can be increased during runtime to provide maximum flexibility for actions
    ///     where the total amount of steps is unknown at the beginning of the operation.
    /// </para>
    /// <typeparam name="TInput">
    ///     The input progress value type (numeric only). The input values for the <see cref="IProgress{T}.Report"/>
    ///     method are expected to be in a range between '0..100'.
    /// </typeparam>
    /// <typeparam name="TOutput">The output progress value type.</typeparam>
    /// </summary>
    public class MultiStepProgress<TInput, TOutput> :
        ProjectionProgressBase<TInput, TOutput>
        where TInput : struct, IConvertible, IComparable, IComparable<TInput>, IEquatable<TInput>
        where TOutput : class, IMultiStepProgressValue, new()
    {
        private TOutput _progress;

        public TOutput Progress => (TOutput)_progress.Clone();

        static MultiStepProgress()
        {
            var input = typeof(TInput);
            if (!input.IsNumeric())
            {
                throw new NotSupportedException(Resources.InputTypeMustBeNumeric);
            }
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="nextHandler">The next progress handler in the chain.</param>
        /// <param name="totalSteps">The initial total amount of steps.</param>
        public MultiStepProgress(IProgress<TOutput> nextHandler, int totalSteps) : base(nextHandler)
        {
            ValidateArgument.For(totalSteps, nameof(totalSteps))
                .GreaterThan(0);

            Init(totalSteps);
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="action">The action to execute when a progress value is reported.</param>
        /// <param name="totalSteps">The initial total amount of steps.</param>
        public MultiStepProgress(Action<TOutput> action, int totalSteps) : base(action)
        {
            ValidateArgument.For(totalSteps, nameof(totalSteps))
                .GreaterThan(0);

            Init(totalSteps);
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="totalSteps">The initial total amount of steps.</param>
        public MultiStepProgress(int totalSteps)
        {
            ValidateArgument.For(totalSteps, nameof(totalSteps))
                .GreaterThan(0);

            Init(totalSteps);
        }

        /// <summary>
        /// Sets a new amount of total steps. Can be used at any time, but does not support decreasing the amount of
        /// total steps.
        /// <para>
        ///     This method causes a progress update.
        /// </para>
        /// </summary>
        /// <param name="totalSteps">The new amount of total steps.</param>
        public void SetTotalSteps(int totalSteps)
        {
            ValidateArgument.For(totalSteps, nameof(totalSteps))
                .GreaterThan(0)
                .GreaterThanOrEqualTo(_progress.TotalSteps);

            if (totalSteps == _progress.TotalSteps)
            {
                return;
            }

            _progress.TotalSteps = totalSteps;

            Report((TInput)Convert.ChangeType(_progress.CurrentProgress, typeof(TInput), CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Sets the current step.
        /// <para>
        ///     This method causes a progress update.
        /// </para>
        /// </summary>
        /// <param name="currentStep">The new current step.</param>
        /// <param name="stepName">An optional name that describes the current step.</param>
        public void SetCurrentStep(int currentStep, string stepName = null)
        {
            ValidateArgument.For(currentStep, nameof(currentStep))
                .InRange(1, _progress.TotalSteps);

            if (currentStep == _progress.CurrentStep)
            {
                return;
            }

            _progress.CurrentStep = currentStep;
            _progress.CurrentStepName = stepName;

            Report(default);
        }

        /// <summary>
        /// Increases the current step counter.
        /// <para>
        ///     This method causes a progress update.
        /// </para>
        /// <param name="stepName">An optional name that describes the current step.</param>
        /// </summary>
        public void NextStep(string stepName = null)
        {
            SetCurrentStep(_progress.CurrentStep + 1, stepName);
        }

        protected override TOutput Transform(TInput value)
        {
            if (_progress.CurrentStep == 0)
            {
                throw new InvalidOperationException(Resources.MultiStepProgressNotStarted);
            }

            var v = Convert.ToDouble(value, CultureInfo.InvariantCulture);
            if (v < 0.0d)
            {
                v = 0.0d;
            }
            if (v > 100.0d)
            {
                v = 100.0d;
            }

            _progress.CurrentProgress = v;
            _progress.TotalProgress = ((double)(_progress.CurrentStep - 1) / _progress.TotalSteps * 100) +
                                      (v / _progress.TotalSteps);

            return Progress;
        }

        private void Init(int totalSteps)
        {
            Contract.Assert(totalSteps > 0);

            _progress = new TOutput
            {
                TotalSteps = totalSteps,
                CurrentStep = 0,
                TotalProgress = 0.0d,
                CurrentProgress = 0.0d
            };
        }
    }

    /// <inheritdoc cref="MultiStepProgress{TInput,TOutput}"/>
    public sealed class MultiStepProgress<TInput> :
        MultiStepProgress<TInput, MultiStepProgressValue>
        where TInput : struct, IConvertible, IComparable, IComparable<TInput>, IEquatable<TInput>
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="nextHandler">The next progress handler in the chain.</param>
        /// <param name="totalSteps">The initial total amount of steps.</param>
        public MultiStepProgress(IProgress<MultiStepProgressValue> nextHandler, int totalSteps) : base(nextHandler, totalSteps)
        {
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="action">The action to execute when a progress value is reported.</param>
        /// <param name="totalSteps">The initial total amount of steps.</param>
        public MultiStepProgress(Action<MultiStepProgressValue> action, int totalSteps) : base(action, totalSteps)
        {
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="totalSteps">The initial total amount of steps.</param>
        public MultiStepProgress(int totalSteps) : base(totalSteps)
        {
        }
    }
}