using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Intive.Patronage2023.Modules.Budget.Api;
using Intive.Patronage2023.Modules.Example.Api;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Infrastructure.Abstractions.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Events;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.TestHost;

using Xunit;

namespace Intive.Patronage2023.Architecture.Tests;

/// <summary>
/// Tests that check if all layers are referenced correctly.
/// </summary>
public class LayerReferenceTests
{
	private static readonly ArchUnitNET.Domain.Architecture Modules = new ArchLoader().LoadAssemblies(
		typeof(Program).Assembly,
		typeof(ExampleModule).Assembly,
		typeof(BudgetModule).Assembly,
		typeof(ICommandBus).Assembly,
		typeof(DomainEvent).Assembly)
		.Build();

	/// <summary>
	/// Tests that checks if there no direct reference to repository from controller.
	/// </summary>
	[Fact]
	public void ControllersShouldNotHaveReferenceToRepositoryTest()
	{
		var controllerBaseClass = Modules.GetClassOfType(typeof(ControllerBase));
		var repositoryInterface = Modules.GetInterfaceOfType(typeof(IRepository<,>));

		IArchRule referenceToRepositoryFromControllerForbidenRule = ArchRuleDefinition.Classes()
			.That()
			.AreAssignableTo(controllerBaseClass)
			.Should()
			.NotDependOnAnyTypesThat()
			.AreAssignableTo(repositoryInterface);

		referenceToRepositoryFromControllerForbidenRule.Check(Modules);
	}

	/// <summary>
	/// Test that checks if repository is in correct project.
	/// </summary>
	[Fact]
	public void RepositoryShouldBeInCorrectProjectTest()
	{
		var repositoryInterface = Modules.GetInterfaceOfType(typeof(IRepository<,>));
		IArchRule repositoryImplementationNamespaceRule = ArchRuleDefinition.Classes()
			.That()
			.AreAssignableTo(repositoryInterface)
			.And()
			.AreNotAbstract()
			.Should()
			.ResideInNamespace("Intive.Patronage2023.Module.*.Infrastructure");

		IArchRule repositoryInterfacesNamespaceRule = ArchRuleDefinition.Interfaces()
			.That()
			.AreAssignableTo(repositoryInterface)
			.And()
			.AreNot(repositoryInterface)
			.Should()
			.ResideInNamespace("Intive.Patronage2023.Modules.*.Domain");

		var combinedRule = repositoryImplementationNamespaceRule.And(repositoryInterfacesNamespaceRule);

		combinedRule.Check(Modules);
	}

	/// <summary>
	/// Test that checks if commands is in correct project.
	/// </summary>
	[Fact]
	public void CommandHandlerShouldBeInCorrectProjectTest()
	{
		var commandInterface = Modules.GetInterfaceOfType(typeof(ICommandHandler<>));
		IArchRule commandHandlerImplementationNamespaceRule = ArchRuleDefinition.Classes()
			.That()
			.AreAssignableTo(commandInterface)
			.Should()
			.ResideInNamespace("Intive.Patronage2023.Modules.*.Application");

		commandHandlerImplementationNamespaceRule.Check(Modules);
	}
}