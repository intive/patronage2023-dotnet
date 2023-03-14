using Bogus;
using FluentAssertions;
using Intive.Patronage2023.Modules.Example.Domain;
using Xunit.Abstractions;

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
		[Fact]
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
			string invalidGuid = "invalid-guid";
			string name = new Faker().Name.FirstName();

			// Act
			Action act = () => ExampleAggregate.Create(Guid.Parse(invalidGuid), name);

			// Assert
			output.WriteLine($"{invalidGuid} {name}");
			act.Should().Throw<FormatException>();
		}

		[Fact]
		public void UpdateName_WithUpdatedName_ShouldUpdateAggregateName()
		{
			// Arrange
			var id = Guid.NewGuid();
			string name = "Tomas";
			string newName = "Tomasz";
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
			string name = "Bartosz";
			string invalidUpdateName = "Bartosz123";

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
			string name = "Tomasz";
			string newName = "Tomasz";
			var aggregate = ExampleAggregate.Create(id, name);

			// Act
			aggregate.UpdateName(newName);

			// Assert
			output.WriteLine($"{name} {newName}");
			aggregate.Name.Should().Be(name);
		}
	}
}
