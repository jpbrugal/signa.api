namespace signa.api.Models;


public class PaginationMetadata
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

    public PaginationMetadata(int page, int pageSize, int totalItems)
    {
        Page = page;
        PageSize = pageSize;
        TotalItems = totalItems;
    }
}