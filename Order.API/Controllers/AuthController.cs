using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Domain.Models;
using SharedKernel;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public ActionResult Login([FromBody] LoginModel login)
    {
        if ((login.Username == "admin" ||  login.Username ==  "user") && login.Password == "123")
        {
            var token = CustomsExtensions.GenerateJwtToken(login.Username, _config);
            return Result.Success(token);
        }

        return Unauthorized();
    }
    //private string GenerateJwtToken(string username)
    //{ 
    //    var role = CustomsExtensions.GetEnumFromDescription<Roles>(username).ToString();
    //    var claims = new[]
    //    {
    //            new Claim(ClaimTypes.Role, role),
    //            new Claim(ClaimTypes.Name, username),
    //    };                      
    //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
    //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //    var token = new JwtSecurityToken(
    //      claims: claims,                                 
    //      expires: DateTime.Now.AddHours(1),              
    //      signingCredentials: creds                       
    //  );

    //    return new JwtSecurityTokenHandler().WriteToken(token);      
    //}  
}

