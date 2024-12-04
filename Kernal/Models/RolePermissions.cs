using Kernel.Enum;

namespace SharedKernel;
public static class RolePermissions
{    
    public static readonly Dictionary<string, List<UserAccessPermission>> RolePermissionsMapping = new()
    {
        { "admin", new List<UserAccessPermission>
            { 
                UserAccessPermission.ReadOrders, 
                UserAccessPermission.CreateOrders,
                UserAccessPermission.UpdateOrders,
                UserAccessPermission.DeleteOrders
            } 
        },
        { "normal", new List<UserAccessPermission>   {
                UserAccessPermission.ReadOrders,
                UserAccessPermission.CreateOrders,
                UserAccessPermission.UpdateOrders
            } 
        }
    };
}