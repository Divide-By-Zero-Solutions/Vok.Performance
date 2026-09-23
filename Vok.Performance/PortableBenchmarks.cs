using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Vok.Performance;

/// <summary>Starts the portable BenchmarkDotNet suite.</summary>
public static class Program {
    /// <summary>Runs the portable benchmark collection.</summary>
    public static void Main(string[] args) {
        BenchmarkRunner.Run<PortableBenchmarks>();
    }
}

[/// <summary>Measures portable prediction and text normalization operations.</summary>
[MemoryDiagnoser]
[SimpleJob(launchCount: 1, warmupCount: 1, iterationCount: 3)]
public class PortableBenchmarks {
    private readonly Dictionary<string, List<string>> _predictionCache = new();

    [Benchmark]
    [PerfCritcal(1000)]
    /// <summary>Measures prediction cache insertion and lookup.</summary>
    public void CachePrediction() {
        _predictionCache["i need"] = ["water", "help", "a break"];
        _predictionCache.TryGetValue("i need", out _);
    }

    [Benchmark]
    [PerfCritcal(1000)]
    /// <summary>Measures invariant phrase normalization.</summary>
    public string NormalizePhrase() {
        return "I Need A Break".ToLowerInvariant();
    }
}
