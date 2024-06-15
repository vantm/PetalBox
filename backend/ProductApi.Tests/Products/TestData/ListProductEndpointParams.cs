using Bogus;

namespace ProductApi.Tests.Products.TestData;

public record ListProductEndpointParams
{
    public int Page { get; init; }
    public int Size { get; init; }

    public void Deconstruct(out int page, out int size)
    {
        page = Page;
        size = Size;
    }

    private static readonly Faker<ListProductEndpointParams> _faker = new Faker<ListProductEndpointParams>()
        .RuleSet(InvalidTestData, rules =>
        {
            rules.RuleFor(o => o.Page, x => x.Random.Bool() ? x.Random.Int(max: 0) : x.Random.Int(min: 2_000_001))
                 .RuleFor(o => o.Size, x => x.Random.Bool() ? x.Random.Int(max: -1) : x.Random.Int(min: 1001));
        })
        .RuleSet(ValidTestData, rules =>
        {
            rules.RuleFor(o => o.Page, x => x.Random.Int(1, 2_000_000))
                 .RuleFor(o => o.Size, x => x.Random.Int(0, 1_000));
        });

    public const string InvalidTestData = nameof(InvalidTestData);
    public const string ValidTestData = nameof(ValidTestData);

    public static ListProductEndpointParams Create(string testData) => _faker.Generate(testData);
}
