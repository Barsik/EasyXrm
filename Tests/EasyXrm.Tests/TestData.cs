using System.Collections.Generic;
using EasyXrm.Extensions.OrganizationServiceExtensions;
using EasyXrm.Tests.Entities.FoobarEntity;
using Microsoft.Xrm.Sdk;

namespace EasyXrm.Tests
{
    public static class TestData
    {
        public static List<Foobar> LoadData(IOrganizationService organizationService)
        {
            var pavel = new Foobar()
            {
                Name = "Pavel",
                Age = 20,
                GenderCode = GenderCode.Male
            };
            var masha = new Foobar()
            {
                Name = "Masha",
                Age = 23,
                GenderCode = GenderCode.Female
            };
            var foobars = new List<Foobar>()
            {
                new Foobar()
                {
                    Name = "Ivan",
                    Age = 40,
                    GenderCode = GenderCode.Male
                },
                pavel,
                new Foobar()
                {
                    Name = "Sasha",
                    Age = 60,
                    GenderCode = GenderCode.Male,
                    ParentFoobarId = pavel.ToEntityReference()
                },
                masha,
                new Foobar()
                {
                    Name = "Dasha",
                    Age = 30,
                    GenderCode = GenderCode.Female,
                    ParentFoobarId = masha.ToEntityReference()
                },
                new Foobar()
                {
                    Name = "Natasha",
                    Age = 50,
                    GenderCode = GenderCode.Female,
                    ParentFoobarId = masha.ToEntityReference()
                },
                new Foobar()
                {
                    Name = "Anya",
                    Age = 18,
                    GenderCode = GenderCode.Female
                }
            };

            foreach (var foobar in foobars)
            {
                organizationService.CreateThenSetId(foobar);
            }

            return foobars;
        }
    }
}