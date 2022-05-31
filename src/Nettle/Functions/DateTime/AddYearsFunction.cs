namespace Nettle.Functions.DateTime
{
    using System;

    public sealed class AddYearsFunction : AddTimeFunctionBase
    {
        public AddYearsFunction()
            : base()
        { }

        protected override DateTime AddTime(DateTime date, double value)
        {
            return date.AddYears((int)value);
        }
    }
}
