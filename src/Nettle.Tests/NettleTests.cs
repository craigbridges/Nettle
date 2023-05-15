namespace Nettle.Tests
{
    using Nettle.Compiler;
    using Nettle.Data;
    using Nettle.NCalc;
    using Nettle.Web;
    using Shouldly;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class NettleTests
    {
        private readonly INettleCompiler _compiler;
        private readonly NettleTestModel _model;

        public NettleTests()
        {
            NettleEngine.RegisterResolvers(new NettleDataResolver(), new NettleNCalcResolver(), new NettleWebResolver());

            _compiler = NettleEngine.GetCompiler();

            _model = new()
            {
                Name = "John Smith",
                Gender = Gender.Male,
                BirthDate = new DateTime(1980, 1, 1),
                IsEmployed = true,
                Addresses = new()
                {
                    new()
                    {
                        AddressLine1 = "1 Fake Street",
                        City = "London",
                        Postcode = "WA1"
                    },
                    new()
                    {
                        AddressLine1 = "500 Long Street",
                        City = "Dreamland",
                        Postcode = "AA1 000"
                    }
                }
            };
        }

        [Fact]
        public async Task BasicModelBinding()
        {
            var source = "Welcome {{Name}}";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.ShouldBe("Welcome John Smith");
        }

        [Fact]
        public async Task BasicIfStatement()
        {
            var source = "{{if $.IsEmployed}}Employed{{/if}}";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.ShouldBe("Employed");
        }

        [Fact]
        public async Task BasicIfElseStatement()
        {
            var source = "{{if $.IsEmployed}}Employed{{else}}Not Employed{{/if}}";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.ShouldBe("Employed");
        }

        [Fact]
        public async Task MultipleIfElseStatement()
        {
            var source = @"{{if $.IsEmployed & $.Addresses.Count == 0}}
                                Employed
                           {{else if $.Addresses.Count > 0}}
                                Has Addresses
                           {{else}}
                                Not Employed
                           {{/if}}";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.Trim().ShouldBe("Has Addresses");
        }

        [Fact]
        public async Task BasicForEachLoop()
        {
            var source = @"{{each $.Addresses}}{{$.City}}{{/each}}";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.Trim().ShouldBe("LondonDreamland");
        }

        [Fact]
        public async Task NestedForEachLoop()
        {
            var source = @"{{each $.Addresses}}{{each $.City}}{{$}}{{/each}}{{/each}}";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.Trim().ShouldBe("LondonDreamland");
        }

        [Fact]
        public async Task WhileLoopCounter()
        {
            var source = @"{{var counter = 1}}
                           {{while counter < 10}}
	                           {{counter++}}
                           {{/while}}
                           {{counter}}";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.Trim().ShouldBe("10");
        }

        [Fact]
        public async Task RenderBasicPartialTemplate()
        {
            _compiler.RegisterTemplate("BasicPartial", "Partial");

            var source = @"[{{> BasicPartial $}}]";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.Trim().ShouldBe("[Partial]");
        }

        [Fact]
        public async Task RenderPartialTemplateInForEachStatement()
        {
            _compiler.RegisterTemplate("AddressPartial", "{{$.City}}");

            var source = @"{{each $.Addresses}}{{> AddressPartial $}}{{/each}}";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.Trim().ShouldBe("LondonDreamland");
        }

        [Fact]
        public async Task AutoRegisterViewsAndRenderPartialTemplates()
        {
            await _compiler.AutoRegisterViews(@"..\..\..\Templates", new CancellationToken());

            var source = @"{{> Template1 $}}{{> Template2 $}}{{> Template3 $}}";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.ShouldBe("123");
        }

        // TODO: different functions (web call, date manipulation), different flags, different models and bindings, assigning variables to async function calls
    }
}
