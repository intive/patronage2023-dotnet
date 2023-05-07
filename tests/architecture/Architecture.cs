using ArchUnitNET.Domain;
using ArchUnitNET.Loader;

using Intive.Patronage2023.Api.Configuration;

using Assembly = System.Reflection.Assembly;

namespace Intive.Patronage2023.Architecture.Tests;

/// <summary>
/// Static class for providing modules.
/// </summary>
public static class Architecture
{
	/// <summary>
	/// Loads and stores all modules.
	/// </summary>
	public static readonly ArchUnitNET.Domain.Architecture All = new ArchLoader().LoadAssembliesRecursively(
		new Assembly[]
		{
			typeof(TelemetryExtensions).Assembly,
		},
		x => x.FullName.Contains("Intive") ? FilterResult.LoadAndContinue : FilterResult.DontLoadAndStop)
		.Build();

	/// <summary>
	/// static property that provides layer definition.
	/// </summary>
	public static readonly LayersDefinition Layers = new LayersDefinition();

	/// <summary>
	/// Class that stores layer definitions.
	/// </summary>
	/// <remarks>
	/// Class created to wrap ArchUnitNET.Fluent.ArchRuleDefinition because it's impossible to create IObjectProvider definitions as static readonly objects.
	/// </remarks>
	public class LayersDefinition
	{
		/// <summary>
		/// Const that stores regex that match module application namespace.
		/// </summary>
		private const string ApplicationRegex = "Intive\\.Patronage2023\\.Modules\\.([A-Z]{1})([a-z]*)\\.Application(.*)";

		/// <summary>
		/// Const that stores regex that match module domain namespace.
		/// </summary>
		private const string DomainRegex = "Intive\\.Patronage2023\\.Modules\\.([A-Z]{1})([a-z]*)\\.Domain(.*)";

		/// <summary>
		/// Const that stores regex that match module infrastructure namespace.
		/// </summary>
		private const string InfrastructureRegex = "Intive\\.Patronage2023\\.Modules\\.([A-Z]{1})([a-z]*)\\.Infrastructure(.*)";

		/// <summary>
		/// Const that stores regex that match module contracts namespace.
		/// </summary>
		private const string ContractsRegex = "Intive\\.Patronage2023\\.Modules\\.([A-Z]{1})([a-z]*)\\.Contracts(.*)";

		/// <summary>
		/// Const that stores regex that match module Api namespace.
		/// </summary>
		private const string ApiRegex = "Intive\\.Patronage2023\\.Modules\\.([A-Z]{1})([a-z]*)\\.Api(.*)";

		/// <summary>
		/// Application layer definition.
		/// </summary>
		public IObjectProvider<IType> Application { get; } = ArchUnitNET.Fluent.ArchRuleDefinition.Types().That().ResideInNamespace(ApplicationRegex, true).As("Application");

		/// <summary>
		/// Application layer definition.
		/// </summary>
		public IObjectProvider<IType> Domain { get; } = ArchUnitNET.Fluent.ArchRuleDefinition.Types().That().ResideInNamespace(DomainRegex, true).As("Domain");

		/// <summary>
		/// Application layer definition.
		/// </summary>
		public IObjectProvider<IType> Infrastructure { get; } = ArchUnitNET.Fluent.ArchRuleDefinition.Types().That().ResideInNamespace(InfrastructureRegex, true).As("Infrastructure");

		/// <summary>
		/// Application layer definition.
		/// </summary>
		public IObjectProvider<IType> Contracts { get; } = ArchUnitNET.Fluent.ArchRuleDefinition.Types().That().ResideInNamespace(ContractsRegex, true).As("Contracts");

		/// <summary>
		/// Application layer definition.
		/// </summary>
		public IObjectProvider<IType> Api { get; } = ArchUnitNET.Fluent.ArchRuleDefinition.Types().That().ResideInNamespace(ApiRegex, true).As("Api");
	}
}