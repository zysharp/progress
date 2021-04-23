namespace ZySharp.Progress
{
    public class MultiStepProgressValue :
        IMultiStepProgressValue
    {
        public int TotalSteps { get; set; }

        public int CurrentStep { get; set; }

        public string CurrentStepName { get; set; }

        public double CurrentProgress { get; set; }

        public double TotalProgress { get; set; }

        public virtual object Clone()
        {
            return new MultiStepProgressValue
            {
                TotalSteps = this.TotalSteps,
                CurrentStep = this.CurrentStep,
                CurrentStepName = this.CurrentStepName,
                CurrentProgress = this.CurrentProgress,
                TotalProgress = this.TotalProgress
            };
        }
    }
}