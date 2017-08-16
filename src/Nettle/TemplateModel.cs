namespace Nettle
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a rendered template model
    /// </summary>
    public class TemplateModel
    {
        /// <summary>
        /// Constructs an empty template model
        /// </summary>
        public TemplateModel()
        {
            this.Pointers = new Dictionary<string, object>();
            this.Variables = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets the models pointers
        /// </summary>
        public Dictionary<string, object> Pointers
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the models variables
        /// </summary>
        public Dictionary<string, object> Variables
        {
            get;
            private set;
        }
    }
}
