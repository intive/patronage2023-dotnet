namespace Intive.Patronage2023.Shared.Abstractions.Errors;

/// <summary>
/// Error Response Example.
/// </summary>
/// <param name="Type">Type.</param>
/// <param name="Title">Title of an Error.</param>
/// <param name="Status">Status Code.</param>
/// <param name="TraceId">Trace Id.</param>
/// <param name="Errors">List of Errors.</param>
public record ErrorExample(string Type, string Title, int Status, string TraceId, List<Error> Errors);