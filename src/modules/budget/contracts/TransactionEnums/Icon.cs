namespace Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;

/// <summary>
/// Represents an Icon.
/// </summary>
/// <param name="IconName">Represents the name of the icon.</param>
/// <param name="Foreground">Represents the color of the icon's foreground.</param>
/// <param name="Background">Represents the color of the icon's background.</param>
public record Icon(string IconName, string Foreground, string Background);