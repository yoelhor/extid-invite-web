# Add guest users to your directory using MS Graph

You can invite anyone to collaborate with your organization by adding them to your directory as a guest user. This sample, craters and sends an invitation email that contains a redemption link. After the user is created in the directory, the user is added to a security groupe. 

The sample code has following three steps:

1. Initializes the auth provider using [OAuth 2.0 client credentials grant flow](https://docs.microsoft.com/azure/active-directory/develop/v2-oauth2-client-creds-grant-flow). With the client credentials grant flow, the app is able to get an access token to call the Microsoft Graph API.
1. [Crate invite user](https://docs.microsoft.com/graph/api/invitation-post?view=graph-rest-1.0&tabs=http)
1. Add the invited [user to a security group](https://docs.microsoft.com/graph/api/group-post-members?view=graph-rest-1.0&tabs=http)

## User flow

1. On the home page type an email address and display name, and hit the **Send invite** button. Azure AD will send an invitation email. 
1. The invited user has to click on the redemption link sent in the communication in the step above, and go through the interactive redemption process in a browser. Once completed, the invited user becomes an external user in the organization.
1. The user is redirected back to this sample app, to the `MicrosoftIdentity/Account/SignIn` endpoint (such as <http://localhost:5000/MicrosoftIdentity/Account/SignIn>)
1. The sign-in endpoint takes the user back to Azure AD to get an access/id token

## Contents

- [tree/master/Models/AppSettings.cs](Models/AppSettings.cs) - Application settings model presents the [appsettings.json](appsettings.json) 

## Setup


