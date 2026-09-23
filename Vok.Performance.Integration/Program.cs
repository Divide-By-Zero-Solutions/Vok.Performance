using BenchmarkDotNet.Running;

namespace Vok.Performance.Integration;

/// <summary>Starts the SQLite integration BenchmarkDotNet suite.</summary>
public static class Program {
    /// <summary>Runs the SQLite vocabulary benchmark collection.</summary>
    public static void Main(string[] args) {
        BenchmarkRunner.Run<SqliteVocabularyBenchmarks>();
    }
}
