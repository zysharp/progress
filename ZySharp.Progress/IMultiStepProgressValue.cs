namespace ZySharp.Progress
{
    /// <summary>
    /// Base interface for all multi-step progress-value classes.
    /// </summary>
    public interface IMultiStepProgressValue
    {
        /// <summary>
        /// The total amount of steps/operations.
        /// </summary>
        int TotalSteps { get; set; }

        /// <summary>
        /// The current step/operation.
        /// </summary>
        int CurrentStep { get; set; }

        /// <summary>
        /// The name of the current step/operation.
        /// </summary>
        string CurrentStepName { get; set; }

        /// <summary>
        /// The progress of the current step/operation.
        /// </summary>
        double CurrentProgress { get; set; }

        /// <summary>
        /// The total progress.
        /// </summary>
        double TotalProgress { get; set; }

        /// <summary>
        /// Clones the current object.
        /// </summary>
        /// <returns>A deep clone of the current object.</returns>
        object Clone();
    }
}