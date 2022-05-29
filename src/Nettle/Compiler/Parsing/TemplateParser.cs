namespace Nettle.Compiler.Parsing
{
    internal sealed class TemplateParser : ITemplateParser
    {
        private readonly IBlockifier _blockifier;

        public TemplateParser(IBlockifier blockifier)
        {
            Validate.IsNotNull(blockifier);

            _blockifier = blockifier;
        }

        public Template Parse(string content)
        {
            if (String.IsNullOrWhiteSpace(content))
            {
                return new Template(content);
            }
            else
            {
                var blocks = _blockifier.Blockify(content);

                return new Template(content, blocks);
            }
        }
    }
}
