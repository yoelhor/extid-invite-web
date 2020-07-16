using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace extid_invite_web
{
    public class InviteWithoutSdkService : IInviteService
    {

        public async Task InviteUser(AppSettings settings, string dispalyName, string email)
        {
            // Acquire an access token
            var postContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", settings.MSGraphClientID),
                new KeyValuePair<string, string>("scope", "https://graph.microsoft.com/.default"),
                new KeyValuePair<string, string>("client_secret", settings.MSGraphClientSecret)
            });

            var myHttpClient = new HttpClient();
            var response = await myHttpClient.PostAsync($"https://login.microsoftonline.com/{settings.TenantId}/oauth2/v2.0/token", postContent);
            var responseString = await response.Content.ReadAsStringAsync();

            // Set the bearer token for the next requests
            JObject responseJson = JObject.Parse(responseString);
            myHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseJson["access_token"].ToString());


            // Create invite
            var postContentCreate = new
            {
                invitedUserDisplayName = dispalyName,
                invitedUserEmailAddress = email,
                inviteRedirectUrl = settings.InviteRedirectUrl,
                sendInvitationMessage = true
            };

            var stringPostContent = new StringContent(JsonConvert.SerializeObject(postContentCreate), Encoding.UTF8, "application/json");
            response = await myHttpClient.PostAsync("https://graph.microsoft.com/v1.0/invitations", stringPostContent);
            responseString = await response.Content.ReadAsStringAsync();
            responseJson = JObject.Parse(responseString);
            var newUserId = responseJson["invitedUser"]["id"].ToString();

            // Add the newly invited user to a security group. This method may return an error message, if the user already member (resend flow)
            try
            {
                stringPostContent = new StringContent("{\"@odata.id\" : \"https://graph.microsoft.com/beta/users/" + newUserId + "\"}", Encoding.UTF8, "application/json");
                response = await myHttpClient.PostAsync($"https://graph.microsoft.com/beta/groups/{settings.ExternalUsersGroupID}/members/$ref", stringPostContent);
                responseString = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}