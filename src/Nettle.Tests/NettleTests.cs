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

            output.EndsWith("Has Addresses");
        }

        // TODO: test foreach, while, render partial, different functions, different flags, different models and bindings, assigning variables to async function calls
    }
}
