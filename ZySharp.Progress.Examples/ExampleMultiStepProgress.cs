using System;

namespace ZySharp.Progress.Examples
{
    public static class ExampleMultiStepProgress
    {
        public static void Run()
        {
            var handler = new MultiStepProgress<int>(PrintProgress, 4);

            // Begin step 1 and report progress to the handler
            handler.SetCurrentStep(1, "Op1");
            ReportRandomPercentageProgress(handler);

            // Begin step 2 and do not report any progress
            handler.NextStep("Op2");
            ReportRandomPercentageProgress(handler);

            // Skip step 3

            // Increase number of total steps
            handler.SetTotalSteps(5);

            // Begin step 4 and report progress to the handler
            handler.SetCurrentStep(4, "Op4");
            ReportRandomPercentageProgress(handler);

            // Begin step 5 and report progress to the handler
            handler.NextStep("Op5");
            handler.Report(100);
        }

        public enum MultiStepOperation
        {
            Op1,
            Op2,
            Op3,
            Op4
        }

        public static void RunEnum()
        {
            var handler = new MultiStepEnumProgress<int, MultiStepOperation>(PrintProgress);

            // Begin step 1 and report progress to the handler
            handler.SetCurrentStep(MultiStepOperation.Op1);
            ReportRandomPercentageProgress(handler);

            // Begin step 2 and do not report any progress
            handler.SetCurrentStep(MultiStepOperation.Op2);

            // Skip step 3

            // Begin step 4 and report progress to the handler
            handler.SetCurrentStep(MultiStepOperation.Op4);
            ReportRandomPercentageProgress(handler);
        }

        private static void ReportRandomPercentageProgress(IProgress<int> handler)
        {
            var r = new Random();
            var i1 = r.Next(1, 50);
            var i2 = r.Next(i1, 99);

            handler.Report(i1);
            handler.Report(i2);
            handler.Report(100);
        }

        private static void PrintProgress(IMultiStepProgressValue value)
        {
            Console.WriteLine(
                $@"[{value.CurrentStep}/{value.TotalSteps}] " +
                $@"{value.CurrentProgress,6:##0.00}% ({value.CurrentStepName}) " +
                $@"{value.TotalProgress,6:##0.00}% (total)");
        }
    }
}