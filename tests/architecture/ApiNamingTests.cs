using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;

using Intive.Patronage2023.Modules.Example.Api;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.TestHost;

using Xunit;

namespace Intive.Patronage2023.Architecture.Tests
{
	/// <summary>
	/// Class containing tests that check if all api components are named correctly.
	/// </summary>
	public class ApiNamingTests
	{
		private static readonly ArchUnitNET.Domain.Architecture Modules = new ArchLoader().LoadAssemblies(
		typeof(Program).Assembly,
		typeof(ExampleModule).Assembly)
		.Build();

		/// <summary>
		/// Test that checks if all controllers are named correctly.
		/// </summary>
		[Fact]
		public void ControllersNamesShouldEndsWithControllerPostfixTest()
		{
			var baseClass = Modules.GetClassOfType(typeof(ControllerBase));

			IArchRule controllerNamePostfixRule = ArchRuleDefinition.Classes().That().AreAssignableTo(baseClass).Should().HaveNameEndingWith("Controller");

			controllerNamePostfixRule.Check(Modules);
		}
	}
}