namespace Bank.Bank.Application.DTOs.Response;

public class DefaultResponse<T>
{
    public bool IsSuccess { get; set;}
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public DefaultResponse(T? data, string message = "")
    {
        if(data != null) IsSuccess = true;
        Data = data;
        if(!string.IsNullOrEmpty(message)) Message = message ?? string.Empty;
    }
    public DefaultResponse(string message)
    {
        IsSuccess = false;
        Message = message;
    }
}