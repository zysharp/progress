using System;

namespace ZySharp.Progress
{
    /// <summary>
    /// Base interface for all multi-step enum progress-value classes.
    /// </summary>
    public interface IMultiStepEnumProgressValue<TEnum> :
        IMultiStepProgressValue
        where TEnum : Enum
    {
        /// <summary>
        /// The enum value of the current step.
        /// </summary>
        TEnum CurrentStepValue { get; set; }
    }
}