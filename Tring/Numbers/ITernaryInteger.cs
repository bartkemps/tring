
#nullable enable
namespace Tring.Numbers
{
  using System.Numerics;

  /// <summary>Defines an integer type that is represented in a base-2 format.</summary>
  /// <typeparam name="TSelf">The type that implements the interface.</typeparam>
  public interface ITernaryInteger<TSelf>:
    ITritwiseOperators<TSelf, TSelf, TSelf>,
    IConvertible,
    ISignedNumber<TSelf>,
    INumber<TSelf>,
    IShiftOperators<TSelf, int, TSelf>
    where TSelf : ITernaryInteger<TSelf>?
  {
    /// <summary>Computes the quotient and remainder of two values.</summary>
    /// <param name="left">The value which <paramref name="right" /> divides.</param>
    /// <param name="right">The value which divides <paramref name="left" />.</param>
    /// <returns>The quotient and remainder of <paramref name="left" /> divided by <paramref name="right" />.</returns>
    static virtual (TSelf Quotient, TSelf Remainder) DivRem(TSelf left, TSelf right)
    {
      var self = left / right;
      return (self, left - self * right);
    }
  }
}
