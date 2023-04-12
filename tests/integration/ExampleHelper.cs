using Bogus;
using Intive.Patronage2023.Modules.Example.Domain;

namespace Intive.Patronage2023.Modules.Example.Application.Tests.Example.GettingExamples
{
    public static class ExampleHelper
    {
        public static ExampleAggregate CreateExample()
        {
            var faker = new Faker();
            var name = faker.Name.FirstName();
            var example = new ExampleAggregate(name);
            return example;
        }
    }
}