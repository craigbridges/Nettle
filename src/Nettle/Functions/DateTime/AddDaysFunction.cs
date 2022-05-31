namespace Nettle.Functions.DateTime
{
    using System;

    public sealed class AddDaysFunction : AddTimeFunctionBase
    {
        public AddDaysFunction() 
            : base()
        { }

        protected override DateTime AddTime(DateTime date, double value)
        {
            return date.AddDays(value);
        }
    }
}
