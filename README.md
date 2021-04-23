# ZySharp Progress

![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)
[![GitHub Actions](https://github.com/flobernd/zysharp-progress/actions/workflows/main.yml/badge.svg)](https://github.com/flobernd/zysharp-progress/actions)
[![NuGet](https://img.shields.io/nuget/v/ZySharp.Progress.svg)](https://nuget.org/packages/ZySharp.Progress)
[![Nuget](https://img.shields.io/nuget/dt/ZySharp.Progress.svg)](https://nuget.org/packages/ZySharp.Progress)

A C# library that provides several extensions to the `IProgress<T>` interface as well as a fluent
API to easily build progress-handler chains.

## Introduction

The `ZySharp Progress` library focuses on improving the developer-experience when working with the standard `IProgress<T>` interface.

Many inbuild classes and extension libraries are using the `IProgress<T>` interface to report progress updates for different operations. Listening to such progress updates requires the developer to either use the `Progress<T>` class or to instantiate a custom class that implements the `IProgress<T>` interface.

Besides some useful standalone progress-handler implementations, like `LambdaProgress<T>`, the library provides several framework classes to assemble progress-handler chains.

## Progress Handler Chains

`ZySharp.Progress` provides a fluent API to create progress-handler chains:

```csharp
var handler = ProgressHandlerChain
    .Create<long>()                     // Accept `long` values [`IProgress<long>`]
    .ToPercent(0, totalDownloadSize)    // Calculate percentage progress from absolute values
    .Distinct()                         // Only report distinct values
    .Throttle(TimeSpan.FromSeconds(1))  // Only report values once every second
    .Build(x => Console.WriteLine(x));  // Call the final handler (lambda or `IProgress<T>` instance)
```

## Multi Step Progress

The `MultiStepProgress` and `MultiStepEnumProgress` handlers can be used to multiplex percentage progress from multiple operations into one final progress value.

The resulting progress value provides information about the current operation and the total progress:

```csharp
public interface MultiStepProgressValue
{
    public int TotalSteps { get; set; }
    public int CurrentStep { get; set; }
    public string CurrentStepName { get; set; }
    public double CurrentProgress { get; set; }
    public double TotalProgress { get; set; }
}
```

The `MultiStepProgress` class handles progress for multi-step operations where each individual step is represented by a simple integer value. The total amount of steps can be increased at any time to provide maximum flexibility for actions where the total amount of steps is unknown at the beginning of the operation.

```csharp
var handler = new MultiStepProgress<int>(PrintProgress, 3);

handler.SetCurrentStep(1, "Op1");   // <- Optional name that is added to the resulting progress value
handler.Report(33);
// ...
handler.NextStep("Op2");
// ...
handler.SetTotalSteps(5);
// ...
handler.SetCurrentStep(4, "Op4");
handler.Report(86);
// ...
handler.NextStep("Op5");
handler.Report(100);
```

The `MultiStepEnumProgress` class handles progress for multi-step operations where each individual step is represented by an enum value.

```csharp
var handler = new MultiStepEnumProgress<int, MultiStepOperation>(PrintProgress);

handler.SetCurrentStep(MultiStepOperation.Op1);
handler.Report(33);
handler.Report(42);
// ...
handler.SetCurrentStep(MultiStepOperation.Op2);
// ...
handler.SetCurrentStep(MultiStepOperation.Op4);
handler.Report(100);
```

## License

ZySharp.Progress is licensed under the MIT license.
