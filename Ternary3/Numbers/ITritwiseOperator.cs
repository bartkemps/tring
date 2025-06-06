namespace Ternary3.Numbers;

/// <summary>Defines a mechanism for performing bitwise operations over two values.</summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
/// <typeparam name="TOther">The type that is used in the operation with <typeparamref name="TSelf" />.</typeparam>
/// <typeparam name="TResult">The type that contains the result of <typeparamref name="TSelf" /> op <typeparamref name="TOther" />.</typeparam>
public interface ITritwiseOperator<TSelf, TOther, TResult> where TSelf : ITritwiseOperator<TSelf, TOther, TResult>?
{

}