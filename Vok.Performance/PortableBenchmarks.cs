using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Vok.Performance;

public static class Program {
    public static void Main(string[] args) {
        BenchmarkRunner.Run<PortableBenchmarks>();
    }
}

[MemoryDiagnoser]
[SimpleJob(launchCount: 1, warmupCount: 1, iterationCount: 3)]
public class PortableBenchmarks {
    private readonly Dictionary<string, List<string>> _predictionCache = new();

    [Benchmark]
    [PerfCritcal(1000)]
    public void CachePrediction() {
        _predictionCache["i need"] = ["water", "help", "a break"];
        _predictionCache.TryGetValue("i need", out _);
    }

    [Benchmark]
    [PerfCritcal(1000)]
    public string NormalizePhrase() {
        return "I Need A Break".ToLowerInvariant();
    }
}
