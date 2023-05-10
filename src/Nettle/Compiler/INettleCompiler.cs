namespace Nettle.Compiler;

using System.Threading.Tasks;

/// <summary>
/// Defines a contract for Nettle compiler
/// </summary>
public interface INettleCompiler
{
    /// <summary>
    /// Compiles the template content specified
    /// </summary>
    /// <param name="templateContent">The template content</param>
    /// <returns>A function that will generate rendered content</returns>
    Func<object, CancellationToken, Task<string>> Compile(string templateContent);

    /// <summary>
    /// Asynchronously compiles the view specified as a template
    /// </summary>
    /// <param name="templatePath">The templates file path</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A function that will generate rendered content</returns>
    Task<Func<object, CancellationToken, Task<string>>> CompileView(string templatePath, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously automatically registers all views found in a directory
    /// </summary>
    /// <param name="directoryPath">The directory path</param>
    /// <param name="cancellationToken">The cancellation token</param>
    Task AutoRegisterViews(string directoryPath, CancellationToken cancellationToken);

    /// <summary>
    /// Registers a template to be used with the compiler
    /// </summary>
    /// <param name="name">The template name</param>
    /// <param name="templateContent">The template content</param>
    void RegisterTemplate(string name, string templateContent);

    /// <summary>
    /// Registers the function specified with the compiler
    /// </summary>
    /// <param name="function">The function register</param>
    void RegisterFunction(IFunction function);

    /// <summary>
    /// Disables the function specified
    /// </summary>
    /// <param name="functionName">The function name</param>
    void DisableFunction(string functionName);

    /// <summary>
    /// Disables all registered functions
    /// </summary>
    void DisableAllFunctions();

    /// <summary>
    /// Enables the function specified
    /// </summary>
    /// <param name="functionName">The function name</param>
    void EnableFunction(string functionName);
}
