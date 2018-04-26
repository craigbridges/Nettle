namespace Nettle.Compiler
{
    using System.ComponentModel;

    /// <summary>
    /// Defines a collection of template flags
    /// </summary>
    public enum TemplateFlag
    {
        /// <summary>
        /// When set, all rendering errors are ignored.
        /// </summary>
        /// <remarks>
        /// In cases where an error is generated, the output for 
        /// that block will be an empty string.
        /// </remarks>
        [Description("Ignore Errors")]
        IgnoreErrors = 0,

        /// <summary>
        /// When set, debug information is appended to the output.
        /// </summary>
        /// <remarks>
        /// The debug information is compiled automatically and 
        /// includes the render time, culture, time zone, model
        /// properties and variables (name and value).
        /// </remarks>
        [Description("Enable Debug Mode")]
        DebugMode = 1,

        /// <summary>
        /// When set, allows implicit model bindings.
        /// </summary>
        /// <remarks>
        /// Implicit model bindings do not require model properties 
        /// or variables to be defined. When a property or variable
        /// is not defined, null is returned. If this flag is not 
        /// set, properties and variables must be defined to use them.
        /// </remarks>
        [Description("Allow Implicit Model Bindings")]
        AllowImplicitBindings = 2,

        /// <summary>
        /// When set, forces variable types to remain constant.
        /// </summary>
        /// <remarks>
        /// By default, variables can be reassigned to any type.
        /// If enforce strict reassignments is on, variables cannot 
        /// change their type once they have been defined.
        /// </remarks>
        [Description("Enforce Strict Variable Reassignments")]
        EnforceStrictReassign = 4,

        /// <summary>
        /// When set, template model inheritance is disabled.
        /// </summary>
        /// <remarks>
        /// By default, nested code blocks inherit their parents
        /// data model including all properties and variables.
        /// Properties from the parent model are then merged into the 
        /// nested model. When a duplicate is found, the newer 
        /// property value is used instead.
        /// 
        /// A common scenario where a nested model inherits from the
        /// parent is during an each loop. Every item in the collection
        /// generates a new model.
        /// 
        /// If model inheritance is disabled, nested models do not
        /// inherit their parents properties or methods.
        /// </remarks>
        [Description("Disable Model Inheritance")]
        DisableModelInheritance = 8,

        /// <summary>
        /// When set, the rendered output is automatically formatted.
        /// </summary>
        /// <remarks>
        /// By default, the Nettle code is replaced with an empty
        /// string during the render process. However, this often 
        /// leaves extra line breaks and white space where the code
        /// had been laid out neatly.
        /// 
        /// Auto format attempts to remove unnecessary line breaks 
        /// and white space from the output. 
        /// </remarks>
        [Description("Auto Format Output")]
        AutoFormat = 16,

        /// <summary>
        /// When set, the rendered output is automatically minified.
        /// </summary>
        /// <remarks>
        /// By default, line breaks and white space is preserved 
        /// during the render process.
        /// 
        /// If the minify flag is set, all line breaks and extra 
        /// white space is removed from the output.
        /// </remarks>
        [Description("Minify Output")]
        Minify = 32,

        /// <summary>
        /// When set, forces dates to be Universal Time Coordinated (UTC).
        /// </summary>
        /// <remarks>
        /// By default, Nettle generates dates using the default time zone.
        /// 
        /// If the use UTC flag is set, all dates generated are always in
        /// UTC time regardless of the default time zone. When set, the 
        /// rendering engine also outputs dates of local kind as UTC.
        /// </remarks>
        [Description("Use UTC Date and Time")]
        UseUtc = 64
    }
}
