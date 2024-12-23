﻿

namespace SharedKernel;


public partial class Result
{
    /// <summary>
    /// Converts an instance of type <see cref="Result"/> to <see cref="Result{T}"/>.
    /// </summary>
    /// <typeparam name="T">A value associated to the result.</typeparam>
    /// <param name="data">The value to be set.</param>
    /// <returns>
    /// An instance of type <see cref="Result{T}"/>.
    /// </returns>
    public Result<T> ToResult<T>(T data) => new()
    {
        Data = data,
        IsSuccess = IsSuccess,
        Message = Message,
        Errors = Errors,
        Status = Status
    };

    /// <summary>
    /// Converts an instance of type <see cref="Result"/> to <see cref="ListedResult{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to enumerate.</typeparam>
    /// <param name="data">The value to be set.</param>
    /// <returns>
    /// An instance of type <see cref="ListedResult{T}"/>.
    /// </returns>
    public ListedResult<T> ToListedResult<T>(IEnumerable<T> data) => new()
    {
        Data = data,
        IsSuccess = IsSuccess,
        Message = Message,
        Errors = Errors,
        Status = Status
    };

}