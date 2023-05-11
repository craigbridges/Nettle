namespace Nettle.Tests
{
    public record class Address
    {
        public required string AddressLine1 { get; init; }
        public string? City { get; init; }
        public required string Postcode { get; init; }
    }
}
