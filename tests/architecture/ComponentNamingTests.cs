using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Domain;
using Intive.Patronage2023.Shared.Abstractions.Events;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Events;

using Xunit;

namespace Intive.Patronage2023.Architecture.Tests;

/// <summary>
/// Tests that checks if names of classes, methods, properties, etc. are correct.
/// </summary>
public class ComponentNamingTests
{
	/// <summary>
	/// Test that check if all classes are in PascalCase.
	/// </summary>
	[Fact]
	public void AllClassesShouldBePascalCase()
	{
		IArchRule classNameConventionRule = ArchRuleDefinition.Classes().Should().HaveName("^[A-Z][a-zA-Z0-9`]*$", true);

		classNameConventionRule.CheckSolution();
	}

	/// <summary>
	/// Checks if repository has repository postfix.
	/// </summary>
	[Fact]
	public void AllRepositoryImplementationShouldEndsWithRepositoryPostfixTest()
	{
		var baseInterface = Architecture.All.GetInterfaceOfType(typeof(IRepository<,>));

		IArchRule repositoryNameConvetnionRule = ArchRuleDefinition.Classes()
			.That()
			.ImplementInterface(baseInterface)
			.And()
			.AreNotAbstract()
			.Should()
			.HaveNameEndingWith("Repository");

		repositoryNameConvetnionRule.CheckSolution();
	}

	/// <summary>
	/// Checks if repository has handle prefix.
	/// </summary>
	[Fact]
	public void AllCommandHandlerImplementationShouldStartsWithHandlePrefixTest()
	{
		var baseInterface = Architecture.All.GetInterfaceOfType(typeof(ICommandHandler<>));

		IArchRule commandHandlerNameConventionRule = ArchRuleDefinition.Classes()
			.That()
			.ImplementInterface(baseInterface)
			.And()
			.AreNotAbstract()
			.Should()
			.HaveNameStartingWith("Handle")
			.OrShould()
			.HaveNameEndingWith("CommandHandler");

		commandHandlerNameConventionRule.CheckSolution();
	}

	/// <summary>
	/// Checks if repository has handle prefix.
	/// </summary>
	[Fact]
	public void AllQueryHandlerImplementationShouldStartsWithHandlePrefixTest()
	{
		var baseInterface = Architecture.All.GetInterfaceOfType(typeof(IQueryHandler<,>));

		IArchRule queryHandlerNameConventionRule = ArchRuleDefinition.Classes()
			.That()
			.ImplementInterface(baseInterface)
			.And()
			.AreNotAbstract()
			.Should()
			.HaveNameStartingWith("Handle")
			.OrShould()
			.HaveNameEndingWith("QueryHandler");

		queryHandlerNameConventionRule.CheckSolution();
	}

	/// <summary>
	/// Checks if all events implementations has Event postfix.
	/// </summary>
	[Fact]
	public void AllEventImplementationShouldEndsWithEventPostfixTest()
	{
		var baseInterface = Architecture.All.GetInterfaceOfType(typeof(IEvent));

		IArchRule eventNameConvetnionRule = ArchRuleDefinition.Classes().That().ImplementInterface(baseInterface).Should().HaveNameEndingWith("Event");

		eventNameConvetnionRule.CheckSolution();
	}

	/// <summary>
	/// Checks if all domain events implementations has DomainEvent postfix.
	/// </summary>
	[Fact]
	public void AllDomainEventImplementationShouldEndsWithDomainEventPostfixTest()
	{
		var baseClass = Architecture.All.GetClassOfType(typeof(DomainEvent));

		IArchRule domainEventNameConventnionRule = ArchRuleDefinition.Classes().That().AreAssignableTo(baseClass).Should().HaveNameEndingWith("DomainEvent");

		domainEventNameConventnionRule.CheckSolution();
	}

	/// <summary>
	/// Checks if all business rule implementations has BusinessRule postfix.
	/// </summary>
	[Fact]
	public void AllBusinessRuleImplementationShouldEndsWithBusinessRulePostfixTest()
	{
		var baseInterface = Architecture.All.GetInterfaceOfType(typeof(IBusinessRule));

		IArchRule businessRuleNameConventionRule = ArchRuleDefinition.Classes().That().ImplementInterface(baseInterface).Should().HaveNameEndingWith("BusinessRule");

		businessRuleNameConventionRule.CheckSolution();
	}

	/// <summary>
	/// Checks if all business rule implementations hasn't SuperImportant Prefix.
	/// </summary>
	[Fact]
	public void AllBusinessRuleImplementationShouldNotStartAsSuperImportantPrefixTest()
	{
		var baseInterface = Architecture.All.GetInterfaceOfType(typeof(IBusinessRule));

		IArchRule businessRuleNameConventionRule = ArchRuleDefinition.Classes().That().ImplementInterface(baseInterface).Should().NotHaveNameStartingWith("SuperImportant");

		businessRuleNameConventionRule.CheckSolution();
	}

	/// <summary>
	/// Checks if all aggregates implementations has Aggregate postfix.
	/// </summary>
	[Fact]
	public void AllAggregateImplementationShouldEndsWithAggregatePostfixTest()
	{
		var baseClass = Architecture.All.GetClassOfType(typeof(Aggregate));

		IArchRule aggregateNameConventionRule = ArchRuleDefinition.Classes().That().AreAssignableTo(baseClass).Should().HaveNameEndingWith("Aggregate");

		aggregateNameConventionRule.CheckSolution();
	}
}