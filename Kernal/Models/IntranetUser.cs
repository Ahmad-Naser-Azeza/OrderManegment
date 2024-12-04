using System.Security.Claims;

namespace SharedKernel;
public class IntranetUser
{
    private bool _IsAuthenticated;
    private long _userId;
    private string _userName;
    private List<string> _Permissions = new List<string>();
    private bool v1;
    private long v2;
    private string? userName;
    private List<long> list;
    private string? _externalCode;
    private string? _customercode;
    private string? _branchcode;
    private string? _supplierCode;
    private string? _timeZone;
    private string? _email;
    private string? _phoneNumber;
    public IntranetUser()
    { }
    public IntranetUser(bool v1, long v2, string v3, List<string> v4, string? v5, string? v6, string? v7, string? v8, string? v9, string? v10, string? v11)
    {
        this._IsAuthenticated = v1;
        this._userId = v2;
        this._userName = v3;
        this._Permissions = v4;
        this._externalCode = v5;
        this._customercode = v6;
        this._branchcode = v7;
        this._supplierCode = v8;
        this._timeZone = v9;
        this._email = v10;
        this._phoneNumber = v11;
    }
    public bool IsAuthenticated { get { return _IsAuthenticated; } }
    public long UserId { get { return _userId; } }
    public string UserName { get { return _userName; } }
    public List<string> Permissions { get { return _Permissions; } }
    public string? ExternalCode { get { return _externalCode; } }
    public string? CustomerCode { get { return _customercode; } }
    public string? BranchCode { get { return _branchcode; } }
    public string? SupplierCode { get { return _supplierCode; } }
    public string? TimeZone { get { return _timeZone; } }
    public string? Email { get { return _email; } }
    public string? PhoneNumber { get { return _phoneNumber; } }

    public static IntranetUser Parse(IEnumerable<Claim> claims)
    {
        if (claims != null && claims.Any())
        {
            var userId = claims.FirstOrDefault(x => x.Type.EqualsIgnoreCase("id"))?.Value.ToLong();
            var userName = claims.FirstOrDefault(x => x.Type.EqualsIgnoreCase("username"))?.Value;

            var ExternalCode = claims.FirstOrDefault(x => x.Type.EqualsIgnoreCase("externalcode"))?.Value;
            var CustomerCode = claims.FirstOrDefault(x => x.Type.EqualsIgnoreCase("customercode"))?.Value;
            var BranchCode = claims.FirstOrDefault(x => x.Type.EqualsIgnoreCase("branchcode"))?.Value;
            var SupplierCode = claims.FirstOrDefault(x => x.Type.EqualsIgnoreCase("suppliercode"))?.Value;
            var TimeZone = claims.FirstOrDefault(x => x.Type.EqualsIgnoreCase("timezone"))?.Value;
            var Email = claims.FirstOrDefault(x => x.Type.EqualsIgnoreCase("email"))?.Value;
            var PhoneNumber = claims.FirstOrDefault(x => x.Type.EqualsIgnoreCase("phonenumber"))?.Value;

            string userPermissions = claims.FirstOrDefault(x => x.Type.EqualsIgnoreCase("userpermissions"))?.Value;
            List<string> permissions = userPermissions.IsNullOrEmpty() ? new List<string>() : userPermissions.Deserialize<List<UserPermission>>().Select(p => p.PermissionCode).Distinct().ToList();

            string userRolesStr = claims.FirstOrDefault(x => x.Type.EqualsIgnoreCase("userroles"))?.Value;
            List<UserRole> userRoles = userRolesStr.IsNullOrEmpty() ? new List<UserRole>() : userRolesStr.Deserialize<List<UserRole>>();

            string roleStr = claims.FirstOrDefault(x => x.Type.EqualsIgnoreCase("role"))?.Value;
            Role role = roleStr.IsNullOrEmpty() ? null : roleStr.Deserialize<Role>();

            if (userRoles.IsNotNullOrEmpty())
                userRoles.ForEach(userRole =>
                {
                    permissions = permissions.Union(userRole.Role.RolePermissions).Distinct().ToList();
                });

            if (role.IsNotNull())
                permissions = permissions.Union(role.RolePermissions).Distinct().ToList();

            var Current = new IntranetUser(true,
                userId.GetValueOrDefault(),
                userName,
                permissions,
                ExternalCode,
                CustomerCode,
                BranchCode,
                SupplierCode,
                TimeZone,
                Email,
                PhoneNumber
                );

            return Current;
        }
        return null;
    }
}
public class UserRole
{
    public Role Role { get; set; }
}
public class Role
{
    public List<string> RolePermissions { get; set; }
}
public class UserPermission
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long PermissionId { get; set; }
    public string PermissionCode { get; set; }
}