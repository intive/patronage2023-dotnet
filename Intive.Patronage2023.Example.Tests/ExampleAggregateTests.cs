using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Intive.Patronage2023.Modules.Example.Domain;
using Bogus;
using System.Collections;
using Xunit.Abstractions;
using Bogus.DataSets;
using Intive.Patronage2023.Modules.Example.Domain.Rules;
using Intive.Patronage2023.Shared.Infrastructure.Domain;

namespace Intive.Patronage2023.Example.Tests
{
	public class ExampleAggregateTests
	{
		private readonly ITestOutputHelper output;
		public ExampleAggregateTests(ITestOutputHelper output)
		{
			this.output = output;
		}

		// Naming Convention: TestedMethodName_StateUnderTest_ExpectedBehavior
		#region Create
			[Theory]
			[MemberData(nameof(GeneratedDate))]
			public void Create_ArrayWithValues_CreateGivenValues(Guid id, string name)
			{
				// Arrange

				// Act
				var aggregate = ExampleAggregate.Create(id, name);

				// Assert
				output.WriteLine($"{aggregate.Id} {aggregate.Name}");

				aggregate.Should().NotBeNull();
				aggregate.Id.Should().Be(id);
				aggregate.Name.Should().Be(name);
			}

			[Fact]
			public void Create_WithInvalidGuid_ShouldThrowFormatException()
			{
				// Arrange
				var invalidGuid = "invalid-guid";
				var name = new Faker().Name.FirstName();

				// Act 
				Action act = () => ExampleAggregate.Create(Guid.Parse(invalidGuid), name);

				// Assert
				output.WriteLine($"{invalidGuid} {name}");
				act.Should().Throw<FormatException>();
			}
		#endregion

		#region UpdateName
			[Fact]
			public void UpdateName_WithUpdatedName_ShouldUpdateAggregateName()
			{
				// Arrange
				var id = Guid.NewGuid();
				var name = "Tomas";
				var newName = "Tomasz";
				var aggregate = ExampleAggregate.Create(id, name);

				// Act
				aggregate.UpdateName(newName);

				// Assert
				output.WriteLine($"{newName} {aggregate.Name}");
				aggregate.Name.Should().Be(newName);
			}

			[Fact]
			public void UpdateName_WithNumberInName_ShouldThrowTrue()
			{
				// Arrange
				var id = Guid.NewGuid();
				var name = "Bartosz";
				var invalidUpdateName = "Bartosz123";

				// Act
				bool isContainsDigits = invalidUpdateName.Any(char.IsDigit);

				// Assert
				output.WriteLine($"{name} {invalidUpdateName}");
				isContainsDigits.Should().BeTrue();
			}

			[Fact]
			public void UpdateName_WithUpdatedTheSameName_ShouldBeTheSameName()
			{
				// Arrange
				var id = Guid.NewGuid();
				var name = "Tomasz";
				var newName = "Tomasz";
				var aggregate = ExampleAggregate.Create(id, name);

				// Act
				aggregate.UpdateName(newName);

				// Assert
				output.WriteLine($"{name} {newName}");
				aggregate.Name.Should().Be(name);
			}

		#endregion

		public static IEnumerable<object[]> GeneratedDate()
		{
			for (int i = 0; i < 10; i++)
			{
				var id = Guid.NewGuid();
				string name = new Faker().Name.FirstName();
				yield return new object[] { id, name };
			}
		}
	}
}
