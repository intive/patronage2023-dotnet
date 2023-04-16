using Intive.Patronage2023.Modules.Example.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Intive.Patronage2023.Modules.Example.Tests
{
	public class GetExamplesTests
	{
		private readonly ExampleRepository exampleRepository;
		private readonly GetExamplesQueryHandler handler;

		public GetExamplesTests()
		{
			var exampleDbContext = new ExampleDbContext();
			exampleRepository = new ExampleRepository(exampleDbContext);
			handler = new GetExamplesQueryHandler(exampleRepository);
		}

		[Fact]
		public async Task Handle_ValidQuery_ReturnsExamples()
		{
			// Arrange
			var query = new ExamplesQuery();

			// Act
			var result = await handler.Handle(query);

			// Assert
			Assert.NotNull(result);
		}

		[Fact]
		public async Task Handle_EmptyQuery_ReturnsAllExamples()
		{
			// Arrange
			var query = new ExamplesQuery { IncludeArchived = true };

			// Act
			var result = await handler.Handle(query);

			// Assert
			Assert.NotNull(result);
		}
	}
}