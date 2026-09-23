[CmdletBinding()]
param(
	[switch]$SkipBenchmark
)

$ErrorActionPreference = 'Stop'
$repositoryRoot = Split-Path -Parent $PSScriptRoot
$portableProjectPath = Join-Path $repositoryRoot 'Vok.Performance\Vok.Performance.csproj'
$integrationProjectPath = Join-Path $repositoryRoot 'Vok.Performance.Integration\Vok.Performance.Integration.csproj'
$readmePath = Join-Path $repositoryRoot 'README.md'
$artifactPaths = @(
	(Join-Path $repositoryRoot 'BenchmarkDotNet.Artifacts\results'),
	(Join-Path $repositoryRoot 'Vok.Performance\BenchmarkDotNet.Artifacts\results'),
	(Join-Path $repositoryRoot 'Vok.Performance.Integration\BenchmarkDotNet.Artifacts\results')
)
$startMarker = '<!-- PERFORMANCE-REPORT:START -->'
$endMarker = '<!-- PERFORMANCE-REPORT:END -->'

if (-not $SkipBenchmark) {
	foreach ($projectPath in @($portableProjectPath, $integrationProjectPath)) {
		dotnet run --project $projectPath --configuration Release -- --exporters github --filter '*'
		if ($LASTEXITCODE -ne 0) {
			throw "Performance benchmark failed for $projectPath with exit code $LASTEXITCODE."
		}
	}
}

$reportFiles = foreach ($artifactPath in $artifactPaths) {
	Get-ChildItem -Path $artifactPath -Filter '*-report-github.md' -ErrorAction SilentlyContinue |
		Where-Object { $_.Name -like 'Vok.Performance.PortableBenchmarks-*' -or $_.Name -like 'Vok.Performance.Integration.SqliteVocabularyBenchmarks-*' }
}

$reportFiles = @($reportFiles | Sort-Object LastWriteTime)

if ($reportFiles.Count -eq 0) {
	$reportFiles = foreach ($artifactPath in $artifactPaths) {
		Get-ChildItem -Path $artifactPath -Filter '*-report.md' -ErrorAction SilentlyContinue |
			Where-Object { $_.Name -like 'Vok.Performance.PortableBenchmarks-*' -or $_.Name -like 'Vok.Performance.Integration.SqliteVocabularyBenchmarks-*' }
	}
	$reportFiles = @($reportFiles | Sort-Object LastWriteTime)
}

if ($reportFiles.Count -eq 0) {
	throw "No BenchmarkDotNet Markdown report was found in the configured performance artifact directories."
}

$reportContent = ($reportFiles | ForEach-Object {
	$benchmarkName = $_.BaseName -replace '-report-github$', '' -replace '-report$', ''
	"### $benchmarkName"
	''
	(Get-Content -Path $_.FullName -Raw).Trim()
	''
}) -join [Environment]::NewLine
$environment = "OS: $([System.Environment]::OSVersion.VersionString); Runtime: $([System.Runtime.InteropServices.RuntimeInformation]::FrameworkDescription); Generated: $((Get-Date).ToUniversalTime().ToString('yyyy-MM-dd HH:mm:ss')) UTC"
$managedSection = @(
	$startMarker
	'## Performance report'
	''
	'> Generated from the dedicated `Vok.Performance` BenchmarkDotNet project. Results are machine-specific and intended for regression tracking, not cross-machine comparison.'
	''
	$environment
	''
	$reportContent.Trim()
	$endMarker
) -join [Environment]::NewLine

if (Test-Path $readmePath) {
	$readmeContent = Get-Content -Path $readmePath -Raw
} else {
	$readmeContent = "# Vok$([Environment]::NewLine)$([Environment]::NewLine)"
}

$escapedStart = [Regex]::Escape($startMarker)
$escapedEnd = [Regex]::Escape($endMarker)
$replacementPattern = "(?s)$escapedStart.*?$escapedEnd"
if ($readmeContent -match $replacementPattern) {
	$readmeContent = [Regex]::Replace($readmeContent, $replacementPattern, [System.Text.RegularExpressions.MatchEvaluator]{ param($match) $managedSection })
} else {
	$readmeContent = $readmeContent.TrimEnd() + [Environment]::NewLine + [Environment]::NewLine + $managedSection + [Environment]::NewLine
}

Set-Content -Path $readmePath -Value $readmeContent -Encoding utf8
Write-Host "Updated $readmePath from $($reportFiles.Count) benchmark report(s)."
