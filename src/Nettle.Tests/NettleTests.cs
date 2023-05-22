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
        public async Task ModelBinding_Simple()
        {
            var source = "Welcome {{Name}}";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.ShouldBe("Welcome John Smith");
        }

        [Fact]
        public async Task ModelBinding_Indexer()
        {
            var source = "Welcome {{Name[0]}}";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.ShouldBe("Welcome J");
        }

        [Fact]
        public async Task IfStatement_Simple()
        {
            var source = "{{if $.IsEmployed}}Employed{{/if}}";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.ShouldBe("Employed");
        }

        [Fact]
        public async Task IfStatement_Single_Else()
        {
            var source = "{{if $.IsEmployed}}Employed{{else}}Not Employed{{/if}}";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.ShouldBe("Employed");
        }

        [Fact]
        public async Task IfStatement_Multiple_Else()
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
        public async Task IfStatement_Complex_Condition()
        {
            var source = @"{{var available = true}}
                           {{var number1 = 15}}
                           {{var number2 = 10}}
                           {{var name1 = ""Craig""}}
                           {{var name2 = ""John""}}

                           {{if available & (number1 > number2) | (name1 != name2)}}
	                           Success
                           {{/if}}";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.Trim().ShouldBe("Success");
        }

        [Fact]
        public async Task ForEachLoop_Simple()
        {
            var source = @"{{each $.Addresses}}{{$.City}}{{/each}}";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.Trim().ShouldBe("LondonDreamland");
        }

        [Fact]
        public async Task ForEachLoop_Nested()
        {
            var source = @"{{each $.Addresses}}{{each $.City}}{{$}}{{/each}}{{/each}}";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.Trim().ShouldBe("LondonDreamland");
        }

        [Fact]
        public async Task WhileLoop_Simple_Counter()
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
        public async Task RenderPartial_Simple_Template()
        {
            _compiler.RegisterTemplate("BasicPartial", "Partial");

            var source = @"[{{> BasicPartial $}}]";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.Trim().ShouldBe("[Partial]");
        }

        [Fact]
        public async Task RenderPartial_Within_ForEachStatement()
        {
            _compiler.RegisterTemplate("AddressPartial", "{{$.City}}");

            var source = @"{{each $.Addresses}}{{> AddressPartial $}}{{/each}}";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.Trim().ShouldBe("LondonDreamland");
        }

        [Fact]
        public async Task RenderPartial_After_AutoRegisterViews()
        {
            await _compiler.AutoRegisterViews(@"..\..\..\Templates", new CancellationToken());

            var source = @"{{> Template1 $}}{{> Template2 $}}{{> Template3 $}}";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.ShouldBe("123");
        }

        [Fact]
        public async Task Function_HtmlEncode()
        {
            var source = "{{@HtmlEncode(\"<>\")}}";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.ShouldBe("&lt;&gt;");
        }

        [Fact]
        public async Task Function_Multiply()
        {
            var source = "{{var firstNumber = 10}}" +
                         "{{var secondNumber = 20}}" +
                         "{{@Multiply(firstNumber, secondNumber)}}";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.ShouldBe("200");
        }

        [Fact]
        public async Task Function_HttpGet()
        {
            var source = "{{@HttpGet(\"https://postman-echo.com/get\")}}";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.ShouldStartWith("{");
        }

        [Fact]
        public async Task Function_HttpPost()
        {
            var source = "{{@HttpPost(\"https://postman-echo.com/post\", \"Test\")}}";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.ShouldContain("Test");
        }

        [Fact]
        public async Task Function_Evaluate()
        {
            var source = "{{@Evaluate(\"((10*5)+150)/10\")}}";

            var template = _compiler.Compile(source);
            var output = await template(_model, new CancellationToken());

            output.ShouldBe("20");
        }

        // TODO: different flags, different models and bindings, assigning variables to async function calls
    }
}
