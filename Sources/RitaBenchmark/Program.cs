// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");
using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using RitaBenchmark;

public static class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<  TestInterop >(
            //  ManualConfig
            //         .Create(DefaultConfig.Instance)
            //         .WithOptions(ConfigOptions.DisableOptimizationsValidator)
        );
    }
}

public class Config : ManualConfig
{
    public Config()
    {
        // Using the WithOptions() factory method:
        // this.WithOptions(ConfigOptions.JoinSummary)
        //     .WithOptions(ConfigOptions.DisableLogFile);
        
        // Or (The ConfigOptions Enum is defined as a BitField)
        this.WithOptions(ConfigOptions.DisableOptimizationsValidator);

    }
}