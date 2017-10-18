namespace Nettle.Compiler
{
    using System.ComponentModel;

    /// <summary>
    /// Defines a collection of template flags
    /// </summary>
    public enum TemplateFlag
    {
        [Description("Ignore Errors")]
        IgnoreErrors = 0,

        [Description("Enable Debug Mode")]
        DebugMode = 1,

        [Description("Allow Implicit Model Bindings")]
        AllowImplicitBindings = 2,

        [Description("Enforce Strict Variable Reassignments")]
        EnforceStrictReassign = 4,

        [Description("Disable Model Inheritance")]
        DisableModelInheritance = 8,

        [Description("Auto Format Output")]
        AutoFormat = 16,

        [Description("Minify Output")]
        Minify = 32,

        [Description("Use UTC Date and Time")]
        UseUtc = 64
    }
}
