// ...existing code...
    private static Int10T Create(Int16 value) => new((Int16)((Int32)value).BalancedModulo((Int32)MaxValueConstant));
    private static Int10T Create(Int32 value) => new((Int16)(value.BalancedModulo((Int32)MaxValueConstant)));
    private static Int10T Create(Int64 value) => new((Int16)(value.BalancedModulo((Int64)MaxValueConstant)));
// ...existing code...

