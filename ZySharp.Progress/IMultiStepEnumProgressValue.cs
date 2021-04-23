using System;

namespace ZySharp.Progress
{
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