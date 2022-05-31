namespace Nettle.Functions.DateTime
{
    using System;

    public sealed class AddMinutesFunction : AddTimeFunctionBase
    {
        public AddMinutesFunction()
            : base()
        { }

        protected override DateTime AddTime(DateTime date, double value)
        {
            return date.AddMinutes(value);
        }
    }
}
