using CustomerApi.Customers.Domain;
using CustomerApi.Customers.Models;
using Riok.Mapperly.Abstractions;

namespace CustomerApi.Customers;

[Mapper]
public partial class CustomerMapper
{
    [MapNestedProperties(nameof(Customer.FullName))]
    [MapNestedProperties(nameof(Customer.Contact))]
    public virtual partial CustomerDto MapToCustomerDto(Customer customer);
}
