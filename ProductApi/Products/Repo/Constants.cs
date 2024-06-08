namespace ProductApi.Products.Repo;

public static class Constants
{
    public static readonly Duration[] Retries = [new(1000), new(3000), new(9000)];
}
