using Kernel.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.ComponentModel;

namespace SharedKernel;
/// <summary>
/// Request Http context with custom values
/// </summary>
public class HttpContext : IHttpContext
{
    #region Members
    private readonly IHttpContextAccessor _httpContextAccessor = null;
    private readonly ILogger<HttpContext> _logger = null;
    #endregion

    #region Constructors
    /// <summary>
    /// Default Constructor 
    /// </summary>
    /// <param name="httpContextAccessor">Http context accessor</param>
    public HttpContext(IHttpContextAccessor httpContextAccessor, ILogger<HttpContext> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    /// <summary>
    /// Current request Http context
    /// </summary>
    public Microsoft.AspNetCore.Http.HttpContext Current => _httpContextAccessor.HttpContext;

    /// <summary>
    /// Request locale
    /// </summary>
    public string Locale
    {
        get
        {
            var hdLocale = HeaderValueOrDefault(Constants.PropertyName.ClientValues.Locale, "");
            if (string.IsNullOrWhiteSpace(hdLocale))
            {
                hdLocale = "en";

                if (Current.IsNotNull() && Current.Request.IsNotNull() && Current.Request.Headers.IsNotNull())
                {
                    if (Current.Request.Headers.ContainsKey(Constants.PropertyName.ClientValues.Locale))
                    {
                        Current.Request.Headers[Constants.PropertyName.ClientValues.Locale] = hdLocale;
                    }
                    else
                    {
                        Current.Request.Headers.Add(Constants.PropertyName.ClientValues.Locale, hdLocale);
                    }
                }
            }

            return hdLocale;
        }
    }

    /// <summary>
    /// Client version
    /// </summary>
    public string Version
    {
        get
        {
            var hdVersion = HeaderValueOrDefault(Constants.PropertyName.ClientValues.Version, "");
            if (string.IsNullOrWhiteSpace(hdVersion))
            {
                hdVersion = "0.0.0";

                if (Current.IsNotNull() && Current.Request.IsNotNull() && Current.Request.Headers.IsNotNull())
                {
                    if (Current.Request.Headers.ContainsKey(Constants.PropertyName.ClientValues.Version))
                    {
                        Current.Request.Headers[Constants.PropertyName.ClientValues.Version] = hdVersion;
                    }
                    else
                    {
                        Current.Request.Headers.Add(Constants.PropertyName.ClientValues.Version, hdVersion);
                    }
                }
            }

            return hdVersion;
        }
    }

    /// <summary>
    /// Exception correlation id
    /// </summary>
    /// 
    public string CorrelationId
    {
        get
        {
            var hdCorrelationId = HeaderValueOrDefault(Constants.PropertyName.ClientValues.CorrelationId, "");
            if (string.IsNullOrWhiteSpace(hdCorrelationId))
            {
                hdCorrelationId = Guid.NewGuid().ToString();

                if (Current.IsNotNull() && Current.Request.IsNotNull() && Current.Request.Headers.IsNotNull())
                {
                    if (Current.Request.Headers.ContainsKey(Constants.PropertyName.ClientValues.CorrelationId))
                    {
                        Current.Request.Headers[Constants.PropertyName.ClientValues.CorrelationId] = hdCorrelationId;
                    }
                    else
                    {
                        Current.Request.Headers.Add(Constants.PropertyName.ClientValues.CorrelationId, hdCorrelationId);
                    }
                }
            }

            return hdCorrelationId;
        }
    }

    /// <summary>
    /// Request source
    /// </summary>
    public string Source
    {
        get
        {
            var hdSource = HeaderValueOrDefault(Constants.PropertyName.ClientValues.Source, "");
            if (string.IsNullOrWhiteSpace(hdSource))
            {
                hdSource = "unknown";

                if (Current.IsNotNull() && Current.Request.IsNotNull() && Current.Request.Headers.IsNotNull())
                {
                    if (Current.Request.Headers.ContainsKey(Constants.PropertyName.ClientValues.Source))
                    {
                        Current.Request.Headers[Constants.PropertyName.ClientValues.Source] = hdSource;
                    }
                    else
                    {
                        Current.Request.Headers.Add(Constants.PropertyName.ClientValues.Source, hdSource);
                    }
                }
            }

            return hdSource;
        }
    }

    /// <summary>
    /// Request ip address
    /// </summary>
    public string IpAddress
    {
        get
        {
            var hdIpAddress = HeaderValueOrDefault(Constants.PropertyName.ClientValues.ClientIp, "");
            if (string.IsNullOrWhiteSpace(hdIpAddress))
            {
                hdIpAddress = "unknown";

                if (Current.IsNotNull() && Current.Request.IsNotNull() && Current.Request.Headers.IsNotNull())
                {
                    if (Current.Request.Headers.ContainsKey(Constants.PropertyName.ClientValues.ClientIp))
                    {
                        Current.Request.Headers[Constants.PropertyName.ClientValues.ClientIp] = hdIpAddress;
                    }
                    else
                    {
                        Current.Request.Headers.Add(Constants.PropertyName.ClientValues.ClientIp, hdIpAddress);
                    }
                }
            }

            return hdIpAddress;
        }
    }

    /// <summary>
    /// Request client agent
    /// </summary>
    public string ClientAgent
    {
        get
        {
            var hdClientAgent = HeaderValueOrDefault(Constants.PropertyName.ClientValues.ClientAgent, "");
            if (string.IsNullOrWhiteSpace(hdClientAgent))
            {
                hdClientAgent = HeaderValueOrDefault("User-Agent", "");
                if (string.IsNullOrWhiteSpace(hdClientAgent))
                {
                    hdClientAgent = "unknown";
                }

                if (Current.IsNotNull() && Current.Request.IsNotNull() && Current.Request.Headers.IsNotNull())
                {
                    if (Current.Request.Headers.ContainsKey(Constants.PropertyName.ClientValues.ClientAgent))
                    {
                        Current.Request.Headers[Constants.PropertyName.ClientValues.ClientAgent] = hdClientAgent;
                    }
                    else
                    {
                        Current.Request.Headers.Add(Constants.PropertyName.ClientValues.ClientAgent, hdClientAgent);
                    }
                }
            }

            return hdClientAgent;
        }
    }

    /// <summary>
    /// Request source
    /// </summary>
    public string Server
    {
        get
        {
            var hdServer = HeaderValueOrDefault(Constants.PropertyName.ClientValues.Server, "");
            if (string.IsNullOrWhiteSpace(hdServer))
            {
                hdServer = "unknown";

                if (Current.IsNotNull() && Current.Request.IsNotNull() && Current.Request.Headers.IsNotNull())
                {
                    if (Current.Request.Headers.ContainsKey(Constants.PropertyName.ClientValues.Server))
                    {
                        Current.Request.Headers[Constants.PropertyName.ClientValues.Server] = hdServer;
                    }
                    else
                    {
                        Current.Request.Headers.Add(Constants.PropertyName.ClientValues.Server, hdServer);
                    }
                }
            }

            return hdServer;
        }
    }

    /// <summary>
    /// Request source
    /// </summary>
    public string RawUrl
    {
        get
        {
            var hdRawUrl = HeaderValueOrDefault(Constants.PropertyName.ClientValues.RawUrl, "");
            if (string.IsNullOrWhiteSpace(hdRawUrl))
            {
                hdRawUrl = "unknown";

                if (Current.IsNotNull() && Current.Request.IsNotNull() && Current.Request.Headers.IsNotNull())
                {
                    if (Current.Request.Headers.ContainsKey(Constants.PropertyName.ClientValues.RawUrl))
                    {
                        Current.Request.Headers[Constants.PropertyName.ClientValues.RawUrl] = hdRawUrl;
                    }
                    else
                    {
                        Current.Request.Headers.Add(Constants.PropertyName.ClientValues.RawUrl, hdRawUrl);
                    }
                }
            }

            return hdRawUrl;
        }
    }

    /// <summary>
    /// Request source
    /// </summary>
    public string UserId
    {
        get
        {
            var hdUserId = HeaderValueOrDefault(Constants.PropertyName.ClientValues.UserId, "");
            if (string.IsNullOrWhiteSpace(hdUserId) || hdUserId == "unknown")
            {
                hdUserId = "unknown";

                if (IntranetUser.IsNotNull() && IntranetUser.UserId > 0)
                {
                    hdUserId = IntranetUser.UserId.ToString();
                }
                if (Current.IsNotNull() && Current.Request.IsNotNull() && Current.Request.Headers.IsNotNull())
                {
                    if (Current.Request.Headers.ContainsKey(Constants.PropertyName.ClientValues.UserId))
                    {
                        Current.Request.Headers[Constants.PropertyName.ClientValues.UserId] = hdUserId;
                    }
                    else
                    {
                        Current.Request.Headers.Add(Constants.PropertyName.ClientValues.UserId, hdUserId);
                    }
                }
            }

            return hdUserId;
        }
    }

    /// <summary>
    /// Extracted user from the request identity
    /// </summary>
    public IntranetUser IntranetUser
    {
        get
        {
            if (Current.IsNotNull() && Current.Request.IsNotNull() && Current.Request.Headers.IsNotNull())
            {
                if (!Current.User.Identity.IsAuthenticated)
                {
                    _logger.LogWarning("User is not authenticated");
                }
                if (Current.User.Identity.IsAuthenticated && !Current.Items.ContainsKey("IntranetUser"))
                {
                    Current.Items["IntranetUser"] = IntranetUser.Parse(Current.User.Claims);
                }

                if (Current.Items.ContainsKey("IntranetUser"))
                {
                    return (IntranetUser)Current.Items["IntranetUser"];
                }
                else
                {
                    _logger.LogWarning("HttpContext does not have IntranetUser item");
                }
            }
            else
            {
                _logger.LogWarning("HttpContext is nothing");
            }

            return new IntranetUser(false, 0, "", null, null, null, null, null, null, null, null) { };
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Get converted http request header value
    /// </summary>
    /// <typeparam name="T">Convert header value type</typeparam>
    /// <param name="key">Header key</param>
    /// <param name="defaultValue">Default header value</param>
    /// <returns>Converted http request header value</returns>
    public T HeaderValueOrDefault<T>(string key, T defaultValue)
    {
        T val = defaultValue;

        try
        {
            StringValues valStr = string.Empty;
            if (!string.IsNullOrWhiteSpace(key))
            {

                if (Current != null && Current.Request != null && Current.Request.Headers.TryGetValue(key, out valStr))
                {
                    var stringValue = valStr.FirstOrDefault();

                    if (!string.IsNullOrWhiteSpace(stringValue))
                    {
                        Type typeOfT = typeof(T);

                        if (typeOfT.IsEnum)
                        {
                            var enumInt = 0;
                            if (int.TryParse(stringValue, out enumInt))
                                return enumInt.ToEnum<T>();
                            else if (Enum.IsDefined(typeOfT, stringValue))
                                return (T)Enum.Parse(typeOfT, stringValue);
                        }

                        return (T)TypeDescriptor.GetConverter(typeOfT).ConvertFromInvariantString(stringValue);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error While get HeaderValue from HttpContext");
        }

        return val;
    }
    #endregion
}