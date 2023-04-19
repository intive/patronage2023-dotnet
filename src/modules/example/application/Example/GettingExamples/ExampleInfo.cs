namespace Intive.Patronage2023.Modules.Example.Application.Example.GettingExamples;

/// <summary>
/// Example information.
/// </summary>
/// <param name="Id">Example ID.</param>
/// <param name="Name">Name of Example.</param>
/// <param name="CreatedOn">Created Date.</param>
public record ExampleInfo(Guid Id, string Name, DateTime CreatedOn);