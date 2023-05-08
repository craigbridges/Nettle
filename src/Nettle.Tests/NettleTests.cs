namespace Nettle.Tests
{
    using Shouldly;
    using System.Threading.Tasks;
    using Xunit;

    public class NettleTests
    {
        [Fact]
        public async Task CanTemplateString()
        {
            var source = @"Welcome {{Name}}";

            var model = new
            {
                Name = "John Smith"
            };

            var compiler = NettleEngine.GetCompiler();
            var template = compiler.Compile(source);
            var output = await template(model);

            output.ShouldBe("Welcome John Smith");

            /* Result:
            Welcome John Smith
            */

        }
    }
}
