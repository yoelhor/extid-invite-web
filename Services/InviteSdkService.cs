using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;

namespace extid_invite_web
{
    public class InviteSdkService : IInviteService
    {

        public async Task InviteUser(AppSettings settings, string dispalyName, string email)
        {
            // Initialize the client credential auth provider
            IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(settings.MSGraphClientID)
                .WithTenantId(settings.TenantId)
                .WithClientSecret(settings.MSGraphClientSecret)
                .Build();
            ClientCredentialProvider authProvider = new ClientCredentialProvider(confidentialClientApplication);

            // Set up the Microsoft Graph service client with client credentials
            GraphServiceClient graphClient = new GraphServiceClient(authProvider);

            var invitation = new Invitation
            {
                SendInvitationMessage = true,
                InvitedUserDisplayName = dispalyName,
                InvitedUserEmailAddress = email,
                InviteRedirectUrl = settings.InviteRedirectUrl
            };

            var user = await graphClient.Invitations
                .Request()
                .AddAsync(invitation);

            string userUri = $"[\"https://graph.microsoft.com/v1.0/users/{user.InvitedUser.Id}\"]";

            try
            {
                await graphClient.Groups[settings.ExternalUsersGroupID].Members.References.Request().AddAsync(user.InvitedUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}