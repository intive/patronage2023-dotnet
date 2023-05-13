using Intive.Patronage2023.Modules.User.Contracts;

namespace Intive.Patronage2023.Modules.User.Domain;

/// <summary>
/// Information required to create new user.
/// </summary>
public class AppUser
{
	/// <summary>
	/// User email.
	/// </summary>
	public string Email { get; set; } = null!;

	/// <summary>
	/// User first name.
	/// </summary>
	public string FirstName { get; set; } = null!;

	/// <summary>
	/// User last name.
	/// </summary>
	public string LastName { get; set; } = null!;

	/// <summary>
	/// Is user enabled.
	/// </summary>
	public bool Enabled { get; set; }

	/// <summary>
	/// User additional information.
	/// </summary>
	public UserAttributes Attributes { get; set; } = null!;

	/// <summary>
	/// User credentials settings.
	/// </summary>
	public UserCredentials[] Credentials { get; set; } = null!;
}