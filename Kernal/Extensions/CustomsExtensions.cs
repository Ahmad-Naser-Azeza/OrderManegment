using Kernel.Enum;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

public static class CustomsExtensions
{
    public static TEnum? GetEnumFromDescription<TEnum>(string description) where TEnum : struct, Enum
    {
        foreach (var value in Enum.GetValues(typeof(TEnum)))
        {
            FieldInfo field = typeof(TEnum).GetField(value.ToString());
            DescriptionAttribute attribute = field?.GetCustomAttribute<DescriptionAttribute>();

            if (attribute != null && attribute.Description == description)
            {
                return (TEnum)value;
            }
        }
        return null; 
    }
    public static string GenerateJwtToken(this string username, IConfiguration config)
    {        
        var role = CustomsExtensions.GetEnumFromDescription<Roles>(username).ToString();

        // Create claims
        var claims = new[]
        {
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Name, username),
        };
       
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));        
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Create the JWT token
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}