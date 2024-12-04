
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace SharedKernel;


/// <summary>
/// Represents the result of an operation that does not return a value.
/// </summary>
/// <remarks>
/// <para>This class defines different types of results for an operation.</para>
/// For example: <c>Result.Invalid</c>, <c>Result.NotFound</c>, among others.
/// </remarks>
public  partial class Result : ResultBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    public Result() { }

    /// <inheritdoc cref="Success{T}(T, string)" />
    public static Result<T> Success<T>(T data)
        => Success(data, ResponseMessages.Success);

    /// <summary>
    /// Represents a successful operation and accepts a values as the result of the operation.
    /// </summary>
    /// <param name="data">The value to be set.</param>
    /// <param name="message">A message of success.</param>
    public static Result<T> Success<T>(T data, string message) => new()
    {
        Data = data,
        IsSuccess = true,
        Message = message,
        Status = ResultStatus.Ok
    };
 

    /// <summary>
    /// Represents a successful operation.
    /// </summary>
    public static Result Success()
        => Success(ResponseMessages.Success);

    /// <summary>
    /// Represents a successful operation and accepts a messages that describes the result.
    /// </summary>
    /// <param name="message">A message of success.</param>
    public static Result Success(string message) => new()
    {
        IsSuccess = true,
        Message = message,
        Status = ResultStatus.Ok
    };

    /// <inheritdoc cref="CreatedResource(string)" />
    public static Result CreatedResource()
        => CreatedResource(ResponseMessages.CreatedResource);

    /// <summary>
    /// Represents a situation in which the service successfully creates a resource.
    /// </summary>
    /// <param name="message">A message of success.</param>
    public static Result CreatedResource(string message) => new()
    {
        IsSuccess = true,
        Message = message,
        Status = ResultStatus.Created
    };

    ///// <inheritdoc cref="CreatedResource(int, string)" />
    //public static Result<CreatedId> CreatedResource(int id)
    //    => CreatedResource(id, ResponseMessages.CreatedResource);

    ///// <summary>
    ///// Represents a situation in which the service successfully creates a resource.
    ///// </summary>
    ///// <param name="id">The ID of the created resource.</param>
    ///// <param name="message">A message of success.</param>
    //public static Result<CreatedId> CreatedResource(int id, string message) => new()
    //{
    //    Data = new CreatedId { Id = id },
    //    IsSuccess = true,
    //    Message = message,
    //    Status = ResultStatus.Created
    //};

    ///// <inheritdoc cref="CreatedResource(Guid, string)" />
    //public static Result<CreatedGuid> CreatedResource(Guid guid)
    //    => CreatedResource(guid, ResponseMessages.CreatedResource);

    ///// <summary>
    ///// Represents a situation in which the service successfully creates a resource.
    ///// </summary>
    ///// <param name="guid">The GUID assigned to the resource.</param>
    ///// <param name="message">A message of success.</param>
    //public static Result<CreatedGuid> CreatedResource(Guid guid, string message) => new()
    //{
    //    Data = new CreatedGuid { Id = guid.ToString() },
    //    IsSuccess = true,
    //    Message = message,
    //    Status = ResultStatus.Created
    //};

    /// <summary>
    /// Represents a situation in which the service successfully updates a resource.
    /// </summary>
    public static Result UpdatedResource()
        => Success(ResponseMessages.UpdatedResource);

    /// <summary>
    /// Represents a situation in which the service successfully deletes a resource.
    /// </summary>
    public static Result DeletedResource()
        => Success(ResponseMessages.DeletedResource);

    /// <summary>
    /// Represents a situation in which the service successfully obtains a resource.
    /// </summary>
    /// <param name="data">The value to be set.</param>
    public static Result<T> ObtainedResource<T>(T data)
        => Success(data, ResponseMessages.ObtainedResource);
  

    public static implicit operator ActionResult(Result result)
    {
        var objectResult = new ObjectResult(result)
        {
            StatusCode = result.Status switch
            {
                ResultStatus.Ok => (int)HttpStatusCode.OK,
                ResultStatus.Created => (int)HttpStatusCode.Created,
                ResultStatus.Unauthorized => (int)HttpStatusCode.Unauthorized,
                ResultStatus.Forbidden => (int)HttpStatusCode.Forbidden,
                ResultStatus.NotFound => (int)HttpStatusCode.NotFound,
                ResultStatus.Conflict => (int)HttpStatusCode.Conflict,
                ResultStatus.CriticalError => (int)HttpStatusCode.InternalServerError,
                _ => (int)HttpStatusCode.BadRequest
            }
        };
        return objectResult;
    }
}

