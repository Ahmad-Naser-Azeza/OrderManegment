namespace SharedKernel;
using Kernel.Enum;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public class AuthorizePermissionAttribute : Attribute
{
    public UserAccessPermission Permission { get; }

    public AuthorizePermissionAttribute(UserAccessPermission permission)
    {
        Permission = permission;
    }
}