using Carter;
using CustomerApi.Addresses.Domain;
using CustomerApi.Addresses.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApi.Addresses;

public class AddressModule : CarterModule
{
    public AddressModule() : base("/addresses")
    {
        WithTags("Address");
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("", async (
            [FromQuery] Guid userId,
            [FromQuery] Guid customerId,
            IAddressRepo repo,
            CancellationToken aborted) =>
        {
            if (customerId == Guid.Empty)
            {
                return Results.NotFound();
            }

            var addresses = await repo.Select(
                CustomerId.FromValue(customerId),
                UserId.FromValue(userId),
                aborted);

            return Results.Ok(addresses);
        });

        app.MapGet("{id:guid}", async (
            Guid id,
            [FromQuery] Guid userId,
            [FromQuery] Guid customerId,
            IAddressRepo repo,
            AddressMapper mapper,
            CancellationToken aborted) =>
        {
            if (id == Guid.Empty || customerId == Guid.Empty || userId == Guid.Empty)
            {
                return Results.NotFound();
            }

            var address = await repo.Find(
                AddressId.FromValue(id),
                CustomerId.FromValue(customerId),
                UserId.FromValue(userId),
                aborted);

            if (address == null)
            {
                return Results.NotFound();
            }

            var dto = mapper.MapToAddressDto(address);
            return Results.Ok(dto);
        })
        .WithName("GetAddress");

        app.MapPost("", async (
            [FromQuery] Guid userId,
            [FromQuery] Guid customerId,
            CreateAddressRequest body,
            IValidator<CreateAddressRequest> validator,
            IAddressRepo repo,
            TimeProvider time,
            AddressMapper mapper,
            CancellationToken aborted) =>
        {
            if (customerId == Guid.Empty || userId == Guid.Empty)
            {
                return Results.NotFound();
            }

            var validationResult = validator.Validate(body);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var address = Address.New(
                CustomerId.FromValue(customerId),
                new(body.AddressName, body.AddressText, body.Phone),
                UserId.FromValue(userId),
                time);

            await repo.Insert(address, aborted);

            var dto = mapper.MapToAddressDto(address);
            return Results.CreatedAtRoute("GetAddress", new
            {
                dto.Id,
                dto.CustomerId,
                dto.UserId
            }, dto);
        });

        app.MapPut("{id:guid}", async (
            Guid id,
            [FromQuery] Guid userId,
            [FromQuery] Guid customerId,
            UpdateAddressRequest body,
            IValidator<UpdateAddressRequest> validator,
            IAddressRepo repo,
            TimeProvider time,
            AddressMapper mapper,
            CancellationToken aborted) =>
        {
            if (id == Guid.Empty || customerId == Guid.Empty || userId == Guid.Empty)
            {
                return Results.NotFound();
            }

            var validationResult = validator.Validate(body);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var address = await repo.Find(
                AddressId.FromValue(id),
                CustomerId.FromValue(customerId),
                UserId.FromValue(userId),
                aborted);

            if (address == null)
            {
                return Results.NotFound();
            }

            var fields = new AddressFields(body.AddressName, body.AddressText, body.Phone);
            var newAddr = address.UpdateFields(fields, time);

            await repo.Update(newAddr, aborted);

            return Results.NoContent();
        });
    }
}
