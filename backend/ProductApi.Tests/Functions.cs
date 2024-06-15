using Bogus;
using System.Collections;

namespace ProductApi.Tests;

public static class Functions
{
    public static IEnumerable<T> Repeat<T>(int n, Func<T> create) => Enumerable.Repeat(0, n).Select(_ => create()).ToArray();

    public static IEnumerable<object[]> MakeTestData<T>(int n, Func<T> create) where T : IEnumerable
    {
        return Repeat(n, () => create().Cast<object>().ToArray());
    }

    public static readonly Faker Fake = new();
}
