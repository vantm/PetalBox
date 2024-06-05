namespace Common;

public record SelectParams(int Offset, int Limit)
{
    public static SelectParams FromPaging(int? page, int? size)
    {
        page ??= 1;
        size ??= 20;
        var offset = (page - 1) * size;
        return new(offset.Value, size.Value);
    }
}
