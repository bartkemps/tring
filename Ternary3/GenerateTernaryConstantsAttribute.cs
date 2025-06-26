namespace Ternary3
{
    /// <summary>
    /// Attribute to indicate that ternary constants should be generated for the target type or member.
    /// </summary>
    /// <param name="enabled">
    /// Whether or not to generate ternary constants.
    /// This can be used to override the assemby level setting for classes or structs.
    /// </param>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class GenerateTernaryConstantsAttribute(bool enabled = true) : Attribute
    {
        public bool Enabled { get; } = enabled;
    }
}
