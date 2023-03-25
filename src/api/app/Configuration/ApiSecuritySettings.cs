namespace Intive.Patronage2023.Api.Configuration
{
	/// <summary>
	/// Api Security Settings.
	/// </summary>
	public class ApiSecuritySettings
	{
		/// <summary>
		/// Array of strings that specifies the allowed origins for Cross-Origin Resource Sharing (CORS).
		/// </summary>
		public string[]? CorsAllowedOrigins { get; set; }
	}
}
