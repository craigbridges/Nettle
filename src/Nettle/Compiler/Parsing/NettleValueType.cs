namespace Nettle.Compiler.Parsing
{
    using System.ComponentModel;

    /// <summary>
    /// Defines a collection of Nettle value types
    /// </summary>
    internal enum NettleValueType
    {
        /// <summary>
        /// Represents text as a sequence of characters
        /// </summary>
        [Description("String")]
        String = 0,

        /// <summary>
        /// Represents any number type
        /// </summary>
        /// <remarks>
        /// Internally numbers are handled as the double type
        /// </remarks>
        [Description("Number")]
        Number = 1,

        /// <summary>
        /// Represents either true or false
        /// </summary>
        [Description("Boolean")]
        Boolean = 2,

        /// <summary>
        /// Represents a model binding path
        /// </summary>
        /// <remarks>
        /// A model binding can be either the name of a property or variable.
        /// 
        /// Dot-separated paths can be used to denote nested properties.
        /// 
        /// Indexers are also supported at the end of paths. They are denoted 
        /// using the [0] syntax and can be used with any enumerable types.
        /// </remarks>
        [Description("Model Binding")]
        ModelBinding = 4,

        /// <summary>
        /// Represents the name of a variable
        /// </summary>
        /// <remarks>
        /// Variables are defined and managed by the template context
        /// </remarks>
        [Description("Variable")]
        Variable = 8,

        /// <summary>
        /// Represents a call to a specific function
        /// </summary>
        [Description("Function Call")]
        Function = 16,

        /// <summary>
        /// Represents a boolean expression
        /// </summary>
        /// <remarks>
        /// A boolean expression is an expression which evaluates to either 
        /// true or false.
        /// 
        /// The expression is broken down into one or more conditions 
        /// joined by operators. A condition can be either a single value 
        /// that evaluates to true or false, or a comparison of two values.
        /// </remarks>
        [Description("Boolean Expression")]
        BooleanExpression = 32,

        /// <summary>
        /// Represents a key value pair of type object-object
        /// </summary>
        /// <remarks>
        /// The key and value types can be any of the Nettle value types
        /// </remarks>
        [Description("Key Value Pair")]
        KeyValuePair = 64,

        /// <summary>
        /// Represents an anonymous type object
        /// </summary>
        /// <remarks>
        /// Anonymous types provide a convenient way to encapsulate a set 
        /// of read-only properties into a single object without having 
        /// to explicitly define a type first.
        /// </remarks>
        [Description("Anonymous Type")]
        AnonymousType = 128
    }
}
