namespace Order.Domain.Common;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IEnumerable<string> Errors { get; }

    protected Result(bool isSuccess, IEnumerable<string> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public static Result Success() => new(true, new List<string>());
    
    public static Result Failure(string error) => new(false, new List<string> { error });
}

public class Result<T> : Result
{
    private readonly T _value;
    
    public T Value 
    {
        get 
        {
            if (IsFailure)
                throw new InvalidOperationException("Cannot access Value on a failure result.");
                
            return _value;
        }
    }

    protected internal Result(T value, bool isSuccess, IEnumerable<string> errors)
        : base(isSuccess, errors)
    {
        _value = value;
    }
    
    public static Result<T> Success(T value) => new(value, true, new List<string>());
    
    public static new Result<T> Failure(IEnumerable<string> errors) => new(default, false, errors);
    
    public static new Result<T> Failure(string error) => new(default, false, new List<string> { error });
}