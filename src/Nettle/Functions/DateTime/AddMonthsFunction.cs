namespace Nettle.Functions.DateTime
{
    using System;

    public sealed class AddMonthsFunction : AddTimeFunctionBase
    {
        public AddMonthsFunction()
            : base()
        { }

        protected override DateTime AddTime(DateTime date, double value)
        {
            return date.AddMonths((int)value);
        }
    }
}
