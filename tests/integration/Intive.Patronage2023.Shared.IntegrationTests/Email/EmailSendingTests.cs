using FluentAssertions;
using Intive.Patronage2023.Shared.Infrastructure.Email;
using Microsoft.Extensions.DependencyInjection;

namespace Intive.Patronage2023.Shared.IntegrationTests.Email;
/// <summary>
/// Class that contains email sending service integration tests.
/// </summary>
public class EmailSendingTests : AbstractIntegrationTests, IClassFixture<SmtpServerFixture>
{
	private IEmailService emailService;

	/// <summary>
	/// Initializes email tests.
	/// </summary>
	/// <param name="fixture">Email fixture.</param>
	/// <param name="dbFixture">Email fixture.</param>
	public EmailSendingTests(SmtpServerFixture fixture, MsSqlTests dbFixture) : base(dbFixture)
	{
		var scope = this.WebApplicationFactory.Services.CreateScope();
		this.emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
	}

	/// <summary>
	/// Integration test that verifes if email service does not throw exeptions when called properly.
	/// </summary>
	/// <returns>Test.</returns>
	[Fact]
	public void SendEmail_WhenCalledProperly_ShouldNotThrowExceptions()
	{
		var emailMessage = new EmailMessage
		{
			Subject = "Test subject",
			Body = "Test body",
			SendFromAddress = new EmailAddress("testFrom", "testFrom@intive.pl"),
			SendToAddresses = new List<EmailAddress> { new("testTo", "testTo@invite.pl") }
		};

		var exception = Record.Exception(() => this.emailService.SendEmail(emailMessage));
		exception.Should().BeNull();
	}
}