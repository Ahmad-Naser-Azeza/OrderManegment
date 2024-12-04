namespace SharedKernel;

public static class ResponseMessages
{
    public const string Conflict = "A conflict occurred due to the current state of a resource";
    public const string CreatedResource = "The resource was successfully created";
    public const string CriticalError = "An unexpected error has occurred on an external server";
    public const string DeletedResource = "The resource has been successfully deleted";
    public const string DivideByZero = "Cannot be divided by zero. The '{0}' parameter cannot be zero";
    public const string Error = "Error";
    public const string FailedConversion = "Conversion failed. Failed to convert the result object to an object of type '{0}'";
    public const string Failure = "An error occurred during the execution of a service";
    public const string FileContent = "File content has been returned successfully";
    public const string Forbidden = "You do not have permissions to perform this action";
    public const string Invalid = "A data validation error occurred";
    public const string NotFound = "Resource not found";
    public const string ObtainedResource = "Resource successfully obtained";
    public const string ObtainedResources = "Resources successfully obtained";
    public const string PropertyFailedValidation = "'{0}' property failed validation. Error was: {1}";
    public const string Success = "Operation successfully executed";
    public const string Unauthorized = "Not authorized";
    public const string UnsupportedStatus = "Result '{0}' conversion is not supported";
    public const string UpdatedResource = "The resource was successfully updated";
    public const string ValidationErrors = "There have been validation errors";
}
