namespace Ternary3.Operators.Operations;

using System.Linq.Expressions;

internal class BinaryOperationBuilder<T> where T : struct
{
    private static readonly FifoCache<int, BinaryOperation> operationCache = new(64);

    private BinaryOperation? operation;

    private readonly ParameterExpression negative1 = Expression.Parameter(typeof(T), "negative1");
    private readonly ParameterExpression positive1 = Expression.Parameter(typeof(T), "positive1");
    private readonly ParameterExpression negative2 = Expression.Parameter(typeof(T), "negative2");
    private readonly ParameterExpression positive2 = Expression.Parameter(typeof(T), "positive2");
    private readonly Expression[] expressions1;
    private readonly Expression[] expressions2;
    private readonly ParameterExpression negativeResult = Expression.Parameter(typeof(T).MakeByRefType(), "negativeResult");
    private readonly ParameterExpression positiveResult = Expression.Parameter(typeof(T).MakeByRefType(), "positiveResult");
    private BinaryTritOperator operationTable;

    public BinaryOperationBuilder(BinaryTritOperator operationTable)
    {
        this.operationTable = operationTable;
        expressions1 = [negative1, Expression.Not(Expression.Or(negative1, positive1)), positive1];
        expressions2 = [negative2, Expression.Not(Expression.Or(negative2, positive2)), positive2];
    }

    public delegate void BinaryOperation(T negative1, T positive1, T negative2, T positive2, out T negativeResult, out T positiveResult);

    public BinaryOperation Build()
    {
        if (operation != null) return operation;
        return operation = operationCache.GetOrCreate(operationTable.Value.Positive << 16 | operationTable.Value.Negative, _ => BuildInner());
    }

    private BinaryOperation BuildInner()
    {
        var negativeMatrix = new bool[3, 3];
        var positiveMatrix = new bool[3, 3];
        var firstNegativeCounts = new int[3];
        var secondNegativeCounts = new int[3];
        var firstPositiveCounts = new int[3];
        var secondPositiveCounts = new int[3];

        // Collect pairs into the matrix
        foreach (var first in new[] { Trit.Negative, Trit.Zero, Trit.Positive })
        {
            foreach (var second in new[] { Trit.Negative, Trit.Zero, Trit.Positive })
            {
                switch (operationTable[first, second].Value)
                {
                    case -1:
                        negativeMatrix[first.Value + 1, second.Value + 1] = true;
                        firstNegativeCounts[first.Value + 1]++;
                        secondNegativeCounts[second.Value + 1]++;
                        break;
                    case 1:
                        positiveMatrix[first.Value + 1, second.Value + 1] = true;
                        firstPositiveCounts[first.Value + 1]++;
                        secondPositiveCounts[second.Value + 1]++;
                        break;
                }
            }
        }

        var negativeExpression = CreateExpression(negativeMatrix, firstNegativeCounts, secondNegativeCounts);
        var positiveExpression = CreateExpression(positiveMatrix, firstPositiveCounts, secondPositiveCounts);
        return CompileExpressionsToLambda(positiveExpression, negativeExpression);
    }

    private BinaryOperation CompileExpressionsToLambda(Expression positiveExpression, Expression negativeExpression)
    {
        var negativeAssign = Expression.Assign(negativeResult, Expression.Convert(negativeExpression, typeof(T)));
        var positiveAssign = Expression.Assign(positiveResult, Expression.Convert(positiveExpression, typeof(T)));
        var block = Expression.Block([negativeAssign, positiveAssign]);
        var lambda = Expression.Lambda<BinaryOperation>(
            block, negative1, positive1, negative2, positive2, negativeResult, positiveResult
        );
        return lambda.Compile();
    }

    private Expression CreateExpression(bool[,] matrix, int[] firstCounts, int[] secondCounts)
    {
        var expressions = GetExpressions(matrix, firstCounts, secondCounts);
        return CombineExpressions(expressions);
    }

    internal IEnumerable<Expression> GetExpressions(bool[,] matrix, int[] firstCounts, int[] secondCounts)
    {
        // Get standard patterns
        var patterns = ExpressionPattern.GetStandardPatterns();

        // Special cases for fast empty return or constant return
        if (firstCounts[0] == 0 && firstCounts[1] == 0 && firstCounts[2] == 0)
        {
            yield return Expression.Constant(0);
            yield break;
        }
        
        if (firstCounts[0] == 3 && firstCounts[1] == 3 && firstCounts[2] == 3)
        {
            yield return CreateTrueConstant();
            yield break;
        }

        var handledMatrix = new bool[3, 3];

        // Try to apply each pattern in order of priority
        foreach (var pattern in patterns)
        {
            if (pattern.Matches(matrix, firstCounts, secondCounts))
            {
                yield return pattern.CreateExpression(matrix, expressions1, expressions2);
                pattern.MarkHandled(matrix, handledMatrix);
            }
        }

        // Handle any remaining unhandled cells
        for (var firstIdx = 0; firstIdx < 3; firstIdx++)
        {
            for (var secondIdx = 0; secondIdx < 3; secondIdx++)
            {
                if (!matrix[firstIdx, secondIdx] || handledMatrix[firstIdx, secondIdx]) continue;
                yield return Expression.And(expressions1[firstIdx], expressions2[secondIdx]);
            }
        }
    }

    private static Expression CreateTrueConstant() => typeof(T) switch
    {
        { } t when t == typeof(uint) => Expression.Constant(uint.MaxValue, typeof(T)),
        { } t when t == typeof(ulong) => Expression.Constant(ulong.MaxValue, typeof(T)),
        { } t when t == typeof(ushort) => Expression.Constant(ushort.MaxValue, typeof(T)),
        { } t when t == typeof(byte) => Expression.Constant(byte.MaxValue, typeof(T)),
        // For other types, use a trick to get the equivalent of "all bits set to 1"
        _ => Expression.Convert(Expression.Constant(~(default(T).GetHashCode() & 0)), typeof(T))
    };

    private static Expression CombineExpressions(IEnumerable<Expression> expressions)
    {
        using var enumerator = expressions.GetEnumerator();
        if (!enumerator.MoveNext()) return Expression.Constant(0);
        var result = enumerator.Current;
        while (enumerator.MoveNext())
        {
            result = Expression.Or(result, enumerator.Current);
        }

        return result;
    }
}