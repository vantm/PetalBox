using CustomerApi.Addresses.Domain;
using CustomerApi.Addresses.Models;
using Dapr.Client;
using Microsoft.Extensions.Options;

namespace CustomerApi.Addresses.Implementations;

public class DaprAddressRepo(DaprClient dapr, IOptions<CommonOptions> options, AddressMapper mapper) : IAddressRepo
{
    public async Task<IEnumerable<AddressDto>> Select(
        CustomerId customerId, UserId userId, CancellationToken cancellationToken)
    {
        const string SqlText = """
                SELECT id, customer_id, address_name, address_text, phone, is_default
                FROM address;
                WHERE customer_id = $1 AND user_id = $2
            """;

        var request = new BindingRequest(options.Value.BindingName, "query")
            .AddSqlText(SqlText)
            .AddSqlParams(customerId.Value, userId.Value);

        var response = await dapr.InvokeBindingAsync(request, cancellationToken);

        var fin = response.Match(docs => docs.Select(mapper.MapToAddressDto));

        return fin.Case switch
        {
            IEnumerable<AddressDto> addresses => addresses,
            _ => throw new Exception("Error while querying")
        };
    }

    public async Task<Address> Find(
        AddressId id, CustomerId customerId, UserId userId, CancellationToken cancellationToken)
    {
        const string SqlText = """
                SELECT id, customer_id, address_name, address_text, phone, is_default
                FROM address;
                WHERE id = $1 AND customer_id = $2 AND user_id = $3
            """;

        var request = new BindingRequest(options.Value.BindingName, "query")
            .AddSqlText(SqlText)
            .AddSqlParams(id.Value, customerId.Value, userId.Value);

        var response = await dapr.InvokeBindingAsync(request, cancellationToken);

        return response.Match(docs => docs.Length == 0 ? null : mapper.MapToAddress(docs[0])).Case switch
        {
            Address addr => addr,
            _ => throw new Exception("Error while querying")
        };
    }

    public Task Insert(Address address, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Update(Address address, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
