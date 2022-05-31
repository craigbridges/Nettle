namespace Nettle.Functions.General
{
    using System;

    public sealed class GetEnvironmentVariableFunction : FunctionBase
    {
        public GetEnvironmentVariableFunction() : base()
        {
            DefineRequiredParameter("Name", "The name of the variable to get.", typeof(string));
        }

        public override string Description => "Retrieves the value of a Nettle environment variable.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var name = GetParameterValue<string>("Name", parameterValues);

            return NettleEnvironment.GetEnvironmentVariable(name ?? String.Empty);
        }
    }
}
