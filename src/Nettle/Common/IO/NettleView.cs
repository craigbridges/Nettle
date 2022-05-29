namespace Nettle
{
    /// <summary>
    /// Represents a Nettle view
    /// </summary>
    /// <param name="Name">The name of the view</param>
    /// <param name="Content">The views content</param>
    public record class NettleView(string Name, string Content);
}
