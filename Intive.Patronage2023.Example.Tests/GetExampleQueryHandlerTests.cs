using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus.DataSets;
using FluentAssertions;
using Intive.Patronage2023.Modules.Example.Application.Example.GettingExamples;
using Intive.Patronage2023.Modules.Example.Domain;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Moq;
using Xunit.Abstractions;

namespace Intive.Patronage2023.Example.Tests
{
	public class GetExampleQueryHandlerTests
	{
		[Fact]
		public async Task Handle_WhenCalled_ReturnsPagedListExampleInfo()
		{
			// Arrange
			var query = new GetExamples();
			var handler = new HandleGetExamples();

			// Assert & Act			
			await Assert.ThrowsAsync<NotImplementedException>(() => handler.Handle(query));
		}
	}
}
