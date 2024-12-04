using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedKernel;
public static class Extensions
{
    public static string NormalizeUrl(this string baseUrl, params string[] parts)
    {
        if (baseUrl.IsNotNullOrWhiteSpace())
        {
            while (baseUrl.EndsWith("/"))
            {
                baseUrl = baseUrl.Remove(baseUrl.Length - 1);
            }

            if (parts.IsNotNullOrEmpty())
            {
                baseUrl = $"{baseUrl}/{string.Join("/", parts)}";
            }

            return baseUrl;
        }

        return string.Empty;
    }
    public static TValue? Deserialize<TValue>(this string json)
    {
        return JsonSerializer.Deserialize<TValue>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
        });
    }
    public static string Serialize(this object? value)
    {
        return JsonSerializer.Serialize(value, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
        });
    }
    public static T ToEnum<T>(this int value)
    {
        Type type = typeof(T);

        if (!type.IsEnum)
            throw new ArgumentException($"{type} is not an enum.");

        return (T)Enum.ToObject(type, value);

    }
    public static IntranetUser? GetUser(this AuthorizationFilterContext authorizationFilterContext)
    {
        if (authorizationFilterContext.HttpContext.User.Identity is not null && 
            authorizationFilterContext.HttpContext.User.Identity.IsAuthenticated && !authorizationFilterContext.HttpContext.Items.ContainsKey("IntranetUser"))
        {
            authorizationFilterContext.HttpContext.Items["IntranetUser"] = IntranetUser.Parse(authorizationFilterContext.HttpContext.User.Claims);
        }

        if (authorizationFilterContext.HttpContext.Items.ContainsKey("IntranetUser"))
        {
            return (IntranetUser)authorizationFilterContext.HttpContext.Items["IntranetUser"]!;
        }

        return null;
    }
    public static object Clone(this object ObjectToBeCloned)
    {
        return ObjectToBeCloned.Clone(ObjectToBeCloned.GetType());
    }
    public static object Clone(this object ObjectToBeCloned, Type TargetObjectType)
    {
        var _destobjectprops = TargetObjectType.GetProperties();
        var _ret = Activator.CreateInstance(TargetObjectType);
        var _srcobjectprops = ObjectToBeCloned.GetType().GetProperties();

        foreach (var _dstprop in _destobjectprops)
        {
            foreach (var _srcprop in _srcobjectprops)
            {
                if (_srcprop.Name.Equals(_dstprop.Name, StringComparison.InvariantCultureIgnoreCase) && _srcprop.CanRead && _dstprop.CanWrite)
                {
                    _dstprop.SetValue(_ret, _srcprop.GetValue(ObjectToBeCloned, null), null);
                    break;
                }
            }
        }

        return _ret;
    }

    public static bool IsHandlerInterface(Type type)
    {
        if (!type.IsGenericType)
            return false;

        var typeDefinition = type.GetGenericTypeDefinition();

        return typeDefinition == typeof(ICommandHandler<>)
               || typeDefinition == typeof(IQueryHandler<,>);
    }

    public static bool HasInterface(this Type type, Type interfaceType) => type.GetInterfacesOf(interfaceType).Length > 0;

    public static Type[] GetInterfacesOf(this Type type, Type interfaceType) => type.FindInterfaces((i, _) => i.GetGenericTypeDefinitionSafe() == interfaceType, null);

    public static Type GetGenericTypeDefinitionSafe(this Type type) => type.IsGenericType
        ? type.GetGenericTypeDefinition()
        : type;

    public static Type MakeGenericTypeSafe(this Type type, params Type[] typeArguments) => type.IsGenericType && !type.GenericTypeArguments.Any()
        ? type.MakeGenericType(typeArguments)
        : type;

    public static T DeserializeOrDefault<T>(string json)
    {
        try
        {
            return string.IsNullOrEmpty(json) ? default(T) : JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception ex)
        {
            return default(T);
        }
    }

    public static string AsJsonString(this object obj)
    {
        var content = JsonSerializer.Serialize(obj, new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
        });
        return content;
    }
}
