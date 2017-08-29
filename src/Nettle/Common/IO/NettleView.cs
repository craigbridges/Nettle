namespace Nettle
{
    /// <summary>
    /// Represents a Nettle view
    /// </summary>
    public class NettleView
    {
        /// <summary>
        /// Constructs the view with the name and content
        /// </summary>
        /// <param name="name">The view name</param>
        /// <param name="content">The views content</param>
        public NettleView
            (
                string name,
                string content
            )
        {
            Validate.IsNotEmpty(name);
            Validate.IsNotEmpty(content);

            this.Name = name;
            this.Content = content;
        }

        /// <summary>
        /// Gets the view name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the views content
        /// </summary>
        public string Content { get; private set; }
    }
}
