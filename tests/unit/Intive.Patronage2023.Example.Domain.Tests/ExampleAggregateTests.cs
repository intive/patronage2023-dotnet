using Bogus;

using FluentAssertions;

using Intive.Patronage2023.Modules.Example.Domain;

using Xunit;
using Xunit.Abstractions;

namespace Intive.Patronage2023.Example.Domain.Tests;

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
		aggregate.Id.Value.Should().Be(id);
		aggregate.Name.Should().Be(name);
	}

	/// <summary>
	/// Test that checks if the method "Create" throws a InvalidOperationException when empty guid is provided.
	/// </summary>
	[Fact]
	public void Create_WhenEmptydGuid_ShouldThrowInvalidOperationException()
	{
		// Arrange
		var emptydGuid = Guid.Empty;
		string name = new Faker().Name.FirstName();

		// Act
		Action act = () => ExampleAggregate.Create(emptydGuid, name);

		// Assert
		act.Should().Throw<InvalidOperationException>();
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
		string name = new Faker().Name.FirstName();
		string newName = new Faker().Name.FirstName();
		var instance = ExampleAggregate.Create(id, name);

		// Act
		instance.UpdateName(newName);

		// Assert
		this.output.WriteLine($"{newName} {instance.Name}");
		instance.Name.Should().Be(newName);
	}

	/// <summary>
	/// Test that check if the method "UpdateName" updates the name of the ExampleAggregate when provided
	/// with name containing digits.
	/// </summary>
	[Fact]
	public void UpdateName_WhenNameWithoutDigits_NewNameShouldContainsDigits()
	{
		// Arrange
		var id = Guid.NewGuid();
		string name = new Faker().Name.FirstName();
		string nameWithDigits = name + "123";
		var instance = ExampleAggregate.Create(id, name);

		// Act
		instance.UpdateName(nameWithDigits);

		// Assert
		instance.Name.Should().MatchRegex(@"\d+");
	}

	/// <summary>
	/// Test that check if the method "UpdateName" updates the name of the aggregate
	/// to the new name, even if the new name is the same as the old name.
	/// </summary>
	[Fact]
	public void UpdateName_WhenNewNameIsSameAsOldName_ShouldUpdateName()
	{
		// Arrange
		var id = Guid.NewGuid();
		string name = new Faker().Name.FirstName();
		string newName = name;
		var instance = ExampleAggregate.Create(id, name);

		// Act
		instance.UpdateName(newName);

		// Assert
		this.output.WriteLine($"{name} {newName}");
		instance.Name.Should().Be(name);
	}
}