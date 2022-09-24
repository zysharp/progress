using System;

namespace ZySharp.Progress
{
    /// <summary>
    /// A reference implementation of the <see cref="IMultiStepEnumProgressValue{TEnum}"/> interface.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum.</typeparam>
    public class MultiStepEnumProgressValue<TEnum> :
        IMultiStepEnumProgressValue<TEnum>
        where TEnum : Enum
    {
        /// <inheritdoc cref="IMultiStepProgressValue.TotalSteps"/>
        public int TotalSteps { get; set; }

        /// <inheritdoc cref="IMultiStepProgressValue.CurrentStep"/>
        public int CurrentStep { get; set; }

        /// <inheritdoc cref="IMultiStepProgressValue.CurrentStepName"/>
        public string? CurrentStepName { get; set; }

        /// <inheritdoc cref="IMultiStepEnumProgressValue{TEnum}.CurrentStepValue"/>
        public TEnum CurrentStepValue { get; set; } = default!;

        /// <inheritdoc cref="IMultiStepProgressValue.CurrentProgress"/>
        public double CurrentProgress { get; set; }

        /// <inheritdoc cref="IMultiStepProgressValue.TotalProgress"/>
        public double TotalProgress { get; set; }

        /// <inheritdoc cref="IMultiStepProgressValue.Clone"/>
        public virtual object Clone()
        {
            return new MultiStepEnumProgressValue<TEnum>
            {
                TotalSteps = this.TotalSteps,
                CurrentStep = this.CurrentStep,
                CurrentStepName = this.CurrentStepName,
                CurrentStepValue = this.CurrentStepValue,
                CurrentProgress = this.CurrentProgress,
                TotalProgress = this.TotalProgress
            };
        }
    }
}