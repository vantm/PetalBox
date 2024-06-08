namespace Common;

public class PageParamsValidator : AbstractValidator<PageParams>
{
    public PageParamsValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .LessThanOrEqualTo(2_000_000);

        RuleFor(x => x.Limit)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(1000);
    }
}

