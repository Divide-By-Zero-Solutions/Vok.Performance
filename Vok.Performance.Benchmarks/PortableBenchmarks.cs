using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Vok.Performance;

namespace Vok.Performance.Benchmarks;

/// <summary>Measures portable prediction and text normalization operations.</summary>
[MemoryDiagnoser]
[SimpleJob(launchCount: 1, warmupCount: 1, iterationCount: 3)]
public class PortableBenchmarks {
    private readonly Dictionary<string, List<string>> _predictionCache = new();

    /// <summary>Measures prediction cache insertion and lookup.</summary>
    [Benchmark]
    [PerfCritical(1000)]
    public void CachePrediction() {
        _predictionCache["i need"] = ["water", "help", "a break"];
        _predictionCache.TryGetValue("i need", out _);
    }

    /// <summary>Measures invariant phrase normalization.</summary>
    [Benchmark]
    [PerfCritical(1000)]
    public string NormalizePhrase() {
        return "I Need A Break".ToLowerInvariant();
    }
}
