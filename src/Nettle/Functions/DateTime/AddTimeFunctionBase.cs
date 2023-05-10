namespace Nettle.Functions.DateTime;

using System;
using System.Threading.Tasks;

/// <summary>
/// Represent a base function for adding time to a date
/// </summary>
public abstract class AddTimeFunctionBase : FunctionBase
{
    public AddTimeFunctionBase() : base()
    {
        DefineRequiredParameter("Date", "The date and time to adjust.", typeof(DateTime));
        DefineRequiredParameter("ValueToAdd", "A number of whole and fractional time units.", typeof(double));
    }

    public override string Description => "Adds a unit of time to a DateTime value.";

    protected override Task<object?> GenerateOutput(FunctionExecutionRequest request, CancellationToken cancellationToken)
    {
        var oldDate = GetParameterValue<DateTime>("Date", request);
        var valueToAdd = GetParameterValue<double>("ValueToAdd", request);
        var newDate = AddTime(oldDate, valueToAdd);

        return Task.FromResult<object?>(newDate);
    }

    protected abstract DateTime AddTime(DateTime date, double value);
}
