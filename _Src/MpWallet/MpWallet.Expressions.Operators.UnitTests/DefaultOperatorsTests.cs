using System.Reflection;
using MpWallet.Operators;

namespace MpWallet.Expressions.Operators.UnitTests;

public sealed class DefaultOperatorsTests
{
    [Fact]
    public void Collection_ShouldContainAllPublicOperatorProperties()
    {
        var properties = typeof(DefaultOperators)
            .GetProperties(BindingFlags.Static | BindingFlags.Public)
            .Where(p => p.PropertyType == typeof(Operator));

        foreach (var property in properties)
        {
            var @operator = (Operator)property.GetValue(null)!;

            var result = DefaultOperators.Collection.TryGet(@operator.Value, @operator.Details.Arity, out var actual);
            
            Assert.True(result);
            Assert.Equal(@operator, actual);
        }
    }

    [Fact]
    public void Collection_ShouldContainOnlyPublicProperties()
    {
        var properties = typeof(DefaultOperators)
            .GetProperties(BindingFlags.Static | BindingFlags.Public)
            .Where(p => p.PropertyType == typeof(Operator))
            .Select(p => (Operator)p.GetValue(null)!)
            .ToHashSet();

        foreach (var @operator in DefaultOperators.Collection)
        {
            Assert.Contains(@operator, properties);
        }
    }
}