using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent;

using Microsoft.AspNetCore.Mvc;

using Xunit;

namespace Intive.Patronage2023.Architecture.Tests;

/// <summary>
/// Class containing tests that check if all api components are named correctly.
/// </summary>
public class ApiNamingTests
{
	/// <summary>
	/// Test that checks if all controllers are named correctly.
	/// </summary>
	[Fact]
	public void ControllersNamesShouldEndsWithControllerPostfixTest()
	{
		var baseClass = Architecture.All.GetClassOfType(typeof(ControllerBase));

		IArchRule controllerNamePostfixRule = ArchRuleDefinition.Classes().That().AreAssignableTo(baseClass).Should().HaveNameEndingWith("Controller");

		controllerNamePostfixRule.CheckSolution();
	}
}