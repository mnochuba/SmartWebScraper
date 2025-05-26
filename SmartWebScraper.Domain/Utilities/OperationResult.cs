using System.Net;

namespace SmartWebScraper.Domain.Utilities;
public class OperationResult
{
    internal OperationResult() { }

    public OperationResult AddError(string errorMessage)
    {
        Errors.Add(errorMessage);
        return this;
    }

    public OperationResult AddErrors(IEnumerable<string> errorMessages)
    {
        if (errorMessages == null) return this;
        Errors.AddRange(errorMessages);
        return this;
    }

    public static OperationResult Failure(HttpStatusCode errorCode = HttpStatusCode.InternalServerError)
    {
        return new OperationResult() { IsSuccessful = false, Code = errorCode };
    }

    public static OperationResult Success()
    {
        return new OperationResult() { IsSuccessful = true, Code = HttpStatusCode.OK };
    }

    public List<string> Errors { get; } = [];

    public bool IsSuccessful { get; set; }
    public HttpStatusCode Code { get; set; }
}

public class OperationResult<T> : OperationResult
{
    public T? Data { get; set; }
    public static new OperationResult<T> Failure(HttpStatusCode errorCode = HttpStatusCode.InternalServerError)
    {
        return new OperationResult<T>() { IsSuccessful = false, Code = errorCode };
    }
    public static OperationResult<T> Success(T data)
    {
        return new OperationResult<T>() { Data = data, IsSuccessful = true, Code = HttpStatusCode.OK };
    }
    public new OperationResult<T> AddError(string errorMessage)
    {
        base.AddError(errorMessage);
        return this;
    }

    public new OperationResult<T> AddErrors(IEnumerable<string> errorMessages)
    {
        base.AddErrors(errorMessages);
        return this;
    }
}
