using LanguageExt.Effects.Traits;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using ProductApi.Products.Domain;
using ProductApi.Products.Endpoints;
using ProductApi.Tests.Products.TestData;

namespace ProductApi.Tests.Products.Endpoints;
public class ListProductEndpointTests
{
    private readonly HasCancel<TestRuntime> _hasCancel;
    private readonly HasServiceProvider _hasServiceProvider;
    private readonly TestRuntime _runtime;
    private readonly PageParams _pageParamsFake = new(default, default);

    public ListProductEndpointTests()
    {
        _hasCancel = Substitute.For<HasCancel<TestRuntime>>();
        _hasServiceProvider = Substitute.For<HasServiceProvider>();
        _runtime = TestRuntime.New(_hasCancel, _hasServiceProvider);
    }

    [Theory]
    [InlineData(LogLevel.Debug, true)]
    [InlineData(LogLevel.Information, false)]
    [InlineData(LogLevel.Warning, false)]
    [InlineData(LogLevel.Error, false)]
    [InlineData(LogLevel.Critical, false)]
    public async Task LogsAreWrittenCorrectly(LogLevel enabledLevel, bool shouldCall)
    {
        // Arrange
        var logger = Substitute.For<ILogger<ListProductEndpoint<TestRuntime>>>();

        logger.IsEnabled(Arg.Is(enabledLevel)).Returns(true);

        _hasServiceProvider
            .RequiredService<ILogger<ListProductEndpoint<TestRuntime>>>()
            .ReturnsEff(logger);

        var ep = ListProductEndpoint<TestRuntime>.New(_pageParamsFake);

        // Act
        _ = await ep.Run(_runtime);

        // Assert
        logger
            .Received(Quantity.Exactly(1))
            .IsEnabled(Arg.Is(LogLevel.Debug));

        logger
            .Received(shouldCall ? Quantity.Exactly(1) : Quantity.None())
            .Log(
                logLevel: Arg.Is(LogLevel.Debug),
                eventId: Arg.Any<EventId>(),
                state: Arg.Any<Arg.AnyType>(),
                exception: Arg.Is(default(Exception?)),
                formatter: Arg.Any<Func<Arg.AnyType, Exception?, string>>()
            );
    }

    [Fact]
    public async Task TheValidatorIsCalled()
    {
        // Arrange
        var validator = Substitute.For<IValidator<PageParams>>();

        _hasServiceProvider
            .RequiredService<ILogger<ListProductEndpoint<TestRuntime>>>()
            .ReturnsEff(NullLogger<ListProductEndpoint<TestRuntime>>.Instance);

        _hasServiceProvider
            .RequiredService<IValidator<PageParams>>()
            .ReturnsEff(validator);

        var ep = ListProductEndpoint<TestRuntime>.New(_pageParamsFake);

        // Act
        _ = await ep.Run(_runtime);

        // Assert
        validator.Received(Quantity.Exactly(1)).Validate(Arg.Is(_pageParamsFake));
    }

    [Fact]
    public async Task TheValidationErrorAreThrown()
    {
        // Arrange
        _hasServiceProvider
            .RequiredService<ILogger<ListProductEndpoint<TestRuntime>>>()
            .ReturnsEff(NullLogger<ListProductEndpoint<TestRuntime>>.Instance);

        var validator = Substitute.For<IValidator<PageParams>>();

        validator.Validate(Arg.Is(_pageParamsFake))
            .Returns(new FluentValidation.Results.ValidationResult()
            {
                Errors = [new() { PropertyName = "Page", ErrorMessage = "Invalid" }]
            });

        _hasServiceProvider
            .RequiredService<IValidator<PageParams>>()
            .ReturnsEff(validator);

        var ep = ListProductEndpoint<TestRuntime>.New(_pageParamsFake);

        Error? error = null;

        // Act
        var app = await ep.Catch(err =>
        {
            error = err;
            return FailEff<IResult>(err);
        }).Run(_runtime);

        // Assert
        Assert.True(app.IsFail);
        Assert.NotNull(error);
        Assert.IsType<ValidationError>(error);

        _hasServiceProvider
            .Received(Quantity.None())
            .RequiredService<IProductRepo<TestRuntime>>();
    }

    [Fact]
    public async Task WhenRepoReturnsData_ThenProducesOk()
    {
        // Arrange
        _hasServiceProvider
            .RequiredService<ILogger<ListProductEndpoint<TestRuntime>>>()
            .ReturnsEff(NullLogger<ListProductEndpoint<TestRuntime>>.Instance);

        var validator = Substitute.For<IValidator<PageParams>>();
        validator.Validate(Arg.Any<PageParams>())
            .Returns(new FluentValidation.Results.ValidationResult());

        _hasServiceProvider
            .RequiredService<IValidator<PageParams>>()
            .ReturnsEff(validator);

        var repo = Substitute.For<IProductRepo<TestRuntime>>();

        var productTitle = Guid.NewGuid().ToString();
        var products = Seq([Product.New(productTitle, 3200, TimeProvider.System)]);

        repo.all(Arg.Any<SelectParams>())
            .ReturnsAff(products);

        _hasServiceProvider
            .RequiredService<IProductRepo<TestRuntime>>()
            .ReturnsEff(repo);

        var ep = ListProductEndpoint<TestRuntime>.New(_pageParamsFake);

        // Act
        var app = await ep.Run(_runtime);

        // Assert
        Assert.True(app.IsSucc);

        app.IfSucc((result) =>
        {
            Assert.IsType<Ok<Seq<Product>>>(result);

            var seq = ((Ok<Seq<Product>>)result).Value;

            Assert.Equal(1, seq.Count);
            Assert.Equal(productTitle, seq[0].Title);
        });
    }
}
