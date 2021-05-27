namespace ZySharp.Progress
{
    /// <summary>
    /// A reference implementation of the <see cref="IMultiStepProgressValue"/> interface.
    /// </summary>
    public class MultiStepProgressValue :
        IMultiStepProgressValue
    {
        /// <inheritdoc cref="IMultiStepProgressValue.TotalSteps"/>
        public int TotalSteps { get; set; }

        /// <inheritdoc cref="IMultiStepProgressValue.CurrentStep"/>
        public int CurrentStep { get; set; }

        /// <inheritdoc cref="IMultiStepProgressValue.CurrentStepName"/>
        public string CurrentStepName { get; set; }

        /// <inheritdoc cref="IMultiStepProgressValue.CurrentProgress"/>
        public double CurrentProgress { get; set; }

        /// <inheritdoc cref="IMultiStepProgressValue.TotalProgress"/>
        public double TotalProgress { get; set; }

        /// <inheritdoc cref="IMultiStepProgressValue.Clone"/>
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