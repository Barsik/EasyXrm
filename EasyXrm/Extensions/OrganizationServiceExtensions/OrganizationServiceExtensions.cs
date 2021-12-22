using System;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace EasyXrm.Extensions.OrganizationServiceExtensions
{
    public static class OrganizationServiceExtensions
    {
        public static EntityReference WhoAmI(this IOrganizationService organizationService)
        {
            if (organizationService == null)
                throw new ArgumentNullException(nameof(organizationService));

            var whoAmIRequest = new WhoAmIRequest();

            var response = (WhoAmIResponse)organizationService.Execute(whoAmIRequest);

            return new EntityReference("systemuser", response.UserId);
        }

        public static void AddUsersToTeam(this IOrganizationService organizationService, Guid[] userIds,
            Guid teamId)
        {
            if (organizationService == null)
                throw new ArgumentNullException(nameof(organizationService));
            organizationService.Execute(new AddMembersTeamRequest()
            {
                TeamId = teamId,
                MemberIds = userIds
            });
        }

        public static void AddUserToTeam(this IOrganizationService organizationService, Guid userId,
            Guid teamId)
        {
            organizationService.AddUsersToTeam(new[] { userId }, teamId);
        }

        public static void RemoveUsersFromTeam(this IOrganizationService organizationService, Guid[] userIds,
            Guid teamId)
        {
            if (organizationService == null)
                throw new ArgumentNullException(nameof(organizationService));
            organizationService.Execute(new RemoveMembersTeamRequest()
            {
                TeamId = teamId,
                MemberIds = userIds
            });
        }

        public static void RemoveUserFromTeam(this IOrganizationService organizationService, Guid userId,
            Guid teamId)
        {
            organizationService.RemoveUsersFromTeam(new[] { userId }, teamId);
        }
        public static int? GetUiLanguageCode(this IOrganizationService organizationService, Guid userId)
        {
            var userSettingsQuery = new QueryExpression("usersettings")
            {
                ColumnSet = new ColumnSet("uilanguageid", "systemuserid"),
                Criteria = new FilterExpression(LogicalOperator.And)
                {
                    Conditions =
                    {
                        new ConditionExpression("systemuserid", ConditionOperator.Equal, userId)
                    }
                }
            };

            var userSettings = organizationService.RetrieveMultiple(userSettingsQuery);

            return userSettings.Entities.Count > 0
                ? userSettings.Entities[0].GetAttributeValue<int>("uilanguageid")
                : (int?)null;
        }
    }
}