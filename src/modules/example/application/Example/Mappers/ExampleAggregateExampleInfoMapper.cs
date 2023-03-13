using Intive.Patronage2023.Modules.Example.Application.Example.GettingExamples;
using Intive.Patronage2023.Modules.Example.Domain;

namespace Intive.Patronage2023.Modules.Example.Application.Example.Mappers
{
	/// <summary>
	/// Mapper class.
	/// </summary>
	public static class ExampleAggregateExampleInfoMapper
	{
		/// <summary>
		/// Mapping method.
		/// </summary>
		/// <param name="entity">Entity to be mapped.</param>
		/// <returns>Returns <ref name="ExampleInfo"/>ExampleInfo.</returns>
		public static ExampleInfo Map(ExampleAggregate entity) =>
			 new(entity.Id, entity.Name, entity.CreatedOn);
	}
}
