using Common.Domain;
using Common.Domain.ValueObjects;

namespace CustomerApi.Addresses.Domain;

public interface IAddressRepo<RT> : IRepo<Address, AddressId, RT> where RT : IAppRuntime
{
}
