namespace Nettle.Functions.DateTime
{
    using System;

    public sealed class AddHoursFunction : AddTimeFunctionBase
    {
        public AddHoursFunction()
            : base()
        { }

        protected override DateTime AddTime(DateTime date, double value)
        {
            return date.AddHours(value);
        }
    }
}
