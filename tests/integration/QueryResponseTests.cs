using Intive.Patronage2023.Modules.Example.Domain;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Intive.Patronage2023.Modules.Example.Tests
{
    public interface IQueryHandler<in T, TResult>
    {
        /// <summary>
        /// Handles the query.
        /// </summary>
        /// <param name="query">Query to handle.</param>
        /// <returns>Task that represents asynchronous operation which returns query resposne.</returns>
        Task<TResult> Handle(T query);
    }

    public class ExamplesQuery
    {
        public bool IncludeArchived { get; set; }
    }

    public class Example
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsArchived { get; set; }
    }

    public interface IExampleRepository
    {
        Task<List<Example>> GetExamplesAsync(bool includeArchived = false);
    }

    public class GetExamplesQueryHandler : IQueryHandler<ExamplesQuery, List<Example>>
    {
        private readonly IExampleRepository _exampleRepository;

        public GetExamplesQueryHandler(IExampleRepository exampleRepository)
        {
            _exampleRepository = exampleRepository;
        }

        public async Task<List<Example>> Handle(ExamplesQuery query)
        {
            return await _exampleRepository.GetExamplesAsync(query.IncludeArchived);
        }
    }

    public class GetExamplesTests
    {
        private readonly Mock<IExampleRepository> _exampleRepositoryMock;
        private readonly GetExamplesQueryHandler _handler;

        public GetExamplesTests()
        {
            _exampleRepositoryMock = new Mock<IExampleRepository>();
            _handler = new GetExamplesQueryHandler(_exampleRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidQuery_ReturnsExamples()
        {
            // Arrange
            var query = new ExamplesQuery();
            var examples = new List<Example> { new Example { Id = 1, Name = "Example 1" } };
            _exampleRepositoryMock.Setup(repo => repo.GetExamplesAsync()).ReturnsAsync(examples);

            // Act
            var result = await _handler.Handle(query);

            // Assert
            Assert.Equal(examples, result);
        }

        [Fact]
        public async Task Handle_EmptyQuery_ReturnsAllExamples()
        {
            // Arrange
            var query = new ExamplesQuery { IncludeArchived = true };
            var examples = new List<Example>
        {
            new Example { Id = 1, Name = "Example 1" },
            new Example { Id = 2, Name = "Example 2", IsArchived = true },
            new Example { Id = 3, Name = "Example 3" }
        };
            _exampleRepositoryMock.Setup(repo => repo.GetExamplesAsync(query.IncludeArchived)).ReturnsAsync(examples);

            // Act
            var result = await _handler.Handle(query);

            // Assert
            Assert.Equal(examples, result);
        }
    }

}
