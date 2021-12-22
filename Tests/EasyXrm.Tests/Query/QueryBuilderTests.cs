using System.Linq;
using EasyXrm.Extensions;
using EasyXrm.Query.QueryBuilding;
using EasyXrm.Tests.Entities.FoobarEntity;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk.Query;
using Xunit;

namespace EasyXrm.Tests.Query
{
    public class QueryBuilderTests
    {
        [Fact]
        public void QueryBuilder_WhenFilter()
        {
            var context = new XrmFakedContext();

            var organizationService = context.GetOrganizationService();

            TestData.LoadData(organizationService);

            var query = QueryBuilder<Foobar>.Create()
                .SetColumns(f => f.Name)
                .AddFilter(LogicalOperator.And, builder =>
                    builder
                        .AddCondition(f => f.Age, ConditionOperator.GreaterEqual, 30)
                        .AddCondition(f => f.GenderCode, ConditionOperator.Equal, GenderCode.Male.Id));


            var foobars = organizationService.RetrieveMultiple(query).Map<Foobar>();

            Assert.NotNull(foobars.FirstOrDefault(f => f.Name == "Ivan"));
            Assert.NotNull(foobars.FirstOrDefault(f => f.Name == "Sasha"));
        }
    }
}