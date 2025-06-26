namespace Ternary3
{
    /// <summary>
    /// Attribute to indicate that ternary constants should be generated for the target type or member.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class GenerateTernaryConstantsAttribute(bool enabled = true) : Attribute
    {
        public bool Enabled { get; } = enabled;
    }
}
