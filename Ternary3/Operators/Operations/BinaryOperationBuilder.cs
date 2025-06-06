namespace Ternary3.Operators.Operations;

using Numbers;
using System;
using System.Linq.Expressions;

internal class BinaryOperationBuilder<T> where T : struct
{
    private static readonly FifoCache<int, BinaryOperation> operationCache = new(64);

    private BinaryOperation? operation;

    private readonly ParameterExpression negative1 = Expression.Parameter(typeof(T), "negative1");
    private readonly ParameterExpression positive1 = Expression.Parameter(typeof(T), "positive1");
    private readonly ParameterExpression negative2 = Expression.Parameter(typeof(T), "negative2");
    private readonly ParameterExpression positive2 = Expression.Parameter(typeof(T), "positive2");
    private readonly Expression zero1;
    private readonly Expression zero2;
    private readonly ParameterExpression negativeResult = Expression.Parameter(typeof(T).MakeByRefType(), "negativeResult");
    private readonly ParameterExpression positiveResult = Expression.Parameter(typeof(T).MakeByRefType(), "positiveResult");
    private TritLookupTable operationTable1;

    public BinaryOperationBuilder(TritLookupTable operationTable)
    {
        operationTable1 = operationTable;
        zero1 = Expression.Not(Expression.Or(negative1, positive1));
        zero2 = Expression.Not(Expression.Or(negative2, positive2));
    }

    public delegate void BinaryOperation(T negative1, T positive1, T negative2, T positive2, out T negativeResult, out T positiveResult);

    public BinaryOperation Build()
    {
        if (operation != null) return operation;
        return operation = operationCache.GetOrCreate(operationTable1.Value, _ => BuildInner());
    }

    private BinaryOperation BuildInner()
    {
        List<(sbyte, sbyte)> negativePairs = new();
        List<(sbyte, sbyte)> positivePairs = new();

        foreach (var positive in new[] { Trit.Negative, Trit.Zero, Trit.Positive })
        {
            foreach (var negative in new[] { Trit.Negative, Trit.Zero, Trit.Positive })
            {
                switch (operationTable1[negative, positive].Value)
                {
                    case -1:
                        negativePairs.Add((negative.Value, positive.Value));
                        break;
                    case 1:
                        positivePairs.Add((negative.Value, positive.Value));
                        break;
                }
            }
        }


        var negativeExpression = CreateExpression(negativePairs);
        var positiveExpression = CreateExpression(positivePairs);
        return CompileExpressionsToLambda(positiveExpression, negativeExpression);
    }

    private Expression CreateExpression(List<(sbyte, sbyte)> pairs)
    {
        var enumerator = pairs.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            return Expression.Constant(0);
        }

        var expression = GetExpression(enumerator.Current);
        while (enumerator.MoveNext())
        {
            var (p1, p2) = enumerator.Current;
            expression = Expression.Or(expression, GetExpression((p1, p2)));
        }

        return expression;
    }

    private Expression GetExpression((sbyte negtive, sbyte positive) pair) =>
        pair switch
        {
            (-1, -1) => Expression.And(negative1, negative2),
            (-1, 0) => Expression.And(negative1, zero2),
            (-1, 1) => Expression.And(negative1, positive2),
            (0, -1) => Expression.And(zero1, negative2),
            (0, 0) => Expression.And(zero1, zero2),
            (0, 1) => Expression.And(zero1, positive2),
            (1, -1) => Expression.And(positive1, negative2),
            (1, 0) => Expression.And(positive1, zero2),
            (1, 1) => Expression.And(positive1, positive2),
            _ => throw new ArgumentException($"Invalid pair: {pair}")
        };

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
}