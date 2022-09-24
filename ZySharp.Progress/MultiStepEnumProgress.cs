using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;

using ZySharp.Progress.Internal;
using ZySharp.Validation;

namespace ZySharp.Progress
{
    /// <summary>
    /// A progress handler that handles progress for multi-step operations where each individual step is represented
    /// by an enum value.
    /// <typeparam name="TInput">
    ///     The input progress value type (numeric only). The input values for the <see cref="IProgress{T}.Report"/>
    ///     method are expected to be in a range between '0..100'.
    /// </typeparam>
    /// <typeparam name="TOutput">The output progress value type.</typeparam>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// </summary>
    public class MultiStepEnumProgress<TInput, TOutput, TEnum> :
        ProjectionProgressBase<TInput, TOutput>
        where TInput : struct, IConvertible, IComparable, IComparable<TInput>, IEquatable<TInput>
        where TOutput : class, IMultiStepEnumProgressValue<TEnum>, new()
        where TEnum : Enum
    {
        // ReSharper disable once StaticMemberInGenericType
        private static readonly Tuple<TEnum, string>[] EnumInfo;

        private readonly TOutput _progress = new()
        {
            TotalSteps = EnumInfo.Length,
            CurrentStep = 0,
            CurrentStepValue = default!,
            CurrentProgress = 0.0d,
            TotalProgress = 0.0d
        };

        /// <summary>
        /// The current progress object.
        /// </summary>
        public TOutput Progress => (TOutput)_progress.Clone();

        static MultiStepEnumProgress()
        {
            var input = typeof(TInput);
            if (!input.IsNumeric())
            {
                throw new NotSupportedException(Resources.InputTypeMustBeNumeric);
            }

            var type = typeof(TEnum);

            var isFlagEnum = type.IsDefined(typeof(FlagsAttribute), true);
            if (isFlagEnum)
            {
                throw new NotSupportedException(Resources.NotSupportedEnumFlags);
            }

            var values = (TEnum[])Enum.GetValues(type);
            if (values.Length == 0)
            {
                throw new NotSupportedException(Resources.NotSupportedEnumEmpty);
            }
            if (values.GroupBy(x => x).Any(x => x.Count() > 1))
            {
                throw new NotSupportedException(Resources.NotSupportedEnumDuplicateValues);
            }

            var names = Enum.GetNames(type);

            Contract.Assert(values.Length == names.Length);
            EnumInfo = values.Select((x, i) => new Tuple<TEnum, string>(x, names[i])).ToArray();
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="nextHandler">The next progress handler in the chain.</param>
        public MultiStepEnumProgress(IProgress<TOutput> nextHandler) : base(nextHandler)
        {
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="action">The action to execute when a progress value is reported.</param>
        public MultiStepEnumProgress(Action<TOutput> action) : base(action)
        {
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        public MultiStepEnumProgress()
        {
        }

        /// <summary>
        /// Sets the current step.
        /// <para>
        ///     This method causes a progress update.
        /// </para>
        /// </summary>
        /// <param name="value">The new current step.</param>
        public void SetCurrentStep(TEnum value)
        {
            if ((_progress.CurrentStep != 0) && value.Equals(Progress.CurrentStepValue))
            {
                return;
            }

            var info = EnumInfo.FirstOrDefault(x => x.Item1.Equals(value));
            ValidateArgument.For(info, nameof(value), v => v.NotNull());

            _progress.CurrentStep = Array.IndexOf(EnumInfo, info) + 1;
            _progress.CurrentStepValue = value;
            _progress.CurrentStepName = info!.Item2;

            Report(default);
        }

        /// <inheritdoc cref="ProjectionProgressBase{TInput,TOutput}.Transform"/>
        protected override TOutput Transform(TInput value)
        {
            if (_progress.CurrentStep == 0)
            {
                throw new InvalidOperationException("not started");
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

            return _progress;
        }
    }

    /// <inheritdoc cref="MultiStepEnumProgress{TInput,TOutput,TEnum}"/>
    public sealed class MultiStepEnumProgress<TInput, TEnum> :
        MultiStepEnumProgress<TInput, MultiStepEnumProgressValue<TEnum>, TEnum>
        where TInput : struct, IConvertible, IComparable, IComparable<TInput>, IEquatable<TInput>
        where TEnum : Enum
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="nextHandler">The next progress handler in the chain.</param>
        public MultiStepEnumProgress(IProgress<MultiStepEnumProgressValue<TEnum>> nextHandler) : base(nextHandler)
        {
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="action">The action to execute when a progress value is reported.</param>
        public MultiStepEnumProgress(Action<MultiStepEnumProgressValue<TEnum>> action) : base(action)
        {
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        public MultiStepEnumProgress()
        {
        }
    }
}