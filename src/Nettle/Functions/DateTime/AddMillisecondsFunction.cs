namespace Nettle.Functions.DateTime
{
    using System;

    public sealed class AddMillisecondsFunction : AddTimeFunctionBase
    {
        public AddMillisecondsFunction()
            : base()
        { }

        protected override DateTime AddTime(DateTime date, double value)
        {
            return date.AddMilliseconds(value);
        }
    }
}
