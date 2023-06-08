using System.Text;
using FluentAssertions;

using Intive.Patronage2023.Shared.Infrastructure.Email;

using Microsoft.Extensions.DependencyInjection;

namespace Intive.Patronage2023.Shared.IntegrationTests.Email;

/// <summary>
/// Class that contains email sending service integration tests.
/// </summary>
public class EmailSendingTests : AbstractIntegrationTests, IClassFixture<SmtpServerFixture>
{
	private readonly SmtpServerFixture fixture;
	private IEmailService emailService;

	/// <summary>
	/// Initializes email tests.
	/// </summary>
	/// <param name="fixture">Email fixture.</param>
	/// <param name="dbFixture">Email fixture.</param>
	public EmailSendingTests(SmtpServerFixture fixture, MsSqlTests dbFixture) : base(dbFixture)
	{
		this.fixture = fixture;
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
		string attachmentContent = "test";
		byte[] attachmentContentBytes = Encoding.UTF8.GetBytes(attachmentContent);
		string attachmentContentBase64 = Convert.ToBase64String(attachmentContentBytes);

		var emailMessage = new EmailMessage
		{
			Subject = "Test subject",
			Body = "Test body",
			SendFromAddress = new EmailAddress("testFrom", "testFrom@intive.pl"),
			SendToAddresses = new List<EmailAddress> { new EmailAddress("testTo", "testTo@invite.pl") },
			Attachments = new List<EmailAttachment>
		{
			new EmailAttachment("attachment.csv", attachmentContentBase64)
		}
		};

		this.emailService.Invoking(service => service.SendEmail(emailMessage))
			.Should().NotThrow();
	}

}