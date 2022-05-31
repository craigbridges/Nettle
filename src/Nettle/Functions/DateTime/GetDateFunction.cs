namespace Nettle.Functions.DateTime
{
    using System;

    public sealed class GetDateFunction : FunctionBase
    {
        public GetDateFunction() 
            : base()
        { }

        public override string Description => "Gets the current date and time.";

        protected override object? GenerateOutput(TemplateContext context, params object?[] parameterValues)
        {
            Validate.IsNotNull(context);

            var forceUtc = context.IsFlagSet(TemplateFlag.UseUtc);

            if (forceUtc)
            {
                return DateTime.UtcNow;
            }
            else
            {
                return TimeZoneInfo.ConvertTime(DateTime.UtcNow, NettleEngine.DefaultTimeZone);
            }
        }
    }
}
