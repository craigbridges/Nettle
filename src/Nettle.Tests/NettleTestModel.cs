namespace Nettle.Tests
{
    using System;
    using System.Collections.Generic;

    public record class NettleTestModel
    {
        public required string Name { get; init; }
        public Gender Gender { get; init; }
        public DateTime? BirthDate { get; init; }
        public bool IsEmployed { get; init; }
        public List<Address> Addresses { get; init; } = new();
    }
}
