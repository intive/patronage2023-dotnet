using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetDetails;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetTransactions;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.UserContext;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;

/// <summary>
/// Mapper class.
/// </summary>
public static class BudgetTransactionInfoMapper
{
	/// <summary>
	/// Mapping method.
	/// </summary>
	/// <param name="source">Collection of transactions.</param>
	/// <param name="users">Collection of users in system.</param>
	/// <returns>Returns <ref name="TransactionInfo"/>BudgetInfo.</returns>
	public static IEnumerable<BudgetTransactionInfo> MapToTransactionInfo(this IEnumerable<BudgetTransactionInfoDto> source, Dictionary<string, UserInfoDto> users) =>
		source.Select(x => new BudgetTransactionInfo
		{
			TransactionType = x.TransactionType,
			BudgetId = x.BudgetId,
			TransactionId = x.TransactionId,
			Name = x.Name,
			Value = x.Value,
			BudgetTransactionDate = x.BudgetTransactionDate,
			CategoryType = x.CategoryType,
			BudgetUser = users.ContainsKey(x.Email) ? users[x.Email].ToBudgetUser() : null,
		});

	/// <summary>
	/// Mapping method.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <returns>Returns <ref name="TransactionInfo"/>BudgetInfo.</returns>
	public static IQueryable<BudgetTransactionInfoDto> MapToTransactionInfoDto(this IQueryable<BudgetTransactionAggregate> query) =>
		query.Select(x => new BudgetTransactionInfoDto
		{
			TransactionType = x.TransactionType,
			BudgetId = x.BudgetId,
			TransactionId = x.Id,
			Name = x.Name,
			Value = x.Value,
			BudgetTransactionDate = x.BudgetTransactionDate,
			CategoryType = x.CategoryType,
			AttachmentUrl = x.AttachmentUrl,
			Email = x.Email,
		});

	private static BudgetUser ToBudgetUser(this UserInfoDto item)
	{
		ArgumentNullException.ThrowIfNull(item, nameof(item));
		var result = new BudgetUser(item.Id, item.Avatar ?? string.Empty, item.FirstName, item.LastName, item.Email);
		return result;
	}
}