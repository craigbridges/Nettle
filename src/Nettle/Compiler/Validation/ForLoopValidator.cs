namespace Nettle.Compiler.Validation
{
    using Nettle.Compiler.Parsing;
    using Nettle.Compiler.Parsing.Blocks;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a for each loop code block validator
    /// </summary>
    internal sealed class ForLoopValidator : IBlockValidator
    {
        /// <summary>
        /// Validates the templates for each loops
        /// </summary>
        /// <param name="template">The template</param>
        /// <returns>An array of errors</returns>
        public TemplateValidationError[] ValidateTemplate
            (
                Template template
            )
        {
            Validate.IsNotNull(template);

            var loops = template.FindBlocks<ForEachLoop>();

            if (false == loops.Any())
            {
                return new TemplateValidationError[] { };
            }
            else
            {
                var errors = new List<TemplateValidationError>();

                foreach (var loop in loops)
                {
                    switch (loop.CollectionType)
                    {
                        case NettleValueType.Number:
                        case NettleValueType.Boolean:

                            errors.Add
                            (
                                new TemplateValidationError
                                (
                                    loop,
                                    "Invalid for each loop collection type."
                                )
                            );

                            break;
                    }
                }

                return errors.ToArray();
            }
        }
    }
}
