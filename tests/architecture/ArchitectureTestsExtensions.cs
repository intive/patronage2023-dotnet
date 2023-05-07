using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Syntax;
using ArchUnitNET.Fluent.Syntax.Elements;
using ArchUnitNET.xUnit;

namespace Intive.Patronage2023.Architecture.Tests;

/// <summary>
/// Extensions for Architecture tests.
/// </summary>
public static class ArchitectureTestsExtensions
{
	/// <summary>
	/// Adds to rule condition that checks if object is in Application layer.
	/// </summary>
	/// <typeparam name="TRuleTypeShouldConjunction">Type of should conjunction.</typeparam>
	/// <typeparam name="TRuleType">Rule type constructed.</typeparam>
	/// <param name="objectsShould">Object that is constructing rule.</param>
	/// <returns>Constructed rule with layer checking.</returns>
	public static TRuleTypeShouldConjunction BeInApplicationLayer<TRuleTypeShouldConjunction, TRuleType>(this ObjectsShould<TRuleTypeShouldConjunction, TRuleType> objectsShould)
		where TRuleType : ICanBeAnalyzed
		where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
	{
		return objectsShould.Be(Architecture.Layers.Application);
	}

	/// <summary>
	/// Adds to rule condition that checks if object is in domain layer.
	/// </summary>
	/// <typeparam name="TRuleTypeShouldConjunction">Type of should conjunction.</typeparam>
	/// <typeparam name="TRuleType">Rule type constructed.</typeparam>
	/// <param name="objectsShould">Object that is constructing rule.</param>
	/// <returns>Constructed rule with layer checking.</returns>
	public static TRuleTypeShouldConjunction BeInDomainLayer<TRuleTypeShouldConjunction, TRuleType>(this ObjectsShould<TRuleTypeShouldConjunction, TRuleType> objectsShould)
		where TRuleType : ICanBeAnalyzed
		where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
	{
		return objectsShould.Be(Architecture.Layers.Domain);
	}

	/// <summary>
	/// Adds to rule condition that checks if object is in infrastructure layer.
	/// </summary>
	/// <typeparam name="TRuleTypeShouldConjunction">Type of should conjunction.</typeparam>
	/// <typeparam name="TRuleType">Rule type constructed.</typeparam>
	/// <param name="objectsShould">Object that is constructing rule.</param>
	/// <returns>Constructed rule with layer checking.</returns>
	public static TRuleTypeShouldConjunction BeInInfrastructureLayer<TRuleTypeShouldConjunction, TRuleType>(this ObjectsShould<TRuleTypeShouldConjunction, TRuleType> objectsShould)
		where TRuleType : ICanBeAnalyzed
		where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
	{
		return objectsShould.Be(Architecture.Layers.Infrastructure);
	}

	/// <summary>
	/// Adds to rule condition that checks if object is in api layer.
	/// </summary>
	/// <typeparam name="TRuleTypeShouldConjunction">Type of should conjunction.</typeparam>
	/// <typeparam name="TRuleType">Rule type constructed.</typeparam>
	/// <param name="objectsShould">Object that is constructing rule.</param>
	/// <returns>Constructed rule with layer checking.</returns>
	public static TRuleTypeShouldConjunction BeInApiLayer<TRuleTypeShouldConjunction, TRuleType>(this ObjectsShould<TRuleTypeShouldConjunction, TRuleType> objectsShould)
		where TRuleType : ICanBeAnalyzed
		where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
	{
		return objectsShould.Be(Architecture.Layers.Api);
	}

	/// <summary>
	/// Adds to rule condition that checks if object is in api layer.
	/// </summary>
	/// <param name="rule">Rule to check on architecture..</param>
	public static void CheckSolution(this IArchRule rule)
	{
		rule.Check(Architecture.All);
	}
}