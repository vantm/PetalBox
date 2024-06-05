using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using ProductApi.Products.Domain;
using ProductApi.Products.Endpoints;
using ProductApi.Tests.Products.TestData;

namespace ProductApi.Tests.Products.Endpoints;

using TestData = IEnumerable<object[]>;

public class ListProductEndpointTests
{
    private static Product NewProduct() => Product.New(
        Fake.Commerce.ProductName(), Fake.Random.Decimal(0.1m, 1_000), TimeProvider.System);

    [Theory]
    [InlineData(1, null)]
    [InlineData(null, 20)]
    [InlineData(null, null)]
    public async Task ShouldReturnsProducts_WhenPageOrSizeNull(int? page, int? size)
    {
        // Arrange
        var repo = Substitute.For<IProductRepo>();

        var products = Repeat(10, NewProduct);

        repo.SelectAsync(Arg.Any<SelectParams>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(EitherWithError(products)));

        var validator = new ListProductEndpoint.EndpointValidator();

        var ep = new ListProductEndpoint(page, size, repo, validator, CancellationToken.None);

        // Act

        var result = await ep.Handle();

        // Assert

        Assert.NotNull(result);
        Assert.IsType<Ok<IEnumerable<Product>>>(result);

        var okResult = (Ok<IEnumerable<Product>>)result!;

        Assert.Equal(products, okResult.Value);
    }

    public static TestData ShouldReturnValidationErrors_WhenPageOrSizeInvalid_TestData() =>
        MakeTestData(5, () => Repeat(1, () => ListProductEndpointParams.Create(ListProductEndpointParams.InvalidTestData)));

    [Theory]
    [MemberData(nameof(ShouldReturnValidationErrors_WhenPageOrSizeInvalid_TestData))]
    public async Task ShouldReturnValidationErrors_WhenPageOrSizeInvalid(ListProductEndpointParams @params)
    {
        // Arrange
        var validator = new ListProductEndpoint.EndpointValidator();

        var repo = Substitute.For<IProductRepo>();

        var (page, size) = @params;

        var ep = new ListProductEndpoint(page, size, repo, validator, CancellationToken.None);

        // Act

        var result = await ep.Handle();

        // Assert

        Assert.NotNull(result);
        Assert.IsType<ProblemHttpResult>(result);

        var problem = (ProblemHttpResult)result!;

        Assert.IsType<HttpValidationProblemDetails>(problem.ProblemDetails);

        var validationProblem = (HttpValidationProblemDetails)problem.ProblemDetails;
        var keys = validationProblem.Errors;

        if (page < 0)
        {
            Assert.Contains("p", keys);
        }
        if (size < 0)
        {
            Assert.Contains("s", keys);
        }
    }

    [Fact]
    public async Task ShouldReturnBadRequest_WhenQueryHasException()
    {
        var validator = new ListProductEndpoint.EndpointValidator();

        var repo = Substitute.For<IProductRepo>();

        var exception = new HttpRequestException("This is a fake exception");
        var error = Error.New(exception);

        repo.SelectAsync(Arg.Any<SelectParams>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Either<Error, IEnumerable<Product>>.Left(error)));

        var ep = new ListProductEndpoint(null, null, repo, validator, CancellationToken.None);

        // Act

        var result = await ep.Handle();

        // Assert

        Assert.NotNull(result);
        Assert.IsType<BadRequest<string>>(result);

        var badRequest = (BadRequest<string>)result!;

        Assert.NotNull(badRequest);
    }
}
