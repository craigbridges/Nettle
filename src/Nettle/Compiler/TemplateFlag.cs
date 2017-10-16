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

        [Description("Debug Mode")]
        DebugMode = 1
    }
}
