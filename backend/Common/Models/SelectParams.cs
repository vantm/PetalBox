namespace Common.Models;

public record SelectParams(int Offset, int Limit)
{
    public static SelectParams FromPaging(PageParams pageParams)
    {
        var page = pageParams.Page ?? 1;
        var limit = pageParams.Limit ?? 20;
        var offset = (page - 1) * limit;
        return new(offset, limit);
    }
}
