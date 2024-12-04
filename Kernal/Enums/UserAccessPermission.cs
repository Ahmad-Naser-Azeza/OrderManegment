using System.ComponentModel;

namespace Kernel.Enum
{
    public enum UserAccessPermission
    {
        ReadOrders, 
        CreateOrders, 
        UpdateOrders,
        DeleteOrders
    }
    public enum Roles
    {
        [Description("admin")]
        admin,
        [Description("user")]
        normal,        
    }
}
