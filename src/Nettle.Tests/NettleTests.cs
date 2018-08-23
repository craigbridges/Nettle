using Shouldly;
using System;
using Xunit;

namespace Nettle.Tests
{
    public class NettleTests
    {
        [Fact]
        public void CanTemplateString()
        {
            var source = @"Welcome {{Name}}";

            var model = new
            {
                Name = "John Smith"
            };

            var compiler = NettleEngine.GetCompiler();
            var template = compiler.Compile(source);
            var output = template(model);
            output.ShouldBe("Welcome John Smith");

            /* Result:
            Welcome John Smith
            */

        }
    }
}
