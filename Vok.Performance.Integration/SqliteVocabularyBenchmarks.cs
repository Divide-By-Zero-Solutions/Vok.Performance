using Vok.Domain.Models;
using Vok.Infrastructure.Services;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Performance.Integration;

/// <summary>Starts the SQLite integration BenchmarkDotNet suite.</summary>
public static class Program {
    /// <summary>Runs the SQLite vocabulary benchmark collection.</summary>
    public static void Main(string[] args) {
        BenchmarkRunner.Run<SqliteVocabularyBenchmarks>();
    }
}

/// <summary>Measures SQLite vocabulary setup, writes, and reads in isolation.</summary>
[MemoryDiagnoser]
[InProcess]
public class SqliteVocabularyBenchmarks {
    private readonly string _databasePath = Path.Combine(Path.GetTempPath(), $"aac-performance-{Guid.NewGuid():N}.db3");
    private SqliteVocabularyService _vocabularyService = null!;

    /// <summary>Creates an isolated temporary database for the benchmark run.</summary>
    [GlobalSetup]
    public void Setup() {
        _vocabularyService = new SqliteVocabularyService(_databasePath);
    }

    /// <summary>Deletes the temporary database after the benchmark run.</summary>
    [GlobalCleanup]
    public void Cleanup() {
        if (File.Exists(_databasePath)) {
            File.Delete(_databasePath);
        }
    }

    /// <summary>Measures adding a vocabulary tile to SQLite.</summary>
    [Benchmark]
    public async Task AddVocabularyTile() {
        await _vocabularyService.AddTileAsync("performance", new AacTile {
            Id = Guid.NewGuid().ToString("N"),
            Label = "Benchmark tile",
            Icon = "⚡"
        });
    }

    /// <summary>Measures reading tiles from a SQLite-backed category.</summary>
    [Benchmark]
    public List<AacTile> ReadVocabularyTiles() {
        return _vocabularyService.GetTiles("performance");
    }
}
