using Vok.Domain.Models;
using Vok.Infrastructure.Services;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Vok.Performance.Integration;

public static class Program {
    public static void Main(string[] args) {
        BenchmarkRunner.Run<SqliteVocabularyBenchmarks>();
    }
}

[MemoryDiagnoser]
[InProcess]
public class SqliteVocabularyBenchmarks {
    private readonly string _databasePath = Path.Combine(Path.GetTempPath(), $"aac-performance-{Guid.NewGuid():N}.db3");
    private SqliteVocabularyService _vocabularyService = null!;

    [GlobalSetup]
    public void Setup() {
        _vocabularyService = new SqliteVocabularyService(_databasePath);
    }

    [GlobalCleanup]
    public void Cleanup() {
        if (File.Exists(_databasePath)) {
            File.Delete(_databasePath);
        }
    }

    [Benchmark]
    public async Task AddVocabularyTile() {
        await _vocabularyService.AddTileAsync("performance", new AacTile {
            Id = Guid.NewGuid().ToString("N"),
            Label = "Benchmark tile",
            Icon = "⚡"
        });
    }

    [Benchmark]
    public List<AacTile> ReadVocabularyTiles() {
        return _vocabularyService.GetTiles("performance");
    }
}
