// See https://aka.ms/new-console-template for more information

using Ternary3.Numbers;
using Ternary3.Operators;

Console.WriteLine("Hello, World!");

{
 Int27T a = 1; // 01
 Int27T b = 2; // 1T
 var c = a | BinaryLookup.And | b; // 0T : the smallest value for each trit
 Console.WriteLine($"1 AND 2 = {(Int27T)c}"); // prints -1
}

{
 TritArray3 a = (Int3T)5; // 1TT
 var b = a | UnaryLookup.AbsoluteValue; // 111
 Console.WriteLine($"Absolute value of 5 = {(Int3T)b}"); // prints 13
}

{
 TritArray3 a = (Int3T)5; // 1TT
 var b = a | Unary.AbsoluteValue; // 111
 Console.WriteLine($"Absolute value of 5 = {(Int3T)b}"); // prints 13
}


{
 TritArray3 a = 5; // 1TT
 var b = a | UnaryLookup.AbsoluteValue; // 111
 Console.WriteLine($"Absolute value of 5 = {(Int3T)b}"); // prints 13
}

{
 Int3T a = 5; // 1TT
 var b = a | UnaryLookup.AbsoluteValue; // 111
 Console.WriteLine($"Absolute value of 5 = {(Int3T)b}"); // prints 13
}

{
 Int3T a = 5; // 1TT
 var b = a | Unary.AbsoluteValue; // 111
 Console.WriteLine($"Absolute value of 5 = {(Int3T)b}"); // prints 13
}

{
 Int9T a = 5; // 1TT
 var b = a | UnaryLookup.AbsoluteValue; // 111
 Console.WriteLine($"Absolute value of 5 = {(Int9T)b}"); // prints 13
}

{
 Int9T a = 5; // 1TT
 var b = a | Unary.AbsoluteValue; // 111
 Console.WriteLine($"Absolute value of 5 = {(Int9T)b}"); // prints 13
}

{
 Int27T a = 5; // 1TT
 var b = a | UnaryLookup.AbsoluteValue; // 111
 Console.WriteLine($"Absolute value of 5 = {(Int27T)b}"); // prints 13
}

{
 Int27T a = 5; // 1TT
 var b = a | Unary.AbsoluteValue; // 111
 Console.WriteLine($"Absolute value of 5 = {(Int27T)b}"); // prints 13
}
