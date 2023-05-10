namespace Nettle.NCalc.Functions;

using global::NCalc;
using Nettle.Functions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class EvaluateFunction : FunctionBase
{
    public EvaluateFunction()
    {
        DefineRequiredParameter("Expression", "The mathematical expression to evaluate", typeof(string));
    }

    public override string Description => "Evaluates a single mathematical expression.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var expression = GetParameterValue<string>("Expression", request);

        if (request.ParameterValues.Length > 1)
        {
            var formatValues = request.ParameterValues.Skip(1);

            expression = String.Format(expression ?? String.Empty, formatValues.ToArray());
        }

        var result = new Expression(expression).Evaluate();

        return Task.FromResult<object?>(result);
    }
}
