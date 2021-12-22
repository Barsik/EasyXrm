using System;
using System.Collections.Generic;
using EasyXrm.Query.QueryBuilding;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace EasyXrm.Extensions.OrganizationServiceExtensions
{
    public static class OrganizationServiceRetrieveMultipleExtensions
    {
        public static IEnumerable<Entity> RetrieveAll(this IOrganizationService organizationService,
            QueryExpression queryExpression)
        {
            if (organizationService == null) throw new ArgumentNullException(nameof(organizationService));
            if (queryExpression == null) throw new ArgumentNullException(nameof(queryExpression));


            var result = new List<Entity>();

            var pageNumber = 1;
            string pagingCookie = null;

            bool moreRecords;
            do
            {
                queryExpression.PageInfo = new PagingInfo()
                {
                    PageNumber = pageNumber,
                    PagingCookie = pagingCookie
                };
                var queryResult = organizationService.RetrieveMultiple(queryExpression);
                pagingCookie = queryResult.PagingCookie;
                pageNumber++;
                result.AddRange(queryResult.Entities);
                moreRecords = queryResult.MoreRecords;
            } while (moreRecords);

            return result;
        }

        public static IEnumerable<Entity> RetrieveAll(this IOrganizationService organizationService,
            string entityName, ColumnSet columnSet = null)
        {
            if (columnSet == null)
            {
                columnSet = new ColumnSet(false);
            }

            var query = QueryBuilder.Create(entityName).SetColumns(columnSet);
            return organizationService.RetrieveAll(query);
        }
    }
}