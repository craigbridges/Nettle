namespace Nettle;

using System.IO;
using System.Threading.Tasks;

/// <summary>
/// Provides a mechanism for reading views into memory
/// </summary>
public static class ViewReader
{
    /// <summary>
    /// Asynchronously reads a view into memory
    /// </summary>
    /// <param name="filePath">The views file path</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The view contents</returns>
    public static async Task<NettleView> ReadAsync(string filePath, CancellationToken cancellationToken)
    {
        Validate.IsNotEmpty(filePath);

        if (false == File.Exists(filePath))
        {
            throw new IOException($"The file path '{filePath}' does not exist.");
        }

        var name = GetViewName(filePath);
        var content = await File.ReadAllTextAsync(filePath, cancellationToken).ConfigureAwait(false);

        return new NettleView(name, content);
    }

    /// <summary>
    /// Asynchronously reads all views found in a directory into memory
    /// </summary>
    /// <param name="path">The directory path</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A collection of matching views</returns>
    /// <remarks>
    /// Nested directories are also searched for matching files
    /// </remarks>
    public static async Task<IEnumerable<NettleView>> ReadAllAsync(string path, CancellationToken cancellationToken)
    {
        Validate.IsNotEmpty(path);

        if (false == Directory.Exists(path))
        {
            throw new IOException($"The directory '{path}' does not exist.");
        }

        var searchPattern = $"*.{NettleEngine.ViewFileExtension}";
        var matchingFiles = Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories);

        var tasks = new List<Task<NettleView>>();

        foreach (var filePath in matchingFiles)
        {
            tasks.Add(ReadAsync(filePath, cancellationToken));
        }

        return await Task.WhenAll(tasks).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the view name from a file path
    /// </summary>
    /// <param name="filePath">The file path</param>
    /// <returns>The view name</returns>
    private static string GetViewName(string filePath)
    {
        return Path.GetFileNameWithoutExtension(filePath);
    }
}
