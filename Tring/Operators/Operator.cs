namespace Tring.Operators;

using Numbers;

internal struct Operator
{
    public static readonly Trit[,] And = new Trit[3, 3]
    {
        { Trit.Positive, Trit.Zero, Trit.Negative }, // Negative (-1) row
        { Trit.Zero, Trit.Zero, Trit.Zero }, // Zero (0) row
        { Trit.Negative, Trit.Zero, Trit.Positive } // Positive (1) row
    };

    public static readonly Trit[,] Or = new Trit[3, 3]
    {
        { Trit.Negative, Trit.Zero, Trit.Positive }, // Negative (-1) row
        { Trit.Zero, Trit.Zero, Trit.Positive }, // Zero (0) row
        { Trit.Positive, Trit.Positive, Trit.Positive } // Positive (1) row
    };

    public static readonly Trit[,] Xor = new Trit[3, 3]
    {
        { Trit.Positive, Trit.Negative, Trit.Zero }, // Negative (-1) row
        { Trit.Negative, Trit.Zero, Trit.Positive }, // Zero (0) row
        { Trit.Zero, Trit.Positive, Trit.Negative } // Positive (1) row
    };
}