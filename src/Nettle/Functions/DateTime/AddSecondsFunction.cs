namespace Nettle.Functions.DateTime
{
    using System;

    public sealed class AddSecondsFunction : AddTimeFunctionBase
    {
        public AddSecondsFunction()
            : base()
        { }

        protected override DateTime AddTime(DateTime date, double value)
        {
            return date.AddSeconds(value);
        }
    }
}
