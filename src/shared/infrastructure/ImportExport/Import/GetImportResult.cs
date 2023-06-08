namespace Intive.Patronage2023.Shared.Infrastructure.ImportExport.Import;

/// <summary>
/// GetImportResult generic command.
/// </summary>
/// <typeparam name="T">Aggregate list command type.</typeparam>
/// <param name="AggregateList">List of aggregates to be persisted.</param>
/// <param name="ImportResult">Results of import with all errors occured and optional URI to file with incorrect items.</param>
public record GetImportResult<T>(T AggregateList, ImportResult ImportResult);