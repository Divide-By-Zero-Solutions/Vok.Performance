# Vok Performance

BenchmarkDotNet projects and performance gates for Vok.

Run `./Update-PerformanceReport.ps1` from this directory to generate the report.

## Documentation

This submodule is documented in the parent repository's [performance guide](../docs/performance.md), [testing strategy](../docs/testing.md), and [Mermaid diagrams](../docs/diagrams.md). The benchmark types and performance attributes also emit XML documentation during builds.

## Responsibilities

- `Vok.Performance` contains portable BenchmarkDotNet scenarios that can run without the Windows application host.
- `Vok.Performance.Integration` contains SQLite-backed scenarios and must isolate temporary data.
- `PerformanceAttributes.cs` defines `PerfCritcalAttribute` and `PerfIgnoreAttribute`, each valid on classes and methods.
