using System;

namespace ZySharp.Progress
{
    public class MultiStepEnumProgressValue<TEnum> :
        IMultiStepEnumProgressValue<TEnum>
        where TEnum : Enum
    {
        public int TotalSteps { get; set; }

        public int CurrentStep { get; set; }

        public string CurrentStepName { get; set; }

        public TEnum CurrentStepValue { get; set; }

        public double CurrentProgress { get; set; }

        public double TotalProgress { get; set; }

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