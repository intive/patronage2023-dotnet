using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Queries;

using Microsoft.AspNetCore.Mvc;

namespace Intive.Patronage2023.Modules.User.Api.Controllers;

/// <summary>
/// User controller.
/// </summary>
[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
	private readonly IExecutionContextAccessor contextAccessor;
	private readonly ICommandBus commandBus;
	private readonly IQueryBus queryBus;

	/// <summary>
	/// Initializes a new instance of the <see cref="UserController"/> class.
	/// </summary>
	/// <param name="commandBus">Command bus.</param>
	/// <param name="queryBus">Query bus.</param>
	/// <param name="contextAccessor">Execution context accessor.</param>
	public UserController(ICommandBus commandBus, IQueryBus queryBus, IExecutionContextAccessor contextAccessor)
	{
		this.commandBus = commandBus;
		this.queryBus = queryBus;
		this.contextAccessor = contextAccessor;
	}
}