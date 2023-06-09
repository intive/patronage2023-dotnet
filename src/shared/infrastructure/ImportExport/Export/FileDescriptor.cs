namespace Intive.Patronage2023.Shared.Infrastructure.ImportExport.Export;

/// <summary>
/// Wrapper of file content.
/// </summary>
/// <param name="Name">Name of the file.</param>
/// <param name="Content">COntent of the file.</param>
public record FileDescriptor(string Name, byte[] Content);