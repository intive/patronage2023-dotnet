namespace Intive.Patronage2023.Shared.Abstractions.UserContext;
/// <summary>
/// User information.
/// </summary>
public abstract record UserInfoBase()
{
	/// <summary>
	/// User Id.
	/// </summary>
	public Guid Id { get; set; }

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
	/// User created timestamp.
	/// </summary>
	public long CreatedTimestamp { get; set; }

	/// <summary>
	/// User created with email or via 3rd party services.
	/// </summary>
	public string CreatedVia { get; set; } = "Email";
}