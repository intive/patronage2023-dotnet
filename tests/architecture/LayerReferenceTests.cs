using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent;

using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Commands;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Xunit;

namespace Intive.Patronage2023.Architecture.Tests;

/// <summary>
/// Tests that check if all layers are referenced correctly.
/// </summary>
public class LayerReferenceTests
{
	/// <summary>
	/// Tests that checks if there no direct reference to repository from controller.
	/// </summary>
	[Fact]
	public void ControllersShouldNotHaveReferenceToRepositoryTest()
	{
		var controllerBaseClass = Architecture.All.GetClassOfType(typeof(ControllerBase));
		var repositoryInterface = Architecture.All.GetInterfaceOfType(typeof(IRepository<,>));

		IArchRule referenceToRepositoryFromControllerForbidenRule = ArchRuleDefinition.Classes()
			.That()
			.AreAssignableTo(controllerBaseClass)
			.Should()
			.NotDependOnAnyTypesThat()
			.AreAssignableTo(repositoryInterface);

		referenceToRepositoryFromControllerForbidenRule.CheckSolution();
	}

	/// <summary>
	/// Test that checks if repository is in correct project.
	/// </summary>
	[Fact]
	public void RepositoryShouldBeInCorrectAssemblyTest()
	{
		var repositoryInterface = Architecture.All.GetInterfaceOfType(typeof(IRepository<,>));
		IArchRule repositoryImplementationNamespaceRule = ArchRuleDefinition.Classes()
			.That()
			.AreAssignableTo(repositoryInterface)
			.And()
			.AreNotAbstract()
			.Should()
			.BeInInfrastructureLayer();

		IArchRule repositoryInterfacesNamespaceRule = ArchRuleDefinition.Interfaces()
			.That()
			.AreAssignableTo(repositoryInterface)
			.And()
			.AreNot(repositoryInterface)
			.Should()
			.BeInDomainLayer();

		var combinedRule = repositoryImplementationNamespaceRule.And(repositoryInterfacesNamespaceRule);

		combinedRule.CheckSolution();
	}

	/// <summary>
	/// Test that checks if commands is in correct project.
	/// </summary>
	[Fact]
	public void CommandHandlerShouldBeInCorrectAssemblyTest()
	{
		var commandInterface = Architecture.All.GetInterfaceOfType(typeof(ICommandHandler<>));
		IArchRule commandHandlerImplementationNamespaceRule = ArchRuleDefinition.Classes()
			.That()
			.AreAssignableTo(commandInterface)
			.And().DoNotHaveNameContaining("User")
			.Should()
			.BeInApplicationLayer();

		commandHandlerImplementationNamespaceRule.CheckSolution();
	}

	/// <summary>
	/// Test that checks if there are no usage of mediatR interfaces directly in command handlers.
	/// </summary>
	[Fact]
	public void MediatRInterfaceShouldNotBeUsedInCommandHandlersTest()
	{
		IArchRule referenceToMediatRForbidenRule = ArchRuleDefinition.Classes()
			.That()
			.AreNot(typeof(MediatRCommandHandlerAdapter<>))
			.Should()
			.NotImplementInterface(typeof(IRequestHandler<>));

		referenceToMediatRForbidenRule.CheckSolution();
	}

	/// <summary>
	/// Test that checks if there are no usage of mediatR interfaces directly in query handlers.
	/// </summary>
	[Fact]
	public void MediatRInterfaceShouldNotBeUsedInQueryHandlersTest()
	{
		IArchRule referenceToMediatRForbidenRule = ArchRuleDefinition.Classes()
			.That()
			.AreNot(typeof(MediatRQueryHandlerAdapter<,>))
			.Should()
			.NotImplementInterface(typeof(IRequestHandler<,>));

		referenceToMediatRForbidenRule.CheckSolution();
	}

	/// <summary>
	/// Test that checks if there are no usage of mediatR interfaces directly in commands.
	/// </summary>
	[Fact]
	public void MediatRInterfaceShouldNotBeUsedInCommandsTest()
	{
		IArchRule referenceToMediatRForbidenRule = ArchRuleDefinition.Classes()
			.Should()
			.NotImplementInterface(typeof(IRequest));

		referenceToMediatRForbidenRule.CheckSolution();
	}

	/// <summary>
	/// Test that checks if there are no usage of mediatR interfaces directly in query.
	/// </summary>
	[Fact]
	public void MediatRInterfaceShouldNotBeUsedInQueriesTest()
	{
		IArchRule referenceToMediatRForbidenRule = ArchRuleDefinition.Classes()
			.That()
			.ImplementInterface(typeof(IRequest<>))
			.Should()
			.NotDependOnAny(typeof(IRequest<>));

		referenceToMediatRForbidenRule.CheckSolution();
	}
}