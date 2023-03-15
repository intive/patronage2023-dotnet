using Bogus;
using FluentAssertions;
using Intive.Patronage2023.Modules.Example.Domain;
using Xunit.Abstractions;

namespace Intive.Patronage2023.Example.Tests
{
	/// <summary>
	/// Tests that check class "ExampleAggregate", which has a Create method
	/// that takes a Guid and a name and returns an instance of the class.
	/// The class also has an UpdateName method that updates the name of the instance.
	/// </summary>
	public class ExampleAggregateTests
	{
		private readonly ITestOutputHelper output;

		/// <summary>
		/// Initializes a new instance of the <see cref="ExampleAggregateTests"/> class.
		/// </summary>
		/// <param name="output">Parameter is of type "ITestOutputHelper".</param>
		public ExampleAggregateTests(ITestOutputHelper output)
		{
			this.output = output;
		}

		/// <summary>
		/// Test that check if the method "Create" returns a valid aggregate when valid arguments are provided.
		/// </summary>
		[Fact]
		public void Create_WhenValidArguments_ShouldReturnValidAggregate()
		{
			// Arrange
			var id = Guid.NewGuid();
			string name = new Faker().Name.FirstName();

			// Act
			var aggregate = ExampleAggregate.Create(id, name);

			// Assert
			this.output.WriteLine($"{aggregate.Id} {aggregate.Name}");

			aggregate.Should().NotBeNull();
			aggregate.Id.Should().Be(id);
			aggregate.Name.Should().Be(name);
		}

		/// <summary>
		/// Test that checks if the method "Create" throws a FormatException when invalid guid is provided.
		/// </summary>
		[Fact]
		public void Create_WhenInvalidGuid_ShouldThrowFormatException()
		{
			// Arrange
			string invalidGuid = "invalid-guid";
			string name = new Faker().Name.FirstName();

			// Act
			Action act = () => ExampleAggregate.Create(Guid.Parse(invalidGuid), name);

			// Assert
			this.output.WriteLine($"{invalidGuid} {name}");
			act.Should().Throw<FormatException>();
		}

		/// <summary>
		/// Test that check if the method "UpdateName" updates the "Name" property of the object
		/// with the specified value when called with a new name.
		/// </summary>
		[Fact]
		public void UpdateName_WhenUpdatedNamePassed_ShouldUpdateName()
		{
			// Arrange
			var id = Guid.NewGuid();
			string name = "Tomas";
			string newName = "Tomasz";
			var aggregate = ExampleAggregate.Create(id, name);

			// Act
			aggregate.UpdateName(newName);

			// Assert
			this.output.WriteLine($"{newName} {aggregate.Name}");
			aggregate.Name.Should().Be(newName);
		}

		/// <summary>
		/// Test that check if the method "UpdateName" updates the name of the ExampleAggregate when provided
		/// with valid arguments and throws an exception when the new name contains digits.
		/// </summary>
		[Fact]
		public void UpdateName_WhenUpdateNameContainsDigits_ShouldThrowTrue()
		{
			// Arrange
			var id = Guid.NewGuid();
			string name = "Bartosz";
			string invalidUpdateName = "Bartosz123";

			// Act
			bool isContainsDigits = invalidUpdateName.Any(char.IsDigit);

			// Assert
			this.output.WriteLine($"{name} {invalidUpdateName}");
			isContainsDigits.Should().BeTrue();
		}

		/// <summary>
		/// Test that check if the method "UpdateName"  updates the name of the aggregate
		/// to the new name, even if the new name is the same as the old name.
		/// </summary>
		[Fact]
		public void UpdateName_WhenNewNameIsSameAsOldName_ShouldUpdateName()
		{
			// Arrange
			var id = Guid.NewGuid();
			string name = "Tomasz";
			string newName = "Tomasz";
			var aggregate = ExampleAggregate.Create(id, name);

			// Act
			aggregate.UpdateName(newName);

			// Assert
			this.output.WriteLine($"{name} {newName}");
			aggregate.Name.Should().Be(name);
		}
	}
}
