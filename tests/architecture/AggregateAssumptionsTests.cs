using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Conditions;

using Intive.Patronage2023.Shared.Infrastructure.Domain;

using Xunit;

namespace Intive.Patronage2023.Architecture.Tests;

/// <summary>
/// Tests that check assumption taken into account for Aggregates.
/// </summary>
public class AggregateAssumptionsTests
{
	/// <summary>
	/// Test that checks if aggregate is in correct project.
	/// </summary>
	[Fact]
	public void AggregateShouldBeInCorrectAssemblyTest()
	{
		var aggregateBaseClass = Architecture.All.GetClassOfType(typeof(Aggregate));
		IArchRule aggregateResidesInDomainLayer = ArchRuleDefinition.Classes()
			.That()
			.AreAssignableTo(aggregateBaseClass)
			.And()
			.AreNotAbstract()
			.Should()
			.BeInDomainLayer();

		aggregateResidesInDomainLayer.CheckSolution();
	}

	/// <summary>
	/// Test that checks that aggregate can't be modified outside aggregate.
	/// </summary>
	[Fact]
	public void AggregateShouldHaveAllPropertiesPrivateToSetTest()
	{
		var aggregateBaseClass = Architecture.All.GetClassOfType(typeof(Aggregate));
		var aggregateTypes = ArchRuleDefinition.Classes().That().AreAssignableTo(aggregateBaseClass);
		IArchRule aggregateResidesInDomainLayerRule = ArchRuleDefinition.PropertyMembers()
		.That()
		.AreDeclaredIn(aggregateTypes)
		.And()
		.ArePublic()
		.And()
		.HaveSetter()
		.Should()
		.HavePrivateSetter()
		.Because("modification of properties outside aggregate is forbiden");

		aggregateResidesInDomainLayerRule.CheckSolution();
	}

	/// <summary>
	/// Test that checks that aggregate can't be created outside aggregate class.
	/// </summary>
	[Fact]
	public void AggregateCanNotHavePublicConstructorTest()
	{
		var aggregateBaseClass = Architecture.All.GetClassOfType(typeof(Aggregate));
		var aggregateTypes = ArchRuleDefinition.Classes().That().AreAssignableTo(aggregateBaseClass);
		IArchRule publicConstructorForbidenRule = ArchRuleDefinition.MethodMembers()
		.That()
		.AreDeclaredIn(aggregateTypes)
		.And()
		.AreConstructors()
		.Should()
		.NotBePublic();

		publicConstructorForbidenRule.CheckSolution();
	}

	/// <summary>
	/// Test that checks that aggregate have parameterless constructor (Entity Framework case).
	/// </summary>
	[Fact]
	public void AggregateShouldHaveOneParameterlessConsturctor()
	{
		var aggregateBaseClass = Architecture.All.GetClassOfType(typeof(Aggregate));
		var aggregateTypesHasParameterlessConstructorRule = ArchRuleDefinition.Classes().That().AreAssignableTo(aggregateBaseClass)
			.Should()
			.FollowCustomCondition(x => new ConditionResult(x, x.GetConstructors().Where(x => !x.Parameters.Any()).Any()), "needs parameterless constructor")
			.Because("entity framework needs it to create objects during querying data.");

		aggregateTypesHasParameterlessConstructorRule.CheckSolution();
	}
}