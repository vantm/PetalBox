using NSubstitute;
using NSubstitute.Core;

namespace ProductApi.Tests;

public static class TestUtilExtensions
{
    public static ConfiguredCall ReturnsAff<T>(this Aff<TestRuntime, T> aff, T value)
        => aff.Returns(Aff<TestRuntime, T>(_ => ValueTask.FromResult(value)));

    public static ConfiguredCall ReturnsEff<T>(this Eff<T> aff, T value)
        => aff.Returns(Eff(() => value));
}
