using BenchmarkDotNet.Running;

namespace Vok.Performance.Benchmarks;

/// <summary>Starts the portable BenchmarkDotNet suite.</summary>
public static class Program {
    /// <summary>Runs the portable benchmark collection.</summary>
    public static void Main(string[] args) {
        BenchmarkRunner.Run<PortableBenchmarks>();
    }
}
