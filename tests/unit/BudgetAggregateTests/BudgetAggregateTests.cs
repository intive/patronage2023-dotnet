using System;
using Intive.Patronage2023.Modules.Budget.Domain;

namespace BudgetAggregateTests;

public class BudgetAggregateTests
{
    [Fact]
    public void Create_WhenPassedProperData_ShouldCreateBudgetAggregate()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        string name = "Test Budget";
        Guid userId = Guid.NewGuid();
        BudgetLimit limit = new BudgetLimit(1000, Currency.PLN);
        BudgetPeriod period = new BudgetPeriod(new DateOnly(2023, 04, 13), new DateOnly(2023, 05, 13));
        string icon = "some icon";
        string describtion = "lorem ipsum.";

        // Act
        var budgetAggregate = BudgetAggregate.Create(id, name, userId, limit, period, icon, describtion);

        // Assert
        Assert.NotNull(budgetAggregate);
        Assert.Equal(id, budgetAggregate.Id);
        Assert.Equal(name, budgetAggregate.Name);
        Assert.Equal(userId, budgetAggregate.UserId);
        Assert.Equal(limit, budgetAggregate.Limit);
        Assert.Equal(period, budgetAggregate.Period);
    }

    /// <summary>
    /// Budget aggregate create method test with empty guid Id.
    /// </summary>
    [Fact]
    public void Create_WhenPassedEmptyId_ShouldThrowInvalidOperatorExeption()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        string name = "Test Budget";
        Guid userId = Guid.NewGuid();
        BudgetLimit limit = new BudgetLimit(1000, Currency.PLN);
        BudgetPeriod period = new BudgetPeriod(new DateOnly(2023, 04, 13), new DateOnly(2023, 05, 13));
        string icon = "some icon";
        string describtion = "lorem ipsum.";

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
             BudgetAggregate.Create(id, name, userId, limit, period, icon, describtion));
    }
}
