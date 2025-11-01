namespace signa.api.Models;

public class ServiceResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    // Optional pagination info
    public PaginationMetadata? Pagination { get; set; }

    // --- Factory Helpers ---
    public static ServiceResponse<T> Ok(
        T data,
        string? message = null,
        PaginationMetadata? pagination = null)
        => new()
        {
            Success = true,
            Message = message ?? "Operation successful.",
            Data = data,
            Pagination = pagination
        };

    public static ServiceResponse<T> Fail(string message)
        => new()
        {
            Success = false,
            Message = message,
            Data = default,
            Pagination = null
        };
}