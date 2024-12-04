using SharedKernel;

namespace Kernel.Contract;

/// <summary>
/// Request custom http context
/// </summary>
public interface IHttpContext
{
    /// <summary>
    /// Get converted http request header value
    /// </summary>
    /// <typeparam name="T">Convert header value type</typeparam>
    /// <param name="key">Header key</param>
    /// <param name="defaultValue">Default header value</param>
    /// <returns>Converted http request header value</returns>
    T HeaderValueOrDefault<T>(string key, T defaultValue);

    /// <summary>
    /// Current request Http context
    /// </summary>
    Microsoft.AspNetCore.Http.HttpContext Current { get; }

    /// <summary>
    /// Extracted user from the request identity
    /// </summary>
    IntranetUser IntranetUser { get; }

    /// <summary>
    /// Request locale
    /// </summary>
    string Locale { get; }

    /// <summary>
    /// Client version
    /// </summary>    
    string Version { get; }

    /// <summary>
    /// Exception correlation id
    /// </summary>
    string CorrelationId { get; }

    /// <summary>
    /// Request source
    /// </summary>
    string Source { get; }

    /// <summary>
    /// Request IpAddress
    /// </summary>
    string IpAddress { get; }

    /// <summary>
    /// Request ClientAgent
    /// </summary>
    string ClientAgent { get; }

    /// <summary>
    /// Request server
    /// </summary>
    string Server { get; }

    /// <summary>
    /// Request rawUrl
    /// </summary>
    string RawUrl { get; }

    /// <summary>
    /// Request userId
    /// </summary>
    string UserId { get; }
}
