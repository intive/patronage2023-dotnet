using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudget;
using Intive.Patronage2023.Modules.Example.Application.Example.CreatingExample;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Domain;
using Intive.Patronage2023.Shared.Abstractions.Events;
using Intive.Patronage2023.Shared.Infrastructure.Abstractions.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Events;

using Xunit;

namespace Intive.Patronage2023.Architecture.Tests;

/// <summary>
/// Tests that checks if names of classes, methods, properties, etc. are correct.
/// </summary>
public class ComponentNamingTests
{
	private static readonly ArchUnitNET.Domain.Architecture Modules = new ArchLoader().LoadAssemblies(
		typeof(ICommandBus).Assembly,
		typeof(DomainEvent).Assembly,
		typeof(CreateExample).Assembly,
		typeof(CreateBudget).Assembly)
		.Build();

	/// <summary>
	/// Test that check if all classes are in PascalCase.
	/// </summary>
	[Fact]
	public void AllClassesShouldBePascalCase()
	{
		IArchRule classNameConventionRule = ArchRuleDefinition.Classes().Should().HaveName("^[A-Z][a-zA-Z0-9`]*$", true);

		classNameConventionRule.Check(Modules);
	}

	/// <summary>
	/// Checks if repository has repository postfix.
	/// </summary>
	[Fact]
	public void AllRepositoryImplementationShouldEndsWithRepositoryPostfixTest()
	{
		var baseInterface = Modules.GetInterfaceOfType(typeof(IRepository<,>));

		IArchRule repositoryNameConvetnionRule = ArchRuleDefinition.Classes()
			.That()
			.ImplementInterface(baseInterface)
			.And()
			.AreNotAbstract()
			.Should()
			.HaveNameEndingWith("Repository");

		repositoryNameConvetnionRule.Check(Modules);
	}

	/// <summary>
	/// Checks if repository has handle prefix.
	/// </summary>
	[Fact]
	public void AllCommandHandlerImplementationShouldStartsWithHandlePrefixTest()
	{
		var baseInterface = Modules.GetInterfaceOfType(typeof(ICommandHandler<>));

		IArchRule commandHandlerNameConventionRule = ArchRuleDefinition.Classes()
			.That()
			.ImplementInterface(baseInterface)
			.And()
			.AreNotAbstract()
			.Should()
			.HaveNameStartingWith("Handle");

		commandHandlerNameConventionRule.Check(Modules);
	}

	/// <summary>
	/// Checks if all events implementations has Event postfix.
	/// </summary>
	[Fact]
	public void AllEventImplementationShouldEndsWithEventPostfixTest()
	{
		var baseInterface = Modules.GetInterfaceOfType(typeof(IEvent));

		IArchRule eventNameConvetnionRule = ArchRuleDefinition.Classes().That().ImplementInterface(baseInterface).Should().HaveNameEndingWith("Event");

		eventNameConvetnionRule.Check(Modules);
	}

	/// <summary>
	/// Checks if all domain events implementations has DomainEvent postfix.
	/// </summary>
	[Fact]
	public void AllDomainEventImplementationShouldEndsWithDomainEventPostfixTest()
	{
		var baseClass = Modules.GetClassOfType(typeof(DomainEvent));

		IArchRule domainEventNameConvetnionRule = ArchRuleDefinition.Classes().That().AreAssignableTo(baseClass).Should().HaveNameEndingWith("DomainEvent");

		domainEventNameConvetnionRule.Check(Modules);
	}

	/// <summary>
	/// Checks if all bussiness rule implementations has BussinessRule postfix.
	/// </summary>
	[Fact]
	public void AllBussinessRuleImplementationShouldEndsWithBussinessRulePostfixTest()
	{
		var baseInterface = Modules.GetInterfaceOfType(typeof(IBusinessRule));

		IArchRule bussinessRuleNameConventionRule = ArchRuleDefinition.Classes().That().ImplementInterface(baseInterface).Should().HaveNameEndingWith("BusinessRule");

		bussinessRuleNameConventionRule.Check(Modules);
	}

	/// <summary>
	/// Checks if all aggregates implementations has Aggregate postfix.
	/// </summary>
	[Fact]
	public void AllAggregateImplementationShouldEndsWithAggregatePostfixTest()
	{
		var baseClass = Modules.GetClassOfType(typeof(Aggregate));

		IArchRule aggregateNameConventionRule = ArchRuleDefinition.Classes().That().AreAssignableTo(baseClass).Should().HaveNameEndingWith("Aggregate");

		aggregateNameConventionRule.Check(Modules);
	}
}